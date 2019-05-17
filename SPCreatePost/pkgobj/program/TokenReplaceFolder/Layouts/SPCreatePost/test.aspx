<%@ Assembly Name="SPCreatePost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6bdeeb8f8b726d17" %>
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
        <input id="bt1" type="button" onclick="IsValid();" value="button" />
         <asp:Label ID="lblTotalScore" runat="server" class="deifenb" Text="0"></asp:Label>
        <input id="Text1" type="text" />
        <asp:Label ID="dd"  runat="server" Text="fdfsdfdf" ></asp:Label>
        <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" OnClientClick="return testValid()" Text="Button" /><label runat ="server" id="showMsg"></label><SharePoint:PeopleEditor id="userID" runat="server" SelectionSet="User" ValidatorEnabled="true" Width="140" AllowEmpty = "true" MultiSelect = "true" MaximumEntities="2" EntitySeparator=" " /></div>
    <SharePoint:DateTimeControl ID="DateTo" runat="server" DateOnly="True" IsRequiredField="True" ErrorMessage="日期不能为空" MinDate="2014-01-01" />
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
应用程序页
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
我的应用程序页
</asp:Content>
