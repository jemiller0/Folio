# Folio command-line import/export utility

## Description

This project includes a command-line application that can be used to load and save data to and from a FOLIO database to JSON files. The data can be loaded and saved using either the FOLIO web API or using the PostgreSQL COPY command.

## Requirements

*Git https://git-scm.com/
*.NET Core SDK v2.2 https://dotnet.microsoft.com/download

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

To load and save data using PostgreSQL COPY, specify the host name, username, password, and database in `ConnectionStrings.config`.

```
vim ConnectionStrings.config
```

To load and save data using the FOLIO web API, Specify the URL, tenant, username, and password in `AppSettings.config`.

```
vim AppSettings.config
```

## Examples

### Save all data to files using COPY

```
dotnet FolioConsoleApplication.dll -save -all
```

### Save all data to files using web API

```
dotnet FolioConsoleApplication.dll -save -all -api
```

### Load all data to database using COPY

```
dotnet ../FolioConsoleApplication.dll -delete -load -all
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
```
