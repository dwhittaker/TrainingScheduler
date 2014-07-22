<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.Admin"
	ValidateRequest    = "false"
	EnableSessionState = "True"
	maintainScrollPositionOnPostBack = "False"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
	<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
		<asp:Panel id ="ConfigLinks" runat = "Server" visible = "True">
			<table>
				<tr><td><asp:hyperlink id="hlCImp" NavigateUrl="ImportList.aspx" text="Create Import" runat="Server"/><hr></a></tr></td>
				<tr><td><asp:hyperlink id="hlSecG" NavigateUrl="SecurityGroups.aspx" text="Add LDAP Group" runat="Server"/><hr></a></tr></td>
				<tr><td><asp:hyperlink id="hlBSec" NavigateUrl="BuildSecurity.aspx" text="Page Security" runat="Server"/><hr></tr></td>
				<tr><td><asp:hyperlink id="hlImportFile" NavigateUrl="ImportFile.aspx" text="Import File" runat="server"/><hr></tr></td>
				<tr><td><asp:hyperlink id="hlMergeRec" NavigateUrl="MergeRecords.aspx" text="Merge Records" runat="server"/><hr></tr></td>
				<tr><td><asp:hyperlink id="hlManSig" NavigateUrl="ManageSig.aspx?skipatt=yes&candel=yes" text="Manage Signatures" runat="server"/><hr></tr></td>
			</table>
		</asp:Panel>
	</asp:Content>
	<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">

	</asp:content>
