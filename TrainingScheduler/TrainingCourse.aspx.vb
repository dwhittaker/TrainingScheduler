Option Strict On
Option Explicit On
'
' Created by SharpDevelop.
' User: screveling
' Date: 8/7/2012
' Time: 11:20 AM
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.Sqlclient
Imports System.Drawing
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System
Imports TrainingScheduler.Utility

	''' <summary>
	''' Description of WebForm1
	''' </summary>
	Public Class TrainingCourse
		Inherits Page
		
		Protected WithEvents TrainingCourses As DataGrid		
		Protected WithEvents txtTrainingCourseID As HiddenField
		Protected WithEvents txtCourseTitle As TextBox
		Protected WithEvents txtCourseDuration As Textbox
		Protected WithEvents ddlCourseType As DropDownList
		Protected WithEvents butInsert As Button
		Protected WithEvents panAddEdit As Panel
		Protected WithEvents butShowInsert As Button
		Protected WithEvents butInsertUpdate As Button		
		Protected WithEvents butCancel As Button
		Protected WithEvents cvCourseType As CustomValidator
		Protected WithEvents chkActive As CheckBox
		Protected WithEvents txtComments As TextBox
		Protected WithEvents ddlView As DropDownList
		Protected WithEvents rdActive As RadioButton
		Protected WithEvents rdInAct As RadioButton
		Protected WithEvents UpPan1 As UpdatePanel
		Protected WithEvents UpPan2 As UpdatePanel
		Protected WithEvents ddlCourseCat As DropDownList
		#Region "The stuff you don't need to see"

		Protected Sub PageInit(sender As Object, e As System.EventArgs)
			CheckLogin(Cbool(Session("login")),Me.Response)
		End Sub
		'----------------------------------------------------------------------
		Protected Sub PageExit(sender As Object, e As System.EventArgs)
		End Sub
		
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
			AddHandler Me.UpPan1.PreRender, New System.EventHandler(AddressOf RefreshGrid)
		End Sub
		Protected Sub PreRend(sender As Object,e As System.EventArgs)
			ControlAccess(CInt(Session("SecurityGroupID")),System.Web.HttpContext.Current.Request.Path.ToString(),Me.Form)
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		
		Private Sub Page_Load(sender As Object, e As System.EventArgs)
			'------------------------------------------------------------------
			Utility.CheckLogin(CBool(Session("login")), Me.Response)			
			If Not IsPostBack Then
				PageSecurity()
				With panAddEdit
					.Visible=False
					.BorderColor = Color.Black
					.BorderStyle = BorderStyle.Solid
					.BorderWidth = Unit.Pixel(1)
				End With
				
				With ddlCourseType
					.DataSource = Utility.GetDataView( _
						"select CourseTypeID, Description from CourseType order by Description")
					.DataValueField = "CourseTypeID"
					.DataTextField = "Description"
					.DataBind
					.Items.Insert(0, New ListItem("Select...", "-1"))
					.SelectedValue = "-1"
				End With
				chkActive.Checked = True
				
				ddlView.DataSource = GetDataView("select coursecatid,category from coursecategory")
				ddlView.DataValueField = "coursecatid"
				ddlView.DataTextField = "Category"
				ddlView.DataBind
				ddlView.Items.Insert(0, New ListItem("All Categories","-1"))
				ddlView.SelectedValue = "-1"
				
				ddlCourseCat.DataSource = GetDataView("select coursecatid,category from coursecategory")
				ddlCourseCat.DataValueField = "coursecatid"
				ddlCourseCat.DataTextField = "Category"
				ddlCourseCat.DataBind
				ddlCourseCat.Items.Insert(0, New ListItem("None Selected","-1"))
				ddlCourseCat.SelectedValue = "-1"
				
				GridLoad()		
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
		Sub GridLoad()
			Dim sql As String
			sql ="select TrainingCourseID, CourseTitle, CourseDuration, tc.CourseTypeID, ct.Description,Comments,tc.CatID,cc.Category,Active from TrainingCourse tc " & _
				"left join CourseType ct on tc.CourseTypeID = ct.CourseTypeID " & _
				"left join CourseCategory cc on tc.CatID = cc.CourseCatID "
			
			If ddlView.SelectedValue <> "-1" Then
				sql = sql + "Where cc.Category Like '" + ddlView.SelectedItem.Text + "' and Active = "
			Else
				sql = sql + "Where cc.Category Like '%' and Active = "
			End If
			
			If rdActive.Checked = True Then
				sql = sql + "1 order by CourseTitle"
			Else
				sql = sql + "0 order by CourseTitle"
			End If
			
			TrainingCourses.datasource = Utility.GetDataView(sql)
			TrainingCourses.databind
		End Sub
		
		Protected Sub TrainingCourses_ItemDataBound(Sender As Object, e As DataGridItemEventArgs) Handles TrainingCourses.ItemDataBound
			If e.Item.ItemIndex <> -1 And e.Item.ItemIndex = TrainingCourses.EditItemIndex Then
				Dim ddl As DropDownList = CType(e.Item.FindControl("ddlCourseType"), DropDownList)
				
				With ddl
					.DataSource = Utility.GetDataView( _
						"select CourseTypeID, Description from CourseType order by Description")
					.DataValueField = "CourseTypeID"
					.DataTextField = "Description"
					.DataBind					
					.SelectedValue = DataBinder.Eval(e.Item.DataItem, "CourseTypeID").ToString()
					
				End With				
				UpPan2.Update()
			End If
			If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.SelectedItem Then
				e.Item.Cells(11).Attributes.Add("onClick", "return ConfirmDeletion();")
			End If
		End Sub
		
		Protected Function ParseDuration(s As String) As string
			Dim d As Double = Double.Parse(s)
			Return d.ToString("f2")
		End Function
		
		Protected Sub butShowInsert_Click(sender As Object, e As EventArgs) Handles butShowInsert.Click
			panAddEdit.Visible = True
			txtTrainingCourseID.value = "-1"
			txtCourseTitle.Text = ""
			txtCourseDuration.Text = ""
			ddlCourseType.SelectedValue = "-1"
			ddlCourseCat.SelectedValue = "-1"
		End Sub
		
		Sub eventHandlerName(sender as Object, e as DataGridCommandEventArgs) Handles TrainingCourses.ItemCommand
			If e.CommandName = "Edit" Then
				panAddEdit.Visible = True
				UpPan1.Update()
				txtTrainingCourseID.value = e.Item.Cells(0).Text
				txtCourseTitle.Text = e.Item.Cells(3).Text
				txtCourseDuration.Text = e.Item.Cells(2).Text
				ddlCourseType.selectedvalue = e.Item.Cells(1).Text	
				ddlCourseCat.SelectedValue = e.Item.Cells(5).Text
				If e.Item.Cells(8).Text.ToString() = "&nbsp;" Then
					txtComments.Text = ""
				Else
					txtComments.Text = e.Item.Cells(8).Text.ToString()
				End If
				
				If e.Item.Cells(9).Text.ToString = "True" Then
					chkActive.Checked = True
				Else	
					chkActive.Checked = False
				End If
				UpPan2.Update
			ElseIf e.CommandName = "Delete" Then
				Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
				Dim delcmd As New SqlCommand("DeleteTrainingCourse",sqlconn)
				Dim ccount As Integer
				Dim tcid As Integer
				
				tcid = CInt(e.Item.Cells(0).Text)
				
				ccount = CInt(GetSQLScalar("select count(trainingcourseid) from courseinstance where TrainingCourseID = " + CStr(tcid)))
				
				If ccount = 0 Then
					With delcmd
						.CommandType = CommandType.StoredProcedure
						.Parameters.Add("@tcid",SqlDbType.Int)
						.Parameters("@tcid").Value = tcid
						sqlconn.Open
						.ExecuteNonQuery()
					End With
					sqlconn.Close
					delcmd = Nothing
					UpPan2.Update()
				End If
			End If
		End Sub
		
		Sub InsertUpdateRecordClicked(sender As Object, e As EventArgs) Handles butInsertUpdate.Click
			If Page.IsValid Then
				If txtTrainingCourseID.value = "-1" Then
					InsertTrainingCourse(txtCourseTitle.Text, txtCourseDuration.Text, ddlCourseType.SelectedValue)
				Else
					UpdateTrainingCourse(txtTrainingCourseID.value, txtCourseTitle.Text, txtCourseDuration.Text, ddlCourseType.SelectedValue)
				End If
				GridLoad()
				panAddEdit.Visible = False
				UpPan2.Update()
			End If
		End Sub
		
		Sub CancelClicked(s As Object, e As EventArgs) Handles butCancel.Click
			panAddEdit.Visible = False
		End Sub
		
		Sub ValidateCourseType(sender As Object, e As ServerValidateEventArgs) Handles cvCourseType.ServerValidate
			If e.Value = "-1" Then
				e.IsValid=False
			Else
				e.IsValid=True
			End If
		End Sub
		
		Protected Sub UpdateTrainingCourse(TrainingCourseID As String, CourseTitle As String, Duration As String, CourseTypeID As string)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim updatecmd As New SqlCommand("UpdateTrainingCourse",sqlconn)
			
			With updatecmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@trainingcourseid", SqlDbType.Int)
				.Parameters.Add("@coursetitle", SqlDbType.VarChar)
				.Parameters.Add("@courseduration", SqlDbType.Decimal)
				.Parameters.Add("@coursetypeid", SqlDbType.Int)
				.Parameters.Add("@comments",SqlDbType.Text)
				.Parameters.Add("@active",SqlDbType.Bit)
				.Parameters.Add("@coursecatid",SqlDbType.Int)
				.Parameters("@trainingcourseid").Value = TrainingCourseID
				.Parameters("@coursetitle").Value  = CourseTitle.trim
				.Parameters("@courseduration").Value  = Double.Parse(duration)
				.Parameters("@coursetypeid").Value = Integer.Parse(coursetypeid)
				.Parameters("@comments").Value = txtComments.Text.ToString()
				If chkActive.Checked = True Then
					.Parameters("@active").Value = 1
				Else	
					.Parameters("@active").value = 0
				End If
				.Parameters("@coursecatid").Value = ddlCourseCat.SelectedValue
				sqlconn.Open
				.ExecuteNonQuery()				
			End With
			updatecmd = Nothing
			sqlconn.Close			
		End Sub
		
		Protected Sub InsertTrainingCourse(CourseTitle As String, Duration As String, CourseTypeID As string)
			Dim sqlconn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("ConnectionString"))
			Dim insertcmd As New SqlCommand("CreateTrainingCourse",sqlconn)
			
			With insertcmd
				.CommandType = CommandType.StoredProcedure
				.Parameters.Add("@coursetitle", SqlDbType.VarChar) 
				.Parameters.Add("@courseduration", SqlDbType.Decimal) 
				.Parameters.Add("@coursetypeid", SqlDbType.Int)	
				.Parameters.Add("@comments", SqlDbType.Text)
				.Parameters.Add("@coursecatid",SqlDbType.Int)
				.Parameters("@coursetitle").Value  = CourseTitle.trim
				.Parameters("@courseduration").Value  = Double.Parse(duration)
				.Parameters("@coursetypeid").Value = Integer.Parse(coursetypeid)
				.Parameters("@comments").Value = txtComments.Text.ToString()
				.Parameters("@coursecatid").Value = ddlCourseCat.SelectedValue
				sqlconn.Open
				.ExecuteNonQuery()
			End With
			insertcmd = Nothing
			sqlconn.Close			
		End Sub
		Protected Sub RefreshGrid(sender As Object,e As EventArgs)
			GridLoad()
		End Sub
	End Class
