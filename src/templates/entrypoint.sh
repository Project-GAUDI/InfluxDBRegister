#! /bin/sh

cat application.info

exec dotnet InfluxDBRegister.dll "$@"
