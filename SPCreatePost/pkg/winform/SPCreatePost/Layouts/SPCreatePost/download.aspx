<%@ Assembly Name="SPCreatePost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6bdeeb8f8b726d17" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="download.aspx.cs" Inherits="SPCreatePost.Layouts.SPCreatePost.download" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div runat ="server" id="divContent"></div>
    <div runat ="server" id="divWord"></div>
    <asp:HyperLink ID="HyperLink1"   runat="server">download</asp:HyperLink>
    <asp:LinkButton ID="LinkButton1"   runat="server">LinkButton</asp:LinkButton>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <asp:Button ID ="btnOk" runat="server" Text="打印" OnClick="btnOk_Click" />
            <div id="exporting" style="display:none;"><br /><font color="#ff0000" size="4.5px">数据正在打印中，请稍等……</font></div>
    <asp:Button runat="server" Text="DownLoad" ID ="btnDownload"  OnClick="Unnamed_Click" ></asp:Button>
    <asp:FileUpload ID="fileUpload" runat="server" />
    <asp:Label ID="lblMsg" runat="server"  Text="lblMsg"></asp:Label><br />
    <asp:TextBox ID="txtShow" runat="server" TextMode="MultiLine" Width ="800px" Height ="600px" ></asp:TextBox>
    
         </ContentTemplate>
     <Triggers>
        <asp:PostBackTrigger ControlID="btnDownload" />
     </Triggers>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
应用程序页
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
我的应用程序页
</asp:Content>
