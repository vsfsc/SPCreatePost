using System;
using System.Net;
using System.Data;
using System.IO;
using Microsoft.SharePoint.Client;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.Configuration;
using System.Collections;

namespace SendEmailAlert
{
    class Program
    {
        static void Main(string[] args)
        {
            //bool sendOk = SendMail("training@mail.neu.edu.cn", "training", "110004cc", new string[] { "962204414@qq.com" }, "title", "body " + "<br>" + "showUrl");
            bool sendOk = SendMail("smartneu@mail.neu.edu.cn", "SmartNEU", "ccc.neu.edu.cn", new string[] { "962204414@qq.com" }, "title", "body " + "<br>" + "showUrl");
            if (sendOk)
                            Console.Write("ik");
                        else
                            Console.Write("no");
            //string adPath = "LDAP://OU=辽宁工程技术大学,OU=iSmart,DC=ccc,DC=neu,DC=edu,DC=cn";//OU=宁夏理工,DC=CCC,DC=NEU,DC=EDU,DC=CN";
            //string adPath = ConfigurationManager.AppSettings["adPath"];
            //siteUrl = ConfigurationManager.AppSettings["siteUrl"];
            //SearchResultCollection dt = GetStudentByClass(adPath);
            //CheckDiscussionsList(dt);

        }
        static string siteUrl;
        static ClientContext clientContext;
        private static void CheckDiscussionsList(SearchResultCollection results)
        {
            StreamWriter write = new StreamWriter("c:\\sendEmail.txt", false);
            string disPlayName = System.Configuration.ConfigurationManager.AppSettings["emailDisplayName"];
            string title = System.Configuration.ConfigurationManager.AppSettings["emailTitle"];
            string body = System.Configuration.ConfigurationManager.AppSettings["emailBody"];
            string showUrl = System.Configuration.ConfigurationManager.AppSettings["retUrl"];
            int delayDays = int.Parse(System.Configuration.ConfigurationManager.AppSettings["delayDays"]);
            CamlQuery oQuery;
            clientContext = new ClientContext(siteUrl);
            Web web = clientContext.Web;
            List oList = web.Lists.GetByTitle("讨论列表");
            string userName, xueHao, mail;
            
            List<string>  emailTo=new List<string>() ;
            foreach (SearchResult sr in results)
            {
                if (sr.Properties.Contains("mail") && sr.Properties["mail"].Count > 0)
                {
                    mail = sr.Properties["mail"][0].ToString();
                }
                else
                    continue;
                userName = sr.Properties["displayName"][0].ToString();
                xueHao = sr.Properties["sAMAccountName"][0].ToString();

                oQuery = new CamlQuery();
                oQuery.ViewXml = "<View><Query><Where><And><Eq><FieldRef Name='Author' /><Value Type='User'>" + userName + "</Value></Eq><Geq><FieldRef Name='Created'/><Value Type='DateTime'>" + DateTime.Today.AddDays(-delayDays).ToString("yyyy-MM-dd") + "</Value></Geq></And></Where></Query></View>";
                ListItemCollection lstItems = oList.GetItems(oQuery);
                clientContext.Load(lstItems, items => items.Include(item => item["Title"], item => item.Id));
                try
                {
                    clientContext.ExecuteQuery();
                    if (lstItems.Count == 0)
                    {
                        emailTo.Add(mail);
                    }
                    else
                    {
                        write.WriteLine(DateTime.Now.ToString() + "   " + xueHao + " 有活动");
                    }
                }
                catch
                {
                    write.WriteLine(DateTime.Now.ToString() + "   " + siteUrl + " 不存在");
                }
            }
            bool sendOk = SendMail("training@mail.neu.edu.cn", disPlayName, "110004cc", emailTo.ToArray() , title, body + "<br>" + showUrl);
            if (sendOk)
                write.WriteLine(DateTime.Now.ToString() + "   " +   " email success");
            else
                write.WriteLine(DateTime.Now.ToString() + "   "   + " email failed");

            write.Close();
            write.Dispose();
        }
        private static SearchResultCollection GetStudentByClass(string adPath)
        {
            using (DirectoryEntry de = new DirectoryEntry(adPath))
            {
                using (DirectorySearcher search = new DirectorySearcher())
                {
                    search.SearchRoot = de;
                    search.Filter = "objectClass=user";
                    search.SearchScope = SearchScope.Subtree;
                    SearchResultCollection results = search.FindAll();
                    return results;
                }
            }
        }
           public static bool SendMail(string fromEmail, string fromDisplayName, string pwd, string[] toMail, string toSubject, string toBody)
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
            client.Port = 25;
            //发送
            try
            {
                client.Send(oMail); //发送邮件
                oMail.Dispose(); //释放资源
                return true;// "恭喜你！邮件发送成功。";
            }
            catch (Exception ex)
            {
                oMail.Dispose(); //释放资源
                return false;// "邮件发送失败，检查网络及信箱是否可用。" + e.Message;
            }


        }
    }
}
