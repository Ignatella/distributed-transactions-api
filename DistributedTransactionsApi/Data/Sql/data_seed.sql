-- AccountType
INSERT INTO AccountType (AccountTypeId, Type)
VALUES ('D7786C58-E50A-43AE-B729-3B7A069F592C', N'Konto 360 Student'),
       ('49171251-97F8-4CF9-B6DC-845BC437E832', N'Konto oszczędzaniowe'),
       ('0DE8B625-1257-4F2B-A390-8FF963880A35', N'XBank 360');

exec usp_Run_Sql_On_Leaves 'delete from AccountType';
exec usp_Run_Sql_On_Leaves
     @sql = N'INSERT INTO AccountType (AccountTypeId, Type) VALUES (''D7786C58-E50A-43AE-B729-3B7A069F592C'', N''Konto 360 Student''), (''49171251-97F8-4CF9-B6DC-845BC437E832'', N''Konto oszczędzaniowe''), (''0DE8B625-1257-4F2B-A390-8FF963880A35'', N''XBank 360'')';


-- Department
-- ToDO