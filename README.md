# Flat Management Program

Todo: Description

## Technical Layout

The project consists of two main code bases: a backend (in WebApiBackend) and a frontend (in Frontend). The backend is a C# project providing a JSON API. The frontend is a ReactJS project that interacts with the API.

## Get Started!

To get the project running, follow these steps:

### Prerequisites
- Visual Studio (for editing C# code) [or the equivalent `dotnet` command line tools, or JetBrains Rider]
- Make sure you've installed the `ASP.NET and Web Development Tools package` in Visual Studio
- Visual Studio Code [or equivalent frontend tooling]
- NPM/NodeJS (for running the frontend)

### Steps

1. Open the backend folder (WebApiBackend) and find the Solution (.sln) file, and open in Visual Studio.
2. Once Visual Studio loads up, you should have a green "IIS Express" button to start the app in the top toolbar. Click it.
3. It'll take some time the first run (as Visual Studio should install all dependencies). Not much will appear to happen once the backend server is running.
4. Open the frontend folder in a command line tool.
5. Run `npm install` to download and install all dependencies.
6. Run `npm start` to start the React development server. 
7. Your browser should open (or you may have to open a link from the React command line tool). You should see the home page, and it should successfully connect to the backend. 