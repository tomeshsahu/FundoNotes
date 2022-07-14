create database Fundonote     --creating Fundonote database
use Fundonote


--creating user table
create table Users( 
UserId int primary key identity,
Firstname varchar(50),
Lastname varchar(50),
Email varchar(50) unique,
Password varchar(255),
CreateDate datetime Default sysdatetime(),
MoidifyDate datetime Default getdate()   --sysdatetime and getdate will give same output 
)


insert into Users(Firstname,Lastname,Email,Password) values('Tomeshwar','Sahu','tomesh@gmail.com','Tomesh@123')

select * from Users


--Created Stored Procedure
create procedure spAddUser(
@Firstname varchar(50), 
@Lastname varchar(50),
@Email varchar(50),
@Password varchar(255)
)
As
Begin try
insert into Users(Firstname,Lastname,Email,Password) values(@Firstname,@Lastname,@Email,@Password)
end try
Begin catch
SELECT 
	ERROR_NUMBER() AS ErrorNumber,
	ERROR_STATE() AS ErrorState,
	ERROR_PROCEDURE() AS ErrorProcedure,
	ERROR_LINE() AS ErrorLine,
	ERROR_MESSAGE() AS ErrorMessage;
END CATCH


exec spAddUser 'Ramesh','kumar','ramesh@gmail.com','Ramesh@123'





alter procedure spLoginUser(
@Email varchar(50),
@Password varchar(255)
)
As
Begin try
select * from Users where Email=@Email and Password = @Password
end try
Begin catch
SELECT 
	ERROR_NUMBER() AS ErrorNumber,
	ERROR_STATE() AS ErrorState,
	ERROR_PROCEDURE() AS ErrorProcedure,
	ERROR_LINE() AS ErrorLine,
	ERROR_MESSAGE() AS ErrorMessage;
END CATCH

exec spLoginUser 'tomesh@gmail.com' ,'Tomesh@123'

exec spGetAllUser


--select * from userSignup
--truncate table userSignup
select * from Users




Create procedure spForgetPasswordUser(
@Email varchar(50)
)
As
Begin try
select * from Users where Email=@Email 
end try
Begin catch
SELECT 
	ERROR_NUMBER() AS ErrorNumber,
	ERROR_STATE() AS ErrorState,
	ERROR_PROCEDURE() AS ErrorProcedure,
	ERROR_LINE() AS ErrorLine,
	ERROR_MESSAGE() AS ErrorMessage;
END CATCH

exec spForgetPasswordUser 'tomesh@gmail.com'

drop procedure spForgetPasswordUser




Create procedure spResetPassword(
@Email varchar(50),
@Password varchar(255)
)
As
Begin try
update Users set Password=@Password where Email=@Email 
end try
Begin catch
SELECT 
	ERROR_NUMBER() AS ErrorNumber,
	ERROR_STATE() AS ErrorState,
	ERROR_PROCEDURE() AS ErrorProcedure,
	ERROR_LINE() AS ErrorLine,
	ERROR_MESSAGE() AS ErrorMessage;
END CATCH

exec spResetPassword 'tomesh@gmail.com','Tomesh@125'
select * from Users




----Notes
-----Add Node------
create table Note(
NoteId int identity(1,1) primary key,
Title varchar(20) not null,
Description varchar(max) not null,
Bgcolor varchar(50) not null,
IsPin bit,
IsArchive bit,
IsRemainder bit,
IsTrash bit,
UserId int not null foreign key references Users(UserId),
RegisteredDate datetime default GETDATE(),
Remainder datetime,
ModifiedDate datetime null
)

---Add Note Store Procedure-----

Alter procedure spAddNote(
@Title varchar(20), 
@Description varchar(max),
@BgColor varchar(50),
@UserId int
)
As
Begin try
insert into Note(Title,Description,Bgcolor,UserId,IsPin,IsArchive,IsRemainder,IsTrash,ModifiedDate) values(@Title,@Description,@BgColor,@UserId,0,0,0,0,GetDate())
end try
Begin catch
SELECT 
	ERROR_NUMBER() AS ErrorNumber,
	ERROR_STATE() AS ErrorState,
	ERROR_PROCEDURE() AS ErrorProcedure,
	ERROR_LINE() AS ErrorLine,
	ERROR_MESSAGE() AS ErrorMessage;
END CATCH

truncate table Note

exec spAddNote 'aaaa','bbbc','cccc',2;


select * from Note

select * from Users




--- Get Note----

alter procedure spGetNote(@UserId int)
As
Begin try
select * from Note where UserId = @UserId and IsTrash=0
end try
Begin catch
SELECT 
	ERROR_NUMBER() AS ErrorNumber,
	ERROR_STATE() AS ErrorState,
	ERROR_PROCEDURE() AS ErrorProcedure,
	ERROR_LINE() AS ErrorLine,
	ERROR_MESSAGE() AS ErrorMessage;
END CATCH


 select * from Note
 exec spGetNote





 --------Update Notes----------

alter procedure spUpdateNote(
@Title varchar(20), 
@Description varchar(max),
@BgColor varchar(50),
@UserId int,
@NoteId int,
@IsPin bit,
@IsArchive bit,
@IsTrash bit
)
As
Begin try
Update Note set Title=@Title, Description=@Description,BgColor=@BgColor,UserId=@UserId,IsPin=@IsPin,IsArchive=@IsArchive,IsTrash=@IsTrash,ModifiedDate=GetDate() where UserId=@UserId and NoteId=@NoteId
end try
Begin catch
SELECT 
	ERROR_NUMBER() AS ErrorNumber,
	ERROR_STATE() AS ErrorState,
	ERROR_PROCEDURE() AS ErrorProcedure,
	ERROR_LINE() AS ErrorLine,
	ERROR_MESSAGE() AS ErrorMessage;
END CATCH