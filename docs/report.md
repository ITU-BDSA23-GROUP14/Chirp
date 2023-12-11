---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2023 Group 14
author:
- "Markus Æbelø Faurbjerg <mfau@itu.dk>"
- "Jonas Fuhr Høyer <jfho@itu.dk>"
- "Ida Barkou Vilstrup <idavilstrup@gmail.com>"
- "Niklas Aaron Sherman Andersen <naaa@itu.dk>"
- "Dagrún Eir Ásgeirsdóttir <daas@itu.dk>"
- "Nicholas Hansen <nicha@itu.dk>"
numbersections: true
---

# Design and Architecture of _Chirp!_

## Domain model

Here comes a description of our domain model.

![Illustration of the _Chirp!_ data model as UML class diagram.](docs/images/domain_model.png)

## Architecture — In the small

## Architecture of deployed application

## User activities

## Sequence of functionality/calls through _Chirp!_

# Process

## Build, test, release, and deployment

## Team work
Project board:


Activity flow:

At the beginning of every week, we created one issue for each task. Each issue followed the same format of:
- Overview: A brief description of the task.
- User Story: A human centric context for the task.
- Acceptance criteria: A list of subtasks required for completing the task.
- Full task description: For the mandatory weekly tasks, we included the full task description from the course Github page. (This was omitted on freestyle features)

When starting a task, we used the Github feature of creating a branch directly connected to each issue, and used the automatic naming convention for these branches. 
Once a branch was created, and specific people were assigned to the issue, the task was moved from "Backlog" to "In progress" on the project board.
Once a task was finished, a pull request was created to the main branch. 
A pull request required at least two reviews and successful test actions to be merged. Team members who did not contribute to the specific task were prioritized as reviewers. If all members worked on the task, this requirement was bypassed.
Once a pull request was merged, the branch was deleted and the task was automatically moved to "Done" on the project board.
If any new issues came up during the week, either in form of bug-fixes or new features, these were created immediately. 

## How to make _Chirp!_ work locally
Prerequisites: git, .NET 7, and Docker Desktop
Clone the repository to a desired directory with the following command:
```
git clone https://github.com/ITU-BDSA23-GROUP14/Chirp.git
```
Then run the next command to start up a docker container:
```
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=33eca922-74a0-11ee-9e21-00155d9a126b" -p 1433:1433 --name sql-server -d mcr.microsoft.com/mssql/server:2022-latest
```
Once the docker container has been initialized, from the Chirp/src/Chirp.Web directory run the following commands to set user-secrets:
```
dotnet user-secrets set "ConnectionStrings:Chirp" "Data Source=localhost,1433;Initial Catalog=Chirp;User=sa;Password=33eca922-74a0-11ee-9e21-00155d9a126b;TrustServerCertificate=True"
dotnet user-secrets set "AzureAdB2C:ClientId" "4f6b92aa-49e6-4e9b-b2ba-5a00ab7402f7"
```
Lastly, to start the project run the following command from the Chirp/src/Chirp.Web directory:
```
dotnet run
```
_Chirp!_ is then running locally and is accessible through http://localhost:5273/ in your chosen browser.

## How to run test suite locally
Prerequisites: .NET 7 and Playwright installed.
Navigate to the outermost directory named "Chirp" and run the 'dotnet test' command in the terminal.

We perform End2End tests, Integration tests, Unit tests and (... PLAYWRIGHT...)

The End2End and integration tests test the outer layer of the architecture. 
End2End tests focus on the UI and the user workflow.
The integration tests focus on the connection between the server and the client, testing HTTP endpoints and pagination. 
The unit tests focus on the domain and application layers, testing our enterprise business rules and application business rules.
(... PLAYWRIGHT...)


# Ethics

## License
We chose to use the MIT License for our application.

## LLMs, ChatGPT, CoPilot, and others
We made use of Chat-GPT.
We intentionally refrained from using it to write code.
We used it to establish a basic understanding of concepts, usually before reading official .NET documentation.  
When testing, we used it to interpret stack traces, which allowed us to locate bugs faster, and then squash them ourselves.

Both of these use cases allowed us to focus on the productive parts of the learning process, while avoiding mindless copy-pasting.