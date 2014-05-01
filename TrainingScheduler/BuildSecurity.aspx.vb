'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 9/9/2013
' Time: 5:39 PM
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
	''' Description of BuildSecurity
	''' </summary>
	Public Class BuildSecurity
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected WithEvents dgPageGrid As DataGrid
		Protected WithEvents btnCheckAll As Button
		Protected btnUpdate As Button
		Protected WithEvents chkSelection As CheckBox
		Protected WIthEvents ddlGroup As DropDownList
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
				Session("ClassCheck") = "Check"
				btnCheckAll.Text = Session("ClassCheck").ToString & " All"
				
				ddlGroup.DataSource = GetDataView("Select GroupID,LDAPGroupName from LDAPGroups")
				ddlGroup.DataValueField = "GroupID"
				ddlGroup.DataTextField = "LDAPGroupName"
				ddlGroup.DataBind
				
				ddlGroup.Items.Insert(0, New ListItem("None Selected","-1"))
				ddlGroup.SelectedValue = "-1"
				
				Datafill()
				
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
			AddHandler Me.ddlGroup.SelectedIndexChanged, New System.EventHandler(AddressOf GroupChange)
			AddHandler Me.PreRender, New System.EventHandler(AddressOf PreRend)
			'------------------------------------------------------------------
		End Sub
		Protected Sub PreRend(sender As Object,e As System.EventArgs)
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Fill"
		Protected Sub CheckAll()
			If Session("ClassCheck").ToString = "Check" then
				For each i As DataGridItem In dgPageGrid.Items
					chkSelection = Ctype(i.FindControl("chkSelection"), CheckBox)
					chkSelection.Checked = True
				Next i
				Session("ClassCheck") = "Un-Check"
				btnCheckAll.Text = Session("ClassCheck").ToString & " All"
			Else If Session("ClassCheck").ToString = "Un-Check"
				For Each i As DataGridItem In dgPageGrid.Items
					chkSelection = CType(i.FindControl("chkSelection"), CheckBox)
					chkSelection.Checked = False
				Next i
				Session("ClassCheck") = "Check"
				btnCheckAll.Text = Session("ClassCheck").ToString & " All"
			End If
		End Sub
		Protected Sub GroupChange(ByVal sender As Object,ByVal e As System.EventArgs)
			Datafill()
		End Sub
		Protected Sub Datafill()
			dgPageGrid.DataSource = GetDataView("Select * from v_GroupPageAccess where groupid = " + CStr(ddlGroup.SelectedValue.ToString) + "order by Title asc")
			dgPageGrid.DataBind
			
			For Each e As DataGridItem In dgPageGrid.Items
				If e.Cells(1).Text <> "&nbsp;"  And ddlGroup.SelectedValue <> "-1" Then
					chkSelection = CType(e.FindControl("chkSelection"),CheckBox)
					If chkSelection isnot Nothing Then
						chkSelection.Checked = True
					End If
				End If
			Next
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Manipulaton"
		Protected Sub UpdateSecurity(sender As Object, e As EventArgs)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim addcmd As New SqlCommand("AddPageAccess",sqlconn)
			Dim delcmd As New SqlCommand("DeletePageAccess",sqlconn)
			Dim f As Integer
			
			f = 0
			
			With addcmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@gid",SqlDbType.Int)
				.Parameters.Add("@pid",SqlDbType.Int)
			End With
			With delcmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@paid",SqlDbType.Int)
			End With
			
			sqlconn.Open
			
			For Each i As DataGridItem In dgPageGrid.Items
				dgPageGrid.SelectedIndex = f
				chkSelection = CType(i.FindControl("chkSelection"), CheckBox)
				If chkSelection.Checked = True And dgPageGrid.SelectedItem.Cells(1).Text = "&nbsp;" Then
					With addcmd
						.Parameters("@gid").Value = CInt(dgPageGrid.SelectedItem.Cells(2).Text)
						.Parameters("@pid").Value = CInt(dgPageGrid.SelectedItem.Cells(3).Text)
						.ExecuteNonQuery
					End With
				ElseIf chkSelection.Checked = False And dgPageGrid.SelectedItem.Cells(1).Text <> "&nbsp;" Then
					With delcmd
						.Parameters("@paid").Value = CInt(dgPageGrid.SelectedItem.Cells(1).Text)
						.ExecuteNonQuery
					End With
				End If
				f = f + 1
			Next
			addcmd = Nothing
			delcmd = Nothing
			sqlconn.Close
			Datafill
		End Sub
		Protected Sub BlockControls(ByVal source As Object, ByVal e As DataGridCommandEventArgs)
			Session("SelectedGroup") = e.Item.Cells(2).Text
			Session("SelectedPage") = e.Item.Cells(3).Text
			Response.Redirect("BlockControls.aspx")
		End Sub
		#End Region
	End Class