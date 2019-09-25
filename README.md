# BugTrackingSystem
This is the prototype of plain BugTrackingSystem.

## Getting start
Clone this reposotiry and open the project in Visual Studio.

Install modules in ClientApp folder.
```sh
$ npm install
```

Run your SQL Server and change the connection string in the file `appsettings.json`.
```json
{
  "ConnectionStrings": {
      "DefaultConnection": "Server=10.0.1.6,1433;Database=BugTrackingSystemDB;User=SA;Password=P@ssw0rd;"
  },
```
Run the server.

The database will be created using the Code First, and also filled with initial data.

To login as admin use username: "admin", password: "Admin".

## Features
 - Authentication support
 - Create (update and delete for admin only) bugs
 - Bugs life cycle
 - Comment any changes
 - Changelog support
 - Sort bugs in main table

## Tech Stack
 - ASP.NET Core
 - Entity Framework Core
 - React.js
