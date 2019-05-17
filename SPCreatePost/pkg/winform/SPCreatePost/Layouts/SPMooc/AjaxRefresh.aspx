<%@ Assembly Name="SPCreatePost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6bdeeb8f8b726d17" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxRefresh.aspx.cs" Inherits="SPCreatePost.Layouts.SPMooc.AjaxRefresh" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
  
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">

            <ContentTemplate>

                <div>

                    <asp:Button ID="Button1" runat="server" Text="异步回送"  />

                    <asp:Button ID="Button2" runat="server" Text="整页回送"  /><br />

                    <br />

                    <asp:Label ID="Label1" runat="server" Text="当前时间" Font-Bold="True" Font-Size="Large"></asp:Label></div>

            </ContentTemplate>

            <Triggers>

                <asp:AsyncPostBackTrigger ControlID="Button1"/>

                <asp:PostBackTrigger ControlID="Button2" />

            </Triggers>

        </asp:UpdatePanel>
 
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
PageTitle
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
PageTitleInTitleArea
</asp:Content>
