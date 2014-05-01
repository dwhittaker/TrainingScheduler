'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 4/17/2012
' Time: 10:04 AM
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'Imports System.Collections
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Data.SqlClient
Imports System.Web.UI.ScriptManager
Imports TrainingScheduler.Utility
Imports System.Collections

''' <summary>
''' Displays Course Calendar and allows user to click on course to load up list of Registered Employees
''' </summary>
Public Class [Default]
	Inherits Page
	'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
	#Region "Data"
	Protected strTdate As String	
	Protected strSdate As String
	Protected strEdate As String
	Protected dgTrainGrid As System.Web.UI.WebControls.DataGrid 
	Protected strCmd As String
	Protected lblparent As TextBox
	Protected strOrder As String
	Protected DVSorted As New DataView
	Protected CLDate As System.Web.UI.WebControls.TextBox
	Protected CName As System.Web.UI.WebControls.Label
	Protected STime As System.Web.UI.WebControls.TextBox
	Protected ETime As System.Web.UI.WebControls.TextBox
	Protected CDesc As System.Web.UI.WebControls.TextBox
	Protected WithEvents ddlMonth As DropDownList
	Protected WithEvents ddlYear As DropDownList
	Protected WithEvents btnGoToDate As Button
	Protected WithEvents ddlIns As DropDownList
	Protected WithEvents ddlLoc As DropDownList
	Protected WithEvents hidparent As HiddenField
	Protected WithEvents panRegisterEmployee As Panel
	Protected WithEvents btnRegister As Button	
	Protected WithEvents rdAllClass As RadioButton
	Protected WithEvents rdRollup As RadioButton
	Protected WithEvents btnChangeView As Button
	Protected WithEvents cwcEmp As ddlSearch
	Protected WithEvents btnExpand As Button
	Protected WithEvents ScriptManager1 As ScriptManager
	#End Region
	'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
	#Region "Page Init & Exit (Open/Close DB connections here...)"

	Protected Sub PageInit(sender As Object, e As EventArgs)
'		If IsPostBack = False Then
'			dtTrdate = Session("dtTrDate")		
'		End If
		CheckLogin(CBool(Session("login")),Me.Response)
		If Not IsPostBack Then
			Session("SOrder") = "Asc"
			Session("SField") = "CourseDate"
		End If
	End Sub
	'----------------------------------------------------------------------
	Protected Sub PageExit(sender As Object, e As EventArgs)
'		Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
'		conn.Open
'		conn.ClearAllPools
'		conn.Close
'		conn.Dispose
	End Sub

	#End Region
	'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
	#Region "Page Load"
	Private Sub Page_Load(sender As Object, e As EventArgs)
		'Dim empreader As SqlDataReader
		
'		hidparent = Page.Master.FindControl("lblparent")
'		If hidparent.Value.ToString <> "" Then
'			Session("ParentPage") = hidparent.Value.ToString
'		End If 
'	------------------------------------------------------------------		
		PageSecurity()
		If Not IsPostBack Then
			rdAllClass.Attributes.Add("View","CourseCalendar")
			rdRollup.Attributes.Add("View","CourseCalendarModRollup")
			Session("Calendar") = "CourseCalendarModRollup"
			With ddlMonth.Items
				.Add(New ListItem("January", "1"))
				.Add(New ListItem("February", "2"))
				.Add(New ListItem("March", "3"))
				.Add(New ListItem("April", "4"))
				.Add(New ListItem("May", "5"))
				.Add(New ListItem("June", "6"))
				.Add(New ListItem("July", "7"))
				.Add(New ListItem("August", "8"))
				.Add(New ListItem("September", "9"))
				.Add(New ListItem("October", "10"))
				.Add(New ListItem("November", "11"))
				.Add(New ListItem("December", "12"))
			End With
			
			With ddlYear.Items
				.Add("2016")
				.Add("2015")
				.Add("2014")
				.Add("2013")
				.Add("2012")
				.Add("2011")
				.Add("2010")
				.Add("2009")
				.Add("2008")
				.Add("2007")
				.Add("2006")
				.Add("2004")
				.Add("2003")		
			End With
			ddlMonth.SelectedValue = CDate(Session("dtCalDate")).Month.ToString()
			ddlYear.SelectedValue = CDate(Session("dtCalDate")).Year.ToString()

			cwcEmp.ConWidth = 200
			cwcEmp.CtrlDataSource = "Select EMP_ID, LAST_NAME + ', ' + FIRST_NAME as FULLNAME FROM v_Employees WHERE Status <>'Terminated' ORDER BY LAST_NAME, FIRST_NAME"
			cwcEmp.CtrlVField =  "EMP_ID"
			cwcEmp.CtrlTField = "FULLNAME"
			cwcEmp.CtrlBind()


			CourseFill()

			Session("SField") = "CourseDate"
			Session("SOrder") = "ASC"
			Session("ExpandedMod") = ""

		End If			
		
		With panRegisterEmployee
			.Style.Add("padding", "10px")
			.BorderStyle = BorderStyle.solid
			.BorderColor = Color.Black
			.BorderWidth = Unit.Pixel(1)
		End With
	End Sub
	Protected Sub PageSecurity()
		'Dim Cpage As String
		'Dim ind As Integer
		
		FormAccess(Me.Form,CInt(Session("SecurityGroupID")))
	End Sub
	#End Region
	'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
	#Region "Initialize Component"

	Protected Overrides Sub OnInit(e As EventArgs)
		InitializeComponent()
		MyBase.OnInit(e)
	End Sub
	'----------------------------------------------------------------------
	Private Sub InitializeComponent()
		AddHandler Me.Load, New System.EventHandler(AddressOf Page_Load)
		AddHandler Me.Init, New System.EventHandler(AddressOf PageInit)
		AddHandler Me.Unload, New System.EventHandler(AddressOf PageExit)
		AddHandler Me.dgTrainGrid.ItemDataBound, New DataGridItemEventHandler(AddressOf _ItemDataBound)
		AddHandler Me.PreRender, New System.EventHandler(AddressOf PreRend)
		'AddHandler Me.rdAllClass.CheckedChanged 
	End Sub
	Private Sub _ItemDataBound(sender As Object, e As DataGridItemEventArgs)
		'Dim exp As String
		Dim vmode As String
		Dim lb As LinkButton
		Dim exp As Button
		Dim sm As ScriptManager
		'Dim btnExp As Button
		
		sm = ScriptManager.GetCurrent(Me)
		If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
			vmode = e.Item.Cells(16).Text.ToString
'			exp = "[" + e.Item.Cells(16).Text.ToString() + "," + e.Item.Cells(5).Text.ToString() + "," + e.Item.Cells(6).Text.ToString() + "]"
		
'			If vmode = "C" And Session("ExpandedMod").ToString.Contains(exp) = True Then
'				btnExp = e.Item.Cells(0).FindControl("btnExpand")
'				btnExp.Text = "-"
'			End If
			
			If rdRollup.Checked = True And vmode = "C" Then
				e.Item.Cells(2).Style.Add("font-style","italic")
				e.Item.Cells(0).FindControl("chkRegister").Visible = False
				e.Item.Cells(0).FindControl("btnExpand").Visible = True
				e.Item.Cells(14).Attributes("onClick") = ""
			End If
			
			If rdRollup.Checked = True And vmode = "E" Then
				e.Item.Attributes("style") = "display:none"
				e.Item.Cells(2).Style.Add("padding-left", "16px")
				e.Item.Cells(2).Style.Add("background-color","#F0FFFF")
				e.Item.Cells(4).Style.Add("background-color","#F0FFFF")
				e.Item.Cells(5).Style.Add("background-color","#F0FFFF")
				e.Item.Cells(6).Style.Add("background-color","#F0FFFF")
				e.Item.Cells(7).Style.Add("background-color","#F0FFFF")
				e.Item.Cells(8).Style.Add("background-color","#F0FFFF")
				e.Item.Cells(9).Style.Add("background-color","#F0FFFF")
				e.Item.Cells(10).Style.Add("background-color","#F0FFFF")
				e.Item.Cells(11).Style.Add("background-color","#F0FFFF")
			End If
			lb = e.Item.Cells(1).FindControl("btnClass")
			exp = e.Item.Cells(0).FindControl("btnExpand")
			sm.RegisterPostBackControl(lb)
			sm.RegisterPostBackControl(exp)
			
		End If
	End Sub
	Private Sub PreRend(sender As Object, e As System.EventArgs)
		If Not ScriptManager.GetCurrent(Me.Page).IsInAsyncPostBack  Then
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
		End If
	End Sub
	#End Region
	'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
	#Region "Navigate"
	Protected Sub btnBack_Click(sender As Object, e As System.EventArgs)
		'This procedure sets the date used to fill the table Backwards a month.  If the current month is december then it sets the 
		'date to january of the previous year
		If ddlMonth.SelectedValue = "1" Then
			ddlMonth.SelectedValue = "12"
			ddlYear.SelectedValue = (Integer.Parse(ddlYear.SelectedValue)-1).ToString()
		Else
			ddlMonth.SelectedValue = (Integer.Parse(ddlMonth.SelectedValue)-1).ToString()
		End If
		Session("dtCalDate") = New DateTime(Cint(ddlYear.SelectedValue), Cint(ddlMonth.SelectedValue), 1)
		CourseFill()
	End Sub
	Protected Sub btnForward_Click(sender As Object, e As System.EventArgs)
		'This procedure sets the date used to fill the table forward a month.  If the current month is december then it sets the date to 
		'january of the next year
		If ddlMonth.SelectedValue = "12" Then
			ddlMonth.SelectedValue = "1"
			ddlYear.SelectedValue = (Integer.Parse(ddlYear.SelectedValue)+1).ToString()
		Else
			ddlMonth.SelectedValue = (Integer.Parse(ddlMonth.SelectedValue)+1).ToString()
		End If
		Session("dtCalDate") = New DateTime(CInt(ddlYear.SelectedValue), CInt(ddlMonth.SelectedValue), 1)
		courseFill()
	End Sub
	
	Protected Sub btnGoToDate_Click(s As Object, e As EventArgs) Handles btnGoToDate.Click
		CourseFill()
		Session("dtCalDate") = New DateTime(CInt(ddlYear.SelectedValue), CInt(ddlMonth.SelectedValue), 1)
	End Sub
	Protected Sub Course_Click(ByVal source As Object, ByVal e As DataGridCommandEventArgs)
		'This procedure will set all of the session variables that will need to be reffered back to in the next page.  
		'It then redirects the user to the Registryform.aspx Page
		Dim pdates As Boolean
		Dim btnI As Button
		Dim sm As ScriptManager
		Dim test As String
		If e.Item.Cells(16).Text.ToString <> "C" Then
			sm = ScriptManager.GetCurrent(Me)
			If e.CommandName.ToString = "" Then
				Session("strCCode") = e.Item.Cells(1).Text
				Session("dtCDate") = e.Item.Cells(5).Text
				Session("strStime") = e.Item.Cells(6).Text
				Session("strEtime") = e.Item.Cells(7).Text
				Session("strSize") = e.Item.Cells(9).Text
				Session("strLocation") = e.Item.Cells(10).Text
				Session("strCTitle") = e.Item.Cells(3).Text
				Session("strIns") = e.Item.Cells(11).Text
				Session("strDesc") = e.Item.Cells(4).Text
				Session("strCourID") = e.Item.Cells(12).Text
				Session("intMonths") = e.Item.Cells(13).Text
			
				pdates = Cbool(GetSQLScalar("Select PastDates from LDAPGroups where GroupID = " + CStr(Session("SecurityGroupID"))))
			
				If CDate(Session("dtCDate")) < Date.Today And pdates = False Then
					Response.Write("<script>alert('You do not have rights to register employees for past classes')</script>")
				Else
					'Response.Redirect("RegistryForm.aspx")'ClientScript.RegisterStartupScript(Me.GetType,"RegistryFormOpen","<script>window.open('RegistryForm.aspx', '','height=709, width=1030,status= no, resizable= no,scrollbars=yes')</script>")
					sm.RegisterStartupScript(Me,Me.GetType(),"redir","document.location='RegistryForm.aspx'",True)
				End If		
			End If
		Else
			btnI = e.Item.FindControl("btnExpand")
			If btnI.Text = "+" Then
				Session("ExpandedMod") = Session("ExpandedMod").ToString() + "[" + e.Item.Cells(15).Text.ToString() + "," + e.Item.Cells(5).Text.ToString() + "," + e.Item.Cells(6).Text.ToString() + "]"
				btnI.Text = "-"
			Else
				Session("ExpandedMod") = Session("ExpandedMod").ToString().Replace("[" + e.Item.Cells(15).Text.ToString() + "," + e.Item.Cells(5).Text.ToString() + "," + e.Item.Cells(6).Text.ToString() + "]","")
				btnI.Text = "+"
			End If
			ExpandModule()
			'Response.Write("<script>alert('This is a module please change view to All Classes to work with the individual classes in this module.')</script>")
		End If
	End Sub
	
	Sub btnRegister_Click(o As Object, e As EventArgs) Handles btnRegister.Click
		Dim classid As String
		Dim pdates As Boolean
		If cwcEmp.CtrlSelValue = "-1" Then
			Response.Write("<script>alert('Select an employee!')</script>")
			return
		End If
		pdates = Cbool(GetSQLScalar("Select PastDates from LDAPGroups where GroupID = " + CStr(Session("SecurityGroupID"))))
		If CDate(Session("dtCDate")) < Date.Today And pdates = False Then
			Response.Write("<script>alert('You do not have rights to register employees for past classes')</script>")
		Else
			For Each i As DataGridItem In dgTrainGrid.Items
				If CType(i.Cells(0).FindControl("chkRegister"), CheckBox).Checked Then
					classid = i.Cells(1).Text
					Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
					Dim insertcmd As New SqlCommand("InsertEmpRegistration",sqlconn)			
					With insertcmd
						.CommandType = CommandType.StoredProcedure
						.Parameters.Add("@empid", SqlDbType.NVarChar)
						.Parameters.Add("@ci", SqlDbType.Int) 
						.Parameters.Add("@user",SqlDbType.VarChar)
						.Parameters("@empid").Value = cwcEmp.CtrlSelValue
						.Parameters("@ci").Value  = classid
						.Parameters("@user").Value = Session("loginusername")
						sqlconn.Open
						.ExecuteNonQuery()
					End With
					insertcmd = Nothing
					sqlconn.Close				
				End If
			Next
			Response.Write("<script>alert('Employee enrolled successfully in selected classes')</script>")
			CourseFill()
		End If
	End Sub
	
	#End Region
	'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
	#Region "DataFill"
	Protected Sub CourseFill()
		'Sets the start and end dates to be the entire month of the date in dtTrDate then fills
		'the data adaptor, grid, and view with select columns from the CourseCalendar view in SQL
		strSdate = ddlMonth.SelectedValue & "/" & "01" & "/" & ddlYear.SelectedValue
		strEdate = ddlMonth.SelectedValue & "/" & System.DateTime.DaysInMonth(Integer.Parse(ddlYear.SelectedValue), Integer.Parse(ddlMonth.SelectedValue)) & "/" & ddlYear.SelectedValue
		strCmd = "select CourseInstanceID,CourseTitle,Description,convert(varchar,CourseDate,101) as 'CourseDate',CourseStartTime,CourseEndTime,CourseDuration,CourseSize,isnull(CourseLocation,'') as 'CourseLocation',isnull(CourseInstructor,'') as 'CourseInstructor',TrainingCourseID,Months,Module,VMode from " + Session("Calendar").ToString +" where coursedate between '" + strSdate + "' and '" + strEdate + "' Order by CONVERT(DATETIME, CourseStartTime, 0)"
'		dsCourse.Clear
'		With daCourse
'			.SelectCommand.Connection.ConnectionString = connStr
'			.SelectCommand.CommandText = strCmd
'		End With
'		sqlconn.Open
'		daCourse.Fill(dsCourse,"CourseCalendar")
'		dvCourse.Table = dsCourse.Tables(0)
		DVSorted = GetDataView(strCmd)
		If rdAllClass.Checked = False Then
			If Session("SField").ToString = "CourseTitle" Then
				'Response.Write("<script>alert('Sorting by Course Title is not allowed while under Module Rollup view.')</script>")
				DVSorted.Sort = "Module,Vmode ASC," + Session("SField").ToString + " " + Session("SOrder").ToString + ",CourseDate ASC"
			Else
				DVSorted.Sort = Session("SField").ToString + " " + Session("SOrder").ToString + "," +  "Module,Vmode ASC"
			End If
		Else
			DVSorted.Sort = Session("SField").ToString + " " + Session("SOrder").ToString  
		End If
		dgTrainGrid.DataSource = DVSorted
		dgTrainGrid.DataBind()
		DVSorted.Dispose()
	End Sub
	Protected Sub ChangeView(sender As Object,e As EventArgs) Handles btnChangeView.Click
		If rdRollup.Checked Then
			Session("Calendar") = rdRollup.Attributes("View").ToString()
		Else
			Session("Calendar") = rdAllClass.Attributes("View").ToString()
		End If
		CourseFill
		Session("ExpandedMod") = ""
	End Sub
	#End Region
	'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
	#Region "Data Manipulation"
	Protected Sub SortData(ByVal source As Object, ByVal e As DataGridSortCommandEventArgs)
		If Session("SOrder").ToString = "Asc" Then
			Session("SOrder") = "Desc"
		Else If Session("SOrder").ToString = "Desc" Then
			Session("SOrder") = "Asc"
		Else
			Session("SOrder") = "Asc"
		End If
		Session("SField") = e.SortExpression.ToString	
		CourseFill()
	End Sub
	
	Protected Sub ExpandModule()
		Dim exptest As String
		For Each i As DataGridItem In dgTrainGrid.Items
			exptest = "[" + i.Cells(15).Text.ToString() + "," +i.Cells(5).Text.ToString() + "," + i.Cells(6).Text.ToString() + "]"
			If Session("ExpandedMod").ToString().Contains(exptest) And i.Cells(16).Text = "E" Then
				i.Attributes("style") = "display:table-row"
			Elseif Session("ExpandedMod").ToString().Contains(exptest) = False And i.Cells(16).Text = "E" Then
				i.Attributes("style") = "display:none"
			End If
		Next
	End Sub
	#End Region
End Class
