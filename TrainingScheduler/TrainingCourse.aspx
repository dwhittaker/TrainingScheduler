<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.TrainingCourse"
	ValidateRequest    = "false"
	EnableSessionState = "true"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
		<table>
			<tr><td><a href="Config.aspx">Back to Course Calendar</a><hr></tr></td>
			<tr><td><asp:hyperlink id="hlCCat" NavigateUrl="CourseCategory.aspx" text="Add course categories" runat="server"/><hr></tr></td>
		</table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
	<script language="JavaScript"> 
       	function ConfirmDeletion() 
      	{
        	return confirm('Are you sure you wish to delete this record?');
    	}
   	</script>
	<h1>Training Courses</h1>							
	<asp:updatepanel id="UpPan1" runat="Server" UpdateMode="Conditional" ChildrenAsTriggers="False">
		<ContentTemplate>
			<p>
			<asp:button id="butShowInsert" runat="server" text="Create a training course" />			
			<asp:panel id="panAddEdit" runat="server">
				<h2>Add/edit a course</h2>
				<asp:hiddenfield id="txtTrainingCourseID" runat="server" />
				<table><tr><td>
				Course title: </td><td><asp:textbox id="txtCourseTitle" runat="server" width = 500/>
				<asp:RequiredFieldValidator id="rfvCourseTitle" runat="server" cssclass="Validator" controltovalidate="txtCourseTitle" ErrorMessage="Please enter a course title" display="Dynamic"/></td></tr><tr><td>
				
				Course duration: </td><td><asp:textbox id="txtCourseDuration" runat="server" />
				<asp:RegularExpressionValidator ID="revCourseDuration" runat="server" CssClass="Validator"
					ErrorMessage="Please enter a number such as 2.5" ControlToValidate="txtCourseDuration" 
					ValidationExpression="^\d*\.?\d*$" display="Dynamic"/>
				<asp:RequiredFieldValidator ID="rfvCourseDuration" runat="server" cssclass="Validator" controltovalidate="txtCourseDuration" errormessage="Please enter the course duration." display="Dynamic" /></td></tr><tr><td>
					
					
				Course type: </td><td><asp:dropdownlist id="ddlCourseType" runat="server" />
				<asp:CustomValidator id="cvCourseType" runat="server" cssclass="Validator" controltovalidate="ddlCourseType" ErrorMessage="Please select a course type." display="Dynamic"/>
				</td></tr>
				<tr><td>Course Category:</td><td><asp:dropdownlist id="ddlCourseCat" runat="server"/><asp:CustomValidator id="cvCourseCat" runat="server" cssclass="Validator" controltovalidate="ddlCourseCat" ErrorMessage="Please select a course type." display="Dynamic"/></tr></td>
				<tr><td>Comments:</td><td><asp:textbox id = "txtComments" runat = "server" width= "600" />
				<tr><td>Active: </td><td><asp:checkbox id = "chkActive" runat = "server" /></tr>
				</table>
				
				<asp:button id="butInsertUpdate" runat="server" text="Save" />
				<asp:button id="butCancel" runat="server" text="Cancel" CausesValidation="False" />
				</p>
			</asp:panel>
			</p>
		</ContentTemplate>
		<Triggers>
			<asp:AsyncPostbackTrigger ControlID="butShowInsert" EventName="Click"/>
			<asp:AsyncPostbackTrigger ControlID="butInsertUpdate" EventName="Click"/>
			<asp:AsyncPostbackTrigger ControlID="butCancel" EventName="Click"/>
		</Triggers>
	</asp:updatepanel>
			<p>
				<table width = 100%>
					<tr>
						<td align="left"><asp:dropdownlist id="ddlView" runat="server" AutoPostBack="True"/></td>
						<td align="left"><asp:radiobutton id = "rdActive" text="Active Course Types" textalign="Right" runat="server" GroupName="View" checked="True" AutoPostback="True"/></td>
						<td align="left"><asp:radiobutton id = "rdInAct" text="Inactive Course Types" textalign="Right" runat="server" GroupName="View" AutoPostback="True"/></td>
					</tr>
				</table>
			</p>
	<asp:UpdatePanel id="UpPan2" runat="Server" UpdateMode="Conditional" ChildrenAsTriggers = "False">
		<ContentTemplate>
			<asp:datagrid id="TrainingCourses" runat="server" 
				backcolor = "black" 
				gridlines="vertical" 
				cssclass="Grid"
				autogeneratecolumns = "false" 
				Width="85%"
				AutoPostBack = "True">
  				<headerstyle CssClass="GridHeader"/>
  				<columns>
  					<asp:boundcolumn DataField="TrainingCourseID" Visible="False" />
  					<asp:boundcolumn DataField="CourseTypeID" Visible="False" />
  					<asp:boundcolumn datafield="CourseDuration" visible="False" />
  					<asp:boundcolumn DataField="CourseTitle" headertext="Course Title">
  						<itemstyle cssclass="GridColumns"/>  						
  					</asp:boundcolumn>
					<asp:templatecolumn headertext="Course Duration">
  						<itemstyle cssclass="GridColumns" />
  						<itemtemplate>
  							<asp:label id="labCourseDuration" runat="server">
  							<%# ParseDuration(DataBinder.Eval(Container.DataItem, "CourseDuration")) %>
  							</asp:label>
  						</itemtemplate>  						
  					</asp:templatecolumn>
  					<asp:boundcolumn datafield="CatID" visible ="False" >
  						<itemstyle cssclass="GridColumns"/>
  					</asp:boundcolumn>
  					<asp:boundcolumn datafield="Category" HeaderText="Category">
  						<itemstyle cssclass="GridColumns"/>
  					</asp:boundcolumn>
  					<asp:boundcolumn datafield="Description" headertext="Course Type">
  						<itemstyle cssclass="GridColumns"/>
  					</asp:boundcolumn>  
					<asp:boundcolumn datafield="Comments" headertext="Comments">
						<itemstyle cssclass="GridColumns"/>
					</asp:boundcolumn>					
					<asp:boundcolumn datafield="Active" headertext="Active">
						<itemstyle cssclass="GridColumns"/>
					</asp:boundcolumn>						
					<asp:TemplateColumn HeaderText="">
       					<ItemTemplate>
         					<asp:linkbutton runat="server" id="lnbEdit" CommandName="Edit">Edit</asp:linkbutton>
       					</ItemTemplate>
       					<Itemstyle HorizontalAlign="Center" cssClass = "GridColumns"/>
    				</asp:TemplateColumn>			
					<asp:TemplateColumn HeaderText="">
       					<ItemTemplate>
         					<asp:linkbutton runat="server" id="lnbDel" CommandName="Delete">Delete</asp:linkbutton>
       					</ItemTemplate>
       					<Itemstyle HorizontalAlign="Center" cssClass = "GridColumns"/>
    				</asp:TemplateColumn>		    				
				</columns>
			</asp:datagrid>
		</ContentTemplate>

	</asp:updatepanel>
			
			
</asp:Content>

