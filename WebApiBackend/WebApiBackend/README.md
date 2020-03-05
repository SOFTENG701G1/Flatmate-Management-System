# Backend
The C# backend provides a REST API for the frontend to consume. Its structure follows that of a standard C# project.

The project is configured by default with an SQLite Database using Entity Framework. 

Key folders are

- Controllers/ contains the controllers that directly service REST endpoints.
- Model/ contains source code forming the datamodel, and the database context code.
- Helpers/ contains useful helper classes.
- Services/ contains business logic code where necessary.
- Dto/ contains classes that are used as format to send/receive JSON from the controllers