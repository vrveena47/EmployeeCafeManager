Everything Build in VS2022

Clone the repository and install dependencies:
# Clone repository
git clone https://github.com/vrveena47/EmployeeCafeManager.git



===========================================================================================
DB Setup
=============================================================================================

CREATE DATABASE CafeEmployeeDb;
GO
USE CafeEmployeeDb;
GO
# 1. TABLE: Cafes
-----------------------
CREATE TABLE Cafes (
    CafeId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(10) NOT NULL,
    Description NVARCHAR(256) NOT NULL,
    Logo VARBINARY(MAX) NULL,  -- Optional logo image
    Location NVARCHAR(100) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE()
);
GO

# 2. TABLE: Employees
-----------------------------
CREATE TABLE Employees (
    EmployeeId CHAR(9) NOT NULL PRIMARY KEY,  -- UIXXXXXXX
    Name NVARCHAR(10) NOT NULL,
    EmailAddress NVARCHAR(255) NOT NULL UNIQUE,
    PhoneNumber CHAR(8) NOT NULL CHECK (PhoneNumber LIKE '[89][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
    Gender NVARCHAR(10) NOT NULL CHECK (Gender IN ('Male', 'Female')),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE()
);
GO


# 3. TABLE: EmployeeCafe
------------------------------
CREATE TABLE EmployeeCafe (
    EmployeeId CHAR(9) NOT NULL PRIMARY KEY,  -- 1 employee = 1 cafe
    CafeId UNIQUEIDENTIFIER NOT NULL,
    StartDate DATE NOT NULL,
    CONSTRAINT FK_Employee FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId) ON DELETE CASCADE,
    CONSTRAINT FK_Cafe FOREIGN KEY (CafeId) REFERENCES Cafes(CafeId) ON DELETE CASCADE
);
GO


# 4. INDEXES FOR PERFORMANCE
----------------------------------
CREATE INDEX IX_Cafes_Location ON Cafes(Location);
CREATE INDEX IX_EmployeeCafe_CafeId ON EmployeeCafe(CafeId);
CREATE INDEX IX_EmployeeCafe_StartDate ON EmployeeCafe(StartDate);
GO


# 5. SEED DATA
--------------------------
-- Cafes
DECLARE @Cafe1 UNIQUEIDENTIFIER = NEWID();
DECLARE @Cafe2 UNIQUEIDENTIFIER = NEWID();
DECLARE @Cafe3 UNIQUEIDENTIFIER = NEWID();

INSERT INTO Cafes (CafeId, Name, Description, Location)
VALUES 
(@Cafe1, 'Wingstop', 'Neighborhood cafe', 'Orchard'),
(@Cafe2, 'KFC', 'Office espresso bar', 'Raffles Place'),
(@Cafe3, 'McDonalds', 'Cozy corner cafe', 'Holland Village');

# Employees
INSERT INTO Employees (EmployeeId, Name, EmailAddress, PhoneNumber, Gender)
VALUES
('UIA000001','Alice Tan','alice@example.com','91234567','Female'),
('UIB000002','Bob Lim','bob@example.com','81234567','Male'),
('UIC000003','Carol Ong','carol@example.com','91230000','Female');

# Assign Employees to Cafes
INSERT INTO EmployeeCafe (EmployeeId, CafeId, StartDate)
VALUES
('UIA000001', @Cafe1, '2024-01-15'),
('UIB000002', @Cafe2, '2023-12-01');
-- Note: Carol Ong not assigned to any cafe yet
GO





===========================================================================================
# Café Employee Manager – Frontend
===========================================================================================
This is the frontend application for the **Café Employee Manager** system, built with **React 18**, **Vite**, **Material-UI**, and **AG Grid**.  
It provides a simple interface to manage Cafés and Employees, consuming APIs exposed by the backend service.

---

## 🚀 Features
- **Cafe Management**
  - List cafes with Logo, Name, Description, Employees, Location
  - Filter cafes by location
  - Add, Edit, and Delete cafes
- **Employee Management**
  - List employees with Id, Name, Email, Phone, Days Worked, Café
  - Filter employees by café
  - Add, Edit, and Delete employees
- **Form Validations**
  - Name: 6–10 characters, alphabets only  
  - Email: Valid email format  
  - Phone: 8 digits, starts with 8 or 9  
- **UI**
  - Built with **Material-UI (MUI v5)**
  - Data tables powered by **AG Grid v34**

---

## 📦 Prerequisites
- **Node.js** >=18.x
- **npm** >=9.x

Ensure the **backend API** is running locally (default: https://localhost:7156).

---

## 🔧 Installation

# Install dependencies
npm install
npm run dev
npm run build


#Application Website
  http://localhost:56544/

======================================================================================
#Backend
======================================================================================
#This is the backend for the Cafe Employee Manager application, built using:


.NET 8 / ASP.NET Core Web API
Entity Framework Core (EF Core)
MediatR (for CQRS pattern)

Autofac (dependency injection)
SQL Server (LocalDB or full instance)
It manages Cafes, Employees, and their relationships, supporting CRUD operations with validation and proper business rules.

#Prerequisites
-------
.NET 8 SDK
SQL Server LocalDB


#Installation
----------

dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package MediatR --version 12.3.0
dotnet add package MediatR.Extensions.Autofac.DependencyInjection --version 12.1.0
dotnet add package Autofac
dotnet add package Autofac.Extensions.DependencyInjection

the API runs on: https://localhost:7156
