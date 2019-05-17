using System;
using System.Web;
using System.Web.UI.WebControls;
using System.DirectoryServices;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SPCreatePost.Layouts.SPCreatePost
{
    public partial class ChangePassword : LayoutsPageBase
    {
     

        protected void Page_Load(object sender, EventArgs e)
        {
            if (SPContext.Current.Site.OpenWeb().CurrentUser == null) return;
            btOK.Click += btOK_Click;
            lblTitle.Text = SPContext.Current.Site.OpenWeb().CurrentUser.Name + " 请修改密码：";
            tboldPass.Focus();
        }

        void btOK_Click(object sender, EventArgs e)
        {
            onClick();
        }
        void onClick()
        {
            string loginName = SPContext.Current.Site.OpenWeb().CurrentUser.LoginName;
            loginName = loginName.Substring(loginName.IndexOf("|") + 1);
            string login = loginName.Substring(loginName.IndexOf("\\") + 1).ToLower();
            if (login == "system")
            {
                lblInfo.Text = "您无权修改密码";
                return;
            }
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {

                lblInfo.Text = ChangeADUserPassword(loginName, tboldPass.Text, tbnewPass.Text);
            });

        }
        public string ChangeADUserPassword(string loginName, string oldPass, string newPass)
        // 用法：ChangeADUserPassword("HQ", "zhangyan", "12345", "23456")
        {
            try
            {
                string strLDAP = "LDAP://" + loginName.Substring(0, loginName.IndexOf("\\"));
                //string fullLoginName = loginName;
                using (DirectoryEntry objDE = new DirectoryEntry("", loginName, oldPass))
                {
                    DirectorySearcher deSearcher = new DirectorySearcher(objDE);
                    deSearcher.Filter = "(&(objectClass=user)(sAMAccountName=" + loginName.Substring(loginName.IndexOf("\\") + 1) + "))";
                    DirectoryEntry usr = deSearcher.FindOne().GetDirectoryEntry();
                    usr.Invoke("ChangePassword", new Object[2] { oldPass, newPass });
                    usr.CommitChanges();
                    //SPWeb spWeb = SPControl.GetContextWeb(Context);
                    //SPUtility.EnsureAuthentication();
                    string r = HttpUtility.UrlDecode(Page.ClientQueryString);
                    r = r.Substring(r.LastIndexOf("=/") + 1);

                    SPUtility.Redirect( r, SPRedirectFlags.Default, Context);
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script defer>reLogin('" + loginName.Replace("\\","\\\\")  + "','" + tbnewPass.Text  + "')</script>");

                }
                return ("更改域用户密码，操作成功！");
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("调用的目标发生了异常"))
                    return "更改密码失败";
                else if (ex.Message.StartsWith("登录失败: 未知的用户名或错误密码"))
                    return "旧密码输入有误";

                else

                    return ("更改密码失败");//,错误信息:" + ex.Message);
            }

        }
    }
}
