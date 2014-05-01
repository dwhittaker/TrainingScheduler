<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.SignIn"
	ValidateRequest    = "false"
	EnableSessionState = "True"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
<%@ Register TagPrefix="uc" TagName="Sig" 
    Src="ESig.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
	<table>
		<tr><td><a href="RegistryForm.aspx">Back to Course Information</a></tr></td>
	</table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">		
			<h1><%= Session("EmpName") %></h1>
			<uc:Sig id="ctSig" runat="Server" visible="false"/>
			<center><asp:label id="lblSSN" runat="Server" text="Enter Social Security Number:"/><asp:textbox id="txtSSN" runat="Server" textmode="password"/>	<asp:button id="btnLogin" runat="Server" onClick="VerSSN" text="Login"/>
</asp:Content>
