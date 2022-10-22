#!/bin/bash

# Setting Up Credentials 
echo "start defining credentials for DBs.........."

echo "setting up local db credentials.........."
declare dbHostLocal="localhost"
declare dbUserLocal="postgres"
declare dbNameLocal="DataEntry"
declare dbPasswordLocal="password"

echo "setting up preprod db credentials.........."
declare dbHostPreprod="cbrelistings-preprod-dataentry.postgres.database.azure.com"
declare dbUserPreprod="cbreadmin@cbrelistings-preprod-dataentry"
declare dbNamePreprod="cbrelistings-preprod-dataentry"

echo "start copying down data.........."
pg_dump -h ${dbHostPreprod} -U ${dbUserPreprod} -c ${dbNamePreprod} > dump.sql

echo "start restoring data into local db.........."
psql -h ${dbHostLocal} -U ${dbUserLocal} -d ${dbNameLocal} -f dump.sql

echo "remove dump file"
rm dump.sql