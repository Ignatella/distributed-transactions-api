create database bank;
go;
use bank;

create table [Transaction]
(
    TransactionId int identity
        constraint Transaction_pk
            primary key,
    FromUserId    int                           not null,
    ToUserId      int                           not null,
    Amount        money                         not null,
    CreateAt      datetime default getutcdate() not null
)
go

create table [User]
(
    UserId    int identity
        constraint User_pk
            primary key,
    FirstName varchar(150)                  not null,
    LastName  varchar(150)                  not null,
    CreatedAt datetime default getutcdate() not null
)
