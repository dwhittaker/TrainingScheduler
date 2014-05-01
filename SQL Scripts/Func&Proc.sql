--Recert Date Function
ALTER function [dbo].[fn_EmployeeRecert](
@ctype int)
returns Table
as
--This function will gather all recert information for employee's based on course type. 
--This function also records whether or not the employee ever took the class and if this class is part of the required classes they need to take.
--08/06/2012 DW: Right now this can only track if they have taken the class once we get the list of required trainings by job title 
Return(
select emp.emp_id,emp.First_Name,emp.Last_Name,(select distinct RecertDate from RecentRecert where emp_id = emp.emp_id and trainingcourseid = @ctype) as 'RecertDate',
(select top 1 EndDate from Exception where EMP_ID = emp.emp_id) as 'ExDate', 'TakenClass' =
case 
	when emp.emp_id in (select EMP_ID from EMPRegistration er join CourseInstance ci on ci.courseinstanceid = er.courseinstanceid where ci.TrainingCourseID = @ctype) then 'Y'
	else 'N'
end,
'N' as 'RequiredClass'
From v_Employees emp
where emp_id not in (select emp_id from EmpRegistration er join courseinstance ci on ci.CourseInstanceID = er.CourseInstanceID where status = 'R' and ci.TrainingCourseID = @ctype and ci.coursedate >= dateadd(mm,9,getdate()))
)
dateadd
--End Recert Date Function
select * from TrainingCourse

--Recert Date SP
alter procedure spEmpRecert(
@ctype int,
@cdate datetime)
as
if @ctype <> 306 
begin
select emp_id,First_Name,Last_Name,'RecertDate' =
Case
	when RecertDate > ExDate or ExDate is null Then RecertDate
	else ExDate
end 
 from fn_EmployeeRecert(@ctype)
where (recertdate between DATEADD(mm, DATEDIFF(m,0,@cdate),0) and DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,@cdate)+1,0))
and (exdate between DATEADD(mm, DATEDIFF(m,0,@cdate),0) and DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,@cdate)+1,0)) or exdate is null))
or (RecertDate is null and ExDate is null and TakenClass = 'N' and RequiredClass = 'Y')
Order by Last_Name
end
else
select emp_id,First_name,Last_Name,RecertDate
from fn_EmployeeRecert(0)
where recertdate is not null

--End Recert Date SP

--Pass Emp
ALTER Procedure [dbo].[PassEmp](
@empID nvarchar(10),
@ci int,
@status nvarchar(2),
@user varchar(30))
as
declare @recertdate as datetime
declare @rmonths as int
IF @status = 'R' 
Begin
	if (select months from coursetype where coursetypeid in (select coursetypeid from trainingcourse tc join courseinstance ci on ci.trainingcourseid = tc.trainingcourseid where ci.courseinstanceid = @ci)) <> 0
	begin
		set @rmonths = (select months from coursetype where coursetypeid in (select coursetypeid from trainingcourse tc join courseinstance ci on ci.trainingcourseid = tc.trainingcourseid where ci.courseinstanceid = @ci))
		set @recertdate = dateadd(month, @rmonths, Getdate())
	end
	else
		set @recertdate = Null
	Update EmpRegistration set CompletionDate = (select coursedate from courseinstance ci where ci.courseinstanceid = @ci), RecertDate = @recertdate, Status = 'A',ModifiedBy = @user,ModifiedDate = Getdate() where EMP_ID = @empID and CourseInstanceID = @ci
End
--

--Remove Attendance
create procedure RemoveAttendance(
@empID nvarchar(10),
@ci int,
@status nvarchar(2),
@user varchar(30))
as
IF @status = 'A'
Begin
	Update EmpRegistration set CompletionDate = Null, RecertDate = Null, Status = 'R',ModifiedBy = @user,ModifiedDate = Getdate() where EMP_ID = @empID and CourseInstanceID = @ci
End
--

select * from empregistration


--DeleteEMPRegistration
ALTER procedure [dbo].[DeleteEmpRegistration](
@empID nvarchar(10),
@ci int)
as
delete from EmpRegistration where EMP_ID = @empID and CourseInstanceID = @ci

if (select displayincalendar from courseinstance where courseinstanceid = @ci) = 0 
Begin
	exec DeleteClass @ci
end
--

--InsertEmpRegistration
ALTER procedure [dbo].[InsertEmpRegistration](
@empid nvarchar(10),
@ci int,
@user varchar(30))
as
If (select count(emp_ID) from empregistration where emp_id = @empid and courseinstanceID = @ci) = 0
begin
Insert into EmpRegistration(EMP_ID,CourseInstanceID,EnrollDate,Status,ModifiedBy,ModifiedDate) Values(@empid,@ci,Getdate(),'R',@user,GetDate())
end
--

--InsertException
ALTER procedure [dbo].[InsertException](
@empID nvarchar(10),
@tcid int,
@sdate as datetime,
@edate as datetime,
@reason as nvarchar(255))
as

Insert into Exception(EMP_ID,TrainingCourseID,StartDate,EndDate,Reason) values(@empID,@tcid,@sdate,@edate,@reason)

--

--DeleteException
ALTER procedure [dbo].[DeleteException](
@exID int)
as

delete Exception where ExceptionID = @exID
--

--UpdateClass
Alter procedure UpdateClass(
@cid int,
@cdate as datetime,
@ins as varchar(10),
@cdesc as varchar(300),
@stime as datetime,
@etime as datetime)
as
update CourseInstance set CourseDate = @cdate, CourseInstructorID = @ins, Description = @cdesc, CourseStartTime = @stime, CourseEndTime = @etime where courseinstanceid = @cid
--

select * from CourseCalendar order by CourseInstructorID

--DeleteClass
Create procedure DeleteClass(
@cid int)
as
delete from CourseInstance where courseinstanceid = @cid
--

--AddClass
Alter Procedure AddClass(
@tcid as int,
@cdate as datetime,
@stime as datetime,
@etime as datetime,
@locid as int,
@insid as varchar(10),
@desc as varchar(300),
@display as int,
@user varchar(30),
@cid int output)
as
Insert into CourseInstance(TrainingCourseID,Description,CourseDate,CourseStartTIme,CourseEndTime,CourseLocationID,CourseInstructorID,ModifiedBy,ModifiedDate,DisplayInCalendar)
values(@tcid,@desc,@cdate,@stime,@etime,@locid,@insid,@user,getdate(),@display)
set @cid = @@Identity
--

--AddClassAttrbiute
Create Procedure AddClassAttribute(
@cid as int,
@aid as int)
as
Insert into ClassAttributes(TrainingCourseID, AttributeID) Values(@cid,@aid)
--

--RemoveClassAttribute
Create Procedure RemoveClassAttribute(
@caid as int)
as
delete from ClassAttributes where ClassAttributeID = @caid
--


--Create Emp Registration Record
ALTER procedure [dbo].[CreateEmpRegistration](
@empid nvarchar(10),
@tcid as int,
@completiondate datetime,
@duration decimal(5,2),
@desc as varchar(300),
@insid as varchar(10),
@locid as int,
@comments varchar(50),
@user varchar(20))
as
declare @etime as datetime
declare @minutes as int
declare @courseinstanceid int
declare @recertdate datetime
declare @recertmonths int

if (select Months from coursetype where coursetypeid in (select coursetypeid from trainingcourse where trainingcourseid = @tcid)) <> 0
begin
	set @recertmonths = (select Months from coursetype where coursetypeid in (select coursetypeid from trainingcourse where trainingcourseid = @tcid))
	set @recertdate = dateadd(mm,@recertmonths,@completiondate)
end 
else 
	set @recertdate = Null
set @minutes = @duration * 60
set @etime = dateadd(mi,@minutes,'8:00AM')

exec AddClass @tcid,@completiondate,'8:00AM',@etime,@locid,@insid,@desc,0,@user,@cid = @courseinstanceid output

insert into EmpRegistration(emp_id, courseinstanceid, enrolldate, completiondate, recertdate, status, comments, modifiedby, modifieddate)
values(@empid, @courseinstanceid, GetDate(), @completiondate, @recertdate, 'A', @comments,@user, GetDate())
--

--Add instructor 
create procedure AddIns(
@Fname varchar(30),
@Lname varchar(30))
as
declare @insid varchar(10)
declare @ID int

select @ID= (select top 1 cast(left(courseinstructorid,Len(courseinstructorID) -1) as int) from CourseInstructor order by courseinstructorid desc)
set @insid = cast(@ID + 1 as Varchar(10)) + 'i'

insert into CourseInstructor(CourseInstructorID,FirstName,LastName) Values(@insid,@Fname,@Lname)
--

--Update instructor
Create procedure UpdateIns(
@insID varchar(10),
@Fname varchar(30),
@Lname varchar(30))
as
update CourseInstructor set FirstName = @Fname, LastName = @Lname where courseInstructorid = @insID
--

--Remove Instructor
Alter Procedure RemoveIns(
@insID varchar(10))
as
If (select count(CourseInstructorID) from CourseInstance where CourseInstructorID = @insID) = 0
begin
	delete CourseInstructor where CourseInstructorID = @insID
end
--

exec RemoveIns '4i'

exec AddIns 'Dan', 'Whittaker2'

select * from courseinstructor

select * from classattributes

select * from CourseInstance

select * from CourseLocation

select * from courseinstance

select * from EmpRegistration er join courseinstance ci on ci.CourseInstanceID = er.CourseInstanceID where er.EMP_ID = 6907509 and ci.TrainingCourseID = 140 --order by RecertDate desc

select * from Employee where last_name like 'Dolin%'



select DATEADD(mm, DATEDIFF(m,0,'08/29/2012'),0) '08/29/2012'

select * from courseinstance where trainingcourseid = 140 and coursedate = '10/28/2011'


--2421028

2421444

select * from empregistration where emp_id = 9692491 and courseINstanceid = 43213


update empregistration set recertdate = '10/31/2011' where empregistrationid = 196290

select EMP_ID from EMPRegistration er join CourseInstance ci on ci.courseinstanceid = er.courseinstanceid where ci.TrainingCourseID = 104
and emp_id = 2421028


select TrainingCourseID,CourseTitle,CourseDuration from trainingcourse


var id = 

select TrainingCourseID,CourseTitle + ': ' + cast(CourseDuration as varchar(4)) as 'Title' from trainingcourse


select TrainingCourseID, CourseTitle + ' (' + cast(CourseDuration as varchar) + ')' as ct from TrainingCourse order by CourseTitle