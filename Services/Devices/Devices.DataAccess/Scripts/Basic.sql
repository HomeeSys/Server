Select d.DeviceNumber as 'Device Number', l.Name as 'Location' from Devices as d left join Locations as l on d.LocationID = l.ID;

-- Select all with timestamp config and location.
select d.DeviceNumber, d.RegisteredDate, d.Description, l.Name, tc.CRON from devices as d
left join locations as l 
on d.LocationID = l.ID
left join TimestampConfigurations as tc
on d.TimestampConfigurationID = tc.ID;