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

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='DelayedOrders' and xtype='U')
BEGIN
    CREATE TABLE DelayedOrders (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        OrderNo NVARCHAR(100) NOT NULL,
        Product NVARCHAR(255) NOT NULL,
        DueDate NVARCHAR(50) NOT NULL,
        DaysLate INT NOT NULL,
        Urgency NVARCHAR(50) NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='BankAccounts' and xtype='U')
BEGIN
    CREATE TABLE BankAccounts (
        Id NVARCHAR(100) PRIMARY KEY,
        Name NVARCHAR(255) NOT NULL,
        Source NVARCHAR(255) NOT NULL,
        UpdatedTime NVARCHAR(100) NOT NULL,
        Reviewed BIT NULL,
        Currency NVARCHAR(10) NOT NULL,
        Balance DECIMAL(18,2) NOT NULL,
        FormattedBalance NVARCHAR(50) NOT NULL,
        IsNegative BIT NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ExpenseItems' and xtype='U')
BEGIN
    CREATE TABLE ExpenseItems (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Code NVARCHAR(50) NOT NULL,
        Name NVARCHAR(255) NOT NULL,
        Amount DECIMAL(18,2) NOT NULL,
        FormattedAmount NVARCHAR(50) NOT NULL,
        Color NVARCHAR(50) NOT NULL,
        Percentage INT NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT * FROM DelayedOrders)
BEGIN
    INSERT INTO DelayedOrders (OrderNo, Product, DueDate, DaysLate, Urgency) VALUES
    ('ORD-2024-001', 'Steel Frame Assembly', 'Feb 15, 2024', 5, 'medium'),
    ('ORD-2024-002', 'Hydraulic Pump', 'Feb 10, 2024', 10, 'high'),
    ('ORD-2024-003', 'Control Panel', 'Feb 18, 2024', 2, 'low'),
    ('ORD-2024-004', 'Motor Assembly', 'Feb 12, 2024', 8, 'medium'),
    ('ORD-2024-005', 'Bearing Kit', 'Feb 16, 2024', 4, 'medium');
END
GO

IF NOT EXISTS (SELECT * FROM BankAccounts)
BEGIN
    INSERT INTO BankAccounts (Id, Name, Source, UpdatedTime, Reviewed, Currency, Balance, FormattedBalance, IsNegative) VALUES
    ('1', 'Bank', 'In QuickBooks', 'Updated 1176 days ago', 1, 'AED', -65919.02, 'AED-65,919.02', 1),
    ('2', '123.1234 PB_test', 'In QuickBooks', 'Updated 300 days ago', NULL, 'AED', 4567.00, 'AED4,567.00', NULL),
    ('3', 'Cash and cash equivalents', 'In QuickBooks', 'Updated 5 days ago', 1, 'AED', 125798.19, 'AED125,798.19', NULL),
    ('4', 'Test Account', 'In QuickBooks', 'Updated 100 days ago', NULL, '€', 200.00, '€200.00', NULL),
    ('5', 'Master card 0011', 'In QuickBooks', 'Updated 50 days ago', NULL, 'AED', 1507.05, 'AED1,507.05', NULL);
END
GO

IF NOT EXISTS (SELECT * FROM ExpenseItems)
BEGIN
    INSERT INTO ExpenseItems (Code, Name, Amount, FormattedAmount, Color, Percentage) VALUES
    ('6010', 'Online Marketing', 10000.00, '$10,000.00', '#00a0dc', 52),
    ('6020', 'Subscriptions', 6000.00, '$6,000.00', '#007bbd', 31),
    ('6090', 'Depreciation', 1100.00, '$1,100.00', '#133e5c', 6),
    ('9090', 'Custom', 2000.00, '$2,000.00', '#26c268', 11);
END
GO
