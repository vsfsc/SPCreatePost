<%@ Assembly Name="SPCreatePost, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6bdeeb8f8b726d17" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LatestBlog.aspx.cs" Inherits="SPCreatePost.Layouts.SPCreatePost.LatestBlog" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <style type="text/css" >
        body{background-color:#e9edf0}
        .NewBlogDiv{margin:10px auto;width:700px;float:left}
        .NewBlogDiv_Title{background-color:#fbfbfb; height:42px;border:1px solid #d9d9d9; border-bottom:1px solid #d9d9d9; line-height:42px}
        .NewBlogDiv_Title h1{margin:0 15px;font-family:微软雅黑;font-size:14px}
        .Newblog {list-style: none;margin:0;padding:0;}
        .Newblog_F_bar{border:1px solid #d9d9d9; border-top:0;zoom: 1;overflow:auto;background-color:#fff}
        .Newblog_N_bar{border:1px solid #d9d9d9;zoom: 1;overflow:auto;background-color:#fff;margin-top:10px}
        .Newblog_bar_Content{float:left;width:550px;margin:10px;}
        .Newblog_headImage{float:left;width:60px; height:60px; border-radius:50%; overflow:hidden;margin:10px}
        .Newblog_headImage img{width:60px;height:60px;display:block}
        .Newblog_Name{font-weight:normal;margin:10px 0;}
        .Newblog_Name a{font-family:微软雅黑;font-size:16px;color:#7ab324;text-decoration:none;cursor:pointer}
        .Newblog_Title{color:#0072c6; font-weight:normal;font-family:微软雅黑;font-size:14px;cursor:pointer; text-decoration:none}
        .Newblog_P{font-family:微软雅黑;line-height:1.5;margin:5px 0 10px;font-size:14px;color:#333;}
        .Newblog_Time{color:#666; font-size:13px;margin:5px 0px}
        .Newblog_NextPage{background-color:#fbfbfb; height:42px;border:1px solid #d9d9d9; border-top:0px; text-align:center; line-height:42px;margin:0 0 15px 0;}
        .Newblog_NextPage h4{margin:0 15px;font-family:微软雅黑;font-size:14px}

        .blogTop{overflow:auto;width:290px;margin-left:10px;float:left;margin-top:10px}
        .blogTop_Title{background-color:#fbfbfb; height:42px;border:1px solid #d9d9d9; border-bottom:1px solid #d9d9d9; line-height:42px}
        .blogTop_Title h1{margin:0 15px;font-family:微软雅黑;font-size:14px}
        .blogTop_Title span{color:#999;margin-left:15px}
        .blogTop_Block{border:1px solid #d9d9d9; border-top:0;border-bottom:1px dotted #efefef;zoom: 1;overflow:auto;background-color:#fff}
        .blogTop_Head{float:left;width:40px; height:40px; overflow:hidden;margin:10px}
        .blogTop_Head img{width:40px;height:40px;display:block}
        .blogTop_Name{float:left;margin:10px}
        .blogTop_Name h3{font-family:微软雅黑;font-size:16px;font-weight:normal;margin:2px 0;color:#7ab324}
        .blogTop_Name h3 a{font-family:微软雅黑;font-size:14px;text-decoration:none;cursor:pointer;color:#666}
        .blogTop_Name p{font-family:微软雅黑;font-size:12px;color:#999}

        .Hotblog_main{overflow: hidden;width:100%;margin-top:10px}
        .Hotblog_Title{background-color:#fbfbfb; height:42px;border:1px solid #d9d9d9; border-bottom:1px solid #d9d9d9; line-height:42px}
        .Hotblog_Title h1{margin:0 15px;font-family:微软雅黑;font-size:14px}
        .hotBlog{ background-color:white;background-color:#fff;overflow: hidden;border:1px solid #d9d9d9;border-top:0}
        .hotBlog li{margin:8px 0; }
        .hotBlog_block{zoom: 1;overflow:auto;vertical-align:middle;}
        .hotBlog_Circle_First{width:15px;height:15px; border-radius:50%;background-color:#df3e3e;text-align:center;color:#fff;font-family:Arial;margin:5px;float:left}
        .hotBlog_Circle_Second{width:15px;height:15px; border-radius:50%;background-color:#df3e3e;text-align:center;color:#fff;font-family:Arial;margin:5px;float:left}
        .hotBlog_block h3{margin-top:3px}
        .hotBlog_block h3 a{font-family:微软雅黑;font-size:13px;text-decoration:none;cursor:pointer;color:#333;margin:5px;font-weight:normal;}


    </style>

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <!-- 最新博文 -->
       <div class="NewBlogDiv"  >
         <div class="NewBlogDiv_Title"><h1>最新博文</h1></div>
          <!-- 最新博文列表 -->
           <div runat="server" id="divBlogContent">
           <ul></ul></div> 
        <div class="Newblog_NextPage"><h4><span style="font-size:13px; font-weight:normal">第<asp:Label ID="lblPage" runat="server" Text=""></asp:Label>页</span>&nbsp;&nbsp;
            <asp:LinkButton ID="btnPrev" runat="server" Enabled="True">上一页</asp:LinkButton>&nbsp;&nbsp;
                <asp:LinkButton ID="btnNext" runat="server" Enabled="True">下一页</asp:LinkButton></h4></div>
      </div> 
    <div class="blogTop">
    <div runat ="server" id="divBlogTotal">
               <div class="blogTop_Title"><h1>博客大咖<span>本月TOP5</span></h1></div>
                <ul  class="Newblog">
            <li id="lv1"></li>
                </ul>
    </div>
    <div class="Hotblog_main" runat="server" id="divHotBlog" >    
        <div class="Hotblog_Title" ><h1>最热博文</h1></div>
        <div  class="hotBlog">
                <ul class="Newblog" >
            <li id="lv2"></li>
        </ul> 
            </div>
            </div></div></asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
 VA博客公共主页
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >

</asp:Content>
