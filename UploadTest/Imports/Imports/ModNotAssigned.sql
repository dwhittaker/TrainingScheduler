select * from cdsimport where [name] like 'Van Guilder Jill'

update cdsimport set moduledate = 'Pre-service Residential Lehigh Valley' where moduledate = 'SPIN Pre-service Residential Lehigh Valley'

select distinct userid,[name],duedate,moduledate from cdsimport 
where userid in (select ImportUID from trainingschedulertest.dbo.v_employees 
where emp_id not in (select emp_id from trainingschedulertest.dbo.empregistration 
where empModId in (select empmodid from trainingschedulertest.dbo.employeemodule where moduleid in (select moduleid from trainingschedulertest.dbo.modules where modulename = moduledate)) and [status] <> 'Z'))
and moduledate in ('Pre-service Residential Lehigh Valley','SPIN FY14','SPIN Adult Services Pre-Service')
order by moduledate,duedate

pre-service LV - 11/30
AS Preservice 9/30,1/31/14,2/28/14,4/30/14
fy14 - 11/30,12/31,1/31/14,2/28/14