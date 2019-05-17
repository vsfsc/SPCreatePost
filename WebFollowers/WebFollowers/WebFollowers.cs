using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.Office.Server.Social;

namespace WebFollowers.WebFollowers
{
    [ToolboxItemAttribute(false)]
    public class WebFollowers : WebPart
    {
        protected override void CreateChildControls()
        {
            SPSocialFollowingManager followingManager = new SPSocialFollowingManager();//u,serviceContext);
            SPSocialActorInfo actorInfo = new SPSocialActorInfo();
            actorInfo.ContentUri = new Uri(SPContext.Current.Web.Url);
            actorInfo.ActorType = SPSocialActorType.Site;

            SPSocialActor[] ifollow = followingManager.GetFollowers(); 
            SPWeb web = SPContext.Current.Web;
        }
    }
}
