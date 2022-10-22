param(
    [string] $ServerUri, # = "dev-dataentry.cbrelistings.com"
    [string] $Token) # log in and grab your adal token from local storage

$ServerUri = $ServerUri.TrimStart("http://").TrimStart("https://").TrimEnd("/").TrimEnd("/graphql")

#Setting this flag will overwrite any changes made after the previous run, don't do this in production
$updateExistingRecords = $true

#Download existing property IDs
Write-Host "Requesting existing IDs for listings and images"
$requestParams = @{
    Uri = "https://$ServerUri/graphql"
    Method = "POST"
    Headers = @{
        "Accept"="application/json, text/plain, */*"; 
        "Accepted"="application/json"; 
        "Authorization"="Bearer $Token"
    }
    ContentType = "application/json; charset=UTF-8"
    Body = "{`"query`": `"{ listings {id, isDeleted, propertyRecordName, photos{id, url},floorplans{id, url}, spaces{photos{id, url},floorplans{id,url}}}}`"}"
}

try {
    $response = Invoke-WebRequest @requestParams -ErrorAction "Stop"
} catch {
    $reader = New-Object "System.IO.StreamReader" -ArgumentList $_.Exception.Response.GetResponseStream()
    $responseBody = $reader.ReadToEnd() | ConvertFrom-Json | ConvertTo-Json;
    $reader.Dispose()
    Write-Error $responseBody
    return
}

Write-Host "Building ID mapping tables"
$existingIdMap = @{}
$photosIdMap = @{}
$floorplansIdMap = @{}
($response.Content | ConvertFrom-Json).data.listings | %{
    if (-not $_.isDeleted) {$existingIdMap."$($_.propertyRecordName)" = $_.id}
    $_.photos | %{ $photosIdMap."$($_.url)" = $_.id }
    $_.spaces.photos | %{ $photosIdMap."$($_.url)" = $_.id }
    $_.floorplans | %{ $floorplansIdMap."$($_.url)" = $_.id }
    $_.spaces.floorplans | %{ $floorplansIdMap."$($_.url)" = $_.id }
}

$stateMap = @(
    @("AL","Alabama"),
    @("AK","Alaska"),
    @("AZ","Arizona"),
    @("AR","Arkansas"),
    @("CA","California"),
    @("CO","Colorado"),
    @("CT","Connecticut"),
    @("DE","Delaware"),
    @("FL","Florida"), 
    @("GA","Georgia"), 
    @("HI","Hawaii"), 
    @("ID","Idaho"), 
    @("IL","Illinois"), 
    @("IN","Indiana"), 
    @("IA","Iowa"), 
    @("KS","Kansas"), 
    @("KY","Kentucky"), 
    @("LA","Louisiana"), 
    @("ME","Maine"), 
    @("MD","Maryland"), 
    @("MA","Massachusetts"), 
    @("MI","Michigan"), 
    @("MN","Minnesota"), 
    @("MS","Mississippi"), 
    @("MO","Missouri"), 
    @("MT","Montana"), 
    @("NE","Nebraska"), 
    @("NV","Nevada"), 
    @("NH","New Hampshire"), 
    @("NJ","New Jersey"), 
    @("NM","New Mexico"), 
    @("NY","New York"), 
    @("NC","North Carolina"), 
    @("ND","North Dakota"), 
    @("OH","Ohio"), 
    @("OK","Oklahoma"),
    @("OR","Oregon"), 
    @("PA","Pennsylvania"), 
    @("RI","Rhode Island"), 
    @("SC","South Carolina"), 
    @("SD","South Dakota"), 
    @("TN","Tennessee"), 
    @("TX","Texas"), 
    @("UT","Utah"), 
    @("VT","Vermont"), 
    @("VA","Virginia"), 
    @("WA","Washington"), 
    @("WV","West Virginia"), 
    @("WI","Wisconsin"), 
    @("WY","Wyoming")
)

$propertyTypeMap = @(
    @("office","Office"),
    @("retail","Retail"),
    @("industrial","Industrial"),
    @("land","Land"),
    @("flexindustrial","FlexIndustrial"),
    @("Healthcare","Healthcare"),
    @("SpecialPurpose","SpecialPurpose"),
    @("Hospitality","Hospitality"),
    @("Multifamily","Multifamily")
)

$statusMap = @(
    @("available", "Available"),
    @("unavailable", "Unavailable")
)

$dimensionsMap = @(
    @('sf','sqft'),
    @('sm','sqm'),
    @('hectare'),
    @('acre'),
    @('ft'),
    @('yd'),
    @('m'),
    @('person','pp'),
    @('desk'),
    @('room'),
    @('whole')
)

$leaseTermMap = @(
    @('monthly'),
    @('quarterly'),
    @('annually', 'yearly'),
    @('once')
)

$chargeKindMap = @(
    @('sale', 'SalePrice'),
    @('lease', 'Rent'),
    @('salelease'),
    @('FlexRent')
)

function Map {
    param ($map,$value)
    $result = $map | ?{$_ -ieq $value} | %{$_[0]} | Select-Object -First 1
    if ($result) {
        Write-Output $result
    } else {
        Write-Output $null
    }
}

function Coalesce {
    Write-Output $args | Where-Object { $_ -ne $null } | Select-Object -First 1
}

function Get-EnglishText {
    param($textType)
    $usResult = $textType | Where-Object { $_.'Common.CultureCode' -ieq 'en-US' } | Select-Object -ExpandProperty 'Common.Text' -First 1
    $gbResult = $textType | Where-Object { $_.'Common.CultureCode' -ieq 'en-GB' } | Select-Object -ExpandProperty 'Common.Text' -First 1
    Write-Output (Coalesce $usResult $gbResult $null)
}

function Get-EnglishLink {
    param($linkType)
    $usResult = $linkType | Where-Object { $_.'Common.CultureCode' -ieq 'en-US' } | Select-Object -ExpandProperty 'Common.Link' -First 1
    $gbResult = $linkType | Where-Object { $_.'Common.CultureCode' -ieq 'en-GB' } | Select-Object -ExpandProperty 'Common.Link' -First 1
    Write-Output (Coalesce $usResult $gbResult $null)
}

function MapSingleAddressLine {
    param([PSCustomObject]$mappedListing,
    [string]$targetPropertyName,
    [string]$addressLine)
    
    if ([Regex]::IsMatch($addressLine, "^\d{5}(?:-\d{4})?$")) { # zip code regex
        if (-not $mappedListing.postalCode) {
            $mappedListing.postalCode = $addressLine
        }
    } elseif ($state = Map $stateMap $addressLine) {
        if (-not $mappedListing.stateOrProvince) {
            $mappedListing.stateOrProvince = $state
        }
    } elseif (-not $mappedListing.city) {
        $mappedListing.city = $addressLine;
    } elseif (($addressLine -ine $mappedListing.city) -and $targetPropertyName) {
        $mappedListing.$targetPropertyName = $addressLine;
    }
}

function Get-Photos {
    param($source)
    if ($source)
    {
        $order = 0
        $photos = [Array]($source | Sort-Object -Property 'Common.Order' | % {
            $imageResource = $_.'Common.ImageResources' | ? { $_.'Common.BreakPoint' -ieq 'original' }
            if (-not $imageResource) {
                $imageResource = $_ | Sort-Object -Property 'Common.Image.Width' -Descending
            }
            [string] $url = $imageResource[0].'Source.Uri'
            [string] $displayText = $_.'Common.ImageCaption'
            $ext = $url.Substring($url.LastIndexOf('.'))
            if (('.jpg','.jpeg','.png') -ieq $ext) {
                $displayText += $ext
            }

            $id = Coalesce $photosIdMap[$url] 0

            Write-Output @{
                id = $id
                url = $url
                displayText = $displayText
                active = $true
                watermark = $false
                primary = $false
                order = $order++
            }
        } | ? { $_.url })
        $firstPhoto = $photos | Select-Object -First 1
        if ($firstPhoto) {
            $firstPhoto.primary = $true
        }
        Write-Output $photos
    } else {
        Write-Output $null
    }
}


$accessMap = @{}
Get-Content '.\Plus-Id-Access.csv' | %{
    $split = $_ -split ','
    $accessMap."$($split[0])" = ($split | Select-Object -Skip 1 -Unique)
}

$mappedListings = @();
$lines = Get-Content .\data.json -Encoding UTF8 #| ?{$_.Contains('US-Plus-454039')} #| Select -First 100 # uncomment to test a partial dataset 
$lineCounter = 0
foreach($line in $lines) {
    $lineCounter++
    Write-Output "Mapping line $lineCounter of $($lines.Length)"
    $data = ConvertFrom-Json $line
    #if ($lineCounter -ne 197) { continue }
    $mappedListing = @{
          propertyRecordName = $data.'_id'
          propertyType = Map $propertyTypeMap $data.'_source'.'Common.UsageType'
          propertySubType = $data.'_source'.'Common.PropertySubType'
          website = $data.'_source'.'Common.Website'
          headline = Get-EnglishText $data.'_source'.'Common.Strapline'
          video = Get-EnglishLink $data.'_source'.'Common.VideoLinks'
          walkThrough = $data.'_source'.'Common.Walkthrough'
          lat = $data.'_source'.'Common.Coordinate'.'lat'
          lng = $data.'_source'.'Common.Coordinate'.'lon'
          floors = $data.'_source'.'Common.NumberOfStoreys'
          buildingDescription = Get-EnglishText $data.'_source'.'Common.LongDescription'
          locationDescription = Get-EnglishText $data.'_source'.'Common.LocationDescription'
          status = Map $statusMap $data.'_source'.'Common.Status'
          availableFrom = $data.'_source'.'Common.AvailableFrom'
          specifications = @{
            leaseType = [string]($data.'_source'.'Common.LeaseTypes' | Select-Object -First 1)
            leaseRateType = $data.'_source'.'Common.LeaseRateType'
            bedrooms = $data.'_source'.'Common.NumberOfBedrooms'
          }
          photos = Get-Photos $data.'_source'.'Common.Photos'
          userNames = [string[]]$accessMap."$($data.'_id')"
        }

    # Address format is all over the place. These are the most common "rules" but it might not be perfect.
    $address = Coalesce $data.'_source'.'Common.Address' $data.'_source'.'Common.ActualAddress'

    $mappedListing.postalCode = $address.'Common.PostCode'
    MapSingleAddressLine $mappedListing $null $address.'Common.Locallity'

    $addressLines = $address.'Common.Line1', $address.'Common.Line2', $address.'Common.Line3', $address.'Common.Line4' -ne $null
    if ($addressLines.Count -eq 1) {
        $mappedListing.street = $addressLines[0];
    } elseif ($addressLines.Count -eq 2) {
        $mappedListing.street = $addressLines[0];
        if ([Regex]::IsMatch($addressLines[1], '\d')) { 
            $mappedListing.street2 = $addressLines[1];
        } else {
            MapSingleAddressLine $mappedListing "street2" $addressLines[1]
        }
    } elseif ($addressLines.Count -eq 3) {
        $mappedListing.propertyName = $addressLines[0];
        $mappedListing.street = $addressLines[1];
        MapSingleAddressLine $mappedListing "street2" $addressLines[2]
    } else {
        # Currently no records use all 4 lines, wrote something just in case
        $mappedListing.propertyName = $addressLines[0];
        $mappedListing.street = $addressLines[1];
        $mappedListing.street2 = $addressLines[2];
        MapSingleAddressLine $mappedListing $null $addressLines[1]
    }

    # Listing type
    $isSale = $data.'_source'.'Common.Aspects' -ieq 'isSale'
    $isLease = $data.'_source'.'Common.Aspects' -ieq 'isLetting'
    if ($isSale -and $isLease) {
        $mappedListing.listingType = "salelease"
    } elseif ($isSale) {
        $mappedListing.listingType = "sale"
    } elseif ($isLease) {
        $mappedListing.listingType = "lease"
    }

    # Brochures
    # The culture code seems to be set as en-GB on all brochures... seems like an error.
    # Some of the links go to a directory listing cbre.box.com and not an actual pdf
    # A majority of the links 
    $allBrochures = Coalesce ($data.'_source'.'Common.Brochures' | ? { $_.'Common.CultureCode' -ieq 'en-US' }) ($data.'_source'.'Common.Brochures' | ? { $_.'Common.CultureCode' -ieq 'en-GB' })
    if ($allBrochures) {
        $mappedListing.brochures = [Array]($allBrochures | % {
            Write-Output @{
                url = $_.'Common.Uri'  -ireplace '^/resources/fileassets/', 'https://wwwlistingssearchcbreeun.blob.core.windows.net/fileassets/'
                displayText = Coalesce $_.'Common.BrochureName' ''
                active = $true
                primary = $false
            }
        } | ? { $_.url })
        $firstBrochure = $mappedListing.brochures | Select-Object -First 1
        if ($firstBrochure) {
            $firstBrochure.primary = $true
        }
    }

    # Floorplans
    if ($data.'_source'.'Common.FloorPlans') {
        $mappedListing.floorplans = [Array]($data.'_source'.'Common.FloorPlans' | % {
            $imageResource = $_.'Common.ImageResources' | Sort-Object -Property 'Common.Image.Width' -Descending | Select-Object -First 1
            $url = $imageResource.'Common.Resource.Uri'  -ireplace '^/resources/fileassets/', 'https://wwwlistingssearchcbreeun.blob.core.windows.net/fileassets/'
            Write-Output @{
                id = Coalesce $floorplansIdMap[$url] 0
                url = $url
                displayText = $_.'Common.ImageCaption'
                active = $true
                primary = $false
                watermark = $false
            }
        })
        $firstFloorplan = $mappedListing.floorplans | Select-Object -First 1
        if ($firstFloorplan) {
            $firstFloorplan.primary = $true
        }
    }

    # Highlights
    if ($data.'_source'.'Common.Highlights') {
        $mappedListing.highlights = [Array]($data.'_source'.'Common.Highlights' | % {
            Write-Output @{
                value = Get-EnglishText $_.'Common.Highlight'
                order = 0
            }
        })
        if ($mappedListing.highlights) {
            $i = 0
            $mappedListing.highlights | % {
                $_.order = $i++
            }
        }
    }

    # Sizes
    if ($data.'_source'.'Common.Sizes') {
        $mappedListing.propertySizes = [Array]($data.'_source'.'Common.Sizes' | % {
            Write-Output @{ 
                sizeKind = $_.'Common.SizeKind'
                measureUnit = $_.'Common.Dimensions' | Select-Object -ExpandProperty 'Common.DimensionsUnits' -First 1
                amount = $_.'Common.Dimensions' | Select-Object -ExpandProperty 'Common.Amount' -First 1
            }
        })
    }

    # Charges
    if ($data.'_source'.'Common.Charges') {
        $mappedListing.chargesAndModifiers = [Array]($data.'_source'.'Common.Charges' | ? { -not ('Rent','FlexRent','SalePrice' -contains $_.'Common.ChargeKind') } | % {
            $result = @{
                chargeType = Map $chargeKindMap $_.'Common.ChargeKind'
                chargeModifier = $_.'Common.ChargeModifer'
                term = Map $leaseTermMap $_.'Common.Interval'
                amount = $_.'Common.Amount'
                perUnitType = $_.'Common.PerUnit'
                currencyCode = $_.'Common.CurrencyCode'
            }

            if ($result.amount) {
                $idx =  $result.amount.ToString().indexOf('.')
                if ($idx -gt -1) {
                    $result.amount = [Int]::Parse($result.amount.ToString().Substring(0,$idx))
                }
            }

            Write-Output $result
        }) | ?{$_.chargeType}
        $mappedListing.specifications.contactBrokerForPrice = $false
        if ($rent = $data.'_source'.'Common.Charges' | ? { 'lease','FlexRent' -ieq (Map $chargeKindMap $_.'Common.ChargeKind') }) {
            $mappedListing.specifications.contactBrokerForPrice = $mappedListing.specifications.contactBrokerForPrice -or [bool]($rent.'Common.OnApplication')
            $mappedListing.specifications.minPrice = $rent.'Common.Amount'
            $mappedListing.specifications.leaseTerm = Map $leaseTermMap $rent.'Common.Interval'
            if ($mappedListing.specifications.leaseTerm -ieq 'annually') 
            {
                $mappedListing.specifications.leaseTerm = 'yearly';
            }

            $mappedListing.specifications.measure = Map $dimensionsMap $rent.'Common.PerUnit'
            $mappedListing.specifications.currencyCode = $rent.'Common.CurrencyCode'
            $mappedListing.specifications.taxModifer = $rent.'Common.TaxModifer'
        }

        if ($sale = $data.'_source'.'Common.Charges' | ? { (Map $chargeKindMap $_.'Common.ChargeKind') -ieq 'sale' }) {
            $mappedListing.specifications.contactBrokerForPrice = $mappedListing.specifications.contactBrokerForPrice -or [bool]($sale.'Common.OnApplication')
            $mappedListing.specifications.currencyCode = $sale.'Common.CurrencyCode'
            $mappedListing.specifications.salePrice = $sale.'Common.Amount'
            $mappedListing.specifications.taxModifer = $sale.'Common.TaxModifer'
        }
    }

    # Sizes
    $mappedListing.specifications.minSpace = ($_.'Common.Sizes' | ? { $_.'Common.SizeKind' -ieq 'MinimumSize' }).'Common.Dimensions'.'Amount' | Select-Object -First 1
    $mappedListing.specifications.maxSpace = ($_.'Common.Sizes' | ? { $_.'Common.SizeKind' -ieq 'MaximumSize' }).'Common.Dimensions'.'Amount' | Select-Object -First 1
    $mappedListing.specifications.totalSpace = ($_.'Common.Sizes' | ? { $_.'Common.SizeKind' -ieq 'TotalSize' }).'Common.Dimensions'.'Amount' | Select-Object -First 1

    # Spaces
    if ($data.'_source'.'Common.FloorsAndUnits') {
        $mappedListing.spaces = $data.'_source'.'Common.FloorsAndUnits' | % {
            $space = @{
                name = Get-EnglishText $_.'Common.SubdivisionName'
                spaceDescription = Get-EnglishText $_.'Common.SpaceDescription'
                status = Map $statusMap $_.'Common.Unit.Status'
                spaceType = Map $propertyTypeMap $_.'Common.Unit.Use'
                availableFrom = $_.'Common.AvailableFrom'
                specifications = @{
                    maxSpace = $_.'Common.Areas'.'Common.Area' | Select -First 1
                    measure = Map $dimensionsMap ($_.'Common.Areas'.'Common.Units' | Select -First 1)
                }
                #No space level photos/floorplans/brochures in plus data
            }

            Write-Output $space
        }
    } 

    # Contacts
    if ($data.'_source'.'Common.ContactGroup'.'Common.Contacts') {
        $mappedListing.contacts = [Array]($data.'_source'.'Common.ContactGroup'.'Common.Contacts' | %{
            if ($_.'Common.EmailAddress') {
                Write-Output @{
                    firstName = ([string]$_.'Common.AgentName').Split(' ', 2)[0]
                    lastName = ([string]$_.'Common.AgentName').Split(' ', 2)[1]
                    phone = $_.'Common.TelephoneNumber'
                    email = $_.'Common.EmailAddress'
                }
            }
        })
    }
    
    # Aspects
    if ($data.'_source'.'Common.Aspects') {
        $mappedListing.aspects = [Array]($data.'_source'.'Common.Aspects' | ?{-not ('isSale','isLetting' -ieq $_)})
    }

    $mappedListings += $mappedListing
    #if ($mappedListing.chargesAndModifiers) { $mappedListing | ConvertTo-Json }
}

$idMap = $mappedListings | % {
    $updateMethod = "createListing"
    if ($existingId = $existingIdMap."$($_.propertyRecordName)") {
        $updateMethod = "updateListing"
        $_.id = $existingId
    }

    if ($updateExistingRecords -or $updateMethod -eq "createListing") {
        Write-Host "$updateMethod $($_.propertyRecordName) $($_.id)"
        $cleanListing =
            [System.Web.HttpUtility]::JavaScriptStringEncode([Regex]::Replace(
                ($_ | ConvertTo-Json -Depth 100), 
                '^\s*"([^"]+)":', '$1:', 
                [System.Text.RegularExpressions.RegexOptions]::Multiline))
        $requestParams = @{
            Uri = "https://$ServerUri/graphql"
            Method = "POST"
            Headers = @{
                "Accept"="application/json, text/plain, */*"; 
                "Accepted"="application/json"; 
                "Authorization"="Bearer $Token"
            }
            ContentType = "application/json; charset=UTF-8"
            Body = "{`"query`": `"mutation { $updateMethod(listing:$cleanListing) {id}}`"}"
        }
        try {
            $response = Invoke-WebRequest @requestParams -ErrorAction "Stop"
        } catch {
            $reader = New-Object "System.IO.StreamReader" -ArgumentList $_.Exception.Response.GetResponseStream()
            $responseBody = $reader.ReadToEnd() | ConvertFrom-Json | ConvertTo-Json;
            $reader.Dispose()
            Write-Error $responseBody
            return
        }

        Write-Host $response.content

        Write-Output @{
            'Plus ID' = $_.propertyRecordName; 
            'DataEntry ID' = ($response.Content | ConvertFrom-Json).data."$updateMethod".id
        }
    } else {
        Write-Host "Skipping existing property: $($_.id) $($_.propertyRecordName)"
    }
}

$idMap | %{"$($_.'Plus ID'),$($_.'DataEntry ID')"} | Set-Content -Path "idMap.csv"