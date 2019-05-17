using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace VisualWebPartProject1.VisualWebPart1
{
    public partial class VisualWebPart1UserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void Unnamed_Click(object sender, EventArgs e)
        {
            string fileName = "mylistform.txt";

            fileName = Server.MapPath(fileName);
            WriteOutFile(fileName);

        }
        #region 写出文件
        /// <summary>
        /// 写出文件
        /// </summary>
        /// <param name="pdfFile">文件对象</param>
        /// <returns>返回是否成功</returns>
        protected bool WriteOutFile(string filePath)
        {
            System.IO.FileInfo pdfFile = new System.IO.FileInfo(filePath);
            //写出文件
            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("gb2312");
            HttpResponse response = HttpContext.Current.Response;
            response.HeaderEncoding = encoding;
            response.Charset = encoding.HeaderName;
            response.Clear();
            response.ContentEncoding = encoding;
            response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(pdfFile.Name));
            response.AddHeader("Content-Length", pdfFile.Length.ToString());
            response.ContentType = "text/plain";// "application/pdf";
            response.WriteFile(pdfFile.FullName);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            return true;
        }
        #endregion
    }
}
