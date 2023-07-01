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
    Amount        money,
    Description   varchar(150),
    CreatedAt     datetime         default getutcdate() not null,
    constraint CH_AccountId_NOT_NULL
        check ([FromAccountId] IS NOT NULL OR [ToAccountId] IS NOT NULL)
)
go

create    proc serviceRemoveUser(@userId uniqueidentifier)
as
begin

    set xact_abort on;

    begin distributed transaction
        DECLARE @addressId uniqueidentifier = (select u.AddressId from LeafUser u where UserId = @userId);
        delete from LeafUser where UserId = @userId;
        delete from Address where AddressId = @addressId;

        -- get all local user accounts
        CREATE TABLE #Accounts
        (
            AccountId uniqueidentifier
        );

        DECLARE @currentUserId uniqueidentifier;

        DECLARE userCursor CURSOR FOR SELECT userId from LeafUser;

        OPEN userCursor;

        FETCH NEXT FROM userCursor INTO @currentUserId;

        WHILE @@FETCH_STATUS = 0
            BEGIN
                -- Execute the remote procedure and insert the result into the temporary table

                INSERT INTO #Accounts EXEC Central.bank.dbo.uspGetUserAccountIds @userId = @currentUserId;

                -- Fetch the next row
                FETCH NEXT FROM userCursor INTO @currentUserId;
            END;

        CLOSE userCursor;
        DEALLOCATE userCursor;

        -- foreign withdraw
        delete
        from [Transaction]
        where FromAccountId is not null
          and FromAccountId not in (select AccountId from #Accounts)
          and ToAccountId is null;
        -- foreign deposit
        delete
        from [Transaction]
        where FromAccountId is null
          and ToAccountId is not null
          and ToAccountId not in (select AccountId from #Accounts);
        -- foreign transaction (from, to)
        delete
        from [Transaction]
        where FromAccountId is not null
          and ToAccountId is not null
          and FromAccountId not in (select AccountId from #Accounts)
          and ToAccountId not in (select AccountId from #Accounts);
        -- from this account
        update [Transaction] set FromAccountId = null where FromAccountId not in (select AccountId from #Accounts);
        -- to this account
        update [Transaction] set ToAccountId = null where ToAccountId not in (select AccountId from #Accounts);


    commit transaction
end


go


CREATE      PROCEDURE uspCreateTransaction(@initiatorUserId uniqueidentifier, @fromAccountNumber VARCHAR(26),
                                    @toAccountNumber VARCHAR(26),
                                    @amount money, @description varchar(150) = NULL)
AS
BEGIN
    set xact_abort on;
    BEGIN DISTRIBUTED Transaction;

    EXEC Central.bank.dbo.uspCreateTransaction @initiatorUserId = @initiatorUserId,
         @fromAccountNumber = @fromAccountNumber,
         @toAccountNumber = @toAccountNumber, @amount = @amount, @description = @description;

    COMMIT TRANSACTION;
end;
go


CREATE      PROCEDURE uspGetUserAccounts(@userId uniqueidentifier)
AS
BEGIN
    set xact_abort on;
    BEGIN DISTRIBUTED Transaction;

    EXEC Central.bank.dbo.uspGetUserAccounts @userId = @userId;

    COMMIT TRANSACTION;
end
go


CREATE     PROCEDURE uspGetUserTransactions(@userId uniqueidentifier)
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


CREATE       PROCEDURE uspSystemCreateTransaction(@fromAccountId uniqueidentifier, @toAccountId uniqueidentifier,
                                                 @amount money, @description varchar(150) = null)
AS
BEGIN
    INSERT INTO [Transaction] (FromAccountId, ToAccountId, Amount, Description)
    VALUES (@fromAccountId, @toAccountId, @amount, @description);
end;


go



CREATE   PROCEDURE uspUserCreate(@userId uniqueidentifier, @phoneNumber varchar(20),
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

