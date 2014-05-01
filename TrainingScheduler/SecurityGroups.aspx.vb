'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 9/9/2013
' Time: 1:56 PM
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
	''' Description of SecurityGroups
	''' </summary>
	Public Class SecurityGroups
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		
		Protected WithEvents txtGroup As TextBox
		Protected WithEvents chkPast As CheckBox
		Protected WithEvents btnAdd As Button
		Protected WithEvents dgGroupGrid As DataGrid

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
			'------------------------------------------------------------------
			If Not IsPostBack Then
				PageSecurity()
				dgGroupGrid.DataSource = GetDataView("Select * from LDAPGroups")
				dgGroupGrid.DataBind
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
			AddHandler Me.dgGroupGrid.ItemDataBound, New DataGridItemEventHandler(AddressOf _ItemDataBound)
			AddHandler Me.PreRender, New System.EventHandler(AddressOf PreRend)
			'------------------------------------------------------------------
			'------------------------------------------------------------------
		End Sub
		Private Sub _ItemDataBound(sender As Object, e As DataGridItemEventArgs)
			If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
				e.Item.Cells(3).Attributes.Add ( "onClick", "return ConfirmDeletion();")
			End If
		End Sub
		Protected Sub PreRend(sender As Object,e As System.EventArgs)
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Manipulate"
		Protected Sub Remove_Group(ByVal source As Object,ByVal e As DataGridCommandEventArgs)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim delcmd As New SqlCommand("DeleteGroup",sqlconn)
			With delcmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@gid",SqlDbType.Int)
				.Parameters("@gid").Value = CInt(e.Item.Cells(0).Text)
				sqlconn.Open
				.ExecuteNonQuery
			End With
			delcmd = Nothing
			sqlconn.Close
			
			ClearForm()
			
			dgGroupGrid.DataSource = GetDataView("Select * from LDAPGroups")
			dgGroupGrid.DataBind
		End Sub
		Protected Sub Add_Group() 
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim addcmd As New SqlCommand("AddGroup",sqlconn)
			With addcmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@gname", SqlDbType.VarChar)
				.Parameters.Add("@pdates", SqlDbType.Bit)
				.Parameters("@gname").Value = txtGroup.Text
				If chkPast.Checked = True Then
					.Parameters("@pdates").Value = 1
				Else
					.Parameters("@pdates").Value = 0
				End If
				sqlconn.Open
				.ExecuteNonQuery
			End With
			addcmd = Nothing
			sqlconn.Close
			
			ClearForm()
			
			dgGroupGrid.DataSource = GetDataView("Select * from LDAPGroups")
			dgGroupGrid.DataBind
		End Sub
		Protected Sub ClearForm()
			txtGroup.Text = ""
			chkPast.Checked = False
		End Sub
		
		#End Region
	End Class
