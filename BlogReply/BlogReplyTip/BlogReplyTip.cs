using System;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace BlogReply.BlogReplyTip
{
    [ToolboxItemAttribute(false)]
    public class BlogReplyTip : WebPart
    {
        #region 事件
        protected override void CreateChildControls()
        {
            CheckPostBlog();
        }
        #endregion
        #region 方法
        
        private string GetHtml()
        {
            StringBuilder htmText = new StringBuilder();
            htmText.AppendLine("<html xmlns=\"http://www.w3.org/1999/xhtml\">");
            htmText.AppendLine("<head>");
            htmText.AppendLine("<style type=\"text/css\">");
            htmText.AppendLine(" #tishi{width:180px;}");
            htmText.AppendLine(".tixing{ width:180px;font-size:12px; border:1px #dadada solid;color:#5781d6;padding:5px 0px;}");
            htmText.AppendLine(".tixinga{ width:180px;font-size:12px; border:1px #dadada solid;}");
            htmText.AppendLine(".tixingb{background-color:#e3e0ff}");
            htmText.AppendLine(".tixingcolor{color:#005eac;margin-left:6px}");
            htmText.AppendLine(".tixingshuzi{color:#005eac; font-family:Arial, Helvetica, sans-serif}");
            htmText.AppendLine(".tixinga ul{ list-style:none; margin:0;padding:0;}");
            htmText.AppendLine(".tixinga ul li{ padding:5px 0; padding-left:6px;}");
            htmText.AppendLine(".tixinga ul li a{ text-decoration:none;color:#002672}");
            htmText.AppendLine(".tixinga ul li a:hover{ text-decoration:underline;color:#002672}");
            htmText.AppendLine(".hulv{ margin-left:10px;}");
            htmText.AppendLine(".tixinga ul li span a{ text-decoration:underline; color:#5781d6}");
            htmText.AppendLine(".tixinga ul li span a:hover{ color:#5781d6}");
            htmText.AppendLine("</style>");
            htmText.AppendLine("</head>");
            htmText.AppendLine("<body>");
            htmText.AppendLine("<div>");
            htmText.AppendLine("</div></body></html>");
            return htmText.ToString(); 
        }
        string photoLibaryName = "PostBlog";
        Table tbl;
        /// <summary>
        /// 检查是否有新回复(支持匿名访问)
        /// </summary>
        private void CheckPostBlog()
        {
            SPWeb web = SPContext.Current.Web;
            if (web.CurrentUser == null) return;
            int authorID = 0;
            try
            {
                authorID = web.Author.ID;
                if (!(web.CurrentUser.ID == web.Author.ID)) return;
            }
            catch
            {
                //未找到用户
                bool isAuthor = false;
                if (authorID == 0)
                {
                    foreach (SPUser sUser in web.Users)
                        if (web.CurrentUser.ID == sUser.ID && web.DoesUserHavePermissions(sUser.LoginName, SPBasePermissions.FullMask))
                        {
                            isAuthor = true;
                            break;
                        }
                    if (!isAuthor) return;
                }
            }
            try
            {
                tbl = new Table();
                TableRow tr = new TableRow();
                TableCell tcell = new TableCell();
                SPList lstNewPost = web.Lists[photoLibaryName];
                string fieldRefName = "Title";
                foreach (SPField fid in lstNewPost.Fields)
                {
                    if (fid.Title == "新回复数")
                    {
                        fieldRefName = fid.InternalName;// +";" + fid.StaticName + ";";
                        break;
                    }
                }
                //string sql = "select ID,新回复数, 标题,文章ID from " + photoLibaryName + "  where 新回复数 >0";// AND 创建时 
                SPQuery query = new SPQuery();
                query.ViewAttributes = "Scope='RecursiveAll'";
                query.Query = "<Where><Gt><FieldRef Name='" + fieldRefName + "'/><Value Type='Integer'>0</Value></Gt></Where>";
                SPListItemCollection items = lstNewPost.GetItems(query );
                //datat
                int rowIndex = 0;
                int tCount = 0;
                foreach (SPListItem itme in items)
                {
                    lstNewPost = web.Lists[listName];
                    query = new SPQuery();
                    query.ViewAttributes = "Scope='RecursiveAll'";
                    query.Query = "<Where><Eq><FieldRef Name='ID'/><Value Type='Integer'>" + itme["文章ID"]+"</Value></Eq></Where>";
                    //sql = "select 标题,ID from " + listName + "  where ID =" + itme["文章ID"];
                    SPListItemCollection blogItems = lstNewPost.GetItems(query);
                    string url = blogItems[0].ParentList.DefaultViewUrl;
                    url = url.Replace("AllPosts.aspx", "Post.aspx?") + "ID=" + itme["文章ID"];

                    tr = new TableRow();
                    tcell = new TableCell();
                    LinkButton lnk = new LinkButton();
                    string title= blogItems[0]["标题"].ToString();
                    if (titleLength > 0 && title.Length > titleLength)
                        title = title.Substring(0, titleLength); 
                    lnk.Text = title ;
                    lnk.CommandArgument = url;
                    lnk.CommandName = itme.ID.ToString();
                    lnk.Click += new EventHandler(lnk_Click);
                    tcell.Controls.Add(lnk);
                    tCount = tCount + Convert.ToInt32(itme["新回复数"]);
                    tcell.Controls.Add(new LiteralControl("(" + itme["新回复数"].ToString() + ")"));
                    tr.Cells.Add(tcell);

                    tcell = new TableCell();
                    lnk = new LinkButton();
                    lnk.CommandName = itme.ID.ToString();
                    lnk.Text = "忽略";
                    lnk.ID = rowIndex.ToString();
                    lnk.Click += new EventHandler(lnk_Click);
                    tcell.Controls.Add(lnk);
                    tr.Cells.Add(tcell);
                    rowIndex += 1;
                    tbl.Rows.Add(tr);

                }
                if (tCount > 0)
                {
                    tr = new TableRow();
                    tcell = new TableCell();
                    tcell.ColumnSpan = 2;
                    tcell.Controls.Add(new LiteralControl("提醒: 您有(" + tCount.ToString() + ")条信息未读"));
                    tr.Cells.Add(tcell);
                    tbl.Rows.AddAt(0, tr);
                }

                this.Controls.Add(tbl);

            }
            catch (Exception)
            {
                this.Controls.Add(new LiteralControl(""));
            }

        }
        /// <summary>
        /// 删除查看过的新提醒
        /// </summary>
        /// <param name="replyID"></param>
        private void DeleteNews(string replyID)
        {
            SPWeb web = SPContext.Current.Web;
            SPList lstReply = web.Lists[photoLibaryName];
            SPQuery query = new SPQuery();
            query.ViewAttributes = "Scope='RecursiveAll'";
            query.Query = "<Where><Eq><FieldRef Name='ID'/><Value Type='Integer'>" + replyID + "</Value></Eq></Where>";

            string sql = "select ID, 标题,文章ID,新回复数 from " + photoLibaryName + "  where ID =" + replyID;// AND 创建时间 = TODAY 
            SPListItemCollection items = lstReply.GetItems(query);
            if (items.Count > 0)
            {
                items[0].Delete();
            }
        }
        /// <summary>
        /// 浏览单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnk_Click(object sender, EventArgs e)
        {
            DeleteNews(((LinkButton)sender).CommandName);
            LinkButton btn=(LinkButton)sender;
            if (((LinkButton)sender).Text != "忽略")
                Page.Response.Redirect(((LinkButton)sender).CommandArgument);
            else
                tbl.Rows[int.Parse (btn.ID)].Visible = false; 
        }

        #endregion
        #region 属性
        string listName = "Posts";
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("存放博客的文章")]
        [WebDescription("存放博客的文章列表名称")]
        public string ListName
        {
            get
            {
                return listName;
            }

            set
            {
                listName = value;
            }
        }
        int titleLength = 0;
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("标题的长度")]
        [WebDescription("提醒的标题长度")]
        public int TitleLength
        {
            get
            {
                return titleLength;
            }

            set
            {
                titleLength = value;
            }
        }

        #endregion
    }
}
