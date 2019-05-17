using System;
using System.Web;
using System.Web.UI;


namespace MyError.Layouts.MyError
{
    public partial class Error :Page 
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
                    srcUrl = "";
            }

        }
        string srcUrl;
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect(srcUrl);
        }
    }
}
