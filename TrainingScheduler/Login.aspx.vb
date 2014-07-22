Option Explicit On
Option Strict On

'
' Created by SharpDevelop.
' User: screveling
' Date: 5/23/2012
' Time: 8:27 AM
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
	''' Creates Session("login") - boolean for logged in or not
	''' Creates Session("loginusername") - i.e. "screveling"
	''' Creates Session("logingroupname") - either "TS Admin" or "TS Registration"
	''' Going to this page while logged logs you out
	''' </summary>
	Public Class Login
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"

		Protected txtUsername As TextBox
		Protected txtPassword As TextBox
		Protected lblMsg As Label
		Protected WithEvents btnLogin As Button
		
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Page Init & Exit (Open/Close DB connections here...)"

		Protected Sub PageInit(sender As Object, e As System.EventArgs)	
		End Sub
		'----------------------------------------------------------------------
		Protected Sub PageExit(sender As Object, e As System.EventArgs)
		End Sub
		
		Private Sub Page_Load(sender As Object, e As System.EventArgs)
			
			'------------------------------------------------------------------
			If Not IsPostBack Then
				If CBool(Session("login")) Then
					Session("login") = False
					Session.Remove("loginusername")
					Session.Remove("logingroupname")
					lblMsg.Text = "You have been logged out.  Close your browser window or log in again below."									
				End If
			End If
			'------------------------------------------------------------------
		End Sub
		#End Region
		
		#Region "Add more events here..."
		Private Sub btnLogin_Click(sender As Object, e As System.EventArgs) Handles btnLogin.Click
			Dim un As String = txtUsername.Text.Trim.ToLower
			Dim groupstr As String
			
			If LdapAuthenticateLogin(un, txtPassword.Text) Then
				groupstr = LdapGroupLookup(un, txtPassword.Text)				
				If groupstr <> "" Then
					Session("login") = true
					Session("loginusername") = un
					Session("logingroupname") = groupstr
					Session("ParentPage") = "REGISTRATION"
					Response.Redirect("Default.aspx")
				Else
					lblMsg.Text = "You do not have access to the training scheduler.  Please contact HR to request access."
				End If
			Else
				lblMsg.Text = "Invalid username or password. Please try again."
			End If
		End Sub
		
		#End Region
				
		#Region "Initialize Component"

		Protected Overrides Sub OnInit(e As System.EventArgs)
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
		End Sub
		#End Region
				
		#Region "MyCode"
		Private Function LdapAuthenticateLogin(un As String, pw As String) As Boolean
			Try
				LdapSearchResult(un, pw)				
				Return true
			Catch ex As Exception		
				Return false
			End Try			
		End Function
		
		Private function LdapGroupLookup(un As String, pw As string) as string
			Dim ldapresult As SearchResult = LdapSearchResult(un, pw)
			Dim groupstr As String = ""
			Dim result As String = ""
			Dim groups() As String
			Dim sgroup As String
			Dim log As Integer
			
			
			log = 0
			' if the user is a member of both admin and registration, record admin
			For Each p As Object In ldapresult.GetDirectoryEntry().Properties("memberOf")
				groupstr = p.ToString()				
				groups = groupstr.Split(CChar(","))
				sgroup = groups(0).Substring(3,groups(0).Length - 3)
				
				log = CInt(GetSQLScalar("Select Count(GroupID) from LDAPGroups where LDAPGroupName = '" + sgroup + "'"))
				
				If log > 0 Then
					Session("SecurityGroupID") = GetSQLScalar("Select GroupID from LDAPGroups where LDAPGroupName = '" + sgroup + "'")
				End If
				
			Next
			
			If ldapresult.GetDirectoryEntry().Properties.Contains("mail") Then
				If ldapresult.GetDirectoryEntry().Properties("mail").Value.ToString <> "" Then
					Session("Email") = ldapresult.GetDirectoryEntry().Properties("mail").Value.ToString
				End if
			End If
			
			
			result = "Yes"
			return result
		End function
		
		Private Function LdapSearchResult(un As String, pw As String) As SearchResult
			Dim Entry As DirectoryEntry = New DirectoryEntry("LDAP://spindc1.spininc.local:389/DC=spininc,DC=local", un, pw)        
			Dim Searcher As DirectorySearcher = New DirectorySearcher(Entry)
			
			Searcher.Filter = ("(sAMAccountName=" & un & ")")
			Return Searcher.FindOne()	
			
		End Function
		#End region
	End Class