<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" %>

<script runat="server">
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        SPUser user = SPContext.Current.Web.CurrentUser;
        if (user == null)
            Response.Redirect(SPContext.Current.Web.Url );
        else
        {
            bool isRight = UserHaveRight(user);
            if (isRight)
                Response.Redirect(SPContext.Current.Web.Url + "/_layouts/15/Viewlst.aspx");
            else
                Response.Redirect(SPContext.Current.Web.Url );
        }
    }
    private bool UserHaveRight( SPUser user)
    {
       
        bool isRight = false;
        SPSecurity.RunWithElevatedPrivileges(delegate()
        {
            try
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb sWeb = spSite.AllWebs[SPContext.Current.Web.ID  ])
                    {
                        isRight = sWeb.DoesUserHavePermissions(user.LoginName, SPBasePermissions.FullMask);
                    }  //return DoesUserHavePermssionsToWeb(ref user, ref sWeb);
                }
            }
            catch
            {
                isRight = false;
            }
        });
        return isRight;
    }
</script>



