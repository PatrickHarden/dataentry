#!/bin/bash
set -euo pipefail
IFS=$'\n\t'

usage() { echo "Usage: $0 -a <AppName> -l <PostgresAdminLogin> -p <PostgresAdminPassword> -f <SQLFile>" 1>&2; exit 1; }

declare appName=""
declare postgresAdminLogin=""
declare postgresAdminPassword=""
declare SQLFile=""

# Initialize parameters specified from command line
while getopts ":a:l:p:f:d:" arg; do
	case "${arg}" in
		a)
			appName=${OPTARG}
			;;
		l)
			postgresAdminLogin=${OPTARG}
			;;
		p)
			postgresAdminPassword=${OPTARG}
			;;
		f)
			SQLFile=${OPTARG}
			;;
		esac
done
shift $((OPTIND-1))

echo "Execute ${SQLFile}"
(
	set -x
	psql -d "host=${appName}.postgres.database.azure.com port=5432 dbname=${appName} user=${postgresAdminLogin}@${appName} password='${postgresAdminPassword}' sslmode=require" -f "${SQLFile}";
)
