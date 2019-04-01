# Folio console application

## Description
 
This project includes a console application that can be used to load and save data to and from a FOLIO database to JSON files. The data can be loaded and saved using SQL or the FOLIO web API.

## Requirements

* Git https://git-scm.com/
* .NET Core SDK v2.2 https://dotnet.microsoft.com/download

## Optional

* Visual Studio 2017 https://visualstudio.microsoft.com/vs/community/
* Visual Studio Code https://code.visualstudio.com/?wt.mc_id=vscom_downloads

## Installation

Get a copy of the project source code by cloning it from the GitHub repository.

```
git clone https://github.com/jemiller0/Folio.git
```

Build the project.

```
cd Folio
dotnet build
```

Run the application without specifying any arguments for the first time to generate default configuration files and see the command-line options.

```
cd FolioConsoleApplication/bin/Debug/netcoreapp2.2
dotnet FolioConsoleApplication.dll
```

To load and save data using PostgreSQL SQL, specify the host name, username, password, and database in `ConnectionStrings.config`.

```
vim ConnectionStrings.config
```

To load and save data using the FOLIO web API, Specify the URL, tenant, username, and password in `AppSettings.config`.

```
vim AppSettings.config
```

## Examples

### Save all using SQL

```
dotnet FolioConsoleApplication.dll -save -all
```

### Save all using web API

```
dotnet FolioConsoleApplication.dll -save -all -api
```

### Load all using SQL

```
dotnet FolioConsoleApplication.dll -delete -load -all
```

### Save all users module using SQL

```
dotnet FolioConsoleApplication.dll -save -allusers
```

### Load all users module using SQL

```
dotnet FolioConsoleApplication.dll -delete -load -allusers
```

### Save users using SQL

```
dotnet FolioConsoleApplication.dll -save -userspath users.json
```

### Load users using SQL

```
dotnet FolioConsoleApplication.dll -delete -load -userspath users.json
```

### Save users, permissions users, and logins without admin using SQL
```
dotnet FolioConsoleApplication.dll -save -userspath users.json -userswhere "id != '53c92517-e96d-4233-b9e6-3cc410bf36bf'" -permissionsuserspath permissionsusers.json -permissionsuserswhere "jsonb->>'userId' != '53c92517-e96d-4233-b9e6-3cc410bf36bf'" -loginspath logins.json -loginswhere "jsonb->>'userId' != '53c92517-e96d-4233-b9e6-3cc410bf36bf'" -verbose
```

### Load users, permissions users, and logins without admin using SQL
```
dotnet FolioConsoleApplication.dll -delete -load -userspath users.json -userswhere "id != '53c92517-e96d-4233-b9e6-3cc410bf36bf'" -permissionsuserspath permissionsusers.json -permissionsuserswhere "jsonb->>'userId' != '53c92517-e96d-4233-b9e6-3cc410bf36bf'" -loginspath logins.json -loginswhere "jsonb->>'userId' != '53c92517-e96d-4233-b9e6-3cc410bf36bf'" -verbose
```

### Save users and permissions users without admin using web API
```
dotnet FolioConsoleApplication.dll -save -userspath users.json -userswhere "id <> 53c92517-e96d-4233-b9e6-3cc410bf36bf" -permissionsuserspath permissionsusers.json -permissionsuserswhere "userId <> 53c92517-e96d-4233-b9e6-3cc410bf36bf" -api
```

### Load users and permissions users without admin using web API
```
dotnet FolioConsoleApplication.dll -delete -load -userspath users.json -userswhere "id <> 53c92517-e96d-4233-b9e6-3cc410bf36bf" -permissionsuserspath permissionsusers.json -permissionsuserswhere "userId <> 53c92517-e96d-4233-b9e6-3cc410bf36bf" -api
```

### Save all inventory module and validate
```
dotnet FolioConsoleApplication.dll -save -allinventory -validate
```

## Parameters

```
-All
-Api
-Delete
-Load
-Save
-Validate
-Verbose
-All{Module}
-{Module}Path <string>
-{Module}Where <string>
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
* Querying data using the web API does not page the results, all data is streamed from one API call
* Logins can't be round-tripped using web API, the API uses a different JSON schema than is used in the database
* SQL views for tables that have large numbers of instances may not be performant
* Doesn't currently use a robust command-line parsing library, mispelled arguments will be ignored
* Loading/saving of InstanceSourcMarc is not supported due to unconventional JSON (I think this is an obsolete table anyway)

## Future enhancements

* Switch to using bulk load web APIs when if/when they become available
* Parse command-line arguments in more robust manner
* Add multi-threading support
* Add support for reading GZipped files
* Switch to new memory optimized Microsoft JSON parser

## References

FOLIO API Documentation
https://dev.folio.org/reference/api/
A Gentle Introduction to CQL
http://zing.z3950.org/cql/intro.html

## Author

Jon Miller
