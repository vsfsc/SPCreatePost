using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;
using Microsoft.Office.Server.Search.Query;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.Office.Server.UserProfiles;
using System.Data;
using System.Collections.Generic;
using Microsoft.SharePoint.ApplicationPages;

namespace SPCreatePost.Layouts.SPCreatePost
{
    public partial class LatestBlog : UnsecuredLayoutsPageBase
    {
        #region 事件
        protected override bool AllowAnonymousAccess
        {
            get
            {
                return true;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CurrentPage = 0;
                try
                {
                    SetButtonState();
                }
                catch (Exception ex)
                {
                    divBlogContent.InnerText = ex.ToString();
                }
                try
                {
                    FillBlogContent();

                }
                catch (Exception ex)
                {
                    divBlogContent.InnerText = ex.ToString();

                }
                try
                {
                    GetBlogTotal();

                }
                catch (Exception ex)
                {
                    divBlogTotal.InnerText = ex.ToString();
                }
                try
                {
                    GetHotBlog();
                }
                catch (Exception ex)
                {
                    divHotBlog.InnerText = ex.ToString();
                }
            }
            btnPrev.Click += btnPrev_Click;
            btnNext.Click += btnNext_Click;
        }

        void btnNext_Click(object sender, EventArgs e)
        {
            NextPreview();
        }

        void btnPrev_Click(object sender, EventArgs e)
        {
            PrevPreview();
        }
        #endregion
        #region 最新博文
        #region 关于换页
        private DataTable GetAllBlogs
        {
            get
            {
                if (ViewState["dsNewBlog"] == null)
                {
                    DataTable dt = FullQuery(1);
                    ViewState["dsNewBlog"] = dt;
                }
                return (DataTable)ViewState["dsNewBlog"];
            }
        }
        //当前页
        private int CurrentPage
        {
            get
            {
                if (ViewState["currentNav"] == null)
                    return -1;
                else
                    return (int)ViewState["currentNav"];
            }
            set
            {
                ViewState["currentNav"] = value;
            }
        }
        //总页数
        private int TotalPage
        {
            get
            {
                DataTable dt = GetAllBlogs;
                int tPage = dt.Rows.Count / pageNumber;
                if (dt.Rows.Count % pageNumber > 0)
                    tPage = tPage + 1;
                return tPage;
            }
        }
        private void SetButtonState()
        {
            if (CurrentPage < TotalPage - 1)
                btnNext.Visible = true;
            else
                btnNext.Visible = false;
            if (CurrentPage > 0)
                btnPrev.Visible = true;
            else
            {
                btnPrev.Visible = false;
            }

        }
        //上一页
        private void PrevPreview()
        {
            if (CurrentPage > 0)
                CurrentPage = CurrentPage - 1;
            FillBlogContent();
            SetButtonState();
        }
        //下一页
        private void NextPreview()
        {
            if (CurrentPage < TotalPage - 1)
                CurrentPage = CurrentPage + 1;
            FillBlogContent();
            SetButtonState();
        }
        #endregion
        #region 方法
        //按时间显示开始的十条
        /// <summary>
        /// 1为博文，2为汇总
        /// </summary>
        /// <param name="queryState"></param>
        /// <returns></returns>
        private DataTable FullQuery(int queryState)
        {
            DataTable queryDataTable = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
          {
              SPServiceContext context = SPServiceContext.Current;// ServerContext.Current;//ServerContext.GetContext
              SearchServiceApplicationProxy ssap = (SearchServiceApplicationProxy)SearchServiceApplicationProxy.GetProxy(SPServiceContext.GetContext(SPContext.Current.Site));
              using (KeywordQuery qry = new KeywordQuery(ssap))
              {
                  qry.EnableStemming = true;
                  qry.TrimDuplicates = true;
                  if (queryState == 1)
                      qry.RowLimit = totalCount;
                  else
                      qry.RowLimit = 5000;
                  string queryText = "-author:系统帐户 -author:administrator ContentClass:STS_ListItem_Posts";
                  if (queryState > 1)
                  {
                      double dValue = -30;
                      //if (queryState == 3)
                      //    dValue = -60;
                      queryText = queryText + " Created:" + DateTime.Now.AddDays(dValue).ToString("yyyy-MM-dd") + ".." + DateTime.Now.ToString("yyyy-MM-dd");
                  }
                  qry.QueryText = queryText;// 
                  qry.SelectProperties.AddRange(new string[] { "WorkId", "Title", "Author","AuthorOWSUSER", "Created", "Path", "EditorOWSUSER", "HitHighlightedSummary", "ParentLink" });
                  qry.SortList.Add("Created", Microsoft.Office.Server.Search.Query.SortDirection.Descending);
                  SearchExecutor searchExecutor = new SearchExecutor();
                  ResultTableCollection resultTableCollection = searchExecutor.ExecuteQuery(qry);
                  IEnumerator<ResultTable> iResult = resultTableCollection.Filter("TableType", KnownTableTypes.RelevantResults).GetEnumerator();
                  iResult.MoveNext();
                  ResultTable resultTable = iResult.Current;
                  queryDataTable = resultTable.Table;
                  foreach (DataRow dr in queryDataTable.Rows  )
                  {
                      string author = dr["Author"].ToString();
                      if (author.IndexOf(";") > 0)//多个作者
                      {
                          dr["Author"] = author.Substring(0, author.IndexOf(";"));
                      }
                  }
                  queryDataTable.AcceptChanges();
              }
          });
            return queryDataTable;
        }
        //获取用户头像
        private string GetPhotoUrlByUserProfile(string loginName)
        {
            string photoUrl = "";
            if (SPContext.Current.Site.OpenWeb().CurrentUser == null)
            {
                photoUrl = "/_layouts/15/images/PersonPlaceholder.200x150x32.png";
                return photoUrl;
            }
            string accountName = loginName.Substring(loginName.LastIndexOf("|") + 1);
            using (SPSite site = new SPSite(SPContext.Current.Site.Url))
            {
                SPServiceContext serviceContext = SPServiceContext.GetContext(site);
                UserProfileManager upm = new UserProfileManager(serviceContext);
                if (upm.UserExists(accountName))
                {

                    UserProfile u = upm.GetUserProfile(accountName);
                    if (u[PropertyConstants.PictureUrl].Value == null)
                        photoUrl = "/_layouts/15/images/PersonPlaceholder.200x150x32.png";
                    else
                        photoUrl = u[PropertyConstants.PictureUrl].Value.ToString();
                }
            }
            return photoUrl;
        }
        private int pageNumber = 15;
        private int totalCount = 75;
        private void FillBlogContent()
        {
            StringBuilder strContent = new StringBuilder();
            strContent.AppendLine("<ul  class=\"Newblog\">");
            DataTable dt = GetAllBlogs;
            int rowIndex;
            for (int i = 0; i < pageNumber; i++)
            {
                rowIndex = CurrentPage * pageNumber + i;
                if (rowIndex == dt.Rows.Count) break;
                strContent.AppendLine(GetBlogItem(dt.Rows[rowIndex], i));
            }
            strContent.AppendLine("</ul>");
            divBlogContent.InnerHtml = strContent.ToString();
            lblPage.Text = (CurrentPage + 1).ToString();
        }
        private string GetBlogItem(DataRow dr, int i)
        {
            StringBuilder strContent = new StringBuilder();
            string photoUrl = GetPhotoUrlByUserProfile(dr["AuthorOWSUSER"].ToString());
            strContent.AppendLine("<li>");
            if (i == 0)
                strContent.AppendLine("<div class=\"Newblog_F_bar\">");
            else
                strContent.AppendLine("<div class=\"Newblog_N_bar\">");
            strContent.AppendLine(" <div class=\"Newblog_headImage\"> ");
            strContent.AppendLine("   <img src='" + photoUrl + "'/>");
            strContent.AppendLine(" </div>");
            strContent.AppendLine("<div class=\"Newblog_bar_Content\">");
            strContent.AppendLine("<h3 class=\"Newblog_Name\"><a href='" + dr["ParentLink"] + "'>" + dr["Author"] + "</a></h3>");
            strContent.AppendLine("<h3><a href='" + dr["Path"] + "'class=\"Newblog_Title\">" + dr["Title"] + "</a></h3>");
            string strBody = dr["HitHighlightedSummary"].ToString();
            if (strBody.Length > 200)
                strBody = strBody.Substring(0, 200) + "......";
            else
                strBody = strBody + "......";
            strContent.AppendLine("<p class=\"Newblog_P\">" + strBody + "</p>");
            strContent.AppendLine("<p class=\"Newblog_Time\">" + ((DateTime)dr["Created"]).AddHours(8).ToString("yyyy年MM月dd日 HH:mm:ss") + "</p>");
            strContent.AppendLine("</div>");
            strContent.AppendLine("</div>");
            strContent.AppendLine("</li>");
            return strContent.ToString();
        }
        #endregion
        #endregion
        #region 博客统计
        /// <summary>
        /// 统计一个月时间的博客汇总
        /// </summary>
        private void GetBlogTotal()
        {
            DataTable dt = GetMonthBlogs;
            DataView dataView = dt.DefaultView;
            if (dataView.Count == 0) return;
            DataTable dtDistinct = dataView.ToTable(true, "Author", "AuthorOWSUSER", "ParentLink");//注：其中ToTable（）的第一个参数为是否 
            dtDistinct.Columns.Add("Count", typeof(int));
            foreach (DataRow dr in dtDistinct.Rows)
            {
                object count = dt.Compute("count(Author)", "Author='" + dr["Author"].ToString() + "'");
                dr["Count"] = count;
            }
            dtDistinct.AcceptChanges();
            DataRow[] drs = dtDistinct.Select("Count>0", "Count DESC");
            StringBuilder strContent = new StringBuilder();
            int i = 1;
            string author = "";
            foreach (DataRow dr1 in drs)
            {
                if (i > 5) break;
                if (author.IndexOf(dr1["Author"].ToString()) < 0)
                {
                    strContent.AppendLine(GetTotalItem(dr1));
                    author = author + dr1["Author"].ToString();
                    i = i + 1;
                }
            }
            divBlogTotal.InnerHtml = divBlogTotal.InnerHtml.Replace("<li id=\"lv1\"></li>", strContent.ToString());
        }
        private string GetTotalItem(DataRow dr)
        {
            string photoUrl = GetPhotoUrlByUserProfile(dr["AuthorOWSUSER"].ToString());
            int totalCount = (int)dr["Count"];
            string author = dr["Author"].ToString();
            StringBuilder strContent = new StringBuilder();
            strContent.AppendLine("<li>");
            strContent.AppendLine("<div class=\"blogTop_Block\">");
            strContent.AppendLine("<div class=\"blogTop_Head\">");
            strContent.AppendLine("<img src='" + photoUrl + "'/>");
            strContent.AppendLine(" </div>");
            strContent.AppendLine("<div class=\"blogTop_Name\">");
            strContent.AppendLine("<h3><a href='" + dr["ParentLink"].ToString() + "'>" + author + "</a></h3>");
            strContent.AppendLine("<p>本月" + totalCount.ToString() + "篇</p>");
            strContent.AppendLine("</div>");
            strContent.AppendLine("</div>");
            strContent.AppendLine("</li>");
            return strContent.ToString();

        }
        #endregion
        #region 最热博文
        private void GetHotBlog()
        {
            DataTable dt = GetMonthBlogs;
            DataView dataView = dt.DefaultView;
            if (dataView.Count == 0) return;
            DataTable dtAuthor = dataView.ToTable(true, "ParentLink", "EditorOWSUSER");
            DataTable dtDistinct = dataView.ToTable(true, "Path", "Title", "EditorOWSUSER");//注：其中ToTable（）的第一个参数为是否 
            dtDistinct.Columns.Add("HitCount", typeof(int));
            foreach (DataRow drAuthor in dtAuthor.Rows)
            {
                string path = drAuthor["ParentLink"].ToString();
                SPList myBlog = GetBlogList(path);
                if (myBlog == null) break; 
                path=drAuthor["EditorOWSUSER"].ToString();
                DataRow[] drsBlog = dtDistinct.Select("EditorOWSUSER='"+path+"'");  
                foreach (DataRow dr in drsBlog)
                {
                    dr["HitCount"] = GetHotCount(myBlog,dr["Path"].ToString() );
                }
            }
            dtDistinct.AcceptChanges();
            DataRow[] drs = dtDistinct.Select("HitCount>0", "HitCount DESC");
            StringBuilder strContent = new StringBuilder();
            int i = 1;
            foreach (DataRow dr1 in drs)
            {
                if (i > 10) break;
                strContent.AppendLine(GetHotItem(dr1, i));
                i = i + 1;
            }
            divHotBlog.InnerHtml = divHotBlog.InnerHtml.Replace("<li id=\"lv2\"></li>", strContent.ToString());
        }
        private string GetHotItem(DataRow dr, int i)
        {
            int totalCount = (int)dr["HitCount"];
            StringBuilder strContent = new StringBuilder();
            strContent.AppendLine("<li><div  class=\"hotBlog_block\">");
            strContent.AppendLine("<div class=\"hotBlog_Circle_Second\">" + i.ToString() + "</div>");
            strContent.AppendLine("<h3><a href='" + dr["Path"] + "'>" + dr["Title"] + "</a></h3></div></li>");
            return strContent.ToString();

        }
        private SPList GetBlogList(string url)
        {
            SPList myBlog = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    string siteUrl = url.Substring(0, url.IndexOf("/Blog"));
                    using (SPSite mySite = new SPSite(siteUrl))
                    {
                        SPWeb myWeb = mySite.AllWebs["Blog"];
                        myBlog = myWeb.Lists["Posts"];
                    }
                });
            }
            catch
            { }
            return myBlog;
            
        }
        private int GetHotCount(SPList myBlog, string url)
        {
            int hotCount = 0;
            int id = int.Parse(url.Substring(url.IndexOf("=") + 1));
            try
            {
                SPListItem myBlogList = myBlog.GetItemById(id);
                if (myBlogList["LikesCount"] != null)
                    hotCount = int.Parse(myBlogList["LikesCount"].ToString());
            }
            catch
            { }
            return hotCount;
        }
        #endregion
        #region 获取近一个的博客
        private DataTable GetMonthBlogs
        {
            get
            {
                if (ViewState["dsMonthBlog"] == null)
                {
                    DataTable dt = FullQuery(2);
                    ViewState["dsMonthBlog"] = dt;
                }
                return (DataTable)ViewState["dsMonthBlog"];
            }
        }
        #endregion
    }
}
