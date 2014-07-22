<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.ManageSig"
	ValidateRequest    = "false"
	EnableSessionState = "True"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
<%@ Register TagPrefix="CWC" TagName="ddlSearch" src="ddlSearch.ascx" %> 
<%@ Register TagPrefix="uc" TagName="Sig" 
    Src="ESig.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
	<table>
		<tr><td><a href="Admin.aspx">Back to Admin Screen</a></tr></td>
	</table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">		
			Employee: <CWC:ddlSearch id="cwcEmp" runat="Server" autopostback="false"/><br><br>
			<asp:button id="btnGet" runat="Server" text = "Get Signature"/>
	<asp:placeholder id="Place" runat="server" />
</asp:Content>