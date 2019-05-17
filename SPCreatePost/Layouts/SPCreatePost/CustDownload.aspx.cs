using System;
using System.Text;
using System.IO;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Ionic.Zip;

namespace SPCreatePost.Layouts.SPCreatePost
{
    public partial class CustDownload : UnsecuredLayoutsPageBase
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
            //if (Request["fn"] != null)// && Request["fd"] != null)
            //{
            //    string fileName = Request["fn"] == string.Empty ? "ImageInfo" : Request["fn"];
            //    //string fileData = Request["fd"];
            //    fileName = Server.MapPath(fileName);
            //    //byte[] bytes = Convert.FromBase64String(fileData);
            //    //string normalString = Encoding.Default.GetString(bytes);
            //    //byte[] fileContent = Encoding.Default.GetBytes(normalString);
            //    FileInfo file = new FileInfo(fileName);
            //    System.IO.FileStream filestream = new System.IO.FileStream(fileName, System.IO.FileMode.Open);
            //    long filesize = filestream.Length;
            //    int i = Convert.ToInt32(filesize);
            //    byte[] fileContent = new byte[i];
            //    filestream.Read(fileContent, 0, i);
            //    filestream.Close();
            //    Response.Clear();
            //    Response.ClearHeaders();
            //    Response.ClearContent();
            //    Response.Buffer = true;
            //    Response.AddHeader("Content-Disposition",
            //      "attachment;filename=" + file.Name  );// + ".txt");
            //    Response.ContentType = "text/plain";
            //    Response.BinaryWrite(fileContent);
            //    Response.End();
            //}
            if (Request["fn"] == "Zip")
            {
                Response.Clear();
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "filename=AllWords.zip");
                string excelPath = Server.MapPath("/_layouts/15/db/excel/");
                DirectoryInfo path = new DirectoryInfo(excelPath);
                using (ZipFile zip = new ZipFile(System.Text.Encoding.Default))//解决中文乱码问题
                {
                    int k = 0;
                    foreach (FileInfo f in path.GetFiles())
                    {
                        zip.AddFile(f.FullName, "");//filename列
                        k++;
                    }
                    if (k > 0)
                    {
                        zip.Save(Response.OutputStream);
                    }
                    else
                    {
                        return;
                    }

                }
                Response.End();
            }
        }
    }
}
