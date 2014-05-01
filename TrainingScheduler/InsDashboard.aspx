<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.InsDashboard"
	ValidateRequest    = "false"
	EnableSessionState = "True"
	maintainScrollPositionOnPostBack = "False"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
	<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
	</asp:Content>
	<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
			<script language="JavaScript"> 
       			function ConfirmDeletion() 
           		{
             		return confirm('Are you sure you wish to delete this record?');
           		}
   			</script>
			<table width = 100%>
				<tr>
					<td>
						<table style="border:2px blue solid" width = 100% cellpadding = 10>
							<tr>
								<td><center><h1>Actions for Instructor</h1></center></td>
							</tr>
						</table>
						<asp:panel id = "AddPanel" runat = "server" Visible = "True">
							<h2>Add Action</h2>
							<br>
							<b>Action:</b><asp:dropdownlist id="ddlAction" runat="Server" />
							<br>
							<b>Action Description:</b><asp:textbox id = "ADesc" runat ="server" width = 300/>
							<br>
							<b>Department:</b> <asp:dropdownlist id="ddlDept" runat = "server" />
							<br>
							<b>Action Date:</b> <asp:textbox id ="ADate" runat="server" enabled="True"/><asp:RegularExpressionValidator runat=server ControlToValidate="ADate" ErrorMessage="Date Invalid" 
							ValidationExpression="^((((0[13578])|([13578])|(1[02]))[\/](([1-9])|([0-2][0-9])|(3[01])))|(((0[469])|([469])|(11))[\/](([1-9])|([0-2][0-9])|(30)))|((2|02)[\/](([1-9])|([0-2][0-9]))))[\/]\d{4}$|^\d{4}$" 
							CSSClass = "Validator"/>
							<br>
							<b>Start Time:</b> <asp:textbox id = "STime" runat = "server"/>
							<asp:RegularExpressionValidator ID="StartTimeValidator" runat="server"
					    	ErrorMessage="Invalid Time."    ControlToValidate="STime" 
					    	ValidationExpression="^ *(1[0-2]|[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$" 
					    	Display="Dynamic"/>
							<b>End Time:</b> <asp:textbox id = "ETime" runat = "server"/>
							<asp:RegularExpressionValidator ID="EndTimeValidator" runat="server"
					    	ErrorMessage="Invalid Time."    ControlToValidate="STime" 
					    	ValidationExpression="^ *(1[0-2]|[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$" 
					    	Display="Dynamic"/>
					    	<br>
					    	<asp:button id = "btnSaveUpdate" Text = "Save" runat="server" />
					    	<asp:button id = "btnClear" text = "Clear" runat="server" OnClick = "ClearForm"/>
						</asp:panel>
						<asp:panel id = "panByInstructor" runat = "server" Visible = "True">
							<h2>View Actions by Instructor</h2>
							<p><b>Instructor:</b> <asp:dropdownlist id="ddlInstructor" runat="server" AutoPostBack="True"/><br />
						</asp:panel>
						<table style="border:2px blue solid" width = 100% cellpadding = 10>
							<tr style="border:2px blue solid">
								<td width = 37%> </td>
								<td width = 25%>
									<table width = 100%>
										<tr>
											<td align= "right"><asp:button text="&#60;" id="btnBack" OnClick="btnBack_Click" runat="server" autopostback="true"/></td>
											<td><center><b>
											<asp:dropdownlist id="ddlMonth" runat="server" />
											<asp:dropdownlist id="ddlYear" runat="server" />
											<asp:button id="btnGoToDate" runat="server" text="Go to date" />
											</b></center></td>
											<td align = "left"><asp:button text="&#62;" id="btnForward" OnClick="btnForward_Click" runat="server" enabled="False"/></td>
										</tr>
									</table>
								</td>
								<td width = 37%> </td>
							</tr>
							<tr border = 1>
								<td colspan = 3 align = Center>
								<asp:datagrid id="dgActionGrid" 
								autogeneratecolumns = "false" 
								backcolor = "black" 
								runat="server" 
								gridlines="vertical" 
								cssclass="Grid"
								onItemCommand="Action_Click"
								onDeleteCommand="Remove_Action"
								AllowSorting = "True" 
								OnSortCommand = "SortData"
								AutoPostBack = "True">
									<headerstyle CssClass="GridHeader"/>
									<columns>	
									    <asp:boundcolumn headertext="Ins Action ID" datafield="InsActionID" visible = "False" SortExpression = "InsActionID">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>
										<asp:boundColumn HeaderText="Ins ID" DataField="InstructorID" visible = "False">
											<Itemstyle cssClass="GridColumns"/>
         								</asp:boundColumn>
										<asp:boundColumn HeaderText="Ins Name" DataField="InstructorName" visible = "True">
											<Itemstyle cssClass="GridColumns"/>
         								</asp:boundColumn>
         								<asp:boundColumn HeaderText="Action ID" DataField="ActionID" visible = "False">
											<Itemstyle cssClass="GridColumns"/>
         								</asp:boundColumn>
         								<asp:ButtonColumn HeaderText="Action Name" DataTextField="actionname" SortExpression = "ActionName">
											<Itemstyle cssClass="GridFColumn"/>
         								</asp:ButtonColumn>
         								<asp:boundcolumn HeaderText = "DeptID" DataField="DeptID" visible = "False">
         									<Itemstyle cssClass="GridColumn"/>
         								</asp:boundcolumn>
         								<asp:boundcolumn HeaderText = "For Dept" datafield = "DeptName" visible = "True">
         									<Itemstyle cssClass="GridColumns"/>
         								</asp:boundcolumn>
         								<asp:boundcolumn headertext ="Start Time" datafield ="ActionStartTime">
         									<Itemstyle cssClass="GridColumns"/>
         								</asp:boundcolumn>
										<asp:boundcolumn headertext="End Time" datafield="ActionEndTime">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>
										<asp:boundcolumn headertext="Comments" datafield="Comments">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>										
										<asp:boundcolumn headertext="Date" datafield="Date" SortExpression = "Date">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>										
										<asp:boundcolumn headertext="Action Month" datafield="ActionMonth" visible="False">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>
										<asp:boundcolumn headertext="Action Year" datafield="ActionYear" visible = "False">
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
					</td>
				</tr>
			</table>
	</asp:Content>

