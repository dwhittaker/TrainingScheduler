<%@ Control
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.ddlSearch"
%>
<script Language="Javascript">
var ddl;
var txt;
var arrText;
var arrValue;
var Timer;
var tset;
function UpdateTimer<%= txtSearch.ClientId %>(value){
	if(tset == 'Yes'){
		clearTimeout(Timer);
		tset = 'No';
	}
	Timer = setTimeout(function(){UpdateSearch<%= txtSearch.ClientId %>(value);},500);
	tset = 'Yes';
}
function UpdateSearch<%= txtSearch.ClientId %>(value){
	ddl = document.getElementById('<%= ddlList.ClientID %>');
	ddl.options.length = 0;
	for(var i = 0;i < arrText.length;i++){
		if(arrText[i].toLowerCase().indexOf(value.toLowerCase()) != -1 || arrText[i] == 'None selected'){
			AddDLItem<%= txtSearch.ClientId %>(arrText[i],arrValue[i]);
		}
	}
	ddl.selectedIndex = 0;
}
function GetItems(){		
		ddl = document.getElementById('<%= ddlList.ClientID %>');
		arrText = new Array();
		arrValue = new Array();
		for(var i = 0; i < ddl.options.length;i++){
			arrText[i] = ddl.options[i].text;
			arrValue[i] = ddl.options[i].value;
		}
}
function AddDLItem<%= txtSearch.ClientId %>(text, value){
	var itm = document.createElement("option");
	itm.text = text;
	itm.value = value;
	ddl.options.add(itm);
}
window.onload = GetItems;
</script>


<asp:UpdatePanel id="UpPan2" runat="Server" UpdateMode = "Conditional" rendermode="Inline">
<contenttemplate>
&nbsp;<asp:DropDownList id="ddlList" runat="Server" Autopostback="True" style="position:absolute"/><asp:TextBox id="txtSearch" runat="Server" style="position:absolute"/>
</contenttemplate>
<triggers>
	<asp:PostBackTrigger ControlID="ddlList"/>
	<asp:AsyncPostBackTrigger ControlID="txtSearch" EventName="TextChanged"/>
</triggers>
</asp:updatepanel>
