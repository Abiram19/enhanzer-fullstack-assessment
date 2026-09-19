# Enhanzer Full Stack Developer Assessment

This project is a 2-page web application (Login & Purchase Bill Form) built with Angular (Frontend) and ASP.NET Core Web API (Backend) as per the Enhanzer assignment requirements.

## Technologies Used
*   **Frontend:** Angular 17+, TypeScript, Tailwind CSS
*   **Backend:** ASP.NET Core (.NET 8), C#, Entity Framework Core
*   **Database:** SQL Server
*   **Architecture:** Component-based architecture (Angular), REST API

## Prerequisites
*   Node.js and npm
*   .NET 8 SDK
*   SQL Server (LocalDB or SQLEXPRESS)

## Setup Instructions

### 1. Database Setup
1.  Open SQL Server Management Studio (SSMS) or Azure Data Studio.
2.  Connect to your local SQL Server instance (e.g., `.\SQLEXPRESS`).
3.  Execute the provided `database_script.sql` script located in the root directory. This will create the `EnhanzerAssessmentDb` database and the required tables (`Location_Details`, `PurchaseBills`, `PurchaseBillItems`).

### 2. Backend Setup (.NET Core API)
1.  Navigate to the `backend` folder:
    ```bash
    cd backend
    ```
2.  Restore the NuGet packages:
    ```bash
    dotnet restore
    ```
3.  Ensure the `DefaultConnection` string in `appsettings.json` points to your active SQL Server instance:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=.\\SQLEXPRESS;Database=EnhanzerAssessmentDb;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=False;TrustServerCertificate=True"
    }
    ```
4.  Run the application:
    ```bash
    dotnet run
    ```
    The backend API will start on `http://localhost:5260`.

### 3. Frontend Setup (Angular)
1.  Open a new terminal window and navigate to the `frontend` folder:
    ```bash
    cd frontend
    ```
2.  Install the dependencies:
    ```bash
    npm install
    ```
3.  Start the Angular development server:
    ```bash
    npm start
    ```
4.  The application will be available at `http://localhost:4200`.

## Features Implemented
*   **Login Page:** Authenticates via the external `POS_Api/Invoke` endpoint. Captures `User_Locations` on success and inserts them into `Location_Details`.
*   **Purchase Bill Form:** Protected by an Angular AuthGuard. Includes autocomplete for items, dynamic batch dropdowns loaded from the database, and real-time field calculations for gross total, discounts, tax, and net total.
*   **Backend Storage:** Successfully maps and submits complex Purchase Bill requests using nested objects into SQL Server.

## Important Note regarding Staging API
During testing, the external staging API endpoint occasionally returned `"Invalid Login Details"` despite correct credentials. This application strictly implements the error handling to proxy this 401 response to the UI. If you experience login issues during evaluation, please ensure the external endpoint is accepting the provided `Welcome#5` password. 
