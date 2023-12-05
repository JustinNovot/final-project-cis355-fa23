# CIS 355 Final Project README File Installation

### Setting Up the Database
Before you are able to start on your project there are a few steps and installations that you must accomplish. 

### Prerequisites
- Install VS Code
- Install Docker 

### Initial Setup 
- Open up the VS Code and connect to the devcontainer. 

### Database Configuration
1. **Restore Dotnet Tools**: 
   Run `dotnet tools restore` in the terminal to install the Entity Framework (EF) CLI, necessary for applying EF migrations.

2. **Apply Database Migrations**: 
   Use `dotnet ef update` to apply all EF migrations and create the database if it doesn't exist.

### Running the Application
After setting up the database, start the application using `dotnet run` or through your IDE. It will automatically create a default admin account.

### Overview
Users will be provided with a default admin account that is created automatically upon development of the database. Immediate access to administrative features will be provided immediately.  

### Account Details
- **Username**: `admin`
- **Email**: `admin@admin.com`
- **Password**: `password`
- **Role**: `Admin`

### Usage
A user can use this account to sign in and access administrative areas. These features and settings can create an ideal setting for setups and tests. 

### Troubleshooting
If you can't access the application with the admin account, check if the database has been seeded correctly and look for any startup errors in the application logs.
