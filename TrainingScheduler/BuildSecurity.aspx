<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.BuildSecurity"
	ValidateRequest    = "false"
	EnableSessionState = "True"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
		<table>
			<tr><td><a href="SecurityGroups.aspx">Return to Security Group List</a></tr></td>
		</table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
		<table width = 100% border = 0>
			<tr border = 0>
				<td>
					<b>LDAP Group: </b><asp:dropdownlist id="ddlGroup" runat = "Server" width =380 Autopostback="True"/>
				</td>
			</tr>
			<tr>
				<td colspan = 3>
					<table border = 1 bordercolor = "Black" width =100% >
						<tr>
							<td align = "center" bgcolor="Blue" colspan = 4>
								<font size = medium color = White><b>Available Pages</b></font>
							</td>
						</tr>
						<tr>
							<td align="Right" width = 50%><asp:button id = "btnCheckAll" Text = "Check All" OnClick = "CheckAll" runat="server"/></td>
							<td align="Left"><asp:button id = "btnUpdate" Text = "Update Security" runat="server" OnClick = "UpdateSecurity"/></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td align= Center colspan = 4>
					<asp:datagrid id="dgPageGrid" 
							autogeneratecolumns = "False" 
							backcolor = "black" 
							runat="server" 
							gridlines="vertical"
							AutoPostBack = "False"
							onEditCommand = "BlockControls">
						<headerstyle CssClass="GridHeader"/>
						<columns>
							<asp:TemplateColumn HeaderText="View">
       							<ItemTemplate>
         							<asp:CheckBox ID="chkSelection" Runat="server" />
       							</ItemTemplate>
       							<Itemstyle HorizontalAlign="Center" cssClass = "GridFColumn"/>
    						</asp:TemplateColumn>		
							<asp:boundcolumn headertext="Page Access ID" datafield="PageAccessID" visible = "false">
								<Itemstyle cssClass="GridFColumn"/>
							</asp:boundcolumn>
							<asp:boundcolumn headertext="GroupID" datafield="GroupID" visible = "False">
								<Itemstyle cssClass="GridColumns"/>
							</asp:boundcolumn>
							<asp:boundcolumn headertext="PageID" datafield="PageID" visible = "False">
								<Itemstyle cssClass="GridColumns"/>
							</asp:boundcolumn>										
							<asp:boundcolumn headertext="Title" datafield="Title">
								<Itemstyle cssClass="GridColumns"/>
							</asp:boundcolumn>		
						    <asp:TemplateColumn HeaderText="">
       							<ItemTemplate>
         							<asp:linkbutton runat="server" id="lnbEdit" CommandName="Edit">Block Controls For this Page</asp:linkbutton>
       							</ItemTemplate>
       							<Itemstyle HorizontalAlign="Center" cssClass = "GridFColumn"/>
    						</asp:TemplateColumn>							
						</columns>
					</asp:datagrid>
				</td>
			</tr>
		</table>
</asp:Content>