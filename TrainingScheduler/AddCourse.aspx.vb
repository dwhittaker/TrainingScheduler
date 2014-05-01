'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 9/11/2012
' Time: 2:00 PM
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Data.SqlClient
Imports TrainingScheduler.Utility
	''' <summary>
	''' Description of AddCourse
	''' </summary>
	Public Class AddCourse
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected txtSTime As TextBox
		Protected txtETime As TextBox
		Protected txtCDate As TextBox
		Protected txtDesc As TextBox
		Protected ddlLoc As DropDownList
		Protected ddlIns As DropDownList
		Protected cwcClass As ddlSearch
		Protected lblDuration As HiddenField
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Page Init & Exit (Open/Close DB connections here...)"

		Protected Sub PageInit(sender As Object, e As System.EventArgs)
			CheckLogin(Cbool(Session("login")),Me.Response)
		End Sub
		'----------------------------------------------------------------------
		Protected Sub PageExit(sender As Object, e As System.EventArgs)
		End Sub

		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Page Load"
		Private Sub Page_Load(sender As Object, e As System.EventArgs)
			'------------------------------------------------------------------
			If Not IsPostBack Then
				PageSecurity()

				ddlLoc.DataSource = GetDataView("Select * from courselocation")
				ddlLoc.DataValueField = "CourseLocationID"
				ddlLoc.DataTextField = "LocationName"
				ddlLoc.DataBind
				ddlLoc.Items.Insert(0, New ListItem("None selected","-1"))
				ddlLoc.SelectedValue = "2"
				
				ddlIns.DataSource = GetDataView("Select * from Instructors order by InsName")
				ddlIns.DataValueField = "EMP_ID"
				ddlIns.DataTextField = "InsName"
				ddlIns.DataBind
				ddlIns.Items.Insert(0, New ListItem("None selected","-1"))
				ddlIns.SelectedValue = "-1"
				
				cwcClass.ConWidth = 380
				cwcClass.CtrlDataSource = "select TrainingCourseID,CourseTitle + " & "' ('" & " + cast(CourseDuration as varchar(50)) + ')' as 'Title' from trainingcourse where active = 1 order by coursetitle asc"
				cwcClass.CtrlVField = "TrainingCourseID"
				cwcClass.CtrlTField = "Title"
				cwcClass.CtrlBind()
				
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
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Add more events here..."

		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Initialize Component"

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
			AddHandler Me.cwcClass.IndChange, New ddlSearch.IndChangeEventHandler(AddressOf Class_Change)
			'------------------------------------------------------------------
			'------------------------------------------------------------------
		End Sub
		Protected Sub PreRend(sender As Object,e As System.EventArgs)
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
		End Sub
		Protected Sub Class_Change(sender As Object,e As System.EventArgs)
			Dim intStart As Integer
			Dim dur As String
			Dim Val As String
			
			Val = cwcClass.CtrlSelText.ToString()
			
			intStart = cwcClass.CtrlSelText.LastIndexOf("(")
			
			dur = Val.Substring(intStart + 1,4)
			
			lblDuration.Value = dur.ToString
			
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Fill"
		Protected Sub ClearFrm()
			txtSTime.Text = ""
			txtETime.Text = ""
			txtCDate.Text = ""
			txtDesc.Text = ""
			ddlIns.SelectedIndex = 0
			ddlLoc.SelectedIndex = 2
			cwcClass.CtrlSelValue = "-1"
		End Sub

		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Manipulate"
		Protected Sub AddClass()
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim insertcmd As New SqlCommand("AddClass",sqlconn)
			With insertcmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@tcid",SqlDbType.Int)
				.Parameters.Add("@cdate",SqlDbType.DateTime)
				.Parameters.Add("@stime",SqlDbType.DateTime)
				.Parameters.Add("@etime",SqlDbType.DateTime)
				.Parameters.Add("@locid",SqlDbType.Int)
				.Parameters.Add("@insid",SqlDbType.NVarChar)
				.Parameters.Add("@desc",SqlDbType.NVarChar)
				.Parameters.Add("@user",SqlDbType.NVarChar)
				.Parameters.Add("@display",SqlDbType.Int)
				.Parameters.Add("@cid",SqlDbType.int)
				.Parameters("@cid").Direction = ParameterDirection.Output
				.Parameters("@tcid").Value = cwcClass.CtrlSelValue
				.Parameters("@cdate").Value = txtCDate.Text.ToString
				.Parameters("@stime").Value = txtSTime.Text.ToString
				.Parameters("@etime").Value = txtETime.Text.ToString
				.Parameters("@display").Value = 1
				If ddlLoc.SelectedIndex = 0 Then
					.Parameters("@locid").Value = DBNull.Value
				Else
					.Parameters("@locid").Value = ddlLoc.SelectedItem.Value.ToString
				End If
				If ddlIns.SelectedIndex = 0 Then
					.Parameters("@insid").Value = DBNull.Value
				Else
					.Parameters("@insid").Value = ddlIns.SelectedItem.Value.ToString
				End If
				If txtDesc.Text.ToString = "" Then
					.Parameters("@desc").Value = DBNull.Value
				Else
					.Parameters("@desc").Value = txtDesc.Text.ToString
				End If
				.Parameters("@user").Value = Session("loginusername")
				sqlconn.Open
				.ExecuteNonQuery
			End With
			sqlconn.Close
			insertcmd = Nothing
			Response.Write("<script>alert('Class Added')</script>")
			ClearFrm()
		End Sub
		#End Region
	End Class
