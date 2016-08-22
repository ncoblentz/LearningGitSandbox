#!/bin/bash

set -e
psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" <<-EOSQL
    CREATE USER aspnetcorekestrelresearch WITH PASSWORD 'Start123!';
    CREATE DATABASE aspnetcorekestrelresearch;
    GRANT ALL PRIVILEGES ON DATABASE aspnetcorekestrelresearch TO aspnetcorekestrelresearch;
EOSQL