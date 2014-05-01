<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.BuildModule"
	ValidateRequest    = "false"
	EnableSessionState = "True"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
<%@ Register TagPrefix="CWC" TagName="ddlSearch" src="ddlSearch.ascx" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
		<table>
			<tr><td><a href="Modules.aspx">Return to Module List</a></tr></td>
		</table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
	<script language="JavaScript"> 
      	function ConfirmDeletion() 
       	{
        	return confirm('Are you sure you wish to delete this record?');
        }
   	</script>
	<table border = 0>
		<tr border = 0>
			<td>
				<b>Module Name: <asp:label id="lblModule" runat = "Server" width =380 cssClass="Values"/></b>
			</td>
			<td>
			</td>
			<td>
			</td>
		</tr>
		<tr border = 0>
			<td>
				<b>Class:</b><CWC:ddlSearch id="cwcClass" runat="server" CauseValidation="False"/><asp:RequiredFieldValidator id="RequiredFieldValidator2" validationgroup = "Add" ControlToValidate="cwcClass" Text="Class Required!"  CSSClass = "Validator" runat="server"/>
			</td>
			<td>
			
			</td>
			<td>
			</td>
		</tr>
		<tr border = 0>
			<td>
				<b>Start Date: </b><asp:textbox id ="txtSdate" runat = "Server" width = 180/><asp:RequiredFieldValidator id="RequiredFieldValidator1" validationgroup = "Add" ControlToValidate="txtSDate" Text="Date Required!" CSSClass = "Validator" runat="server"/><asp:RegularExpressionValidator runat=server ControlToValidate="txtSDate" ErrorMessage="Date Invalid" ValidationExpression="^((((0[13578])|([13578])|(1[02]))[\/](([1-9])|([0-2][0-9])|(3[01])))|(((0[469])|([469])|(11))[\/](([1-9])|([0-2][0-9])|(30)))|((2|02)[\/](([1-9])|([0-2][0-9]))))[\/]\d{4}$|^\d{4}$" CSSClass = "Validator"/> 
			</td>
			<td>
			</td>
			<td>
				<b>End Date: </b><asp:textbox id ="txtEdate" runat = "Server" width =180/><asp:RegularExpressionValidator runat=server ControlToValidate="txtEDate" ErrorMessage="Date Invalid" ValidationExpression="^((((0[13578])|([13578])|(1[02]))[\/](([1-9])|([0-2][0-9])|(3[01])))|(((0[469])|([469])|(11))[\/](([1-9])|([0-2][0-9])|(30)))|((2|02)[\/](([1-9])|([0-2][0-9]))))[\/]\d{4}$|^\d{4}$" CSSClass = "Validator"/>  
			</td>
		</tr>
		<tr>
			<td align= "Center">
				<asp:button id = "btnAdd" Text = "Add Class" OnClick = "AddEdit_Class" causevalidation = "true" validationgroup = "Add" runat="server"/>
				
			</td>
			<td align = "left"><asp:button id = "btnClear" Text = "Clear Form" OnClick = "ClearForm" runat="server"/></td>
			<td align= "Right">
			</td>
		</tr>
		<tr>
			<td colspan = 3>
				<table border = 1 bordercolor = "Black" width =100% >
					<tr>
						<td align = "center" bgcolor="Blue">
							<font size = medium color = White><b>Classes</b></font>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td align= Center colspan = 3>
				<asp:datagrid id="dgModClassGrid" 
					autogeneratecolumns = "False" 
					backcolor = "black" 
					runat="server" 
					gridlines="vertical"
					onDeleteCommand="Remove_Class"
					onEditCommand="Edit_Class"
					AutoPostBack = "False">
					<headerstyle CssClass="GridHeader"/>
					<columns>
						<asp:boundcolumn headertext="Module ID" datafield="ModuleID" visible = "False">
							<Itemstyle cssClass="GridFColumn"/>
						</asp:boundcolumn>
						<asp:boundcolumn headertext="CourseTitle" datafield="CourseTitle" visible = "True">
							<Itemstyle cssClass="GridFColumn"/>
						</asp:boundcolumn>
						<asp:boundcolumn headertext="Start Date" datafield="StartDate" >
							<Itemstyle cssClass="GridColumns"/>
						</asp:boundcolumn>
						<asp:boundcolumn headertext="End Date" datafield="EndDate" >
							<Itemstyle cssClass="GridColumns"/>
						</asp:boundcolumn>
						<asp:boundcolumn headertext="ModuleClassID" datafield="ModuleClassID" visible = "False">
							<Itemstyle cssClass="GridColumns"/>
						</asp:boundcolumn>
						<asp:boundcolumn headertext="TrainingCourseID" datafield="TrainingCourseID" visible = "False">
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
			</td>
		</tr>
	</table>
</asp:Content>
