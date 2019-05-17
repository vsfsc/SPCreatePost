
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckUser.aspx.cs" Inherits="SPCreatePost.Layouts.SPCreatePost.CheckUser" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <script type="text/jscript">
        function showResult()
        {
            document.getElementById("userid").value = "ok";
        }
    </script>
</head>

<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"> 
</asp:ScriptManager> 
  <asp:UpdatePanel runat ="server" ID ="UpdatePanel1" UpdateMode="Conditional"><ContentTemplate> 
                <table cellspacing="0" cellpadding="0" class="ms-input" style="top:35%; width: 390px; height: 121px; margin-left: 170px; position: absolute;">
                          
                                <tr><td style="width: 90px;"></td>
                                <td colspan="3">
                                <span id="error" style="color:Red;"></span></td></tr>
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
                                    <input  runat="server" type="password" class="ms-long" id="password"  name="password" value="" onkeydown="if(event.keyCode==13)Button1_onclick()" style="background-color:white"/> 
                                </td>
                            <td style="width: 80px; height: 45px;  color:#333; position:relative" align="right" >
                               
                                      <input id="btnLogin"  runat="server" class="button_off" onserverclick="btnLogin_ServerClick" type="button" value="" style=" position: absolute;top:-30px;left:15px;padding:0;margin:0; width:60px;height:60px;"/>

                                               
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
     
         </ContentTemplate>
       <Triggers >
           
           <asp:AsyncPostBackTrigger  ControlID="btnLogin" EventName="ServerClick"  />
       </Triggers>
   </asp:UpdatePanel>  
 </div>
    </form>
</body>
</html>
