'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 8/29/2012
' Time: 11:53 AM
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
	''' Description of Exception
	''' </summary>
	Public Class RecertEx
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected WithEvents cwcEmp As ddlSearch
		Protected dgExGrid As System.Web.UI.WebControls.DataGrid
		Protected txtRea As System.Web.UI.WebControls.TextBox
		Protected txtSDate As System.Web.UI.WebControls.TextBox
		Protected txtEDate As System.Web.UI.WebControls.TextBox
		Protected lblExID As HiddenField
		Protected sqlconn As New SqlConnection(System.Configuration.ConfigurationSettings.AppSettings("ConnectionString"))
		Protected WithEvents btnAddNew As Button 
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
			PageSecurity()
			If IsPostBack = False Then
				cwcEmp.ConWidth = 380
				cwcEmp.CtrlDataSource = "select emp_id,last_name +  ', ' + first_name as Name from Employee order by last_name"
				cwcEmp.CtrlVField =  "emp_id"
				cwcEmp.CtrlTField = "Name"
				cwcEmp.CtrlBind()
			End If
			ExGridFill("select exceptionid,emp_id,Last_Name,First_name,StartDate,EndDate,Reason from v_Exceptions order by cast(enddate as datetime) desc")
			cwcEmp.Attributes.Add("SQLField","EMP_ID")
			txtSDate.Attributes.Add("SQLField","StartDate")
			txtEDate.Attributes.Add("SQLField","EndDate")
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
			AddHandler Me.dgExGrid.ItemDataBound, New DataGridItemEventHandler(AddressOf _ItemDataBound)
			AddHandler Me.PreRender, New System.EventHandler(AddressOf PreRend)
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
		
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Fill"
		Protected Sub ListFill()

		End Sub
		Protected Sub ExGridFill(strCmd As String)
			'Fills grid 
			dgExGrid.DataSource = GetDataView(strCmd)
			dgExGrid.DataBind
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Manipulate"
		Protected Sub Remove_Ex(ByVal source As Object, ByVal e As DataGridCommandEventArgs)
			Dim deleteex As New SqlCommand("DeleteException",sqlconn)
			With deleteex
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@exID",SqlDbType.Int)
				.Parameters("@exID").Value = e.Item.Cells(0).Text
				sqlconn.Open
				.ExecuteNonQuery
			End With
			sqlconn.Close
			deleteex = Nothing
			ExGridFill("select exceptionid,emp_id,Last_Name,First_name,StartDate,EndDate,Reason from v_Exceptions order by cast(enddate as datetime) desc")
		End Sub
		Protected Sub AddEx()
			If Page.IsValid Then
				If btnAddNew.Text = "Update" Then 
					Dim updateex As New SqlCommand("UpdateException",sqlconn)
					With updateex
						.CommandType = CommandType.StoredProcedure
						.Parameters.Add("@exid",SqlDbType.Int)
						.Parameters.Add("@empID",SqlDbType.NVarChar)
						.Parameters.Add("@sdate",SqlDbType.DateTime)
						.Parameters.Add("@edate",SqlDbType.DateTime)
						.Parameters.Add("@reason",SqlDbType.NVarChar)
						.Parameters("@exid").Value = lblExID.Value.ToString()
						.Parameters("@empID").Value = cwcEmp.CtrlSelValue.ToString
						.Parameters("@sdate").Value = txtSDate.Text.ToString
						.Parameters("@edate").Value = txtEDate.Text.ToString
						.Parameters("@reason").Value = txtRea.Text.ToString
						sqlconn.Open
						.ExecuteNonQuery
					End With
					sqlconn.Close
					updateex = Nothing
					ExGridFill("select exceptionid,emp_id,Last_Name,First_name,StartDate,EndDate,Reason from v_Exceptions order by cast(enddate as datetime) desc")
					ClearFrm()
				Else
					Dim insertex As New SqlCommand("InsertException",sqlconn)
					With insertex
						.CommandType = CommandType.StoredProcedure
						.Parameters.Add("@empID",SqlDbType.NVarChar)
						.Parameters.Add("@sdate",SqlDbType.DateTime)
						.Parameters.Add("@edate",SqlDbType.DateTime)
						.Parameters.Add("@reason",SqlDbType.NVarChar)
						.Parameters("@empID").Value = cwcEmp.CtrlSelValue.ToString
						.Parameters("@sdate").Value = txtSDate.Text.ToString
						.Parameters("@edate").Value = txtEDate.Text.ToString
						.Parameters("@reason").Value = txtRea.Text.ToString
						sqlconn.Open
						.ExecuteNonQuery
					End With
					sqlconn.Close
					insertex = Nothing
					ExGridFill("select exceptionid,emp_id,Last_Name,First_name,StartDate,EndDate,Reason from v_Exceptions order by cast(enddate as datetime) desc")
				End if
			End If
		End Sub
		Protected Sub Update_Ex(ByVal source As Object, ByVal e As DataGridCommandEventArgs)
			lblExID.Value = e.Item.Cells(0).Text
			txtRea.Text = e.Item.Cells(7).Text
			txtSDate.Text = e.Item.Cells(5).Text
			txtEDate.Text = e.Item.Cells(6).Text
			cwcEmp.CtrlSelValue = e.Item.Cells(1).Text
			btnAddNew.Text = "Update"
		End Sub
		Protected Sub FilterEx()
			Dim ctrlcoll As ControlCollection
			Dim txtData As System.Web.UI.WebControls.TextBox
			Dim ddlData As ddlSearch
			Dim strCmd As String
			Dim test As String
			strCmd = "select exceptionid,emp_id,Last_Name,First_name,StartDate,EndDate,Reason from v_Exceptions where "
			ctrlcoll = Nothing
			For Each c As Control In Form.Controls
				If c.GetType.ToString = "System.Web.UI.WebControls.ContentPlaceHolder" And c.ID = "Main" Then
					ctrlcoll = c.Controls
				End If
			Next
			If ctrlcoll IsNot Nothing Then
				For Each ctrl As Control In ctrlcoll
					test = ctrl.GetType.ToString
					If ctrl.GetType.ToString = "System.Web.UI.WebControls.TextBox" Then
						txtData = CType(ctrl,TextBox)
						If txtData.Text <> "" Then
							strCmd = strCmd & txtData.Attributes("SQLField").ToString & " = '" & txtData.Text.ToString & "' AND "
						End If
					Else If ctrl.GetType.ToString = "ASP.ddlsearch_ascx"
						ddlData = CType(ctrl,ddlSearch)
						If ddlData.CtrlSelValue <> "-1"
							strCmd = strCmd & ddlData.Attributes("SQLField").ToString & " = " & ddlData.CtrlSelValue & " AND "
						End If
					End If
				Next
				strCmd = strCmd.ToString.Substring(0,strCmd.Length - 5)
				ExGridFill(strCmd + " order by cast(enddate as datetime) desc")
			End If
		End Sub
		Protected Sub ClearFrm()
			cwcEmp.CtrlSelValue = "-1"
			txtRea.Text = ""
			txtSDate.Text = ""
			txtEDate.Text = ""
			btnAddNew.Text = "Add New Exception"
			lblExID.Value = ""
			ExGridFill("select exceptionid,emp_id,Last_Name,First_name,StartDate,EndDate,Reason from v_Exceptions order by cast(enddate as datetime) desc")
		End Sub
		#End Region
	End Class
