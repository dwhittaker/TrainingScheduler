<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.TrainingRecords"
	ValidateRequest    = "false"
	EnableSessionState = "true"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
<%@ Register TagPrefix="CWC" TagName="ddlSearch" src="ddlSearch.ascx" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
		<table>
			<tr><td><asp:hyperlink id="hlAtt" NavigateUrl="Attendance.aspx" text="Course Attendance" runat="server"/><hr></tr></td>
			<tr><td><asp:hyperlink id="hlRecert" NavigateUrl="RecertEx.aspx" text="Exception" runat="server"/><hr></tr></td>
		</table>
</asp:Content>
	<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
			<script language="JavaScript"> 
       			function ConfirmDeletion() 
           		{
             		return confirm('Are you sure you wish to delete this record?');
           		}
   			</script>
			<h1>Employee Training Records</h1>
			
				<h2>Select an employee to view training records.</h2>
				<p>
				<CWC:ddlSearch id="cwcEmp" runat="server" /><br><br></p>
				<asp:UpdatePanel id="UpPan1" runat="Server" UpdateMode = "Conditional">
					<ContentTemplate>
						<asp:panel id="panCreateButton" runat="server" visible="false"><asp:button id="butShowCreateRecord" text="Create" runat="server" /></asp:panel>
					</ContentTemplate>
				</asp:UpdatePanel>
				<asp:UpdatePanel id="UpPan2" runat="Server" UpdateMode = "Conditional">
					<ContentTemplate>
						<asp:panel id="panRecordEdit" runat="server" visible="false" >
							<h2>Edit Course Details</h2>
							<asp:textbox id="EmpRegistrationID" runat="server" visible="False" />
							<asp:label id="CourseTitleLabel" width="150" runat="server">Course title:</asp:label>
							<asp:label id="CourseTitle" style="font-weight:bold;" runat="server" />
					 		<br />
							<asp:label id="DescriptionLabel" width="150" runat="server">Description:</asp:label>
							<asp:label id="labDescription" runat="server"></asp:label>
					 		<br />
							<asp:label id="DurationLabel" runat="server" width="150">Duration:</asp:label>
							<asp:label id="labDuration" runat="server"></asp:label>
					 		<br />
							<asp:label id="LocationLabel" width="150" runat="server">Location:</asp:label>
							<asp:label id="labLocation" runat="server"></asp:label>
					 		<br />
							<asp:label id="InstructorLabel" width="150" runat="server">Instructor:</asp:label>
							<asp:label id="labInstructor" runat="server"></asp:label>
					 		<br />
							<asp:label id="CompletedDateLabel" width="150" runat="server">Completed date:</asp:label>
							<asp:textbox id="EditCompletedDate" runat="server" ValidationGroup="Editg" />
							<asp:RegularExpressionValidator ID="EditCompletedDateValidator" runat="server" ValidationGroup="Editg"
					    		ErrorMessage="Invalid date."    ControlToValidate="EditCompletedDate" CssClass="Validator"
					    		ValidationExpression="(^\d{1,2}\/\d{1,2}\/\d{4}$)|(^[\s]*$)" 
					    		Display="Dynamic">
							</asp:RegularExpressionValidator><br />
							<asp:label id="RecertDateLabel" runat="server" width="150">Recert date:</asp:label>
							<asp:textbox id="EditRecertDate" runat="server" ValidationGroup="Editg" />
							<asp:RegularExpressionValidator ID="EditRecertDateValidator" runat="server" ValidationGroup="Editg"
					    		ErrorMessage="Invalid date." ControlToValidate="EditRecertDate" CssClass="Validator"
					    		ValidationExpression="(^\d{1,2}\/\d{1,2}\/\d{4}$)|(^[\s]*$)" 
					    		Display="Dynamic">
							</asp:RegularExpressionValidator>
					 		<br />
							<asp:label id="CommentsLabel" runat="server" width="150">Comments</asp:label>
							<asp:textbox id="EditComments" runat="server" /> (optional)
					 		<br />
							<asp:button id="butSaveRecord" runat="server" text="Save" ValidationGroup="Editg" />
							<asp:button id="butCancelEdit" runat="server" text="Cancel" />
							<asp:label id="lblErr1" runat="Server" CssClass="Validator" visible="false"/>
						</asp:panel>			
					</ContentTemplate>
				</asp:UpdatePanel>
				<asp:UpdatePanel id="UpPan3" runat="Server" UpdateMode="Conditional" ChildrenAsTriggers = "True">
					<ContentTemplate>
						<asp:panel id="panRecordCreate" runat="server" visible="false" >
							<h2>Create Training Record</h2>
							<asp:label id="CourseTitleLabel2" width="150" runat="server">Course title:</asp:label>
							<asp:dropdownlist id="ddlTrainingCourse" runat="server" autopostback="True" ValidationGroup="Createg" />
							<asp:CompareValidator id="TrainingCourseValidator" runat="server"  ValidationGroup="Createg" ControlToValidate="ddlTrainingCourse" ErrorMessage="Select a Training Course."  CssClass="Validator" Operator="NotEqual" ValueToCompare="-1"></asp:CompareValidator>
					 		<br />
							<asp:label id="DescriptionLabel2" width="150" runat="server">Description:</asp:label>
							<asp:textbox id="Description" runat="server"  ValidationGroup="Createg" />
							<asp:customvalidator id="DescVal" runat="server"  ValidationGroup="Createg" controltovalidate="Description" CssClass="Validator" ErrorMessage="A description is required for Inservice classes." validateemptytext="True"></asp:customvalidator>
					 		<br />
							<asp:label id="DurationLabel2" runat="server" width="150">Duration:</asp:label>
							<asp:textbox id="Duration" ValidationGroup="Createg" runat="server" />
							<asp:RegularExpressionValidator ID="DurValidator" runat="server" ValidationGroup="Createg"
					    		ErrorMessage="Enter a number like 1.5" ControlToValidate="Duration" CssClass="Validator"
					    		ValidationExpression="^\d*\.?\d*$" 
					    		Display="Dynamic"></asp:regularexpressionvalidator>
							<asp:requiredfieldvalidator ID="RfDurValidator" runat="server" ValidationGroup="Createg"
					    		ErrorMessage="Please enter the duration"    ControlToValidate="Duration" CssClass="Validator"></asp:requiredfieldvalidator>
							<br />
							<asp:label id="LocationLabel2" width="150" runat="server">Location:</asp:label>
							<asp:dropdownlist id="ddlLocation" runat="server" />					
							<br />
							<asp:label id="InstructorLabel2" width="150" runat="server">Instructor:</asp:label>
							<asp:dropdownlist id="ddlInstructor" runat="server" />
							<br />
							<asp:label id="CompletedDateLabel2" width="150" runat="server">Completed date:</asp:label>
							<asp:textbox id="CreateCompletedDate" runat="server" ValidationGroup="Createg" />
							<asp:RegularExpressionValidator ID="CreateCompletedDateValidator" runat="server" ValidationGroup="Createg"
					    		ErrorMessage="Invalid date."    ControlToValidate="CreateCompletedDate" CssClass="Validator"
					    		ValidationExpression="(^\d{1,2}\/\d{1,2}\/\d{4}$)|(^[\s]*$)" 
					    		Display="Dynamic">
							</asp:RegularExpressionValidator><br />
							<asp:label id="RecertDateLabel2" runat="server" width="150">Recert date:</asp:label>
							<asp:textbox id="CreateRecertDate" runat="server" ValidationGroup="Createg" />
							<asp:RegularExpressionValidator ID="CreateRecertDateValidator" runat="server" ValidationGroup="Createg"
					    		ErrorMessage="Invalid date." ControlToValidate="CreateRecertDate" CssClass="Validator"
					    		ValidationExpression="(^\d{1,2}\/\d{1,2}\/\d{4}$)|(^[\s]*$)" 
					    		Display="Dynamic">
							</asp:RegularExpressionValidator>
					 		<br />
							<asp:label id="CommentsLabel2" runat="server" width="150">Comments</asp:label>
							<asp:textbox id="CreateComments" runat="server" /> (optional)
					 		<br />
							<asp:button id="butCreateRecord"  ValidationGroup="Createg" runat="server" text="Create Record" />
							<asp:button id="butCancelCreate" runat="server" text="Cancel" />
							<asp:label id="lblErr2" runat="Server" CssClass="Validator" visible="false"/>
						</asp:panel>			
					</ContentTemplate>
				</asp:UpdatePanel>
			<h2>Training records:</h2>
			<p>
			<asp:UpdatePanel id="UpPan4" runat="Server" UpdateMode="Conditional">
			<ContentTemplate>
			<asp:datagrid id="EmployeeTrainingRecords" runat="server" 
				backcolor = "black" 
				gridlines="vertical" 
				cssclass="Grid"
				autogeneratecolumns = "false" 
				Width="85%"
				AllowSorting="true"
				>
  				<headerstyle CssClass="GridHeader"/>
  				<columns>
  					<asp:templatecolumn HeaderText="">
  						<itemtemplate>
							<asp:linkbutton runat="server" id="lnbEdit" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EmpRegistrationID") %>' OnCommand="RowCommandHandler" >Edit</asp:linkbutton><br />
							<asp:linkbutton runat="server" id="lnbDelete" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "EmpRegistrationID") %>' OnCommand="RowCommandHandler">Delete</asp:linkbutton>				
						</itemtemplate>
						<itemstyle cssclass="GridColumns" />
					</asp:templatecolumn>					
					<asp:boundcolumn headertext="Completed" datafield="completiondate" DataFormatString="{0: MM/dd/yyyy}" sortexpression="completiondate">
						<Itemstyle cssClass="GridColumns"/>
					</asp:boundcolumn>
					<asp:boundcolumn headertext="Course<br />date" datafield="coursedate" DataFormatString="{0: MM/dd/yyyy}" sortexpression="coursedate">
						<Itemstyle cssClass="GridColumns"/>
					</asp:boundcolumn>
					<asp:boundcolumn headertext="Recert" datafield="recertdate" DataFormatString="{0: MM/dd/yyyy}" sortexpression="recertdate">
						<Itemstyle cssClass="GridColumns"/>
					</asp:boundcolumn>
					<asp:boundcolumn headertext="Title/Description" datafield="coursetitledesc" sortexpression="coursetitledesc">
						<Itemstyle cssClass="GridColumns"/>
					</asp:boundcolumn>
					<asp:boundcolumn headertext="Status" datafield="description">
						<Itemstyle cssClass="GridColumns"/>
					</asp:boundcolumn>
					<asp:boundcolumn headertext="Comments" datafield="comments">
						<Itemstyle cssClass="GridColumns"/>
					</asp:boundcolumn>
					<asp:templatecolumn headertext="Dur.">
						<itemtemplate>
						<%# ParseDuration(DataBinder.Eval(Container.DataItem, "Duration")) %>
						</itemtemplate>
						<Itemstyle cssClass="GridColumns"/>
					</asp:templatecolumn>					
					<asp:boundcolumn headertext="Location" datafield="locationname" sortexpression="locationname">
						<Itemstyle cssClass="GridColumns"/>
					</asp:boundcolumn>
					<asp:boundcolumn headertext="Instructor" datafield="insname" sortexpression="insname">
						<Itemstyle cssClass="GridColumns"/>
					</asp:boundcolumn>
				</columns>
			</asp:datagrid>
			</ContentTemplate>
			<Triggers>
				<asp:AsyncPostBackTrigger ControlID="butCreateRecord" EventName ="Click"/>
				<asp:AsyncPostBackTrigger ControlID="butSaveRecord" EventName ="Click"/>
				<asp:AsyncPostBackTrigger ControlID="cwcEmp" EventName ="IndChange"/>
			</Triggers>
			</asp:UpdatePanel>
			<!--<asp:boundcolumn headertext="Location" datafield="locationname">
						<Itemstyle cssClass="GridColumns"/>
					</asp:boundcolumn>
					<asp:boundcolumn headertext="Instructor" datafield="insname">
						<Itemstyle cssClass="GridColumns"/>
					</asp:boundcolumn>-->
			<!--<asp:ButtonColumn Text="Edit" HeaderText="" CommandName="Edit" >
						<itemstyle cssclass="GridColumns" />
					</asp:buttoncolumn>
					<asp:ButtonColumn Text="Delete" CommandName="Delete" >		
						<Itemstyle cssClass="GridColumns"/>
					</asp:buttoncolumn>		-->
			</p>
</asp:Content>

