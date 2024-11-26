#! /bin/bash

cat application.info

exec dotnet InfluxDBRegister.dll "$@"
