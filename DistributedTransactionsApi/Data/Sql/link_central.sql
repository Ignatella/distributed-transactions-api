USE master;
GO

EXEC sp_addlinkedserver
     @server = 'Central',
     @srvproduct = '',
     @provider = 'SQLNCLI',
     @datasrc = 'central',
     @location = '',
     @provstr = '',
     @catalog = 'Bank';
GO

EXEC sp_addlinkedsrvlogin @rmtsrvname = 'Central', @useself = 'True', @locallogin = NULL;
EXEC sp_serveroption 'Central', 'rpc', 'true';
EXEC sp_serveroption 'Central', 'rpc out', 'true';