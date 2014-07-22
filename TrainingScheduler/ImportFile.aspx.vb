'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 7/18/2013
' Time: 3:17 PM
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
Imports System.IO
Imports System.Text.RegularExpressions
	''' <summary>
	''' Description of ImportFile
	''' </summary>
	Public Class ImportFile
		Inherits Page
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected WithEvents fuFileUpload As FileUpload
		Protected WithEvents ddlImpType As DropDownList
		Protected WithEvents btnUploadFile As Button
		Dim ImpField As New ArrayList()
		Dim strSQL As String
		Dim intEid As Integer
		Dim Err As New ArrayList()
		Dim RErr As New ArrayList()
		Dim ecount As Integer
		Dim strRunSql As String
		Dim strfound As String
		
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Page Init & Exit (Open/Close DB connections here...)"

		Protected Sub PageInit(sender As Object, e As System.EventArgs)
			CheckLogin(Cbool(Session("login")),Me.Response)	
		End Sub
		'----------------------------------------------------------------------
		Protected Sub PageExit(sender As Object, e As System.EventArgs)
		End Sub

		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Page Load"
		Private Sub Page_Load(sender As Object, e As System.EventArgs)
			'------------------------------------------------------------------
			If fuFileUpload.FileName <> "" Then
				Session("SampleFile") = fuFileUpload.PostedFile.FileName 'fuSampleFile.FileName
			End If
			If Not IsPostBack Then
				PageSecurity()
				ddlImpType.DataSource = GetDataView("Select * from Import where active = 1")
				ddlImpType.DataValueField = "ImportID"
				ddlImpType.DataTextField = "ImportName"
				ddlImpType.DataBind()
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
			'------------------------------------------------------------------
		End Sub
		Protected Sub PreRend(sender As Object,e As System.EventArgs)
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Generate Import"
		Protected Sub GetImportFields()	
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim ddefcmd As New SqlCommand("Select Data from ImportDataDef where ImportID = " + ddlImpType.SelectedValue.ToString() + " and (data like '{Ex:%' or data like '%{Ex:%')",sqlconn)
			Dim cdefcmd As New SqlCommand("Select ImportCriteriaField,Criteria from ImportCriteriaDef where ImportID = " + ddlImpType.SelectedValue.ToString(),sqlconn)
			Dim ddr As SqlDataReader
			Dim i As Integer
			Dim carray() As String
			Dim calarray() As String
			Dim strItem As String
			Dim strField As String
			Dim j As Integer
			j = 1
			ddefcmd.Connection.Open
			i = 0
			strfound = ""
			ddr = ddefcmd.ExecuteReader
			While ddr.Read()
				If strfound.IndexOf(ddr.GetString(0)) = -1 Then
					If ddr.GetString(0).IndexOf("{SQL") > -1 Then
						calarray = Regex.Split(ddr.GetString(0).Replace("{SQL",""),"{|}")
						For Each strItem In calarray
							If strItem.IndexOf("Ex:") > -1 Then
								strField = "{" + strItem + "}"
								If strfound.IndexOf(strField) = -1 Then
									ImpField.Add(i)
									ImpField(i) = strField
									i = i + 1
									strfound = strfound + strField
								End If
							End If
						Next
					Else
						ImpField.Add(i)
						ImpField(i) = ddr.GetString(0)
						strfound = strfound + ddr.GetString(0)
						i = i + 1
					End If
				End If

				'CSVData.Add(i)
				'CSVData(i) = ""
			End While
			ddefcmd.Connection.Close
			ddr = Nothing
			cdefcmd.Connection.Open
			ddr = cdefcmd.ExecuteReader
			While ddr.Read()
				carray = Regex.Split(ddr.GetString(1),"{|}")
				For Each strItem In carray
					If strItem.IndexOf("Ex:") > -1 Then
						'test1 = strItem
						'test1 = strItem.IndexOf("Ex:")
						strField = "{" + strItem + "}"
						If strfound.IndexOf(strField) = -1 Then
							ImpField.Add(i)
							ImpField(i) = "{" + strItem + "}"
							strfound = strfound + strField
							i = i + 1
						End if
					End If
				Next strItem
			End While
		End Sub
		Protected Sub BuildSQL()
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim ddefcmd As New SqlCommand("Select ImportColumn,Data from ImportDataDef where ImportID = " + ddlImpType.SelectedValue.ToString(),sqlconn)
			Dim cdefcmd As New SqlCommand("Select ImportCriteriaField,Criteria from ImportCriteriaDef where ImportID = " + ddlImpType.SelectedValue.ToString(),sqlconn)
			Dim strAction As String
			Dim strTable As String
			Dim ddr As SqlDataReader
			Dim intTrim As Integer
			Dim intTest As Integer
			
			strAction = CStr(GetSQLScalar("Select ImportAction from Import where ImportID = " + ddlImpType.SelectedValue.ToString()))
			strTable = CStr(GetSQLScalar("Select TableName from ImportType where ImportTypeID in (select importtypeid from import where importid = " + ddlImpType.SelectedValue.ToString() + ")"))
			
			If strAction = "Update" Then
				strSQL = strAction + " " + strTable + " Set "
			End If
			
			ddefcmd.Connection.Open
			ddr = ddefcmd.ExecuteReader
			
			While ddr.Read
				If Integer.TryParse(ddr.GetString(1),intTest) Then
					strSQL = strSQL + ddr.GetString(0) + " = " + ddr.GetString(1) + ", "
				ElseIf ddr.GetString(1).IndexOf("{Ex:") > -1
					strSQL = strSQL + ddr.GetString(0) + " = " + ddr.GetString(1) + ", "
				ElseIf ddr.GetString(1).IndexOf("{SQL:") > -1
					strSQL = strSQL + ddr.GetString(0) + " = " + ddr.GetString(1) + ", "
				Else
					strSQL = strSQL + ddr.GetString(0) + " = '" + ddr.GetString(1) + "', "
				End If				
			End While

			ddefcmd.Connection.Close
			ddr = Nothing
			
			intTrim = strSQL.Length - strSQL.LastIndexOf(",")
			strSQL = strSQL.Substring(0,strSQL.Length - (intTrim))
			
			cdefcmd.Connection.Open
			ddr = cdefcmd.ExecuteReader
			
			If ddr.FieldCount > 0 Then
				strSQL = strSQL + " Where "
			End If
			
			While ddr.Read
				strSQL = strSQL + ddr.GetString(0) + " " + ddr.GetString(1) + " And "
			End While
			
			intTrim = strSQL.Length - strSQL.LastIndexOf("And")
			strSQL = strSQL.Substring(0,strSQL.Length - (intTrim + 1))
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Run Import"
		Protected Sub FileImport(sender As Object, e As System.EventArgs) Handles btnUploadFile.Click
			Dim action As String
			
			ecount = 0
			
			action = CStr(GetSQLScalar("Select ImportAction from Import where importid = " + CStr(ddlImpType.SelectedValue)))
			
			GetImportFields
			ImpRawData
			
			If action <> "Update" And action <> "Insert" And action <> "Delete" Then
				SPImport(action)
			Else
				BuildSQL
				TableImport(action)
			End If
			
			
		End Sub
		Protected Sub SPImport(action As String)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim sqlconn2 As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim sqlconn3 As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim Rcmd As New SqlCommand("Select Distinct Recordid from ImportRawData order by recordid",sqlconn)
			Dim drRecords As SqlDataReader
			Dim Fcmd As New SqlCommand()
			Dim drFields As SqlDataReader
			Dim cbcmd As New SqlCommand(action,sqlconn3)
			Dim spcmd As New SqlCommand(action, sqlconn3)
			Dim result As Object
			Dim test As Boolean
			
			cbcmd.CommandType = CommandType.StoredProcedure
			Fcmd.Connection = sqlconn2
			
			Fcmd.CommandText = "Select ImportColumn,Data from ImportDataDef where ImportID = " + ddlImpType.SelectedValue.ToString()
			
			spcmd.CommandType = CommandType.StoredProcedure
			
			sqlconn.Open
			drRecords = Rcmd.ExecuteReader
			While drRecords.Read
				sqlconn2.Open()
				drFields = Fcmd.ExecuteReader
				While drFields.Read
					If drFields.GetString(1).IndexOf("{SQL") > -1 Then
							strRunSql = ""
							strSQL = drFields.GetString(1)
							ReplaceCalcFields(drRecords.GetInt32(0),action,drFields.GetString(0))
							If result <> "NULL" Then
								result = strRunSql.Replace("'","")
							Else 
								result = "Null"
							End If 
					ElseIf drFields.GetString(1).Length > 0 Then
						If drFields.GetString(1).IndexOf("{Ex:") > -1 Then
							test = "Select Data from ImportRawData where RecordID = " + CStr(drRecords.GetInt32(0)) + " and ColumnName = " + CStr(drFields.GetString(1))
							result = GetSQLScalar(CStr("Select Data from ImportRawData where RecordID = " + CStr(drRecords.GetInt32(0)) + " and ColumnName = '" + CStr(drFields.GetString(1))+ "'"))
						Else
							result = drFields.GetString(1)
						End If
						
					Else
						result = "Null"
					End If
					If result <> "Null" AND result <> "NULL" Then
						sqlconn3.Open()
						SqlCommandBuilder.DeriveParameters(cbcmd)
						sqlconn3.Close()
						If cbcmd.Parameters(drFields.GetString(0)).SqlDbType.ToString = "Bit" Then
							spcmd.Parameters.AddWithValue(drFields.GetString(0),CBool(result))
						Else
							spcmd.Parameters.AddWithValue(drFields.GetString(0),result)
						End if 
						spcmd.Parameters(drFields.GetString(0)).SqlDbType = cbcmd.Parameters(drFields.GetString(0)).SqlDbType
						'test = cbcmd.Parameters(drFields.GetString(0)).SqlDbType.ToString()
					Else
						'test = drFields.GetString(0)
						spcmd.Parameters.AddWithValue(drFields.GetString(0),DBNull.Value)
						spcmd.Parameters(drFields.GetString(0)).SqlDbType = cbcmd.Parameters(drFields.GetString(0)).SqlDbType
						'test = cbcmd.Parameters(drFields.GetString(0)).SqlDbType.ToString()
					End If
					
				End While
				sqlconn3.Open
				Try
					spcmd.ExecuteNonQuery()
				Catch ex As Exception
					AddError(drRecords.GetInt32(0),"Error: " + ex.ToString())
				End Try
				
				sqlconn3.Close
				sqlconn2.Close()
				spcmd.Parameters.Clear()
			End While
			sqlconn.Close()
			Fcmd = Nothing
			Rcmd = Nothing
			spcmd = Nothing
			cbcmd = Nothing
			FillErrorLog()
			Response.Write("<script>alert('Data uploaded')</script>")
		End Sub
		Protected Sub TableImport(action As String)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim sqlconn2 As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim sqlconn3 As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim Rcmd As New SqlCommand("Select Distinct Recordid from ImportRawData order by recordid",sqlconn)
			Dim Fcmd As New SqlCommand()
			Dim Ecmd As New SqlCommand()
			Dim drFields As SqlDataReader
			Dim drRecords As SqlDataReader
			Dim intTest As Integer
			Dim strDat As New ArrayList()
			Dim strFel As New ArrayList()
			Dim dcount As Integer
			Dim ccount As Integer
			Dim strerr As String
			Dim i As Integer
			intEid = 1
			dcount = 0
			
			Fcmd.Connection = sqlconn2
			'Ecmd.Connection = sqlconn2
			
			sqlconn.Open
			drRecords = Rcmd.ExecuteReader
			While drRecords.Read
				strRunSql = strSQL
				ReplaceCalcFields(CStr(drRecords.GetInt32(0)),action,"")
				Fcmd.CommandText = "Select Distinct ColumnName,Data from ImportRawData where RecordID = " + CStr(drRecords.GetInt32(0).ToString)
				sqlconn2.Open
				drFields = Fcmd.ExecuteReader
				While drFields.Read				
					If Integer.TryParse(drFields.GetString(1).ToString,intTest) Then
						strRunSql = strRunSql.ToLower.Replace(drFields.GetString(0).ToLower,drFields.GetString(1).ToString)	
					Else
						strRunSql = strRunSql.ToLower.Replace(drFields.GetString(0).ToLower,"'" + drFields.GetString(1).ToString.Replace("'","''") + "'")
					End If
					'strFel.Add(dcount)
					'strDat.Add(dcount)
					'strFel(dcount) = drFields.GetString(0)
					'strDat(dcount) = drFields.GetString(1)
					dcount = drRecords.GetInt32(0)
				End While
				sqlconn2.Close
				Fcmd.CommandText = strRunSQL.Replace("status = 'a'","status = 'A'")
				sqlconn2.Open
				ccount = Fcmd.ExecuteNonQuery
				sqlconn2.Close
				
				If action = "Update" Then
					strerr = "Error: Could not find matching record to update"
				Else If action = "Insert" Then
					strerr = "Error: " + strRunSql
				End If
				
				If ccount < 1 Then
					'For i = 0 To dcount - 1
						'Ecmd.CommandText = "Insert Into ImportErrors(RecordID,ColumnName,Data) Values(" + CStr(intEid) + ",'" + strFel(i).ToString() + "','" + strDat(i).ToString() + "','" + strerr + "' )"
						'Ecmd.CommandText = "Insert Into ImportErrors(RecordID,ColumnName,Data) Select RecordID,ColumnName,Data from ImportRawData where RecordID = " + CStr(dcount)
						'sqlconn2.Open
						'Ecmd.ExecuteNonQuery()
						'sqlconn2.Close
						
						'Ecmd.CommandText = "Update ImportErrors set error = '" + strerr + "' where RecordID = " + CStr(dcount)
						'sqlconn2.Open
						'Ecmd.ExecuteNonQuery
						'sqlconn2.Close
					'Next
					'RErr.Add(ecount)
					'Err.Add(ecount)
					'RErr(ecount) = dcount
					'Err(ecount) = strerr
					'ecount = ecount + 1
					AddError(dcount,strerr)
				End If
				strFel.Clear()
				strDat.Clear()
				dcount = 0	
			End While
			
			FillErrorLog()
			sqlconn.Close		
			Fcmd = Nothing
			Response.Write("<script>alert('Data uploaded')</script>")
		End Sub
		Protected Sub ImpRawData()
			Dim l As Integer
			Dim f As Integer
			Dim c As Integer
			Dim offset As Integer
			Dim len As Integer
			Dim strlines() As String
			Dim strcols() As String
			Dim strData() As String
			Dim strFind As String
			Dim Field As String
			Dim bolqual As Boolean
			Dim fulldat As String
			Dim test As String
			Dim fname As String
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim cmd As New SqlCommand("InsertRawData",sqlconn)
			
			With cmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@rid",SqlDbType.Int)
				.Parameters.Add("@cname",SqlDbType.VarChar)
				.Parameters.Add("@data",SqlDbType.VarChar)
			End With
			
			c = 0
			len = 0
			f = 0
			
			ClearTables()
			
			If Session("SampleFile").ToString <> "" Then
				fname = UploadFile(Request,"C:\Inetpub\Training\TrainingScheduler\Uploads")
				Dim strReader As StreamReader = File.OpenText(fname)
				strlines = strReader.ReadToEnd().Split(Environment.NewLine)
				strcols = strlines(0).Split(",")
				c = strcols.GetLength(len)
				For l = 1 To strlines.GetLength(len) - 2
					c = 0
					f = 0
					offset = 0
					strData = strlines(l).Split(",")
					For Each strFind In ImpField
						Field = strFind.Substring(4,strFind.ToString.Length - 5).ToLower
						For f = 0 To strcols.GetLength(c) - 1
							If strData(f).IndexOf("""") = 0 Then
								test = strData(f).IndexOf("""")
								bolqual = True
								fulldat = strData(f)
								Do Until bolqual = False
									offset = offset + 1
									fulldat = fulldat + "," + strData(f + offset)
									If strData(f + offset).IndexOf("""") > -1 Then
										'fulldat = fulldat + "," + strData(f)
										fulldat = fulldat.Replace("""","")
										bolqual = False
									End If
								Loop
							End If
							If Field = strcols(f).ToLower Then
								With cmd
									.Parameters("@rid").Value = l
									.Parameters("@cname").Value = strFind
									If strData(f).IndexOf("""") = 0
										'fulldat = fulldat.Replace("\","")
										.Parameters("@data").Value = fulldat
									Else
										.Parameters("@data").Value = strData(f + offset).Trim()
									End If
									offset = 0
									sqlconn.Open
									.ExecuteNonQuery
								End With
								strReader.Close
								sqlconn.Close
								Exit For
							End If	
						Next
						f = 1
					Next strFind
				Next l
			End If	
			
			CleanUploads(fname)
		End Sub
		Protected Sub ReplaceCalcFields(CID As Integer, action As String,pname As String)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim sqlconn2 As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim ddefcmd As New SqlCommand()
			Dim fcmd As New SqlCommand("Select Distinct ColumnName,Data from ImportRawData where RecordID = " + CStr(CID),sqlconn2)
			Dim ecmd As New SqlCommand()
			Dim drdata As SqlDataReader
			Dim drfields As SqlDataReader			
			Dim tempcalc As String
			Dim intTest As Integer
			Dim test As String
			Dim decTest As String
			Dim result As Object
			Dim strMess As String
			Dim message As String
		
			
			
			If action <> "Update" And action <> "Insert" And action <> "Delete" Then
				ddefcmd.CommandText = "Select ImportColumn,Data from ImportDataDef where ImportID = " + ddlImpType.SelectedValue.ToString() + " and ImportColumn = '" + pname + "'"
			Else
				ddefcmd.CommandText = "Select ImportColumn,Data from ImportDataDef where ImportID = " + ddlImpType.SelectedValue.ToString() + " and data like '{SQL:%'"
			End If
			
			ddefcmd.Connection = sqlconn
			
			
			
			sqlconn.Open
			drdata = ddefcmd.ExecuteReader
			While drdata.Read()
				tempcalc = drdata.GetString(1)
				sqlconn2.Open
				drfields = fcmd.ExecuteReader
				While drfields.Read()
					tempcalc = tempcalc.ToLower()
					If Integer.TryParse(drfields.GetString(1).ToString,intTest) Then
						tempcalc = tempcalc.Replace(drfields.GetString(0).ToLower(),drfields.GetString(1).ToString)	
					Else
						tempcalc = tempcalc.Replace(drfields.GetString(0).ToLower(),"'" + drfields.GetString(1).ToString + "'")
					End If
				End While
				sqlconn2.Close
				tempcalc = tempcalc.Substring(5,tempcalc.Length - 6)
				'tempcalc = tempcalc.Substring(0,tempcalc.Length - 1)
				Try
					result = GetSQLScalar(tempcalc)
				Catch ex As Exception
					AddError(CID,"Error:" + tempcalc.Replace("'","''"))
				End Try
				
				If result is DBNull.Value Then
					result = "NULL"
				Else
					result = CStr(result)
				End If
				If Integer.TryParse(result,intTest) Or Decimal.TryParse(result,decTest) Then
					strRunSQL = strSQL.Replace(drdata.GetString(1),CStr(result))
				ElseIf result = "NULL"
					strRunSQL = strSQL.Replace(drdata.GetString(1),CStr(result))
				Else
					strRunSQL = strSQL.Replace(drdata.GetString(1),"'" + CStr(result) + "'")		
				End if
			End While
			sqlconn.Close
			drdata = Nothing
			drfields = Nothing
			ddefcmd = Nothing
		End Sub
		Public Sub ClearTables
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim trunrd As New SqlCommand("truncate table importrawdata",sqlconn)
			Dim trunie As New SqlCommand("truncate table importerrors",sqlconn)
			
			sqlconn.Open()
			
			trunrd.ExecuteNonQuery()
			trunie.ExecuteNonQuery()
			
			trunrd = Nothing
			trunie = Nothing
			
			sqlconn.Close()
		End Sub
		Public Sub FillErrorLog()
			Dim i As Integer
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim Ecmd As New SqlCommand("ErrorVerify",sqlconn)
			Dim crito As Integer
			
			crito = GetSQLScalar("Select Count(ImpDataFID) from ImportDataDef where Verify = 1")
			
			If crito > 0  Then
				crito = 0
			Else
				crito = 1
			End If
			
			With Ecmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@impid",SqlDbType.Int)
				.Parameters.Add("@recid",SqlDbType.Int)
				.Parameters.Add("@critonly",SqlDbType.Int)
			End With
			
			For i = 0 To ecount - 1
				
				
				With Ecmd
					.Parameters("@impid").Value = ddlImpType.SelectedValue
					.Parameters("@recid").Value = RErr(i)
					.Parameters("@critonly").Value = crito
					sqlconn.Open
					.ExecuteNonQuery
				End With
				
				sqlconn.Close
				
'				Ecmd.CommandText = "Insert Into ImportErrors(RecordID,ColumnName,Data) Select RecordID,ColumnName,Data from ImportRawData where RecordID = " + CStr(RErr(i))
'				sqlconn.Open
'				Ecmd.ExecuteNonQuery()
'				sqlconn.Close
'				
'				
'				Ecmd.CommandText = "Update ImportErrors set error = '" + Err(i) + "' where RecordID = " + CStr(RErr(i))
'				sqlconn.Open
'				Ecmd.ExecuteNonQuery
'				sqlconn.Close

				
			Next
			Ecmd = Nothing
		End Sub
		Public Sub AddError(rid As Integer,Ierr As String)
			RErr.Add(ecount)
			Err.Add(ecount)
			RErr(ecount) = rid
			Err(ecount) = Ierr
			ecount = ecount + 1
		End Sub
		#End Region
	End Class
