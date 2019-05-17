using System;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SPCreatePost.Layouts.SPCreatePost
{
    public partial class download : UnsecuredLayoutsPageBase
    {
        protected override bool AllowAnonymousAccess
        {
            get
            {
                return true;
            }
        }
      
        protected void Page_Load(object sender, EventArgs e)
        {
            //byte[] bytes = Convert.FromBase64String(infoHidden.Value);
            //string normalString = Encoding.Default.GetString(bytes);
            //byte[] fileContent = Encoding.Default.GetBytes(normalString);
            //fileName = Server.MapPath(fileName);
            //FileInfo file = new FileInfo(fileName);
            //System.IO.FileStream filestream = new System.IO.FileStream(fileName, System.IO.FileMode.Open);
            //long filesize = filestream.Length;
            //int i = Convert.ToInt32(filesize);
            //byte[] fileBuffer = new byte[i];
            //filestream.Read(fileBuffer, 0, i);
            //filestream.Close();
            //string myFileName = file.Name;
            //string fileData = Convert.ToBase64String(fileBuffer);// "&fd=" + Server.UrlEncode(fileData)+
            string fileName = "../db/export/Afghanistan(qq)_201512011624282858.txt";
            divContent.Controls.Add(new LiteralControl("<a href='CustDownload.aspx?fn=" + Server.UrlEncode(fileName) + "'>CustDownload</a>"));
            divWord.Controls.Add(new LiteralControl("<a href='CustDownload.aspx?fn=Zip'>NewWordsDownload</a>"));
            //HyperLink1.NavigateUrl = "CustDownload.aspx;// ;
        }
        protected void btnOk_Click(object sender, EventArgs e)
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "exported", "document.getElementById('" + btnOk.ClientID + "').disabled=false;document.getElementById('exporting').style.display='none';document.getElementById('" + btnDownload.ClientID + "').click();", true);
        }
        protected void Unnamed_Click(object sender, EventArgs e)
        {
            string fileName = "../db/export/Afghanistan(qq)_201512011624282858.txt";
            //byte[] bytes = Convert.FromBase64String(infoHidden.Value);
            //string normalString = Encoding.Default.GetString(bytes);
            //byte[] fileContent = Encoding.Default.GetBytes(normalString);
            fileName = Server.MapPath(fileName);
            FileInfo file = new FileInfo(fileName);
            System.IO.FileStream filestream = new System.IO.FileStream(fileName, System.IO.FileMode.Open);
            long filesize = filestream.Length;
            int i = Convert.ToInt32(filesize);
            byte[] fileBuffer = new byte[i];
            filestream.Read(fileBuffer, 0, i);
            filestream.Close();
            string myFileName = file.Name;
            string fileData = Convert.ToBase64String(fileBuffer);
            //Response.Redirect("CustDownload.aspx?fn=" + Server.UrlEncode(myFileName) + "&fd=" + Server.UrlEncode(fileData));

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
