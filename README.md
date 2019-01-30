:warning: PLEASE NOTE THIS PROJECT IS ALPHA AND IS NOT YET SECURED OR INTENDED FOR PRODUCTION

# tasktServer
tasktServer is the optional web component and orchestrator for [taskt](https://github.com/saucepleez/taskt), a free and open-source process automation (rpa) client.

![taskt server photo](https://i.imgur.com/dFIEMwy.png)
## What does this project do?
The intended goal of this project is to track and manage taskt workers by providing a central point for reporting and administration.

## Technology stack 
This project uses React.JS for the front-end, .NET Core for the back-end API, and Entity Framework Core for interfacing with SQL Server database.  The taskt workers communicate with the back-end API URLs.

## How do I connect a bot?
Launch taskt and select the 'Settings' screen from the action bar. Once opened, select the 'Server' tab and add the URL of your tasktServer instance to the associated textbox and select 'Test Connection'.

![taskt settings](https://i.imgur.com/JrL6yoS.png)

After successfully testing the connection for the first time, taskt will request and store a GUID from the server.  The server will create a new entry in the worker list and begin accepting updates from the client.
