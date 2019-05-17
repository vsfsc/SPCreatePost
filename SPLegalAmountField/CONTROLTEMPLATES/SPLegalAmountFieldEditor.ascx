<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true"   Inherits="SPLegalAmountField.SPLegalAmountFieldEditor"  %>
<table cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td>
            <b>

                <asp:Literal runat="server" ID="Literal2" Text="RelevanceListField"></asp:Literal></b>
        </td>
        <td>
         
            <asp:DropDownList ID="DrRelevanceListField" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <b>
                 <asp:Literal ID="Literal1" runat="server" Text="AmountField_EditorFieldTextboxWidth"></asp:Literal></b>
        </td>
        <td>
         
            <asp:TextBox ID="txtTextboxWidth" runat="server"></asp:TextBox>
        </td>
    </tr>
</table>