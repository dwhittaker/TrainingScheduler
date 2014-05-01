'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 6/5/2013
' Time: 3:30 PM
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
	''' Description of ImportList
	''' </summary>
	Public Class ImportList
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"

		Protected WithEvents dgImpGrid As DataGrid
		Protected WithEvents txtImport As TextBox
		Protected WithEvents ddlImpType As DropDownList
		Protected WithEvents ddlImpAction As DropDownList
		Protected WithEvents chkImpActive As CheckBox
		Protected WithEvents btnAdd As Button
		Protected WithEvents btnClear As Button

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
			If Not Page.IsPostBack Then
				PageSecurity()
				
				ddlImpAction.Enabled = False
				
				ddlImpType.DataSource = Utility.GetDataView("Select ImportTypeID, ImportType from ImportType")
				ddlImpType.DataValueField = "ImportTypeID"
				ddlImpType.DataTextField = "ImportType"
				ddlImpType.DataBind()
				ddlImpType.Items.Insert(0, New ListItem("None selected", "-1"))
				ddlImpType.SelectedValue = "-1"	
			End If
			
			dgImpGrid.DataSource = GetDataView("Select importid,importname,import.importtypeid,importtype,importaction,active from import join importtype on import.importtypeid = importtype.importtypeid")
			dgImpGrid.DataBind()
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
			AddHandler Me.dgImpGrid.ItemDataBound, New DataGridItemEventHandler(AddressOf _ItemDataBound)
			AddHandler Me.PreRender, New System.EventHandler(AddressOf PreRend)
			AddHandler Me.ddlImpType.SelectedIndexChanged, New System.EventHandler(AddressOf ImpTypeChange)
			'------------------------------------------------------------------
			'------------------------------------------------------------------
		End Sub
		Private Sub _ItemDataBound(sender As Object, e As DataGridItemEventArgs)
			If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
				e.Item.Cells(8).Attributes.Add ( "onClick", "return ConfirmDeletion();")
			End If
		End Sub
		Protected Sub PreRend(sender As Object,e As System.EventArgs)
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
		End Sub
		Protected Sub ImpTypeChange()
			Dim impaction As String
			If ddlImpType.SelectedValue <> "-1" Then
				ddlImpAction.Enabled = True
			End If
			
			impaction = CStr(GetSQLScalar("Select ImportType from ImportType where importtypeid = " + ddlImpType.SelectedValue.ToString))
			
			If impaction <> "Stored Procedure" Then
				ddlImpAction.Items.Clear()
				ddlImpAction.Items.Insert(0, New ListItem("None selected", "-1"))
				ddlImpAction.Items.Insert(1, New ListItem("Insert", "Insert"))
				ddlImpAction.Items.Insert(2, New ListItem("Update", "Update"))
				ddlImpAction.Items.Insert(3, New ListItem("Delete", "Delete"))
				ddlImpAction.SelectedValue = "-1"
			Else
				ddlImpAction.Items.Clear()
				ddlImpAction.DataSource = GetDataView("Select name from sysobjects where type = 'P'")
				ddlImpAction.DataTextField = "name"
				ddlImpAction.DataValueField = "name"
				ddlImpAction.DataBind
				ddlImpAction.Items.Insert(0, New ListItem("None selected", "-1"))
				ddlImpAction.SelectedValue = "-1"
				
			End If

		End Sub
			'------------------------------------------------------------------
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Manipulation"
		Protected Sub Remove_Import(ByVal source As Object,ByVal e As DataGridCommandEventArgs)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("COnnectionString"))
			Dim delcmd As New SqlCommand("DeleteImport",sqlconn)
			If e.CommandName = "Delete" Then
				With delcmd
					.CommandType = CommandType.StoredProcedure
					.Parameters.Add("@ImpID",SqlDbType.Int)
					.Parameters("@ImpID").Value = CInt(e.Item.Cells(0).Text.ToString)
					sqlconn.Open
					.ExecuteNonQuery()
				End With
				delcmd = Nothing
				sqlconn.Close
				ClearData()
				dgImpGrid.DataSource = GetDataView("Select importid,importname,import.importtypeid,importtype,importaction,active from import join importtype on import.importtypeid = importtype.importtypeid")
				dgImpGrid.DataBind()
			End If
		End Sub
		Protected Sub Build_Import(ByVal source As Object, ByVal e As DataGridCommandEventArgs)
			If e.CommandName = "" Then
				Session("ImpID") = e.Item.Cells(0).Text
				Response.Redirect("BuildImport.aspx")
			End If
		End Sub
		Protected Sub Edit_Import(ByVal source As Object, ByVal e As DataGridCommandEventArgs)
			If e.CommandName.ToString = "Edit" Then
				chkImpActive.Enabled = True
				btnAdd.Text = "Update Import"
				txtImport.Text = e.Item.Cells(1).Text.ToString()
				ddlImpType.SelectedValue = e.Item.Cells(3).Text
				ImpTypeChange()
				ddlImpAction.SelectedValue = e.Item.Cells(5).Text
				chkImpActive.Checked = CBool(e.Item.Cells(6).Text)
				Session("ImpID") = e.Item.Cells(0).Text
			End If
		End Sub
		Protected Sub AddEdit_Import(ByVal source As Object, ByVal e As EventArgs) Handles btnAdd.click
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim addcmd As New SqlCommand("AddImport",sqlconn)
			Dim updatecmd As New SqlCommand("UpdateImport",sqlconn)
			If btnAdd.Text = "Add Import" Then				
				With addcmd
					.CommandType = CommandType.StoredProcedure
					.Parameters.Add("@ImpName",SqlDbType.VarChar)
					.Parameters.Add("@ImpType",SqlDbType.Int)
					.Parameters.Add("@ImpAction",SqlDbType.VarChar)
					.Parameters.Add("@Active",SqlDbType.Bit)
					.Parameters("@ImpName").Value = txtImport.Text
					.Parameters("@ImpType").Value = ddlImpType.SelectedValue
					.Parameters("@ImpAction").Value = ddlImpAction.SelectedValue
					.Parameters("@Active").Value = 1
					sqlconn.Open
					.ExecuteNonQuery()
				End With
				addcmd = Nothing
				sqlconn.Close
				ClearData()
			ElseIf btnAdd.Text = "Update Import"
				With updatecmd
					.CommandType = CommandType.StoredProcedure
					.Parameters.Add("@ImpID",SqlDbType.Int)
					.Parameters.Add("@ImpName",SqlDbType.VarChar)
					.Parameters.Add("@ImpType",SqlDbType.Int)
					.Parameters.Add("@ImpAction",SqlDbType.VarChar)
					.Parameters.Add("@Active",SqlDbType.Bit)
					.Parameters("@ImpID").Value = CInt(Session("ImpID").ToString())
					.Parameters("@ImpName").Value = txtImport.Text
					.Parameters("@ImpType").Value = ddlImpType.SelectedValue
					.Parameters("@ImpAction").Value = ddlImpAction.SelectedValue
					If chkImpActive.Checked = True Then
						.Parameters("@Active").Value = 1
					Else
						.Parameters("@Active").Value = 0
					End If
					sqlconn.Open
					.ExecuteNonQuery()
				End With
				updatecmd = Nothing
				sqlconn.Close
				ClearData()
			End If
			dgImpGrid.DataSource = GetDataView("Select importid,importname,import.importtypeid,importtype,importaction,active from import join importtype on import.importtypeid = importtype.importtypeid")
			dgImpGrid.DataBind()
		End Sub
		Protected Sub Clearform(ByVal source As Object, ByVal e As EventArgs) Handles btnClear.click 
			ClearData()
		End Sub
		Protected Sub ClearData()
			txtImport.Text = ""
			ddlImpType.SelectedValue = "-1"
			ddlImpAction.SelectedValue = "-1"
			chkImpActive.Enabled = False
			ddlImpAction.Enabled = False
			btnAdd.Text = "Add Import"
		End Sub
		#End Region
	End Class
