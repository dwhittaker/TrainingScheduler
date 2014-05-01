'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 5/3/2013
' Time: 4:37 PM
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


	Public Class SignIn
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"

		Protected WithEvents ctSig As UserControl
		Protected WithEvents txtSSN As TextBox
		Protected WithEvents btnLogin As Button
		Protected WithEvents lblSSN As Label
		Protected WithEvents Test As ddlSearch
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Page Init & Exit (Open/Close DB connections here...)"

		Protected Sub PageInit(sender As Object, e As System.EventArgs)
			CheckLogin(CBool(Session("login")),Me.Response)
			Session("LocalPath") = "C:\Inetpub\Training\TrainingScheduler\"
			If System.IO.File.Exists(CStr(Session("LocalPath")) + CStr (Session("SPath")) + "EmpSig.jpg") Then
				Session("HasSig") = "True"
			Else
				Session("HasSig") = "False"
			End If
		End Sub
		'----------------------------------------------------------------------
		Protected Sub PageExit(sender As Object, e As System.EventArgs)
		End Sub

		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Page Load"
		Private Sub Page_Load(sender As Object, e As System.EventArgs)
			Dim gettest As String
			If Session("SSNVer") = "False" Then
				txtSSN.Visible = True
				btnLogin.Visible = True
				ctSig.Visible = False
				lblSSN.Visible = True
			Else
				txtSSN.Visible = False
				btnLogin.Visible = False
				ctSig.Visible = True
				lblSSN.Visible = False
			End If


		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
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
			'------------------------------------------------------------------
			'------------------------------------------------------------------
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Verify"
		Protected Sub VerSSN(ByVal source As Object, ByVal e As EventArgs) Handles btnLogin.Click
			If CInt(txtSSN.Text) = Session("EmpID") Then
				txtSSN.Visible = False
				btnLogin.Visible = False
				ctSig.Visible = True
				lblSSN.Visible = False
				Session("SSNVer") = "True"
			Else
				Response.Write("<script>alert('Could not verify SSN. Please try again with out dashes or double check the name on the top of the screen.')</script>")
				txtSSN.Visible = True
				btnLogin.Visible = True
				ctSig.Visible = False
				lblSSN.Visible = False
				Session("SSNVer") = "False"
			End If
		End Sub
		#End Region
	End Class
