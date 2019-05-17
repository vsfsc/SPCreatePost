using System;
using System.Xml;
using Microsoft.SharePoint;
using System.Web.UI;
using Microsoft.SharePoint.WebControls;
using System.DirectoryServices;

namespace SPCreatePost.Layouts.SPCreatePost
{
    public partial class New1 : LayoutsPageBase
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
            
        }
        protected void btnLogin_ServerClick(object sender, EventArgs e)
        {
            //userid.Value = "run ik";
            //CheckUserPassword("ccc", userid.Value, password.Value);
            string domainName =error.Attributes["Title"];
            bool checkResult = CheckUserPassword(domainName, userid.Value, password.Value); 
            if (checkResult)
            {
                //error.InnerText =password.Value;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script defer>UserValidate('" + userid.Value + "','" + password.Value + "');</script>");
                //ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "", "<script type='text/javascript'>UserValidate('" + userid.Value + "','" + password.Value + "');</script>", true);
            }
            else
                error.InnerText ="用户名或密码错误";

            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script defer>showResult();</script>");
            //else
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script defer>alert('用户名或密码错误');</script>");
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script defer>alert('保存成功');if (window.opener.document.getElementById('btnSearch')!=null) window.opener.document.getElementById('btnSearch').click();window.close()</script>");
        }
        private bool CheckUserPassword(string domainName, string userName, string pwd)
        {
            try
            {
                string strLDAP = "LDAP://" + domainName;
                //string fullLoginName = loginName;
                using (DirectoryEntry objDE = new DirectoryEntry("", userName, pwd))
                {
                    DirectorySearcher deSearcher = new DirectorySearcher(objDE);
                    deSearcher.Filter = "(&(objectClass=user)(sAMAccountName=" + userName + "))";
                    DirectoryEntry usr = deSearcher.FindOne().GetDirectoryEntry();

                }
                //userid.Value = "success";
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script defer> document.getElementById('<%=userid.ClientID%>'.value='dddd' );</script>");

                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "new1", "<script>alert('dd');</script>", true);

                return true;
            }
            catch
            {
                //userid.Value = "用户名或密码错误";
                //ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "updateScript", "<script defer>alert('用户名或密码错误');</script>", true);              
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script defer>alert('用户名或密码错误');</script>");
                return false;
            }


        }

    }
}
