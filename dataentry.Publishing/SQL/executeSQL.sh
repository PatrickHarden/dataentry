#!/bin/bash
set -euo pipefail
IFS=$'\n\t'

usage() { echo "Usage: $0 -a <AppName> -l <PostgresAdminLogin> -p <PostgresAdminPassword> -u <postgresFuncUser> -d <PostgresFuncPassword>" 1>&2; exit 1; }

declare appName=""
declare postgresAdminLogin=""
declare postgresAdminPassword=""
declare postgresFuncUser=""
declare postgresFuncPassword=""

# Initialize parameters specified from command line
while getopts ":a:l:p:u:d:" arg; do
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
		u)
			postgresFuncUser=${OPTARG}
			;;
		d)
			postgresFuncPassword=${OPTARG}
			;;
		esac
done
shift $((OPTIND-1))

for f in *.sql; do
	echo "Execute ${f}"
	(
		set -x
		psql -d "host=${appName}.postgres.database.azure.com port=5432 dbname=${appName} user=${postgresAdminLogin}@${appName} password='${postgresAdminPassword}' sslmode=require" -v "postgresFuncUser=${postgresFuncUser}" -v "postgresFuncPassword=${postgresFuncPassword}" -f "$f";
	)
done

echo "Create User ${postgresFuncUser}"
(
	sql="
	DO \$$
	DECLARE													
  		rolCount bigint := 0;
	BEGIN
		SELECT INTO rolCount COUNT(*)							
			FROM pg_roles										
			WHERE rolname='${postgresFuncUser}';					
		IF rolCount = 0 THEN									
			CREATE USER ${postgresFuncUser}						
				WITH LOGIN NOSUPERUSER NOCREATEDB NOCREATEROLE
				INHERIT NOREPLICATION							
				CONNECTION LIMIT -1								
				PASSWORD '${postgresFuncPassword}';		
		END IF;
	END \$$"

	set -x
	psql -d "host=${appName}.postgres.database.azure.com port=5432 dbname=${appName} user=${postgresAdminLogin}@${appName} password='${postgresAdminPassword}' sslmode=require" -v "postgresFuncUser=${postgresFuncUser}" -v "postgresFuncPassword=${postgresFuncPassword}" -c "$sql";
)

echo "GRANT User ${postgresFuncUser} to all tables in schema public"
(
	sql="
	GRANT SELECT
		ON ALL TABLES IN SCHEMA public
		TO ${postgresFuncUser}"

	set -x
	psql -d "host=${appName}.postgres.database.azure.com port=5432 dbname=${appName} user=${postgresAdminLogin}@${appName} password='${postgresAdminPassword}' sslmode=require" -v "postgresFuncUser=${postgresFuncUser}" -v "postgresFuncPassword=${postgresFuncPassword}" -c "$sql";
)

echo "GRANT User ${postgresFuncUser} to function in public.getlistingsbystate"
(
	sql="
	GRANT EXECUTE 
		ON FUNCTION public.getlistingsbystate
		TO ${postgresFuncUser}"

	set -x
	psql -d "host=${appName}.postgres.database.azure.com port=5432 dbname=${appName} user=${postgresAdminLogin}@${appName} password='${postgresAdminPassword}' sslmode=require" -v "postgresFuncUser=${postgresFuncUser}" -v "postgresFuncPassword=${postgresFuncPassword}" -c "$sql";
)

echo "GRANT User ${postgresFuncUser} to procedure in public.updatelistingtostate"
(
	sql="
	GRANT EXECUTE 
		ON PROCEDURE public.updatelistingtostate
		TO ${postgresFuncUser}"

	set -x
	psql -d "host=${appName}.postgres.database.azure.com port=5432 dbname=${appName} user=${postgresAdminLogin}@${appName} password='${postgresAdminPassword}' sslmode=require" -v "postgresFuncUser=${postgresFuncUser}" -v "postgresFuncPassword=${postgresFuncPassword}" -c "$sql";
)

echo "GRANT User ${postgresFuncUser} update on table public.ListingData"
(
	sql="
	GRANT UPDATE
		ON TABLE public.\"ListingData\"
		TO ${postgresFuncUser}"

	set -x
	psql -d "host=${appName}.postgres.database.azure.com port=5432 dbname=${appName} user=${postgresAdminLogin}@${appName} password='${postgresAdminPassword}' sslmode=require" -v "postgresFuncUser=${postgresFuncUser}" -v "postgresFuncPassword=${postgresFuncPassword}" -c "$sql";
)
