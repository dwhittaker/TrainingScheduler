'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 8/24/2012
' Time: 1:46 PM
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
	Public Class AttendanceForm
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected strCCode As String
		Protected dtCDate As DateTime
		Protected strStime As String
		Protected strEtime As String
		Protected strSize As String
		Protected strLoc As String
		Protected strCTitle As String
		Protected strInst As String
		Protected strDTime As String
		Protected strDesc As String
		Protected dgAttGrid As System.Web.UI.WebControls.DataGrid 
		Protected chkSelection As System.Web.UI.WebControls.CheckBox
		Protected btnCheckAll As System.Web.UI.WebControls.Button
		Protected strdaCmd As String
		Protected btnRemAttend As System.Web.UI.WebControls.Button
		Protected WithEvents fuSignUp As FileUpload
		Protected WithEvents btnUpload As Button
		Protected WithEvents btnView As Button
		Protected WithEvents btnDelete As Button
		Protected lblFile As Label
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Page Init & Exit (Open/Close DB connections here...)"

		Protected Sub PageInit(sender As Object, e As System.EventArgs)
			CheckLogin(Cbool(Session("login")),Me.Response)
			dgAttGrid.EnableViewState = False
		End Sub
		'----------------------------------------------------------------------
		Protected Sub PageExit(sender As Object, e As System.EventArgs)
		End Sub

		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Page Load"
		Private Sub Page_Load(sender As Object, e As System.EventArgs)
			'------------------------------------------------------------------
			Dim fexists As String
			fexists = GetSQLScalar("Select UpID from Uploads where ParentID = " + Session("strCCode").ToString)
			If fexists <> "" Then
				fuSignUp.Visible = False
				lblFile.Visible = True
				lblFile.Text = GetSQLScalar("Select [Path] from Uploads where ParentID = " + Session("strCCode").ToString)
				btnView.Attributes.Add("onClick","Javascript:ViewSignIn('" + lblFile.Text.Replace("\","\\") + "');return false;")
				btnView.Visible = True
				btnUpload.Visible = False
				btnDelete.Visible = True
			End If
			ClassFill()
			If Not IsPostBack Then
				PageSecurity()
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
			AddHandler Me.PreRender, New System.EventHandler(AddressOf PreRend)
			'------------------------------------------------------------------
		End Sub
		Protected Sub PreRend(sender As Object,e As System.EventArgs)
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "DataFill"
		Protected Sub ClassFill()
			strCCode = Session("strCCode").ToString
			dtCDate = CDate(Session("dtCDate"))
			strStime = Session("strStime").ToString
			strEtime = Session("strEtime").ToString
			strSize = Session("strSize").ToString
			strLoc = Session("strLocation").ToString
			strCTitle = Session("strCTitle").ToString
			strInst = Session("strIns").ToString
			strDesc = Session("strDesc").ToString
			strDTime = dtCDate & " - " & strStime & " to " & strEtime
			strdaCmd = "select * from RegisteredEmployees where CourseInstanceID = " & strCCode & " order by last_name asc"
'			dsEmp.Clear
'			With daEmp
'				.SelectCommand.Connection.ConnectionString = connEmpStr
'				.SelectCommand.CommandText = strdaCmd
'			End With
'			sqlEmpconn.Open
'			daEmp.Fill(dsEmp,"RegisteredEmployees")
'			dvEmp.Table = dsEmp.Tables(0)
			dgAttGrid.DataSource = GetDataView(strdaCmd)
			dgAttGrid.DataBind
		End Sub
		Protected Sub CheckAll()
			If Session("ClassCheck").ToString = "Check" then
				For each i As DataGridItem In dgAttGrid.Items
					chkSelection = Ctype(i.FindControl("chkSelection"), CheckBox)
					chkSelection.Checked = True
				Next i
				Session("ClassCheck") = "Un-Check"
				btnCheckAll.Text = Session("ClassCheck").ToString & " All"
			Else If Session("ClassCheck").ToString = "Un-Check"
				For Each i As DataGridItem In dgAttGrid.Items
					chkSelection = CType(i.FindControl("chkSelection"), CheckBox)
					chkSelection.Checked = False
				Next i
				Session("ClassCheck") = "Check"
				btnCheckAll.Text = Session("ClassCheck").ToString & " All"
			End If
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Manipulation"
		Protected Sub MarkAttendance(sender As Object, e As EventArgs)
			Dim btnButton As Button = sender
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim strcmd As String
			Dim f As Integer
			
			If btnButton.ID.ToString = "btnAttend" Then
				strcmd = "PassEmp"
			ElseIf btnButton.ID.ToString = "btnNoShow" Then
				strcmd = "MarkNoShow"
			ElseIf btnButton.ID.ToString = "btnRemAttend" Then
				strcmd = "RemoveAttendance"
			End If
			
			Dim updatecmd As New SqlCommand(strcmd,sqlconn)
				
			f = 0
			With updatecmd
						.CommandType = CommandType.StoredProcedure
						.Parameters.Add("@empID", SqlDbType.NVarChar)
						.Parameters.Add("@ci", SqlDbType.Int) 
						.Parameters.Add("@status",SqlDbType.VarChar)
						.Parameters.Add("@user",SqlDbType.VarChar)
						sqlconn.Open
			End With
			For Each i As DataGridItem In dgAttGrid.Items
				chkSelection = CType(i.FindControl("chkSelection"), CheckBox)
				If chkSelection.Checked = True Then
					dgAttGrid.SelectedIndex = f
					With updatecmd
						.Parameters("@empid").Value = dgAttGrid.SelectedItem.Cells(1).Text.ToString
						.Parameters("@ci").Value  = strCCode
						.Parameters("@status").Value = dgAttGrid.SelectedItem.Cells(8).Text.ToString
						.Parameters("@user").Value = Session("loginusername")
						.ExecuteNonQuery()
					End With
				End If
				f = f + 1
			Next i
			updatecmd = Nothing
			sqlconn.Close
			ClassFill()
			Session("ClassCheck") = "Check"
			btnCheckAll.Text = Session("ClassCheck").ToString & " All"
		End Sub
		Protected Sub UploadSignIn() Handles btnUpload.Click
			Dim upath As String
			Dim vpath As String
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim addcmd As New SqlCommand("InsertUpload",sqlconn)
			upath = "C:\Inetpub\Training\TrainingScheduler\Uploads\" +  Session("strCCode").ToString()
			UploadFile(Request,upath)
				
			vpath = "\TrainingScheduler\Uploads\" + Session("strCCode").ToString() + "\" + System.IO.Path.GetFileName(Request.Files(0).FileName.ToString())
				
			vpath = vpath.Replace(" ","")
				
			With addcmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@par", SqlDbType.VarChar)
				.Parameters.Add("@parid", SqlDbType.Int)
				.Parameters.Add("@path", SqlDbType.VarChar)
				.Parameters("@par").Value = "CI"
				.Parameters("@parid").Value = Session("strCCode").ToString()
				.Parameters("@path").Value = vpath
				sqlconn.Open()
				.ExecuteNonQuery()
			End With
				
			sqlconn.Close()
			addcmd = Nothing
				
			Response.Write("<script>alert('File uploaded')</script>")
			fuSignUp.Visible = False
			lblFile.Visible = True
			lblFile.Text = GetSQLScalar("Select [Path] from Uploads where ParentID = " + Session("strCCode").ToString)
			btnView.Attributes.Add("onClick","Javascript:ViewSignIn('" + vpath.Replace("\","\\") + "');return false;")
			btnUpload.Visible = False
			btnView.Visible = True
			btnDelete.Visible = True
		End Sub
		Protected Sub DeleteSignIn() Handles btnDelete.Click
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim delcmd As New SqlCommand("DeleteUpload",sqlconn)
			Dim delid As Integer
			Dim delpath As String
			
			delid = CInt(GetSQLScalar("Select UpID from Uploads where ParentID = "+ Session("strCCode").ToString))
			
			delpath = GetSQLScalar("Select [Path] from Uploads where ParentID = " + Session("strCCode").ToString)
			
			delpath = "C:\Inetpub\Training" + delpath
			
			With delcmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@uid",SqlDbType.Int)
				.Parameters("@uid").Value = delid
				sqlconn.Open
				.ExecuteNonQuery()
			End With
			sqlconn.Close()
			delcmd = Nothing
			
			CleanUploads(delpath)
			
			fuSignUp.Visible = True
			lblFile.Text = ""
			btnView.Attributes.Clear()
			btnUpload.Visible = True
			btnView.Visible = False
			btnDelete.Visible = False
			
		End Sub
		#End Region
	End Class