'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 1/24/2013
' Time: 12:26 PM
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
	''' Description of WebForm1
	''' </summary>
	Public Class InsDashboard
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected DVSorted As New DataView
		Protected WithEvents ddlMonth As DropDownList
		Protected WithEvents ddlYear As DropDownList
		Protected WithEvents btnGoToDate As Button
		Protected WithEvents btnForward As Button
		Protected WithEvents btnBack As Button
		Protected WithEvents ddlInstructor As DropDownList 
		Protected strSdate As String
		Protected strEdate As String
		Protected WithEvents dgActionGrid As DataGrid
		Protected strCurIns As String
		Protected WithEvents panByInstructor As Panel
		Protected WithEvents ddlAction As DropDownList
		Protected WithEvents btnSaveUpdate As Button
		Protected WithEvents Stime As TextBox
		Protected WithEvents ETime As TextBox
		Protected WithEvents ADesc As TextBox
		Protected WithEvents ADate As TextBox
		Protected WithEvents ddlDept As DropDownList
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
			Dim StrSQL As String
			Dim DRSQL As SqlDataReader		
			Dim Otheri As Boolean
			Dim test As String
			'------------------------------------------------------------------

			If Not Page.IsPostBack Then
				PageSecurity()
				With ddlMonth.Items
					.Add(New ListItem("January", "1"))
					.Add(New ListItem("February", "2"))
					.Add(New ListItem("March", "3"))
					.Add(New ListItem("April", "4"))
					.Add(New ListItem("May", "5"))
					.Add(New ListItem("June", "6"))
					.Add(New ListItem("July", "7"))
					.Add(New ListItem("August", "8"))
					.Add(New ListItem("September", "9"))
					.Add(New ListItem("October", "10"))
					.Add(New ListItem("November", "11"))
					.Add(New ListItem("December", "12"))
				End With
			
				With ddlYear.Items
					.Add("2016")
					.Add("2015")
					.Add("2014")
					.Add("2013")
					.Add("2012")
					.Add("2011")
					.Add("2010")
					.Add("2009")
					.Add("2008")
					.Add("2007")
					.Add("2006")
					.Add("2004")
					.Add("2003")				
				End With
				ddlMonth.SelectedValue = CDate(Session("dtCalDate")).Month.ToString()
				ddlYear.SelectedValue = CDate(Session("dtCalDate")).Year.ToString()

				Session("InsID") = GetSQLScalar("Select EMP_ID from Employee where Email = '" + Session("Email").ToString() + "'")
				
				test = GetSQLScalar("Select EMP_ID from Employee where Email = '" + Session("Email").ToString() + "'")
'				Session("InsID") = getdatareader("Select EMP_ID from Employee where Email = '" + Session("Email").ToString()+ "'").GetValue(0).ToString()
'		 		
'				StrSQL = "Select EMP_ID,InsName from Instructors where EMP_ID In (Select distinct InstructorID from InsActionCalendar where ActionMonth = " + ddlMonth.SelectedItem.Value.ToString() + "and ActionYear= " + ddlYear.SelectedItem.Value.ToString() + " and InstructorID <> '" + Session("InsID").ToString() + "') Union ALL Select EMP_ID," + "InsName from Instructors where EMP_ID ='" + Session("InsID").ToString() + "'"
'				
'				ddlInstructor.DataSource = GetDataReader(StrSQL)
'				ddlInstructor.DataValueField = "EMP_ID"
'				ddlInstructor.DataTextField = "InsName"
'				ddlInstructor.DataBind
'				ddlInstructor.Items.Insert(0, New ListItem("None selected","-1"))
'				ddlInstructor.SelectedValue = "-1"

				InsFill()
				
				StrSQL = "Select * from Actions"
				ddlAction.DataSource= GetDataView(StrSQL)
				ddlAction.DataValueField = "ActionID"
				ddlAction.DataTextField = "ActionName"
				ddlAction.DataBind
				ddlAction.Items.Insert(0, New ListItem("None selected","-1"))
				ddlAction.SelectedValue="-1"
				
				
				StrSQL = "Select * from Department"
				ddlDept.DataSource = GetDataView(StrSQL)
				ddlDept.DataValueField = "DeptID"
				ddlDept.DataTextField = "DeptName"
				ddlDept.DataBind
				ddlDept.Items.Insert(0, New ListItem("None selected","-1"))
				ddlDept.SelectedValue = "-1"
				
				Otheri = CBool(GetSQLScalar("select otherins from LDAPGroups where GroupID = " + Session("SecurityGroupID").ToString))
				
				If Otheri = False Then
					panByInstructor.Visible = False
				End If
				
				Session("SField") = "Date"
				Session("SOrder") = "Asc"
			End If 
			strCurIns = GetCurrentIns
			ActionFill(strCurIns)
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
			AddHandler Me.dgActionGrid.ItemDataBound, New DataGridItemEventHandler(AddressOf _ItemDataBound)
			AddHandler Me.PreRender,New System.EventHandler(AddressOf PreRend)
		End Sub
		Private Sub _ItemDataBound(sender As Object, e As DataGridItemEventArgs)
			If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
				e.Item.Cells(11).Attributes.Add ( "onClick", "return ConfirmDeletion();")
			End If
		End Sub
		Protected Sub PreRend(sender As Object,e As System.EventArgs)
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
		End Sub
			'------------------------------------------------------------------
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Navigate"
		Protected Sub btnBack_Click(sender As Object, e As System.EventArgs)
		'This procedure sets the date used to fill the table Backwards a month.  If the current month is december then it sets the 
		'date to january of the previous year
			If ddlMonth.SelectedValue = "1" Then
				ddlMonth.SelectedValue = "12"
				ddlYear.SelectedValue = (Integer.Parse(ddlYear.SelectedValue)-1).ToString()
			Else
				ddlMonth.SelectedValue = (Integer.Parse(ddlMonth.SelectedValue)-1).ToString()
			End If
			Session("dtCalDate") = New DateTime(CInt(ddlYear.SelectedValue), CInt(ddlMonth.SelectedValue), 1)
			btnForward.Enabled = True
			ActionFill(strCurIns)
			InsFill()
			
		End Sub
		
		Protected Sub btnForward_Click(sender As Object, e As System.EventArgs) Handles btnForward.Click
		'This procedure sets the date used to fill the table forward a month.  If the current month is december then it sets the date to 
		'january of the next year
			Dim strCDate As DateTime
			Dim strDate As String = strCDate.Now.Month.ToString()
			Dim strYear As String = strCDate.Now.Year.ToString()
			If ddlMonth.SelectedValue = "12" Then
				ddlMonth.SelectedValue = "1"
				ddlYear.SelectedValue = (Integer.Parse(ddlYear.SelectedValue)+1).ToString()
			Else
				ddlMonth.SelectedValue = (Integer.Parse(ddlMonth.SelectedValue)+1).ToString()
			End If
			Session("dtCalDate") = New DateTime(CInt(ddlYear.SelectedValue), CInt(ddlMonth.SelectedValue), 1)
			If strDate = ddlMonth.SelectedValue.ToString And strYear = ddlYear.SelectedValue.ToString() Then
				btnForward.Enabled =False
			End If 
			ActionFill(strCurIns)
			InsFill()
		End Sub
		
		Protected Sub btnGoToDate_Click(s As Object, e As EventArgs) Handles btnGoToDate.Click
			Dim strCDate As DateTime
			Dim strDate As String = strCDate.Now.Month.ToString()
			Dim strYear As String = strCDate.Now.Year.ToString()
			Session("dtCalDate") = New DateTime(CInt(ddlYear.SelectedValue), CInt(ddlMonth.SelectedValue), 1)
			If strDate = ddlMonth.SelectedValue.ToString And strYear = ddlYear.SelectedValue.ToString() Then
				btnForward.Enabled =False
			End If 
			ActionFill(strCurIns)
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Fill"
		Private Sub ActionFill(strIns As String)
			Dim strcmd As String
			Dim stryear As Integer
			Dim Otheri As Boolean
			'Dim DRSQL As SqlDataReader	
'			strSdate = ddlMonth.SelectedValue & "/" & "01" & "/" & ddlYear.SelectedValue
'			strEdate = ddlMonth.SelectedValue & "/" & System.DateTime.DaysInMonth(Integer.Parse(ddlYear.SelectedValue), Integer.Parse(ddlMonth.SelectedValue)) & "/" & ddlYear.SelectedValue
			stryear = CInt(ddlYear.SelectedValue.ToString())

			Otheri = CBool(GetSQLScalar("select otherins from LDAPGroups where GroupID = " + Session("SecurityGroupID").ToString))

			If ddlInstructor.SelectedValue = "-1" And Otheri = True Then
				strIns = "%"
			End If
			strcmd = "Select * from InsActionCalendar where InstructorID like '" + strIns + "' and ActionMonth = " + ddlMonth.SelectedValue.ToString() + " and ActionYear = " + ddlYear.SelectedValue.ToString()
			DVSorted = GetDataView(strCmd)
			DVSorted.Sort = Session("SField").ToString + " " + Session("SOrder").ToString
			dgActionGrid.DataSource = DVSorted
			dgActionGrid.DataBind()
		End Sub
		
		Private Function GetCurrentIns As String
			If ddlInstructor.SelectedValue <> "-1" Then
				GetCurrentIns = ddlInstructor.Selectedvalue.ToString()	
			Else
				GetCurrentIns = Session("InsID").ToString()
			End If
		End Function
		Public Sub InsFill()
			Dim StrSQL As String
			If Not Session("InsID") Is Nothing Then
				StrSQL = "Select EMP_ID,InsName from Instructors where EMP_ID In (Select distinct InstructorID from InsActionCalendar where ActionMonth = " + ddlMonth.SelectedItem.Value.ToString() + "and ActionYear= " + ddlYear.SelectedItem.Value.ToString() + " and InstructorID <> '" + Session("InsID").ToString() + "') Union ALL Select EMP_ID," + "InsName from Instructors where EMP_ID ='" + Session("InsID").ToString() + "'"	
			Else
				StrSQL = "Select EMP_ID,InsName from Instructors where EMP_ID In (Select distinct InstructorID from InsActionCalendar where ActionMonth = " + ddlMonth.SelectedItem.Value.ToString() + "and ActionYear= " + ddlYear.SelectedItem.Value.ToString() + ")"
			End If
			
				
			ddlInstructor.DataSource = GetDataView(StrSQL)
			ddlInstructor.DataValueField = "EMP_ID"
			ddlInstructor.DataTextField = "InsName"
			ddlInstructor.DataBind
			ddlInstructor.Items.Insert(0, New ListItem("None selected","-1"))
			ddlInstructor.SelectedValue = "-1"
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Manipulation"
		Protected Sub SortData(ByVal source As Object, ByVal e As DataGridSortCommandEventArgs)
			If Session("SOrder").ToString = "Asc" Then
				Session("SOrder") = "Desc"
			Else If Session("SOrder").ToString = "Desc" Then
				Session("SOrder") = "Asc"
			Else
				Session("SOrder") = "Asc"
			End If
			Session("SField") = e.SortExpression.ToString
			ActionFill(strCurIns)
		End Sub
		Protected Sub UpdateAction()
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim updatecmd As New SqlCommand("UpdateInsAction",sqlconn)	

			With updatecmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@aid",SqlDbType.VarChar)
				.Parameters.Add("@stime",SqlDbType.DateTime)
				.Parameters.Add("@etime",SqlDbType.DateTime)
				.Parameters.Add("@Comments",SqlDbType.VarChar)
				.Parameters.Add("@Date",SqlDbType.DateTime)
				.Parameters.Add("@ModifiedBy",SqlDbType.VarChar)
				.Parameters.Add("@Insactionid",SqlDbType.VarChar)
				.Parameters.Add("@Dept",SqlDbType.Int)
				.Parameters("@aid").Value = ddlAction.SelectedItem.Value.ToString()
				.Parameters("@stime").Value = Stime.Text.ToString.Trim()
				.Parameters("@etime").Value = ETime.Text.ToString.Trim()
				.Parameters("@Comments").Value = ADesc.Text.ToString()
				.Parameters("@Date").Value = ADate.Text.ToString()
				.Parameters("@ModifiedBy").Value =  Session("loginusername")
				.Parameters("@Insactionid").Value = Session("SelAction")
				If CDbl(ddlDept.Selectedvalue) = -1 Then
					.Parameters("@Dept").Value = DBNull.Value
				Else
					.Parameters("@Dept").Value = ddlDept.SelectedValue
				End If
				sqlconn.Open
				.ExecuteNonQuery
			End With
			updatecmd = Nothing
			sqlconn.Close
			ActionFill(strCurIns)
			ClearForm()
			
		End Sub
		
		Protected Sub Action_Click(ByVal source As Object, ByVal e As DataGridCommandEventArgs)
			Dim strtest As String
			If e.CommandName <> "Delete" Then
				btnSaveUpdate.Text = "Update"
			End If		
			If e.CommandName.ToString = "" Then
				Session("SelAction") = e.Item.Cells(0).Text
			
				For i As Integer = 0 To ddlAction.Items.Count - 1
					If ddlAction.Items(i).Value.ToString = e.Item.Cells(3).Text Then
						ddlAction.SelectedIndex = i
						Exit For 
					Else
						ddlAction.SelectedIndex = -1
					End If						
				Next
			
				ADesc.Text = e.Item.Cells(9).Text
			
				If e.Item.Cells(5).Text.ToString = "&nbsp;" Or e.Item.Cells(5).Text.ToString = ""  Then
					ddlDept.SelectedValue = -1
				Else
					strtest = e.Item.Cells(5).Text.ToString()
					ddlDept.SelectedValue = e.Item.Cells(5).Text
				End If
				
				Stime.Text = e.Item.Cells(7).Text
			
				ETime.Text = e.Item.cells(8).Text
			
				ADate.Text = e.Item.Cells(10).Text
			End If
		End Sub
			 
		
		Protected Sub Remove_Action(ByVal source As Object, ByVal e As DataGridCommandEventArgs)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim delcmd As New SqlCommand("DeleteAction",sqlconn)
			With delcmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@aid",SqlDbType.VarChar)
				.Parameters("@aid").Value = e.Item.Cells(0).Text
				sqlconn.Open
				.ExecuteNonQuery
			End With
			delcmd = Nothing
			sqlconn.Close
			ActionFill(strCurIns)
		End Sub
		Protected Sub btnSaveUpdate_Click(sender As Object, e As EventArgs) Handles btnSaveUpdate.click
			If btnSaveUpdate.Text = "Save" Then
					Add_Action()
			ElseIf btnSaveUpdate.Text = "Update" Then
					UpdateAction()
			End If
		End Sub
		Protected Sub Add_Action()
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim addcmd As New SqlCommand("AddInsActions",sqlconn)		
			With addcmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@insid",SqlDbType.VarChar)
				.Parameters.Add("@aid",SqlDbType.VarChar)
				.Parameters.Add("@stime",SqlDbType.DateTime)
				.Parameters.Add("@etime",SqlDbType.DateTime)
				.Parameters.Add("@Comments",SqlDbType.VarChar)
				.Parameters.Add("@Date",SqlDbType.DateTime)
				.Parameters.Add("@ModifiedBy",SqlDbType.VarChar)
				.Parameters.Add("@Dept",SqlDbType.Int)
				.Parameters("@insid").Value = Session("InsID").ToString()
				.Parameters("@aid").Value = ddlAction.SelectedItem.Value.ToString()
				.Parameters("@stime").Value = Stime.Text.ToString.Trim()
				.Parameters("@etime").Value = ETime.Text.ToString.Trim()
				.Parameters("@Comments").Value = ADesc.Text.ToString()
				.Parameters("@Date").Value = ADate.Text.ToString()
				.Parameters("@ModifiedBy").Value =  Session("loginusername")
				If CDbl(ddlDept.Selectedvalue) = -1 Then
					.Parameters("@Dept").Value = DBNull.Value
				Else
					.Parameters("@Dept").Value = ddlDept.SelectedValue
				End If
				sqlconn.Open
				.ExecuteNonQuery
			End With
			addcmd = Nothing
			sqlconn.Close
			ActionFill(Session("InsID").ToString())
			ClearForm()
		End Sub
		Protected Sub ClearForm()
			ddlAction.SelectedValue = "-1"
			ddlDept.SelectedIndex = CInt("-1")
			Stime.Text = ""
			ETime.Text = ""
			ADesc.Text = ""
			ADate.Text = ""
			btnSaveUpdate.Text = "Save"
		End Sub
		Protected Sub ddlInstructor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlInstructor.SelectedIndexChanged
			strCurIns = GetCurrentIns()
			ActionFill(strCurIns)
		End Sub
		#End Region
	End Class
