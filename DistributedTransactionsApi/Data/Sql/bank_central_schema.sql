create table AccountType
(
    AccountTypeId uniqueidentifier default newid()      not null
        primary key,
    Type          varchar(100)                          not null,
    CreatedAt     datetime         default getutcdate() not null
)
go

create table Account
(
    AccountId     uniqueidentifier default newid()      not null
        primary key,
    AccountTypeId uniqueidentifier                      not null
        constraint FK_Account_AccountType
            references AccountType,
    AccountNumber varchar(16)                           not null
        check (len([AccountNumber]) = 16),
    Balance       money                                 not null,
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

create table UserAccount
(
    UserId    uniqueidentifier not null
        constraint FK_UserAccount_User
            references MasterUser,
    AccountId uniqueidentifier not null
        constraint FK_UserAccount_Account
            references Account,
    primary key (UserId, AccountId)
)
go


CREATE PROCEDURE usp_Run_Sql_On_Leaves @sql nvarchar(MAX)
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

