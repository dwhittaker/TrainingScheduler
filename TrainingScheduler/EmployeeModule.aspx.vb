'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 8/23/2013
' Time: 10:59 AM
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
	''' Description of EmployeeModule
	''' </summary>
	Public Class EmployeeModule
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		'Protected WithEvents ddlEmp As DropDownList
		Protected WithEvents ddlMod As DropDownList
		Protected WithEvents txtDate As TextBox
		Protected WithEvents chkRegister As CheckBox
		Protected WithEvents btnAdd As Button
		Protected WithEvents dgEmpModGrid As DataGrid
		Protected WithEvents cwcEmp As ddlSearch

		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Page Init & Exit (Open/Close DB connections here...)"

		Protected Sub PageInit(sender As Object, e As System.EventArgs)
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
'				ddlEmp.DataSource = GetDataView("select emp_id,last_name +  ', ' + first_name as Name from v_Employees where status <> 'Terminated' order by last_name, First_Name")
'				ddlEmp.DataValueField = "emp_id"
'				ddlEmp.DataTextField = "Name"
'				ddlEmp.DataBind
'				
'				ddlEmp.Items.Insert(0,New ListItem("None Selected","-1"))
'				ddlEmp.SelectedValue = "-1"
				
				ddlMod.DataSource = GetDataView("select ModuleID,ModuleName from modules where active = 1")
				ddlMod.DataValueField = "ModuleID"
				ddlMod.DataTextField = "ModuleName"
				ddlMod.DataBind
				
				ddlMod.Items.Insert(0,New ListItem("None Selected","-1"))
				ddlMod.SelectedValue = "-1"
				
				cwcEmp.ConWidth = 380
				cwcEmp.CtrlDataSource = "select emp_id, last_name + ', ' + first_name + ' (' + dept_name + ' ' + job_title + ')' as empinfo from v_employees where status <> 'Terminated' order by last_name, First_Name"
				cwcEmp.CtrlVField =  "emp_id"
				cwcEmp.CtrlTField = "empinfo"
				cwcEmp.CtrlBind()
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
			'AddHandler Me.ddlEmp.SelectedIndexChanged, New System.EventHandler(AddressOf Emp_Change)
			AddHandler Me.dgEmpModGrid.ItemDataBound, New DataGridItemEventHandler(AddressOf _ItemDataBound)
			AddHandler Me.PreRender, New System.EventHandler(AddressOf PreRend)
			AddHandler Me.cwcEmp.IndChange, New ddlSearch.IndChangeEventHandler(AddressOf Emp_Change)
			'------------------------------------------------------------------
			'------------------------------------------------------------------
		End Sub
		Private Sub _ItemDataBound(sender As Object, e As DataGridItemEventArgs)
			If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
				e.Item.Cells(4).Attributes.Add ( "onClick", "return ConfirmDeletion();")
			End If
		End Sub
		Protected Sub PreRend(sender As Object,e As System.EventArgs)
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
		End Sub
			'------------------------------------------------------------------
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Fill"
		Protected Sub Emp_Change(ByVal sender As Object,ByVal e As System.EventArgs)
			If cwcEmp.SeletedValue <> "-1" Then
				ddlMod.Enabled = True
				chkRegister.Enabled = True
				txtDate.Enabled = True
				EmpModFill(cwcEmp.CtrlSelValue.ToString)
			Else
				ddlMod.Enabled = False
				chkRegister.Enabled = False
				txtDate.Enabled = False
			End If

		End Sub
		Protected Sub EmpModFill(strEmp As String)
			dgEmpModGrid.DataSource = GetDataView("select emp.EmpModID,emp.ModuleID,m.ModuleName,convert(varchar,emp.DateAssigned,101) as 'DateAssigned' from employeemodule emp join Modules m on m.ModuleID = emp.ModuleID where emp.EMP_ID = " + strEmp)
			dgEmpModGrid.DataBind
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Manipulation"
		Protected Sub Add_Mod(ByVal source As Object, ByVal e As EventArgs)
			Dim AReg As Integer
			Dim regsql As String
			Dim enr As String
			
			regsql = "select count(empmodid) from employeemodule where emp_id = '" + cwcEmp.CtrlSelValue.ToString() + "' and ModuleID = " + CStr(ddlMod.SelectedValue) + " and DateAssigned = '" + txtDate.Text.ToString() + "'"
			
			AReg = CInt(GetSQLScalar(regsql))
			
			If AReg = 0 Then
				Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
				Dim addcmd As New SqlCommand("InsertEmpModule",sqlconn)
				With addcmd
					.CommandType = CommandType.StoredProcedure
					.Parameters.Add("@empid",SqlDbType.VarChar)
					.Parameters.Add("@modid",SqlDbType.Int)
					.Parameters.Add("@adate",SqlDbType.DateTime)
					.Parameters.Add("@reg",SqlDbType.Bit)
					.Parameters.Add("@user",SqlDbType.VarChar)
					.Parameters.Add("@enrolled",SqlDbType.Int)
					.Parameters("@enrolled").Direction = Data.ParameterDirection.Output
					.Parameters("@empid").Value = cwcEmp.CtrlSelValue
					.Parameters("@modid").Value = CInt(ddlMod.SelectedValue)
					.Parameters("@adate").Value = CDate(txtDate.Text)
					If chkRegister.Checked = True Then
						.Parameters("@reg").Value = 1
					Else
						.Parameters("@reg").Value = 0
					End If
					.Parameters("@user").Value = CStr(Session("loginusername"))
				
					sqlconn.Open
					.ExecuteNonQuery
					enr = .Parameters("@enrolled").Value
					If enr = 1 Then
						Response.Write("<script>alert('Module Added')</script>")
					Else	
						Response.Write("<script>alert('No Classes Exist to enroll person')</script>")
					End If 
				End With
				addcmd = Nothing
				sqlconn.Close
				EmpModFill(cwcEmp.CtrlSelValue.ToString)
			Else
				Response.Write("<script>alert('You cannot add an employee to the same module twice for one date.  If there has been a change to the module either remove the module from the employee and re-add it or add the new classes to the employee individually.')</script>")
			End If 
		End Sub
		Protected Sub Remove_Mod(ByVal source As Object,ByVal e As DataGridCommandEventArgs)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim delcmd As New SqlCommand("DeleteEmpModule",sqlconn)
			Dim Attended As Integer
			
			Attended = GetSQLScalar("select count(status) from empregistration where status = 'A' and EmpModID = " + CStr(e.Item.Cells(0).Text))
			
			If Attended = 0 Then
				With delcmd
					.CommandType = CommandType.StoredProcedure
					.Parameters.Add("@empmodID",SqlDbType.Int)
					.Parameters("@empmodID").Value = CInt(e.Item.Cells(0).Text)
					sqlconn.Open
					.ExecuteNonQuery
				End With
				delcmd = Nothing
				sqlconn.Close
			Else
				Response.Write("<script>alert('Employee has completed classes in this module it can not be deleted')</script>")
			End If
			EmpModFill(cwcEmp.CtrlSelValue.ToString)
		End Sub
		#End Region
	End Class
