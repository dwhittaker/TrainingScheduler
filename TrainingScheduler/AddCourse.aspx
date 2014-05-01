<%@ Page
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.AddCourse"
	ValidateRequest    = "false"
	EnableSessionState = "True"
	MasterPageFile="~/SiteMaster.master"
%>
<%@ MasterType VirtualPath="~/SiteMaster.master" %>
<%@ Register TagPrefix="CWC" TagName="ddlSearch" src="ddlSearch.ascx" %> 
	<asp:Content ID="Content1" ContentPlaceHolderID="Links" Runat="Server">
		<table>
			<tr><td><a href="Config.aspx">Back to Course Calendar</a></tr></td>
		</table>
	</asp:Content>
	<asp:Content ID="Content2" ContentPlaceHolderID="Main" Runat="Server">
		<script type="text/javascript">

 			function UpdateDuration(d) 
			{
 				var d1 = new Date( '1/1/1970 ' + document.getElementById("Main_txtSTime").value);
				var dur = parseFloat(d.value);
				var min = 60 * dur;
				var d2 = new Date( d1.getTime() + (min * 60 * 1000 ) );
				var time;
				
				
				time = formatAMPM(d2);				
							
 				document.getElementById('Main_txtETime').value = time;
				
			}
			//function ClassChange(d)
			//{
			//	var intStart;
			//	var intStop;
			//	var intLen;
				
			//	document.getElementById('Main_txtETime').value = "";
				
			//	if (d.options[d.selectedIndex].value != '-1')
			//	{
			//		intStart = d.options[d.selectedIndex].text.lastIndexOf('(');
			//		intStop = d.options[d.selectedIndex].text.lastIndexOf(')');
					//intLen = intStop - intStart;
					//document.getElementById("Main_ddlClass").setAttribute("Duration", d.options[d.selectedIndex].text.substring(intStart +1, intStop));
					//document.getElementById("Main_cwcClass_ddlList").setAttribute("Duration", d.options[d.selectedIndex].text.substring(intStart +1,intStop));
					//document.getElementById("Main_lblDuration").value = document.getElementById("Main_cwcClass_ddlList").getAttribute("Duration");
			//	}
				
			//}
			function formatAMPM(date) 
			{
  				var hours = date.getHours();
  				var minutes = date.getMinutes();
  				var ampm = hours >= 12 ? 'PM' : 'AM';
  				var hours = hours % 12;
  				hours = hours ? hours : 12; // the hour '0' should be '12'
  				minutes = minutes < 10 ? '0'+minutes : minutes;
  				strTime = hours + ':' + minutes + ' ' + ampm;
  				return strTime;
			}
		</script>
		<asp:hiddenfield id="lblDuration" runat="server"/>
			<table width = 100% height=50% border = 0>
				<tr border = 0>
					<td>
						<b>Class:</b><CWC:ddlSearch id="cwcClass" runat="server" Autopostback="True"/><asp:RequiredFieldValidator id="RequiredFieldValidator1" validationgroup = "Add" ControlToValidate="cwcClass$txtSearch" Text="Class Required!" CSSClass = "Validator" runat="server"/>
					</td>
					<td>
						<b>Course Date: </b><asp:textbox id="txtCDate" runat = "Server" width =200 causevalidation = "False"></asp:textbox><asp:RequiredFieldValidator id="RequiredFieldValidator2" validationgroup = "Add" ControlToValidate="txtCDate" Text="Date Required!" CSSClass = "Validator" runat="server"/><asp:RegularExpressionValidator runat=server ControlToValidate="txtCDate" ErrorMessage="Date Invalid" ValidationExpression="^((((0[13578])|([13578])|(1[02]))[\/](([1-9])|([0-2][0-9])|(3[01])))|(((0[469])|([469])|(11))[\/](([1-9])|([0-2][0-9])|(30)))|((2|02)[\/](([1-9])|([0-2][0-9]))))[\/]\d{4}$|^\d{4}$" CSSClass = "Validator"/>
					</td>
					<td>
					</td>
				</tr>
				<tr>
					<td colspan = 2>
						<b>Description: </b><asp:textbox id = "txtDesc" runat="server" width =580/>
					</td>
					<td></td>
				</tr>
				<tr border = 0>
					<td>
						<b>Start Time: </b><asp:textbox id="txtSTime" runat = "Server" width =380 onblur = "UpdateDuration(this.form.Main_lblDuration);" causevalidation = "False"/><asp:RequiredFieldValidator id="RequiredFieldValidator3" ControlToValidate="txtSTime"  validationgroup = "Add" Text="Start Time Required!" CSSClass = "Validator" runat="server"/><asp:RegularExpressionValidator runat=server ControlToValidate="txtSTime" ErrorMessage="Time Invalid" ValidationExpression="^ *(1[0-2]|[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$" CSSClass = "Validator"/>
					</td>
					<td>
						<b>End Time: </b><asp:textbox id="txtETime" runat = "Server" width = 200 causevalidation = "False"/><asp:RequiredFieldValidator id="RequiredFieldValidator4" ControlToValidate="txtETime"  validationgroup = "Add" Text="End Time Required!" CSSClass = "Validator" runat="server"/><asp:RegularExpressionValidator runat=server ControlToValidate="txtETime" ErrorMessage="Time Invalid" ValidationExpression="^ *(1[0-2]|[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$" CSSClass = "Validator"/>
					</td>
					<td>
					</td>
				</tr>
				<tr border = 0>
					<td>
						<b>Course Location: </b><asp:dropdownlist id="ddlLoc" runat = "Server" width =380/>
					</td>
					<td>
						<b>Course Instructor: </b><asp:dropdownlist id="ddlIns" runat = "Server" width = 200/>
					</td>
					<td>
					</td>
				</tr>
				<tr>
					<td colspan = 3>
						<table>
							<tr>
								<td align="Right" width = "33%"><asp:button id = "btnAddNew" Text = "Add New Class" OnClick = "AddClass" runat="server" causvalidation = "True" validationgroup = "Add"/></td>
								<td align="center" width = "33%"></td>
								<td align="left" width = "33%"><asp:button id = "btnClear" Text = "Clear Form" runat="server" OnClick = "ClearFrm" causesvalidation = "False" /></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
	</asp:Content>
