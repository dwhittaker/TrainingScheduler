'
' Created by SharpDevelop.
' User: dwhittaker
' Date: 11/27/2013
' Time: 4:03 PM
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
Imports TrainingScheduler.Utility

	''' <summary>
	''' Description of ddlSearch
	''' </summary>
	<ValidationProperty("SeletedValue")>
	Public Class ddlSearch
		Inherits UserControl
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Data"
		Protected WithEvents ddlList As DropDownList
		Protected WithEvents txtSearch As TextBox
		Protected strDatasource As String
		Protected strVField As String
		Protected strTField As String
		Public Event IndChange(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Protected Spacer As HtmlControl
		Protected UpPan2 As UpdatePanel

		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Page Init & Exit (Open/Close DB connections here...)"

		Protected Sub PageInit(sender As Object, e As System.EventArgs)
			txtSearch.Attributes.Add("OnKeyUp","UpdateTimer" + txtSearch.ClientID.ToString() + "(this.value);")
		End Sub
		'----------------------------------------------------------------------
		Protected Sub PageExit(sender As Object, e As System.EventArgs)
		End Sub

		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Page Load"
		Private Sub Page_Load(sender As Object, e As System.EventArgs)
			
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Properties"
		Public ReadOnly Property SeletedValue
			Get
				Dim value As String = ddlList.SelectedValue.ToString
				If value = "-1" Then
					Return ""
				Else
					Return value
				End If
			End Get
		End Property
		Public Property ConWidth As Integer
			Get
				Return ddlList.Width.Value
			End Get
			Set(ByVal value As Integer)
				ddlList.Width = value
				txtSearch.Width = value - 16
			End Set
		End Property
		Public Property CtrlDataSource As String
			Get
				Return strDatasource
			End Get
			Set(ByVal value As String)
				strDatasource = value
			End Set
		End Property
		Public Property CtrlVField As String
			Get
				Return strVField
			End Get
			Set(ByVal value As String)
				strVField = value
			End Set
		End Property
		Public Property CtrlTField As String
			Get
				Return strTField
			End Get
			Set(ByVal value As String)
				strTField = value
			End Set
		End Property
		Public ReadOnly Property CtrlSelText As String
			Get
				Return ddlList.SelectedItem.Text.ToString()
			End Get
		End Property
		Public Property  CtrlSelValue As String
			Get
				Return ddlList.SelectedValue.ToString()
			End Get
			Set(ByVal value As String)
				ddlList.SelectedValue = value
				txtSearch.Text = ddlList.SelectedItem.Text
			End Set
		End Property
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		#Region "Ctrl Procedures"
		Public Sub CtrlBind()
			'Dim lstItem As ListItem
			ddlList.DataSource = GetDataView(strDatasource)
			ddlList.DataValueField = strVField
			ddlList.DataTextField = strTField
			ddlList.DataBind()
			
			ddlList.Items.Insert(0, New ListItem("None selected","-1"))
			ddlList.SelectedValue = "-1"
			
		End Sub
		Public Sub SetTxtFocus()
			txtSearch.Focus()
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
			AddHandler Me.ddlList.SelectedIndexChanged, New System.EventHandler(AddressOf SelChanged)
			'------------------------------------------------------------------
			'------------------------------------------------------------------
		End Sub
		Protected Sub SelChanged(ByVal sender As Object,ByVal e As System.EventArgs) 
			txtSearch.Text = ddlList.SelectedItem.Text
			RaiseEvent IndChange(sender, e)
		End Sub
		#End Region
		'<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

	End Class
