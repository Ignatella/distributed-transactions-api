create table [User]
(
    UserId       uniqueidentifier primary key DEFAULT NEWID(),
    FirstName    varchar(150)                                      not null,
    LastName     varchar(150)                                      not null,
    PhoneNumber  varchar(20)                                       not null,
    DepartmentId uniqueidentifier                                  not null,
    CreatedAt    datetime                     default getutcdate() not null,
);

create table [AccountType]
(
    AccountTypeId uniqueidentifier primary key default newid(),
    Type          varchar(100)                                      not null,
    CreatedAt     datetime                     default getutcdate() not null
)

create table [Account]
(
    AccountId     uniqueidentifier primary key default newid(),
    AccountTypeId uniqueidentifier                                  not null,
    AccountNumber varchar(16)                                       not null CHECK (LEN(AccountNumber) = 16),
    Balance       money                                             not null,
    CreatedAt     datetime                     default getutcdate() not null,
    CONSTRAINT FK_Account_AccountType FOREIGN KEY (AccountTypeId) REFERENCES AccountType (AccountTypeId),
)

create table [Transaction]
(
    TransactionId uniqueidentifier primary key default newid(),
    FromUserId    uniqueidentifier                                  not null,
    ToUserId      uniqueidentifier                                  not null,
    Amount        money                                             not null,
    Description   varchar(150),
    CreatedAt     datetime                     default getutcdate() not null,
);
