'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 5/24/2012
' Time: 12:03 PM
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
Imports System.IO
Public Class Utility		
	Protected strReaderval As String	
	Shared Sub CheckLogin(log As Boolean, p As HttpResponse)
		If log = False Then
			p.Redirect("Login.aspx")
		End if
	End Sub
	Shared Function GetDataView(strSQL As String) As DataView
		Dim connStr As String = System.Configuration.ConfigurationManager.AppSettings("ConnectionString")
		Dim sqlconn As New SqlConnection(connStr)
		Dim daData As New SqlDataAdapter("",connstr)
		Dim dsData As New DataSet("Data")
		Dim dvData As New DataView
		dsData.Clear
		With daData
			.SelectCommand.Connection.ConnectionString = connStr
			.SelectCommand.CommandText = strSQL
		End With
		sqlConn.Open
		daData.Fill(dsData)
		dvData.Table = dsData.Tables(0)
		sqlConn.Close
		GetDataView = dvData
	End Function
	Shared Function GetDataReader(strSQL As String) as SqlDataReader
		Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
		Dim cmd As New SqlCommand(strSQL,conn)
		cmd.Connection.Open
		GetDataReader = cmd.ExecuteReader
	End Function
	Shared Function GetSQLScalar (strSQL As String) As Object
		Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
		Dim cmd As New SqlCommand(strSQL,conn)
		conn.Open
		GetSQLScalar = cmd.ExecuteScalar
		cmd = Nothing
		conn.Close
		'cmd.Connection.Dispose
	End Function
'	Shared Function AssignReader(strSQL As String) As String(,)
'		Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
'		Dim cmd As New SqlCommand(strSQL,conn)
'		cmd.Connection.Open
'		AssignReader = cmd.ExecuteReader
'		cmd.Connection.Close
'	End Function
'	Shared Sub BuildDDL(DDL As DropDownList,array As String(,))
'		Dim i As Integer
'		i = 1
'		For i = 1 To array.Length
'			DDL.Items.Add(New ListItem(array(1,i),array(0,i)))
'		Next
'	End Sub
	Shared Sub FormAccess(frmCheck As HtmlForm, sgroup As Integer)
		Dim v As Integer
		For Each c As Control In frmCheck.Controls
			If TypeOf(c) Is HyperLink Then
				With TryCast(c, HyperLink)
					v = CInt(GetSQLScalar("Select count(PageID) from PageAccess where pageid in (select PageID from Pages where Url = '" + .NavigateUrl.ToString + "') and GroupID = " + CStr(sgroup)))
					If c.ID = "hlLogout" Then
						v = 1
					End If
					If v = 0 Then
						.Visible = False
					End If
				End With
			ElseIf TypeOf(c) Is ContentPlaceHolder Then
				For Each con As Control In c.Controls
					If TypeOf(con) Is HyperLink Then
						With TryCast(con, HyperLink)
							v = CInt(GetSQLScalar("Select count(PageID) from PageAccess where pageid in (select PageID from Pages where Url = '" + .NavigateUrl.ToString + "') and GroupID = " + CStr(sgroup))) 
							If c.ID = "hlLogout" Then
								v = 1
							End If
							If v = 0 Then
								.Visible = False
							End If
						End With
					End If
				Next
			End If	
		Next
	End Sub
	Shared Function PageAccess(CPage As String, sgroup As Integer) As Boolean
		Dim ind As Integer
		Dim a As Integer
			
		ind = CPage.LastIndexOf("/")
		If ind > 0 Then
			CPage = CPage.Substring(ind,Cpage.Length - ind)			
			ind = CPage.Length - 1
			CPage = CPage.Substring(1,ind)
		End If

		a = CInt(GetSQLScalar("Select count(PageID) from PageAccess where pageid in (select PageID from Pages where Url = '" + CPage.ToString + "') and GroupID = " + CStr(sgroup)))
		If a = 0 Then
			PageAccess = False
		Else
			PageAccess = True
		End If
	End Function
	Shared Sub ControlAccess(sgroup As Integer, CPage as String, Con As Control)
		Dim v As Integer
		Dim p As Integer
		Dim ind As Integer
		
		ind = CPage.LastIndexOf("/")
		If ind > 0 Then
			CPage = CPage.Substring(ind,Cpage.Length - ind)			
			ind = CPage.Length - 1
			CPage = CPage.Substring(1,ind)
		End If
			
		p = CInt(GetSQLScalar("Select pageid from pages where Url = '" + CPage + "'"))
		For Each c As Control In Con.Controls
			If TypeOf(c) Is ContentPlaceHolder Then
			Else
				If c.ID <> "" Then
					v = CInt(GetSQLScalar("Select count(controlid) from blockedcontrols where groupid = " + CStr(sgroup) + " and pageid = " + CStr(p) + " and control = '" + CStr(c.ID) + "'"))
					If v > 0 Then
						c.Visible = False
					End If
				End If
			End If
			If c.Controls.Count > 0 Then
				ControlAccess(sgroup,CPage,c)
			End If
		Next
	End Sub
	Shared Function UploadFile(CReq As HttpRequest,fpath As String)
		Dim upfiles As HttpFileCollection = CReq.Files
		Dim SampFile As HttpPostedFile = upfiles(0)
		
		If Directory.Exists(fpath) <> True Then
			Directory.CreateDirectory(fpath)
		End If
		SampFile.SaveAs(fpath & "\" & System.IO.Path.GetFileName(SampFile.FileName.Replace(" ","")))
		UploadFile = fpath & "\" 
		UploadFile = UploadFile & SampFile.FileName.Substring(SampFile.FileName.LastIndexOf("\") + 1,SampFile.FileName.Length - (SampFile.FileName.LastIndexOf("\") + 1)).ToString()
	End Function
	Shared Sub CleanUploads(fname As String)
		File.Delete(fname)
	End Sub
End Class
