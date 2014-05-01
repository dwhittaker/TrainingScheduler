'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 9/16/2013
' Time: 10:57 AM
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
'
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
Imports TrainingScheduler.Utility

	''' <summary>
	''' Description of BlockControls
	''' </summary>
	Public Class BlockControls
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected WithEvents lblGroup As Label
		Protected WithEvents lblPage As Label
		Protected WithEvents txtControl As TextBox
		Protected WithEvents btnAdd As Button
		Protected WithEvents dgBlkCtrlGrid As DataGrid
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
			Dim SPage As String
			Dim SGroup As String
			If Not IsPostBack Then
				PageSecurity()
				SGroup = GetSQLScalar("Select LDAPGroupName from LDAPGroups where groupid = " + CStr(Session("SelectedGroup")))
				SPage = GetSQLScalar("Select Title from Pages where pageid = " + CStr(Session("SelectedPage")))
				lblGroup.Text = SGroup
				lblPage.Text = SPage
				dgBlkCtrlGrid.DataSource = GetDataView("Select * from BlockedControls where pageid = " + CStr(Session("SelectedPage")) + " and groupid = " + CStr(Session("SelectedGroup")))
				dgBlkCtrlGrid.DataBind
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
			'------------------------------------------------------------------
			'------------------------------------------------------------------
		End Sub
		Protected Sub PreRend(sender As Object,e As System.EventArgs)
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Manipulation"
		Protected Sub Remove_Ctrl(ByVal source As Object, ByVal e As DataGridCommandEventArgs)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim delcmd As New SqlCommand("DeleteBlockedControl",sqlconn)
			With delcmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@cid",SqlDbType.Int)
				.Parameters("@cid").Value = CInt(e.Item.Cells(0).Text)
				
				sqlconn.Open
				.ExecuteNonQuery
			End With
			
			dgBlkCtrlGrid.DataSource = GetDataView("Select * from BlockedControls where pageid = " + CStr(Session("SelectedPage")) + " and groupid = " + CStr(Session("SelectedGroup")))
			dgBlkCtrlGrid.DataBind
			
			delcmd = Nothing
			sqlconn.Close
		End Sub
		Protected Sub AddCtrl(sender As Object, e As System.EventArgs) Handles btnAdd.Click
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim addcmd As New SqlCommand("AddBlockedControl",sqlconn)
			With addcmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@gid",SqlDbType.Int)
				.Parameters.Add("@pid",SqlDbType.Int)
				.Parameters.Add("@ctrl",SqlDbType.VarChar)
				.Parameters("@gid").Value = CInt(Session("SelectedGroup"))
				.Parameters("@pid").Value = CInt(Session("SelectedPage"))
				.Parameters("@ctrl").Value = CStr(txtControl.Text)
				
				sqlconn.Open
				.ExecuteNonQuery
			End With
			addcmd = Nothing
			sqlconn.Close
			
			dgBlkCtrlGrid.DataSource = GetDataView("Select * from BlockedControls where pageid = " + CStr(Session("SelectedPage")) + " and groupid = " + CStr(Session("SelectedGroup")))
			dgBlkCtrlGrid.DataBind
		End Sub
		#End Region
	End Class