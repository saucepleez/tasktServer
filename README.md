## THIS PROJECT IS CURRENTLY UNDER DEVELOPMENT AND IS PRE-ALPHA

# tasktServer
tasktServer is the optional web companion and back-end orchestration tool for taskt, a free and open-source process automation (rpa) client.  You can find the taskt client project [here](https://github.com/saucepleez/taskt).

![server-image](https://i.imgur.com/nGE0KFI.png)

## Project Purpose
This project is the web companion and back-end orchestration/administration tool for taskt which allows you to link your taskt clients together as a robotic workforce. Create assignments that your robot workers will perform and view/manage task executions by reviewing the agenda.

## Technology
The web application runs on ASP.NET Core MVC v2.0, uses MS SQL Server to store logs and client data, and communicates with the robot workers through web sockets.

## Quick Start
Initial documentation on how to connect clients to the server [here](https://github.com/saucepleez/tasktServer/wiki/Connecting-Clients).

## License
This project is licensed under the Apache License - see the LICENSE.md file for details. This project is free for personal or commercial use.

## Functional Roadmap
- [X] Enable taskt Clients to Connect to Server 
- [ ] Create and Build Assignments (jobs that are assigned to workers)
- [ ] Populate Agenda with upcoming/in progress/completed assignments
