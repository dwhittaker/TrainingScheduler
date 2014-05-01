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
			
			<h1>Training Courses</h1>							
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
				<tr><td>Comments:</td><td><asp:textbox id = "txtComments" runat = "server" width= "600" />
				<tr><td>Active: </td><td><asp:checkbox id = "chkActive" runat = "server" /></tr>
				</table>
				
				<asp:button id="butInsertUpdate" runat="server" text="Save" />
				<asp:button id="butCancel" runat="server" text="Cancel" CausesValidation="False" />
				</p>
			</asp:panel>
			</p>
		<asp:datagrid id="TrainingCourses" runat="server" 
				backcolor = "black" 
				gridlines="vertical" 
				cssclass="Grid"
				autogeneratecolumns = "false" 
				Width="85%">
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
  					<asp:boundcolumn datafield="Description" headertext="Course Type">
  						<itemstyle cssclass="GridColumns"/>
  					</asp:boundcolumn>  
					<asp:boundcolumn datafield="Comments" headertext="Comments">
						<itemstyle cssclass="GridColumns"/>
					</asp:boundcolumn>					
					<asp:boundcolumn datafield="Active" headertext="Active">
						<itemstyle cssclass="GridColumns"/>
					</asp:boundcolumn>						
					<asp:EditCommandColumn HeaderText="" EditText="Edit" UpdateText="Save" CancelText="Cancel">
						<itemstyle cssclass="GridColumns" />
					</asp:EditCommandColumn>		
				</columns>
			</asp:datagrid>
			
</asp:Content>

