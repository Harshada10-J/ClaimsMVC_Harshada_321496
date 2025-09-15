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




# Application workflow

<img width="400" height="500" alt="image" src="https://github.com/user-attachments/assets/0288019d-2fb4-4648-8e06-c672f03b1182" />


The Application starts with the log in page





<img width="400" height="500" alt="image" src="https://github.com/user-attachments/assets/d1cd6280-c697-46a8-8a3d-02a03ca48416" />



New User have to register on clicking register


In database there is CompanyEmployees table which has the employee number and email in that so only existing employees can register
<img width="400" height="500" alt="image" src="https://github.com/user-attachments/assets/a02a3493-e9ee-4254-bcff-9f83b2e8c062" />


This image shows error when a non existing employee tries to register


After Registering you will lead to the Employee Dashboard

# Employee Dashboard
<img width="3510" height="1846" alt="image" src="https://github.com/user-attachments/assets/e730fae4-ef77-43f0-9736-766104164bc9" />


**Submit claims(Claims/Submit) page**
<img width="3510" height="1846" alt="image" src="https://github.com/user-attachments/assets/33750117-4d47-407a-ae6c-a1fccb73c96f" />

Submit button leads to history page


**History page(CLaims/HIstory)**
<img width="3510" height="1846" alt="image" src="https://github.com/user-attachments/assets/c5b35967-ddd9-421e-91f8-33bc0c7675f6" />


When Clicked on view(view icon in history page) leads to Details(Details/ClaimID) page

**Details page(/Claims/Details/7005)**
<img width="3510" height="2326" alt="image" src="https://github.com/user-attachments/assets/9eeab2cd-44ef-47d0-8a49-da2de759ac6a" />

After submitting a claim the new claim is added in home page which lists 5 recent claims submitted

**Updated home page**
<img width="3510" height="1884" alt="image" src="https://github.com/user-attachments/assets/be3f7d2e-37dd-408b-a9ea-ad4c3ddeafb7" />

# CPD Dashboard

CPD has only login not registration activity keeping in mind they will already be given access by the company to the portal

**CPD dashboard**
<img width="3510" height="1846" alt="image" src="https://github.com/user-attachments/assets/5a97d579-f336-434a-bc63-ac338b932e88" />

**Claims hIstory page**
<img width="3776" height="2001" alt="image" src="https://github.com/user-attachments/assets/66820850-580c-4bf3-89b0-7e3c70648311" />


**Audit log page**

When clicked on view audit log it is directed to the /Audit page which contains log of activities like login logout submitted,approved and rejected

<img width="3775" height="2001" alt="image" src="https://github.com/user-attachments/assets/97ee09dc-5d82-4f5b-9e2f-5d80e5a4f633" />

**Details Page**

When clicked on Review in pending claims of home page or view icon of a claim in the claims history page it directs to details/Id page

Pending will have approve or reject buttons

<img width="3765" height="2008" alt="image" src="https://github.com/user-attachments/assets/64f96c45-5d80-4185-a35b-1cd8779379bf" />

**Approve page**

When clicked on Approve that leads to Approve page Here we show the claim limits that is available for that employee and remaining claim ammount that can be reimbursed for the employee

<img width="3510" height="1846" alt="image" src="https://github.com/user-attachments/assets/67e15720-88c0-4b44-a3e3-1fcb772947fa" />

Cancel redirects back to the details page

**Reject page**

Clicked on reject directs to reject page where the cpd can reject and also a reason for rejection is given

<img width="3510" height="1846" alt="image" src="https://github.com/user-attachments/assets/cbdacbbc-a65e-4658-a827-8794c60abc58" />

After either Approving or rejecting the status is updated to approved 

<img width="3152" height="1355" alt="image" src="https://github.com/user-attachments/assets/fbc70c91-536e-4e65-87a6-4dda33a4a8ea" />


If the claim is approved mail is sent to Employee

<img width="1722" height="605" alt="image" src="https://github.com/user-attachments/assets/1fb49356-c5e2-4671-973c-b146299959c2" />















