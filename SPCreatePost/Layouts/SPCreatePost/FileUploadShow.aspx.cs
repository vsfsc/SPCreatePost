using System;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SPCreatePost.Layouts.SPCreatePost
{
    public partial class FileUploadShow : LayoutsPageBase
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            this.FileUpload1.Attributes.Add("onchange", Page.ClientScript.GetPostBackEventReference(this.FileUpload1, "onchange"));
        } 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                var ctrl = Request.Params[Page.postEventSourceID];
                var args = Request.Params[Page.postEventArgumentID];
                OnchangeHandle(ctrl, args);
            } 
        }
        private void OnchangeHandle(string ctrl, string args)
        {
            if (ctrl == this.FileUpload1.UniqueID && args == "onchange")
            {
                this.Image1.Visible = true;
                Session["UploadBytes"] = this.FileUpload1.FileBytes;
                Session["UploadContentType"] = this.FileUpload1.PostedFile.ContentType;

                this.Image1.ImageUrl = "PreviewImage.ashx";
            }
        } 
    }
}
