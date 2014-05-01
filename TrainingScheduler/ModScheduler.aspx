<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.ModScheduler"
	ValidateRequest    = "false"
	EnableSessionState = "True"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
	<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
		<table>
			<tr><td><a href="Config.aspx">Back to Course Calendar</a></tr></td>
		</table>
	</asp:Content>
	<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
			<table border = 0>
				<tr border = 0>
					<td>
						<b>Module: </b><asp:dropdownlist id="ddlMod" runat = "Server" width =380/>
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr border = 0>
					<td>
						<b>Scheduled Date: </b><asp:textbox id ="txtDate" runat = "Server"/> 
						<asp:RegularExpressionValidator ID="CreateRecertDateValidator" runat="server" ValidationGroup="Createg"
					    ErrorMessage="Invalid date." ControlToValidate="txtDate" CssClass="Validator"
					    ValidationExpression="(^\d{1,2}\/\d{1,2}\/\d{4}$)|(^[\s]*$)" 
					    Display="Dynamic">
						</asp:RegularExpressionValidator>
						<asp:RequiredFieldValidator id="RequiredFieldValidator1" validationgroup = "Add" ControlToValidate="txtDate" 
						Text="Date Required!" CSSClass = "Validator" runat="server"/>
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr border = 0>
					<td>
						<b>Scheduled Time: </b><asp:textbox id ="txtTime" runat = "Server"/> 
						<asp:RegularExpressionValidator runat=server ControlToValidate="txtTime" ErrorMessage="Time Invalid" 
						ValidationExpression="^ *(1[0-2]|[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$" CSSClass = "Validator"/>
						<asp:RequiredFieldValidator id="RequiredFieldValidator2" validationgroup = "Add" ControlToValidate="txtTime" 
						Text="Date Required!" CSSClass = "Validator" runat="server"/>
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr>
					<td align= "Center">
						<asp:button id = "btnAdd" Text = "Schedule Module" OnClick = "Sched_Module"  causvalidation = "True" validationgroup = "Add" runat="server"/>
						<asp:button id = "btnClear" Text = "Clear Form" OnClick = "ClearForm" runat="server"/>
					</td>
					<td align= "Left" colspan = 2>
					</td>
				</tr>
			</table>
	</asp:Content>