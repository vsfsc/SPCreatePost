using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SPCreatePost.Layouts.SPCreatePost
{
    public partial class Error : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Page.Request.QueryString["Source"] != null)
                {
                    string errMsg = Page.Request.QueryString["ErrMsg"];
                    srcUrl = Page.Request.QueryString["Source"];
                    lblErrMsg.Text = errMsg;
                }
                else
                    srcUrl = SPContext.Current.Web.Url;
            }

        }
        string srcUrl;
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect(srcUrl );
        }
    }
}
