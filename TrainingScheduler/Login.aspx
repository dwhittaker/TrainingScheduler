<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.Login"
	ValidateRequest    = "false"
	EnableSessionState = "true"
%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<link href="TrainingScheduler.css" type="text/css" rel="stylesheet" />
		<table width = 100% style = "border-collapse: collapse">
			<tr>
				<td rowspan = 2 width="170" height="155" style="border-bottom: 5px solid blue;"><img src="Images\SPIN-Logo-transparent-bgd.gif" width="170" height="165"></td>
				<td align = "Center"><font size = 6 color=blue><b>SPIN Training Scheduler</b></font></td>
			</tr>
			<tr>
				<td class="bottom">
				</td>
			</tr>
		</table>
	</head>
	<body>
		<form id="Form_Login" method="post" runat="server">

			<table>

				<tr>
					<td colspan="2">						
						<p><asp:Label runat="server" id="lblMsg">Please login below.</asp:Label></p>
						<p>
						Enter your windows username and password:</p>
					</td>
				</tr>

				<tr>
					<td>
						Username:
					</td>
					<td>
						<asp:TextBox runat="server" id="txtUsername" />						
					</td>
				</tr>
				<tr>
					<td>
						Password:
					</td>
					<td>
						<asp:TextBox runat="server" id="txtPassword" textmode="password" />						
					</td>
				</tr>

				<tr>
					<td colspan="2" align="center">
						<asp:Button runat="server" id="btnLogin" text="Login" />
					</td>
				</tr>

			</table>

		</form>
	</body>
</html>
