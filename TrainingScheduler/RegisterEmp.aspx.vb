'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 7/26/2012
' Time: 1:07 PM
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
	''' Description of WebForm1
	''' </summary>
	Public Class RegisterEmp
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected dgRegGrid As System.Web.UI.WebControls.DataGrid 
		Protected strCCode As String
		Protected dtCdate As DateTime
		Protected strdaCmd As String
		'Protected dlEmp As System.Web.UI.WebControls.DropDownList
		Protected WithEvents cwcEmp As ddlSearch
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Page Init & Exit (Open/Close DB connections here...)"

		Protected Sub PageInit(sender As Object, e As System.EventArgs)
			CheckLogin(CBool(Session("login")),Me.Response)
		End Sub
		'----------------------------------------------------------------------
		Protected Sub PageExit(sender As Object, e As System.EventArgs)
		End Sub

		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Page Load"
		Private Sub Page_Load(sender As Object, e As System.EventArgs)
			If IsPostBack = False Then
				PageSecurity()
				RegFill()
				ListFill()
			End If
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
			'------------------------------------------------------------------
			'AddHandler _Button_Ok.ServerClick, New EventHandler(AddressOf Click_Button_Ok)
			'AddHandler _Input_Name., New EventHandler(AddressOf Changed_Input_Name)
			'------------------------------------------------------------------
		End Sub
		Protected Sub PreRend(sender As Object,e As System.EventArgs)
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Fill"
		Protected Sub RegFill()
			strCCode = Session("strCourID").ToString
			dtCDate = CDate(Session("dtCDate"))
			strdaCmd = "exec spEmpRecert " & strCCode & ",'" & dtCdate & "'"
			dgRegGrid.datasource = GetDataView(strdaCmd)
			dgRegGrid.DataBind
		End Sub
		Protected Sub ListFill()
'			Dim strEmp As String = "select emp_id, last_name + ', ' + first_name + ' (' + dept_name + ' ' + job_title + ')' as empinfo from v_employees where status <> 'Terminated' order by last_name, First_Name"
'			dlEmp.DataSource = GetDataView(strEmp)
'			dlEmp.DataValueField = "emp_id"
'			dlEmp.DataTextField = "empinfo"
'			dlEmp.DataBind
'			
'			dlEmp.Items.Insert(0, New ListItem("None selected","-1"))
'			dlEmp.SelectedValue = "-1"
						
			cwcEmp.ConWidth = 500
			cwcEmp.CtrlDataSource = "select emp_id, last_name + ', ' + first_name + ' (' + dept_name + ' ' + job_title + ')' as empinfo from v_employees where status <> 'Terminated' order by last_name, First_Name"
			cwcEmp.CtrlVField =  "emp_id"
			cwcEmp.CtrlTField = "empinfo"
			cwcEmp.CtrlBind()
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Manipulation"
		Protected Sub Emp_Click(ByVal source As Object, ByVal e As EventArgs)
			If Page.IsValid Then
				Dim dgEventArgs As DataGridCommandEventArgs
				If source.ToString = "System.Web.UI.WebControls.DataGrid" Then
					dgEventArgs = CType(e,DataGridCommandEventArgs)
					EnrollEmp(dgEventArgs.Item.Cells(0).Text,Session("strCCode").ToString)
				Else
					'EnrollEmp(dlEmp.SelectedItem.Value.ToString,Session("strCCode").ToString)
					EnrollEmp(cwcEmp.CtrlSelValue.ToString,Session("strCCode").ToString)
				End If
			End If 
		End Sub
		Protected Sub EnrollEmp(empID As String, ciID As String)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim insertcmd As New SqlCommand("InsertEmpRegistration",sqlconn)			
			With insertcmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@empid", SqlDbType.NVarChar)
				.Parameters.Add("@ci", SqlDbType.Int) 
				.Parameters.Add("@user",SqlDbType.VarChar)
				.Parameters("@empid").Value = empID
				.Parameters("@ci").Value  = ciID
				.Parameters("@user").Value = Session("loginusername")
				sqlconn.Open
				.ExecuteNonQuery()
			End With
			insertcmd = Nothing
			sqlconn.Close
			RegFill()
			Response.Write("<script>alert('Employee enrolled successfully')</script>")
		End Sub
		#End Region
	End Class
