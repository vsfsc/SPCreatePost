using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.Office.Server.Social;
using Microsoft.Office.Server.UserProfiles;
using System.Data;
using System.Reflection;

namespace PersonalStastics.PersonalStastics
{
    [ToolboxItemAttribute(false)]
    public class PersonalStastics : WebPart, IWebPartTable
    {
        DataTable _table;
        protected override void CreateChildControls()
        {
            //bool isExists = PersonalSiteExits();
            //if (!isExists) return;
            int newsCount = NewsCount;
            int[] followedCount = GetFollowedCount();
            int followedSites = followedCount[0];
            int followedDocs = followedCount[1];
            int blogsCount = BlogsCount;
            this.Controls.Add(new LiteralControl("发布的微博：" + newsCount.ToString() + "<br>"));
            this.Controls.Add(new LiteralControl("发布的博客：" + blogsCount.ToString() + "<br>"));
            this.Controls.Add(new LiteralControl("关注的网站：" + followedSites.ToString() + "<br>"));
            this.Controls.Add(new LiteralControl("关注的文档：" + followedDocs.ToString() + "<br>"));
        }
        #region 使用的方法
        private string GetAccountName()
        {
            string loginName;
            if (Page.Request.QueryString["accountname"] == null)
            {
                loginName = this.Context.User.Identity.Name;// loginUser.LoginName;
                loginName = loginName.Substring(loginName.IndexOf("|") + 1).ToLower();
            }
            else
                loginName = Page.Request.QueryString["accountname"];
            return loginName;
        }
        private void SaveData(int newsCount, int[] followedCount, int blogsCount)
        {
            string siteUrl = SPContext.Current.Site.Url;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite mySite = new SPSite(siteUrl))
                {
                    using (SPWeb myWeb = mySite.OpenWeb(SPContext.Current.Web.ID ) )
                    {
                        myWeb.AllowUnsafeUpdates = true;
                        SPList mylist = myWeb.Lists["个人数据统计"];
                        SPListItem item = mylist.Items[0]; 
                        item["数量"] = newsCount;
                        item.Update();
                        item = mylist.Items[1];
                        item["数量"] = followedCount[0];
                        item.Update();
                        item = mylist.Items[2];
                        item["数量"] = followedCount[1];
                        item.Update();
                        item = mylist.Items[3];
                        item["数量"] = blogsCount; 
                        item.Update();
                        myWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        //获取当前用户发布的新闻源
        private int GetPublishdNews()
        {
            SPSocialFeedManager feedManager = new SPSocialFeedManager();
            SPSocialFeedOptions socialOptions = new SPSocialFeedOptions();
            socialOptions.MaxThreadCount = int.MaxValue;
            int i = 0;
            try
            {
                string acountName = GetAccountName();
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPSocialFeed feed = feedManager.GetFeedFor(acountName, socialOptions);//.GetFeed(SPSocialFeedType.Personal, socialOptions);
                    SPSocialThread[] threads = feed.Threads;
                    i = threads.Length;
                    foreach (SPSocialThread thread in threads)
                    {
                        if (thread.Attributes.ToString() != "None")
                        {
                            i = i + 1;
                        }
                        else
                        {
                            //this.Controls.Add(new  LiteralControl( thread.RootPost.Text) );
                        }
                    }
                });
            }
            catch
            {

            }
            return i;
        }
        //当前用户正在关注的文档和网站
        private int[] GetFollowedCount()
        {
            int followedSiteCount = 0;
            int followedDocCount = 0;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                   {
                       SPServiceContext serviceContext = SPServiceContext.GetContext(site);
                       UserProfileManager upm = new UserProfileManager(serviceContext);
                       string accountName = GetAccountName();
                       UserProfile u = upm.GetUserProfile(accountName);
                       
                       SPSocialFollowingManager followingManager = new SPSocialFollowingManager(u, serviceContext);
                       followedSiteCount = followingManager.GetFollowedCount(SPSocialActorTypes.Sites);
                       followedDocCount = followingManager.GetFollowedCount(SPSocialActorTypes.Documents);
                   }
               });
            }
            catch
            {

            }
            return new int[] { followedSiteCount, followedDocCount };
        }
        private bool PersonalSiteExits()
        {
            string loginName = GetAccountName();
            string login = loginName.Substring(loginName.IndexOf("\\") + 1).ToLower();
            string url = SPContext.Current.Site.Url;
            string siteUrl = url.Substring(0, url.IndexOf("/", 7)) + "/personal/" + login;
            try
            {
                SPSite site = new SPSite(siteUrl);
                return true;
            }
            catch
            {

            }
            return false;
        }
        //获取当前用户的博客数量
        private int GetAllBlogs()
        {
            string loginName = GetAccountName();
            string login = loginName.Substring(loginName.IndexOf("\\") + 1).ToLower();
            string url = SPContext.Current.Site.Url;
            int i = 0;
            try
            {
                string siteUrl = url.Substring(0, url.IndexOf("/", 7)) + "/personal/" + login;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite mySite = new SPSite(siteUrl))
                    {
                        SPWeb myWeb = mySite.AllWebs["Blog"];
                        SPList myBlog = myWeb.Lists["Posts"];
                        i = myBlog.ItemCount;
                    }
                });

            }
            catch
            {

            }
            return i;
        }
        private void ChartTableData()
        {
            _table = new DataTable();

            DataColumn col = new DataColumn();
            col.DataType = typeof(string);
            col.ColumnName = "Title";
            _table.Columns.Add(col);

            col = new DataColumn();
            col.DataType = typeof(int);
            col.ColumnName = "Value";
            _table.Columns.Add(col);
            int newsCount = NewsCount;
            int[] followedCount = GetFollowedCount();
            int followedSites = followedCount[0];
            int followedDocs = followedCount[1];
            int blogsCount = BlogsCount;

            DataRow row = _table.NewRow();
            row["Title"] = "发布微博";
            row["Value"] = newsCount;
            _table.Rows.Add(row);
            row = _table.NewRow();
            row["Title"] = "发布博客";
            row["Value"] = blogsCount;
            _table.Rows.Add(row);
            row = _table.NewRow();
            row["Title"] = "关注网站";
            row["Value"] = followedSites;
            _table.Rows.Add(row);

            row = _table.NewRow();
            row["Title"] = "关注文档";
            row["Value"] = followedDocs;
            _table.Rows.Add(row);

        }
        #endregion
        #region 属性
        public int FollowedCount
        {
            get
            {
                if (ViewState["followedCount"] == null)
                {
                    ViewState["followedCount"] = GetFollowedCount();
                }
                return (int)ViewState["followedCount"];
            }
        }
        public int BlogsCount
        {
            get
            {
                if (ViewState["blogsCount"] == null)
                {
                    ViewState["blogsCount"] = GetAllBlogs();
                }
                return (int)ViewState["blogsCount"];
            }
        }
        public int NewsCount
        {
            get
            {
                if (ViewState["newsCount"]==null )
                {
                    ViewState["newsCount"] = GetPublishdNews();
                }
                return (int)ViewState["newsCount"];
            }
           
        }
        #endregion
        #region 传值
        public PropertyDescriptorCollection Schema
        {
            get
            {
                return TypeDescriptor.GetProperties(_table.DefaultView[0]);
            }
        }


        public void GetTableData(TableCallback callback)
        {
            ChartTableData();
            callback(_table.Rows);
        }

        public bool ConnectionPointEnabled
        {
            get
            {
                object o = ViewState["ConnectionPointEnabled"];
                return (o != null) ? (bool)o : true;
            }
            set
            {
                ViewState["ConnectionPointEnabled"] = value;
            }
        }

        [ConnectionProvider("Table", typeof(TableProviderConnectionPoint),
      AllowsMultipleConnections = true)]
        public IWebPartTable GetConnectionInterface()
        {
            return this;// new PersonalStastics();
        }

        public class TableProviderConnectionPoint : ProviderConnectionPoint
        {
            public TableProviderConnectionPoint(MethodInfo callbackMethod,
        Type interfaceType, Type controlType, string name, string id,
        bool allowsMultipleConnections)
                : base(callbackMethod, interfaceType, controlType, name, id,
                  allowsMultipleConnections)
            {
            }

            public override bool GetEnabled(Control control)
            {
                return ((PersonalStastics)control).ConnectionPointEnabled;
            }

        }
        #endregion
    }

}
