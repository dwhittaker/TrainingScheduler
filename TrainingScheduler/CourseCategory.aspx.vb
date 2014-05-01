'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 3/26/2014
' Time: 10:34 AM
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
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
	''' Description of CourseCategory
	''' </summary>
	Public Class CourseCategory
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected WithEvents txtCatName As TextBox
		Protected WithEvents chkRecert As CheckBox
		Protected WithEvents btnCreate As Button
		Protected WithEvents btnCancel As Button
		Protected WithEvents dgCatGrid As DataGrid
		Protected WithEvents PanAddEdit As Panel
		Protected WithEvents btnSaveEdit As Button
		Protected WithEvents ddlReport As DropDownList
		Protected WithEvents UpPan3 As UpdatePanel
		Protected WithEvents UpPan4 As UpdatePanel
		Protected WithEvents PanCreate As Panel
		Protected WithEvents UpPan5 As UpdatePanel
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
				Datafill()
				PageSecurity()
			End If
			'------------------------------------------------------------------
			
			ddlReport.DataSource = GetDataView("Select RgID, ReportGroup from ReportGroups")
			ddlReport.DataValueField = "RgID"
			ddlReport.DataTextField = "ReportGroup"
			ddlReport.DataBind()
			ddlReport.Items.Insert(0,New ListItem("----None Selected----","-1"))
			ddlReport.SelectedValue = "-1"
			
			ScriptManager.GetCurrent(Me.Page).RegisterAsyncPostBackControl(btnSaveEdit)
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
			Dim test As String
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Manipulation"
		Protected Sub CreateCat(sender As Object, e As System.EventArgs) Handles btnCreate.Click
			PanAddEdit.Visible = True
			UpPan3.Update()
			PanCreate.Visible = False
			UpPan5.Update()
			btnSaveEdit.Text = "Save Category"
			PanCreate.Visible = False
		End Sub
		Protected Sub SaveEdit_Click(sender As Object, e As System.EventArgs) Handles btnSaveEdit.Click
			Datafill()
			Dim test As String
			test = PanAddEdit.Visible.ToString()
			test = PanCreate.Visible.ToString()
			PanAddEdit.Visible = False
			UpPan3.Update()
			PanCreate.Visible = True
			UpPan5.Update()
		End Sub
		Protected Sub Cancel(sender As Object, e As System.EventArgs) Handles btnCancel.Click
			ClearFrm()
			PanAddEdit.Visible = False
			UpPan3.Update()
			PanCreate.Visible = True
			UpPan5.Update()
		End Sub
		Public Sub ClearFrm()
			
		End Sub
		Public Sub Datafill()
			dgCatGrid.DataSource = GetDataView("Select * from v_CourseCat")
			dgCatGrid.DataBind()
		End Sub
		#End Region
	End Class