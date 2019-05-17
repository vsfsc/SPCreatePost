<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testDateControl.aspx.cs" Inherits="SPCreatePost.Layouts.SPCreatePost.testDateControl" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
<script type="text/javascript">
    function ValidateDate(e)
    {
        alert(e.value);
    }
    function updateDates() {

        var userDateFrom = document.getElementById('<%=dtFrom.Controls[0].ClientID%>').value;
        var userDateTo = document.getElementById('<%=dtTo.Controls[0].ClientID%>').value;

     //var today = new Date();

     //var dd = today.getDate();

     //var mm = today.getMonth() + 1;//January is 0!`

     //var yyyy = today.getFullYear();

     //if (dd < 10) { dd = '0' + dd }

     //if (mm < 10) { mm = '0' + mm }

     //var today = mm + '/' + dd + '/' + yyyy;

     if (new Date(userDateFrom).getTime() > new Date(userDateTo).getTime()) {


         alert('开始时间不能大于结束时间');
         document.getElementById('<%=dtFrom.Controls[0].ClientID%>').focus();
                return;

            }

        }

 </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
   <iframe src='http://va.neu.edu.cn/_layouts/15/WopiFrame.aspx?sourcedoc=/Shared%20Documents/%e6%95%b0%e5%ad%97%e5%8c%96%e5%ad%a6%e4%b9%a0.ppt' width='600px'  height='470px' frameborder='0'></iframe>
<SharePoint:DateTimeControl ID="dtFrom" runat="server"   OnDateChanged="dtFrom_DateChanged"   DateOnly ="true" OnValueChangeClientScript="alert(this.value)"></SharePoint:DateTimeControl>
<SharePoint:DateTimeControl ID="dtTo" runat="server"   DateOnly ="true"  OnValueChangeClientScript="updateDates()"></SharePoint:DateTimeControl>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
应用程序页
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
我的应用程序页
</asp:Content>
