--AS Great job! 100%
use RecordKeeperDB
go
drop table if exists ExecutiveOrders 
go
drop table if exists president
go
drop table if exists Party
go

	create table dbo.party(
			PartyId int not null identity primary key,
--AS: Formatting tip: Each constraint should be indented on its own line when column has more than one constraint, the first constraint too.
			PartyName varchar(50) not null 
				constraint ck_President_Party_Name_should_not_be_blank check (PartyName <>'') 
				constraint u_Party_PartyName unique,
			YearStarted int not null 
				constraint ck_Year_Started_must_be_greater_than_zero check (Yearstarted >0),
			Color varchar(15) not null 
				constraint ck_Color_cannot_be_blank check (Color <>'')
	)
go 

		create table dbo.president(
			PresidentId int not null identity (1000,1) primary key,
			PartyId int not null 
				constraint f_Party_President foreign key references Party(PartyId) ,	
			Num int not null 
				constraint ck_President_Num_should_be_greater_than_zero check (Num > 0) constraint u_President_Num unique, 
			FirstName varchar(100) not null 
				constraint ck_President_First_Name_should_not_be_blank check (FirstName <>'') , 
			LastName varchar(100) not null 
				constraint ck_President_Last_Name_should_not_be_blank check (LastName <>'') , 
			DateBorn date not null,
			DateDied datetime2,
			YearBorn as datepart(year,DateBorn),
			YearDied as datepart(year,DateDied),
			TermStart int not null 
				constraint ck_President_Term_Start_can_not_be_before_1789 check (TermStart >= 1789),
			TermEnd int,
		constraint ck_President_Term_End_can_not_be_before_term_Start check (TermEnd >= TermStart),
		constraint ck_President_Must_be_35_years check (TermStart - datepart(year,DateBorn) >= 35)
	)
go

	alter table president drop column if exists AgeAtDeath
		alter table president add AgeAtDeath as datepart(year,DateDied) - datepart(year,DateBorn) persisted 
		
	alter table president drop column if exists YearsServed
		alter table president add YearsServed as TermEnd - Termstart persisted 
	
	 alter table president drop column if exists NumberOfTerms
 		alter table president add NumberOfTerms as (TermEnd - Termstart)/4 persisted 

go
alter table President drop column if exists Code
go
alter table President add Code as replace(concat(Num,'-',FirstName,'-',LastName),' ','-') persisted
go
		create table dbo.ExecutiveOrders(
			ExecutiveOrdersID int not null identity primary key ,
			PresidentId int not null 
				constraint f_ExecutiveOrders_President foreign key references president(PresidentId),
			OrderNumber int not null 
				constraint u_ExecutiveOrders_OrderNumber unique 
				constraint ck_Order_Number_must_be_greater_than_zero check(OrderNumber >0),
			PageNumber int not null 
				constraint ck_Page_Number_must_be_greater_than_zero check(PageNumber >0),
			YearIssued int not null 
				constraint ck_Year_issued_cannot_be_in_the_future check (YearIssued < Datepart(year,getdate())),
			OrderName varchar(1000) 
				constraint ck_Order_Name_cannot_be_blank check (OrderName<>''),
			ExecutiveOrdersDesc as concat('Exec. Order No. ',OrderNumber,', ','3 C.F.R.',' ',PageNumber,' ',YearIssued,'. ',OrderName) persisted,
			UpheldByCourt bit not null,
			DateRecorded date not null default getdate() 
		)
		go

