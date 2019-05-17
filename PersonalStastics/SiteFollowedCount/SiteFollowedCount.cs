using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

using Microsoft.Office.Server.UserProfiles;
using Microsoft.Office.Server.Social;
using System.Linq;

namespace PersonalStastics.SiteFollowedCount
{
    [ToolboxItemAttribute(false)]
    public class SiteFollowedCount : WebPart
    {
        #region 方法和事件
        protected override void CreateChildControls()
        {
            //if (SPContext.Current.Web.CurrentUser == null)
            //    return;
            //int count = GetSiteFollowedCount();
            //Label lbl = new Label();
            //lbl.Text = webTitleCust.Replace("@", count.ToString());
            //lbl.Font.Size = FontSize;
            //this.Controls.Add(lbl);
        }
        private int GetSiteFollowedCount()
        {
            string siteUrl = SPContext.Current.Web.Url;
            int i = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite currentSite = new SPSite(SPContext.Current.Site.Url))
                {
                    SPServiceContext contexto = SPServiceContext.GetContext(currentSite);

                    UserProfileManager perfiles = new UserProfileManager(contexto);

                    foreach (UserProfile userProfile in perfiles)
                    {
                        try
                        {
                            //Check if the user profile is already created
                            if (userProfile.PersonalSiteInstantiationState == PersonalSiteInstantiationState.Created)
                            {
                                SPSocialFollowingManager followingManager = new SPSocialFollowingManager(userProfile, contexto);

                                SPSocialActor[] followedUserSites = followingManager.GetFollowed(SPSocialActorTypes.Sites);

                                SPSocialActor matchSite = followedUserSites.Where(st => st.Uri.AbsoluteUri.Contains(siteUrl)).SingleOrDefault();
                                if (matchSite != null)
                                {
                                    i = i + 1;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            this.Controls.Add(new LiteralControl (ex.ToString() ));
                        }
                    }
                }
            });
            return i;
        }
        #endregion
        #region 属性
        int fontSize = 12;
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("字号大小")]
        [WebDescription("")]
        public int FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                fontSize = value;
            }
        }
        string webTitleCust = "本门课程有@人关注";
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("显示的文字内容")]
        [WebDescription("")]
        public string  WebTitleCust
        {
            get
            {
                return webTitleCust;
            }
            set
            {
                webTitleCust = value;
            }
        }       
        #endregion
    }
}
