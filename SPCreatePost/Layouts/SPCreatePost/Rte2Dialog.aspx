<%@ Assembly Name="Microsoft.SharePoint.ApplicationPages" %> 
<%@ Page Language="C#" CodeFile="Rte2Dialog.aspx.cs" Inherits="Rte2Dialog"   %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Import Namespace="Microsoft.SharePoint" %>

<script src="/_layouts/<%=System.Threading.Thread.CurrentThread.CurrentUICulture.LCID%>/core.js"></script>
<script src="/_layouts/<%=System.Threading.Thread.CurrentThread.CurrentUICulture.LCID%>/form.js"></script>
<html dir="<SharePoint:EncodedLiteral runat='server' text='<%$Resources:wss,multipages_direction_dir_value%>' EncodeMethod='HtmlEncode'/>">
	<!--TOOLBAR_EXEMPT-->
	<head>
	    	<base target = "_self"/>
		<SharePoint:CssLink ID="CssLink1" runat="server"/>
		<title><asp:Literal id="PageTitle" runat="server"/></title>
		<style type="text/css">
			body
			{
				margin-left:10px;
			}
			input
			{
				font-family:Verdana;
				font-size:8pt;
			}
			td
			{
				font-family:Verdana;
				font-size:8pt;
			}
			button
			{
				font-family:Verdana;
				font-size:8pt;
				width:<asp:Label id="ButtonWidth" text="<%$Resources:wss,RteDialog_ButtonWidth%>" runat="server"/>;
			}
		    .style1
            {
                width: 244px;
            }
		</style>
		<script language="JavaScript">
			function onWindowKeyDownHandler()
			{
				if (event.keyCode == 27)
				{
					window.close();
				}
			}
			function onLoadHandler()
			{
				document.pageName =
					"<asp:Literal id="PageName" runat="server"/>";
				var arg = window.dialogArguments;
				if (document.pageName == "CreateLink" &&
					arg != null)
				{
					StrFirst.value = arg.text;
					document.allowRelativeLinks = arg.allowRelativeLinks;
				}
				RTE_DialogResize();
			}
function Radio3_onclick() {
   document.getElementById("FileUpload3").disabled = "";
   document.getElementById("StrSecond").disabled = "disabled";
   document.getElementById("Button1").disabled="";
   document.getElementById("SecondCaption").disabled="disabled";
    document.getElementById("Label2").disabled="";
}

function Radio2_onclick() {
   document.getElementById("FileUpload3").disabled = "disabled";
   document.getElementById("StrSecond").disabled = "";
    document.getElementById("Button1").disabled="disabled";
    document.getElementById("SecondCaption").disabled="";
    document.getElementById("Label2").disabled="disabled";
}

function Button1_onclick() {
//alert("tesg");
UploadImage();
}

   function callback_UploadImage(res)
    {
	    document.getElementById("Label1").innerHTML = res.value;
    }

    function UploadImage()
    {
	        Rte2Dialog.InsertImage(callback_UploadImage);
    }	
    function SaveAndClose()
    {
            var arr = new Array();
			arr[0] = document.getElementById("StrFirst").value;
			arr[1] = document.getElementById("StrSecond").value;

			 window.returnValue = arr;
			 window.close();
 
    }
    </script>
		<script language="JavaScript" for="OKButton" event="onclick">
		<!--
			var arr = new Array();
			var valid = 1;
			arr[0] = StrFirst.value;
			arr[1] = StrSecond.value;
			if (document.pageName == "InsertTable")
			{
				if (arr[0] < 1 ||
					arr[1] < 1 ||
					isNaN(Number(arr[0])) ||
					isNaN(Number(arr[1])))
				{
					alert("<SharePoint:EncodedLiteral runat='server' text='<%$Resources:wss,RteDialog_RowColValid%>' EncodeMethod='EcmaScriptStringLiteralEncode'/>");
					valid = 0;
				}
				if (valid && arr[0] * arr[1] > 625)
				{
					alert("<SharePoint:EncodedLiteral runat='server' text='<%$Resources:wss,RteDialog_RowColLimit%>' EncodeMethod='EcmaScriptStringLiteralEncode'/>");
					valid = 0;
				}
			}
			if (document.pageName == "CreateLink")
			{
				if (!IsSafeHrefAlert(arr[1], document.allowRelativeLinks))
				{
					valid = false;
				}
			}
			if (valid)
			{
				window.returnValue = arr;
				//window.close();
			}
		-->
		</script>
	</head>
	<body class="ms-BuilderBackground" onkeydown="onWindowKeyDownHandler();" onload="onLoadHandler();">
		<form id="form1" runat="server">
		<center>
	    <table style="border-top: none;" cellspacing="5" id="DialogTable">
	        <tr>
		    <td><label for="StrFirst"><asp:Label id="FirstCaption" runat="server"/></label></td>
		    <td ><input id="StrFirst" type="text" size=30 maxlength=1024></input></td>
		</tr>
		<tr>
		    <td><input id="Radio2" type="radio" name="imgUpload"  onclick="return Radio2_onclick()" /><label for="StrSecond"><asp:Label id="SecondCaption" Enabled="false" runat="server"/></label></td>
		    <td ><input id="StrSecond" runat="server" type="text" size=30 maxlength=1024 disabled="disabled"></input></td>
		</tr>
		<tr>
		    <td>
                <input id="Radio3" type="radio" name="imgUpload" checked="checked" onclick="return Radio3_onclick()" /><asp:Label id="Label2"  Text="上传:" runat="server"/></td>
		    <td class="style1"><input id="FileUpload3" type="file" runat="server" />
            &nbsp;</td>
		</tr>

		<tr><td align="<SharePoint:EncodedLiteral runat='server' text='<%$Resources:wss,multipages_direction_right_align_value%>' EncodeMethod='HtmlEncode'/>" colspan="2">
		    <asp:Button ID="Button1" runat="server" class="ms-ButtonHeightWidth" onclick="Button1_Click"  Text="上传"  />&nbsp;&nbsp;
					<button id="CancelButton" class="ms-ButtonHeightWidth" onclick="SaveAndClose();"><asp:Label id="CancelButton" text="<%$Resources:wss,RteDialog_OK%>" runat="server"/></button>		
				
                    </td></tr>
						<tr>
		    <td colspan="2"><span id="Label1" runat="server" style="color:Red; font-weight:bold;"></span>
            </td>
		</tr>
			</table>
		</center>
		<br/>
	</form>
	</body>
</html>
