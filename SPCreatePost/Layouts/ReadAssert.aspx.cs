using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.Office.Server.Social;
using System.Linq;
namespace SPCreatePost.Layouts
{
    public partial class ReadAssert : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int count = GetSiteFollowedCount();
            txtDes.Text = "本网站关注人数：" + count.ToString();
        }
        //读取资产库的信息
        protected void btnRead_Click(object sender, EventArgs e)
        {
            ReadNotice();
        }
        private void ReadNotice()
        {
            string listName = "文档";
            txtDes.Text = "";
            SPList assert = SPContext.Current.Web .Lists[listName];
            SPQuery query = new SPQuery();
            foreach (SPField field in assert.Fields)
            {
                txtDes.Text += "internaName---" + field.InternalName + "---staticnaem---" + field.StaticName + "---title---" + field.Title + "-type-"+ field.Type.ToString ()+"<br>";
            }
        }
        private int GetSiteFollowedCount()
        {
            SPUser user = SPContext.Current.Web.CurrentUser;
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
                       }
                   }
               }
           });
            return i;
        }
        private void ReadAsset()
        {
            string listName = "我的资产库";
            txtDes.Text = "";
            SPList assert = SPContext.Current.Web.Lists[listName];
            SPQuery query = new SPQuery();
            string txtValue = "视频呈现";//
            query.ViewAttributes = "Scope='RecursiveAll'";
            query.Query = "<Where><Eq><FieldRef Name='ContentType'/><Value Type='Text'>" + txtValue + "</Value></Eq></Where>";
            //foreach (SPField field in assert.Fields)
            //{
            //    txtDes.Text += "internaName---" + field.InternalName + "---staticnaem---" + field.StaticName + "---title---" + field.Title + "<br>";
            //}
            //缩略图 URL url.string(0,len(url)-4)).replace("_",".")
            foreach (SPListItem   list in assert.GetItems(query))
            {
                if (list["分数"] == null)
                { list["分数"] = 10; list.Update(); }
                string url = list["缩略图 URL"].ToString();
                url = url.Substring(0, url.Length - 4).Replace("_",".");
                txtDes.Text += " " + list.Name + "---" + list.ContentType.Name + "--VideoUrl--" + url + "<br>";
            }
              

        }
    }
}
