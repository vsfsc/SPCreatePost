<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentList.aspx.cs" Inherits="StudentList.Layouts.StudentList.StudentList" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
     <table style="width:auto">
        <tr>
            <td colspan="7">
            <asp:Label ID="errlable" runat="server" Text="Label"></asp:Label></td>
        </tr>
        <tr id="trCondition" runat ="server">
            <td>开始时间：</td><td><SharePoint:DateTimeControl ID="dateFrom"  runat="server" DateOnly="True" IsRequiredField="True" MinDate="2014-01-01" /></td>
               <td>&nbsp;&nbsp;截止时间：</td><td><SharePoint:DateTimeControl ID="DateTo" runat="server" DateOnly="True" IsRequiredField="True" ErrorMessage="日期不能为空" MinDate="2014-01-01" /></td><td>&nbsp;&nbsp;班级：</td><td><asp:DropDownList ID="lstBanJi" runat="server"></asp:DropDownList></td>
            <td>&nbsp;&nbsp;<asp:Button ID="btnSearch" runat="server" Text="查询"  /></td>
        </tr>
        <tr>
            <td colspan="7">
            <asp:GridView runat="server" ID ="GridView2" Width ="100%" ></asp:GridView></td>
        </tr>
        <tr>
            <td colspan="7">
            <asp:GridView runat="server" ID ="GridView1"  Width ="100%" ></asp:GridView></td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
学习情况查询
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
</asp:Content>
