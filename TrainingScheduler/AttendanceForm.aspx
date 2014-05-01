<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.AttendanceForm"
	ValidateRequest    = "false"
	EnableSessionState = "true"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
	<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
		<table>
			<tr><td><a href="Attendance.aspx">Back to Course Calendar</a></tr></td>
		</table>
	</asp:Content>
	<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
			<script language="JavaScript"> 
    			function ViewSignIn(url)
    			{
    				var url1 = url;
    				popupWindow = window.open(url1,'myWindow','toolbar=1,scrollbars=1,location=1,statusbar=1,menubar=1,resizable=1,width=1000,height=1000');
    			}
   			</script>
			<table width = 100% border = 0>
				<tr border = 0>
					<td>
						<b>Course: </b><asp:label id="lblCourse" runat = "Server" width =380 cssClass="Values"> <%= strCTitle %> </asp:label>
					</td>
					<td>
						<b>Instructor: </b><asp:label id="lblInst" runat = "Server" width = 200 cssClass="Values"> <%= strInst %> </asp:label>
					</td>
					<td>
					</td>
				</tr>
				<tr>
					<td colspan = 2>
						<b>Description: </b><asp:label id ="lblDesc" runat = "Server" Width =580 cssClass="Values"> <%= strDesc %> </asp:label>
					</td>
					<td></td>
				</tr>
				<tr>
					<td>
						<b>Date/Time: </b><asp:label id="lblDTime" runat = "Server" width =380 cssClass="Values"><%= strDTime %></asp:label>
					</td>
					<td>
						<b>Building: </b><asp:label id="lblBuild" runat = "Server" width =200 cssCLass="Values"><%= strLoc %></asp:label>
					</td>
					<td>
					</td>
				</tr>
				<tr>
					<td colspan = 3><b>Sign In Sheet: </b><asp:FileUpload id = "fuSignUp" text="Choose File" runat = "server" width = 600/><asp:label id = "lblFile" runat="Server" cssCLass="Values" Visible = "False"/></td>
				</tr>
				<tr>
					<td colspan = 3><asp:Button id = "btnUpload" runat = "server" text="Upload Sign In Sheet"/><asp:Button id="btnView" runat="Server" text="View Sign In Sheet" visible = "false"/><asp:Button id = "btnDelete" runat = "server" text="Delete Sign in Sheet" visible = "false"/></td>
				</tr>
				<tr border = 0>
					<td align = Center colspan=3>
						<b>Class Size: </b><b><asp:label id="lblCSize" runat="server" cssClass="Values"><%= strSize %></asp:label></b>
					</td>
				</tr>
				<tr>
					<td colspan = 3>
						<table border = 1 bordercolor = "Black" width =100% >
							<tr>
								<td align = "center" bgcolor="Blue" colspan = 4>
									<font size = medium color = White><b>Employee Registration List</b></font>
								</td>
							</tr>
							<tr>
								<td align="Right" width = 33%><asp:button id = "btnCheckAll" Text = "Check All" OnClick = "CheckAll" runat="server"/></td>
								<td align="Center"><asp:button id = "btnAttend" Text = "Mark Attendance" runat="server" OnClick = "MarkAttendance"/></td>
								<td align="Center"><asp:button id = "btnNoShow" Text = "Mark as No Show" runat="server" Onclick = "MarkAttendance" /></td>
								<td align="Left"><asp:button id = "btnRemAttend" Text = "Un-Mark Attendance" runat="server" OnClick = "MarkAttendance"/></td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td align= Center colspan = 4>
						<asp:datagrid id="dgAttGrid" 
								autogeneratecolumns = "False" 
								backcolor = "black" 
								runat="server" 
								gridlines="vertical"
								AutoPostBack = "False">
						<headerstyle CssClass="GridHeader"/>
						<columns>
						     	<asp:TemplateColumn HeaderText="Select">
       								<ItemTemplate>
         								<asp:CheckBox ID="chkSelection" Runat="server" />
       								</ItemTemplate>
       								<Itemstyle HorizontalAlign="Center" cssClass = "GridFColumn"/>
    							</asp:TemplateColumn>
								<asp:boundcolumn headertext="Emp ID" datafield="Emp_ID" visible = "False">
									<Itemstyle cssClass="GridFColumn"/>
								</asp:boundcolumn>
								<asp:boundcolumn headertext="Last Name" datafield="Last_Name">
									<Itemstyle cssClass="GridColumns"/>
								</asp:boundcolumn>
								<asp:boundcolumn headertext="First Name" datafield="First_Name">
									<Itemstyle cssClass="GridColumns"/>
								</asp:boundcolumn>										
								<asp:boundcolumn headertext="Dept Name" datafield="Dept_Name">
									<Itemstyle cssClass="GridColumns"/>
								</asp:boundcolumn>										
								<asp:boundcolumn headertext="Job Title" datafield="Job_Title">
									<Itemstyle cssClass="GridColumns"/>
								</asp:boundcolumn>																		
								<asp:boundcolumn headertext="Date Enrolled" datafield="EnrollDate">
									<Itemstyle cssClass="GridColumns"/>
								</asp:boundcolumn>		
								<asp:boundcolumn headertext="Date Completed" datafield="CompletionDate">
									<Itemstyle cssClass="GridColumns"/>
								</asp:boundcolumn>				
								<asp:boundcolumn headertext="Status" datafield="Status">
									<Itemstyle cssClass="GridColumns"/>
								</asp:boundcolumn>													
							</columns>
						</asp:datagrid>
					</td>
				</tr>
			</table>
	</asp:Content>
