'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 8/9/2013
' Time: 9:54 AM
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
	''' Description of Modules
	''' </summary>
	Public Class Modules
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected WithEvents dgModGrid As DataGrid
		Protected WithEvents txtModule As TextBox
		Protected WithEvents chkModActive As CheckBox
		Protected WithEvents btnAdd As Button
		Protected WithEvents btnClear As Button
		Protected strCrit As String
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
			End If
			'strCrit = "where (enddate >= GetDate() or enddate is null)"
			dgModGrid.DataSource = GetDataView("Select * from Modules" + strCrit)
			dgModGrid.DataBind()
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
			AddHandler Me.dgModGrid.ItemDataBound, New DataGridItemEventHandler(AddressOf _ItemDataBound)
			AddHandler Me.PreRender, New System.EventHandler(AddressOf PreRend)
			'------------------------------------------------------------------
			'------------------------------------------------------------------
		End Sub
		Private Sub _ItemDataBound(sender As Object, e As DataGridItemEventArgs)
			If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
				e.Item.Cells(5).Attributes.Add ( "onClick", "return ConfirmDeletion();")
			End If
		End Sub
		Protected Sub PreRend(sender As Object,e As System.EventArgs)
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Maniuplation"
		Protected Sub Remove_Module(ByVal source As Object,ByVal e As DataGridCommandEventArgs)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim delcmd As New SqlCommand("DeleteModule",sqlconn)
			Dim mods As Integer
			
			mods = GetSQLScalar("Select count(moduleid) from employeemodule where moduleid = " + CStr(e.Item.Cells(0).Text))
			If mods = 0 Then
				If e.CommandName = "Delete"
					With delcmd
						.CommandType = CommandType.StoredProcedure
						.Parameters.Add("@mid", SqlDbType.Int)
						.Parameters("@mid").Value = CInt(e.Item.Cells(0).Text)
						sqlconn.Open
						.ExecuteNonQuery
					End With
					delcmd = Nothing
					sqlconn.Close
					dgModGrid.DataSource = GetDataView("Select * from Modules")
					dgModGrid.DataBind()
				End If
			Else
				Response.Write("<script>alert('Module has been assigned to employees and can not be deleted')</script>")
			End If

		End Sub	
		Protected Sub Edit_Module(ByVal source As Object,ByVal e As DataGridCommandEventArgs)
			btnAdd.Text = "Update"
			If e.CommandName = "Edit" Then
				txtModule.Text = e.Item.Cells(1).Text
				chkModActive.Checked = CBool(e.Item.Cells(3).Text)
				Session("mid") = CInt(e.Item.Cells(0).Text)
				chkModActive.Enabled = True
			End If
		End Sub	
		Protected Sub Build_Module(ByVal source As Object,ByVal e As DataGridCommandEventArgs)
			If e.CommandName = "" Then
				If PageAccess("/TrainingScheduler/BuildModule.aspx",CInt(Session("SecurityGroupID"))) = True Then
					Session("mid") = e.Item.Cells(0).Text
					Response.Redirect("BuildModule.aspx")
				Else
					Response.Write("<script>alert('You do not have rights to view that screen')</script>")
				End If

			End If
		End Sub
		Protected Sub AddEdit_Module(ByVal source As Object, ByVal e As EventArgs) Handles btnAdd.Click
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("COnnectionString"))
			Dim addcmd As New SqlCommand("AddModule",sqlconn)
			Dim upcmd As New SqlCommand("UpdateModule",sqlconn)
			If btnAdd.Text = "Add Module" Then
				With addcmd 
					.CommandType = CommandType.StoredProcedure
					.Parameters.Add("@mname",SqlDbType.VarChar)
					.Parameters("@mname").Value = txtModule.Text
					sqlconn.Open
					.ExecuteNonQuery
				End With
				addcmd = Nothing
				sqlconn.Close
			Else
				With upcmd
					.CommandType = CommandType.StoredProcedure
					.Parameters.Add("@mid",SqlDbType.Int)
					.Parameters.Add("@mname",SqlDbType.VarChar)
					.Parameters.Add("@active",SqlDbType.Bit)
					.Parameters("@mid").Value = CInt(Session("mid"))
					.Parameters("@mname").Value = txtModule.Text
					If chkModActive.Checked = True Then
						.Parameters("@active").Value = 1
					Else
						.Parameters("@active").Value = 0
					End If
					sqlconn.Open
					.ExecuteNonQuery
				End With
				upcmd = Nothing
				sqlconn.Close
				ClearForm()
			End If
			dgModGrid.DataSource = GetDataView("Select * from Modules")
			dgModGrid.DataBind()
		End Sub
		Protected Sub ClearForm() Handles btnClear.Click
			btnAdd.Text = "Add Module"
			txtModule.Text = ""
			chkModActive.Checked = 1
			chkModActive.Enabled = False
		End Sub
		#End Region
	End Class
