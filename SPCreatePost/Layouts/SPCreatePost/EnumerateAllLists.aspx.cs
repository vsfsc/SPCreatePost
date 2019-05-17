using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPCreatePost.Layouts.SPCreatePost
{

    public partial class EnumerateAllLists : LayoutsPageBase
    {
        protected TextBox txtSiteUrl;
        protected TextBox txtWebUrl;
        protected TextBox txtResult;
        protected Button btnRun;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                txtSiteUrl.Text = "http://localhost";
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            using (SPSite mySite = new SPSite(txtSiteUrl.Text))
            {
                using (SPWeb myWeb = mySite.OpenWeb(txtWebUrl.Text))
                {
                    foreach (SPList mylist in myWeb.Lists)
                    {
                        try
                        {
                            sb.AppendLine(mylist.Title + "    " + mylist.TemplateFeatureId + "    " + mylist.BaseTemplate.ToString());

                        }
                        catch
                        {
                            sb.AppendLine("Error " + mylist.Title);
                        }

                    }

                }
            }
            txtResult.Text = sb.ToString();
        }
    }
}