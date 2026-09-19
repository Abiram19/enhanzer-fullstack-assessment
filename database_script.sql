-- Enhanzer Full Stack Developer Assessment
-- Database Schema Script

CREATE DATABASE EnhanzerAssessmentDb;
GO

USE EnhanzerAssessmentDb;
GO

-- 1. Location_Details Table
CREATE TABLE [dbo].[Location_Details] (
    [Company_Code]  NVARCHAR (100) NOT NULL,
    [Location_Code] NVARCHAR (100) NOT NULL,
    [Location_Name] NVARCHAR (255) NOT NULL,
    [CreatedAt]     DATETIME2 (7)  DEFAULT (sysutcdatetime()) NOT NULL,
    CONSTRAINT [PK_Location_Details] PRIMARY KEY CLUSTERED ([Company_Code] ASC, [Location_Code] ASC)
);
GO

-- 2. PurchaseBills Table
CREATE TABLE [dbo].[PurchaseBills] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [CompanyCode] NVARCHAR (100) NULL,
    [CreatedAt]   DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_PurchaseBills] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

-- 3. PurchaseBillItems Table
CREATE TABLE [dbo].[PurchaseBillItems] (
    [Id]             INT             IDENTITY (1, 1) NOT NULL,
    [PurchaseBillId] INT             NOT NULL,
    [ItemCode]       NVARCHAR (100)  NOT NULL,
    [ItemName]       NVARCHAR (255)  NOT NULL,
    [Batch]          NVARCHAR (100)  NOT NULL,
    [StandardCost]   DECIMAL (18, 2) NOT NULL,
    [StandardPrice]  DECIMAL (18, 2) NOT NULL,
    [Margin]         DECIMAL (18, 2) NOT NULL,
    [Qty]            INT             NOT NULL,
    [FreeQty]        INT             NOT NULL,
    [Discount]       DECIMAL (18, 2) NOT NULL,
    [TotalCost]      DECIMAL (18, 2) NOT NULL,
    [TotalSelling]   DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_PurchaseBillItems] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PurchaseBillItems_PurchaseBills_PurchaseBillId] FOREIGN KEY ([PurchaseBillId]) REFERENCES [dbo].[PurchaseBills] ([Id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_PurchaseBillItems_PurchaseBillId]
    ON [dbo].[PurchaseBillItems]([PurchaseBillId] ASC);
GO
