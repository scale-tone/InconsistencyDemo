USE master
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'InconsistencyDemo')
BEGIN
    ALTER DATABASE InconsistencyDemo set single_user with rollback immediate
    DROP DATABASE InconsistencyDemo
END
GO

CREATE DATABASE InconsistencyDemo
GO

USE InconsistencyDemo

CREATE TABLE [dbo].[LogData]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [TranCount] [int] NULL,
    [Xact] [int] NULL,
    [TranId] [bigint] NULL,
    [CorrelationId] [nvarchar](128) NULL,
    [InsertedAt] [datetime] NOT NULL
) ON [PRIMARY]

ALTER TABLE [dbo].[LogData] ADD  CONSTRAINT [DF_LogData_InsertedAt]  DEFAULT (getdate()) FOR [InsertedAt]

CREATE TABLE [dbo].[ImportantBusinessData]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Message] [nvarchar](MAX) NULL,
    [CorrelationId] [nvarchar](128) NULL,
    [InsertedAt] [datetime] NOT NULL
) ON [PRIMARY]

ALTER TABLE [dbo].[ImportantBusinessData] ADD  CONSTRAINT [DF_ImportantBusinessData_InsertedAt]  DEFAULT (getdate()) FOR [InsertedAt]
