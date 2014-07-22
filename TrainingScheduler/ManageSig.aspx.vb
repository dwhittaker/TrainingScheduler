'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 7/2/2014
' Time: 2:44 PM
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
Imports System.Web.UI.UserControls
Imports System.IO
Imports CustomWebControls

	''' <summary>
	''' Description of ManageSig
	''' </summary>
	Public Class ManageSig
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected WithEvents Place As PlaceHolder
		Protected WithEvents cwcEmp As ddlSearch
		Protected WithEvents btnGet As Button
		Protected pstback As String
		Protected AddSig As Boolean
		Protected VisSig As Boolean
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
			Session("LocalPath") = "C:\Inetpub\Training\TrainingScheduler\"

			'------------------------------------------------------------------
			If Not IsPostBack Then
				PageSecurity()
				cwcEmp.ConWidth = 380
				cwcEmp.CtrlDataSource = "select emp_id,last_name + ', ' + first_name + ' (' + isnull(socialsecurityno,emp_id) + ', ' + dept_name + ' ' + job_title + ')' as empinfo from v_employees order by last_name, First_Name"
				cwcEmp.CtrlVField = "emp_id"
				cwcEmp.CtrlTField = "empinfo"
				cwcEmp.CtrlBind()
			Else
				pstback = Page.Request.Params.get("__EVENTTARGET").ToString() 
				If pstback = "ctl00$Main$cwcEmp$ddlList" Then
					Session("VisSig") = False
				End If
				If Not Session("VisSig") Is Nothing Then
					If Session("VisSig").ToString = "True" Then
						RefreshSig()
						Session("VisSig") = False
					End If	
				End If
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
			AddHandler Me.Place.PreRender, New System.EventHandler(AddressOf Place_PreRend)
			AddHandler Me.cwcEmp.IndChange, New ddlSearch.IndChangeEventHandler(AddressOf cwcEmp_Change)
			AddHandler Me.btnGet.Click, New System.EventHandler(AddressOf btnGet_Click)
			'------------------------------------------------------------------
		End Sub
		Protected Sub PreRend(sender As Object,e As System.EventArgs)
			Dim fctrl As Control
			Dim ctrl As String
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
			if String.IsNullOrEmpty(pstback) Then
				For Each ctrl In Page.Request.Form
					fctrl = Page.FindControl(ctrl)
					If Typeof fctrl Is System.Web.UI.WebControls.Button Then
						pstback = fctrl.ID.ToString()
						exit For
					End If
				Next
			End If
			If pstback = "btnSave" Or pstback = "delSig" Then
				For Each fctrl In Me.Place.Controls
					If fctrl.ID = "MEmpSig" Then
						Me.Place.Controls.Remove(fctrl)
						btnGet.Visible = True
						Session("VisSig") = False
						Exit For 
					End If
				Next
			End If
		End Sub
		Protected Sub Place_PreRend(sender As Object, e As System.EventArgs)
			If AddSig = True Then
				AddSig = False
				RefreshSig()
				Session("VisSig") = True
			End If
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data Manipulation"
		Protected Sub cwcEmp_Change(ByVal sender As Object,ByVal e As System.EventArgs)
			Session("VisSig") = False
			btnGet.Visible = True
		End Sub
		Protected Sub btnGet_Click(sender As Object,e As System.EventArgs) 
			AddSig = True
			btnGet.Visible = False
		End Sub
		Protected Sub RefreshSig()
			Dim sig As UserControl = LoadControl("ESig.ascx")
			Session("EmpID") = cwcEmp.CtrlSelValue
			Session("SPath") = "Signatures\" + cwcEmp.CtrlSelValue.ToString() + "\"
			If System.IO.File.Exists(CStr(Session("LocalPath")) + CStr (Session("SPath")) + "EmpSig.jpg") Then
				Session("HasSig") = "True"
			Else
				Session("HasSig") = "False"
			End If
			sig.ID = "MEmpSig"
			Me.Place.Controls.Add(sig)
		End Sub
		Protected Sub DisposeSig()
			Dim sig As UserControl = LoadControl("ESig.ascx")
			sig.ID = "MEmpSig"
			Me.Place.Controls.Remove(sig)
			sig.Visible = False
		End Sub
		#End Region
	End Class
