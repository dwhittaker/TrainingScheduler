'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 8/20/2013
' Time: 4:25 PM
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
	''' Description of BuildModule
	''' </summary>
	Public Class BuildModule
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected WithEvents ddlClass As DropDownList
		Protected WithEvents lblModule As Label
		Protected WithEvents txtSdate As TextBox
		Protected WithEvents txtEdate As TextBox
		Protected WithEvents dgModClassGrid As DataGrid
		Protected WithEvents btnAdd As Button
		Protected WithEvents btnClear As Button
		Protected WithEvents cwcClass As ddlSearch
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
				
				cwcClass.ConWidth = 380
				cwcClass.CtrlDataSource = "select TrainingCourseID,CourseTitle + " & "' ('" & " + cast(CourseDuration as varchar(50)) + ')' as 'Title' from trainingcourse where active = 1 order by coursetitle asc"
				cwcClass.CtrlVField = "TrainingCourseID"
				cwcClass.CtrlTField = "Title"
				cwcClass.CtrlBind()
			End If
			lblModule.Text = CStr(GetSQLScalar("Select ModuleName from Modules where ModuleID = " + CStr(CInt(Session("mid")))))
			
			dgModClassGrid.DataSource = GetDataView ("Select * from v_ModuleCourses where ModuleID = " + CStr(CInt(Session("mid"))))
			dgModClassGrid.DataBind()
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
			AddHandler Me.dgModClassGrid.ItemDataBound, New DataGridItemEventHandler(AddressOf _ItemDataBound)
			AddHandler Me.PreRender, New System.EventHandler(AddressOf PreRend)
			'------------------------------------------------------------------
			'------------------------------------------------------------------
		End Sub
		Private Sub _ItemDataBound(sender As Object, e As DataGridItemEventArgs)
			If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
				e.Item.Cells(7).Attributes.Add ( "onClick", "return ConfirmDeletion();")
			End If
		End Sub
		Protected Sub PreRend(sender As Object,e As System.EventArgs)
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Manipulation"
		Protected Sub AddEdit_Class(ByVal source As Object, ByVal e As EventArgs) Handles btnAdd.Click
			Dim cExist As String
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim addcmd As New SqlCommand("AddModuleClass",sqlconn)
			Dim upcmd As New SqlCommand("UpdateModuleCourse",sqlconn)
			If btnAdd.Text = "Add Class" Then
				cExist = CStr(GetSQLScalar("select count(trainingcourseid) from ModuleCourses where ModuleID = " + CStr(Session("mid")) + " and trainingcourseid = " + CStr(cwcClass.CtrlSelValue)))
				If cExist = "0" Then
					With addcmd
						.CommandType = CommandType.StoredProcedure
						.Parameters.Add("@mid",SqlDbType.Int)
						.Parameters.Add("@tcid",SqlDbType.Int)
						.Parameters.Add("@sdate",SqlDbType.DateTime)
						.Parameters.Add("@edate",SqlDbType.DateTime)
						.Parameters("@mid").Value = CInt(Session("mid"))
						.Parameters("@tcid").Value = CInt(cwcClass.CtrlSelValue)
						.Parameters("@sdate").Value = CDate(txtSdate.Text)
						If txtEdate.Text = "" Then
							.Parameters("@edate").Value = DBNull.Value
						Else
							.Parameters("@edate").Value = CDate(txtEdate.Text)
						End If
						sqlconn.Open
						.ExecuteNonQuery
					End With
					addcmd = Nothing
					sqlconn.Close
				Else
					Response.Write("<script>alert('Class is already part of this module')</script>")
				End If
			Else
				With upcmd
					.CommandType = CommandType.StoredProcedure
					.Parameters.Add("@mid",SqlDbType.Int)
					.Parameters.Add("@tcid",SqlDbType.Int)
					.Parameters.Add("@sdate",SqlDbType.DateTime)
					.Parameters.Add("@edate",SqlDbType.DateTime)
					.Parameters("@mid").Value = CInt(Session("mid"))
					.Parameters("@tcid").Value = CInt(cwcClass.CtrlSelValue)
					.Parameters("@sdate").Value = CDate(txtSdate.Text)
					If txtEdate.Text = "" Then
						.Parameters("@edate").Value = DBNull.Value
					Else
						.Parameters("@edate").Value = CDate(txtEdate.Text)
					End If
					sqlconn.Open
					.ExecuteNonQuery
				End With
				upcmd = Nothing
				sqlconn.Close
			End If
			dgModClassGrid.DataSource = GetDataView ("Select * from v_ModuleCourses where ModuleID = " + CStr(CInt(Session("mid"))))
			dgModClassGrid.DataBind()
			ClearForm()
		End Sub
		Protected Sub ClearForm() Handles btnClear.Click
			txtEdate.text = ""
			txtSdate.Text = ""
			cwcClass.CtrlSelValue = "-1"
			btnAdd.Text = "Add Class"
		End Sub
		Protected Sub Remove_Class(ByVal source As Object,ByVal e As DataGridCommandEventArgs)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim delcmd As New SqlCommand("DeleteModuleCourse",sqlconn)
			With delcmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@mcid",SqlDbType.Int)
				.Parameters("@mcid").Value = CInt(e.Item.Cells(4).Text)
				sqlconn.Open
				.ExecuteNonQuery
			End With
			delcmd = Nothing
			sqlconn.Close
			dgModClassGrid.DataSource = GetDataView ("Select * from v_ModuleCourses where ModuleID = " + CStr(CInt(Session("mid"))))
			dgModClassGrid.DataBind()
			ClearForm()
		End Sub
		Protected Sub Edit_Class(ByVal source As Object,ByVal e As DataGridCommandEventArgs)
			cwcClass.CtrlSelValue = CInt(e.Item.Cells(5).Text)
			btnAdd.Text = "Update Class"
			txtSdate.Text = CStr(e.Item.Cells(2).Text)
			If CStr(e.Item.Cells(3).Text) = "&nbsp;" Then
				txtEdate.Text = ""
			Else
				txtEdate.Text = CStr(e.Item.Cells(3).Text)
			End If
		End Sub
		#End Region
	End Class
