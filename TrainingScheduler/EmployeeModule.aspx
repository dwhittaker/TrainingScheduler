<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.EmployeeModule"
	ValidateRequest    = "false"
	EnableSessionState = "True"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
<%@ Register TagPrefix="CWC" TagName="ddlSearch" src="ddlSearch.ascx" %> 
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
						<b>Employee:</b><CWC:ddlSearch id="cwcEmp" runat="server" Autopostback="True" />
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr border = 0>
					<td>
						<b>Module: </b><asp:dropdownlist id ="ddlMod" runat = "Server" width = 380 enabled = "false" causevalidation = "false"/> 
						<asp:RequiredFieldValidator id="RequiredFieldValidator1" validationgroup = "Add" ControlToValidate="ddlMod" 
						Text="Module Name Required!" InitialValue = "-1" CSSClass = "Validator" runat="server"/>
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr border = 0>
					<td>
						<b>Module Date: </b><asp:textbox id ="txtDate" runat = "Server" enabled = "false"/> 
						<asp:RegularExpressionValidator ID="CreateRecertDateValidator" runat="server" ValidationGroup="Createg"
					    ErrorMessage="Invalid date." ControlToValidate="txtDate" CssClass="Validator"
					    ValidationExpression="(^\d{1,2}\/\d{1,2}\/\d{4}$)|(^[\s]*$)" 
					    Display="Dynamic">
						</asp:RegularExpressionValidator>
						<asp:RequiredFieldValidator id="RequiredFieldValidator2" validationgroup = "Add" ControlToValidate="txtDate" 
						Text="Date Required!" CSSClass = "Validator" runat="server"/>
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr border = 0>
					<td>
						<b>Register Employee For Classes: </b><asp:checkbox id ="chkRegister" runat = "Server" checked="true" enabled = "false"/> 
					</asp:RegularExpressionValidator>
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr>
					<td colspan = 3 align= "Left">
						<asp:button id = "btnAdd" Text = "Add Module" OnClick = "Add_Mod" runat="server" causevalidation = "true" validationgroup="Add"/>
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
						<asp:datagrid id="dgEmpModGrid" 
								autogeneratecolumns = "False" 
								backcolor = "black" 
								runat="server" 
								gridlines="vertical"
								onDeleteCommand="Remove_Mod"
								AutoPostBack = "False">
						<headerstyle CssClass="GridHeader"/>
						<columns>
								<asp:boundcolumn headertext="EmpModID" datafield="EmpModID" visible = "False">
									<Itemstyle cssClass="GridFColumn"/>
								</asp:boundcolumn>
								<asp:boundColumn HeaderText="ModuleID" datafield="ModuleID" visible = "False">
									<Itemstyle cssClass="GridColumnS"/>
         						</asp:boundColumn>
         						<asp:boundcolumn headertext="Module" datafield="ModuleName">
         							<itemstyle cssClass="GridColumns"/>
         						</asp:boundcolumn>
         						<asp:boundcolumn headertext="Date Assigned" datafield="DateAssigned">
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