'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 4/19/2012
' Time: 11:21 AM
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Imports System
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls

Namespace UserControl1
	''' <summary>
	''' Description of ClickGrid
	''' </summary>
	Public Class ClickGrid

		Inherits DataGrid
		Implements IPostBackEventHandler
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Constructor"

		Public Sub New()
			AddHandler Init, New EventHandler(AddressOf OnInit)
			AddHandler Load, New EventHandler(AddressOf OnLoad)
		End Sub


'    	When a row is clicked, this is called and event will be raised
'    	with the specific item as sender of the event
   		Public Sub RaisePostBackEvent(ByVal eventArgument As String) Implements System.Web.UI.IPostBackEventHandler.RaisePostBackEvent
        	If Not eventArgument Is Nothing Then
            	Dim i As Integer = Int32.Parse(eventArgument)
            	OnItemClicked(Me.Items(i), EventArgs.Empty)
 
        	End If
    	End Sub
		Public Event ItemClicked(ByVal sender As Object, ByVal e As EventArgs)

    	'Method to raise the event
    	Protected Overridable Sub OnItemClicked(ByVal sender As Object, ByVal e As EventArgs)
        	RaiseEvent ItemClicked(sender, e)
    	End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "OnInit"

		Private Overloads Sub OnInit(sender As Object, e As EventArgs)
		End Sub

		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "OnLoad"

		Private Overloads Sub OnLoad(sender As Object, e As EventArgs)
		End Sub

		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Properties"
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Render"

		Protected Overrides Sub Render(Writer As HtmlTextWriter)
			Writer.Write("ClickGrid Control")
		End Sub
		#End Region
	End Class
End Namespace