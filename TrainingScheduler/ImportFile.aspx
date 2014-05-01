<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.ImportFile"
	ValidateRequest    = "false"
	EnableSessionState = "true"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
	<table>
		<tr>
			<td><asp:FileUpload id = "fuFileUpload" text="Choose File" runat = "server" width = 600/></td>
		</tr>
		<tr>
			<td>Select Import Type: <asp:dropdownlist id = "ddlImpType" runat="server"/></td>
		</tr>
		<tr>
			<td><asp:button id = "btnUploadFile" runat="server" Text="Upload File" onclick="FileImport"/></td>
		</tr>
	</table>
</asp:Content>