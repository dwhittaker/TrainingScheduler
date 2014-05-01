<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.ImportList"
	ValidateRequest    = "false"
	EnableSessionState = "true"
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
						<b>Import Name: </b><asp:textbox id="txtImport" runat = "Server" width =380 Autopostback="True"/>
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr border = 0>
					<td>
						<b>ImportType: </b><asp:dropdownlist id ="ddlImpType" runat = "Server" width = 380 enabled = "True" autopostback = "true"/> 
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr border = 0>
					<td>
						<b>Import Action: </b><asp:dropdownlist id ="ddlImpAction" runat = "Server" width = 380 enabled = "True"/> 
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr border = 0>
					<td>
						<b>Active: </b><asp:checkbox id ="chkImpActive" runat = "Server" width = 380 checked="true" enabled = "false"/> 
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr>
					<td colspan = 3 align= "Left">
						<asp:button id = "btnAdd" Text = "Add Import" OnClick = "AddEdit_Import" runat="server"/>
					</td>
					<td colspan = 3 align= "Right">
						<asp:button id = "btnClear" Text = "Clear Form" OnClick = "ClearForm" runat="server"/>
					</td>
				</tr>
				<tr>
					<td colspan = 3>
						<table border = 1 bordercolor = "Black" width =100% >
							<tr>
								<td align = "center" bgcolor="Blue">
									<font size = medium color = White><b>Imports</b></font>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td align= Center colspan = 3>
						<asp:datagrid id="dgImpGrid" 
								autogeneratecolumns = "False" 
								backcolor = "black" 
								runat="server" 
								gridlines="vertical"
								onDeleteCommand="Remove_Import"
								onEditCommand="Edit_Import"
								onItemCommand="Build_Import"
								AutoPostBack = "False">
						<headerstyle CssClass="GridHeader"/>
						<columns>
								<asp:boundcolumn headertext="Import ID" datafield="ImportID" visible = "False">
									<Itemstyle cssClass="GridFColumn"/>
								</asp:boundcolumn>
								<asp:boundcolumn headertext="Import N" datafield="ImportName" visible = "False">
									<Itemstyle cssClass="GridFColumn"/>
								</asp:boundcolumn>
								<asp:buttoncolumn HeaderText="Import Name" datatextfield="ImportName">
									<Itemstyle cssClass="GridFColumn"/>
         						</asp:buttonColumn>
         						<asp:boundcolumn headertext="Import Type ID" datafield="ImportTypeID" visible = "False">
									<itemstyle cssClass="GridColumns"/>
								</asp:boundcolumn>
								<asp:boundcolumn headertext="Import Type" datafield="ImportType">
									<itemstyle cssClass="GridColumns"/>
								</asp:boundcolumn>
         						<asp:boundcolumn HeaderText="Import Action" datafield = "ImportAction">
         							<Itemstyle cssClass="GridColumns" />
         						</asp:boundcolumn>
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