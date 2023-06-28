create table Address
(
    AddressId   uniqueidentifier default newid() not null
        primary key,
    PostalCode  varchar(10)                      not null,
    City        varchar(100)                     not null,
    Street      varchar(100)                     not null,
    HouseNumber varchar(10)                      not null,
    FlatNumber  varchar(10),
    CreatedAt   datetime         default getutcdate()
)
go

create table LeafUser
(
    UserId       uniqueidentifier default newid()      not null
        primary key,
    PhoneNumber  varchar(20)                           not null,
    AddressId    uniqueidentifier                      not null
        constraint FK_User_Address
            references Address,
    CreatedAt    datetime         default getutcdate() not null,
    DepartmentId uniqueidentifier                      not null
)
go

create table [Transaction]
(
    TransactionId uniqueidentifier default newid()      not null
        primary key,
    FromAccountId uniqueidentifier,
    ToAccountId   uniqueidentifier,
    Amount        money                                 not null,
    Description   varchar(150),
    CreatedAt     datetime         default getutcdate() not null,
    constraint CH_AccountId_NOT_NULL
        check ([FromAccountId] IS NOT NULL OR [ToAccountId] IS NOT NULL)
)
go

CREATE   PROCEDURE uspGetUserAccounts(@userId uniqueidentifier)
AS
BEGIN
    set xact_abort on;
    BEGIN DISTRIBUTED Transaction;

    EXEC Central.bank.dbo.uspGetUserAccounts @userId = @userId;

    COMMIT TRANSACTION;
end
go

    CREATE   PROCEDURE uspGetUserTransactions(@userId uniqueidentifier)
    AS
    BEGIN
        set xact_abort on;
        BEGIN DISTRIBUTED Transaction;

        CREATE TABLE #UserAccounts
        (
            AccountId uniqueidentifier
        );

        INSERT INTO #UserAccounts EXEC Central.bank.dbo.uspGetUserAccountIds @userId = @userId;


        SELECT *
        FROM [Transaction]
        WHERE ToAccountId IN (select AccountId from #UserAccounts)
           OR FromAccountId IN (select AccountId from #UserAccounts)
        order by CreatedAt desc;


        COMMIT TRANSACTION;
    end;
go


CREATE PROCEDURE uspUserCreate(@userId uniqueidentifier, @phoneNumber varchar(20),
                                         @postalCode varchar(10),
                                         @city varchar(100), @street varchar(100), @houseNumber varchar(10),
                                         @flatNumber varchar(10))
AS
BEGIN
    set xact_abort on;
    BEGIN DISTRIBUTED Transaction;

    DECLARE @addressId uniqueidentifier;
    DECLARE @op TABLE
                (
                    AddressId uniqueidentifier
                )

    INSERT INTO Address (PostalCode, City, Street, HouseNumber, FlatNumber)
    OUTPUT inserted.AddressId INTO @op
    VALUES (@postalCode, @city, @street, @houseNumber, @flatNumber);

    SET @addressId = (SELECT TOP 1 AddressId FROM @op);

    INSERT INTO LeafUser (UserId, PhoneNumber, AddressId, DepartmentId)
    VALUES (@userId, @phoneNumber, @addressId, @@servername);

    EXEC Central.bank.dbo.uspUserCreate @userId = @userId, @departmentId = @@servername;

    COMMIT TRANSACTION;
end
go

