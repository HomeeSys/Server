IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE TABLE [Locations] (
        [ID] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Hash] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Locations] PRIMARY KEY ([ID])
    );
    DECLARE @defaultSchema AS sysname;
    SET @defaultSchema = SCHEMA_NAME();
    DECLARE @description AS sql_variant;
    SET @description = N'For example: ''Kitchen'', ''Attic'' etc...';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'Locations', 'COLUMN', N'Name';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE TABLE [Measurements] (
        [ID] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Unit] nvarchar(max) NOT NULL,
        [MinChartYValue] int NOT NULL,
        [MaxChartYValue] int NOT NULL,
        CONSTRAINT [PK_Measurements] PRIMARY KEY ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE TABLE [Periods] (
        [ID] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [TimeFrame] time NOT NULL DEFAULT '00:00:00',
        [MaxAcceptableMissingTimeFrame] int NOT NULL DEFAULT 1,
        CONSTRAINT [PK_Periods] PRIMARY KEY ([ID]),
        CONSTRAINT [CK_Period_MaxAcceptableMissingTimeFrame_Range] CHECK (MaxAcceptableMissingTimeFrame >= 1 AND MaxAcceptableMissingTimeFrame <= 100)
    );
    DECLARE @defaultSchema1 AS sysname;
    SET @defaultSchema1 = SCHEMA_NAME();
    DECLARE @description1 AS sql_variant;
    SET @description1 = N'For example: ''Daily'', ''Weekly'', ''Monthly'', etc...';
    EXEC sp_addextendedproperty 'MS_Description', @description1, 'SCHEMA', @defaultSchema1, 'TABLE', N'Periods', 'COLUMN', N'Name';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE TABLE [Statuses] (
        [ID] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Statuses] PRIMARY KEY ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE TABLE [Raports] (
        [ID] int NOT NULL IDENTITY,
        [RaportCreationDate] datetime2 NOT NULL,
        [RaportCompletedDate] datetime2 NOT NULL,
        [StartDate] datetime2 NOT NULL,
        [EndDate] datetime2 NOT NULL,
        [Message] nvarchar(max) NOT NULL,
        [DocumentHash] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
        [PeriodID] int NOT NULL,
        [StatusID] int NOT NULL,
        CONSTRAINT [PK_Raports] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_Raports_Periods_PeriodID] FOREIGN KEY ([PeriodID]) REFERENCES [Periods] ([ID]) ON DELETE CASCADE,
        CONSTRAINT [FK_Raports_Statuses_StatusID] FOREIGN KEY ([StatusID]) REFERENCES [Statuses] ([ID]) ON DELETE CASCADE
    );
    DECLARE @defaultSchema2 AS sysname;
    SET @defaultSchema2 = SCHEMA_NAME();
    DECLARE @description2 AS sql_variant;
    SET @description2 = N'Date of Raport creation';
    EXEC sp_addextendedproperty 'MS_Description', @description2, 'SCHEMA', @defaultSchema2, 'TABLE', N'Raports', 'COLUMN', N'RaportCreationDate';
    SET @description2 = N'Date of Raport completion';
    EXEC sp_addextendedproperty 'MS_Description', @description2, 'SCHEMA', @defaultSchema2, 'TABLE', N'Raports', 'COLUMN', N'RaportCompletedDate';
    SET @description2 = N'Date of first measurement';
    EXEC sp_addextendedproperty 'MS_Description', @description2, 'SCHEMA', @defaultSchema2, 'TABLE', N'Raports', 'COLUMN', N'StartDate';
    SET @description2 = N'Date of last measurement';
    EXEC sp_addextendedproperty 'MS_Description', @description2, 'SCHEMA', @defaultSchema2, 'TABLE', N'Raports', 'COLUMN', N'EndDate';
    SET @description2 = N'Hash that allows to identify PDF in Azure Blob Storage';
    EXEC sp_addextendedproperty 'MS_Description', @description2, 'SCHEMA', @defaultSchema2, 'TABLE', N'Raports', 'COLUMN', N'DocumentHash';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE TABLE [MeasurementGroups] (
        [ID] int NOT NULL IDENTITY,
        [Summary] nvarchar(max) NOT NULL,
        [RaportID] int NOT NULL,
        [MeasurementID] int NOT NULL,
        CONSTRAINT [PK_MeasurementGroups] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_MeasurementGroups_Measurements_MeasurementID] FOREIGN KEY ([MeasurementID]) REFERENCES [Measurements] ([ID]) ON DELETE CASCADE,
        CONSTRAINT [FK_MeasurementGroups_Raports_RaportID] FOREIGN KEY ([RaportID]) REFERENCES [Raports] ([ID]) ON DELETE CASCADE
    );
    DECLARE @defaultSchema3 AS sysname;
    SET @defaultSchema3 = SCHEMA_NAME();
    DECLARE @description3 AS sql_variant;
    SET @description3 = N'Combined summary for all location groups';
    EXEC sp_addextendedproperty 'MS_Description', @description3, 'SCHEMA', @defaultSchema3, 'TABLE', N'MeasurementGroups', 'COLUMN', N'Summary';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE TABLE [RequestedLocations] (
        [ID] int NOT NULL IDENTITY,
        [LocationID] int NOT NULL,
        [RaportID] int NOT NULL,
        CONSTRAINT [PK_RequestedLocations] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_RequestedLocations_Locations_LocationID] FOREIGN KEY ([LocationID]) REFERENCES [Locations] ([ID]) ON DELETE CASCADE,
        CONSTRAINT [FK_RequestedLocations_Raports_RaportID] FOREIGN KEY ([RaportID]) REFERENCES [Raports] ([ID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE TABLE [RequestedMeasurements] (
        [ID] int NOT NULL IDENTITY,
        [MeasurementID] int NOT NULL,
        [RaportID] int NOT NULL,
        CONSTRAINT [PK_RequestedMeasurements] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_RequestedMeasurements_Measurements_MeasurementID] FOREIGN KEY ([MeasurementID]) REFERENCES [Measurements] ([ID]) ON DELETE CASCADE,
        CONSTRAINT [FK_RequestedMeasurements_Raports_RaportID] FOREIGN KEY ([RaportID]) REFERENCES [Raports] ([ID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE TABLE [LocationGroups] (
        [ID] int NOT NULL IDENTITY,
        [Summary] nvarchar(max) NOT NULL,
        [LocationID] int NOT NULL,
        [MeasurementGroupID] int NOT NULL,
        CONSTRAINT [PK_LocationGroups] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_LocationGroups_Locations_LocationID] FOREIGN KEY ([LocationID]) REFERENCES [Locations] ([ID]) ON DELETE CASCADE,
        CONSTRAINT [FK_LocationGroups_MeasurementGroups_MeasurementGroupID] FOREIGN KEY ([MeasurementGroupID]) REFERENCES [MeasurementGroups] ([ID]) ON DELETE CASCADE
    );
    DECLARE @defaultSchema4 AS sysname;
    SET @defaultSchema4 = SCHEMA_NAME();
    DECLARE @description4 AS sql_variant;
    SET @description4 = N'Verbal summary of data stored for this location';
    EXEC sp_addextendedproperty 'MS_Description', @description4, 'SCHEMA', @defaultSchema4, 'TABLE', N'LocationGroups', 'COLUMN', N'Summary';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE TABLE [SampleGroups] (
        [ID] int NOT NULL IDENTITY,
        [Date] datetime2 NOT NULL,
        [Value] float NOT NULL,
        [LocationGroupID] int NOT NULL,
        CONSTRAINT [PK_SampleGroups] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_SampleGroups_LocationGroups_LocationGroupID] FOREIGN KEY ([LocationGroupID]) REFERENCES [LocationGroups] ([ID]) ON DELETE CASCADE
    );
    DECLARE @defaultSchema5 AS sysname;
    SET @defaultSchema5 = SCHEMA_NAME();
    DECLARE @description5 AS sql_variant;
    SET @description5 = N'Time of measurement';
    EXEC sp_addextendedproperty 'MS_Description', @description5, 'SCHEMA', @defaultSchema5, 'TABLE', N'SampleGroups', 'COLUMN', N'Date';
    SET @description5 = N'Value  of measurement';
    EXEC sp_addextendedproperty 'MS_Description', @description5, 'SCHEMA', @defaultSchema5, 'TABLE', N'SampleGroups', 'COLUMN', N'Value';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE INDEX [IX_LocationGroups_LocationID] ON [LocationGroups] ([LocationID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE INDEX [IX_LocationGroups_MeasurementGroupID] ON [LocationGroups] ([MeasurementGroupID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE INDEX [IX_MeasurementGroups_MeasurementID] ON [MeasurementGroups] ([MeasurementID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE INDEX [IX_MeasurementGroups_RaportID] ON [MeasurementGroups] ([RaportID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE INDEX [IX_Raports_PeriodID] ON [Raports] ([PeriodID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE INDEX [IX_Raports_StatusID] ON [Raports] ([StatusID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE INDEX [IX_RequestedLocations_LocationID] ON [RequestedLocations] ([LocationID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE INDEX [IX_RequestedLocations_RaportID] ON [RequestedLocations] ([RaportID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE INDEX [IX_RequestedMeasurements_MeasurementID] ON [RequestedMeasurements] ([MeasurementID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE INDEX [IX_RequestedMeasurements_RaportID] ON [RequestedMeasurements] ([RaportID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    CREATE INDEX [IX_SampleGroups_LocationGroupID] ON [SampleGroups] ([LocationGroupID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251208222701_#1-Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251208222701_#1-Initial', N'9.0.10');
END;

COMMIT;
GO

