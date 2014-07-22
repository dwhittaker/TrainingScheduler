'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 5/3/2013
' Time: 2:28 PM
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.IO.Directory
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.UserControl
Imports System.Data.SqlClient

	Public Class ESig 
		Inherits UserControl
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected WithEvents mtAr As HiddenField
		Protected WithEvents ltAr As HiddenField
		Protected WithEvents btnSave As Button
		Protected WithEvents nobj As HiddenField
		Protected WithEvents HasSig As HiddenField
		Protected WithEvents SigImg As System.Web.UI.WebControls.Image
		Protected WithEvents btnVerify As Button
		Protected WithEvents btnClear As Button
		Protected WithEvents chkVerify As CheckBox
		Protected mvalue As String
		Protected lvalue As String
		Protected nlvalue As String
		Protected WithEvents delSig As Button
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
		
			'CSM.GetPostBackEventReference(tempX, "")
			
			'

			'------------------------------------------------------------------
			If Not IsPostBack Then
				
			End If
			'Response.Write("<script>this.addEventListener('load',LoadList(),false);</script>")
			'------------------------------------------------------------------
		End Sub
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
			AddHandler Me.btnSave.Click, New System.EventHandler(AddressOf SaveImage)
			'------------------------------------------------------------------
			'------------------------------------------------------------------
		End Sub
		Protected Sub SigExist(sender As Object,e  As EventArgs) Handles mtAr.Load
			Dim CSM As ClientScriptManager = Page.ClientScript
			Dim jscript As String
			If CStr(Session("HasSig")) = "True" Then
				jscript = "document.getElementById('SigCan').style.height = '0px';"
				jscript = jscript + "document.getElementById('SigCan').style.width = '0px';"
				jscript = jscript + "document.getElementById('SigCan').style.visibility='hidden';"
				jscript = jscript + "document.getElementById('OvSig').style.height = '0px';"
				jscript = jscript + "document.getElementById('OvSig').style.width = '0px';"
				jscript = jscript + "document.getElementById('OvSig').style.visibility='hidden';"
''				jscript = jscript + "document.getElementById('Spacer').style.height = '0px';"
''				jscript = jscript + "document.getElementById('Spacer').style.width = '0px';"
''				jscript = jscript + "document.getElementById('Spacer').style.visibility='hidden';"
				CSM.RegisterStartupScript(Page.GetType(),"removediv",jscript,True)
				SigImg.ImageUrl = CStr(Session("SPath")) + "EmpSig.jpg"
				btnSave.Visible = False
				btnClear.Visible = False
				If Not Request.QueryString("candel") Is Nothing Then
					If Request.QueryString("candel").ToString = "yes" Then
						delSig.Visible = True
						btnVerify.Visible = False
						chkVerify.Visible = False
					Else
						delSig.Visible = False
						btnVerify.Visible = True
						chkVerify.Visible = True
					End If
				Else
					delSig.Visible = False
					btnVerify.Visible = True
					chkVerify.Visible = True
				End If
			Else
				SigImg.Visible = False
			End If

		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Sig Related"
		Protected Sub SaveImage(sender As Object, e As EventArgs) 'Handles btnSave.Click
			Dim MoveArray As String()
			Dim LineArray As String()
			Dim NewLArray As String()
			Dim XMove As String
			Dim MoveEl As String()
			Dim LineEl As String()
			Dim NewLEl As String()
			Dim i As Integer
			Dim n As Integer
			Dim newf As Boolean
			Dim pt1 As PointF
			Dim pt2 As PointF
			Dim pt3 As PointF
			Dim pt4 As PointF
			Dim mpt As PointF
			Dim SaveS As New Bitmap(500,100)
			Dim objSig As Graphics = Graphics.FromImage(SaveS)
			Dim SigPen As New Pen(Brushes.Black,4)
			Dim WidPen As New Pen(Brushes.Transparent,1)
			Dim SigPath As New System.Drawing.Drawing2D.GraphicsPath
			Dim SavePath As String	
			
			mvalue = mtAr.Value.ToString()
			lvalue = ltAr.Value.ToString()
			nlvalue = nobj.Value.ToString()
			
			MoveArray = mvalue.Split(CChar("|"))
			LineArray = lvalue.Split(CChar("|"))
			If nlvalue <> "" Then
				NewLArray = nlvalue.Split(CChar("|"))
			End If
		
			i = 0
			n = 0
			
			SaveS.SetResolution(800,800)
			objSig.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
			
'			For Each XMove In MoveArray
'				
''				Sigpath.AddLine(CInt(MoveEl(0)),CInt(MoveEl(1)),CInt(LineEl(0)),CInt(LineEl(1)))
''				If nlvalue <> "" Then
''					NewLEl = NewLArray(n).Split(CChar(","))
''					If (LineEl(0) = NewLEl(0)) And (LineEl(1) = NewLEl(1)) Then
''						'SigPath.CloseFigure()
''						SigPath.StartFigure()
''						If (NewLArray.Length - 1) > n Then
''							n = n + 1
''						End If	
''					End If
''				End If
'				i = i + 1
'			Next XMove
			While i <= LineArray.Length
				If i < MoveArray.Length Then
	
					MoveEl = MoveArray(i).Split(CChar(","))
					pt1 = New PointF(CInt(MoveEl(0)),CInt(MoveEl(1)))
				
					LineEl = LineArray(i).Split(CChar(","))
					pt2 = New PointF(CInt(LineEl(0)),CInt(LineEl(1)))
				
					If i + 2 < LineArray.Length Then
						LineEl = LineArray(i + 1).Split(CChar(","))
						pt3 = New PointF(CInt(LineEl(0)),CInt(LineEl(1)))
				
						LineEl = LineArray(i + 2).Split(CChar(","))
						pt4 = New PointF(CInt(LineEl(0)),CInt(LineEl(1)))
					Else
						pt4 = pt2
						pt3 = pt2
						pt2 = pt1
					End If
					If nlvalue <> "" Then
						newf = False
						While (NewLArray.Length) > n
							NewLEl = NewLArray(n).Split(CChar(","))
							mpt = New PointF(CInt(NewLEl(0)),CInt(NewLEl(1)))
							If pt2 = mpt Then
								pt4 = pt2
								pt3 = pt4
								pt2 = pt1
								newf = True
							Else
								If pt3 = mpt Then
									pt4 = pt3
									pt3 = pt2
									pt2 = pt1
									newf = True
								Else
									newf = True
								End If
							End If
							n = n + 1
						End While
					End If
					n = 0
					SigPath.AddBezier(pt1,pt2,pt3,pt4)
					If newf = True Then
						SigPath.StartFigure()
					End If
				End If
				i = i + 3
			End While 
			
			SavePath = CStr(Session("LocalPath")) + CStr(Session("SPath"))
			
			If System.IO.Directory.Exists(SavePath) Then
				SavePath = SavePath + "EmpSig.jpg"
			Else
				System.IO.Directory.CreateDirectory(SavePath)
				SavePath = SavePath + "EmpSig.jpg"
			End If
			objSig.FillRectangle(Brushes.White,0,0,500,100)
			objSig.DrawPath(SigPen,SigPath)
			
			SaveS.Save(SavePath,ImageFormat.Jpeg)
			
			'SaveS.Save(SavePath)
			
			objSig.Dispose()
			SaveS.Dispose()
			
			SaveAttendance()
			If Not Request.QueryString("skipatt") Is Nothing Then
				If Request.QueryString("skipatt").ToString() <> "yes" Then
					Response.Redirect("RegistryForm.aspx")
				End If
			Else
				Response.Redirect("RegistryForm.aspx")
			End If
		End Sub
	
		Protected Sub VerifySig(sender As Object, e As EventArgs) Handles btnVerify.Click
			SaveAttendance()
			If chkVerify.Checked = True Then
				Response.Redirect("RegistryForm.aspx")
			End If
		End Sub
		Protected Sub DeleteSignature(sender As Object, e As EventArgs) Handles delSig.Click
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim delcmd As New SqlCommand("DeleteSig",sqlconn)
			Dim test As String
			If System.IO.File.Exists(CStr(Session("LocalPath")) + CStr (Session("SPath")) + "EmpSig.jpg") Then
				System.IO.File.Delete(CStr(Session("LocalPath")) + CStr (Session("SPath")) + "EmpSig.jpg")
			End If
			With delcmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@empid",SqlDbType.VarChar)
				.Parameters("@empid").Value = CStr(Session("EmpID"))
				sqlconn.Open
				.ExecuteNonQuery()
			End With
			sqlconn.Close
			delcmd = Nothing
		End Sub
		Protected Sub SaveAttendance()
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim addcmd As New SqlCommand("AddEmpSig",sqlconn)
			Dim updatecmd As New SqlCommand("PassEmp",sqlconn)	 
			If CStr(Session("HasSig")) = "False" Then
				With addcmd
					.CommandType = CommandType.StoredProcedure
					.Parameters.Add("@empid",SqlDbType.VarChar)
					.Parameters.Add("@spath",SqlDbType.VarChar)
					.Parameters("@empid").Value = CStr(Session("EmpID"))
					.Parameters("@spath").Value = CStr(Session("SPath"))
					sqlconn.Open
					.ExecuteNonQuery()
				End With
				addcmd = Nothing
				sqlconn.Close
				If Not Request.QueryString("skipatt") Is Nothing Then
					If Request.QueryString("skipatt").ToString = "yes" Then
						chkVerify.Checked = False
					Else
						chkVerify.Checked = True
					End If
				Else
					chkVerify.Checked = True
				End If
			End If
			If chkVerify.Checked = True Then
				With updatecmd
					.CommandType = CommandType.StoredProcedure
					.CommandType = CommandType.StoredProcedure
					.Parameters.Add("@empID", SqlDbType.NVarChar)
					.Parameters.Add("@ci", SqlDbType.Int) 
					.Parameters.Add("@status",SqlDbType.VarChar)
					.Parameters.Add("@user",SqlDbType.VarChar)
					.Parameters("@empid").Value = CStr(Session("EmpID"))
					.Parameters("@ci").Value  = CStr(Session("strCCode"))
					.Parameters("@status").Value = "R"
					.Parameters("@user").Value = Session("loginusername")
					sqlconn.Open
					.ExecuteNonQuery()
				End With
				updatecmd = Nothing
				sqlconn.Close
			End If
		End Sub
		#End Region
	End Class
