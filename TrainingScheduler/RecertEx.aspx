<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.RecertEx"
	ValidateRequest    = "false"
	EnableSessionState = "true"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
<%@ Register TagPrefix="CWC" TagName="ddlSearch" src="ddlSearch.ascx" %> 
	<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
		<table>
			<tr><td><a href="TrainingRecords.aspx">Back to Employee Training Records</a></tr></td>
		</table>
	</asp:Content>
	<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
		<script language="JavaScript"> 
       		function ConfirmDeletion() 
           	{
             	return confirm('Are you sure you wish to delete this record?');
           	}
   		</script>
		<table width = 100% border = 0>
				<tr border = 0>
					<td>
						<b>Employee:</b><CWC:ddlSearch id="cwcEmp" runat="server" causevalidation="false"/>
					</td>
					<td>
						<asp:RequiredFieldValidator id="RequiredFieldValidator1" validationgroup = "Add" ControlToValidate="cwcEmp" Text="Employee Required!" CSSClass = "Validator" runat="server"/>
						<asp:hiddenfield id="lblExID" runat = "server"/>
					</td>
					<td>
					</td>
				</tr>
				<tr>
					<td colspan = 2>
						<b>Reason: </b><asp:textbox id ="txtRea" runat = "Server" Width =580 />
					</td>
					<td></td>
				</tr>
				<tr border = 0>
					<td>
						<b>Start Date: </b><asp:textbox id="txtSDate" runat = "Server" width =380 causevalidation="false"/><asp:RequiredFieldValidator id="RequiredFieldValidator2" validationgroup = "Add" ControlToValidate="txtSDate"  Text="Start Date Required!" CSSClass = "Validator" runat="server"/><asp:RegularExpressionValidator runat=server ControlToValidate="txtSDate" ErrorMessage="Date Invalid" ValidationExpression="^((((0[13578])|([13578])|(1[02]))[\/](([1-9])|([0-2][0-9])|(3[01])))|(((0[469])|([469])|(11))[\/](([1-9])|([0-2][0-9])|(30)))|((2|02)[\/](([1-9])|([0-2][0-9]))))[\/]\d{4}$|^\d{4}$" CSSClass = "Validator"/>
					</td>
					<td>
						<b>End Date: </b><asp:textbox id="txtEDate" runat = "Server" causevalidation="false" width = 200/><asp:RequiredFieldValidator id="RequiredFieldValidator3" validationgroup = "Add" ControlToValidate="txtEDate"  Text="Start Date Required!" CSSClass = "Validator" runat="server"/></asp:textbox><asp:RegularExpressionValidator runat=server ControlToValidate="txtEDate" ErrorMessage="Date Invalid" ValidationExpression="^((((0[13578])|([13578])|(1[02]))[\/](([1-9])|([0-2][0-9])|(3[01])))|(((0[469])|([469])|(11))[\/](([1-9])|([0-2][0-9])|(30)))|((2|02)[\/](([1-9])|([0-2][0-9]))))[\/]\d{4}$|^\d{4}$" CSSClass = "Validator"/>
					</td>
					<td>
					</td>
				</tr>
				<tr>
					<td colspan = 3>
						<table border = 1 bordercolor = "Black" width =100% >
							<tr>
								<td align = "center" bgcolor="Blue" colspan = 3>
									<font size = medium color = White><b>Exceptions</b></font>
								</td>
							</tr>
							<tr>
								<td align="Right" width = "33%"><asp:button id = "btnAddNew" Text = "Add New Exception" validationgroup = "Add" OnClick = "AddEx" runat="server"/></td>
								<td align="center" width = "33%"><asp:button id = "btnFilter" Text = "Apply Filter" runat="server" OnClick = "FilterEx"/></td>
								<td align="left" width = "33%"><asp:button id = "btnClear" Text = "Clear Form" runat="server" OnClick = "ClearFrm" causesvalidation = "False" /></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td align= Center colspan = 3>
						<asp:datagrid id="dgExGrid" 
								autogeneratecolumns = "False" 
								backcolor = "black" 
								runat="server" 
								gridlines="vertical"
								onDeleteCommand="Remove_Ex"
								onItemCommand="Update_Ex"
								AutoPostBack = "False">
						<headerstyle CssClass="GridHeader"/>
						<columns>
								<asp:boundcolumn headertext="Exception ID" datafield="ExceptionID" visible = "False">
									<Itemstyle cssClass="GridFColumn"/>
								</asp:boundcolumn>
								<asp:boundcolumn headertext="Emp ID" datafield="Emp_ID" visible = "false">
									<itemstyle cssClass="GridColumns"/>
								</asp:boundcolumn>
								<asp:boundcolumn headertext="Last Name" datafield="Last_Name" visible = "false">
									<Itemstyle cssClass="GridFColumn"/>
								</asp:boundcolumn>
								<asp:buttoncolumn headertext="Last Name" datatextfield = "Last_Name">
									<itemstyle cssClass="GridFColumn"/>
								</asp:buttoncolumn>
								<asp:boundcolumn headertext="First Name" datafield="First_Name">
									<Itemstyle cssClass="GridColumns"/>
								</asp:boundcolumn>																
								<asp:boundcolumn headertext="Start Date" datafield="StartDate">
									<Itemstyle cssClass="GridColumns"/>
								</asp:boundcolumn>																		
								<asp:boundcolumn headertext="End Date" datafield="EndDate">
									<Itemstyle cssClass="GridColumns"/>
								</asp:boundcolumn>		
								<asp:boundcolumn headertext="Reason" datafield="Reason">
									<Itemstyle cssClass="GridColumns"/>
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