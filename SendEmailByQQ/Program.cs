using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Net;
using System.Net.Mail;
using System.DirectoryServices;
using System.Data;
using System.IO;
using System.Configuration;

namespace SendEmailByQQ
{
    class Program
    {
        static void Main(string[] args)
        {
            siteUrl = ConfigurationManager.AppSettings["siteUrl"];
            string[] teacherList =getBanJi( ConfigurationManager.AppSettings["teacherList"]);
            GetStudentInfo(siteUrl, teacherList);
        }
        static  string siteUrl;
        static ClientContext clientContext;
        #region 外加的方法
        private static string GetTeacherClassLists(List oList, string dispName)
        {
            CamlQuery oQuery = new CamlQuery();
            oQuery.ViewXml = "<View><Query><Where><Eq><FieldRef Name='Title'/><Value Type='Text'>" + dispName + "</Value></Eq></Where></Query></View>";
            ListItemCollection lstItems = oList.GetItems(oQuery);
            clientContext.Load(lstItems, items => items.Include(item => item["Title"], item => item["ClassList"], item => item.Id));
            clientContext.ExecuteQuery();
            if (lstItems.Count > 0)
            {
                try
                {
                    string bjLists = lstItems[0]["ClassList"].ToString();
                    return bjLists;
                }
                catch
                {
                    return "";
                }

            }
            return "";
        }
        private static void GetStudentInfo(string siteUrl, string[] dispName)
        {
            string listName = "课表";
            string _subADPath = "LDAP://OU=东北大学本科生,DC=ccc,DC=neu,DC=edu,DC=cn";
            clientContext = new ClientContext(siteUrl);
            Site site = clientContext.Site;
            Web web = clientContext.Web;
            clientContext.Load(web, w => w.Title);
            List oList = web.Lists.GetByTitle(listName);
            ArrayList classLists = new ArrayList();
              CamlQuery oQuery = new CamlQuery();
            string xmlQuery = "<Eq><FieldRef Name='Title'/><Value Type='Text'>" + dispName[0] + "</Value></Eq>";
            for (int i = 1; i < dispName.Length; i++)
            {
                xmlQuery = "<Or><Eq><FieldRef Name='Title'/><Value Type='Text'>" + dispName[i] + "</Value></Eq>" + xmlQuery + "</Or>";
            }
            oQuery.ViewXml = "<View><Query><Where>"+xmlQuery+"</Where></Query></View>";

            ListItemCollection lstItems = oList.GetItems(oQuery);
            clientContext.Load(lstItems, items => items.Include(item => item["Title"], item => item["ClassList"], item => item.Id));
            clientContext.ExecuteQuery();
            if (lstItems.Count > 0)
            {
                foreach (ListItem item in lstItems )
                {
                    try
                    {
                        string bjLists = item["ClassList"].ToString();
                        string[] bjs = getBanJi(bjLists);
                        foreach (string subBj in bjs)
                        {
                            if (!classLists.Contains(subBj) && subBj != "")
                                classLists.Add(subBj);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            string bjAdPath;
            DataTable dt = new DataTable();
            dt.Columns.Add("xueHao", typeof(string));
            dt.Columns.Add("mail", typeof(string));
            dt.Columns.Add("blog", typeof(string));
            foreach (string subBj in classLists)
            {
                bjAdPath = GetClassEntry(_subADPath, subBj);
                if (bjAdPath != "")
                {
                    GetStudentByClass(bjAdPath, ref dt, subBj);
                }
            }
            CheckBlog(dt);

        }
        /// <summary>
        /// 遍历博客文章,最近7天无更新发邮件
        /// SendMail("training@mail.neu.edu.cn", "training", "110004cc", new string[] { txtEmail.Text }, "密码重置成功", "密码重置为身份证后8位：" + pwd);
        /// </summary>
        /// <param name="dt"></param>
        private static void CheckBlog(DataTable dt)
        {
            StreamWriter write = new StreamWriter("c:\\sendEmail.txt", false);
            string disPlayName = System.Configuration.ConfigurationManager.AppSettings["emailDisplayName"];
            string title = System.Configuration.ConfigurationManager.AppSettings["emailTitle"];
            string body = System.Configuration.ConfigurationManager.AppSettings["emailBody"];
            int delayDays = int.Parse(System.Configuration.ConfigurationManager.AppSettings["delayDays"]);
            CamlQuery oQuery = new CamlQuery();
            oQuery.ViewXml = "<View><Query><Where><Geq><FieldRef Name='PublishedDate'/><Value Type='DateTime'>" + DateTime.Today.AddDays(-delayDays).ToString("yyyy-MM-dd") + "</Value></Geq></Where></Query></View>";
            foreach (DataRow dr in dt.Rows)
            {
                string siteUrl = dr["blog"].ToString();
                clientContext = new ClientContext(siteUrl);
                Web web = clientContext.Web;
                List oList = web.Lists.GetByTitle("Posts");
                ListItemCollection lstItems = oList.GetItems(oQuery);
                clientContext.Load(lstItems, items => items.Include(item => item["Title"], item => item.Id));
                try
                {
                    clientContext.ExecuteQuery();
                    if (lstItems.Count == 0)
                    {
                        write.WriteLine(DateTime.Now.ToString() + "   " + dr["xueHao"].ToString());
                        SendMail("training@mail.neu.edu.cn", disPlayName, "110004cc", new string[] { dr["mail"].ToString() }, title, body + "<br>" + siteUrl);
                    }
                }
                catch
                {
                    write.WriteLine(DateTime.Now.ToString() + "   " + siteUrl + " 不存在");
                    //SendMail("training@mail.neu.edu.cn", "系统管理员", "110004cc", new string[] { dr["mail"].ToString() }, "您的学习记录没有创建", "您的学习记录还没有创建，请及时创建！<br>" + siteUrl);
                }
            }
            write.Close();
            write.Dispose();
        }

        //班级下面的学生,返回班级人数
        private static void  GetStudentByClass(string adPath, ref DataTable dt, string bjName)
        {
            using (DirectoryEntry de = new DirectoryEntry(adPath))
            {
                using (DirectorySearcher search = new DirectorySearcher())
                {
                    search.SearchRoot = de;
                    search.Filter = "objectClass=user";
                    search.SearchScope = SearchScope.Subtree;
                    SearchResultCollection results = search.FindAll();
                    Console.WriteLine(results.Count);
                    string userName;
                    string xueHao;
                    int dtCount = dt.Rows.Count;
                    DataRow dr;
                        int i = 0;
                    foreach (SearchResult sr in results)
                    {
                        userName = sr.Properties["displayName"][0].ToString();
                        xueHao = sr.Properties["sAMAccountName"][0].ToString();
                        if (sr.Properties.Contains("mail") && sr.Properties["mail"].Count > 0)
                        {
                            dr = dt.NewRow();
                            dr["blog"] = siteUrl + "/personal/" + xueHao + "/Blog";
                            dr["xueHao"] = xueHao;
                            dr["mail"] = sr.Properties["mail"][0].ToString();
                            dt.Rows.Add(dr);
                            i = i + 1;

                        }
                        else
                        {
                            Console.WriteLine(xueHao);
                        }

                    }
                    Console.WriteLine(i);
                    dt.AcceptChanges();
                }
            }
        }
        private static string GetClassEntry(string adPath, string className)
        {
            using (DirectoryEntry de = new DirectoryEntry(adPath))
            {
                using (DirectorySearcher search = new DirectorySearcher())
                {
                    search.SearchRoot = de;
                    search.Filter = "(&(objectClass=organizationalUnit)(OU=" + className + "))";
                    search.SearchScope = SearchScope.Subtree;
                    SearchResult results = search.FindOne();
                    if (results == null)
                        return "";
                    else
                        return (results.Path);
                }
            }
        }
        private static string[] getBanJi(string bjLists)
        {
            bjLists = bjLists.Replace("；", ";");
            bjLists = bjLists.Replace(" ", "");
            if (bjLists.EndsWith(";"))
                bjLists = bjLists.Substring(0, bjLists.Length - 1);
            string[] bjs = bjLists.Split(';');
            return bjs;
        }
        /// <summary>
        /// 以指定的邮箱向多个用户发送邮件
        /// </summary>
        /// <param name="fromEmail">发送邮件的源</param>
        /// <param name="fromDisplayName">显示名称</param>
        /// <param name="pwd">发送源的邮箱密码</param>
        /// <param name="toMail">发送的目标邮箱</param>
        /// <param name="toSubject">发送的主题</param>
        /// <param name="toBody">发送的内容</param>
        /// <returns></returns>
        public  static bool SendMail(string fromEmail, string fromDisplayName, string pwd, string[] toMail, string toSubject, string toBody)
        {
            ////设置发件人信箱,及显示名字
            MailAddress from = new MailAddress(fromEmail, fromDisplayName);
            //设置收件人信箱,及显示名字 
            //MailAddress to = new MailAddress(TextBox1.Text, "");


            //创建一个MailMessage对象
            MailMessage oMail = new MailMessage();

            oMail.From = from;
            for (int i = 0; i < toMail.Length; i++)
            {
                oMail.To.Add(toMail[i].ToString());
            }


            oMail.Subject = toSubject; //邮件标题 
            oMail.Body = toBody; //邮件内容

            oMail.IsBodyHtml = true; //指定邮件格式,支持HTML格式 
            oMail.BodyEncoding = System.Text.Encoding.GetEncoding("GB2312");//邮件采用的编码 
            //oMail.Priority = MailPriority.High;//设置邮件的优先级为高
            //Attachment oAttach = new Attachment("");//上传附件
            //oMail.Attachments.Add(oAttach); 

            //发送邮件服务器 +
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.neu.edu.cn";// fromEmail.Substring(fromEmail.IndexOf("@") + 1); //163.com指定邮件服务器smtp.sina.com
            client.Credentials = new NetworkCredential(fromEmail, pwd);//指定服务器邮件,及密码

            //发送
            try
            {
                client.Send(oMail); //发送邮件
                oMail.Dispose(); //释放资源
                return true;// "恭喜你！邮件发送成功。";
            }
            catch
            {
                oMail.Dispose(); //释放资源
                return false;// "邮件发送失败，检查网络及信箱是否可用。" + e.Message;
            }


        }
//        using System.Diagnostics;
//using System.ComponentModel;

//namespace UEVTool
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
            
//           Process unlvAccProcess = new Process();

//           unlvAccProcess.StartInfo.FileName = "accuracy.exe";
//           unlvAccProcess.StartInfo.Arguments = "std.txt" + " result.txt" + " reprot.txt";
//           unlvAccProcess.StartInfo.UseShellExecute = false;
//           unlvAccProcess.StartInfo.RedirectStandardOutput = true;
//           unlvAccProcess.Start();

//}
//    }
//}
        #endregion
    }
}
