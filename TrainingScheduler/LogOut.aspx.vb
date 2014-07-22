'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 7/3/2014
' Time: 10:53 AM
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
Imports System.DirectoryServices
Imports TrainingScheduler.Utility


	''' <summary>
	''' Description of LogOut
	''' </summary>
	Public Class LogOut
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
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
			Session.Abandon()
			If Request.QueryString("redirect") = "yes" Then
				Response.Redirect("Login.aspx")
			End If
			'------------------------------------------------------------------
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
			'------------------------------------------------------------------
			'------------------------------------------------------------------
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
	End Class