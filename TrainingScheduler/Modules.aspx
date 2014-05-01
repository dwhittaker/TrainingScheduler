<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.Modules"
	ValidateRequest    = "false"
	EnableSessionState = "True"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
		<table>
			<tr><td><a href="Config.aspx">Return to Course Calendar</a></tr></td>
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
				<b>Module Name: </b><asp:textbox id="txtModule" runat = "Server" width =380/><asp:RequiredFieldValidator id="RequiredFieldValidator2" validationgroup = "Add" ControlToValidate="txtModule" Text="Module Name Required!" CSSClass = "Validator" runat="server"/>
			</td>
			<td>
			</td>
			<td>
			</td>
		</tr>
		<tr border = 0>
			<td>
				<b>Active: </b><asp:checkbox id ="chkModActive" runat = "Server" width = 380 checked="true" enabled = "false"/> 
			</td>
			<td>
			</td>
			<td>
			</td>
		</tr>
		<tr>
			<td align= "Center">
				<asp:button id = "btnAdd" Text = "Add Module" OnClick = "AddEdit_Module"  causvalidation = "True" validationgroup = "Add" runat="server"/>
				<asp:button id = "btnClear" Text = "Clear Form" OnClick = "ClearForm" runat="server"/>
			</td>
			<td align= "Left" colspan = 2>
				
			</td>
		</tr>
		<tr>
			<td colspan = 3>
				<table border = 1 bordercolor = "Black" width =100% >
					<tr>
						<td align = "center" bgcolor="Blue">
							<font size = medium color = White><b>Modules</b></font>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td align= Center colspan = 3>
				<asp:datagrid id="dgModGrid" 
					autogeneratecolumns = "False" 
					backcolor = "black" 
					runat="server" 
					gridlines="vertical"
					onDeleteCommand="Remove_Module"
					onEditCommand="Edit_Module"
					onItemCommand="Build_Module"
					AutoPostBack = "False">
					<headerstyle CssClass="GridHeader"/>
					<columns>
						<asp:boundcolumn headertext="Module ID" datafield="ModuleID" visible = "False">
							<Itemstyle cssClass="GridFColumn"/>
						</asp:boundcolumn>
						<asp:boundcolumn headertext="Module N" datafield="ModuleName" visible = "False">
							<Itemstyle cssClass="GridFColumn"/>
						</asp:boundcolumn>
						<asp:buttoncolumn headertext = "Module Name" datatextfield="ModuleName">
							<Itemstyle cssClass="GridFColumn"/>
						</asp:buttoncolumn>
         				<asp:boundcolumn headertext="Active" datafield="Active">
         					<itemstyle cssClass="GridColumns"/>
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
