--CourseCalendar View for Main Registration Screen
ALTER view CourseCalendar
as
select ci.CourseInstanceID,tc.TrainingCourseID,tc.CourseTitle,ci.Description,ci.CourseDate as 'CourseDate',Right(convert(varchar,ci.CourseStartTime),7) as 'CourseStartTime',
			Right(convert(varchar,ci.CourseEndTime),7) as 'CourseEndTime',convert(float,datediff(mi,ci.CourseStartTime,ci.CourseEndTime)/60.0) as CourseDuration,(select count(emp_id) from empregistration er where er.courseinstanceid = ci.courseinstanceid) as 'CourseSize',
			cl.LocationName as 'CourseLocation', InsName as 'CourseInstructor',ct.Months from courseinstance ci
			join TrainingCourse tc on tc.TrainingCourseID = ci.TrainingCourseID
			left join CourseLocation cl on cl.CourseLocationID = ci.CourseLocationID
			left join Instructors ins on ins.EMP_ID = ci.CourseInstructorID
			left join CourseType ct on ct.CourseTypeID = tc.CourseTypeID
where ci.displayincalendar = 1
--
select CourseTitle,Description,CourseDate,CourseStartTime,CourseEndTime,CourseDuration,COurseSize,CourseLocation,isnull(CourseInstructor,'') from coursecalendar where CourseDate between '07/01/2012' and '07/31/2012' order by CourseDate
select CourseTitle,Description,CourseDate,CourseStartTime,CourseEndTime,CourseDuration,CourseSize,CourseLocation,CourseInstructor from coursecalendar order by convert(varchar,CourseDate,101)

select * from CourseInstance order by courseinstructorid

--RecentRecertView
alter view RecentRecert
as
select distinct emp.emp_id,emp.first_name,emp.last_name,tc.trainingcourseid,tc.coursetitle,(select top 1 RecertDate from empregistration join courseinstance ci on ci.courseinstanceid = empregistration.courseinstanceid where emp_id = emp.emp_id and ci.trainingcourseid = tc.trainingcourseid order by RecertDate desc) as 'RecertDate'
from TrainingCourse tc
join courseinstance cui on cui.TrainingCourseID = tc.TrainingCourseID
join empregistration er on er.courseinstanceid = cui.courseinstanceid
join v_Employees emp on emp.emp_id = er.emp_id
--

--v_Exceptions
alter view [dbo].[v_Exceptions]
as
select ex.ExceptionID,ex.Emp_ID,emp.Last_Name,emp.First_Name,ex.StartDate,ex.EndDate,ex.Reason
from Exception ex 
Join v_Employees emp on emp.EMP_ID = ex.EMP_ID
--

--RegisteredEmployees
alter view RegisteredEmployees
as
select emp.EMP_ID,emp.First_Name,emp.Last_Name,emp.Dept_Name,emp.Job_Title,er.EnrollDate,er.CompletionDate,er.Status,CourseInstanceID
from EmpRegistration er
join v_Employees emp on emp.EMP_ID = er.EMP_ID
--

--Instructor View
Create view Instructors
as
select EMP_ID, Last_Name + ', ' + First_Name as 'InsName'from Employee
union all
select CourseInstructorID, LastName + ', ' + FirstName from CourseInstructor

--EmployeeView
create view v_Employees
as
select * from Employee
union all
select emp_id,Last_Name,First_Name,Address1,Address2,City,State,ZipCode,SocialSecurityNO,Job_Code,Job_Title,Hire_Date,End_date,Dept_Code,Dept_Name from SubCOntractors


select * from CourseInstructor

select * from coursecalendar where coursedate = '01/02/2007' order by coursedate

select * from empRegistration

select * from subcontractors




