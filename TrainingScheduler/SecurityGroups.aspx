<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.SecurityGroups"
	ValidateRequest    = "false"
	EnableSessionState = "True"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
	<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
		<table>
			<tr><td><a href="Admin.aspx">Back to Admin Screen</a></tr></td>
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
						<b>LDAP Group Name: </b><asp:textbox id ="txtGroup" runat = "Server" width = 380 causevalidation = "false"/> 
						<asp:RequiredFieldValidator id="RequiredFieldValidator1" validationgroup = "Add" ControlToValidate="txtGroup" 
						Text="LDAP Group Name Required!" CSSClass = "Validator" runat="server"/>
					</td>
					<td><asp:label id ="lblGid" runat="server" visible="False"/>
					</td>
					<td>
					</td>
				</tr>
				<tr border = 0>
					<td>
						<b>Work with Past Dates: </b><asp:checkbox id ="chkPast" runat = "Server"/> 
					</asp:RegularExpressionValidator>
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr border = 0>
					<td>
						<b>Access Other Instructor Dashboard data: </b><asp:checkbox id ="chkOI" runat = "Server"/> 
					</asp:RegularExpressionValidator>
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr>
					<td colspan = 3 align= "Left">
						<asp:button id = "btnAdd" Text = "Add Group" OnClick = "Add_Group" runat="server" causevalidation = "true" validationgroup="Add"/>
					</td>
				</tr>
				<tr>
					<td colspan = 3>
						<table border = 1 bordercolor = "Black" width =100% >
							<tr>
								<td align = "center" bgcolor="Blue">
									<font size = medium color = White><b>LDAP Groups</b></font>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td align= Center colspan = 3>
						<asp:datagrid id="dgGroupGrid" 
								autogeneratecolumns = "False" 
								backcolor = "black" 
								runat="server" 
								gridlines="vertical"
								onEditCommand="Edit_Group"
								onDeleteCommand="Remove_Group"
								AutoPostBack = "False">
						<headerstyle CssClass="GridHeader"/>
						<columns>
								<asp:boundcolumn headertext="Group ID" datafield="GroupID" visible = "False">
									<Itemstyle cssClass="GridFColumn"/>
								</asp:boundcolumn>
         						<asp:boundcolumn headertext="GName" datafield="LDAPGroupName" visible = "False">
         							<itemstyle cssClass="GridColumns"/>
         						</asp:boundcolumn>
         						<asp:buttoncolumn headertext="Group Name" datatextfield="LDAPGroupName" CommandName="Edit">
         							<itemstyle cssClass="GridFColumn"/>
         						</asp:buttoncolumn>
         						<asp:boundcolumn headertext="Work with Past Data" datafield="PastDates">
         							<itemstyle cssClass="GridColumns"/>
         						</asp:boundcolumn>
         						<asp:boundcolumn headertext="Other Instructor Dashboard" datafield="OtherIns">
         							<itemstyle cssClass="GridColumns"/>
         						</asp:boundcolumn>
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