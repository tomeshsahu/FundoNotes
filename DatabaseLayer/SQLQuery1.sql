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

exec spForgetPasswordUser ''

