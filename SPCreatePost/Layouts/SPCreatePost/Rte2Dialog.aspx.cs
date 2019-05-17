using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Globalization;
using Microsoft.SharePoint.Utilities;

public partial class Rte2Dialog : System.Web.UI.Page
{
    // Methods
    protected override void InitializeCulture()
    {
        base.InitializeCulture();
        string str = base.Request.QueryString["LCID"];
        if (!string.IsNullOrEmpty(str))
        {
            int num;
            while (int.TryParse(str, out num))
            {
                CultureInfo culture = new CultureInfo(num);
                SPUtility.SetThreadCulture(culture, culture);
                break;
            }
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string scriptLiteralToEncode = base.Request.QueryString["Dialog"];
            this.PageName.Text = SPHttpUtility.EcmaScriptStringLiteralEncode(scriptLiteralToEncode);
            if (!string.IsNullOrEmpty(scriptLiteralToEncode) && (((scriptLiteralToEncode == "CreateLink") || (scriptLiteralToEncode == "InsertImage")) || (scriptLiteralToEncode == "InsertTable")))
            {
                this.PageTitle.Text = SPHttpUtility.HtmlEncode("插入图片");
                this.FirstCaption.Text = SPHttpUtility.HtmlEncode("提示：");
                this.SecondCaption.Text = SPHttpUtility.HtmlEncode("地址：");
            }
        }
    }

    public string InsertImage()
    {
        try
        {
            if (this.FileUpload3.PostedFile.ContentLength != 0)
            {
                SPWeb web = SPContext.Current.Web;
                web.AllowUnsafeUpdates = true;

                SPList list = web.Lists["照片"];
                SPFolderCollection spfolders = list.RootFolder.SubFolders;
                ArrayList arr = new ArrayList(spfolders.Count);
                //获取上传图片的文件名称(包含后缀)
                string[] imgTemp = FileUpload3.PostedFile.FileName.Split('\\');
                string imgFileName = imgTemp[imgTemp.Length - 1];

                foreach (SPFolder spf in spfolders)
                {
                    arr.Add(spf.Name);
                }
                if (!arr.Contains("Article"))
                {
                    list.RootFolder.SubFolders.Add("Article");
                }
                if (!arr.Contains("Comments"))
                {
                    list.RootFolder.SubFolders.Add("Comments");
                }
                list.RootFolder.SubFolders["Article"].Files.Add(imgFileName, FileUpload3.PostedFile.InputStream, true);//true覆盖原有文件
                web.AllowUnsafeUpdates = false;
                return imgFileName + " 上传成功!#" + web.ServerRelativeUrl + "/" + list.RootFolder.Url + "/" + imgFileName;
            }
            return "#";
        }
        catch (Exception ex)
        {
            this.Label1.InnerText = ex.Message;
            return "#";
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        this.form1.Target = "_self";
        this.Button1.Enabled = false;
        if (this.FileUpload3.PostedFile.ContentLength <= 0)
        {
            this.Label1.InnerText = "请选择文件。";
            return;
        }
        try
        {
            if (this.FileUpload3.PostedFile.ContentLength != 0)
            {
                SPWeb web = SPContext.Current.Web;
                web.AllowUnsafeUpdates = true;
                SPList list;
                try
                {
                    list = web.Lists["照片"];
                }
                catch (Exception)
                {
                     web.Lists.Add("照片", "临时照片存放库.", SPListTemplateType.PictureLibrary);
                     list = web.Lists["照片"];
                }
                
                SPFolderCollection spfolders = list.RootFolder.SubFolders;
                ArrayList arr = new ArrayList(spfolders.Count);
                //获取上传图片的文件名称(包含后缀)
                string[] imgTemp = FileUpload3.PostedFile.FileName.Split('\\');
                string imgFileName = imgTemp[imgTemp.Length - 1];

                foreach (SPFolder spf in spfolders)
                {
                    arr.Add(spf.Name);
                }
                if (!arr.Contains("Article"))
                {
                    list.RootFolder.SubFolders.Add("Article");
                }
                if (!arr.Contains("Comments"))
                {
                    list.RootFolder.SubFolders.Add("Comments");
                }
                list.RootFolder.SubFolders["Article"].Files.Add(imgFileName, FileUpload3.PostedFile.InputStream, true);//true覆盖原有文件
                web.AllowUnsafeUpdates = false;
                this.Label1.InnerText = imgFileName + " 上传成功!单击“确定”插入图片。";
                this.StrSecond.Value = web.ServerRelativeUrl + "/" + list.RootFolder.Url + "/Article/" + imgFileName;
            }
        }
        catch (Exception ex)
        {
            this.Button1.Enabled = true;
            this.Label1.InnerText = ex.Message;
        }
    }
}