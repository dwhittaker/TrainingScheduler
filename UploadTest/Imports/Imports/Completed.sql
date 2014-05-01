select * from modules where moduleid in (select distinct moduleid from employeemodule)


select * from employee where emp_id in (select distinct emp_id from empregistration where completiondate is not null and EmpModID in (select empmodid from employeemodule where moduleid in (18,19,20,21,23,26,27,28,29))
and completiondate between '1/1/2014' and '1/31/2014')
order by last_name