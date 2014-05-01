<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.CourseInstructor"
	ValidateRequest    = "false"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
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
				<tr>
					<td colspan = 3 align= "Left">
						<asp:button id = "btnAdd" Text = "Add New Instructor" runat="server"/>
					</td>
				</tr>
				<tr border = 0>
					<td>
						<asp:panel id = "InsPanel" runat = "server" visible = "False">
							<asp:hiddenfield id="InsID" runat="Server"/>
							<b>First Name:</b><asp:textbox id = "Fname" runat="Server"/>
							<br>
							<b>Last Name:</b><asp:textbox id = "LName" runat ="Server"/>
							<br>
							<asp:button id="btnSaveUpdate" runat ="server" Text = "Save"/>
							<asp:button id="btnCancel" runat="Server" text = "Cancel"/>
						</asp:panel>
					</td>
					<td>
					</td>
					<td>
					</td>
				</tr>

				<tr>
					<td colspan = 3>
						<table border = 1 bordercolor = "Black" width =100% >
							<tr>
								<td align = "center" bgcolor="Blue">
									<font size = medium color = White><b>Class Attributes</b></font>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td align= Center colspan = 3>
						<asp:datagrid id="dgInsGrid" 
								autogeneratecolumns = "False" 
								backcolor = "black" 
								runat="server" 
								gridlines="vertical"
								onDeleteCommand="Remove_Ins"
								AutoPostBack = "False">
						<headerstyle CssClass="GridHeader"/>
						<columns>
								<asp:boundcolumn headertext="CourseInstructorID" datafield="CourseInstructorID" visible = "False">
									<Itemstyle cssClass="GridFColumn"/>
								</asp:boundcolumn>
								<asp:boundcolumn HeaderText="First Name" DataField="FirstName">
									<Itemstyle cssClass="GridColumnS"/>
         						</asp:boundcolumn>
         						<asp:boundcolumn HeaderText="Last Name" DataField="LastName">
									<Itemstyle cssClass="GridColumnS"/>
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