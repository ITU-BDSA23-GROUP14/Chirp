# Chirp

This is a project for the course 'Analysis, Design and Software Architecture (Autumn 2023)' at the IT-University of Copenhagen.

The Razor page application is deployed and can be accessed at: https://bdsagroup14chirprazor.azurewebsites.net/ \
The CLI application is deployed and can be accessed at:         https://bdsagroup14chirpremotedb.azurewebsites.net/cheeps 

## Running the project
To run the project on your local PC, a few things are needed:

- You need to have Docker Desktop up and running.

- You need to have an environment variable set defining ConnectionStrings:Chirp. This can also be done using dotnet user-secrets as such (from the src/Chirp.Web folder):

```
dotnet user-secrets set "ConnectionStrings:Chirp" "Data Source=localhost,1433;Initial Catalog=Chirp;User=sa;Password=33eca922-74a0-11ee-9e21-00155d9a126b;TrustServerCertificate=True"
```

- To get authentication to work, you need to have an enviroment variable set defining AzureAdB2C:ClientId. This can also be done using dotnet user-secrets as such (from the src/Chirp.Web folder):

```
dotnet user-secrets set "AzureAdB2C:ClientId" "4f6b92aa-49e6-4e9b-b2ba-5a00ab7402f7"
```

- Once the above steps are done, you can run the project by running the "run.bat" file.

## Group members
Markus Æbelø Faurbjerg - mfau@itu.dk \
Niklas Aaron Sherman Andersen - naaa@itu.dk \
Dagrún Eir Ásgeirsdóttir - daas@itu.dk \
Ida Barkou Vilstrup - idavilstrup@gmail.com \
Jonas Fuhr Høyer - jfho@itu.dk

# Issue formatting
All issues should follow this format

### Overview
The overview should summarize the objectives of the task

### User story
The user story should provide human centric context to the task 

### Acceptance critera
The acceptance criteria should be a list of subtasks that collectively complete the task 

### Full task description
The full task description should be a copy of the task provided by the course

# Releases
Release rules are based on: https://semver.org/

Releases are of the format <vx.y.z> where 
- x: MAJOR version when you make incompatible API changes
- y: MINOR version when you add functionality in a backward compatible manner
- z: PATCH version when you make backward compatible bug fixes