'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 5/23/2012
' Time: 4:50 PM
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.MasterPage
Imports TrainingScheduler.Utility

	Public Class SiteMaster
		Inherits MasterPage
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected WithEvents lblparent As HiddenField
	

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
			
'			Dim CSM As ClientScriptManager = Page.ClientScript
'			CSM.GetPostBackEventReference(lblparent, "")
			'------------------------------------------------------------------
			If IsPostBack Then
				If lblparent.Value.ToString <> "" Then
					Session("ParentPage") = lblparent.Value.ToString
				End If 

			End If
'			test = Session("SampleFile").ToString()
'			If (Page.ToString() <> "ASP.buildimport_aspx" Or Page.ToString() <> "ASP.importfile_aspx") And Session("SampleFile").ToString <> "" Then
'				Session("SampleFile") = ""
'			End If
			'------------------------------------------------------------------
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Add more events here..."
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Initialize Component"
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "lblParent"
		Protected Sub parentload() Handles lblparent.Load
				If lblparent.Value.ToString <> "" Then
					Session("ParentPage") = lblparent.Value.ToString
				End If 
		End Sub
		#End Region
	End Class
