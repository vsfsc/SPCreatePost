<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="WorkEvaluate.Layouts.WorkEvaluate.test" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="JavaScript1.js"></script>
 <script type ="text/javascript" src ="../SPMooc/Validate.js"></script>
    <%--<script type="text/javascript" src="/_layouts/mediaplayer.js"></script>--%>
   <%-- <script type="text/javascript">
        function IsValid() {
            var name = document.getElementById('<%=TextBox1.ClientID%>');
            alert(GetChineseLength(name.value));
        }

    </script>--%>
    <script type="text/javascript">
   
    </script> 
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    		<div id="hitsDiv">当前访问次数：0</div>
		<script type="text/javascript">
			//页面加载，增加一次访问次数，并输出当前的访问次数
			window.onload = function() {
				var itemId = getQueryString("ID");
				updateListItem(itemId);
			}
			var oListItem;
			//更新指定ID的项
			function updateListItem(itemId) {
				var clientContext = new SP.ClientContext();
				var oList = clientContext.get_web().get_lists().getByTitle('创意');
				this.oListItem = oList.getItemById(itemId);
 clientContext.load(oListItem , 'ID');
				alert(oListItem.get_item("ID"));
				var hits = 0;
				//if(oListItem != null) {
				//	if(oListItem.get_item('HitCount')!= null) {
				//		hits = oListItem.get_item('HitCount') ;
				//	} 
				//}
								hitsDiv.innerHTML = "当前访问次数：" + hits;
				clientContext.executeQueryAsync(Function.createDelegate(this, this.onQuerySucceeded), Function.createDelegate(this, this.onQueryFailed));
			}

			//更新成功
			function onQuerySucceeded() {
			    //alert('Item updated!');
                var hits = 0;
				if(oListItem != null) {
					if(oListItem.get_item('HitCount')!= null) {
						hits = oListItem.get_item('HitCount') ;
					} 
				}
				hitsDiv.innerHTML = "当前访问次数：" + hits;

			}

			//更新失败
			function onQueryFailed(sender, args) {
				alert('Request failed. ' + args.get_message() + '\n' + args.get_stackTrace());
			}

			//读取URL中参数
			function getQueryString(name) {
				var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
				var r = window.location.search.substr(1).match(reg);
				if(r != null) return unescape(r[2]);
				return null;
			}
		</script>
    <div runat ="server" id="divTest"></div>
<%--    <iframe src='http://va.neu.edu.cn/CollegeComputer/ImageProcessing/_layouts/15/WopiFrame.aspx?sourcedoc=/CollegeComputer/ImageProcessing/DocLib4/实用配色方案.pdf&action=embedview' width='600px' height='400px' frameborder='0'></iframe>--%>
<%--    <div id="divUsers" runat="server" ></div>--%>
   <%-- <div id="MediaPlayerHost">
        <script type="text/javascript" >
            {
                mediaPlayer.createMediaPlayer(
                  document.getElementById('MediaPlayerHost'),
                  'MediaPlayerHost',
                  '500px', '333px',
                  {
                      displayMode: 'Inline',
                      mediaTitle: 'video show',
                      mediaSource: '',
                      previewImageSource: '',
                      autoPlay: false ,
                      loop: false ,
                      mediaFileExtensions: 'wmv;wma;avi;mpg;mp3;',
                      silverlightMediaExtensions: 'wmv;wma;mp3;'
                  });
            }
        </script>
      </div>
    <div id="MediaPlayerHost1">
        <script type="text/javascript" >
            {
                mediaPlayer.createMediaPlayer(
                  document.getElementById('MediaPlayerHost1'),
                  'MediaPlayerHost',
                  '500px', '333px',
                  {
                      displayMode: 'Inline',
                      mediaTitle: 'video show',
                      mediaSource: 'http://va.neu.edu.cn/CollegeComputer/Networks/DocLib/互联网学习/互联网学习.mp4',
                      previewImageSource: '',
                      autoPlay: false,
                      loop: false,
                      mediaFileExtensions: 'wmv;wma;avi;mpg;mp3;',
                      silverlightMediaExtensions: 'wmv;wma;mp3;'
                  });
            }
        </script>
      </div>--%>
    <div id="divUsers" runat="server" >
        <asp:CheckBoxList ID="CheckBoxList1" runat="server"></asp:CheckBoxList>
        <input id="bt1" type="button" onclick="IsValid();" value="button" />
         <asp:Label ID="lblTotalScore" runat="server" class="deifenb" Text="0"></asp:Label>
        <asp:Label ID="dd"  runat="server" Text="fdfsdfdf" ></asp:Label>
        <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" OnClientClick="return testValid()" Text="Button" /><label runat ="server" id="showMsg"></label><SharePoint:PeopleEditor id="userID" runat="server" SelectionSet="User" ValidatorEnabled="true" Width="140" AllowEmpty = "true" MultiSelect = "true" MaximumEntities="2" EntitySeparator=" " /></div>
    <asp:Button ID="Button2" runat="server" Text="Register" OnClick ="Button2_Click" />
    <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
应用程序页
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
我的应用程序页
</asp:Content>
