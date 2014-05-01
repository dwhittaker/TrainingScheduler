<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.BuildImport"
	ValidateRequest    = "false"
	EnableSessionState = "true"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
	<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
		<table>
			<tr><td><a href="ImportList.aspx">Back to Imports</a></tr></td>
		</table>
	</asp:Content>
	<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
		<script language="JavaScript"> 
       		function ConfirmDeletion() 
           	{
             	return confirm('Are you sure you wish to delete this record?');
           	}
   		</script>
		<table border = 1>
			Please select sample file:<br><br>
			<asp:FileUpload id = "fuSampleFile" text="Choose File" runat = "server" width = 500/>
			<br>
			<asp:button id = "btnRead" text = "Read Sample File" runat = "server" />
			<br>
			<tr>
				<td bgcolor = "blue"><font color = "white">Import Data fields:</font></td>
				<td bgcolor = "blue" style="border-left: solid 2px blue;"><font color = "white">Import Criteria:<font></td>
			</tr>
			<tr>
				<td style="vertical-align:top;">
					<table>
						<tr>
							<td><asp:dropdownlist id="ddlTblFields" runat="server"/></td>
							<td width = 50 align="center">=</td>
							<td align="center"><asp:dropdownlist id="ddlExtFields" runat="server"/></td>
						</tr>
						<tr>
							<td></td>
							<td></td>
							<td align="center">Or</td>
						</tr>
						<tr>
							<td colspan = 3 align="center"><asp:textbox id="txtSpData" runat = "server"/></td>
							<td></td>
							<td></td>
						</tr>
						<tr>
							<td colspan = 3 align="center"><asp:button id="btnAddData" runat="server" text="Add Data" OnClick = "ReadCSVFile" autopostback="False"/></td>
							<td></td>
							<td></td>
						</tr>
						<tr>
							<td colspan= 3 align="center"><asp:checkbox id="chkDataVerify" runat="Server" Text="Use Data to verify if a record was previously updated"/></td>
							<td></td>
							<td></td>
						<tr>
						<tr>
							<td colspan = 3 align="center">
								<asp:datagrid id="dgDataDefGrid"								
									autogeneratecolumns = "False" 
									backcolor = "black" 
									runat="server" 
									gridlines="vertical"
									onDeleteCommand = "DeleteDataDef"
									AutoPostBack = "False">
									<headerstyle CssClass = "GridHeader"/>
									<columns>
										<asp:boundcolumn headertext="Import Data Field ID" datafield="ImpDataFID" visible = "False">
											<Itemstyle cssClass="GridFColumn"/>
										</asp:boundcolumn>
										<asp:boundcolumn headertext="Import Column" datafield="ImportColumn">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>
										<asp:boundcolumn headertext="Data" datafield="Data">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>
										<asp:boundcolumn headertext="ImportID" datafield="ImportID" visible="False">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>
										<asp:boundcolumn headertext="Error Verify" datafield="Verify">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>
						   		 		<asp:TemplateColumn HeaderText="">
       										<ItemTemplate>
         										<asp:linkbutton runat="server" id="lnbDelData" CommandName="Delete">Delete</asp:linkbutton>
       										</ItemTemplate>
       										<Itemstyle HorizontalAlign="Center" cssClass = "GridColumns"/>
    									</asp:TemplateColumn>	
									</columns>
								</asp:datagrid>
							</td>
							<td></td>
							<td></td>
						</tr>
					</table>
				</td>
				<td style="border-Left: solid 2px blue; vertical-align:top;">
					<table>
						<tr>
							<td align = "center">Enter table field critera will by applied to: <asp:dropdownlist id = "ddlTblFCriteria" runat = "server"/></td>
						</tr>
						<tr>
							<td align = "center">Criteria:<br><asp:textbox id = "txtCrit" runat = "server" width = 500 height = 50 textmode = "multiline"/>  <asp:listbox id = 'lstExtFields' runat = "server" SelectionMode = "Single"/><asp:button id = "btnCritFAdd" runat="server" text="Add Field" onclick = "AppendField"/></td>
						</tr>
						<tr>
							<td align = "center"><asp:button id ="btnCritAdd" runat = "Server" Text="Add Critera to Import" onclick="AddCrit"/></td>
						</tr>
						<tr>
							<td align = "center"><asp:checkbox id="chkCritVerify" runat="Server" Text="Check Criteria for possible failure after import"/></td>
						</tr>
						<tr>
							<td align="center">Assign Priority for Error Check: <asp:textbox id = "txtCritPri" runat = "Server"/></td>
						</tr>
						<tr>
							<td align = "center">
								<asp:datagrid id="dgDataCritGrid"								
									autogeneratecolumns = "False" 
									backcolor = "black" 
									runat="server" 
									gridlines="vertical"
									onDeleteCommand = "DeleteCritDef"
									AutoPostBack = "False">
									<headerstyle CssClass = "GridHeader"/>
									<columns>
										<asp:boundcolumn headertext="Import Critera Field ID" datafield="ImpCriID" visible = "False">
											<Itemstyle cssClass="GridFColumn"/>
										</asp:boundcolumn>
										<asp:boundcolumn headertext="Import Critera Column" datafield="ImportCriteriaField">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>
										<asp:boundcolumn headertext="Criteria" datafield="Criteria">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>
										<asp:boundcolumn headertext="Critera" datafield="ImportID" visible="False">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>
										<asp:boundcolumn headertext="Error Verify" datafield="Verify">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>
										<asp:boundcolumn headertext="Priority" datafield="Priority">
											<Itemstyle cssClass="GridColumns"/>
										</asp:boundcolumn>
						   				<asp:TemplateColumn HeaderText="">
       										<ItemTemplate>
         										<asp:linkbutton runat="server" id="lnbDelCri" CommandName="Delete">Delete</asp:linkbutton>
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
			<tr></tr>
		</table>
	</asp:Content>