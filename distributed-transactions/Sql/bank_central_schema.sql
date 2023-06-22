-- tables
create table [Department]
(
    DepartmentId uniqueidentifier primary key default newid(),
    Code         varchar(50)                                       not null,
    DbCode       varchar(50)                                       not null,
    Name         varchar(150)                                      not null,
    CreatedAt    datetime                     default getutcdate() not null,
);
go;

create table [User]
(
    UserId       uniqueidentifier primary key DEFAULT NEWID(),
    FirstName    varchar(150)                                      not null,
    LastName     varchar(150)                                      not null,
    PhoneNumber  varchar(20)                                       not null,
    DepartmentId uniqueidentifier                                  not null,
    CreatedAt    datetime                     default getutcdate() not null,
    CONSTRAINT FK_User_Department FOREIGN KEY (DepartmentId) REFERENCES Department (DepartmentId),
);
go;

create table [AccountType]
(
    AccountTypeId uniqueidentifier primary key default newid(),
    Type          varchar(100)                                      not null,
    CreatedAt     datetime                     default getutcdate() not null
)
go;

create table [Account]
(
    AccountId     uniqueidentifier primary key default newid(),
    AccountTypeId uniqueidentifier                                  not null,
    AccountNumber varchar(16)                                       not null CHECK (LEN(AccountNumber) = 16),
    Balance       money                                             not null,
    CreatedAt     datetime                     default getutcdate() not null,
    CONSTRAINT FK_Account_AccountType FOREIGN KEY (AccountTypeId) REFERENCES AccountType (AccountTypeId),
)
go;

create table [Transaction]
(
    TransactionId uniqueidentifier primary key default newid(),
    FromUserId    uniqueidentifier                                  not null,
    ToUserId      uniqueidentifier                                  not null,
    Amount        money                                             not null,
    Description   varchar(150),
    CreatedAt     datetime                     default getutcdate() not null,

    CONSTRAINT FK_Transaction_User_From FOREIGN KEY (FromUserId) REFERENCES [User] (UserId),
    CONSTRAINT FK_Transaction_User_To FOREIGN KEY (ToUserId) REFERENCES [User] (UserId),
);
go;

-- procedures
CREATE OR ALTER PROCEDURE usp_Run_Sql_On_Leaves @sql nvarchar(MAX)
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

