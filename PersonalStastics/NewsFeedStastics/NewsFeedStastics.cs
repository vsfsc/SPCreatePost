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
using Microsoft.SharePoint.Utilities;
using System.Data;

namespace PersonalStastics.NewsFeedStastics
{
    [ToolboxItemAttribute(false)]
    public class NewsFeedStastics : WebPart
    {
        #region 事件
        protected override void CreateChildControls()
        {
            //SPWeb web = SPContext.Current.Web;
            //SPTimeZone timeZone = web.RegionalSettings.TimeZone;
            //DateTime currentDate = DateTime.Now;
            //DateTime date = timeZone.LocalTimeToUTC(currentDate );

           //string dt= SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Parse(currentDate.ToString ()));

            //this.Controls.Add(new LiteralControl(currentDate.ToString() + "<br>"));
            //this.Controls.Add(new LiteralControl(date.ToString() + "<br>"));
            //this.Controls.Add(new LiteralControl(dt + "<br>"));
            //return;
            //if (SPContext.Current.Web.CurrentUser == null) return;
            //StatisticList(  SPContext.Current.Web.CurrentUser);

            //int[] newsCount = NewsCount;
            //int blogsCount = BlogsCount;
            //this.Controls.Add(new LiteralControl("个人总数：" + newsCount[0].ToString() + "<br>"));
            //this.Controls.Add(new LiteralControl("团队总数：" + newsCount[1].ToString() + "<br>"));
            //this.Controls.Add(new LiteralControl("当日更新：" + newsCount[2].ToString() + "<br>"));
            //this.Controls.Add(new LiteralControl("本周总数：" + newsCount[3].ToString() + "<br>"));
            //this.Controls.Add(new LiteralControl("发布的博客：" + blogsCount.ToString() + "<br>"));

            int[] result = StatisticAllList(ListName, SubWebUrl);
            this.Controls.Add(new LiteralControl("Newsfeed：" + result[0].ToString() + "<br>"));

            string[] lstName = ListName.Split(';');
            for (int i = 0; i < lstName.Length; i++)
                this.Controls.Add(new LiteralControl(lstName[i] + result[i + 1].ToString() + "<br>"));

            

        }
        #endregion
        #region 属性
        public int[] NewsCount
        {
            get
            {
                if (ViewState["newsCount"] == null)
                {
                    ViewState["newsCount"] = GetPublishdNews();
                }
                return (int[])ViewState["newsCount"];
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
        #endregion
        #region 方法
        private int  GetTotalNewsFeeds()
        {
            SPSocialFeedOptions socialOptions = new SPSocialFeedOptions();
            socialOptions.MaxThreadCount = int.MaxValue;
            int i = 0;
            
            try
            {
                string acountName = GetAccountName();
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.AllWebs[SPContext.Current.Web.ID])
                        {
                            SPServiceContext serviceContext = SPServiceContext.GetContext(site);
                            UserProfileManager upm = new UserProfileManager(serviceContext);
                            string accountName = GetAccountName();
                            UserProfile u = upm.GetUserProfile(accountName);
                            SPSocialFeedManager feedManager = new SPSocialFeedManager(u, serviceContext);
                            SPSocialFeed feed = feedManager.GetFeedFor(web.Url, socialOptions);
                            SPSocialThread[] threads = feed.Threads;
                            foreach (SPSocialThread thread in threads)
                            {
                                if (thread.Attributes.ToString() != "None")
                                {
                                    string actorAccount;
                                    if (thread.Actors.Length == 2)
                                        actorAccount = thread.Actors[1].AccountName;
                                    else
                                        actorAccount = thread.Actors[0].AccountName;
                                    if (actorAccount.ToLower() == accountName.ToLower())
                                        i = i + 1;
                                }
                            }
                           
                            EnumerateNewsfeeds (ref i,web,feedManager,socialOptions,accountName  );
                        }
                    }
                });
            }
            catch
            {

            }
            return i;
        }
        /// <summary>
        /// 遍历当前网站下面子网站的新闻源
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="pWeb"></param>
        private void EnumerateNewsfeeds(ref int totalCount, SPWeb pWeb, SPSocialFeedManager feedManager, SPSocialFeedOptions socialOptions, string accountName)
        {
            foreach (SPWeb subWeb in pWeb.Webs)
            {
                SPSocialFeed feed = feedManager.GetFeedFor(subWeb.Url, socialOptions);
                SPSocialThread[] threads = feed.Threads;
                int i = 0;
                foreach (SPSocialThread thread in threads)
                {
                    if (thread.Attributes.ToString() != "None")
                    {
                        string actorAccount;
                        if (thread.Actors.Length == 2)
                            actorAccount = thread.Actors[1].AccountName;
                        else
                            actorAccount = thread.Actors[0].AccountName;
                        if (actorAccount.ToLower() == accountName.ToLower())
                            i = i + 1;
                    }
                }
                totalCount = totalCount + i;
                EnumerateNewsfeeds(ref totalCount, subWeb, feedManager, socialOptions, accountName);
            }

        }
        private void EnumerateList(ref int totalCount, SPWeb pWeb, string mList, string accountName)
        {
            SPList sList;
            SPQuery oQuery;

            foreach (SPWeb subWeb in pWeb.Webs)
            {
                sList = subWeb.Lists.TryGetList(mList);


                oQuery = new SPQuery();
                oQuery.ViewAttributes = "Scope='RecursiveAll'";
                oQuery.Query = "<Where><Eq><FieldRef Name='Author'/><Value Type='Text'>" + accountName + "</Value></Eq></Where>";
                try
                {
                    SPListItemCollection lstItems = sList.GetItems(oQuery);
                    totalCount = totalCount + lstItems.Count;//个人
                }
                catch
                { }
            }

        }
        /// <summary>
        /// 当前用户当前网站内容的统计，包括子网站的遍历
        /// </summary>
        /// <param name="logUser"></param>
        /// <param name="lstName"></param>
        private int[] StatisticAllList(string ListName,string subWebUrl)
        {
            SPQuery oQuery;
            SPList sList;
            SPUser logUser = SPContext.Current.Web.CurrentUser;
            string[] lstName =  ListName.Split(';');
            int[] itmCounts = new int[lstName.Length +1];
            int i = 1;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.AllWebs[SPContext.Current.Web.ID])
                    {
                        itmCounts[0] =GetTotalNewsFeeds ();//微博
                        i = 0;
                        int j = 0;
                        foreach (string mList in lstName)
                        {
                            try
                            {
                                if (mList == "Posts" && subWebUrl != "")//统计备忘录
                                {
                                    SPWeb subWeb = web.Webs[subWebUrl];
                                    sList = subWeb.Lists.TryGetList(mList);
                                }
                                else
                                {
                                    sList = web.Lists.TryGetList(mList);
                                }
                                i = i + 1;
                                oQuery = new SPQuery();
                                oQuery.ViewAttributes = "Scope='RecursiveAll'";
                                oQuery.Query = "<Where><Eq><FieldRef Name='Author'/><Value Type='Text'>" + logUser.Name + "</Value></Eq></Where>";
                                SPListItemCollection lstItems = sList.GetItems(oQuery);
                                j= lstItems.Count;//个人
                                EnumerateList(ref j, web, mList, logUser.Name);
                                itmCounts[i] =j;
                            }
                            catch
                            { }
                        }
                    }
                }
            });
            return itmCounts;
        }
        private void ShowListitemDetails( ref Table tbl, SPListItemCollection lstItems)
        {
            TableRow tr;
            TableCell tc;
            foreach (SPListItem lstItem in lstItems)
            {
                tr = new TableRow();
                tc = new TableCell();
                tc.Controls.Add(new LiteralControl (lstItem.Url ));
                tr.Cells.Add(tc);
                tc = new TableCell();
                tc.Text = lstItem.Title;
                tr.Cells.Add(tc);
                tbl.Rows.Add(tr);
            }
        }

        //获取当前用户发布的新闻源
        /// <summary>
        ///统计不同时间的用户和总的新闻源
        /// </summary>
        /// <param name="typeID">0-个人总数，1-团队总数，2-当日更新，3-本周更新</param>
        /// <returns></returns>
        private int[] GetPublishdNews( )
        {
            SPSocialFeedOptions socialOptions = new SPSocialFeedOptions();
            socialOptions.MaxThreadCount =  int.MaxValue;
            int i = 0;
            int j = 0;
            int[] totalTimes = new int[4];

            try
            {
                string acountName = GetAccountName();
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.AllWebs[SPContext.Current.Web.ID])
                        {
                            SPServiceContext serviceContext = SPServiceContext.GetContext(site);
                            UserProfileManager upm = new UserProfileManager(serviceContext);
                            string accountName = GetAccountName();
                            UserProfile u = upm.GetUserProfile(accountName);
                            SPSocialFeedManager feedManager = new SPSocialFeedManager(u, serviceContext);
                            //this.Controls.Add(new LiteralControl(accountName + "<br>"));
                            //SPSocialFeed feed = feedManager.GetFeedFor(accountName, socialOptions);//.GetFeed  
                            SPSocialFeed feed = feedManager.GetFeedFor(web.Url, socialOptions);//.GetFeed(SPSocialFeedType.Personal,  socialOptions);
                            SPSocialThread[] threads = feed.Threads;
                            foreach (SPSocialThread thread in threads)
                            {
                                if (thread.Attributes.ToString() != "None")
                                {
                                    //if (thread.RootPost.Text != null)
                                    //    this.Controls.Add(new LiteralControl("Text  " + thread.RootPost.Text + ""));
                                    //else if (thread.RootPost.Attachment != null)
                                    //    this.Controls.Add(new LiteralControl( thread.RootPost.Attachment.AttachmentKind.ToString () + "<img src='" + thread.RootPost.Attachment.Uri.ToString() + "'/><br>"));
                                    ////if (thread.RootPost.Text == "http://xqx2012/_layouts/15/studentlist/studentlist.aspx")
                                    ////    Console.WriteLine(thread.RootPost.Text );
                                    string actorAccount;
                                    if (thread.Actors.Length == 2)
                                        actorAccount = thread.Actors[1].AccountName;
                                    //this.Controls.Add(new LiteralControl("AccountName  " + thread.Actors[1].AccountName + "<br>"));
                                    else
                                        actorAccount = thread.Actors[0].AccountName;
                                    //this.Controls.Add(new LiteralControl("AccountName  " + thread.Actors[0].AccountName + "<br>"));
                                    //当前用户
                                    if (actorAccount.ToLower() == accountName.ToLower())
                                        i = i + 1;
                                    j = j + 1;
                                }
                            }
                            totalTimes[0] = i;//个人总数
                            totalTimes[1] = j;//团队总数

                            socialOptions = new SPSocialFeedOptions();
                            socialOptions.MaxThreadCount = int.MaxValue;
                            socialOptions.NewerThan = DateTime.Now.Date.AddDays(-1).AddHours(8);
                            feed = feedManager.GetFeedFor(accountName   , socialOptions);
                            threads = feed.Threads;
                            totalTimes[2] = threads.Length;//当日更新

                            socialOptions = new SPSocialFeedOptions();
                            socialOptions.MaxThreadCount = int.MaxValue;

                            socialOptions.NewerThan = DateTime.Now.Date.AddDays(-7).AddHours(8);
                            feed = feedManager.GetFeedFor(accountName , socialOptions);
                            threads = feed.Threads;
                            totalTimes[3] = threads.Length;//本周更新


                        }
                    }
                });
            }
            catch
            {

            }
            return totalTimes ;
        }
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
        //获取当前用户的博客数量
        private int GetAllBlogs()
        {
            string loginName = GetAccountName();
            string login = loginName.Substring(loginName.IndexOf("\\") + 1).ToLower();
            string url = SPContext.Current.Site.Url;
            string blogType = SPContext.Current.Web.Title;
            int i = 0;
            try
            {
                string siteUrl;
                if (url.IndexOf("/", 7) > 0)
                    siteUrl = url.Substring(0, url.IndexOf("/", 7)) + "/personal/" + login;
                else
                    siteUrl = url + "/personal/" + login;
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite mySite = new SPSite(siteUrl))
                    {
                        SPWeb myWeb = mySite.AllWebs["Blog"];
                        SPList myBlog = myWeb.Lists["Posts"];
                        //foreach (SPField myFiled in myBlog.Fields)
                        //    this.Controls.Add(new LiteralControl("Title   " + myFiled.Title + " ;   InternalNameName    " + myFiled.InternalName+"<br/>"));
                        SPQuery oQuery = new SPQuery();
                        oQuery.ViewAttributes = "Scope='RecursiveAll'";
                        oQuery.Query = "<Where><Eq><FieldRef Name='PostCategory'/><Value Type='Text'>" + blogType + "</Value></Eq></Where>";
                        SPListItemCollection lstItems = myBlog.GetItems(oQuery);

                        i = lstItems.Count;

                    }
                });

            }
            catch
            {

            }
            return i;
        }
        #endregion
        #region 统计文档库和wiki
        /// <summary>
        /// 统计文档的数量（将统计的文档当成字符串，作为参数），以及当前用户所发的量Author
        /// </summary>
        private void StatisticList(SPUser logUser)
        {
            string[] lstName = ListName.Split(';');
            
            SPQuery oQuery;
            SPList sList;
            int[] itmCounts =new int[4];
            SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        using (SPWeb web = site.AllWebs[SPContext.Current.Web.ID])
                        {
                            //设置sharepoint时间格式
                            SPTimeZone timeZone = web.RegionalSettings.TimeZone;
                            foreach (string mList in lstName)
                            {
                                try
                                {
                                    if (mList == "Posts" && SubWebUrl != "")
                                    {
                                        SPWeb subWeb = web.Webs[SubWebUrl];
                                        sList = subWeb.Lists.TryGetList(mList);
                                    }
                                    else
                                        sList = web.Lists.TryGetList(mList);
                                    oQuery = new SPQuery();
                                    oQuery.ViewAttributes = "Scope='RecursiveAll'";
                                    oQuery.Query = "<Where><Eq><FieldRef Name='Author'/><Value Type='Text'>" + logUser.Name + "</Value></Eq></Where>";
                                    SPListItemCollection lstItems = sList.GetItems(oQuery);
                                    itmCounts[0] = lstItems.Count;//个人
                                    itmCounts[1] = sList.ItemCount;//全部
                                    oQuery = new SPQuery();
                                    DateTime currentDate = DateTime.Now;
                                    DateTime qDate = currentDate.AddDays(-1);
                                    DateTime cDate = timeZone.LocalTimeToUTC(qDate);
                                    string dt = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Parse(cDate.ToString()));

                                    oQuery.ViewAttributes = "Scope='RecursiveAll'";
                                    oQuery.Query = "<Where><And><Eq><FieldRef Name='Author'/><Value Type='Text'>" + logUser.Name + "</Value></Eq><Geq><FieldRef Name='Created'/><Value Type='DateTime'>" + dt + "</Value></Geq></And></Where>";
                                    lstItems = sList.GetItems(oQuery);
                                    itmCounts[2] = lstItems.Count;//当日更新
                                   

                                    qDate = currentDate.AddDays(-7);
                                    cDate = timeZone.LocalTimeToUTC(qDate );
                                    dt = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Parse(cDate.ToString()));

                                    oQuery.ViewAttributes = "Scope='RecursiveAll'";
                                    oQuery.Query = "<Where><And><Eq><FieldRef Name='Author'/><Value Type='Text'>" + logUser.Name + "</Value></Eq><Geq><FieldRef Name='Created'/><Value Type='DateTime'>" + dt + "</Value></Geq></And></Where>";
                                    lstItems = sList.GetItems(oQuery);
                                    itmCounts[3] = lstItems.Count;//本周更新

                                    this.Controls.Add(new LiteralControl(mList + "<br/>"));
                                    this.Controls.Add(new LiteralControl("个人总数 : " + itmCounts[0].ToString() + "<br/>"));
                                    this.Controls.Add(new LiteralControl("团队总数 : " + itmCounts[1].ToString() + "<br/>"));
                                    this.Controls.Add(new LiteralControl("当日更新 : " + itmCounts[2].ToString() + "<br/>"));
                                    this.Controls.Add(new LiteralControl("本周总数 : " + itmCounts[3].ToString() + "<br/>"));
                                }
                                catch
                                { }
                            }
                        }
                    }
                }
                );
        }
        #endregion
        #region 属性
        string subWebUrl = "Blogs";
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("用来统计博客的内容，参数为空则统计当前网站下面的")]
        [WebDescription("")]
        public string SubWebUrl
        {
            get
            {
                return subWebUrl;
            }
            set
            {
                subWebUrl = value;
            }
        }
        string listName = "新闻公告;文档;网站页面;页面;讨论列表;Posts";
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("对当前网站的列表统计，各列表之间用分号隔开")]
        [WebDescription("")]
        public string ListName
        {
            get
            {
                return listName ;
            }
            set
            {
                listName = value;
            }
        }
        #endregion
    }
}
