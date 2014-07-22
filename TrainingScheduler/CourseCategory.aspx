<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.CourseCategory"
	ValidateRequest    = "false"
	EnableSessionState = "true"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
	<table>			
		<tr><td><asp:hyperlink id="hlCtype" NavigateUrl="TrainingCourse.aspx" text="Back to Training Courses" runat="server"/><hr></tr></td>
	</table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
   	<script language="JavaScript"> 
   		function ConfirmDeletion() 
        {
        	return confirm('Are you sure you wish to delete this record?');
        }
   	</script>
   	<asp:hiddenfield id ="DoNotScroll" runat="Server"/>
   	<table>
   		<tr>
   			<td colspan = 2>
   			   <asp:UpdatePanel id = "UpPan3" runat = "Server" UpdateMode = "Conditional" ChildrenAsTriggers = "false">
   					<ContentTemplate>	
   						<asp:panel id = "PanAddEdit" runat="Server" visible = "False">
   							<table>
   								<tr>
   									<td>Category name</td>
   									<td><asp:textbox id="txtCatName" runat="Server"/><asp:RequiredFieldValidator id="RequiredFieldValidator1" validationgroup = "Add" ControlToValidate="txtCatName" Text="Category Name Required!" CSSClass = "Validator" runat="server"/>
   								</tr>
   								<tr>
   									<td>Allow just recert records to be scheduled:</td>
   									<td><asp:checkbox id="chkRecert" runat="Server"/>
   								</tr>
   								<tr>
   									<td>Append description to course name on reports:</td>
   									<td><asp:checkbox id="chkApD" runat="Server"/>
   								</tr>
   								<tr>
   									<td>Reporting Group:</td>
   									<td><asp:dropdownlist id="ddlReport" runat="Server"/><asp:RequiredFieldValidator id="RequiredFieldValidator2" validationgroup = "Add" ControlToValidate="ddlReport" InitialValue = "-1" Text="Report Group Required!" CSSClass = "Validator" runat="server"/>
   								</tr>
   								<tr>
   									<asp:hiddenfield id="hdfCatID" runat="Server"/>
   								</tr>
   								<tr>
   									<td>
   										<table>
   											<tr>
   												<td align = "center"><asp:button id="btnSaveEdit" Text = "Save Category" runat="server" causevalidation = "True" validationgroup = "Add"/></td>
   												<td align = "center"><asp:button id="btnCancel" Text = "Cancel" runat="server"/><td>
   											</tr>
   										</table>
   									</td>
   								</tr>
   							</table>
   						</asp:panel>
   					</ContentTemplate>
   				<Triggers>
					<asp:AsyncPostBackTrigger ControlID="btnCreate" EventName ="Click"/>
					<asp:AsyncPostBackTrigger ControlID="btnCancel" EventName ="Click"/>
					<asp:AsyncPostBackTrigger ControlID="btnSaveEdit" EventName ="Click"/>
				</Triggers>
   				</asp:UpdatePanel>
   			</td>
   		</tr>
   		<tr>
   			<td align = "center">
   				<asp:UpdatePanel id="UpPan5" runat="Server" UpdateMode = "Conditional" ChildrenAsTriggers = "false">
   					<ContentTemplate>
   					<asp:panel id="PanCreate" runat="Server">
   						<asp:button id="btnCreate" Text = "Create Category" runat="server"/>
   					</asp:panel>
   					</ContentTemplate>
   					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="btnCreate" EventName ="Click"/>
					</Triggers>
   				</asp:UpdatePanel>
   			</td>
   		</tr>
   		<tr>
   			<td colspan = 3>	
   			<asp:UpdatePanel id="UpPan4" runat="Server" UpdateMode = "Conditional" ChildrenAsTriggers = "False">
   				<ContentTemplate>
   					<asp:datagrid id="dgCatGrid" 
					autogeneratecolumns = "false" 
					backcolor = "black" 
					runat="server" 
					gridlines="vertical" 
					cssclass="Grid"
					onEditCommand = "EditCat"
					onDeleteCommand = "DeleteCat"
					AutoPostBack = "True">
						<headerstyle CssClass="GridHeader"/>
						<columns>
							<asp:boundcolumn headertext="CategoryID" datafield="CourseCatID" visible = "False">
								<Itemstyle cssClass="GridColumns"/>
							</asp:boundcolumn>
         					<asp:boundcolumn headertext ="Category" datafield ="Category">
         						<Itemstyle cssClass="GridFColumn"/>
         					</asp:boundcolumn>
         					<asp:boundcolumn headertext ="Recert Allowed" datafield ="Recert">
         						<Itemstyle cssClass="GridColumns"/>
         					</asp:boundcolumn>	
         					<asp:boundcolumn headertext ="Append Description" datafield ="AppendDesc">
         						<Itemstyle cssClass="GridColumns"/>
         					</asp:boundcolumn>	
         					<asp:boundcolumn headertext ="Report Group" datafield ="ReportGroup">
         						<Itemstyle cssClass="GridColumns"/>
         					</asp:boundcolumn>		         				
							<asp:TemplateColumn HeaderText="">
       							<ItemTemplate>
         							<asp:linkbutton runat="server" id="lnbEdit" CommandName="Edit">Edit</asp:linkbutton>
       							</ItemTemplate>
       							<Itemstyle HorizontalAlign="Center" cssClass = "GridColumns"/>
    						</asp:TemplateColumn>         				
							<asp:TemplateColumn HeaderText="">
       							<ItemTemplate>
         							<asp:linkbutton runat="server" id="lnbDelete" CommandName="Delete">Delete</asp:linkbutton>
       							</ItemTemplate>
       							<Itemstyle HorizontalAlign="Center" cssClass = "GridColumns"/>
    						</asp:TemplateColumn>												
						</columns>
					</asp:datagrid>
				</ConTentTemplate>
				<Triggers>
					<asp:AsyncPostBackTrigger ControlID="btnSaveEdit" EventName ="Click"/>
				</Triggers>
			</asp:UpdatePanel>
   			</td>
   		</tr>
   	</table>
</asp:Content>
