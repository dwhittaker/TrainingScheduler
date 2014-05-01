'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 7/16/2013
' Time: 2:18 PM
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
Imports System.IO
	''' <summary>
	''' Description of BuildImport
	''' </summary>
	Public Class BuildImport
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected WithEvents btnRead As Button
		Protected WithEvents fuSampleFile As FileUpload
		Protected WithEvents ddlExtFields As DropDownList
		Protected WithEvents ddlTblFields As DropDownList
		Protected WithEvents btnAddData As Button
		Protected WithEvents txtSpData As TextBox
		Protected WithEvents dgDataDefGrid As DataGrid
		Protected WithEvents lstExtFields As ListBox
		Protected WithEvents ddlTblFCriteria As DropDownList
		Protected WithEvents txtCrit As TextBox
		Protected WithEvents btnCritFAdd As Button
		Protected WithEvents btnCritAdd As Button
		Protected WithEvents dgDataCritGrid As DataGrid
		Protected WithEvents chkCritVerify As CheckBox
		Protected WithEvents chkDataVerify As CheckBox
		Protected WithEvents txtCritPri As TextBox
		Dim ImpAction As String
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
			Dim ImpType As Integer
			Dim TableN As String
			If fuSampleFile.FileName <> "" Then
				Session("SampleFile") = fuSampleFile.PostedFile.FileName 'fuSampleFile.FileName
			End If
		
			ImpAction = GetSQLScalar("Select ImportAction from Import where ImportID = " + Session("ImpID").ToString())
			
			'------------------------------------------------------------------
			If Not IsPostBack Then
				PageSecurity()
				ImpType = CInt(GetSQLScalar("Select ImportTypeID from Import Where importid = " + Session("ImpID").ToString()))
				
				If GetSQLScalar("select tablename from importtype where importtypeid = " + ImpType.ToString()) is DBNull.Value Then
					TableN = "Null"
				Else
					TableN = CStr(GetSQLScalar("select tablename from importtype where importtypeid = " + ImpType.ToString()))
				End If
			
				ddlTblFields.DataSource = GetDataView("select c.name from sys.columns c join sys.tables t on t.object_id = c.object_id where t.name = '" + TableN.ToString() + "'")
				ddlTblFields.DataValueField = "name"
				ddlTblFields.DataTextField = "name"
				ddlTblFields.DataBind()
				ddlTblFields.Items.Insert(0,New ListItem("---None Selected---","-1"))
				ddlTblFields.SelectedValue ="-1"
				
				ddlTblFCriteria.DataSource = GetDataView("select c.name from sys.columns c join sys.tables t on t.object_id = c.object_id where t.name = '" + TableN.ToString() + "'")
				ddlTblFCriteria.DataValueField = "name"
				ddlTblFCriteria.DataTextField = "name"
				ddlTblFCriteria.DataBind()
				ddlTblFCriteria.Items.Insert(0,New ListItem("---None Selected---","-1"))
				ddlTblFCriteria.SelectedValue ="-1"
				
				ddlExtFields.Items.Insert(0,New ListItem("---None Selected---","-1"))
				ddlExtFields.SelectedValue ="-1"
				
				If ImpAction <> "Update" And ImpAction <> "Insert" And ImpAction <> "Delete" Then
					PullParams(ImpAction)
				End If
				
				DataFill()
				
				'If Session("SampleFile").ToString <> "" Then
				'	ReadCSVFile
				'End If
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
			AddHandler Me.dgDataDefGrid.ItemDataBound, New DataGridItemEventHandler(AddressOf _ItemDataBound)
			AddHandler Me.dgDataCritGrid.ItemDataBound, New DataGridItemEventHandler(AddressOf Crit_ItemDataBound)
			AddHandler Me.PreRender, New System.EventHandler(AddressOf PreRend)
			'------------------------------------------------------------------
		End Sub
		Private Sub Crit_ItemDataBound(sender As Object, e As DataGridItemEventArgs)
			If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
				e.Item.Cells(6).Attributes.Add ( "onClick", "return ConfirmDeletion();")
			End If
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
		#Region "Data Manipulation"
		Protected Sub ReadCSVFile() Handles btnRead.Click
			Dim strlines() As String
			Dim strcols() As String 
			Dim i As Integer
			Dim l As Integer
			Dim fname As String
			If Session("SampleFile").ToString <> "" And CInt(ddlExtFields.Items.Count) <= 1 Then
				fname = UploadFile(Request,"C:\Inetpub\Training\TrainingScheduler\Uploads")
				Dim strreader As StreamReader = File.OpenText(fname)
				strlines = strreader.ReadToEnd().Split(Environment.NewLine)
				strcols = strlines(0).Split(",")
				For i = 0 To strcols.GetLength(l) - 1
					ddlExtFields.Items.Insert(i + 1, New ListItem(strcols(i).ToString(),strcols(i).ToString))
					lstExtFields.Items.Insert(i,New ListItem(strcols(i).ToString(),strcols(i).ToString()))
				Next
				strreader.Close()
				CleanUploads(fname)
			Else
				If CInt(ddlExtFields.Items.Count) <= 1
					Response.Write("<script>alert('Please Select a File')</script>")
				End if
			End If
			
		End Sub
		
		Protected Sub DeleteDataDef(ByVal source As Object, ByVal e As DataGridCommandEventArgs)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim cmd As New SqlCommand("DeleteDataDef",sqlconn)
			With cmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@ddefid",SqlDbType.Int)
				.Parameters("@ddefid").Value = e.Item.Cells(0).Text
				sqlconn.Open
				.ExecuteNonQuery()
			End With
			cmd = Nothing
			sqlconn.Close()
			DataFill()
		End Sub
		
		Protected Sub DeleteCritDef(ByVal source As Object, ByVal e As DataGridCommandEventArgs)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim cmd As New SqlCommand("DeleteCriteriaDef",sqlconn)
			With cmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@impcid",SqlDbType.Int)
				.Parameters("@impcid").Value = e.Item.Cells(0).Text
				sqlconn.Open
				.ExecuteNonQuery()
			End With
			cmd = Nothing
			sqlconn.Close()
			DataFill()
		End Sub
		
		Protected Sub AddDataDef() handles btnAddData.Click
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim cmd As New SqlCommand("AddDataDef",sqlconn)
			If ddlExtFields.SelectedValue <> "-1" And txtSpData.Text <> "" Then
				Response.Write("<script>alert('Please select an external field or specific data not both')</script>")
			Else
				With cmd
					.CommandType = CommandType.StoredProcedure
					.Parameters.Add("@tblf",SqlDbType.VarChar)
					.Parameters.Add("@data",SqlDbType.VarChar)
					.Parameters.Add("@impID",SqlDbType.Int)
					.Parameters.Add("@ver",SqlDbType.Bit)
					.Parameters("@tblf").Value = ddlTblFields.SelectedValue
					If ddlExtFields.SelectedValue <> "-1" And txtSpData.text = "" Then
						.Parameters("@data").Value = "{Ex:" + ddlExtFields.SelectedValue.ToString() + "}"
					Else
						.Parameters("@data").Value = txtSpData.Text
					End If
					If chkDataVerify.Checked = True Then
						.Parameters("@ver").Value = 1
					Else
						.Parameters("@ver").Value = 0
					End If
					.Parameters("@impID").Value  = Session("ImpID").ToString()
					sqlconn.Open
					.ExecuteNonQuery()
				End With
				cmd = Nothing
				sqlconn.Close()
				ClearData()
				DataFill()
			End If

		End Sub
		Protected Sub ClearData()
			ddlTblFields.SelectedValue = "-1"
			ddlExtFields.SelectedValue = "-1"
			txtSpData.Text = ""
			ddlTblFCriteria.SelectedValue = "-1"
			txtCrit.Text = ""
		End Sub
		Protected Sub AddCrit(sender As Object, e As System.EventArgs) Handles btnCritAdd.Click
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim cmd As New SqlCommand("AddCriteriaDef",sqlconn)
			With cmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@critf",SqlDbType.VarChar)
				.Parameters.Add("@crit",SqlDbType.Text)
				.Parameters.Add("@impid",SqlDbType.Int)
				.Parameters.Add("@ver",SqlDbType.Bit)
				.Parameters.Add("@Pri",SqlDbType.Int)
				.Parameters("@critf").Value = ddlTblFCriteria.SelectedValue
				.Parameters("@crit").Value = CStr(txtCrit.Text)
				.Parameters("@impid").Value = Session("ImpID").ToString()
				If chkCritVerify.Checked = True Then
					.Parameters("@ver").Value = 1
					If txtCritPri.Text = "" Then
						.Parameters("@Pri").Value = DBNull.Value
					Else
						.Parameters("@Pri").Value = txtCritPri.Text
					End If
				Else
					.Parameters("@ver").Value = 0
					.Parameters("@Pri").Value = DBNull.Value
				End If

				sqlconn.Open
				.ExecuteNonQuery
			End With
			sqlconn.Close
			ClearData()
			DataFill()
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Fill"
		Protected Sub DataFill()
			dgDataDefGrid.DataSource = GetDataView("select * from importdatadef where importid = " + Session("ImpID").ToString())
			dgDataDefGrid.DataBind()
			dgDataCritGrid.DataSource = GetDataView("select * from ImportCriteriaDef where importid = " + Session("ImpID").ToString())
			dgDataCritGrid.DataBind()
		End Sub
		Protected Sub AppendField(sender As Object, e As System.EventArgs) Handles btnCritFAdd.Click
			Dim sqlcrit As String
			sqlcrit = txtCrit.Text + "{Ex:" + lstExtFields.SelectedValue.ToString() + "}"
			txtCrit.Text = sqlcrit
		End Sub
		Protected Sub PullParams(strStore As String)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim sqlcmd As New SqlCommand(strStore,sqlconn)
			Dim i As Integer
			
			i = 1
			
			sqlcmd.CommandType = CommandType.StoredProcedure
			
			sqlconn.Open()
			SqlCommandBuilder.DeriveParameters(sqlcmd)
			sqlconn.Close()
			
			
			For Each p As SqlParameter In sqlcmd.Parameters
				If p.Direction = ParameterDirection.Input Or p.Direction = ParameterDirection.InputOutput Then
					ddlTblFields.Items.Insert(i,New ListItem(CStr(p.ParameterName.ToString() + " - " + p.SqlDbType.ToString()),CStr(p.ParameterName)))
					i = i + 1
				End If
			Next
		End Sub
		#End Region
	End Class
