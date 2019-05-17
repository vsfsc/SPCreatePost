using System;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.WebControls;

namespace SPCreatePost.Layouts.SPCreatePost
{
    public partial class CustomLogin : UnsecuredLayoutsPageBase
    {
        protected override bool AllowAnonymousAccess
        {
            get
            {
                return true;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void lbInternalUsers_Click(object sender, EventArgs e)
        {
            try
            {

                if (null != SPContext.Current && null != SPContext.Current.Site)
                {

                    SPIisSettings iisSettings = SPContext.Current.Site.WebApplication.IisSettings[SPUrlZone.Default];

                    if (null != iisSettings && iisSettings.UseWindowsClaimsAuthenticationProvider)
                    {

                        SPAuthenticationProvider provider = iisSettings.WindowsClaimsAuthenticationProvider;

                        Redirect(provider);

                    }

                }

            }

            catch (Exception ex)
            {

            }

        }

        private void Redirect(SPAuthenticationProvider provider)
        {

            string comp = HttpContext.Current.Request.Url.GetComponents(UriComponents.Query, UriFormat.SafeUnescaped);

            string url = provider.AuthenticationRedirectionUrl.ToString();

            if (provider is SPWindowsAuthenticationProvider)
            {

                comp = EnsureUrl(comp, true);

            }

            SPUtility.Redirect(url, SPRedirectFlags.Default, this.Context, comp);

        }

        //http://skyrim:6050/_windows/default.aspx?ReturnUrl=

        private string EnsureUrl(string url, bool urlIsQueryStringOnly)
        {

            if (!url.Contains("ReturnUrl="))
            {

                if (urlIsQueryStringOnly)
                {

                    url = url + (string.IsNullOrEmpty(url) ? "" : "&");

                }

                else
                {

                    url = url + ((url.IndexOf('?') == -1) ? "?" : "&");

                }

                url = url + "ReturnUrl=";

            }

            return url;

        }

    }
}