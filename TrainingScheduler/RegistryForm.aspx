<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.RegistryForm"
	ValidateRequest    = "False"
	EnableSessionState = "true"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
	<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
		<table>
			<tr><td><a href="Default.aspx">Back to Course Calendar<hr></a></tr></td>
			<tr><td><asp:hyperlink id="hlRegEmp" NavigateUrl="RegisterEmp.aspx" text="Enroll Employee" runat="server"/><hr></tr></td>
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
					<td style="vertical-align: top;">
						<b>Course: </b><asp:label id="lblCourse" runat = "Server" width =380 cssClass="Values" style="vertical-align: top;"> <%= strCTitle %> </asp:label>
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
				<tr border = 0>
					<td align = Center colspan=3>
						<b>Class Size: </b><b><asp:label id="lblCSize" runat="server" cssClass="Values"><%= strSize %></asp:label></b>
					</td>
				</tr>
				<tr>
					<td colspan = 3>
						<table border = 1 bordercolor = "Black" width =100% >
							<tr>
								<td align = "center" bgcolor="Blue">
									<font size = medium color = White><b>Employee Registration List</b></font>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td align= Center colspan = 3>
						<asp:datagrid id="dgEmpGrid" 
								autogeneratecolumns = "False" 
								backcolor = "black" 
								runat="server" 
								gridlines="vertical"
								onItemCommand="Emp_Click"
								onDeleteCommand="Remove_Emp"
								AutoPostBack = "False">
						<headerstyle CssClass="GridHeader"/>
						<columns>
								<asp:boundcolumn headertext="Emp ID" datafield="Emp_ID" visible = "False">
									<Itemstyle cssClass="GridFColumn"/>
								</asp:boundcolumn>
								<asp:boundcolumn headertext="Last Name" datafield="Last_Name" visible="False">
									<Itemstyle cssClass="GridFColumn"/>
								</asp:boundcolumn>
								<asp:ButtonColumn headertext="Last Name" DataTextField="Last_Name" visible="True">
									<Itemstyle cssClass="GridFColumn"/>
								</asp:ButtonColumn>
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
								<asp:TemplateColumn headertext = "Signature" visible = "true">
                            		<ItemTemplate>
                                		<asp:Image ID="imgEsig" 
                                    		ImageUrl='<%# DataBinder.Eval(Container.DataItem, "Spath")%>' 
                                    		runat="server" height = "50" width = "200" />
                            		</ItemTemplate>
                            		<Itemstyle cssClass="GridColumns"/>
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

