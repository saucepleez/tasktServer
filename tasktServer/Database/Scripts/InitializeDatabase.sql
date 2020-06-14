IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Assignments] (
    [AssignmentID] uniqueidentifier NOT NULL,
    [Enabled] bit NOT NULL,
    [AssignmentName] nvarchar(max) NULL,
    [PublishedScriptID] uniqueidentifier NOT NULL,
    [AssignedWorker] uniqueidentifier NOT NULL,
    [Frequency] int NOT NULL,
    [Interval] int NOT NULL,
    [NewTaskDue] datetime2 NOT NULL,
    CONSTRAINT [PK_Assignments] PRIMARY KEY ([AssignmentID])
);

GO

CREATE TABLE [BotStore] (
    [StoreID] uniqueidentifier NOT NULL,
    [BotStoreName] nvarchar(max) NULL,
    [BotStoreValue] nvarchar(max) NULL,
    [LastUpdatedOn] datetime2 NOT NULL,
    [LastUpdatedBy] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_BotStore] PRIMARY KEY ([StoreID])
);

GO

CREATE TABLE [LoginProfiles] (
    [ID] uniqueidentifier NOT NULL,
    [Username] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [Password] nvarchar(max) NULL,
    [LastSuccessfulLogin] datetime2 NOT NULL,
    CONSTRAINT [PK_LoginProfiles] PRIMARY KEY ([ID])
);

GO

CREATE TABLE [PublishedScripts] (
    [PublishedScriptID] uniqueidentifier NOT NULL,
    [WorkerID] uniqueidentifier NOT NULL,
    [PublishedOn] datetime2 NOT NULL,
    [ScriptType] int NOT NULL,
    [FriendlyName] nvarchar(max) NULL,
    [ScriptData] nvarchar(max) NULL,
    CONSTRAINT [PK_PublishedScripts] PRIMARY KEY ([PublishedScriptID])
);

GO

CREATE TABLE [Tasks] (
    [TaskID] uniqueidentifier NOT NULL,
    [WorkerID] uniqueidentifier NOT NULL,
    [WorkerType] nvarchar(max) NULL,
    [MachineName] nvarchar(max) NULL,
    [UserName] nvarchar(max) NULL,
    [IPAddress] nvarchar(max) NULL,
    [ExecutionType] nvarchar(max) NULL,
    [Script] nvarchar(max) NULL,
    [Status] nvarchar(max) NULL,
    [Remark] nvarchar(max) NULL,
    [TaskStarted] datetime2 NOT NULL,
    [TaskFinished] datetime2 NOT NULL,
    [TotalSeconds] float NOT NULL,
    CONSTRAINT [PK_Tasks] PRIMARY KEY ([TaskID])
);

GO

CREATE TABLE [WorkerPools] (
    [WorkerPoolID] uniqueidentifier NOT NULL,
    [WorkerPoolName] nvarchar(max) NULL,
    CONSTRAINT [PK_WorkerPools] PRIMARY KEY ([WorkerPoolID])
);

GO

CREATE TABLE [Workers] (
    [WorkerID] uniqueidentifier NOT NULL,
    [MachineName] nvarchar(max) NULL,
    [UserName] nvarchar(max) NULL,
    [LastCheckIn] datetime2 NOT NULL,
    [Status] int NOT NULL,
    CONSTRAINT [PK_Workers] PRIMARY KEY ([WorkerID])
);

GO

CREATE TABLE [AssignedPoolWorker] (
    [AssignedPoolWorkerItemID] uniqueidentifier NOT NULL,
    [WorkerID] uniqueidentifier NOT NULL,
    [WorkerPoolID] uniqueidentifier NULL,
    CONSTRAINT [PK_AssignedPoolWorker] PRIMARY KEY ([AssignedPoolWorkerItemID]),
    CONSTRAINT [FK_AssignedPoolWorker_WorkerPools_WorkerPoolID] FOREIGN KEY ([WorkerPoolID]) REFERENCES [WorkerPools] ([WorkerPoolID]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_AssignedPoolWorker_WorkerPoolID] ON [AssignedPoolWorker] ([WorkerPoolID]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200614195951_initdb', N'3.1.5');

GO

