'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 4/30/2014
' Time: 12:39 PM
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
Imports System.IO

	''' <summary>
	''' Description of MergeRecords
	''' </summary>
	Public Class MergeRecords
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected WithEvents dgSourceEmp As DataGrid
		Protected WithEvents dgMergeEmp As DataGrid
		Protected WithEvents cwcSourceEmp As ddlSearch
		Protected WithEvents cwcMergeEmp As ddlSearch
		Protected WithEvents btnMerge As Button
		Protected WithEvents UpPan1 As UpdatePanel
		Protected WithEvents UpPan2 As UpdatePanel
		Protected WithEvents hdNames As HiddenField
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
				cwcSourceEmp.ConWidth = 380
				cwcSourceEmp.CtrlDataSource = "select emp_id,last_name + ', ' + first_name + ' (' + isnull(socialsecurityno,emp_id) + ', ' + dept_name + ' ' + job_title + ')' as empinfo from v_employees order by last_name, First_Name"
				cwcSourceEmp.CtrlVField = "emp_id"
				cwcSourceEmp.CtrlTField = "empinfo"
				cwcSourceEmp.CtrlBind()
				
				
				cwcMergeEmp.ConWidth = 380
				cwcMergeEmp.CtrlDataSource = "select emp_id,last_name + ', ' + first_name + ' (' + isnull(socialsecurityno,emp_id) + ', ' + dept_name + ' ' + job_title + ')' as empinfo from v_employees order by last_name, First_Name"
				cwcMergeEmp.CtrlVField = "emp_id"
				cwcMergeEmp.CtrlTField = "empinfo"
				cwcMergeEmp.CtrlBind()
				
				dgSourceEmp.DataSource = GetDataView("select Data, Info from v_GenEmpInfo where emp_id = " + cwcSourceEmp.CtrlSelValue.ToString())
				dgSourceEmp.DataBind()
				
				dgMergeEmp.DataSource = GetDataView("select Data, Info from v_GenEmpInfo where emp_id = " + cwcMergeEmp.CtrlSelValue.ToString())
				dgMergeEmp.DataBind()
				
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
			AddHandler Me.cwcSourceEmp.IndChange, New ddlSearch.IndChangeEventHandler(AddressOf SEmp_Change)
			AddHandler Me.cwcMergeEmp.IndChange, New ddlSearch.IndChangeEventHandler(AddressOf MEmp_Change)
		End Sub
		Protected Sub PreRend(sender As Object,e As System.EventArgs)
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Manipulation"
		Protected Sub SEmp_Change(ByVal sender As Object, ByVal e As System.EventArgs)
			dgSourceEmp.DataSource = GetDataView("select Data, Info from v_GenEmpInfo where emp_id = " + cwcSourceEmp.CtrlSelValue.ToString())
			dgSourceEmp.DataBind()
			CheckNames()
		End Sub
		Protected Sub MEmp_Change(ByVal sender As Object, ByVal e As System.EventArgs)
			dgMergeEmp.DataSource = GetDataView("select Data, Info from v_GenEmpInfo where emp_id = " + cwcMergeEmp.CtrlSelValue.ToString())
			dgMergeEmp.DataBind()
			CheckNames()
		End Sub
		Protected Sub MergeEmp(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMerge.Click
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim mergecmd As New SqlCommand("MergeRecord",sqlconn)
			Dim mout As Integer
			
			If cwcSourceEmp.CtrlSelValue = "-1" Or cwcMergeEmp.CtrlSelValue = "-1" Then
				Response.Write("<script>alert('You must fill out both Source Employee and Merge To Employee fields.')</script>")
			Else
				HandleSig(CInt(GetSQLScalar("select count(emp_id) from empsignatures where emp_id = " + cwcSourceEmp.CtrlSelValue.ToString())),CInt(GetSQLScalar("select count(emp_id) from empsignatures where emp_id = " + cwcMergeEmp.CtrlSelValue.ToString())))
				With mergecmd
					.CommandType = CommandType.StoredProcedure
					.Parameters.Add("@srec",SqlDbType.VarChar)
					.Parameters.Add("@mrec",SqlDbType.VarChar)
					.Parameters.Add("@ssig",SqlDbType.Bit)
					.Parameters.Add("@msig",SqlDbType.Bit)
					.Parameters.Add("@merged",SqlDbType.Int)
					.Parameters("@merged").Direction = ParameterDirection.Output
					
					.Parameters("@srec").Value = cwcSourceEmp.CtrlSelValue
					.Parameters("@mrec").Value = cwcMergeEmp.CtrlSelValue
					.Parameters("@ssig").Value = GetSQLScalar("select count(emp_id) from empsignatures where emp_id = " + cwcSourceEmp.CtrlSelValue.ToString())
					.Parameters("@msig").Value = GetSQLScalar("select count(emp_id) from empsignatures where emp_id = " + cwcMergeEmp.CtrlSelValue.ToString())
					sqlconn.Open()
					.ExecuteNonQuery()
					mout = .Parameters("@merged").Value
				End With
				sqlconn.Close()
				mergecmd = Nothing
				If mout = 1 Then
					Response.Write("<script>alert('Records Merged Ran')</script>")
				Else
					Response.Write("<script>alert('Merge Failed: The records you are attempting to merge are the same')</script>")
				End If
				
			End If
			
		End Sub
		
		Protected Sub CheckNames()
			Dim SName As String
			Dim MName As String
			
			SName = GetSQLScalar("Select Last_Name + ' ' + First_Name from employee where emp_id = " + cwcSourceEmp.CtrlSelValue.ToString())
			MName = GetSQLScalar("Select Last_Name + ' ' + First_Name from employee where emp_id = " + cwcMergeEmp.CtrlSelValue.ToString())
			
			If SName <> MName Then
				hdNames.Value = "No"
			Else 
				hdNames.Value = "Yes"
			End If
		End Sub
		Protected Sub HandleSig(ssig As Integer,msig As Integer)
			Dim localpath As String
			Dim spath As String
			Dim mpath As String
	
			
			localpath = "C:\Inetpub\Training\TrainingScheduler\"
			
			If ssig > 0 And msig > 0 Then
				spath = localpath + CStr(GetSQLScalar("select sigpath from empsignatures where emp_id = " + cwcSourceEmp.CtrlSelValue.ToString))
				DelSig(spath)
			ElseIf ssig > 0 And msig = 0
				spath = localpath + CStr(GetSQLScalar("select sigpath from empsignatures where emp_id = " + cwcSourceEmp.CtrlSelValue.ToString))
				If CStr(GetSQLScalar("select sigpath from empsignatures where emp_id = " + cwcMergeEmp.CtrlSelValue.ToString)) = "" Then
					mpath = localpath + "Signatures\" + cwcMergeEmp.CtrlSelValue.ToString + "\"
				Else
					mpath = localpath + CStr(GetSQLScalar("select sigpath from empsignatures where emp_id = " + cwcMergeEmp.CtrlSelValue.ToString))
				End If
				
				If Not Directory.Exists(mpath) Then
					Directory.CreateDirectory(mpath)
				End If
				If File.Exists(spath + "EmpSig.jpg") Then
					File.Move(spath + "EmpSig.jpg",mpath + "EmpSig.jpg")
					Directory.Delete(spath)
				End If	
			End If
		End Sub
		Protected Sub DelSig(delpath As String)
			If File.Exists(delpath + "EmpSig.jpg") Then
				File.Delete(delpath + "EmpSig.jpg")
			End If
			If Directory.Exists(delpath) Then
				Directory.Delete(delpath)
			End If
		End Sub
		#End Region
	End Class
