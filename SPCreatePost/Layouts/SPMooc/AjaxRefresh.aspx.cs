using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SPCreatePost.Layouts.SPMooc
{
    public partial class AjaxRefresh : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Button1.Click += Button1_Click;
            this.Button2.Click += Button2_Click;
            if (Page.Title != "")
                ViewState["title"] = Page.Title;
        }
        void Button1_Click(object sender, EventArgs e)
        {
            this.Label1.Text = "更新时间：" + System.DateTime.Now.ToString();
            Page.Title = ViewState["title"].ToString();
        }

        void Button2_Click(object sender, EventArgs e)
        {
            this.Label1.Text = "更新时间：" + System.DateTime.Now.ToString();
        }
    }
}
