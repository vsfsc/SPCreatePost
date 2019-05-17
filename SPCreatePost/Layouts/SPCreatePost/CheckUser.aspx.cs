using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.DirectoryServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Net;
using System.DirectoryServices.Protocols;

namespace SPCreatePost.Layouts.SPCreatePost
{
    public partial class CheckUser : LayoutsPageBase
    {
        protected UpdatePanel UpdatePanel1;
        protected HtmlInputText userid;
        protected HtmlInputText password;
        protected override bool AllowAnonymousAccess
        {
            get
            {
                return true;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_ServerClick(object sender, EventArgs e)
        {
            //userid.Value = "run ik";
            CheckUserPassword("ccc", userid.Value, password.Value); 

            //bool checkResult = CheckUserPassword("ccc",userid.Value, password.Value); 
            //if (checkResult)
            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script defer>showResult();</script>");
            //else
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script defer>alert('用户名或密码错误');</script>");
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script defer>alert('保存成功');if (window.opener.document.getElementById('btnSearch')!=null) window.opener.document.getElementById('btnSearch').click();window.close()</script>");

        }
        public bool CheckADUser(string username, string userpass)
        {
            bool flag = false;
            NetworkCredential nc = new NetworkCredential(username, userpass, "ccc");
            LdapConnection lc = new LdapConnection(new LdapDirectoryIdentifier("127.0.0.1"));
            try
            {
                lc.Bind(nc);
                flag = true;
            }
            catch (LdapException ex)
            {
                flag = false;
            }
            finally
            {
                lc.Dispose();
            }

            return flag;
        }
        private bool CheckUserPassword(string domainName, string userName, string pwd)
        {
            try
            {
                //string strLDAP = "LDAP://" + domainName;
                //string fullLoginName = loginName;
                using (DirectoryEntry objDE = new DirectoryEntry("", userName, pwd))
                {
                    DirectorySearcher deSearcher = new DirectorySearcher(objDE);
                    deSearcher.Filter = "(&(objectClass=user)(sAMAccountName=" + userName + "))";
                    DirectoryEntry usr = deSearcher.FindOne().GetDirectoryEntry();

                }
                userid.Value = "success";
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script defer>showResult();</script>");

                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "updateScript", "<script defer>alert('dd');</script>", true);

                return true;
            }
            catch
            {
                userid.Value = "用户名或密码错误";
                //ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "updateScript", "<script defer>alert('用户名或密码错误');</script>", true);              
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script defer>alert('用户名或密码错误');</script>");
                return false;
            }
       

        }


    }
}
