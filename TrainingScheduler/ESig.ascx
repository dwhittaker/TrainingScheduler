<%@ Control
	Language           = "VB"
	AutoEventWireup    = "false"
	Inherits           = "TrainingScheduler.ESig"
%>
<script Language="Javascript">
var OX = 0;
var OY = 0;
var tempX = 0;
var tempY = 0;
var i = 0;
var newline = 0;
var ct = 0;
var n = 0;
var newobj = new Array();
var mtarray = new Array();
var ltarray = new Array();
var sigpath;
var ubrow;
var newp;
var BrowserDetect = {
	init: function () {
		this.browser = this.searchString(this.dataBrowser) || "An unknown browser";
		this.version = this.searchVersion(navigator.userAgent)
			|| this.searchVersion(navigator.appVersion)
			|| "an unknown version";
		this.OS = this.searchString(this.dataOS) || "an unknown OS";
	},
	searchString: function (data) {
		for (var i=0;i<data.length;i++)	{
			var dataString = data[i].string;
			var dataProp = data[i].prop;
			this.versionSearchString = data[i].versionSearch || data[i].identity;
			if (dataString) {
				if (dataString.indexOf(data[i].subString) != -1)
					return data[i].identity;
			}
			else if (dataProp)
				return data[i].identity;
		}
	},
	searchVersion: function (dataString) {
		var index = dataString.indexOf(this.versionSearchString);
		if (index == -1) return;
		return parseFloat(dataString.substring(index+this.versionSearchString.length+1));
	},
	dataBrowser: [
		{
			string: navigator.userAgent,
			subString: "Chrome",
			identity: "Chrome"
		},
		{ 	string: navigator.userAgent,
			subString: "OmniWeb",
			versionSearch: "OmniWeb/",
			identity: "OmniWeb"
		},
		{
			string: navigator.vendor,
			subString: "Apple",
			identity: "Safari",
			versionSearch: "Version"
		},
		{
			prop: window.opera,
			identity: "Opera",
			versionSearch: "Version"
		},
		{
			string: navigator.vendor,
			subString: "iCab",
			identity: "iCab"
		},
		{
			string: navigator.vendor,
			subString: "KDE",
			identity: "Konqueror"
		},
		{
			string: navigator.userAgent,
			subString: "Firefox",
			identity: "Firefox"
		},
		{
			string: navigator.vendor,
			subString: "Camino",
			identity: "Camino"
		},
		{		// for newer Netscapes (6+)
			string: navigator.userAgent,
			subString: "Netscape",
			identity: "Netscape"
		},
		{
			string: navigator.userAgent,
			subString: "MSIE",
			identity: "Explorer",
			versionSearch: "MSIE"
		},
		{
			string: navigator.userAgent,
			subString: "Gecko",
			identity: "Mozilla",
			versionSearch: "rv"
		},
		{ 		// for older Netscapes (4-)
			string: navigator.userAgent,
			subString: "Mozilla",
			identity: "Netscape",
			versionSearch: "Mozilla"
		}
	],
	dataOS : [
		{
			string: navigator.platform,
			subString: "Win",
			identity: "Windows"
		},
		{
			string: navigator.platform,
			subString: "Mac",
			identity: "Mac"
		},
		{
			   string: navigator.userAgent,
			   subString: "iPhone",
			   identity: "iPhone/iPod"
	    },
		{
			string: navigator.platform,
			subString: "Linux",
			identity: "Linux"
		}
	]

};
BrowserDetect.init();
var moved;


//Load Event Listeners
function LoadList(){
	ubrow = BrowserDetect.browser + ' ' + BrowserDetect.version;
	if(ubrow != 'Explorer 8'||ubrow != 'Explorer 7'){
		document.getElementById('OvSig').addEventListener('touchstart',startTouch,false);
		document.getElementById('OvSig').addEventListener('touchmove',touchMove,false);
		document.getElementById('OvSig').addEventListener('touchend',touchEnd,false);
		document.getElementById('OvSig').addEventListener('touchenter',startTouch,false);
	
		document.getElementById('Spacer').style.height = document.getElementById('SigCan').style.height;
		//document.getElementById('OvSig').style.left = document.getElementById('SigCan').style.left;
	}
	return false;
}

function startTouch(e){
	if(e.target.tagName.toLowerCase() == 'svg'){
		document.getElementById('SigCan').innerHTML = ''
		touchEnd(e);
	}
	e.preventDefault();
  	TrackNow();
  	return false;
}

function touchMove(e){
	e.preventDefault();
	getMouseXY(e.touches[0]);
	return false;
}

function touchEnd(e){
	e.preventDefault;
	StopTrack();
	return false;
}


//Set value to isSig to true (mainly for mouse movement to track when button is pressed down
function TrackNow(){
document.getElementById('<%=isSig.ClientId%>').value = 'True';

}


//Set new line flag and reset moved flag. Reset isSig value
function StopTrack(){
	if(moved == 1){		
		document.getElementById('<%=isSig.ClientId%>').value = 'False';
		newline = 1;
		moved = 0;
	}
	return false;
}

// Main function to retrieve mouse x-y pos.s, fill textboxes for server side code.
function getMouseXY(e) {
	var signon;
	var obj;
	signon = document.getElementById('<%=isSig.ClientId%>').value;
	if (signon == 'True'){

		OX = tempX;
		OY = tempY;
	

	
		if (OX == 0){
			OX = tempX;
		}
		if (OY == 0){
			OY = tempY;
		}
		obj = document.getElementById('SigCan');
		
    	tempX = e.clientX - document.getElementById('SigCan').offsetLeft;
    	tempY = e.clientY - document.getElementById('SigCan').offsetTop;
    	
    	//Remove all offsets
    	//if(obj.offsetParent){
    	//	do{
    	//		tempX -= obj.offsetLeft;
    	//		tempY -= obj.offsetTop;
    	//	}while (obj = obj.offsetParent)
    	//}
    	
    	//Fill newline array, reset new line flag, set the old X & Y to current X & Y so line starts and new position.
    	if (newline == 1){
    		newobj[n] = OX + ',' + OY;
    		OX = tempX;
    		OY = tempY;
    		n = n + 1;
    		newline = 0;
    	}
		//Fill moveto and lineto arrays
    	mtarray[i] = OX + "," + OY;
    	ltarray[i] = tempX + "," + tempY;
    	
    	//Fill textboxes to be used in server side code
    	i = i + 1;
    	if (mtarray[1] != '') {
    		//document.getElementById('<%=mtAr.ClientId%>').value = tempX + "," + tempY;
  			document.getElementById('<%=mtAr.ClientId%>').value = mtarray.join('|');
  			document.getElementById('<%=ltAr.ClientId%>').value = ltarray.join('|');
  			document.getElementById('<%=nobj.ClientId%>').value = newobj.join('|');
  		}
		mtarray[0] = ltarray[0];
		
		//set moved flag to 1 so touchend code only runs after finger has been moved
		moved = 1;
 	
 		//Build svg and insert into inner html of the div segment
  		return DrawSig();

  	}
  return true;
}

function DrawSig(){
	var inhtml;
	
	sigpath = ''

	//check browser
	ubrow = BrowserDetect.browser + ' ' + BrowserDetect.version;
	
	//get path information
	sigpath = BuildPath();
	
	//if using IE 8 or 7 insert vml into inner HTML of div
	if(ubrow == 'Explorer 8'||ubrow == 'Explorer 7'){

		document.namespaces.add('v', 'urn:schemas-microsoft-com:vml', "#default#VML");
		inhtml = "<v:group style='position:absolute;antialias:true;height:100px;width:500px' coordsize='500,100' coordorigin='0,0'><v:shape style='postition:absolute;height:100px;width:500px' strokeweight = '3pt' ><v:stroke joinstyle='round' endcap='round'/>";
		inhtml = inhtml + "<v:path v ='" + sigpath + " '/>";
		inhtml = inhtml + "</v:shape></v:group>";
		
		document.getElementById('SigCan').innerHTML = inhtml;

		//document.getElementById('ctl00_mtAr').value = inhtml
		
	}
	//if using any other browser insert svg into inner HTML of div
	else{
		inhtml = "<svg><g fill='none' stroke='black' stroke-width='3'><path d='" + sigpath + "'/></g></svg>";
		try{
			document.getElementById('SigCan').innerHTML = inhtml.replace('undefined','');
			//LoadSVGList();
		}
		catch(err){
			txt = 'Error: ' + err.message;
			alert(txt);
		}
	}
	return true;
}	

function BuildPath(){
	var path;
	//Build vml path for ie 7 & 8
	if(ubrow == 'Explorer 8'||ubrow == 'Explorer 7'){
			path = 'M ' + mtarray[0] + ' L ' + ltarray[0];
		for(var p = 1;p < i; p++){
			path = path + ' M ' + mtarray[p] +' L ' + ltarray[p];
		}
	}
	//Build svg path for other browsers
	else{
		var pt1;
		var pt2;
		var pt3;
		var pt4;
		var oldcp;
		for(var p = 0;p < i; p++){
			pt3 = ''
			pt4 = ''
			if (typeof(ltarray[p + 2]) != 'undefined'){
				pt1 = mtarray[p];
				pt2 = ltarray[p];
				pt3 = ltarray[p + 1];
				pt4 = ltarray[p + 2];
				
				for(var r = 0; r <= n -1; r++){
					if(pt2 == newobj[r]){
						pt2 = pt1;
						pt3 = pt1;
						pt4 = pt2;
						oldcp = '';
					}
					else{
						if(pt3 == newobj[r]){
							pt2 = pt1;
							pt3 = pt2;
							pt4 = pt3;
							oldcp = ''
						}
					}
				}
				path = path + ' M ' + pt1.replace(',',' ');
				path = path + ' C ' + ' ' + pt2.replace(',',' ') + ', ' + pt3.replace(',',' ') + ', ' + pt4.replace(',',' ');
			}
			p = p + 2
		}
	}
	oldcp = '';
	return path;
	
}
function ClearSig(){

	document.getElementById('SigCan').innerHTML = '';
	for (var c = 0;c <= i; c++){
		mtarray = [];
		ltarray = [];
		newobj = [];
	}
	i = 0;
	n = 0;
	newline = 0;
	document.getElementById('<%=mtAr.ClientId%>').value = '';
  	document.getElementById('<%=ltAr.ClientId%>').value = '';
  	document.getElementById('<%=nobj.ClientId%>').value = '';
  	return false;
}


</script>
<style> 
 v\:* {behavior=url(#default#VML)}
</style>
<html>
<head>
	<script>

	</script>
</head>
<body>
	<table>
		<tr>
			<td style="" colspan=3>
				<div id ="SigCan" style="width:500px;height:100px;border:1px solid #c3c3c3;background:white;cursor:crosshair;position:absolute;">	
				</div>
				<div id ="OvSig" style = "width:500px;height:100px;border:1px solid #c3c3c3;background:transparent;cursor:crosshair;position:absolute;margin-bottom:100px;" onmousemove = "return getMouseXY(event);" onmousedown = "return TrackNow(event);" onmouseup = "return StopTrack(event);">
				</div>
				<script>
					LoadList();
				</script>
				<asp:image runat = "Server" id ="SigImg"/>
			</td>
		</tr>
		<tr>
			<td><asp:hiddenfield runat="Server" id="mtAr"/></td>
			<td><asp:hiddenfield runat="Server" id="ltAr"/><asp:hiddenfield runat="Server" id ="nobj"/></td>
			<td><asp:hiddenfield runat="Server" id="isSig"/></td>
		</tr>
		<tr>
			<table class="Buttons">
				<tr>
					<td><asp:button runat="Server" id = "btnSave" text="Save Signature" autopostback = "False"/><asp:checkbox id="chkVerify" runat="Server" text="I verify the above signature is mine." visible="false"/></td>
					<td><asp:button id ="btnVerify" runat="server" text="Verify" visible = "false" onclick="VerifySig" /></td>
					<td><asp:button id ="delSig" runat="Server" text="Delete Existing Signature" visible = "false" /></td>
					<td align="right"><asp:button id="btnClear" runat="Server" text = "Clear Signature" OnClientClick="return ClearSig();"/></td>
				</tr>
			</table>
		</tr>
	</table>
</body>
</html>








