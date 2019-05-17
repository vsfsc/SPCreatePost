<%@ Assembly Name="SPCreatePost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6bdeeb8f8b726d17" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppTest.aspx.cs" Inherits="SPCreatePost.Layouts.AppTest" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
   <%-- <script type ="text/javascript">
        function MoveLayer() {
            var obj = document.getElementById("<%=div_Message.ClientID %>");
          obj.scrollTop = obj.scrollHeight;


      }
  </script>--%>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
<%--<asp:Button ID="btnQuery" runat="server"  Text="Button" OnClick="btnQuery_Click1" />--%>
<%--    <asp:ScriptManager ID="ScriptManager1" runat="server">
         </asp:ScriptManager>--%>
    <div style="height: 100px; overflow: scroll" id="div_Message" runat="server"/>
                       <asp:Button ID="Button2" runat="server" Text="FeatureProper" OnClick="Button1_Click1" />

   <%-- <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>
         <asp:UpdatePanel ID="UpdatePanel1"   runat="server">
              <ContentTemplate>
                  <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="2000">
  </asp:Timer>
  <div style="height: 100px; overflow: scroll" id="div_Message" runat="server"/>
                   <asp:Button ID="Button1" runat="server" Text="FeatureProper" OnClick="Button1_Click1" />
              </ContentTemplate>
         </asp:UpdatePanel>
         
         <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
              <ContentTemplate>
                   <%= DateTime.Now %>
              </ContentTemplate>
         </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="up1">
    <ContentTemplate>
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <asp:FileUpload ID="fu" runat="server" />
        <asp:Button ID="BtnUpload" runat="server" Text="Upload" onclick="Btn_Click" />
     </ContentTemplate>
     <Triggers>
        <asp:PostBackTrigger ControlID="BtnUpload" />
     </Triggers>
    </asp:UpdatePanel>--%>

</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
应用程序页
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
我的应用程序页
</asp:Content>
