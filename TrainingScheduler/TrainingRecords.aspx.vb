Option Strict On
Option Explicit On
'
' Created by SharpDevelop.
' User: screveling
' Date: 8/7/2012
' Time: 11:20 AM
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.Sqlclient
Imports System.Drawing
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System
Imports Microsoft.VisualBasic
Imports TrainingScheduler.Utility


	''' <summary>
	''' Description of WebForm1
	''' </summary>
	Public Class TrainingRecords
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		
		Protected WithEvents EmployeeTrainingRecords As DataGrid		
		Protected WithEvents butShowCreateRecord As Button
		Protected WithEvents panCreateButton As Panel
		Protected WithEvents cwcEmp As ddlSearch

		'edit
		Protected WithEvents panRecordEdit As Panel
		Protected withevents EmpRegistrationID As TextBox
		Protected WithEvents CourseTitle As label
		Protected WithEvents labDescription As Label
		Protected WithEvents labDuration As Label
		Protected WithEvents labLocation As Label
		Protected WithEvents labInstructor As label
		Protected WithEvents EditCompletedDate As TextBox
		Protected WithEvents EditRecertDate As textbox
		Protected WithEvents EditComments As TextBox
		Protected WithEvents butSaveRecord As Button
		Protected WithEvents butCancelEdit As Button
		
		'create
		Protected WithEvents panRecordCreate As Panel
		Protected WithEvents ddlTrainingCourse As DropDownList
		Protected WithEvents Description As TextBox
		Protected withevents DescVal As CustomValidator
		Protected WithEvents Duration As TextBox
		Protected WithEvents ddlLocation As DropDownList
		protected WithEvents ddlInstructor As DropDownList
		protected WithEvents CreateCompletedDate As TextBox
		Protected WithEvents CreateRecertDate As textbox
		Protected WithEvents CreateComments As TextBox
		Protected WithEvents butCreateRecord As Button
		Protected WithEvents butCancelCreate As Button		
		Protected WithEvents UpPan1 As UpdatePanel
		Protected WithEvents UpPan2 As UpdatePanel
		Protected WithEvents UpPan3 As UpdatePanel
		Protected WithEvents UpPan4 As UpdatePanel
				
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Page Init & Exit (Open/Close DB connections here...)"
		Protected Overrides Sub OnInit(e As EventArgs)
			InitializeComponent()
			MyBase.OnInit(e)
		End Sub
		'----------------------------------------------------------------------
		Private Sub InitializeComponent()
			'------------------------------------------------------------------
			AddHandler Me.Load, New System.EventHandler(AddressOf Page_Load)
			AddHandler Me.Init, New System.EventHandler(AddressOf PageInit)
			AddHandler Me.Unload, New System.EventHandler(AddressOf PageExit)	
			AddHandler Me.PreRender, New System.EventHandler(AddressOf PreRend)
			AddHandler Me.cwcEmp.IndChange, New ddlSearch.IndChangeEventHandler(AddressOf cwcEmp_SelIndChange)
		End Sub
		Protected Sub PageInit(sender As Object, e As System.EventArgs)
			CheckLogin(Cbool(Session("login")),Me.Response)
		End Sub
		'----------------------------------------------------------------------
		Protected Sub PageExit(sender As Object, e As System.EventArgs)
		End Sub
		Protected Sub PreRend(sender As Object, e As System.EventArgs)
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		
		Private Sub Page_Load(sender As Object, e As System.EventArgs)
			'------------------------------------------------------------------
			Dim Cpage As String
			Dim ind As Integer
			Utility.CheckLogin(CBool(Session("login")), Me.Response)
			
			If Not IsPostBack Then		
				PageSecurity()
		
				cwcEmp.ConWidth = 575
				cwcEmp.CtrlDataSource = "select emp_id, last_name + ', ' + first_name + ' (' + dept_name + ' ' + job_title + ')' as empinfo from v_employees order by empinfo"
				cwcEmp.CtrlVField =  "emp_id"
				cwcEmp.CtrlTField = "empinfo"
				cwcEmp.CtrlBind()
				
				ddlLocation.DataSource = Utility.GetDataView( _
					"select CourseLocationID, LocationName From CourseLocation order by LocationName")
				ddlLocation.DataValueField = "CourseLocationID"
				ddlLocation.DataTextField = "LocationName"
				ddlLocation.DataBind
				ddlLocation.Items.Insert(0, New ListItem("None selected", "-1"))
				ddlLocation.SelectedValue = "-1"				
				
				ddlInstructor.DataSource = Utility.GetDataView( _
					"select EMP_ID, InsName From Instructors order by InsName")
				ddlInstructor.DataValueField = "EMP_ID"
				ddlInstructor.DataTextField = "InsName"
				ddlInstructor.DataBind
				ddlInstructor.Items.Insert(0, New ListItem("None selected", "-1"))
				ddlInstructor.SelectedValue = "-1"		
				
				ddlTrainingCourse.datasource=Utility.GetDataView( _
					"select TrainingCourseID, CourseTitle + ' (' + cast(CourseDuration as varchar(5)) + ')' as CourseTitle from TrainingCourse order by CourseTitle")
				ddlTrainingCourse.DataValueField="TrainingCourseID"
				ddlTrainingCourse.DataTextField = "CourseTitle"
				ddlTrainingCourse.DataBind
				ddlTrainingCourse.Items.Insert(0, New ListItem("Select...", "-1"))
				ddlTrainingCourse.SelectedValue = "-1"
				
				With panRecordEdit
					.Style.Add("padding", "10px")
					.BorderStyle = BorderStyle.solid
					.BorderColor = Color.Black
					.BorderWidth = Unit.Pixel(1)
				End With
				
				butCancelCreate.CausesValidation=False
				butCancelEdit.CausesValidation=False
				
				Session("SField") = "coursetitledesc"
				Session("SOrder") = "ASC"
			End If
				
			'------------------------------------------------------------------
		End Sub
		Protected Sub PageSecurity()
			If PageAccess(System.Web.HttpContext.Current.Request.Path.ToString(),CInt(Session("SecurityGroupID"))) = False Then
				Response.Redirect("Default.aspx")
			Else
				FormAccess(Me.Form,CInt(Session("SecurityGroupID")))
			End If
		End Sub
		
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		
		Sub GridLoad()
			Dim dv As DataView = Utility.GetDataView(String.Format( _
				"select er.EmpRegistrationID, er.comments, locationname, recertdate, " & _
				"(case when coursedate='1/1/1900' then null else coursedate end) as coursedate, " & _ 
				"completiondate, coursetitle + isnull('/<br />' + description, '') as coursetitledesc, " & _ 
				"datediff(mi, coursestarttime, courseendtime)/60.0 as duration, " & _
				"insname from empregistration er " & _
				"inner join courseinstance ci on er.courseinstanceid = ci.courseinstanceid " & _
				"inner join trainingcourse tc on ci.trainingcourseid = tc.trainingcourseid " & _
				"left join instructors cin on ci.courseinstructorid = cin.emp_id " & _
				"left join courselocation cl on ci.courselocationid = cl.courselocationid " & _
				"where er.emp_id = '{0}' order by coursetitle", cwcEmp.CtrlSelValue))
					
			dv.Sort = CStr(Session("SField")) & " " & CStr(Session("SOrder")			)
			EmployeeTrainingRecords.datasource = dv
			EmployeeTrainingRecords.databind
		End Sub
		
		Sub grid_sort(o As Object, e As DataGridSortCommandEventArgs) Handles EmployeeTrainingRecords.SortCommand
			Dim sf As String = CStr(Session("SField"))
			dim so As string = CStr(Session("SOrder"))
			If sf = e.SortExpression Then
				If so = "ASC" Then
					so = "DESC"
				Else
					so = "ASC"
				End If
			Else
				sf = e.SortExpression
				so = "ASC"
			End If
			Session("SField") = sf
			Session("SOrder") = so
			GridLoad()
		End Sub
		
		Protected Function ParseDuration(s As String) As string
			Dim d As Double = Double.Parse(s)
			Return d.ToString("f2")
		End Function
		
		Protected Sub cwcEmp_SelIndChange(s As Object, e As EventArgs)
			If cwcEmp.CtrlSelValue <> "-1" Then
				GridLoad()
				panCreateButton.Visible=True	
				UpPan1.Update()
			Else
				panCreateButton.Visible=False
				UpPan1.Update()
				EmployeeTrainingRecords.Datasource = Nothing
				EmployeeTrainingRecords.DataBind
			End If	
			
		End Sub
		
		Sub RowCommandHandler(sender as Object, e As CommandEventArgs)
			If e.CommandName = "Edit" Then
				panRecordEdit.Visible = True
				UpPan2.Update()
				panRecordCreate.Visible = False
				UpPan3.Update()
							
				Dim dr As sqldatareader = Utility.GetDataReader(String.Format( _
					"select empregistrationid, coursetitle, description, locationname, datediff(mi, coursestarttime, courseendtime)/60.0 as duration, " & _
					"insname, " & _
					"completiondate, recertdate, er.comments from empregistration er " & _
					"inner join courseinstance ci on er.courseinstanceid = ci.courseinstanceid " & _
					"inner join trainingcourse tc on ci.trainingcourseid = tc.trainingcourseid " & _
					"left join instructors cin on ci.courseinstructorid = cin.emp_id " & _
					"left join courselocation cl on ci.courselocationid = cl.courselocationid " & _
					"where EmpRegistrationID = {0}", e.CommandArgument))
				
				If dr.HasRows Then
					dr.Read()
					EmpRegistrationID.Text = dr("empregistrationid").ToString()
					CourseTitle.Text = dr("coursetitle").ToString()
					labDescription.Text = dr("description").ToString()
					labDuration.Text = dr("duration").ToString()
					labLocation.Text = dr("locationname").ToString()
					labInstructor.Text = dr("insname").ToString()
					
					If dr("completiondate").Equals(System.DBNull.Value) Then
						EditCompletedDate.Text = ""
					Else
						EditCompletedDate.Text = CDate(dr("completiondate")).ToString("d")
					End If
									
					If dr("recertdate").Equals(System.DBNull.Value) Then
						EditRecertDate.Text = ""
					Else
						EditRecertDate.Text = CDate(dr("recertdate")).ToString("d")						
					End If
					
					EditComments.Text = dr("comments").ToString()
				End If			
			ElseIf e.CommandName = "Delete"
				Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
				Dim deletecmd As New SqlCommand("DeleteEmpRegistration",sqlconn)
				
				With deletecmd
					.CommandType = CommandType.StoredProcedure
					.Parameters.Add("@empid", SqlDbType.NVarChar)
					.Parameters.Add("@ci", SqlDbType.Int)
					.Parameters.Add("@erid", SqlDbType.int)				
					
					.Parameters("@empid").Value = System.DBNull.value
					.Parameters("@ci").Value = System.DBNull.value
					.Parameters("@erid").Value = e.CommandArgument
					
					sqlconn.Open
					.ExecuteNonQuery()
				End With
				deletecmd = Nothing
				sqlconn.Close
				
				GridLoad()
				panRecordEdit.Visible=False
				UpPan2.Update()
				panRecordCreate.Visible=False
				UpPan3.Update()
			End If
			
		End Sub
		
		Protected Sub TrainingCourse_Change(sender As Object, e As EventArgs) Handles ddlTrainingCourse.SelectedIndexChanged
			Dim dur As String = ddlTrainingCourse.SelectedItem.Text.Substring(ddlTrainingCourse.SelectedItem.Text.lastIndexOf("(")+1)
			Duration.Text = dur.Substring(0, dur.IndexOf(")"))
		End Sub
		
		Private Sub ItemDataBound(sender As Object, e As DataGridItemEventArgs) Handles EmployeeTrainingRecords.ItemDataBound			
			dim tcnum As Integer
			Dim cnum As Integer
			Dim c As control
			For tcnum = 0 To e.item.Cells.Count-1
				For cnum = 0 To e.Item.Cells(tcnum).Controls.Count - 1
					c = e.Item.Cells(tcnum).Controls(cnum)					
					'Response.Write(tcnum & ", " & cnum & " - " & c.ID & " - " & c.GetType().ToString() & "<br />")
					If c.ID = "lnbDelete" Then
						CType(c, LinkButton).Attributes.Add("onClick", "return ConfirmDeletion();")			
						Exit sub
					End If
				Next
			Next
			'Response.Write("</p>")
			'Response.Write(c.id)
			'CType(e.Item.Cells(0).Controls(3), LinkButton).Attributes.Add ( "onClick", "return ConfirmDeletion();")			
		End Sub
		
		Sub butCreateRecord_Click(sender As Object, e As EventArgs) Handles butShowCreateRecord.Click
			panRecordEdit.Visible=False
			UpPan2.Update()
			panRecordCreate.Visible = True
			UpPan3.Update()
			
			ddlTrainingCourse.SelectedValue="-1"
			Description.Text = ""
			Duration.Text = ""	
			ddlLocation.SelectedValue="-1"
			ddlInstructor.SelectedValue="-1"
			CreateRecertDate.Text = ""
			CreateCompletedDate.Text = ""
			CreateComments.Text = ""
			UpPan1.Update()
		End Sub
		
		Sub SaveRecordClicked(sender As Object, e As EventArgs) Handles butSaveRecord.Click
			If Not Page.IsValid Then
				Exit Sub
			End if								
			
			UpdateTrainingRecord(EmpRegistrationID.Text, EditRecertDate.Text.Trim, Editcompleteddate.Text.Trim, EditComments.Text.Trim)	
			
			GridLoad()
			panRecordEdit.Visible = False		
			UpPan2.Update()
			UpPan1.Update()
		End Sub
		
		Sub CreateRecordClicked(sender As Object, e As EventArgs) Handles butCreateRecord.click
			If Not Page.IsValid Then
				Exit Sub
			End If					
			
			CreateTrainingRecord( _
					cwcEmp.CtrlSelValue, _
					ddlTrainingCourse.SelectedValue, _
					Description.Text.Trim, _
					Double.Parse(Duration.Text.Trim), _
					ddlLocation.SelectedValue, _
					ddlInstructor.SelectedValue, _
					CreateCompletedDate.Text.trim, _
					CreateRecertDate.Text.Trim, _
					CreateComments.Text.Trim)
			GridLoad()
			panRecordCreate.Visible = False			
			UpPan3.Update()
			UpPan1.Update()
		End Sub
		
		Sub canceledit_Click(sender As Object, e As EventArgs) Handles butCancelEdit.click
			panRecordEdit.Visible= False
			UpPan2.Update()
			UpPan1.Update()
		End Sub
		Sub cancelcreate_Click(sender As Object, e As EventArgs) Handles butCancelCreate.click
			panRecordCreate.Visible= False
			UpPan3.Update()
			UpPan1.Update()
		End Sub
		
		Protected Sub UpdateTrainingRecord(EmpRegistrationID As String, recertdate As String, completiondate As string, comments As string)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim updatecmd As New SqlCommand("UpdateEmpRegistration",sqlconn)
			
			With updatecmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@empregistrationid", SqlDbType.Int)
				.Parameters.Add("@recertdate", SqlDbType.DateTime) 
				.Parameters.Add("@completiondate", SqlDbType.DateTime) 
				.Parameters.Add("@comments", SqlDbType.VarChar)
				.Parameters.Add("@user",SqlDbType.VarChar)
				.Parameters("@empregistrationid").Value = EmpRegistrationID
				If recertdate = "" Then
					.Parameters("@recertdate").Value  = System.DBNull.value
				Else
					.Parameters("@recertdate").Value  = DateTime.Parse(recertdate)
				End If
				If completiondate = "" Then
					.Parameters("@completiondate").Value  = System.DBNull.value
				Else
					.Parameters("@completiondate").Value  = DateTime.Parse(completiondate)
				End If
				.Parameters("@comments").Value = IIf(comments="", System.DBNull.value, comments)
				.Parameters("@user").Value = Session("loginusername")
				sqlconn.Open
				.ExecuteNonQuery()
			End With
			updatecmd = Nothing
			sqlconn.Close
			'RegFill()
		End Sub
		
		Protected Sub CreateTrainingRecord(empid As String, trainingcourseid As String, description As string, duration As Double, locationid As String, instructorid As String, completiondate As string, recertdate As String, comments As string)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim createcmd As New SqlCommand("CreateEmpRegistration",sqlconn)
			
			With createcmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@empid", SqlDbType.NVarChar)
				.Parameters.Add("@tcid", SqlDbType.Int)
				.Parameters.Add("@completiondate", SqlDbType.DateTime)
				.Parameters.Add("@recertdate", SqlDbType.DateTime)
				.Parameters.Add("@duration", SqlDbType.Float)
				.Parameters.Add("@desc", SqlDbType.VarChar)
				.Parameters.Add("@insid", SqlDbType.varchar)
				.Parameters.Add("@locid", SqlDbType.Int)			
				.Parameters.Add("@comments", Sqldbtype.VarChar)
				.Parameters.Add("@user",SqlDbType.VarChar)
				
				.Parameters("@empid").Value = empid
				.Parameters("@tcid").Value = trainingcourseid
				.Parameters("@desc").Value = iif(description = "", System.DBNull.value, description)
				.Parameters("@duration").Value = duration
				.Parameters("@locid").Value = iif(locationid="-1", System.DBNull.value, locationid)
				.Parameters("@insid").Value = iif(instructorid="-1", System.DBNull.value, instructorid)
				If recertdate = "" Then
					.Parameters("@recertdate").Value  = System.DBNull.value
				Else
					.Parameters("@recertdate").Value  = DateTime.Parse(recertdate)
				End If
				If completiondate = "" Then
					.Parameters("@completiondate").Value  = System.DBNull.value
				Else
					.Parameters("@completiondate").Value  = DateTime.Parse(completiondate)
				End If		
				.Parameters("@comments").Value = iif(comments="", System.DBNull.value, comments)
				.Parameters("@user").Value = Session("loginusername")
				sqlconn.Open
				.ExecuteNonQuery()
			End With
			createcmd = Nothing
			sqlconn.Close
			'RegFill()
		End Sub
		
		Protected Sub DescValidatorLogic(sender As Object, args As ServerValidateEventArgs) Handles DescVal.ServerValidate
			
			If ddlTrainingCourse.SelectedItem.Text = "Inservice (0.00)" Then
				If Description.Text.trim="" Then
			
					args.IsValid = False
					Exit sub
				End If
			End If
			args.IsValid=true
		End Sub

		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<




		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
	End Class
