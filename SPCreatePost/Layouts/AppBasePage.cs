using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;

namespace SPCreatePost.Layouts
{
    public class AppBasePage : System.Web.UI.Page
    {
        #region 定义
        SPWeb m_spWeb;
        SPUser m_spUser;
        #endregion
        #region "属性"
        //登录用户id
        public long LoginID
        {
            get
            {
                if (ViewState["LoginID"] == null)
                {
                    long loginID = 0;
                    try
                    {
                        DataSet dsLogin = LoginUser;
                        if (dsLogin != null && dsLogin.Tables[0].Rows.Count > 0)
                            loginID = (long)dsLogin.Tables[0].Rows[0]["UserID"];
                    }
                    catch
                    { }
                    ViewState["LoginID"] = loginID;
                }
                return (long)ViewState["LoginID"];
            }
        }
        //登录用户角色ID,多角色用户
        public List<int> LoginRole
        {
            get
            {
                if (ViewState["LoginRole"] == null)
                {
                    List<int> roleID = new List<int>();
                    try
                    {
                        //DataSet dsLogin = LoginUser;
                        //if (dsLogin != null && dsLogin.Tables[0].Rows.Count > 0 && dsLogin.Tables[0].Rows[0]["RoleID"] != null)
                        //    roleID.Add((int)dsLogin.Tables[0].Rows[0]["RoleID"]);
                        DataSet dsRole = null;// DAL.User.GetUserRoleByUserID(LoginID);
                        foreach (DataRow dr in dsRole.Tables[0].Rows)
                        {
                            roleID.Add((int)dr["RoleID"]);
                        }
                    }
                    catch
                    { }
                    ViewState["LoginRole"] = roleID;
                }
                return (List<int>)ViewState["LoginRole"];
            }
        }
        private DataSet LoginUser
        {
            get
            {
                if (ViewState["LoginUser"] == null)
                {
                    DataSet dsLogin = null;
                    try
                    {
                        //string  login=SPUser.LoginName.Substring(SPUser.LoginName.IndexOf("\\") + 1);
                        dsLogin = null;// DAL.User.GetUserByAccount(LoginAccount);
                    }
                    catch
                    { }
                    ViewState["LoginUser"] = dsLogin;
                }
                return (DataSet)ViewState["LoginUser"];
            }
        }
        /// <summary>
        /// 登录帐号
        /// </summary>
        public string LoginAccount
        {
            get
            {
                if (ViewState["loginAccount"] == null)
                {
                    string logName = "";
                    if (SPUser != null)
                    {
                        logName = SPUser.LoginName.Substring(SPUser.LoginName.IndexOf("\\") + 1);
                    }
                    ViewState["loginAccount"] = logName;
                }
                return (string)ViewState["loginAccount"];
            }
        }
        #endregion
        #region 属性
        /// <summary>
        /// 当前网站的比赛ID
        /// </summary>
        public long ContestID
        {
            get
            {
                //if (ViewState["ContestID"] == null)
                //{
                //    ViewState["ContestID"] = System.Configuration.ConfigurationManager.AppSettings["contestID"];
                //}
                return Convert.ToInt64(ViewState["ContestID"]);
            }

        }
        /// <summary>
        /// Gets the current <c>SPWeb</c>.
        /// </summary>
        public SPWeb SPWeb
        {
            get
            {
                if (m_spWeb == null)
                {
                    string subWebName = "";
                    try
                    {
                        subWebName = System.Configuration.ConfigurationManager.AppSettings["webName"];
                    }
                    catch
                    {
                    }
                    if (string.IsNullOrEmpty(subWebName))
                        try
                        {
                            m_spWeb = SPContext.Current.Web;// SPControl.GetContextWeb(HttpContext.Current);
                        }
                        catch
                        {
                            m_spWeb = null;
                        }
                    else
                    {

                        try
                        {
                            m_spWeb = SPContext.Current.Site.AllWebs[subWebName];
                            string url = m_spWeb.Url;
                        }
                        catch
                        {
                            m_spWeb = SPControl.GetContextWeb(HttpContext.Current);
                        }
                    }
                }
                return m_spWeb;
            }
        }
        public SPUser SPUser
        {
            get
            {
                if (m_spUser == null)
                    m_spUser = SPWeb.CurrentUser;
                return m_spUser;

            }
        }
        /// <summary>
        /// 当前用户是否当前网站管理员
        /// </summary>
        /// <returns></returns>
        public bool IsWebAdmin
        {
            get
            {
                bool right = SPWeb.DoesUserHavePermissions(SPBasePermissions.FullMask);
                return right;
            }
        }
        #endregion
        #region "方法"
        public bool ShowMessage(UpdatePanel UpdatePanel1, string msg)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "updateScript", "alert('" + msg + "')", true);
            return true;
        }
        public void OpenWindow(UpdatePanel UpdatePanel1, string fileName)
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "openScript", "window.open('" + fileName + "','_blank')", true);
            //page.ClientScript.RegisterStartupScript(page.GetType(), "open", "<script defer>window.showModalDialog('" + fileName + "','_blank','dialogWidth=1002px;dialogHeight=600px');</script>");
        }
        #endregion
    }
}
