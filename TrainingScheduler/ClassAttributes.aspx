<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.ClassAttributes"
	ValidateRequest    = "false"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
<%@ Register TagPrefix="CWC" TagName="ddlSearch" src="ddlSearch.ascx" %> 
	<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
		<table>
			<tr><td><a href="Config.aspx">Back to Course Calendar</a></tr></td>
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
						<b>Course Type:</b><CWC:ddlSearch id="cwcClass" runat="server" Autopostback="True"/>
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr border = 0>
					<td>
						<b>Prerequisite Training: </b><asp:dropdownlist id ="ddlAtt" runat = "Server" width = 380 enabled = "false"/> 
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr border = 0>
					<td>
						<b>Prerequisite Training as of: </b><asp:textbox id ="txtAttDate" runat = "Server" enabled = "false"/> 
						<asp:RegularExpressionValidator ID="CreateRecertDateValidator" runat="server" ValidationGroup="Createg"
					    ErrorMessage="Invalid date." ControlToValidate="txtAttDate" CssClass="Validator"
					    ValidationExpression="(^\d{1,2}\/\d{1,2}\/\d{4}$)|(^[\s]*$)" 
					    Display="Dynamic">
					</asp:RegularExpressionValidator>
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>
				<tr>
					<td colspan = 3 align= "Left">
						<asp:button id = "btnAdd" Text = "Add Attribute" OnClick = "Add_Att" runat="server"/>
					</td>
				</tr>
				<tr>
					<td colspan = 3>
						<table border = 1 bordercolor = "Black" width =100% >
							<tr>
								<td align = "center" bgcolor="Blue">
									<font size = medium color = White><b>Prerequisite Trainings</b></font>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td align= Center colspan = 3>
						<asp:datagrid id="dgAttGrid" 
								autogeneratecolumns = "False" 
								backcolor = "black" 
								runat="server" 
								gridlines="vertical"
								onDeleteCommand="Remove_Att"
								AutoPostBack = "False">
						<headerstyle CssClass="GridHeader"/>
						<columns>
								<asp:boundcolumn headertext="ClassAttributeID" datafield="ClassAttributeID" visible = "False">
									<Itemstyle cssClass="GridFColumn"/>
								</asp:boundcolumn>
								<asp:boundColumn HeaderText="Prerequisite Training" datafield="AttributeName">
									<Itemstyle cssClass="GridColumnS"/>
         						</asp:boundColumn>
         						<asp:boundcolumn headertext="Training as of:" datafield="AsOfDate">
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