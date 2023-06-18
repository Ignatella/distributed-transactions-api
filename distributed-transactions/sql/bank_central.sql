create database bank;
go;
use bank;

create table Department
(
    DepartmentId   int identity
        constraint Department_pk
            primary key,
    DepartmentName varchar(50)                   not null,
    CreatedAt      datetime default getutcdate() not null
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
go

create table Balance
(
    BalanceId int identity
        constraint Balance_pk
            primary key,
    UserId    int
        constraint Balance_fk_User
            references [User],
    Balance   money,
    CreatedAt datetime default getutcdate() not null
)
go

create table UserDepartment
(
    UserDepartmentId int identity
        constraint UserDepartmentId_pk
            primary key,
    UserId           int
        constraint User_Department_fk_User
            references [User],
    DepartmentId     int
        constraint User_Department_fk_Department
            references Department,
    CreatedAt        datetime default getutcdate() not null
)
