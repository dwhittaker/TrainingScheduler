<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.Attendance"
	ValidateRequest    = "false"
	EnableSessionState = "True"
	maintainScrollPositionOnPostBack = "True"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
	<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
	</asp:Content>
	<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
			<table width = 100%>
				<tr>
					<td>
						<table style="border:2px blue solid" width = 100% cellpadding = 10>
							<tr>
								<td><center><h1>COURSE SCHEDULE CALENDAR FOR ATTENDANCE</h1><asp:HiddenField id="DoNotScroll" runat="Server"/></center></td>
							</tr>
							<tr>
								<td><center><h2>Please Click on a class name to open it</h2></center></td>
							</tr>
						</table>
						<table width = 100%>
							<tr>
								<td colspan = 3 align="center"><h2>Calendar View Options</h2></td>
							</tr>
							<tr>
								<td align="right"><asp:radiobutton id = "rdAllClass" text="All Classes" textalign="Right" runat="server" GroupName="View"/></td>
								<td align="center"><asp:radiobutton id = "rdRollUp" text="Roll Up Classes By Module" textalign="Right" runat="server" GroupName="View" checked="True"/></td>
								<td align="left"><asp:button id="btnChangeView" text="Apply View Change" runat="Server" onClick="ChangeView"/></td>
							</tr>
						</table>
						<table style="border:2px blue solid" width = 100% cellpadding = 10>
							<tr style="border:2px blue solid">
								<td width = 37%> </td>
								<td width = 25%>
									<table width = 100%>
										<tr>
											<td align= "right"><asp:button text="&#60;" id="btnBack" OnClick="btnBack_Click" runat="server"/></td>
											<td><center><b>
											<asp:dropdownlist id="ddlMonth" runat="server" />
											<asp:dropdownlist id="ddlYear" runat="server" />
											<asp:button id="btnGoToDate" runat="server" text="Go to date" />
											</b></center></td>
											<td align = "left"><asp:button text="&#62;" id="btnForward" OnClick="btnForward_Click" runat="server"/></td>
										</tr>
									</table>
								</td>
								<td width = 37%> </td>
							</tr>
							<tr border = 1>
								<td colspan = 3 align = Center>
								<asp:datagrid id="dgTrainGrid" 
								autogeneratecolumns = "false" 
								backcolor = "black" 
								runat="server" 
								gridlines="vertical" 
								cssclass="Grid"
								onItemCommand="Course_Click"
								AllowSorting = "True" 
								OnSortCommand = "SortData"
								AutoPostBack = "True">
									<headerstyle CssClass="GridHeader"/>
									<columns>
										<asp:templatecolumn HeaderText="Reg" visible="False">
					  						<itemtemplate>
												<asp:label id="lblExpand" runat="server" Text="+" />
											</itemtemplate>
											<itemstyle cssclass="GridColumns" />
										</asp:templatecolumn>	
									    <asp:boundcolumn headertext="Course ID" datafield="CourseInstanceID" visible = "False" SortExpression = "CourseInstanceID">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>
										<asp:ButtonColumn HeaderText="Course Title" DataTextField="CourseTitle" SortExpression = "CourseTitle">
											<Itemstyle cssClass="GridFColumn"/>
         								</asp:ButtonColumn>
         								<asp:boundcolumn headertext ="Course T" datafield ="CourseTitle" visible = "False">
         									<Itemstyle cssClass="GridColumns"/>
         								</asp:boundcolumn>
         								<asp:boundcolumn headertext ="Description" datafield ="Description">
         									<Itemstyle cssClass="GridColumns"/>
         								</asp:boundcolumn>
										<asp:boundcolumn headertext="Course Date" datafield="CourseDate" SortExpression = "CourseDate">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>
										<asp:boundcolumn headertext="Start Time" datafield="CourseStartTime">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>										
										<asp:boundcolumn headertext="End Time" datafield="CourseEndTime">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>										
										<asp:boundcolumn headertext="Duration" datafield="CourseDuration">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>
										<asp:boundcolumn headertext="Size" datafield="CourseSize">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>										
										<asp:boundcolumn headertext="Location" datafield="CourseLocation">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>				
										<asp:boundcolumn headertext="Instructor" datafield="CourseInstructor">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>				
										<asp:boundcolumn headertext="Training Course ID" datafield="TrainingCourseID" visible = "False">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>				
										<asp:boundcolumn headertext="Recert Months" datafield="Months" visible = "False">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>						
										<asp:TemplateColumn HeaderText="" visible = "false">
       									<ItemTemplate>
         									<asp:linkbutton runat="server" id="lnbDelete" CommandName="Delete">Delete</asp:linkbutton>
       									</ItemTemplate>
       									<Itemstyle HorizontalAlign="Center" cssClass = "GridColumns"/>
    									</asp:TemplateColumn>	
										<asp:boundcolumn headertext="Module" datafield="Module" visible = "False">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>	
										<asp:boundcolumn headertext="View" datafield="VMode" visible = "False">
											<ItemStyle cssClass="GridColumns"/>
										</asp:boundcolumn>											
									</columns>
									</asp:datagrid>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
	</asp:content>