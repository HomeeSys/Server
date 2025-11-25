-- Clear database and reseed tables.
delete from [Raports];
delete from [MeasurementGroups];
delete from [LocationGroups];
delete from [SampleGroups];
delete from [RequestedLocations];
delete from [RequestedMeasurements];
delete from [Measurements];
delete from [Locations];
delete from [Periods];
delete from [Statuses];

--	Create 'Locations' seed
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

--	Create 'Statuses' seed
INSERT INTO Statuses (Name, Description)
VALUES
('Suspended', 'Waiting for user interaction'),
('Pending', 'This raport is waiting for its computation'),
('Processing', 'This raport is being computated'),
('Completed', 'Raport was created successfully'),
('Failed', 'Failed to generate raport'),
('Deleted Room', 'This raport was deleted');

--	Create 'Periods' seed
INSERT INTO Periods (Name, TimeFrame, MaxAcceptableMissingTimeFrame)
VALUES
('Hourly',  '00:01:00.0000000', 5),
('Daily',   '01:00:00.0000000', 5),
('Weekly',  '23:59:59.9999999', 5),
('Monthly', '23:59:59.9999999', 5);

-- Create 'Measurements' seed
INSERT INTO Measurements (Name, Unit, MinChartYValue, MaxChartYValue)
VALUES
('Air Temperature', '°C', 10, 40),
('Relative Humidity', '%', 20, 70),
('Carbon Dioxide', 'ppm', 400, 2000),
('Volatile Organic Compounds', 'ppb', 0, 500),
('Particulate Matter 1um', 'µg/m³', 0, 150),
('Particulate Matter 2.5um', 'µg/m³', 0, 150),
('Particulate Matter 10um', 'µg/m³', 0, 200),
('Formaldehyde', 'ppb', 0, 100),
('Carbon Monoxide', 'ppm', 0, 20),
('Ozone', 'ppb', 0, 100),
('Ammonia', 'ppm', 0, 5),
('Air Flow Rate', 'm³/h', 0, 1500),
('Air Ionization Level', 'ions/cm³', 0, 50000),
('Oxygen Concentration', '%', 15, 25),
('Radon Concentration', 'Bq/m³', 0, 200),
('Illuminance level', 'lux', 0, 2000),
('Sound Pressure Level', 'dB', 20, 80);
