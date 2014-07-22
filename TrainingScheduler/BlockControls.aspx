<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.BlockControls"
	ValidateRequest    = "false"
	EnableSessionState = "True"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
	<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
		<table>
			<tr><td><a href="Default.aspx">Back to Course Calendar</a></tr></td>
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
						<b>LDAP Group: </b><asp:label id="lblGroup" runat = "Server"/>
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr border = 0>
					<td>
						<b>Page Name: </b><asp:label id="lblPage" runat = "Server"/>
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr border = 0>
					<td>
						<b>Control ID: </b><asp:textbox id ="txtControl" runat = "Server"/> 
						<asp:RequiredFieldValidator id="RequiredFieldValidator2" validationgroup = "Add" ControlToValidate="txtControl" 
						Text="Control Name Required" CSSClass = "Validator" runat="server"/>
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr>
					<td colspan = 3 align= "Left">
						<asp:button id = "btnAdd" Text = "Block Control" OnClick = "AddCtrl" runat="server" causevalidation = "true" validationgroup="Add"/>
					</td>
				</tr>
				<tr>
					<td colspan = 3>
						<table border = 1 bordercolor = "Black" width =100% >
							<tr>
								<td align = "center" bgcolor="Blue">
									<font size = medium color = White><b>Blocked Controls</b></font>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td align= Center colspan = 3>
						<asp:datagrid id="dgBlkCtrlGrid" 
								autogeneratecolumns = "False" 
								backcolor = "black" 
								runat="server" 
								gridlines="vertical"
								onDeleteCommand="Remove_Ctrl"
								AutoPostBack = "False">
						<headerstyle CssClass="GridHeader"/>
						<columns>
								<asp:boundcolumn headertext="ControlID" datafield="ControlID" visible = "False">
									<Itemstyle cssClass="GridFColumn"/>
								</asp:boundcolumn>
								<asp:boundColumn HeaderText="GroupID" datafield="GroupID" visible = "False">
									<Itemstyle cssClass="GridColumnS"/>
         						</asp:boundColumn>
         						<asp:boundcolumn headertext="PageID" datafield="PageID" visible = "False">
         							<itemstyle cssClass="GridColumns"/>
         						</asp:boundcolumn>
         						<asp:boundcolumn headertext="Control Name" datafield="Control">
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
