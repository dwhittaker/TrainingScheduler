--Recert Date Function
alter function [dbo].[fn_EmployeeRecert](
@ctype int)
returns Table
as
--This function will gather all recert information for employee's based on course type. 
--This function also records whether or not the employee ever took the class and if this class is part of the required classes they need to take.
--08/06/2012 DW: Right now this can only track if they have taken the class once we get the list of required trainings by job title 
Return(
select emp.emp_id,emp.First_Name,emp.Last_Name,(select top 1 RecertDate from EmpRegistration er join courseinstance ci on ci.CourseInstanceID = er.CourseInstanceID where er.EMP_ID = emp.emp_id and ci.TrainingCourseID = @ctype order by RecertDate desc) as 'RecertDate',
(select top 1 EndDate from Exception where EMP_ID = emp.emp_id and TrainingCourseID = @ctype) as 'ExDate', 'TakenClass' =
case 
	when emp.emp_id in (select EMP_ID from EMPRegistration er join CourseInstance ci on ci.courseinstanceid = er.courseinstanceid where ci.TrainingCourseID = @ctype) then 'Y'
	else 'N'
end,
'N' as 'RequiredClass'
From Employee emp
where emp_id not in (select emp_id from EmpRegistration er join courseinstance ci on ci.CourseInstanceID = er.CourseInstanceID where status = 'R' and ci.TrainingCourseID = @ctype)
)
--End Recert Date Function

--Recert Date SP
create procedure spEmpRecert(
@ctype int,
@cdate datetime)
as
select emp_id,First_Name,Last_Name,'RecertDate' =
Case
	when RecertDate > ExDate or ExDate is null Then RecertDate
	else ExDate
end 
 from fn_EmployeeRecert(@ctype)
where (recertdate between DATEADD(mm, DATEDIFF(m,0,@cdate),0) and DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,@cdate)+1,0))
and (exdate between DATEADD(mm, DATEDIFF(m,0,@cdate),0) and DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,@cdate)+1,0)) or exdate is null))
or (RecertDate is null and ExDate is null and TakenClass = 'N' and RequiredClass = 'Y')

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
	set @rmonths = (select months from coursetype where coursetypeid in (select coursetypeid from trainingcourse tc join courseinstance ci on ci.trainingcourseid = tc.trainingcourseid where ci.courseinstanceid = @ci))
	set @recertdate = dateadd(month, @rmonths, Getdate())
	Update EmpRegistration set CompletionDate = GetDate(), RecertDate = @recertdate, Status = 'A',ModifiedBy = @user,ModifiedDate = Getdate() where EMP_ID = @empID and CourseInstanceID = @ci
End
--

--DeleteEMPRegistration
ALTER procedure [dbo].[DeleteEmpRegistration](
@empID nvarchar(10),
@ci int)
as
delete from EmpRegistration where EMP_ID = @empID and CourseInstanceID = @ci
--

--InsertEmpRegistration
ALTER procedure [dbo].[InsertEmpRegistration](
@empid nvarchar(10),
@ci int,
@user varchar(30))
as
Insert into EmpRegistration(EMP_ID,CourseInstanceID,EnrollDate,Status,ModifiedBy,ModifiedDate) Values(@empid,@ci,Getdate(),'R',@user,GetDate())
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
Create procedure UpdateClass(
@cid int,
@cdate as datetime,
@stime as datetime,
@etime as datetime)
as
update CourseInstance set CourseDate = @cdate, CourseStartTime = @stime, CourseEndTime = @etime where courseinstanceid = @cid
--

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
@insid as int,
@desc as varchar(300),
@user varchar(30))
as
Insert into CourseInstance(TrainingCourseID,Description,CourseDate,CourseStartTIme,CourseEndTime,CourseLocationID,CourseInstructorID,ModifiedBy,ModifiedDate)
values(@tcid,@desc,@cdate,@stime,@etime,@locid,@insid,@user,getdate())
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