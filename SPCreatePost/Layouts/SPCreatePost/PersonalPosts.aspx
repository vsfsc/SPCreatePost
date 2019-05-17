<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalPosts.aspx.cs" Inherits="SPCreatePost.Layouts.SPCreatePost.PersonalPosts" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript">
        var pb_strConfirmCloseMessage;
        var pb_blnCloseWindow = false;
        pb_strConfirmCloseMessage = "确实要离开该页面吗?您的文章内容还没有进行保存！按“确定”继续，或按“取消”留在当前页面。";
        function ConfirmClose() {
            window.event.returnValue = pb_strConfirmCloseMessage;
            pb_blnCloseWindow = true;
        }
        function ShowConfirmClose(blnValue) {
            if (blnValue) {
                document.body.onbeforeunload = ConfirmClose;
            } else {
                document.body.onbeforeunload = null;
            }
        }
        window.onbeforeunload = function ()  // this is body.onload?
        {
            ShowConfirmClose(true);
        }
</script>
</asp:Content>


<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    serverUrl:<asp:TextBox ID="txtSiteUrl"  Width ="200" runat="server"></asp:TextBox>
    dorUrl:<asp:TextBox ID="txtDocUrl"  Width ="200" runat="server"></asp:TextBox>
    <div id="divShow" runat ="server"></div>
<asp:Button ID="btnGet" runat="server" OnClick ="btnGet_Click" Text="获取关注" />
<asp:Literal ID="des" runat="server"></asp:Literal>

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
应用程序页
</asp:Content>
<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
我的应用程序页
</asp:Content>
