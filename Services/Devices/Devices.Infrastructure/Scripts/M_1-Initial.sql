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
    WHERE [MigrationId] = N'20251209170350_#1-Initial'
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
    SET @description = N'Name of location, for example: ''Kitchen''...';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'Locations', 'COLUMN', N'Name';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209170350_#1-Initial'
)
BEGIN
    CREATE TABLE [MeasurementTypes] (
        [ID] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Unit] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_MeasurementTypes] PRIMARY KEY ([ID])
    );
    DECLARE @defaultSchema1 AS sysname;
    SET @defaultSchema1 = SCHEMA_NAME();
    DECLARE @description1 AS sql_variant;
    SET @description1 = N'For example: ''Air Temperature''';
    EXEC sp_addextendedproperty 'MS_Description', @description1, 'SCHEMA', @defaultSchema1, 'TABLE', N'MeasurementTypes', 'COLUMN', N'Name';
    SET @description1 = N'For example: ''ppm''';
    EXEC sp_addextendedproperty 'MS_Description', @description1, 'SCHEMA', @defaultSchema1, 'TABLE', N'MeasurementTypes', 'COLUMN', N'Unit';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209170350_#1-Initial'
)
BEGIN
    CREATE TABLE [Statuses] (
        [ID] int NOT NULL IDENTITY,
        [Type] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Statuses] PRIMARY KEY ([ID])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209170350_#1-Initial'
)
BEGIN
    CREATE TABLE [Timestamps] (
        [ID] int NOT NULL IDENTITY,
        [Cron] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Timestamps] PRIMARY KEY ([ID])
    );
    DECLARE @defaultSchema2 AS sysname;
    SET @defaultSchema2 = SCHEMA_NAME();
    DECLARE @description2 AS sql_variant;
    SET @description2 = N'Timestamp measurement configuration stored in CRON format';
    EXEC sp_addextendedproperty 'MS_Description', @description2, 'SCHEMA', @defaultSchema2, 'TABLE', N'Timestamps', 'COLUMN', N'Cron';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209170350_#1-Initial'
)
BEGIN
    CREATE TABLE [Devices] (
        [ID] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [DeviceNumber] uniqueidentifier NOT NULL,
        [RegisterDate] datetime2 NOT NULL,
        [LocationID] int NOT NULL,
        [TimestampID] int NOT NULL,
        [StatusID] int NOT NULL,
        CONSTRAINT [PK_Devices] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_Devices_Locations_LocationID] FOREIGN KEY ([LocationID]) REFERENCES [Locations] ([ID]) ON DELETE CASCADE,
        CONSTRAINT [FK_Devices_Statuses_StatusID] FOREIGN KEY ([StatusID]) REFERENCES [Statuses] ([ID]) ON DELETE CASCADE,
        CONSTRAINT [FK_Devices_Timestamps_TimestampID] FOREIGN KEY ([TimestampID]) REFERENCES [Timestamps] ([ID]) ON DELETE CASCADE
    );
    DECLARE @defaultSchema3 AS sysname;
    SET @defaultSchema3 = SCHEMA_NAME();
    DECLARE @description3 AS sql_variant;
    SET @description3 = N'This is unique name for device';
    EXEC sp_addextendedproperty 'MS_Description', @description3, 'SCHEMA', @defaultSchema3, 'TABLE', N'Devices', 'COLUMN', N'DeviceNumber';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209170350_#1-Initial'
)
BEGIN
    CREATE TABLE [DeviceMeasurementTypes] (
        [ID] int NOT NULL IDENTITY,
        [DeviceID] int NOT NULL,
        [MeasurementTypeID] int NOT NULL,
        CONSTRAINT [PK_DeviceMeasurementTypes] PRIMARY KEY ([ID]),
        CONSTRAINT [FK_DeviceMeasurementTypes_Devices_DeviceID] FOREIGN KEY ([DeviceID]) REFERENCES [Devices] ([ID]) ON DELETE CASCADE,
        CONSTRAINT [FK_DeviceMeasurementTypes_MeasurementTypes_MeasurementTypeID] FOREIGN KEY ([MeasurementTypeID]) REFERENCES [MeasurementTypes] ([ID]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209170350_#1-Initial'
)
BEGIN
    CREATE INDEX [IX_DeviceMeasurementTypes_DeviceID] ON [DeviceMeasurementTypes] ([DeviceID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209170350_#1-Initial'
)
BEGIN
    CREATE INDEX [IX_DeviceMeasurementTypes_MeasurementTypeID] ON [DeviceMeasurementTypes] ([MeasurementTypeID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209170350_#1-Initial'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Devices_DeviceNumber] ON [Devices] ([DeviceNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209170350_#1-Initial'
)
BEGIN
    CREATE INDEX [IX_Devices_LocationID] ON [Devices] ([LocationID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209170350_#1-Initial'
)
BEGIN
    CREATE INDEX [IX_Devices_StatusID] ON [Devices] ([StatusID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209170350_#1-Initial'
)
BEGIN
    CREATE INDEX [IX_Devices_TimestampID] ON [Devices] ([TimestampID]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251209170350_#1-Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251209170350_#1-Initial', N'9.0.7');
END;

COMMIT;
GO

