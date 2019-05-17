using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.Office.Server.Social;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Drawing;

namespace SPCreatePost.Layouts.SPCreatePost
{
    public partial class MyNewsfeed : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                GetAllPost();
        }
        #region 使用的方法
        //获取当前用户的新闻源
        private void GetAllPost()
        {
            SPSocialFeedManager feedManager = new SPSocialFeedManager();
            SPSocialFeedOptions socialOptions = new SPSocialFeedOptions();
            socialOptions.MaxThreadCount = int.MaxValue;
            SPSocialFeed feed = feedManager.GetFeed(SPSocialFeedType.Personal, socialOptions);
            IterateThroughFeed(feed, true);
        }
        void IterateThroughFeed(SPSocialFeed feed, bool isCurrentUserOwner)
        {
            SPSocialThread[] threads = feed.Threads;
            // Iterate through each thread in the feed.
            TableRow row;
            TableCell cell;
            foreach (SPSocialThread thread in threads)
            {
                if (thread.Attributes.ToString() != "None")//thread.ThreadType == SPSocialThreadType.Normal)
                {
                    //thread.Attributes-CanReply, CanLock/ None关注的新闻源不可以回复和锁定//rootPost.Attributes-CanLike（点赞）, CanDelete, UseAuthorImage, CanFollowUp(后续)
                    // If a thread contains more than two replies, the server returns // a thread digest that contains only the two most recent replies.
                    // To get all replies, call SocialFeedManager.GetFullThread.
                    SPSocialPost rootPost = thread.RootPost;
                    SPSocialActor[] actors = thread.Actors;
                    //SPSocialActor author = thread.Actors[rootPost.AuthorIndex];
                    row = new TableRow();
                    cell = new TableCell();
                    cell.Text = actors[rootPost.AuthorIndex].Name+"<br>"; 
                    int totalReplies = thread.TotalReplyCount;
                    if (rootPost.Overlays != null)
                    {
                        if (rootPost.Overlays.Length == 0)
                            cell.Text = rootPost.Text;// + "&nbsp;&nbsp;(" + totalReplies + " 回复) " + rootPost.CreatedTime.AddHours(8).ToString();
                        else
                        {
                            string lnk = rootPost.Overlays[0].LinkUri.ToString();
                            cell.Text = "<a href='" + lnk + "'>" + rootPost.Text + "</a>";
                        }
                    }
                    cell.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + rootPost.CreatedTime.AddHours(8).ToString();
                    cell.Text += "<a target='_blank' href='/my/ThreadView.aspx?ThreadID=" + rootPost.Id + "'><b>回复</b></a>";
                    row.Cells.Add(cell);
                    tblPosts.Rows.Add(row);
                    if (totalReplies > 0)
                    {
                        SPSocialPost[] replies = thread.Replies;
                        for (int j = 0; j < replies.Length; j++)
                        {
                            row = new TableRow();
                            tblPosts.Rows.Add(row);
                            cell = new TableCell();
                            cell.VerticalAlign = VerticalAlign.Top;
                            SPSocialPost reply = replies[j];
                            SPSocialAttachment atta= reply.Attachment;
                           
                            cell.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + actors[reply.AuthorIndex].Name + ";&nbsp;&nbsp;<b>回复</b>  " + reply.Text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp" + reply.CreatedTime.AddHours(8).ToString();

                            row.Cells.Add(cell);
                            row = new TableRow();
                            cell = new TableCell();
                            cell.HorizontalAlign = HorizontalAlign.Left;
                            if (atta!=null  && atta.AttachmentKind == SPSocialAttachmentKind.Image)
                                cell.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img src='" + atta.Uri.ToString() + "'/>";
                            row.Cells.Add(cell);
                            tblPosts.Rows.Add(row);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
