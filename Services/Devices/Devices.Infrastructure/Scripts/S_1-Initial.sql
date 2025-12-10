-- Clear database and reseed tables.
delete from [Locations];
delete from [Devices];
delete from [Timestamps];
delete from [MeasurementTypes];
delete from [Statuses];
delete from [DeviceMeasurementTypes];

-- Create 'Locations' seed with explicit Hash values
INSERT INTO Locations (Name, Hash)
VALUES
('Kitchen',       '138DFE8E-F8C6-462D-A768-0B7910BA61B0'),
('Living Room',   '4E4D3F6C-88E0-49B4-A1EC-6F56820F3E2B'),
('Bedroom',       'B7A3E5AF-0CE9-4A77-9BB4-2F8B35F707C9'),
('Bathroom',      '9D9B7487-3180-46D8-BB63-9A3E9EEE31C2'),
('Garage',        '5C1A06C8-0C78-4C56-9A27-6ADB0D59C2C1'),
('Dining Room',   '6B8E51E9-B20D-4FBE-8F52-2F1E187C3E2F'),
('Basement',      'E03A04F0-8DBF-41E8-9C01-A07F9245DA19'),
('Attic',         '0A9EA1ED-6C2C-4AC8-9D7F-83EDE7A45C67'),
('Laundry Room',  '81B3176C-01C3-4C2B-8E3E-4E4C95514637'),
('Office',        'F8B22A79-4127-48D7-9279-90D6CAC5300E'),
('Pantry',        'D2CF6F18-9FEB-49CA-B0C8-DF4F777269B7'),
('Storage Room',  '3E5F3B79-1C7E-4AC8-8AF0-53266F9073D7'),
('Guest Room',    'B6E15A99-0A22-4512-A604-6D5B5C553F27');

DECLARE @loc1 INT,  @loc2 INT,  @loc3 INT,  @loc4 INT,  @loc5 INT,
        @loc6 INT,  @loc7 INT,  @loc8 INT,  @loc9 INT,  @loc10 INT,
        @loc11 INT, @loc12 INT, @loc13 INT;

SELECT
    @loc1  = MAX(CASE WHEN Name = 'Kitchen'       THEN ID END),
    @loc2  = MAX(CASE WHEN Name = 'Living Room'   THEN ID END),
    @loc3  = MAX(CASE WHEN Name = 'Bedroom'       THEN ID END),
    @loc4  = MAX(CASE WHEN Name = 'Bathroom'      THEN ID END),
    @loc5  = MAX(CASE WHEN Name = 'Garage'        THEN ID END),
    @loc6  = MAX(CASE WHEN Name = 'Dining Room'   THEN ID END),
    @loc7  = MAX(CASE WHEN Name = 'Basement'      THEN ID END),
    @loc8  = MAX(CASE WHEN Name = 'Attic'         THEN ID END),
    @loc9  = MAX(CASE WHEN Name = 'Laundry Room'  THEN ID END),
    @loc10 = MAX(CASE WHEN Name = 'Office'        THEN ID END),
    @loc11 = MAX(CASE WHEN Name = 'Pantry'        THEN ID END),
    @loc12 = MAX(CASE WHEN Name = 'Storage Room'  THEN ID END),
    @loc13 = MAX(CASE WHEN Name = 'Guest Room'    THEN ID END)
FROM Locations;


--	Create 'MeasurementTypes' seed
insert into MeasurementTypes (Name, Unit)
values
('Air Temperature', '°C'),
('Relative Humidity', '%'),
('Carbon Dioxide', 'ppm'),
('Volatile Organic Compounds', 'ppb'),
('Particulate Matter 1um', 'µg/m³'),
('Particulate Matter 2.5um', 'µg/m³'),
('Particulate Matter 10um', 'µg/m³'),
('Formaldehyde', 'ppb'),
('Carbon Monoxide', 'ppm'),
('Ozone', 'ppb'),
('Ammonia', 'ppm'),
('Air Flow Rate', 'm³/h'),
('Air Ionization Level', 'ions/cm³'),
('Oxygen Concentration', '%'),
('Radon Concentration', 'Bq/m³'),
('Illuminance level', 'lux'),
('Sound Pressure Level', 'dB');

--	Create 'Statuses' seed
insert into Statuses (Type)
values
('Offline'),
('Online'),
('Disabled');

--  ID for 'Offline' status
DECLARE @statusOfflineID INT;
SET @statusOfflineID = (
    SELECT ID FROM Statuses WHERE Type = 'Offline'
);

--	Create 'Timestamps' seed
insert into Timestamps (Cron)
values
('0/1 * * * * ?'),		--	1 second
('0/5 * * * * ?'),		--	5 seconds
('0/15 * * * * ?'),		--	15 seconds
('0/30 * * * * ?'),		--	30 seconds
('0 0/1 * * * ?'),		--	1 minute
('0 0/5 * * * ?'),		--	5 minutes
('0 0/15 * * * ?'),		--	15 minutes
('0 0/30 * * * ?'),		--	30 minutes
('0 0 0/1 * * ?');		--	1 hour

--  ID for '30 seconds' timestamp
DECLARE @timestampHalfMinuteID INT;
SET @timestampHalfMinuteID = (
    SELECT ID FROM Timestamps WHERE Cron = '0/1 * * * * ?'
);

-- Create 'Devices' seed
INSERT INTO Devices (Name, DeviceNumber, LocationID, TimestampID, StatusID, RegisterDate)
VALUES
('KP1',  '8070047F-F866-408F-BF2F-8822EB0F5E76', @loc1,  @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP2',  '7580D3CC-D573-4CCC-AE84-553AEB249E3D', @loc2,  @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP3',  '81D84312-F412-41DF-826F-14CD1D2A7ECF', @loc3,  @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP4',  '152D4C7C-1BB3-4149-BBD5-0F82E8A00650', @loc4,  @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP5',  'C1E34A36-CE40-4D66-82EB-ED5DBCC6B8CC', @loc5,  @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP6',  '34AD0AA3-2395-43E6-8876-4A84C2A89186', @loc6,  @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP7',  '3841C988-E084-4255-A1CC-D8729C641FB3', @loc7,  @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP8',  '21091E66-6208-4DE8-A02E-673A26C5622B', @loc8,  @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP9',  '6017D722-3C94-4A7A-AC2B-BB8AB663AC59', @loc9,  @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP10', '0D504773-20B2-4919-A6BA-31A42C318D40', @loc10, @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP11', 'CDDFB749-B0FD-40B6-A3A5-4F74300AF25E', @loc11, @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP12', 'F5709C82-6799-4E9C-B1B6-F14E3A47820B', @loc12, @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP13', 'F26CEB49-F48F-4B64-9446-D9A03E024547', @loc13, @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP14', '6B2353E6-3792-4068-BE05-FD0E085AC964', @loc1,  @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP15', '28AFF0DE-80DA-41F8-8160-5CBD813B1C36', @loc2,  @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP16', '45CA78C9-24A3-4BC0-A5A5-1BBD57AA9ADC', @loc3,  @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP17', 'F78F9DD6-1F07-47C2-8F34-B55AD460FF09', @loc4,  @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP18', '9A021DF5-9D34-4735-AC9E-1C864C9DA55C', @loc5,  @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP19', '782A6BA4-229F-4161-B566-99027DD02861', @loc6,  @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP20', 'B5A71533-6019-4B74-AF50-EB8B4FDA00D1', @loc7,  @timestampHalfMinuteID, @statusOfflineID, GETDATE()),
('KP21', 'BDBE0C6C-A297-46EF-9AB6-41AB574FACC7', @loc8,  @timestampHalfMinuteID, @statusOfflineID, GETDATE());


-- Variables for filling 'DeviceMeasurementTypes'
DECLARE @devicesCount INT;
DECLARE @measurementTypesCount INT;
DECLARE @devicesCounter INT;
DECLARE @measurementTypesCounter INT;
DECLARE @minDevicesID INT;
DECLARE @minMeasurementTypesID INT;

--  Count should be equal to size of table
SELECT @devicesCount = COUNT(*) FROM Devices;
SELECT @measurementTypesCount = COUNT(*) FROM MeasurementTypes;
--  Counter should start with smalles ID from table
SELECT @minDevicesID = MIN(ID) FROM Devices;
SELECT @minMeasurementTypesID = MIN(ID) FROM MeasurementTypes;

--  We have to add min ID value to count becuase counter is min ID based!!!!
SET @devicesCount = @devicesCount + @minDevicesID;
SET @measurementTypesCount = @measurementTypesCount + @minMeasurementTypesID;

SET @devicesCounter = @minDevicesID;

WHILE @devicesCounter < @devicesCount
BEGIN
    SET @measurementTypesCounter = @minMeasurementTypesID;

    WHILE @measurementTypesCounter < @measurementTypesCount
    BEGIN
        INSERT INTO DeviceMeasurementTypes (DeviceID, MeasurementTypeID) 
        VALUES (@devicesCounter, @measurementTypesCounter)

        SET @measurementTypesCounter = @measurementTypesCounter + 1;
    END

    SET @devicesCounter = @devicesCounter + 1;
END