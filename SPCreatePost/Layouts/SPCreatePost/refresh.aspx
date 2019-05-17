
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="refresh.aspx.cs" Inherits="SPCreatePost.Layouts.SPCreatePost.refresh"  %>

<form id="form1" runat="server">  
    <div runat="server" id ="divSaveAs" style="text-align:center;padding-left:20px;"></div>
       <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
         <asp:ScriptManager ID="ScriptManager1" runat="server">
         </asp:ScriptManager>
         <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="True">
             <ContentTemplate>
                 <asp:Label ID="Label2" runat="server"></asp:Label>
                 <br />
             </ContentTemplate>
         </asp:UpdatePanel>
                 <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
    <div id="doing" runat="server" style="Z-INDEX: 12000; LEFT: 0px; WIDTH: 100%; CURSOR: wait; POSITION: absolute; TOP: 0px; HEIGHT: 100%"> 
             <table width="100%" height="100%"> 
                 <tr align="center" valign="middle"> 
                     <td> 
                         <table width="169" height="62" bgcolor="#99cccc" style="FILTER: Alpha(Opacity=75); WIDTH: 169px; HEIGHT: 62px"> 
                             <tr align="center" valign="middle"> 
                                 <td>正在导出…… .<br/> 
                                     请勿关闭页面 .</td> 
                             </tr> 
                         </table> 
                     </td> 
                 </tr> 
             </table> 
         </div> 
    </form>  
