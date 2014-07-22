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
		Protected WithEvents chkApD As CheckBox
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
		Protected WithEvents hdfCatID As HiddenField
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
				ddlReport.DataSource = GetDataView("Select RgID, ReportGroup from ReportGroups")
				ddlReport.DataValueField = "RgID"
				ddlReport.DataTextField = "ReportGroup"
				ddlReport.DataBind()
				ddlReport.Items.Insert(0,New ListItem("----None Selected----","-1"))
				ddlReport.SelectedValue = "-1"
			End If
			'------------------------------------------------------------------
			

			
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
			AddHandler Me.dgCatGrid.ItemDataBound, New DataGridItemEventHandler(AddressOf _ItemDataBound)
			'------------------------------------------------------------------
			'------------------------------------------------------------------
		End Sub
		Protected Sub PreRend(sender As Object,e As System.EventArgs)
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
		End Sub
		
		Private Sub _ItemDataBound(sender As Object, e As DataGridItemEventArgs)
			If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
				e.Item.Cells(6).Attributes.Add ( "onClick", "return ConfirmDeletion();")
			End If
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
		Protected Sub CancelCat(sender As Object, e As System.EventArgs) Handles btnCancel.Click
			ClearFrm()
		End Sub
		Protected Sub SaveEdit_Click(sender As Object, e As System.EventArgs) Handles btnSaveEdit.Click
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim addcmd As New SqlCommand("AddCourseCat",sqlconn)
			Dim editcmd As New SqlCommand("UpdateCourseCat",sqlconn)
			If btnSaveEdit.Text = "Save Category" Then
				With addcmd
					.CommandType = CommandType.StoredProcedure
					.Parameters.Add("@cat",SqlDbType.VarChar)
					.Parameters.Add("@rec",SqlDbType.Bit)
					.Parameters.Add("@apd",SqlDbType.Bit)
					.Parameters.Add("@rg",SqlDbType.Int)
					.Parameters("@cat").Value = txtCatName.Text
					If chkRecert.Checked = True Then
						.Parameters("@rec").Value = True
					Else
						.Parameters("@rec").value = False
					End If
					If chkApD.Checked = True Then
						.Parameters("@apd").Value = True
					Else
						.Parameters("@apd").Value = False
					End If
					.Parameters("@rg").Value = ddlReport.SelectedItem.Value
					sqlconn.Open
					.ExecuteNonQuery()
				End With
				addcmd = Nothing
				sqlconn.Close()
			Else
				With editcmd
					.CommandType = CommandType.StoredProcedure
					.Parameters.Add("@catid",SqlDbType.Int)
					.Parameters.Add("@cat",SqlDbType.VarChar)
					.Parameters.Add("@rec",SqlDbType.Bit)
					.Parameters.Add("@apd",SqlDbType.Bit)
					.Parameters.Add("@rg",SqlDbType.Int)
					.Parameters("@catid").Value = hdfCatID.Value.ToString()
					.Parameters("@cat").Value = txtCatName.Text
					If chkRecert.Checked = True Then
						.Parameters("@rec").Value = True
					Else
						.Parameters("@rec").Value = False
					End If
					If chkApD.Checked = True Then
						.Parameters("@apd").Value = True
					Else
						.Parameters("@apd").Value = False
					End If
					.Parameters("@rg").Value = ddlReport.SelectedItem.Value
					sqlconn.Open
					.ExecuteNonQuery()
				End With
				editcmd = Nothing
				sqlconn.Close()
			End If
			Datafill()
			dgCatGrid.Visible = True
			UpPan4.Update()
			ClearFrm()
'			PanAddEdit.Visible = False
'			UpPan3.Update()
'			PanCreate.Visible = True
'			UpPan5.Update()
		End Sub
		Public Sub Datafill()
			dgCatGrid.DataSource = GetDataView("Select * from v_CourseCat")
			dgCatGrid.DataBind()
			'UpPan4.Update()
		End Sub
		Protected Sub EditCat(sender As Object, e As DataGridCommandEventArgs)
			Dim lsti As ListItem
			PanAddEdit.Visible = True
			txtCatName.Text = e.Item.Cells(1).Text
			hdfCatID.Value = e.Item.Cells(0).Text
			If e.Item.cells(2).Text = "True" Then
				chkRecert.Checked = True
			Else
				chkRecert.Checked = False
			End If
			If e.Item.Cells(3).Text = "True" Then
				chkApD.Checked = True
			Else
				chkApD.Checked = False
			End If
			lsti = ddlReport.Items.FindByText(e.Item.Cells(4).Text.ToString())
			ddlReport.SelectedValue =  lsti.Value.ToString()
			btnSaveEdit.Text = "Update Category"
			UpPan3.Update()
			PanCreate.Visible = False
			UpPan5.Update()
		End Sub
		Protected Sub DeleteCat(sender As Object, e As DataGridCommandEventArgs)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim delcmd As New SqlCommand("DeleteCourseCat",sqlconn)
			
			With delcmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@catid",SqlDbType.Int)
				.Parameters("@catid").Value = e.Item.Cells(0).Text.ToString()
				
				sqlconn.Open
				.ExecuteNonQuery()
			End With
			delcmd = Nothing
			sqlconn.Close
			
			Datafill()
			UpPan4.Update()
		End Sub
		Protected Sub ClearFrm()
			txtCatName.Text = ""
			chkRecert.Checked = False
			chkApD.Checked = False
			ddlReport.SelectedValue= "-1"
			btnSaveEdit.Text = "Save Category"
			PanAddEdit.Visible = False
			UpPan3.Update()
			PanCreate.Visible = True
			UpPan5.Update()
		End Sub
		#End Region
	End Class