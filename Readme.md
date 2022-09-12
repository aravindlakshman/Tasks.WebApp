# Tasks API
This API has been created using ASP.NET 6 web api and Sql server for storage. 
This code is deployed in Azure as per the specification.
Visual Studio 2022 was used for development.

##  Swagger endpoint in Cloud
### https://taskswebapi20220911203421.azurewebsites.net/swagger/index.html

## Functionalities exposed in Swagger
	1. POST /api/Tasks : To add a new task
	2. PUT  /api/tasks : To update a existing task
	3. GET  /api/Tasks : To retrieve all tasks from database


## Enhancements which can be done to the API
1. Localization - Place all the strings used in the application in resource files for English and Spanish, similiar to production API
2. ApplicationInsights - Complete rest of the steps to enable application insights telemetry.