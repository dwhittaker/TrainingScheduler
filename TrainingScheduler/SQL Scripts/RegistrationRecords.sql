select * from EmpRegistration where emp_id = 2421594 order by completiondate

select * from Employee where last_name like 'Dolin%'


select * from training_scheduler.dbo.Training_Course

select * from training_scheduler.dbo.emp_Registration_records

select * from training_scheduler.dbo.emp_Training_Records where completed_date <> '01/01/1900'

select * from training_scheduler.dbo.emp_Training_Records where emp_record_no not in (select emp_record_no from training_scheduler.dbo.emp_Training_Records et join training_scheduler.dbo.emp_Registration_records er on er.course_code = et.course_code and er.Course_Date = et.Completed_date and er.emp_id = et.emp_ID) and completed_Date <> '01/01/1900' and last_name like 'Meehan'

--Insert into Emp Reg
insert into EmpRegistration
select er.Emp_ID,CourseInstanceID =
case
	when er.Course_Code = '309' then (select ci.CourseInstanceID from courseinstance ci where Rtrim(Ltrim(ci.Description)) = Rtrim(Ltrim(er.Course_Title)) and ci.CourseDate = er.Course_Date and ci.CourseStartTime =er.Course_Start_Time and ci.CourseEndTime= er.Course_End_Time )
	else (select ci.CourseInstanceID from courseinstance ci join trainingcourse tr on tr.TrainingCourseID = ci.TrainingCourseID where Rtrim(Ltrim(tr.CourseTitle)) = (select RTrim(Ltrim(Course_Title)) from training_scheduler.dbo.training_course where course_code = er.Course_Code) and ci.CourseDate = er.Course_Date and ci.CourseStartTime = er.Course_Start_Time and ci.CourseEndTime = er.Course_End_Time)
end,Date_Enrolled,CompletedDate = 
case
	when er.course_code = '309' then (select distinct ec.Completed_Date from training_scheduler.dbo.emp_Training_Records ec where ec.course_code = er.course_code and ec.Completed_date = er.Course_Date)
	else (select distinct ec.Completed_Date from training_scheduler.dbo.emp_Training_Records ec where Rtrim(Ltrim(ec.Course_Title)) = RTrim(Ltrim(er.Course_Title)) and ec.Completed_date = er.Course_Date)
end,status,null,'DWHITTAKER',getdate()
from training_scheduler.dbo.emp_Registration_records er
where emp_id in (select emp_id from employee)
--

--Update recert Date
declare @erID int
declare @CIID int
declare @comDate datetime
declare @months int
declare @rdate datetime

declare recert cursor for
select EmpRegistrationID, CourseInstanceID,CompletionDate from EmpRegistration

open recert

fetch next from recert into @erID,@CIID,@comdate
WHILE @@Fetch_Status = 0
begin
	set @months = (select Months from CourseType ct join TrainingCourse tr on tr.CourseTypeID = ct.CourseTypeID join CourseInstance ci on ci.TrainingCourseID = tr.TrainingCourseID where courseinstanceid = @CIID)
	if @months <> Null
	begin
		set @rdate = dateadd(mm,@months,@comdate)
		update EmpRegistration set recertdate = @rdate where EmpRegistrationID = @erID
	end 
	else 
		set @rdate = Null
	
	
	fetch next from recert into @erID,@CIID,@comdate
end
close recert
deallocate recert
--
select top 1 * from employee

select * from empregistration where courseinstanceid is null

delete from employee

delete from empregistration

select * from 

select * from training_scheduler.dbo.emp_Registration_records where emp_id = '9692491' and date_enrolled = '2011-09-12 00:00:00.000'

select * from training_scheduler.dbo.Course_Calendar where course_code in ('A17','M115') and course_date in ('06/22/2012','06/26/2012')


select * from courseinstance where trainingcourseid = 37 and coursedate = '2009-06-24 00:00:00.000' 

insert into employee
select * from training_scheduler.dbo.employee

select * from training_scheduler.dbo.emp_Registration_records where emp_id = 2421466
order by course_date

select * from CourseInstance where courseInstanceid = 4150

 (e-time Leave Request)


select * from training_scheduler.dbo.course_calendar where course_title like '%e-time Leave Req%'

select * from courseinstance where description like '%e-time Leave Req%'

select * from trainingcourse where coursetitle like '%inser%'


select * from training_scheduler.dbo.course_calendar where course_date = '2005-01-27' and course_start_time = '2004-09-30 13:00:00.000' and course_end_time = '2004-09-30 16:00:00.000'

select * from CourseInstance where courseinstanceid = '9396'

select * from trainingcourse where coursetitle like 'PDR Update/Navigation Plan'