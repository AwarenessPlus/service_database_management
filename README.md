# service_database_management
This API REST service made in .NET CORE manage the request for access to data in database. This service allows to make request from web app

The service exposes the followings endpoins: 

## Endpoints

## Medics Controller

###  Ping

 > GET /api/medics/ping

This endpoint allows the client proof if the service are working good.


###  Get Medic Data

 > GET /api/medics/{medicID}

This endpoint allow to the user to get his user informatión, except the password. 

### Modify Medic Data

 > PUT /api/medics/{medicID}

This endpoint allow to the user to modify his informatión, except his username and password.


## Patients Controller

###  Ping

 > GET /api/patients/ping

This endpoint allows the client proof if the service are working good.

### Get Patient Info
 > GET /api/patients/{documentID}

This endpoint allows the user to get the patient info from database using the documentID.

### Create New Patient
 > POST /api/patients/new-patient

This endpoint allows connect the service to the vital signs monitor

## Procedures Controller

###  Ping

 > GET /api/procedures/ping

This endpoint allows the client proof if the service are working good.

###  Get list of Procedures by medicID

  > GET /api/procedures/{medicId}

This endpoint allows the user to get the list of the procedures that the user has created

###  Create a new Procedure

  > POST /api/procedures/new-procedure

This endpoint allows the user to create a new procedure with procedure and patient info

###  Delete a Procedure

  > DELETE /api/procedures/{id}

This endpoint allows the user to delete a procedure from the database

###  upload a video for a Procedure

  > Post /api/procedures/upload-video/{id}

This endpoint allows the user to upload the video recorded from the procedure made by the HoloLens

###  get a video of a Procedure

  > GET /api/procedures/get-video/{id}

This endpoin allows the user to get the video previously uploaded for a procedure

# Installation 

## Enviroment
you need to downlowad or check the following things depending on your development environment

### Visual Studio
- Visual Studio 2019 with the ASP.NET and web development workload.


### Visual Studio Code

- Visual Studio Code
- C# for Visual Studio
- .NET 5.0 SDK

### Visual Studio for Mac

- For Visual Studio for Mac .NET 5.0.

## On visual studio 

### NuGet

You need install the following NuGets for work properly: 

** Microsoft.EntityFrameworkCore [from Microsoft](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/)
* System.IdentityModel.Tokens.JWT [from .NET](https://www.nuget.org/packages/System.IdentityModel.Tokens.Jwt/)
* Microsoft.AspNetCore.Authentication.JwtBearer [from ASP.NET Core](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer/)
* Swashbuckle.AspNetCore [from domaindrivendev ](https://www.nuget.org/packages/Swashbuckle.AspNetCore/5.6.3?_src=template)
* System.Drawing.Common [ from Microsoft ](https://www.nuget.org/packages/System.Drawing.Common/5.0.2?_src=template )


## For documentation 


The service  include Swagger which allows you to view and test each endpoint. [Swagger]( https://swagger.io/) 


