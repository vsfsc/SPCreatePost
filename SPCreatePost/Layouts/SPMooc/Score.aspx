<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Score.aspx.cs" Inherits="SPCreatePost.Layouts.SPMooc.Score" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <link rel="stylesheet" type="text/css" href="css/keyboard.css" />
    <script type="text/javascript" src="css/keyboard.js"></script> 
     <script    src="Validate.js" type="text/jscript"></script> 
    <script type="text/javascript" src="../SPCreatePost/JavaScript1.js"></script> 
    <script   type="text/javascript">
       
    </script> 
    <style type="text/css">
      *{ margin:0; padding:0}
      body{background:#e1e1e3; margin:0 auto;text-align:center;}
      .miancontent{width:1002px;border:1px solid #ccc;text-align:left;background:white;margin:10px auto 0;padding:0;}
      .WorkInfomaition{background:#ecece2;height:40px;font-size:14px;padding:0px;color:#45443c;vertical-align:middle;line-height:40px; overflow:hidden}
    .WorkInfomaition ul{list-style:none;margin:0;padding:0;margin-left:10px;}
    .WorkInfomaition ul li{float:left;padding:0 10px; font-size:16px;font-family:微软雅黑;}
    .MainTitle{margin:0;padding:0; height:40px;width:100%;background:#45443c;vertical-align:middle;line-height:40px; overflow:hidden;color:#fff;font-family:"微软雅黑";font-size:20px}
    .ImgF{ float:left;margin-left:20px;}
    .fl{float:left;}
    .mainDiv{height:100px;background:url(images/fen30.gif) no-repeat 20px 15px}
    .mainDiv2{height:100px;background:url(images/fen40.gif) no-repeat 20px 15px}
    .wd500{width:500px}
    .mt{ margin-top:25px}
    .h30{height:30px;width:150px; font-size:24px;}
    .ptitle{font-family:微软雅黑;font-size:24px;margin:15px 0 0 0px;}
    .pin{ font-family:微软雅黑;font-size:14px;margin-top:2px;color:#45443c}
    .buttonstyle{ width:120px;height:45px; background:#867f37; color:White;font-family:微软雅黑;font-size:24px; cursor:pointer; margin:8px 150px 0 100px ;}
    </style>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
     <div id="mainContent" style="margin:0;padding:0">
      <div class="MainTitle">
        <span style="margin-left:20px"> 辽宁省大学计算机设计大赛决赛打分平台
          </span>
      &nbsp;
               <asp:ScriptManager ID="ScriptManager1" runat="server">
          </asp:ScriptManager> 
      </div>
      <div class="WorkInfomaition">
            <ul>
             
             <li>
                 作品编号：<asp:Label  ID="lblWorksNum" runat="server"></asp:Label>
             </li>    
             <li>
                作品名称： <asp:Label  ID="lblWorksName" runat="server"/>
             </li >
             <li> 作品类别：<asp:Label  ID="ddlWorksType" runat="server"></asp:Label>
             </li>     
            <li  style="float:right"><span style="color:#00aa00">序号：</span><asp:DropDownList 
                    ID="ddlNumber" runat="server" Width="90px" AutoPostBack="True">
                  </asp:DropDownList>
            </li>
                        <asp:HiddenField ID="HiddenField1" runat="server" />
           </ul>
     </div>
    <!-- 打分 -->
      <div id="divContent" runat="server"> 
          <div class="mainDiv"><div class="fl" style="margin-left:150px;width:500px"><p class="ptitle">作品完整性</p><p class="pin" >作品完整、简捷、明确，主题
          符合大赛要求，内容能够清晰表达设计意图</p></div><input type="text"  runat="server"  value="" class="keyboardInput mt h30" id="sss"/></div>
      
<div style="height:20px;">&nbsp;</div>  
        <div class="mainDiv2"><div class="fl wd500" style="margin-left:150px"><p class="ptitle">作品质量性</p>
        <p class="pin">布置合理、色彩协调、特色鲜明、有新创意、技术运用有创新、技巧多且合理、设计开发规范</p>
        </div><input type="text" value="" class="keyboardInput mt h30" /></div>
   <div style="height:20px;">&nbsp;</div>
       
       <div class="mainDiv"><div class="fl wd500" style="margin-left:150px"><p class="ptitle">现场表现</p>
       <p class="pin">作品介绍明确清晰、演示流畅不出错、答辩正确简要、作品各方面协调配合、总体印象突出</p>
       </div><input type="text" value="" class="keyboardInput mt h30"/>
      </div>
   <div style="height:20px;">&nbsp;
                    </div>
	</div>   
       <!-- end #mainContent --></div>
	 <div  style=" margin:0; clear:both; width:100%;padding:0; height:60px;text-align:right;background:#ecece2;">
	<span style="font-family:微软雅黑;font-size:32px;color:#45443c;">总分</span>
	<span runat="server" id ="lblTotalScore"  style="color:#ff8a00;font-family:微软雅黑;font-size:36px;"></span> 
         <asp:Button ID="btnSubmit" runat="server" Text="提 交"  OnClientClick="javascript:return checkSubmitSelt()" CssClass="buttonstyle" BorderWidth="0"/></div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
应用程序页
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
我的应用程序页
</asp:Content>
