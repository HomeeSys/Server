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
CREATE TABLE [Devices] (
    [ID] int NOT NULL IDENTITY,
    [DeviceNumber] uniqueidentifier NOT NULL,
    [Spread] float NOT NULL,
    CONSTRAINT [PK_Devices] PRIMARY KEY ([ID]),
    CONSTRAINT [CK_Device_Spread_Range] CHECK (Spread >= 0 AND Spread <= 100)
);
DECLARE @defaultSchema AS sysname;
SET @defaultSchema = SCHEMA_NAME();
DECLARE @description AS sql_variant;
SET @description = N'This is unique name for device';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'Devices', 'COLUMN', N'DeviceNumber';
SET @description = N'Measurement value spread, expressed in percentage.';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'Devices', 'COLUMN', N'Spread';

CREATE TABLE [Locations] (
    [ID] int NOT NULL IDENTITY,
    [Name] nvarchar(450) NOT NULL,
    [Hash] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Locations] PRIMARY KEY ([ID])
);

CREATE TABLE [Measurements] (
    [ID] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Unit] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Measurements] PRIMARY KEY ([ID])
);
DECLARE @defaultSchema1 AS sysname;
SET @defaultSchema1 = SCHEMA_NAME();
DECLARE @description1 AS sql_variant;
SET @description1 = N'Name of measurement, for example ''Air Temperature''.';
EXEC sp_addextendedproperty 'MS_Description', @description1, 'SCHEMA', @defaultSchema1, 'TABLE', N'Measurements', 'COLUMN', N'Name';
SET @description1 = N'Unit';
EXEC sp_addextendedproperty 'MS_Description', @description1, 'SCHEMA', @defaultSchema1, 'TABLE', N'Measurements', 'COLUMN', N'Unit';

CREATE TABLE [ChartTemplates] (
    [ID] int NOT NULL IDENTITY,
    [MeasurementID] int NOT NULL,
    CONSTRAINT [PK_ChartTemplates] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_ChartTemplates_Measurements_MeasurementID] FOREIGN KEY ([MeasurementID]) REFERENCES [Measurements] ([ID]) ON DELETE CASCADE
);

CREATE TABLE [ChartOffsets] (
    [ID] int NOT NULL IDENTITY,
    [Time] int NOT NULL,
    [Value] float NOT NULL,
    [LocationID] int NOT NULL,
    [ChartTemplateID] int NOT NULL,
    CONSTRAINT [PK_ChartOffsets] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_ChartOffsets_ChartTemplates_ChartTemplateID] FOREIGN KEY ([ChartTemplateID]) REFERENCES [ChartTemplates] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_ChartOffsets_Locations_LocationID] FOREIGN KEY ([LocationID]) REFERENCES [Locations] ([ID]) ON DELETE CASCADE
);

CREATE TABLE [Samples] (
    [ID] int NOT NULL IDENTITY,
    [ChartTemplateID] int NOT NULL,
    [Time] time NOT NULL,
    [Value] float NOT NULL,
    CONSTRAINT [PK_Samples] PRIMARY KEY ([ID]),
    CONSTRAINT [FK_Samples_ChartTemplates_ChartTemplateID] FOREIGN KEY ([ChartTemplateID]) REFERENCES [ChartTemplates] ([ID]) ON DELETE CASCADE
);

CREATE INDEX [IX_ChartOffsets_ChartTemplateID] ON [ChartOffsets] ([ChartTemplateID]);

CREATE INDEX [IX_ChartOffsets_LocationID] ON [ChartOffsets] ([LocationID]);

CREATE INDEX [IX_ChartTemplates_MeasurementID] ON [ChartTemplates] ([MeasurementID]);

CREATE UNIQUE INDEX [IX_Devices_DeviceNumber] ON [Devices] ([DeviceNumber]);

CREATE UNIQUE INDEX [IX_Locations_Name] ON [Locations] ([Name]);

CREATE INDEX [IX_Samples_ChartTemplateID] ON [Samples] ([ChartTemplateID]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251208224330_#1-Initial', N'9.0.11');

COMMIT;
GO

