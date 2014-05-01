'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 9/14/2012
' Time: 2:05 PM
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
	''' Description of ClassAttributes
	''' </summary>
	Public Class ClassAttributes
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected ddlCourse As DropDownList
		Protected ddlAtt As DropDownList
		Protected dgAttGrid As DataGrid
		Protected sqlconn As New SqlConnection(System.Configuration.ConfigurationSettings.AppSettings("ConnectionString"))
		Protected txtAttDate As TextBox
		Protected cwcClass As ddlSearch
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
				
				ddlAtt.DataSource = GetDataView("select AttributeID, AttributeName from Attributes order by attributename asc")
				ddlAtt.DataValueField = "AttributeID"
				ddlAtt.DataTextField = "AttributeName"
				ddlAtt.DataBind()
				ddlAtt.Items.Insert(0, New ListItem("None selected", "-1"))
				ddlAtt.SelectedValue = "-1"
				
				cwcClass.ConWidth = 380
				cwcClass.CtrlDataSource = "select TrainingCourseID, CourseTitle from TrainingCourse order by coursetitle asc"
				cwcClass.CtrlVField = "TrainingCourseID"
				cwcClass.CtrlTField = "CourseTitle"
				cwcClass.CtrlBind()
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
			AddHandler Me.dgAttGrid.ItemDataBound, New DataGridItemEventHandler(AddressOf _ItemDataBound)
			AddHandler Me.PreRender, New System.EventHandler(AddressOf PreRend)
			AddHandler Me.cwcClass.IndChange, New ddlSearch.IndChangeEventHandler(AddressOf Course_Change)
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
		#Region "Data Fill"
		Protected Sub Att_Fill(strCourse As String)
			dgAttGrid.Datasource = GetDataView("select ClassAttributeID, AttributeName,AsofDate from ClassAttributes Join Attributes on Attributes.AttributeID = ClassAttributes.AttributeID where trainingcourseid = " & CInt(strCourse))
			dgAttGrid.DataBind	
		End Sub
		Protected Sub Course_Change(ByVal sender As Object, ByVal e As System.EventArgs)
			If cwcClass.CtrlSelValue <> "-1" Then
				ddlAtt.Enabled = True
				txtAttDate.Enabled = True
				Att_Fill(cwcClass.CtrlSelValue)
			Else
				ddlAtt.SelectedValue = "-1"
				ddlAtt.Enabled = False
				txtAttDate.Enabled = False
			End If
		End Sub
		#End Region	
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Manipulate"
		Protected Sub Remove_Att(ByVal source As Object, ByVal e As DataGridCommandEventArgs)
			Dim deletecmd As New SqlCommand("RemoveClassAttribute",sqlconn)
			With deletecmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@caid",SqlDbType.Int)
				.Parameters("@caid").Value = CInt(e.Item.Cells(0).Text)
				sqlconn.Open
				.ExecuteNonQuery()
			End With
			deletecmd = Nothing
			sqlconn.Close
			Att_Fill(cwcClass.CtrlSelValue)
		End Sub
		Protected Sub Add_Att
			Dim addcmd As New SqlCommand("AddClassAttribute",sqlconn)
			With addcmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@cid",SqlDbType.Int)
				.Parameters.Add("@aid",SqlDbType.Int)
				.Parameters.Add("@adate",SqlDbType.DateTime)
				.Parameters("@cid").Value = CInt(cwcClass.CtrlSelValue)
				.Parameters("@aid").Value = CInt(ddlAtt.SelectedItem.Value)
				.Parameters("@adate").Value = CDate(txtAttDate.Text)
				sqlconn.Open
				.ExecuteNonQuery()
			End With
			addcmd = Nothing
			sqlconn.Close
			Att_Fill(cwcClass.CtrlSelValue)
		End Sub
		#End Region
	End Class
