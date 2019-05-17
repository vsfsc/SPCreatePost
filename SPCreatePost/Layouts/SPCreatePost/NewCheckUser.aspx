<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewCheckUser.aspx.cs" Inherits="SPCreatePost.Layouts.SPCreatePost.New1" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
      <script type="text/jscript" >
          function clearName() {
              document.getElementById('<%=userid.ClientID%>').value = "";
          }
          function retUrl(key) {
              var svalue = window.location.search.match(new RegExp("[\?\&]" + key + "=([^\&]*)(\&?)", "i"));
              return svalue ? svalue[1] : svalue;
          }
          function autoCheckUser() {
              document.getElementById('<%=btnLogin.ClientID%>').serverclick();
           }
          function UserValidate(strName, strPWD) {
              document.getElementById('<%=error.ClientID%>').innerHTML = "";
<%--              var strName =document.getElementById('<%=userid.ClientID%>').value;//登陆的名字建议使用：域\名字
              var strPWD = document.getElementById('<%=password.ClientID%>').value;//登陆密码--%>
                 var location = '/login/default.aspx';
                  var auth = null;

                  if (window.XMLHttpRequest)// Firefox, Opera 8.0+, Safari
                      auth = new XMLHttpRequest();
                  else {
                      try {
                          auth = new ActiveXObject("Msxml2.XMLHTTP");
                      }
                      catch (e) {
                          auth = new ActiveXObject("Microsoft.XMLHTTP");
                      }
                  }
                  strName = strName.toLowerCase();
                  if (strName.indexOf("\\") < 0) {
                      strName = "ccc\\" + strName;
                  }
                  auth.open('post', location, false, strName, strPWD);
                  auth.send();
                  switch (auth.status) {
                      case 200:
                          window.location.href = '/login/default.aspx'; // 登陆页面
                          var c = retUrl("Resource");
                          if (c == null)
                              window.location.href = '/default.aspx';
                          else
                              window.location.href = c;
                          break;
                      case 401:
                          {

                              document.getElementById('error').innerText = "帐号或密码错误！";
                          }
                          break;

                      default: document.getElementById('error').innerText = '抱歉，请再试一次！';

                  }
              }
    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
                <table cellspacing="0" cellpadding="0" class="ms-input" style="top:35%; width: 390px; height: 121px; margin-left: 170px; position: absolute;">
                                <tr><td style="width: 90px;"></td>
                                <td colspan="3">
                                <span id="error" runat="server" title="ccc"   style="color:Red;"></span></td></tr>
                            <tr><td style="width: 90px; color:#333" align="right" rowspan="2">
                                <div style="width:90px;height:90px;">
                                 <img src="/_layouts/15/images/login/header.jpg" width="90px" height="90px" style="padding:0;margin:0px;margin-right:0"/>
                                </div>
                                </td>
                              
                                <td style="height: 45px; width: 220px;" align="left" >
                                   <input runat="server"  type="text" id="userid" class="ms-long" name="userid" onkeydown="if(event.keyCode==13)event.keyCode=9" value="请输入账号" onfocus="clearName()" style="background-color:white"/></td>
                                   <td style="width: 80px; height: 45px; "></td>    
                            </tr>  

                            <tr>
                              <td style="height: 45px; width: 220px;" align="left" >
                                    <input  runat="server" type="password"   class="ms-long" id="password"  name="password" value="" onkeydown="if(event.keyCode==13)document.getElementById('<%=btnLogin.ClientID%>').serverclick();" style="background-color:white"/> 
                                </td>
                            <td style="width: 80px; height: 45px;  color:#333; position:relative" align="right" >
                                      <input id="btnLogin"    runat="server" class="button_off" onserverclick="btnLogin_ServerClick" type="button" value="" style=" position: absolute;top:-30px;left:15px;padding:0;margin:0; width:60px;height:60px;"/>
                                </td>
                            </tr>
                            <tr>
                                        <td style="width: 90px;" align="right">
                                         </td>
                                         <td  width="220px"  align="left">
                                        <input id="Button2"  class="button_off2"  onclick="alert('忘记密码请联系管理员')" type="button" value="忘记密码" />
                                        </td><td valign="bottom" colspan="2">
                                                      </td>
                            </tr>
                        
                </table>
 
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
应用程序页
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
我的应用程序页
</asp:Content>
