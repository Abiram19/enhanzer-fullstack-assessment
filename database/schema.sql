IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'EnhanzerAssessmentDb')
BEGIN
    CREATE DATABASE EnhanzerAssessmentDb;
END
GO

USE EnhanzerAssessmentDb;
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Location_Details' and xtype='U')
BEGIN
    CREATE TABLE Location_Details (
        Company_Code NVARCHAR(100) NOT NULL,
        Location_Code NVARCHAR(100) NOT NULL,
        Location_Name NVARCHAR(255) NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT PK_Location_Details PRIMARY KEY (Company_Code, Location_Code)
    );
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PurchaseBills' and xtype='U')
BEGIN
    CREATE TABLE PurchaseBills (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        CompanyCode NVARCHAR(100) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PurchaseBillItems' and xtype='U')
BEGIN
    CREATE TABLE PurchaseBillItems (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        PurchaseBillId INT NOT NULL,
        ItemName NVARCHAR(50) NOT NULL,
        Batch NVARCHAR(100) NULL,
        Qty INT NOT NULL,
        StandardCost DECIMAL(18,2) NOT NULL,
        StandardPrice DECIMAL(18,2) NOT NULL,
        Discount DECIMAL(5,2) NOT NULL,
        TotalCost DECIMAL(18,2) NOT NULL,
        TotalSelling DECIMAL(18,2) NOT NULL,
        CONSTRAINT FK_PurchaseBillItems_PurchaseBills FOREIGN KEY (PurchaseBillId) REFERENCES PurchaseBills(Id) ON DELETE CASCADE
    );
END
GO
