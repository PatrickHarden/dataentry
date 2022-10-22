[CmdletBinding()]
param(
    [string] $schemaPath = '.\ingestion_schema.json',
    [string] $outputPath = '.\Generated\',
    [string] $namespace = 'dataentry.Services.Integration.Edp.Model',
    [string] $access = 'public',
	[string] $graphQLObjectName = 'EdpGraphQLObject',
	[string] $graphQLMutationName = 'EdpGraphQLMutation'
)

$scalar = @{
    'Boolean' = 'bool?'
    'Float'   = 'decimal?'
    'ID'      = 'string'
    'Int'     = 'int?'
    'Map'     = 'string'
    'String'  = 'string'
    'Time'    = 'DateTime?'
}

$nonNullScalar = @{
    'Boolean' = 'bool'
    'Float'   = 'decimal'
    'ID'      = 'string'
    'Int'     = 'int'
    'Map'     = 'string'
    'String'  = 'string'
    'Time'    = 'DateTime'
}

function Format-CSharpVerbatimString {
    param(
        [Parameter(Position = 1)]
        [string] $value
    )
    return $value -replace '"', '""'
}

function Format-CSharpIdentifier {
    param(
        [Parameter(Position = 1)]
        [string] $value
    )
    [string] $clean = [Regex]::Replace($value, "[^A-Za-z0-9_]", "")
    if (-not $clean) {
        Write-Error "$value cannot be converted to a valid C# Identifier"
    }
    else {
        Write-Output $clean
    }
}

function Get-PropertyType {
    param(
        [Parameter(Position = 1)]
        [PSObject] $value,
        [switch] $nonNull
    )
    if ($value.kind -ieq 'LIST') {
        Write-Output "List<$(Get-PropertyType $value.ofType)>"
    }
    elseif ($value.kind -ieq 'NON_NULL') {
        Write-Output (Get-PropertyType $value.ofType -nonNull)
    }
    elseif ($value.kind -ieq 'INPUT_OBJECT' -or $value.kind -ieq 'OBJECT' -or $value.kind -ieq 'ENUM') {
        Write-Output (Format-CSharpIdentifier $value.name)
    }
    elseif ($value.kind -ieq 'SCALAR') {
        if ($nonNull) {
            Write-Output $nonNullScalar."$($value.name)"
        }
        else {
            Write-Output $scalar."$($value.name)"
        }
    }
}

if (-not (Test-Path $outputPath)) {
    New-Item -Path $outputPath -ItemType Directory
}

$schema = Get-Content $schemaPath | ConvertFrom-Json

$objectLookup = @{ }
$schema.data.__schema.types | `
    Where-Object { 'OBJECT', 'INPUT_OBJECT' -ieq $_.kind } | `
    ForEach-Object {
    $objectLookup."$($_.name)" = $_
}

$resultFieldsLookup = @{ }
function Get-ResultFields {
    param(
        [Parameter(Position = 1)]
        [PSObject] $definition,
        [Parameter(Position = 2)]
        [PSObject] $fields
    )
    
    $allResults = $resultFieldsLookup."$($definition.name)"
    if (-not $allResults) {
        $allResults = @()
        $fields | ForEach-Object {
            $result = Format-CSharpVerbatimString $_.name
            $inner = Get-ResultFieldsForType $_.type
            if ($inner) {
                $result = "$result{$inner}"
            }
            $allResults += $result
            Write-Output $result
        }
        $resultFieldsLookup."$($definition.name)" = $allResults
    }
    else {
        Write-Output $allResults;
    }
}

function Get-ResultFieldsForType {
    param(
        [Parameter(Position = 1)]
        [PSObject] $value,
        [switch] $nonNull
    )

    if ('LIST', 'NON_NULL' -ieq $value.kind ) {
        Write-Output (Get-ResultFieldsForType $value.ofType)
    }
    elseif ('OBJECT' -ieq $value.kind) {
        $obj = $objectLookup."$($value.name)"
        Write-Output ((Get-ResultFields $obj $obj.fields) -join ',')
    }
    elseif ('INPUT_OBJECT' -ieq $value.kind) {
        $obj = $objectLookup."$($value.name)"
        Write-Output ((Get-ResultFields $obj $obj.inputFields) -join ',')
    }
    else {
        Write-Output ([string]::Empty)
    }
}

function BuildObject {
    param(
        [Parameter(Position = 1)]
        [PSObject] $definition,
        [Parameter(Position = 2)]
        [PSObject] $fields
    )
    $className = Format-CSharpIdentifier $definition.name
    $resultFields = @()
    $properties = $fields | ForEach-Object {
        $propertyComment = [string]::Empty
        if ($_.description) {
            $propertyComment = `
                "        ///<summary>
        /// $($_.description)
        ///</summary>
"
        }
        $propertyName = Format-CSharpIdentifier $_.name
        $propertyType = Get-PropertyType $_.type
        $jsonOptions = , "@`"$(Format-CSharpVerbatimString $_.name)`""
        if ($_.type.kind -ieq 'NON_NULL') {
            $jsonOptions += 'Required = Required.Always'
        }
        else {
            $jsonOptions += 'NullValueHandling = NullValueHandling.Ignore'
        }
        Write-Output @{
            fieldName  = `
                "                yield return @`"$(Format-CSharpVerbatimString $_.name)`";"
            definition = `
                "$($propertyComment)        [JsonProperty($($jsonOptions -join ', '))]
        public $propertyType $propertyName { get; set; }"
        }
    }
    $resultFields += Get-ResultFields $definition $fields 
    Set-Content -Path (Join-Path $outputPath "$className.cs") -Value `
        "using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace $namespace
{
    $access partial class $className : $graphQLObjectName
    {
        [JsonIgnore]
        public override string ResultFields => @`"{$($resultFields -join ',')}`";

$($properties.definition -join "`n`n")
    }
}"
}

$schema.data.__schema.types | `
    Where-Object { $_.kind -ieq 'INPUT_OBJECT' } | `
    ForEach-Object {
    BuildObject $_ $_.inputFields
}

$schema.data.__schema.types | `
    Where-Object { $_.kind -ieq 'OBJECT' -and $_.name -ieq 'ResultData' } | `
    ForEach-Object {
    BuildObject $_ $_.fields
}

$schema.data.__schema.types | `
    Where-Object { $_.kind -ieq 'OBJECT' -and $_.name -ieq 'Mutation' } | `
    Select-Object -ExpandProperty 'fields' | `
    ForEach-Object {
    $className = "$(Format-CSharpIdentifier $_.name)Mutation"
    $resultType = Format-CSharpIdentifier (Get-PropertyType $_.type)
    $argReturns = $_.args | ForEach-Object {
        Write-Output `
            "                yield return new KeyValuePair<string, object>(@`"$(Format-CSharpVerbatimString $_.name)`", $(Format-CSharpIdentifier $_.name));"
    }
    $properties = $_.args | ForEach-Object {
        $propertyComment = [string]::Empty
        if ($_.description) {
            $propertyComment = `
                "        ///<summary>
        /// $($_.description)
        ///</summary>
"
        }
        $propertyName = Format-CSharpIdentifier $_.name
        $propertyType = Get-PropertyType $_.type
        Write-Output `
            "$($propertyComment)        public $propertyType $propertyName { get; set; }"
    }
    Set-Content -Path (Join-Path $outputPath "$className.cs") -Value `
        "using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace $namespace
{
    $access partial class $className : $graphQLMutationName<$resultType>
    {
        [JsonIgnore]
        protected override string QueryName { get { return @`"$(Format-CSharpVerbatimString $_.name)`"; } }
        
        [JsonIgnore]
        protected override IEnumerable<KeyValuePair<string, object>> Args 
        {
            get 
            {
$($argReturns -join "`n")
            }
        }

$($properties -join "`n`n")
    }
}"
}