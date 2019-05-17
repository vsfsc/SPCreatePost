using System;
using System.Net;
using Microsoft.SharePoint;
using Microsoft.Office.Server;
using Microsoft.Office.Server.Search;
using Microsoft.Office.Server.Search.Query;
using Microsoft.Office.Server.Search.Administration;
using System.Collections.Generic;
using System.Data;

using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Social;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.Office.Server.Social;
using Microsoft.SharePoint.Administration;

namespace SPCreatePost.Layouts.SPCreatePost
{
    public partial class PersonalPosts : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //txtSiteUrl.Text = SPContext.Current.Site.Url;// "http://xqx2012/personal/xueqingxia";// SPContext.Current.Site.Url;
                //txtDocUrl.Text =SPContext.Current.Site.Url+ "/personal/xueqingxia/Blog";
                //FullQuery("2015-03-19", "2015-03-19");
            }
            //Response.Write (DateTime.Now.ToString("yyyy年MM月dd日" ));
            //ReadBlog();
        }
        private void ReadBlog()
        {
            string url = "http://xqx2012/my/personal/zhaomeng/Blog/Lists/Posts/ViewPost.aspx?ID=34";
            //string url1 = url.Substring(0, url.IndexOf("/Lists/Posts")-1);

            int id = int.Parse(url.Substring(url.IndexOf("=") + 1));
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                string siteUrl = url.Substring(0, url.IndexOf("/Blog"));
                using (SPSite mySite = new SPSite(siteUrl))
                {
                    SPWeb myWeb = mySite.AllWebs["Blog"];
                    SPList myBlog = myWeb.Lists["Posts"];
                    SPListItem myBlogList = myBlog.GetItemById(id);
                    Response.Write(myBlogList.ListItems.Count );
                    Response.Write(myBlogList["Title"]);
                    Response.Write(myBlogList["Body"]);
                    string cateName = myBlogList["PostCategory"].ToString();
                    Response.Write(cateName.Substring ( cateName.IndexOf(";#")+2));
                    Response.Write("CategoryID:" + myBlogList["CategoryID"]);
                    //foreach (SPField myField in myBlog.Fields)
                    //{
                    //    Response.Write(myField.Title + "---" + myField.TypeDisplayName + "---" + myField.InternalName + "---" + " title TypeDisplayName InternalName<br>");
                    //}
                }
            });
        }
        ClientContext clientContext;
         
        SPSocialFollowingManager followingManager;
        #region 开始关注
        //"以下代码示例使当前用户开始关注或停止关注某目标项目"
        void GuanZhuAndStopGuanZhu()
        {
            des.Text = "";
            // Replace the following placeholder values with the URL of the target
            // server and target document (or site).
            string serverUrl = txtSiteUrl.Text;
            string contentUrl = txtDocUrl.Text;
            SPSocialActorType contentType = SPSocialActorType.Site;
            //WriteFollowedCount(contentType);
            using (SPSite site = new SPSite(serverUrl))
            {
                using (new Microsoft.SharePoint.SPServiceContextScope(SPServiceContext.GetContext(site)))
                {
                    SPServiceContext serviceContext = SPServiceContext.GetContext(site);
                    UserProfileManager upm = new UserProfileManager(serviceContext);
                    ////Create user sample string sAccount = "mydomain\\myalias"; 
                    ////To set prop values on user profile 
                    string accountName = SPContext.Current.Web.CurrentUser.LoginName;
                    accountName = accountName.Substring(accountName.LastIndexOf("|") + 1);
                    //if (accountName == "SHAREPOINT\\system")
                    //    accountName = "ccc\\xueqingxia";
                    ////UserProfile u = upm.GetUserProfile(accountName);
                    followingManager = new SPSocialFollowingManager();//u,serviceContext);

                    int followedCount = followingManager.GetFollowedCount(SPSocialActorTypes.Sites);
                    des.Text = "关注的网站的个数：" + followedCount.ToString();
                    // Create a SocialActorInfo object to represent the target item.
                    SPSocialActorInfo actorInfo = new SPSocialActorInfo();
                    actorInfo.ContentUri = new Uri(contentUrl);
                    actorInfo.ActorType = contentType;
                    //actorInfo.AccountName = accountName;

                    // Find out whether the current user is following the target item.
                    bool isFollowed = followingManager.IsFollowed(actorInfo);
                    des.Text += " isFollowed:" + isFollowed.ToString();
                    try
                    {
                        //SPSocialFollowResult result = followingManager.Follow(actorInfo);
                        //// If the result is AlreadyFollowing, then stop following 
                        //// the target item.
                        //des.Text += "FollowResult:" + result.ToString();
                        //if (result == SPSocialFollowResult.AlreadyFollowing)
                        //{
                        if (isFollowed)
                        {
                            des.Text += " StopFollowing: ";
                            bool isToped = followingManager.StopFollowing(actorInfo);
                            des.Text += isToped.ToString();
                        }
                        else
                        {
                            des.Text += " Follow: ";
                            SPSocialFollowResult result = followingManager.Follow(actorInfo);
                            // If the result is AlreadyFollowing, then stop following 
                            // the target item.
                            des.Text += "FollowResult:" + result.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        des.Text += ex.ToString();
                    }
                }
            }

        }

        // Get the count of the items that the current user is following.
        void WriteFollowedCount(SPSocialActorType type)
        {

            // Set the parameter for the GetFollowedCount method, and
            // handle the case where the item is a site. 
            SPSocialActorTypes types = SPSocialActorTypes.Documents;
            if (type != SPSocialActorType.Document)
            {
                types = SPSocialActorTypes.Sites;
            }

            int followedCount = followingManager.GetFollowedCount(types);
            des.Text ="关注的网站的个数："+ followedCount.ToString();
            //clientContext.ExecuteQuery();
            //Console.WriteLine("{0} followed {1}", followedCount.Value, types.ToString().ToLower());
        }
        #endregion
        #region 正在关注的文档
        //以下代码示例获取当前用户正在关注的文档和网站并获取有关用户的关注内容状态的信息
        void GetGuanZhu()
        {
            // Replace the following placeholder values with the URLs of
            // the target server, document, and site.
             string serverUrl = txtSiteUrl.Text ;
             //string docContentUrl =txtDocUrl.Text ;
             string siteContentUrl = txtDocUrl.Text; // do not use a trailing '/' for a subsite

            // Get the client context.
            //ClientContext clientContext = new ClientContext(serverUrl);

            // Get the SocialFollowingManager instance.
            SPSocialFollowingManager followingManager = new SPSocialFollowingManager();

            // Create SocialActorInfo objects to represent the target 
            // document and site.
            //SPSocialActorInfo docActorInfo = new SPSocialActorInfo();
            //docActorInfo.ContentUri = new Uri(docContentUrl);
            //docActorInfo.ActorType = SPSocialActorType.Site;
            SPSocialActorInfo siteActorInfo = new SPSocialActorInfo();
            siteActorInfo.ContentUri = new Uri(siteContentUrl);
            siteActorInfo.ActorType = SPSocialActorType.Site;

            // Find out whether the current user is following the target
            // document and site.
            //ClientResult<bool> isDocFollowed = followingManager.IsFollowed(docActorInfo);
            bool isSiteFollowed = followingManager.IsFollowed(siteActorInfo);

            // Get the count of documents and sites that the current
            // user is following.
            //ClientResult<int> followedDocCount = followingManager.GetFollowedCount(SocialActorTypes.Documents);
            int followedSiteCount = followingManager.GetFollowedCount(SPSocialActorTypes.Sites);

            // Get the documents and the sites that the current user
            // is following.
            //ClientResult<SocialActor[]> followedDocResult = followingManager.GetFollowed(SocialActorTypes.Documents);
           SPSocialActor[]  followedSiteResult = followingManager.GetFollowed(SPSocialActorTypes.Sites);

            // Get the information from the server.
            clientContext.ExecuteQuery();

            //// Write the results to the console window.
            //Console.WriteLine("Is the current user following the target document? {0}", isDocFollowed.Value);
            //Console.WriteLine("Is the current user following the target site? {0}", isSiteFollowed.Value);
            //if (followedDocCount.Value > 0)
            //{
            //    IterateThroughContent(followedDocCount.Value, followedDocResult.Value);
            //} 
            if (followedSiteCount > 0)
            {
                IterateThroughContent(followedSiteCount, followedSiteResult);
            }
            Console.ReadKey();
        }

        // Iterate through the items and get each item's display
        // name, content URI, and absolute URI.
        void IterateThroughContent(int count, SPSocialActor[] actors)
        {
            SPSocialActorType actorType = actors[0].ActorType;
            //Console.WriteLine("\nThe current user is following {0} {1}s:", count, actorType.ToString().ToLower());
            foreach (SPSocialActor actor in actors)
            {
                
                //Console.WriteLine("  - {0}", actor.Name);
                //Console.WriteLine("\tContent URI: {0}", actor.ContentUri);
                //Console.WriteLine("\tURI: {0}", actor.Uri);
            }
        }
        #endregion

        protected void btnGet_Click(object sender, EventArgs e)
        {
            //GetUserProfile();
            //return;
            //ClientGuan();
            //GuanZhuAndStopGuanZhu();
            //ClientGuanZhu();
            FollowingAndStopFollowing();
        }
        private void GetAllMySites()
        {
            SPWebApplication myApps = SPContext.Current.Site.WebApplication;
            foreach (SPSite mySite in myApps.Sites )
            {

            }
        }
        //最新的关注的网站和取消的网站
        private void FollowingAndStopFollowing()
        {
            SPUser us = SPContext.Current.Web.CurrentUser;
            SPSocialFollowingManager _followManager = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite elevatedSite = new SPSite(SPContext.Current.Web.Url))
                {
                    SPServiceContext serverContext = SPServiceContext.GetContext(SPContext.Current.Site);
                    UserProfileManager profileManager = new UserProfileManager(serverContext);
                    UserProfile profile = profileManager.GetUserProfile(us.LoginName);
                    if (profile != null)
                    {
                        //Create a Social Manager profile
                        _followManager = new SPSocialFollowingManager(profile);
                        SPSocialActorInfo actorInfo = null;
                        //SPSocialActor[] actors = _followManager.GetFollowers();
                        SPSocialActor[] ifollow = _followManager.GetFollowed(SPSocialActorTypes.Sites);
                        foreach (SPSocialActor follower in ifollow)
                        {
                            actorInfo = new SPSocialActorInfo();
                            actorInfo.ContentUri = follower.Uri ;
                            actorInfo.ActorType = SPSocialActorType.Site;
                            if (!_followManager.IsFollowed(actorInfo))
                            {
                                _followManager.Follow(actorInfo);
                                des.Text = "follow:OK";
                            }
                            else
                            {
                                _followManager.StopFollowing(actorInfo);
                                des.Text = "stoppFollowing OK";
                            }
                        }
                    }
                }
            });
        }
        //查询结果
        private int FullQuery(string txtFrom, string txtTo)
        {
            SPServiceContext context = SPServiceContext.Current;// ServerContext.Current;//ServerContext.GetContext
            SearchServiceApplicationProxy ssap = (SearchServiceApplicationProxy)SearchServiceApplicationProxy.GetProxy(SPServiceContext.GetContext(SPContext.Current.Site));
            using (KeywordQuery qry = new KeywordQuery(ssap))
            {
                qry.EnableStemming = true;
                qry.TrimDuplicates = true;
                qry.RowLimit = 10000;
                string queryText = "ContentClass:STS_ListItem_Posts";
                //queryText = "-Author:系统帐户 -Author:administrator ";
                qry.QueryText = queryText +" Created:" + txtFrom + ".." + txtTo;
                qry.SelectProperties.AddRange(new string[] { "WorkId", "Title", "Author", "Created", "Path", "ContentClass","FileExtension","EditorOWSUSER", "HitHighlightedSummary", "ParentLink"  });
                qry.SortList.Add("Created", Microsoft.Office.Server.Search.Query.SortDirection.Descending);
                SearchExecutor searchExecutor = new SearchExecutor();
                ResultTableCollection resultTableCollection = searchExecutor.ExecuteQuery(qry);
                IEnumerator<ResultTable> iResult = resultTableCollection.Filter("TableType", KnownTableTypes.RelevantResults).GetEnumerator();
                iResult.MoveNext();
                ResultTable resultTable = iResult.Current;
                DataTable queryDataTable = resultTable.Table;
                Response.Clear();
                for (int i = 0; i < queryDataTable.Columns.Count; i++)
                {
                    //Response.Write(i.ToString() + "--name:" + queryDataTable.Columns[i].ColumnName + "--value:" + queryDataTable.Rows[0][i].ToString() + "<br>");
                
                }
                for (int i = 0; i < queryDataTable.Rows.Count; i++)
                {
                    for (int j = 0; j<queryDataTable.Columns.Count; j++)
                    {
                        string colName = queryDataTable.Columns[j].ColumnName;
                        Response.Write(colName + ":" + queryDataTable.Rows[i][colName].ToString() + "<br>");

                    }
                }
             //for (int i = 0; i < queryDataTable.Rows.Count; i++)
                //{
                //    //Response.Write(i.ToString() + "<br>");
                //    Response.Write("Author:" + "--value:" + queryDataTable.Rows[i]["Author"].ToString() + "<br>");
                //    Response.Write("EditorOWSUSER:" + "--value:" + queryDataTable.Rows[i]["EditorOWSUSER"].ToString() + "<br>");
                //    Response.Write("ParentLink:" + "--value:" + queryDataTable.Rows[i]["ParentLink"].ToString() + "<br>");
                //    //Response.Write("DocId:" + "--value:" + queryDataTable.Rows[i]["DocId"].ToString() + "<br>");
                //    //Response.Write("EditorOWSUSER:" + "--value:" + queryDataTable.Rows[i]["EditorOWSUSER"].ToString() + "<br>");
                //    //Response.Write("Title:" + "--value:" + queryDataTable.Rows[i]["Title"].ToString() + "<br>");

                //}

                    return queryDataTable.Rows.Count;

            }
        }
        //获取用户配置文件
        private void GetUserProfile()
        {
            SPSite site = SPContext.Current.Site;//(txtSiteUrl.Text))
            SPServiceContext serviceContext = SPServiceContext.GetContext(site);
            UserProfileManager upm = new UserProfileManager(serviceContext);
            string accountName = SPContext.Current.Web.CurrentUser.LoginName;
            accountName = accountName.Substring(accountName.LastIndexOf("|") + 1);
            des.Text = "";
            if ( accountName.ToLower()  == "sharepoint\\system")
                accountName = "ccc\\xueqingxia";
            if (upm.UserExists(accountName))
            {

                UserProfile u = upm.GetUserProfile(accountName);
              
                Response.Write(u[PropertyConstants.PictureUrl].Value);
                Response.Write(u.PersonalSite.Url);
            }
        }
        private void ClientGuanZhu()
        {
            SPSite site = SPContext.Current.Site;//(txtSiteUrl.Text))
            SPServiceContext serviceContext = SPServiceContext.GetContext(site);
            UserProfileManager upm = new UserProfileManager(serviceContext);
            string accountName = SPContext.Current.Web.CurrentUser.LoginName;
            accountName = accountName.Substring(accountName.LastIndexOf("|") + 1);
            des.Text = "";
            if (accountName == "sharepoint\\system")
                accountName = "ccc\\xueqingxia";
            if (upm.UserExists(accountName))
            {
                UserProfile u = upm.GetUserProfile(accountName);
                Response.Write(u[PropertyConstants.PictureUrl].Value);
                SPSocialFollowingManager follow = new SPSocialFollowingManager(u, serviceContext);
                SPSocialActorInfo socialInfo = new SPSocialActorInfo();
                des.Text = follow.GetFollowedCount(SPSocialActorTypes.Sites).ToString() ;
                foreach (SPSocialActor spFollow in follow.GetFollowed(SPSocialActorTypes.Sites))
                {
                    //spFollow.Status = SPSocialStatusCode.InternalError;
                    socialInfo.ContentUri = spFollow.Uri;
                    //socialInfo.AccountName = u.AccountName;
                    socialInfo.ActorType = SPSocialActorType.Site;
                    follow.StopFollowing(socialInfo);
                    des.Text += "ok! ";
                }
            }

        }
        private void ClientGuan()
        {
             ClientContext clientContext = new ClientContext(txtSiteUrl.Text );
             NetworkCredential cc = new NetworkCredential("userb", "123456", "ccc");
             clientContext.Credentials = cc;

            SocialFollowingManager followingManager = new SocialFollowingManager(clientContext); 
            SocialActorInfo actorInfo = new SocialActorInfo();
            actorInfo.ContentUri = txtDocUrl.Text;
            actorInfo.ActorType = SocialActorType.Site;
            //string accountName = SPContext.Current.Web.CurrentUser.LoginName;
            //accountName = accountName.Substring(accountName.LastIndexOf("|") + 1);
            //actorInfo.AccountName = accountName;
            ClientResult<bool> isFollowed = followingManager.IsFollowed(actorInfo);
                clientContext.ExecuteQuery();
         
                if (isFollowed.Value == true)
                {
                    //If the user already following you can stop following by calling the StopFollowing() method. 
                    followingManager.StopFollowing(actorInfo);
                    clientContext.ExecuteQuery();
                    //des.Text =accountName+ "    StopFollowing OK";
                }
                else
                { //If the user is not follwing then you can follow by using the Follow() method. 
                    followingManager.Follow(actorInfo);
                    clientContext.ExecuteQuery();
                    //des.Text = accountName + "    Follow OK";
                }
               //});
                //}
            //}
            //catch (Exception ex)
            //{
            //    //des.Text += des.Text + ex.ToString();
            //}
        }
    }
}
