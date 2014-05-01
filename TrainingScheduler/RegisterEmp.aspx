<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.RegisterEmp"
	ValidateRequest    = "false"
	EnableSessionState = "true"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
<%@ Register TagPrefix="CWC" TagName="ddlSearch" src="ddlSearch.ascx" %> 
	<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
		<table>
			<tr><td><a href="RegistryForm.aspx">Back to Course Information</a></tr></td>
		</table>
	</asp:Content>
	<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
		<table>
			<tr>
				<td bgcolor = "blue"><font color = "white">Employees Needing Recert:</font></td>
				<td bgcolor = "blue" style="border-left: solid 2px blue;"><font color = "white">Enroll Individual Employee:<font></td>
			</tr>
			<tr>
				<td style="vertical-align:top;">	
						<asp:datagrid id="dgRegGrid" 
								autogeneratecolumns = "False" 
								backcolor = "black" 
								runat="server" 
								gridlines="vertical"
								AutoPostBack="True"
								onItemCommand="Emp_Click">
						<headerstyle CssClass="GridHeader"/>
						<columns>
								<asp:boundcolumn headertext="Emp ID" datafield="Emp_ID" visible = "False">
									<Itemstyle cssClass="GridFColumn"/>
								</asp:boundcolumn>
								<asp:buttoncolumn headertext="Last Name" DataTextField="Last_Name" >
									<Itemstyle cssClass="GridFColumn"/>
								</asp:buttoncolumn>
								<asp:boundcolumn headertext="First Name" datafield="First_Name">
									<Itemstyle cssClass="GridColumns"/>
								</asp:boundcolumn>										
								<asp:boundcolumn headertext="Recert Date" datafield="RecertDate">
									<Itemstyle cssClass="GridColumns"/>
								</asp:boundcolumn>								
						</columns>
						</asp:datagrid>
					</td>
				<td style="border-Left: solid 2px blue; vertical-align:top;" width = 500><CWC:ddlSearch id="cwcEmp" runat="server" /><br><asp:RequiredFieldValidator id="RequiredFieldValidator1" validationgroup = "Add" ControlToValidate="cwcEmp" ErrorMessage = "Employee Required!" CSSClass = "Validator" runat="server"/><br><br>
				<center><asp:Button id="b1" Text="Enroll Employee" runat="server" validationgroup="Add" OnClick = "Emp_Click" /></center>
				</td>
			</tr>
			<tr></tr>
		</table>
	</asp:Content>