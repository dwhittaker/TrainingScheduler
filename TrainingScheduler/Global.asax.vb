Imports System.Collections
Imports System
Imports System.ComponentModel
Imports System.Web
Imports System.Web.SessionState

''' <summary>
''' Summary description for Global.
''' </summary>
Public Class [Global]
	Inherits HttpApplication
	'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
	#Region "global"
	''' <summary>
	''' Required designer variable.
	''' </summary>
	'Private components As System.ComponentModel.IContainer = Nothing

	Public Sub New()
		InitializeComponent()
	End Sub

	#End Region
	'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

	Protected Sub Application_Start(sender As Object, e As EventArgs)

	End Sub

	Protected Sub Session_Start(sender As Object, e As EventArgs)
		Session("dtTrDate") = DateTime.Today
		Session("strCCode") = ""
		Session("strCTitle") = ""
		Session("dtCDate") = DateTime.Today
		Session("strStime") = ""
		Session("strEtime") = ""
		Session("strSize") = ""
		Session("strLocation") = ""
		Session("strIns") = ""
		Session("strDesc") = ""
		Session("strCourID") = ""
		Session("intRMonths") = ""
		Session("LoginName") = ""
		Session("ParentPage") = ""
		Session("ClassCheck") = "Check"
		Session("SOrder") = ""
		Session("dtCalDate") = DateTime.Today
		Session("ReaderVal") = ""
		Session("SampleFile") = ""
		Session("ExpandedMod") = ""
	End Sub

	Protected Sub Application_BeginRequest(sender As Object, e As EventArgs)

	End Sub

	Protected Sub Application_EndRequest(sender As Object, e As EventArgs)

	End Sub

	Protected Sub Application_AuthenticateRequest(sender As Object, e As EventArgs)

	End Sub

	Protected Sub Application_Error(sender As Object, e As EventArgs)

	End Sub

	Protected Sub Session_End(sender As Object, e As EventArgs)

	End Sub

	Protected Sub Application_End(sender As Object, e As EventArgs)

	End Sub

	#Region "Web Form Designer generated code"
	''' <summary>
	''' Required method for Designer support - do not modify
	''' the contents of this method with the code editor.
	''' </summary>
	Private Sub InitializeComponent()

	End Sub
	#End Region
End Class
