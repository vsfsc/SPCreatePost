using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Navigation;
using Microsoft.SharePoint.WebControls;
using System.Web.UI;
using  System.ComponentModel;

namespace SPCreatePost.Layouts
{
    public partial class AppTest : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //btnQuery.Click+=btnQuery_Click;
            //SPWeb web = SPContext.Current.Web;

            //SPNavigationNodeCollection nodes = web.Navigation.QuickLaunch;
            //foreach (SPNavigationNode node in nodes)
            //    div_Message.InnerText += node.Title;
            string path = "SPCreatePost";
            Response.Write(Server.MapPath(path));
            using (SPSite site = new SPSite("http://xqx2012/eLearning"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList lst = web.Lists["workFile"];
                    SPDocumentLibrary lstDoc = (SPDocumentLibrary)lst;
                    SPFile file = lstDoc.RootFolder.Files[0];
                    string url = file.Url;
                    //SPFile file = web.GetFile(url);
                    //Response.Write(file.Name );
                    //SPFolder folder = web.Folders[file.ParentFolder.Name];
                    //file = folder.Files[file.Name];

                    //var context = WebOperationContext.Current.OutgoingResponse;
                    //context.ContentType = "application/octet-stream";
                    //context.Headers.Add("Content-Disposition", "attachment; filename=" + file.Name  );
                    //Response.Write("header");

                }
            }
             
        }
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            div_Message.InnerHtml += System.DateTime.Now.ToString() + "<br/>";

            //ClientScript.RegisterStartupScript(ClientScript.GetType(), "", "<script type='text/javascript'>MoveLayer();</" + "script>");
        }
        protected void Button1_Click1(object sender, EventArgs e)
        {
            using (SPSite mySite = new SPSite(SPContext.Current.Site.ID ))
            {
                Guid featureID = new Guid("6e1e5426-2ebd-4871-8027-c5ca86371ead");
                SPFeature feature = mySite.Features[featureID];
                if (feature ==null)
                {
                    feature=mySite.Features.Add(featureID);
                }
                string p= feature.TimeActivated.ToString()+"    "+feature.Definition.Status ;
                //foreach (SPFeatureProperty prop in define.Properties)
                //{
                //    p = prop.Name + "    " + prop.Value + "-------";
                    div_Message.InnerHtml += p + "<br/>";
                //}
            }
            ////#1
            //ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "UpdatePanel1", "alert(1)", true);
            ////#2
            //ScriptManager.RegisterStartupScript(this.UpdatePanel2, this.GetType(), "UpdatePanel2", "alert(2)", true);
        }
        protected void Btn_Click(object sender, EventArgs e)
        {
            //if (fu.HasFile)
            //{
            //    Label1.Text = fu.FileName;
            //}
            //创建一个BackgroundWorker线程
            BackgroundWorker bw = new BackgroundWorker();
            //创建一个DoWork事件，指定bw_DoWork方法去做事
            bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            //开始执行
            bw.RunWorkerAsync();
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 1000; i++)
            {
                //this.richTextBox1.Text += i + Environment.NewLine;
            }
        }
        protected void btnQuery_Click1(object sender, EventArgs e)
        {
            int i = 1;
            i = i + 100;
            //SPCreatePostDLL.TestDLL m = new TestDLL();
            //m.Test();
            //string txt = TestDLL.Test();
            //txt = txt.Substring(0, 3);
        }
    }
}
