select * from training_scheduler.dbo.Course_Calendar


select * from CourseINstance

delete from courseinstance

set =  

--Insert data into courseInstance table
insert into CourseInstance(TrainingCourseID,Description,CourseDate,CourseStartTime,CourseEndTime,CourseLocationID,CourseInstructorID,ModifiedBy,ModifiedDate)
select (select TrainingCourseID from TrainingCourse where CourseTitle =(select RTrim(Ltrim(Course_Title)) from training_scheduler.dbo.training_course where course_code = cc.Course_Code)),         --RTrim(Ltrim(cc.Course_title)))
coursedescription = 
Case
	when cc.course_code = '309' then cc.Course_Title
	else null
end,cc.Course_Date,cc.Course_Start_Time,
Course_End_Time,(select CourseLocationID from CourseLocation where LocationName = Rtrim(LTrim(cc.Course_Location))) as CourseLocationID,Null,'DWHITTAKER',getdate()
from training_scheduler.dbo.Course_Calendar cc
--

select * from CourseInstance where trainingcourseid is null

select * from trainingcourse where coursetitle like 'Safety%'

select * from training_scheduler.dbo.training_course where course_title like '%Diabetes (BS)%'

select * from training_scheduler.dbo.training_course where Course_Type like 'OneTime Only'


delete from TrainingCourse

update trainingcourse set CourseTitle = RTrim(LTrim(CourseTitle))


select * from training_scheduler.dbo.Training_Course

--Populate Training Course table
insert into TrainingCourse
select RTrim(Ltrim(Course_Title)),Course_Duration,IsNull((select CourseTypeID from coursetype where Description = Course_Type),1)
from training_scheduler.dbo.Training_Course 
where RTrim(Ltrim(Course_Title)) not in (select coursetitle from TrainingCourse)
--
select * from CourseType

select * from TrainingCourse where CourseTypeID is null

select * from 