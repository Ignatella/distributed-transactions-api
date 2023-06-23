USE master;
GO

CREATE PROCEDURE dbo.AddLinkedServer
    @server NVARCHAR(128),
    @datasrc NVARCHAR(4000)
AS
BEGIN
    EXEC sp_addlinkedserver
         @server = @server,
         @srvproduct = '',
         @provider = 'SQLNCLI',
         @datasrc = @datasrc,
         @location = '',
         @provstr = '',
         @catalog = 'Bank';

    EXEC sp_addlinkedsrvlogin @rmtsrvname = @server, @useself = 'True', @locallogin = NULL;
    EXEC sp_serveroption @server, 'rpc', 'true';
    EXEC sp_serveroption @server, 'rpc out', 'true';
END;
GO

DECLARE @i INT = 1;
WHILE @i <= 4
BEGIN
    DECLARE @serverName NVARCHAR(10) = 'Leaf_' + CAST(@i AS NVARCHAR(1));
    DECLARE @dataSource NVARCHAR(10) = 'leaf_' + CAST(@i AS NVARCHAR(1));

    EXEC dbo.AddLinkedServer @server = @serverName, @datasrc = @dataSource;

    SET @i = @i + 1;
END;


