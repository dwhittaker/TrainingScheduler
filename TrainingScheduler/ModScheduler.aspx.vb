'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 9/11/2013
' Time: 1:47 PM
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
	''' Description of ModScheduler
	''' </summary>
	Public Class ModScheduler
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected WithEvents btnAdd As Button
		Protected WithEvents btnClear As Button
		Protected WithEvents ddlMod As DropDownList
		Protected WithEvents txtDate As TextBox
		Protected WithEvents txtTime As TextBox
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
				ddlMod.DataSource = GetDataView("select ModuleID,ModuleName from modules where active = 1")
				ddlMod.DataValueField = "ModuleID"
				ddlMod.DataTextField = "ModuleName"
				ddlMod.DataBind
				
				ddlMod.Items.Insert(0,New ListItem("None Selected","-1"))
				ddlMod.SelectedValue = "-1"
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
		Protected Sub Sched_Module(ByVal source As Object, ByVal e As EventArgs)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim addcmd As New SqlCommand("ScheduleModule",sqlconn)
			
			With addcmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@mid",SqlDbType.Int)
				.Parameters.Add("@date",SqlDbType.DateTime)
				.Parameters.Add("@time",SqlDbType.DateTime)
				.Parameters.Add("@user",SqlDbType.VarChar)
				.Parameters("@mid").Value = ddlMod.SelectedValue
				.Parameters("@date").Value = CStr(txtDate.Text)
				.Parameters("@time").Value = CStr(txtTime.Text)
				.Parameters("@user").Value = CStr(Session("loginusername"))
				
				sqlconn.Open
				
				.ExecuteNonQuery
			End With
			sqlconn.Close
			addcmd = Nothing
			Response.Write("<script>alert('Module has been scheduled')</script>")
		End Sub
		Protected Sub ClearForm(ByVal source As Object, ByVal e As EventArgs)
			ddlMod.SelectedValue = "-1"
			txtDate.Text = ""
			txtTime.Text = ""
		End Sub
		#End Region
	End Class
