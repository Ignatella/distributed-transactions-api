create table Address
(
    AddressId   uniqueidentifier default newid() not null
        primary key,
    PostalCode  varchar(10)                      not null,
    City        varchar(100)                     not null,
    Street      varchar(100)                     not null,
    HouseNumber varchar(10)                      not null,
    FlatNumber  varchar(10)
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
    FromAccountId uniqueidentifier                      not null,
    ToAccountId   uniqueidentifier                      not null,
    Amount        money                                 not null,
    Description   varchar(150),
    CreatedAt     datetime         default getutcdate() not null
)
go

