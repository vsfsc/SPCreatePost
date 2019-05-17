using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SPCreatePost.Layouts.SPCreatePost
{
    public partial class MediaPlayer : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(Page.AppRelativeVirtualPath);
        }
    }
}
