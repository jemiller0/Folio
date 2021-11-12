# Folio console application

## Description

This project includes a console application that can be used to load and save data to and from a FOLIO database and JSON files. The data can be loaded and saved using SQL or the FOLIO web API.

## Requirements

* Git https://git-scm.com/
* .NET 6 SDK https://dotnet.microsoft.com/download

## Optional

* Visual Studio 2022 https://visualstudio.microsoft.com/vs/community/
* Visual Studio Code https://code.visualstudio.com/?wt.mc_id=vscom_downloads

## Installation

Get a copy of the project source code by cloning it from the GitHub repository.

```
git clone https://github.com/jemiller0/Folio.git
```

Change to the project directory.

```
cd Folio
```

If you are going to use SQL and are using a tenant name other than the default "diku" tenant name, change the tenant name to the name of your tenant. Replace TENANT below with the name of your tenant. Warning: this command does a recursive replace on all files in the current directory. Make sure you run it from the project directory.

```
grep -rlZ 'diku_' . | xargs -0 sed -i 's/diku_/TENANT_/g'
```

Build the project.

```
dotnet build
```

Run the application without specifying any arguments for the first time to generate default configuration files and see the command-line options.

```
cd FolioConsoleApplication/bin/Debug/net6.0
./folio
```

To load and save data using PostgreSQL SQL, specify the host name, username, password, and database for the FolioContext connection string in `ConnectionStrings.config`.

```
nano ConnectionStrings.config
```

To load and save data using the FOLIO web API, specify the URL, tenant, username, and password for the FolioServiceClient connection string in `ConnectionStrings.config`.

```
nano ConnectionStrings.config
```

To have exceptions sent using email, specify the smtpHost, emailAddress, and emailName in `AppSettings.config`.

```
nano AppSettings.config
```

## Examples

### Save all using SQL

```
./folio -save -all
```

### Save all using web API

```
./folio -save -all -api
```

### Load all using SQL

```
./folio -delete -load -all
```

### Save all users module using SQL

```
./folio -save -allusers
```

### Load all users module using SQL

```
./folio -delete -load -allusers
```

### Save users using SQL using default file name

```
./folio -save -users
```

### Save users using SQL

```
./folio -save -userspath users.json
```

### Load users using SQL using default file name

```
./folio -delete -load -users
```

### Load users using SQL

```
./folio -delete -load -userspath users.json
```

### Validate and update users, and disable users that weren't updated excluding the current user using SQL

```
./folio -validate -update -disable -userspath users.json
```

### Validate and update users, and disable users that weren't updated excluding the current user and matching a where filter using SQL

```
./folio -validate -update -disable -userspath users.json -userswhere "jsonb#>>'{customFields,source}' = 'University'"
```

### Update users using API

```
./folio -update -userspath users.json -api
```

### Update users using user-import API

```
./folio -import -userspath users.json -merge -source Library -disable
```

### Save users, permissions users, and logins without admin using SQL
```
./folio -save -userspath users.json -userswhere "id != '53c92517-e96d-4233-b9e6-3cc410bf36bf'" -permissionsuserspath permissionsusers.json -permissionsuserswhere "jsonb->>'userId' != '53c92517-e96d-4233-b9e6-3cc410bf36bf'" -loginspath logins.json -loginswhere "jsonb->>'userId' != '53c92517-e96d-4233-b9e6-3cc410bf36bf'" -verbose
```

### Load users, permissions users, and logins without admin using SQL
```
./folio -delete -load -userspath users.json -userswhere "id != '53c92517-e96d-4233-b9e6-3cc410bf36bf'" -permissionsuserspath permissionsusers.json -permissionsuserswhere "jsonb->>'userId' != '53c92517-e96d-4233-b9e6-3cc410bf36bf'" -loginspath logins.json -loginswhere "jsonb->>'userId' != '53c92517-e96d-4233-b9e6-3cc410bf36bf'" -verbose
```

### Save users and permissions users without admin using web API
```
./folio -save -userspath users.json -userswhere "id <> 53c92517-e96d-4233-b9e6-3cc410bf36bf" -permissionsuserspath permissionsusers.json -permissionsuserswhere "userId <> 53c92517-e96d-4233-b9e6-3cc410bf36bf" -api
```

### Load users and permissions users without admin using web API
```
./folio -delete -load -userspath users.json -userswhere "id <> 53c92517-e96d-4233-b9e6-3cc410bf36bf" -permissionsuserspath permissionsusers.json -permissionsuserswhere "userId <> 53c92517-e96d-4233-b9e6-3cc410bf36bf" -api
```

### Save all inventory module and validate
```
./folio -save -allinventory -validate -force
```

### Validate all
```
./folio -save -all -validate -force -whatif
```

### Save all using SQL to GZip compressed files

```
./folio -save -all -compress
```

### Load all using SQL from GZip compressed files

```
./folio -delete -load -all -compress
```

### Load users using SQL from GZip compressed file

```
./folio -delete -load -userspath users.json.gz
```

### Save all using SQL to path

```
./folio -save -all -path data
```

### Save all using SQL using 16 threads

```
./folio -save -all -threads 16
```

### Query users using API

```
./folio -api -query -users -where 'personal.lastName == "Miller"' -orderby 'username' -select 'id,username,personal.firstName,personal.lastName,personal.addresses[0].countryId as country' -skip 1 -take 3
```

### Query address types using SQL and pipe it to the PowerShell ConvertFrom-Json cmdlet so that it can be displayed as a formatted table

```
./folio -query -addresstypes -where "jsonb->>'addressType' IN ('Campus', 'Home')" -select 'id,addressType' | ConvertFrom-Json
```

```
id                                   addressType
--                                   -----------
7e99be3c-ada2-4e88-9303-a2c8a8e2b08e Campus
2abe6fe7-519f-40fd-ba24-9c2a14fbf4cb Home
```

### Query address types using SQL and pipe it to the PowerShell ConvertFrom-Json cmdlet and then to the ConvertTo-Csv cmdlet to convert it to CSV

```
./folio -query -addresstypes -where "jsonb->>'addressType' IN ('Campus', 'Home')" -select 'id,addressType' | ConvertFrom-Json | ConvertTo-Csv
```

```
"id","addressType"
"7e99be3c-ada2-4e88-9303-a2c8a8e2b08e","Campus"
"2abe6fe7-519f-40fd-ba24-9c2a14fbf4cb","Home"
```

## Parameters

```
-All
-Api
-Compress
-Connection <string>
-ConnectionString <string>
-Delete
-Disable
-Force
-Load
-OrderBy <string>
-Path <string>
-Query
-Select <string>
-Save
-Skip <int>
-Take <int>
-Threads <int>
-TracePath <string>
-UniversalTime
-Validate
-Verbose
-Warning
-WhatIf
-Where <string>
-All{Module}
-{Entity}
-{Entity}Path <string>
-{Entity}Where <string>
```

## Notes

-Where filters use SQL syntax when using SQL and CQL syntax when using the web API

## SQL views

FolioLibrary/Folio.sql contains SQL views that can be helpful for reviewing loaded data

## Warnings

* Uses latest SQL and JSON schemas which may not match the version of FOLIO that you are using
* Querying data using the web API does not page the results, all data is streamed from one API call
* Be careful not to delete admin account you are using to load users
* SQL loading disables foreign key constraints for the session
* Make sure date/time values are in UTC with Z at the end

## Limitations

* SQL support currently assumes you are using the diku default tenant name
* Logins can't be round-tripped using web API, the API uses a different JSON schema than is used in the database
* SQL views for tables that have large numbers of instances may not be performant
* Doesn't currently use a robust command-line parsing library, mispelled arguments will be ignored

## Future enhancements

* Add support for additional FOLIO modules
* Switch to using bulk load web APIs when if/when they become available
* Parse command-line arguments in more robust manner
* Switch to new memory optimized Microsoft JSON parser

## References

FOLIO API Documentation
https://dev.folio.org/reference/api/
A Gentle Introduction to CQL
http://zing.z3950.org/cql/intro.html

## Author

Jon Miller
