using System; 
using System.Collections.Generic; 
using System.Drawing; 
using System.Drawing.Drawing2D; 
using System.Drawing.Imaging; 
using System.IO; 
using System.Linq; 
using System.Web; 
using System.Web.SessionState; 
/// <summary> 
/// Summary description for ImageProcessFactory 
/// </summary> 
namespace Insus.NET
{
    public class ImageProcessFactory : IHttpHandler, IRequiresSessionState
    {
        public ImageProcessFactory()
        {
            // 
            // TODO: Add constructor logic here 
            // 
        }
        public void ProcessRequest(HttpContext context)
        {
            //Checking whether the UploadBytes session variable have anything else not doing anything 
            if ((context.Session["UploadBytes"]) != null)
            {
                byte[] buffer = (byte[])(context.Session["UploadBytes"]);
                context.Response.BinaryWrite(buffer);
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