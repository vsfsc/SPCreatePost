using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web;
using System.Web.SessionState; 


namespace SPCreatePost.Layouts.SPCreatePost
{
    public partial class PreviewImage : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            //Checking whether the UploadBytes session variable have anything else not doing anything 
            if ((HttpContext.Current.Session["UploadBytes"]) != null)
            {
                byte[] file = (byte[])(HttpContext.Current.Session["UploadBytes"]);
                context.Response.ContentType = HttpContext.Current.Session["UploadContentType"].ToString() ;// "image/jpeg";
                context.Response.BinaryWrite(file);  
                //context.Response.BinaryWrite(file);
                //System.IO.MemoryStream stream = new System.IO.MemoryStream();
                //stream = new System.IO.MemoryStream(file);
                //int buffersize = (int)stream.Length;
                //byte[] buffer = new byte[buffersize];
                //int conentsize = stream.Read(buffer, 0, buffersize);
                //context.Response.OutputStream.Write(buffer, 0, conentsize);
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
