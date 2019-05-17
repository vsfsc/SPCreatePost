using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Net.Mail;
using System.Net;
 using System.Configuration;
using System.Data;
using System.IO;

namespace SendEmailOfInnershare
{
    class Program
    {
        static void Main(string[] args)
        {

        }
        static string siteUrl;
        static ClientContext clientContext;

        private static void  GetNameList()
        {
            string userList = System.Configuration.ConfigurationManager.AppSettings["nameList"]; 
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
    }
}
