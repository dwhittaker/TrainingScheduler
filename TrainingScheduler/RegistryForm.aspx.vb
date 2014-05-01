'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 4/20/2012
' Time: 1:26 PM
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
	''' Description of RegistryForm
	''' </summary>
	Public Class RegistryForm
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected strCCode As String
		Protected dtCDate As DateTime
		Protected strStime As String
		Protected strEtime As String
		Protected strSize As String
		Protected strLoc As String
		Protected strCTitle As String
		Protected strInst As String
		Protected strDTime As String
		Protected strDesc As String
		Protected dgEmpGrid As System.Web.UI.WebControls.DataGrid 
		Protected strdaCmd As String
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
			EmpFill()
			strSize = dgEmpGrid.Items.Count.ToString()
			
			If Not IsPostBack Then
				PageSecurity()
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
			AddHandler Me.dgEmpGrid.ItemDataBound, New DataGridItemEventHandler(AddressOf _ItemDataBound)
			AddHandler Me.PreRender,New System.EventHandler(AddressOf PreRend)
			'------------------------------------------------------------------

			'------------------------------------------------------------------
		End Sub
		Private Sub _ItemDataBound(sender As Object, e As DataGridItemEventArgs)
			If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
				e.Item.Cells(10).Attributes.Add ( "onClick", "return ConfirmDeletion();")
			End If
		End Sub
		Protected Sub PreRend(sender As Object,e As System.EventArgs)
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
		End Sub

		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Fill"
		Protected Sub EmpFill()
			strCCode = Session("strCCode").ToString
			dtCDate = CDate(Session("dtCDate"))
			strStime = Session("strStime").ToString
			strEtime = Session("strEtime").ToString
			strLoc = Session("strLocation").ToString
			strCTitle = Session("strCTitle").ToString
			strInst = Session("strIns").ToString
			strDesc = Session("strDesc").ToString
			strDTime = dtCDate & " - " & strStime & " to " & strEtime
			strdaCmd = "select * from RegisteredEmployees where CourseInstanceID = " & strCCode & " order by last_name asc"
'			dsEmp.Clear
'			With daEmp
'				.SelectCommand.Connection.ConnectionString = connEmpStr
'				.SelectCommand.CommandText = strdaCmd
'			End With
'			sqlEmpconn.Open
'			daEmp.Fill(dsEmp,"RegisteredEmployees")
'			dvEmp.Table = dsEmp.Tables(0)
			dgEmpGrid.DataSource = GetDataView(strdaCmd)
			dgEmpGrid.DataBind
		End Sub
		# End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Manipulation"
		Protected Sub Remove_Emp(ByVal source As Object, ByVal e As DataGridCommandEventArgs)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim deletecmd As New SqlCommand("DeleteEmpRegistration",sqlconn)
			With deletecmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@empid", SqlDbType.NVarChar)
				.Parameters.Add("@ci", SqlDbType.Int) 
				.Parameters("@empid").Value = e.Item.Cells(0).Text
				.Parameters("@ci").Value  = strCCode
				sqlconn.Open
				.ExecuteNonQuery()
			End With
			deletecmd = Nothing
			sqlconn.Close
			EmpFill()	
		End Sub 
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Navigation"
		Protected Sub Emp_Click(ByVal source As Object, ByVal e As DataGridCommandEventArgs)
			If e.CommandName.ToString  <> "Delete" then
				If CStr(e.Item.Cells(8).Text) = "R" Then
					Session("EmpName") = e.Item.Cells(3).Text.ToString() + " " +  e.Item.Cells(1).Text.ToString()	
					Session("EmpID") = e.Item.Cells(0).Text.ToString()	
					Session("SPath") = "Signatures\" + e.Item.Cells(0).Text.ToString() + "\"	
					Session("SSNVer") = "False"
					Response.Redirect("SignIn.aspx")
				End If
			End If

		End Sub
		#End Region
	End Class
