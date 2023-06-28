create table AccountType
(
    AccountTypeId uniqueidentifier default newid()      not null
        primary key,
    Type          varchar(100)                          not null,
    CreatedAt     datetime         default getutcdate() not null
)
go

create table Department
(
    DepartmentId uniqueidentifier default newid()      not null
        primary key,
    Code         varchar(50)                           not null,
    Name         varchar(150)                          not null,
    CreatedAt    datetime         default getutcdate() not null
)
go

create table MasterUser
(
    UserId       uniqueidentifier default newid()      not null
        primary key,
    DepartmentId uniqueidentifier                      not null
        constraint FK_User_Department
            references Department,
    CreatedAt    datetime         default getutcdate() not null
)
go

create table Account
(
    AccountId     uniqueidentifier default newid()      not null
        primary key,
    AccountTypeId uniqueidentifier                      not null
        constraint FK_Account_AccountType
            references AccountType,
    AccountNumber varchar(26)                           not null
        check (len([AccountNumber]) > 1),
    Balance       money                                 not null,
    CreatedAt     datetime         default getutcdate() not null,
    UserId        uniqueidentifier                      not null
        constraint FK_Account_User
            references MasterUser
            on delete cascade
)
go

CREATE   PROCEDURE dbo.uspGenerateAccountNumber
    @accountNumber varchar(26) OUTPUT
AS
BEGIN
    DECLARE @datePart VARCHAR(12) = (SELECT REPLACE(REPLACE(REPLACE(CONVERT(VARCHAR(14), GETUTCDATE(), 120), '-', ''), ':', ''), ' ', ''));
    DECLARE @randomPart VARCHAR(16) = (SELECT STR(FLOOR(RAND() * (10000000000000000 - 1000000000000000)  + 1000000000000000), 16));

    SET @accountNumber = @datePart + @randomPart;
END;
go

create proc uspGetUserAccountIds(@userId uniqueidentifier)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT AccountId FROM Account WHERE UserId = @userId;
end
go

CREATE   PROCEDURE uspGetUserAccounts(@userId uniqueidentifier)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT A.UserId,
           A.AccountId,
           A.AccountTypeId,
           A.AccountNumber,
           A.Balance,
           A.CreatedAt,
           T.AccountTypeId,
           T.Type,
           T.CreatedAt
    FROM Account A
             JOIN AccountType T on A.AccountTypeId = T.AccountTypeId
    where UserId = @userId;
end
go


CREATE   PROCEDURE usp_Run_Sql_On_Leaves @sql nvarchar(MAX)
AS
BEGIN
    BEGIN DISTRIBUTED TRANSACTION;

    -- Get the list of linked servers starting with a specific name
    DECLARE @LinkedServerNamePrefix NVARCHAR(100) = 'Leaf_';
    DECLARE @LinkedServerName NVARCHAR(100);
    DECLARE @DynamicSQL NVARCHAR(MAX);

    DECLARE LinkedServerCursor CURSOR FOR
        SELECT name
        FROM sys.servers
        WHERE is_linked = 1
          AND name LIKE @LinkedServerNamePrefix + '%';

    OPEN LinkedServerCursor;

    FETCH NEXT FROM LinkedServerCursor INTO @LinkedServerName;

    WHILE @@FETCH_STATUS = 0
        BEGIN

            print @LinkedServerName;
            -- Execute the command on each linked server
            SET @DynamicSQL = 'EXECUTE(''' + REPLACE(@sql, '''', '''''') + ''') AT ' + @LinkedServerName + ';';
            EXEC sp_executesql @DynamicSQL;

            FETCH NEXT FROM LinkedServerCursor INTO @LinkedServerName;
        END;

    CLOSE LinkedServerCursor;
    DEALLOCATE LinkedServerCursor;

    -- Commit the distributed transaction
    COMMIT TRANSACTION;
END;
go



CREATE     PROCEDURE uspUserCreate(@userId uniqueidentifier, @departmentId uniqueidentifier)
AS
BEGIN
    insert into MasterUser (userid, departmentid) values (@userId, @departmentId);

    DECLARE @accMain VARCHAR(26);
    DECLARE @accSav VARCHAR(26);

    EXEC dbo.uspGenerateAccountNumber @accountNumber = @accMain OUTPUT;
    EXEC dbo.uspGenerateAccountNumber @accountNumber = @accSav OUTPUT;

    insert into Account (UserId, AccountTypeId, AccountNumber, Balance)
    values (@userId, '49171251-97F8-4CF9-B6DC-845BC437E832', @accMain, 0),
           (@userId, 'D7786C58-E50A-43AE-B729-3B7A069F592C', @accSav, 0);

END;
go

