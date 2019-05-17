using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.DirectoryServices;

namespace SPCreatePost.Layouts.SPCreatePost
{
    public partial class ADTest : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(Domain );
        }
        public static string Domain
        {
            get
            {
                DirectoryEntry root = new DirectoryEntry("LDAP://rootDSE");

                string domain = (string)root.Properties["ldapServiceName"].Value;
                //domain = domain.Substring(0, domain.IndexOf("."));
                return domain;

            }
        }
    }
}
