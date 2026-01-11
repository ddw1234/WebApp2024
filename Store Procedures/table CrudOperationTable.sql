create table CrudOperationTable 
(
Id int Identity(1,1) Primary key,
UserName Varchar(200) NULL,
Age int
)


Insert into CrudOperationTable ( UserName, Age) values ( 'abc', 21)
Select * from CrudOperationTable