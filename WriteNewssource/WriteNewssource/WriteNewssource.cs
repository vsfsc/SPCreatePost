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


namespace WriteNewssource.WriteNewssource
{
    [ToolboxItemAttribute(false)]
    public class WriteNewssource : WebPart
    {// server side new post
        private void AddNewssource(string postTitle, string docLinkUrl)
        {
            SPSocialDataItem item = new SPSocialDataItem()
             {
                       ItemType = SPSocialDataItemType.Document,
                       Text = postTitle,
                       Uri = new Uri(docLinkUrl)
             };
            //SPSocialDataItem item = item2;
            SPSocialPostCreationData data = new SPSocialPostCreationData();
            data.ContentText = "{0}";
            data.ContentItems = new SPSocialDataItem[] { item } ;
            try
            {
                SPSocialFeedManager feedManager = new SPSocialFeedManager();
                if (!this.GetPost(feedManager, postTitle, docLinkUrl))
                {
                    SPContext.Current.Web.ValidateFormDigest();
                    feedManager.CreatePost(null, data);
                    this.Controls.Add(new LiteralControl("The post was published."));
                }
                else
                {
                    this.Controls.Add(new LiteralControl("The post has published."));
                }
            }
            catch (Exception exception)
            {
                this.Controls.Add(new LiteralControl("Error publishing the post: " + exception.Message));
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            this.AddNewssource("鸟岛", "http://xqx2012/DocLib1/IMGP2947.JPG");
        }
        TextBox txtDocUrl;
        protected override void CreateChildControls()
        {
            if (SPContext.Current.Web.CurrentUser != null)
            {
                FormDigest child = new FormDigest();
                child.ID = "myFormDigest";
                this.Controls.Add(child);
                txtDocUrl = new TextBox();
                txtDocUrl.ID = "txtDocUrl";
                txtDocUrl.Text = SPContext.Current.Site.Url;
                this.Controls.Add(txtDocUrl);
                Button button = new Button();
                button.Text = "分享";
                button.Click += new EventHandler(this.btn_Click);
                this.Controls.Add(button);
                Button btn2 = new Button();
                btn2.Text = "关注";
                btn2.Click += btn2_Click;
                this.Controls.Add(btn2);
            }
        }

        void btn2_Click(object sender, EventArgs e)
        {
            //FollowingAndStopFollowing();
            GuanZhuAndStopGuanZhu();
        }
        void GuanZhuAndStopGuanZhu()
        {
            // Replace the following placeholder values with the URL of the target
            // server and target document (or site).
            //string serverUrl = SPContext.Current.Site.Url;
            string contentUrl = txtDocUrl.Text;
            SPSocialActorType contentType = SPSocialActorType.Site;
            SPSocialFollowingManager followingManager = null;
            //using (SPSite site = new SPSite(serverUrl))
            //{
            //using (new Microsoft.SharePoint.SPServiceContextScope(SPServiceContext.GetContext(site)))
            //{
            //SPServiceContext serviceContext = SPServiceContext.GetContext(site);
            //UserProfileManager upm = new UserProfileManager(serviceContext);
            ////Create user sample string sAccount = "mydomain\\myalias"; 
            ////To set prop values on user profile 
            //string accountName = SPContext.Current.Web.CurrentUser.LoginName;
            //accountName = accountName.Substring(accountName.LastIndexOf("|") + 1);
            //if (accountName == "SHAREPOINT\\system")
            //    accountName = "ccc\\xueqingxia";
            ////UserProfile u = upm.GetUserProfile(accountName);
            followingManager = new SPSocialFollowingManager();//u,serviceContext);


            // Create a SocialActorInfo object to represent the target item.
            SPSocialActorInfo actorInfo = new SPSocialActorInfo();
            actorInfo.ContentUri = new Uri(contentUrl);
            actorInfo.ActorType = contentType;
            //actorInfo.AccountName = accountName;

            // Find out whether the current user is following the target item.
            bool isFollowed = followingManager.IsFollowed(actorInfo);
            this.Controls.Add(new LiteralControl(" isFollowed:" + isFollowed.ToString()));
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
                    this.Controls.Add(new LiteralControl(" StopFollowing: "));
                    bool isToped = followingManager.StopFollowing(actorInfo);
                    this.Controls.Add(new LiteralControl(isToped.ToString()));
                }
                else
                {
                    //this.Controls.Add(new LiteralControl(" Follow: "));
                    //SPSocialFollowResult result = followingManager.Follow(actorInfo);
                    //// If the result is AlreadyFollowing, then stop following 
                    //// the target item.
                    //this.Controls.Add(new LiteralControl("FollowResult:" + result.ToString()));
                }
                SPSocialActor[] ifollow = followingManager.GetFollowed(SPSocialActorTypes.Sites);
                int followedCount = followingManager.GetFollowedCount(SPSocialActorTypes.Sites);
                this.Controls.Add(new LiteralControl("GetFollowedCount：" + followedCount.ToString() + "  GetFollowed "+ifollow.Length));
            }
            catch (Exception ex)
            {
                if (SPContext.Current.Web.Url == txtDocUrl.Text)
                    Page.Response.Redirect(SPContext.Current.Web.Url);
                //this.Controls.Add(new LiteralControl(ex.ToString()));
            }
            //}
            //}
        }
        private void FollowingAndStopFollowing()
        {
            //SPUser us = SPContext.Current.Web.CurrentUser;
            SPSocialFollowingManager _followManager = null;
            //SPSecurity.RunWithElevatedPrivileges(delegate()
            //{
            //using (SPSite elevatedSite = new SPSite(SPContext.Current.Site.Url ))
            //{
            //SPSite elevatedSite = new SPSite(SPContext.Current.Site.Url);
            //SPServiceContext serverContext = SPServiceContext.GetContext(SPContext.Current.Site);
            //UserProfileManager profileManager = new UserProfileManager(serverContext);
            //UserProfile profile = profileManager.GetUserProfile(us.LoginName);
            //if (profile != null)
            //{
            //Create a Social Manager profile
            _followManager = new SPSocialFollowingManager();//profile);
            SPSocialActorInfo actorInfo = null;
            //SPSocialActor[] actors = _followManager.GetFollowers();
            SPSocialActor[] ifollow = _followManager.GetFollowed(SPSocialActorTypes.Sites);
            //foreach (SPSocialActor follower in ifollow)
            //{
            actorInfo = new SPSocialActorInfo();
            actorInfo.ContentUri = new Uri(txtDocUrl.Text);// follower.Uri;
            actorInfo.ActorType = SPSocialActorType.Site;
            SPSocialFollowResult result;
            if (!_followManager.IsFollowed(actorInfo))
            {
                result = _followManager.Follow(actorInfo);
                this.Controls.Add(new LiteralControl("follow: " + result.ToString()));
            }
            else
            {
                bool r = _followManager.StopFollowing(actorInfo);
                this.Controls.Add(new LiteralControl("stoppFollowing " + r.ToString()));
            }
            //}
            //}
            //}
            //});
        }
        private bool GetPost(SPSocialFeedManager feedManager, string txtText, string linkUrl)
        {
            SPSocialFeed feed = feedManager.GetFeed( SPSocialFeedType.Personal, new SPSocialFeedOptions());
            foreach (SPSocialThread thread in feed.Threads)
            {
                SPSocialPost post = thread.RootPost ;
                if ((post.Text == txtText) && (post.Overlays[0].LinkUri == new Uri(linkUrl)))
                {
                    return true;
                }
            }
            return false;
        }

      
    }
}
