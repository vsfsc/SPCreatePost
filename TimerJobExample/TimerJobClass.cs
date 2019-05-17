using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Utilities;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace TimerJobExample
{
    public class TimerJobClass : SPJobDefinition  
    {
        public TimerJobClass() : base() { }

        public TimerJobClass(string jobName, SPWebApplication webApp) : base(jobName, webApp, null, SPJobLockType.Job) { this.Title = jobName; }
        public override void Execute(Guid targetInstanceId)
        {
            base.Execute(targetInstanceId);
            List<string> emailTo = new List<string>();
            SPWebApplication webApp = this.Parent as SPWebApplication;
            SPContentDatabase contentDB = webApp.ContentDatabases[targetInstanceId];
            if (contentDB == null)
                contentDB = webApp.ContentDatabases[0];
            SPWeb web = contentDB.Sites[0].RootWeb;


            // 取参数，通过Properties可以从外部传递参数，但要求参数可以被序列化和反列化
            string siteUrl = this.Properties["SiteUrl"].ToString();
            //上面的内容没有用到

            string disPlayName =ConfigurationManager.AppSettings["emailDisplayName"];
            string title = ConfigurationManager.AppSettings["emailTitle"];
            string body = ConfigurationManager.AppSettings["emailBody"];
            string showUrl = ConfigurationManager.AppSettings["retUrl"];
            int delayDays = int.Parse(System.Configuration.ConfigurationManager.AppSettings["delayDays"]);
            SPList oList = web.Lists.TryGetList("讨论列表");
            SPQuery oQuery = new SPQuery();
            string userName = "系统帐户";
            emailTo.Add("962204414@qq.com");
            oQuery.Query = "<Where><And><Eq><FieldRef Name='Author' /><Value Type='User'>" + userName + "</Value></Eq><Geq><FieldRef Name='Created'/><Value Type='DateTime'>" + DateTime.Today.AddDays(-delayDays).ToString("yyyy-MM-dd") + "</Value></Geq></And></Where>";
            SPListItemCollection lstItems = oList.GetItems(oQuery);
            if (lstItems.Count == 0)
            {
                //实现自己业务逻辑，找出复合条件的并发mail做相关通知。
                bool sendOk = SendMail("training@mail.neu.edu.cn", disPlayName, "110004cc", emailTo.ToArray(), title, body);

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
