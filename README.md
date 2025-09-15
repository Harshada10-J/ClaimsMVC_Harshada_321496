**Web Based Claims Processing System (MVC)**
This is a complete, end-to-end web application built with ASP.NET Core MVC to automate the process of medical expense reimbursement for a company. 
Secure User Authentication: A full registration and login system built with ASP.NET Core Identity.
Role-Based Access Control: The application distinguishes between two main roles:
Employee: Can register, submit new claims, and view their personal claim history.
CPD (Claims Processing Department): Can review all claims, approve or reject them.
Master Employee Validation: New user registrations are validated against a pre-existing master list of company employees, ensuring only authorized personnel can create an account.
Dynamic Claims Workflow:
Employees can submit claims with multiple line items.
CPD staff can view a filterable history of all claims.
The approval process includes validation against designation-based claim limits.
Professional Frontend: A server-side rendered user interface built with Razor Views and styled professionally with Bootstrap.
Technology Stack
Backend Framework: ASP.NET Core MVC (.NET 8)
Language: C#
Database: Microsoft SQL Server
ORM: Entity Framework Core 8 (using a Database-First approach)
Security: ASP.NET Core Identity (Cookie Authentication)
Frontend: Razor Views (.cshtml), Bootstrap 5
Email Service: SendGrid integration for notifications.

Setup and Running the Application
To clone and run this application on your local machine, please follow these steps.
Prerequisites
.NET 8 SDK
Microsoft SQL Server (e.g., SQL Server Express LocalDB)
A SendGrid account and API key (for email notifications).
Instructions
Clone the Repository
Open your terminal and run the following command:
git clone [https://github.com/your-username/ClaimsMVC.git](https://github.com/your-username/ClaimsMVC.git)
cd ClaimsMVC


Database Setup
Ensure your SQL Server instance is running.
The application is configured to connect to a database named claims_processing. You must run the SQL scripts provided in the project to create the necessary tables (Users, Roles, Claims, etc.).
(This is built using database first approach)
Update the connection string in the appsettings.json file to point to your local SQL Server instance.
Configure User Secrets
This application uses User Secrets to securely store the SendGrid API key. From your terminal in the project's root folder, run the following commands:
# Initialize the user secrets for the project
dotnet user-secrets init

# Set your SendGrid API key
dotnet user-secrets set "SendGridApiKey" "YOUR_SECRET_SENDGRID_API_KEY"


Restore Packages and Build
Run the following commands to ensure all dependencies are downloaded and the project is compiled.
dotnet restore
dotnet build


Run the Application
The best way to run the application for development is to use the https launch profile.

**dotnet run --launch-profile https**

