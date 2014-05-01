'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 10/23/2012
' Time: 3:32 PM
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
	''' Description of CourseInstructor
	''' </summary>
	Public Class CourseInstructor
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected withevents btnAdd As Button
		Protected InsPanel As Panel
		Protected Fname As TextBox
		Protected Lname As TextBox
		Protected InsID As HiddenField
		Protected WithEvents btnSaveUpdate As Button
		Protected WithEvents btnCancel As Button
		Protected WithEvents dgInsGrid As DataGrid
		Protected sqlconn As New SqlConnection(System.Configuration.ConfigurationSettings.AppSettings("ConnectionString"))
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
			End If
		dgInsGrid.Datasource = GetDataView("select CourseInstructorID, FirstName, LastName from CourseInstructor")
		dgInsGrid.DataBind	
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
		Public Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
			InsPanel.Visible = True
			btnSaveUpdate.Text = "Save"
		End Sub
		Sub eventHandlerName(sender As Object, e As DataGridCommandEventArgs) Handles dgInsGrid.ItemCommand
			If e.CommandName = "Edit" Then
				InsPanel.Visible = True
				btnSaveUpdate.Text = "Update"
				InsID.Value = e.Item.Cells(0).Text
				Fname.Text = e.Item.Cells(1).Text
				Lname.Text = e.Item.Cells(2).Text
			End If
		End Sub
		Public Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
			Fname.Text = ""
			Lname.Text = ""
			InsID.Value = ""
			InsPanel.Visible = False
		End Sub
		Public Sub btnSaveUpdate_Click(sender As Object, e As EventArgs) Handles btnSaveUpdate.Click
			Dim savecmd As New SqlCommand("AddIns",sqlconn)
			Dim updatecmd As New SqlCommand("UpdateIns",sqlconn)
			If btnSaveUpdate.Text = "Save" Then
				With savecmd
					.CommandType = CommandType.StoredProcedure
					.Parameters.Add("@Fname",SqlDbType.VarChar)
					.Parameters.Add("@Lname",SqlDbType.VarChar)
					.Parameters("@Fname").Value = Fname.Text.ToString()
					.Parameters("@Lname").Value = Lname.Text.ToString()
					sqlconn.Open
					.ExecuteNonQuery
				End With
			Else If btnSaveUpdate.Text = "Update" Then
				With updatecmd
					.CommandType = CommandType.StoredProcedure
					.Parameters.Add("@insID",SqlDbType.VarChar)
					.Parameters.Add("@Fname",SqlDbType.VarChar)
					.Parameters.Add("@Lname",SqlDbType.VarChar)
					.Parameters("@insID").Value = InsID.Value.ToString()
					.Parameters("@Fname").Value = Fname.Text.ToString()
					.Parameters("@Lname").Value = Lname.Text.ToString()
					sqlconn.Open
					.ExecuteNonQuery
				End With
			End If
			savecmd = Nothing
			updatecmd = Nothing
			sqlconn.Close
			dgInsGrid.Datasource = GetDataView("select CourseInstructorID, FirstName, LastName from CourseInstructor")
			dgInsGrid.DataBind	
			Fname.Text = ""
			Lname.Text = ""
			InsID.Value = ""
			InsPanel.Visible = False
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
			AddHandler Me.dgInsGrid.ItemDataBound, New DataGridItemEventHandler(AddressOf _ItemDataBound)
			AddHandler Me.PreRender, New System.EventHandler(AddressOf PreRend)
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
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Manipulation"
		Public Sub Remove_Ins(ByVal source As Object, ByVal e As DataGridCommandEventArgs)
			Dim deletecmd As New SqlCommand("RemoveIns",sqlconn)
			With deletecmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@insID",SqlDbType.VarChar)
				.Parameters("@insID").Value = e.Item.Cells(0).Text.ToString
				sqlconn.Open
				.ExecuteNonQuery
			End With
			deletecmd = Nothing
			sqlconn.Close
			dgInsGrid.Datasource = GetDataView("select CourseInstructorID, FirstName, LastName from CourseInstructor")
			dgInsGrid.DataBind	
		End Sub
		#End Region
	End Class
