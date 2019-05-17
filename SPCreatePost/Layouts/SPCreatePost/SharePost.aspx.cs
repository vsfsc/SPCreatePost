using System;
using System.Net;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Social;

namespace SPCreatePost.Layouts.SPCreatePost
{
    public partial class SharePost : LayoutsPageBase
    {
        protected string AccessToken
        {
            get
            {
                if (ViewState["accessToken"] == null)
                {
                    throw new NullReferenceException("Access token is empty.");
                }
                else
                {
                    return (string)ViewState["accessToken"];
                }
            }
            set
            {
                ViewState["accessToken"] = value;
            }
        }
        protected string HostWeb
        {
            get
            {
                if (ViewState["hostWeb"] == null)
                {
                    throw new NullReferenceException("HostWeb is empty.");
                }
                else
                {
                    return (string)ViewState["hostWeb"];
                }
            }
            set
            {
                ViewState["hostWeb"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                // The following code gets the client context and Title property by using TokenHelper.
                // To access other properties, you may need to request permissions on the host web.

                //var contextToken = TokenHelper.GetContextTokenFromRequest(Page.Request);
                //HostWeb = Page.Request["SPHostUrl"];

                //SharePointContextToken spContextToken = TokenHelper.ReadAndValidateContextToken(contextToken, Request.Url.Authority);
                //AccessToken = TokenHelper.GetAccessToken(spContextToken, new Uri(HostWeb).Authority).AccessToken;
            }

        }

        static void clientContext_ExecutingWebRequest(object sender, WebRequestEventArgs e)
        {
            e.WebRequestExecutor.WebRequest.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
        }
        static string newUrl = "http://xqx2012/my";
        protected void btnCreateSite_Click(object sender, EventArgs e)
        {
            string docLinkUrl = "http://xqx2012/DocLib1/IMGP2947.JPG";

            ClientContext ctx = new ClientContext(newUrl );
            ctx.Credentials = CredentialCache.DefaultCredentials;
            //ctx.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(clientContext_ExecutingWebRequest);
            //CredentialCache cc = new CredentialCache();
            //cc.Add(new Uri(newUrl), "NTLM", CredentialCache.DefaultNetworkCredentials);
            NetworkCredential cc = new NetworkCredential("userb", "123123", "ccc");
            //ctx.Credentials = cc;
            //ctx.AuthenticationMode = ClientAuthenticationMode.Default;

            //ctx.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            //using (var ctx = TokenHelper.GetClientContextWithAccessToken(HostWeb, AccessToken))
            //{
            try
            {
                SocialDataItem docLink = new SocialDataItem
                   {
                       ItemType = SocialDataItemType.Document,
                       Text = "link to picture",
                       Uri = docLinkUrl
                   };
                SocialPostCreationData postCreationData = new SocialPostCreationData();
                postCreationData.ContentText = "{0}";
                postCreationData.ContentItems = new SocialDataItem[1] { docLink };
                SocialFeedManager feedManager = new SocialFeedManager(ctx);
                // Publish the post. This is a root post to the user's feed, so specify
                // null for the targetId parameter.
                feedManager.CreatePost(null, postCreationData);

                //clientContext.ExecuteQuery();
                SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   ctx.ExecuteQuery();
               });
                lbMessage.Text = "success!";

            }
            catch (Exception ex)
            {
                lbMessage.Text = ex.Message;
            }
            //}
        }
    }
}
