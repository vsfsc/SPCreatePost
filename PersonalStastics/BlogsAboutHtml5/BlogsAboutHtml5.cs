using System;
using System.ComponentModel;
using System.Web.UI.HtmlControls;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.Office.Server.Search.Query;
using Microsoft.Office.Server.Search.Administration;
using Microsoft.Office.Server.UserProfiles;
using System.Data;
using System.Collections.Generic;
using System.Text;


namespace PersonalStastics.BlogsAboutHtml5
{
    [ToolboxItemAttribute(false)]
    public class BlogsAboutHtml5 : WebPart
    {
       
        private void AddStyle()
        {
            StringBuilder txt = new StringBuilder();
            txt.AppendLine("<style type=\"text/css\" >");
            //txt.AppendLine("body{background-color:#e9edf0}");
            txt.AppendLine(" .NewBlogDiv{margin:10px auto;width:"+ShowdivWidth.ToString() +"px;float:left}");
            txt.AppendLine(".NewBlogDiv_Title{background-color:#fbfbfb; height:42px;border:1px solid #d9d9d9; border-bottom:1px solid #d9d9d9; line-height:42px}");
            txt.AppendLine(".NewBlogDiv_Title h1{margin:0 15px;font-family:微软雅黑;font-size:14px}");
            txt.AppendLine(".Newblog {list-style: none;margin:0;padding:0;}");
            txt.AppendLine(".Newblog_F_bar{border:1px solid #d9d9d9;zoom: 1;overflow:auto;background-color:#fff}");
            txt.AppendLine(".Newblog_N_bar{border:1px solid #d9d9d9;zoom: 1;overflow:auto;background-color:#fff;margin-top:10px}");
            txt.AppendLine(".Newblog_bar_Content{float:left;width:"+(ShowdivWidth-120).ToString()  +"px;margin:10px;}");
            txt.AppendLine(".Newblog_headImage{float:left;width:60px; height:60px; border-radius:50%; overflow:hidden;margin:10px}");
            txt.AppendLine(".Newblog_headImage img{width:60px;height:60px;display:block}");
            txt.AppendLine(".Newblog_Name{font-weight:normal;margin:10px 0;}");
            txt.AppendLine(".Newblog_Name a{font-family:微软雅黑;font-size:16px;color:#7ab324;text-decoration:none;cursor:pointer}");
            txt.AppendLine(".Newblog_Title{color:#0072c6; font-weight:normal;font-family:微软雅黑;font-size:14px;cursor:pointer; text-decoration:none}");
            txt.AppendLine(".Newblog_P{font-family:微软雅黑;line-height:1.5;margin:5px 0 10px;font-size:14px;color:#333;}");
            txt.AppendLine(".Newblog_Time{color:#666; font-size:13px;margin:5px 0px}");
            txt.AppendLine(".Newblog_NextPage{background-color:#fbfbfb; height:42px;border:1px solid #d9d9d9; border-top:0px; text-align:center; line-height:42px;margin:0 0 15px 0;}");
            txt.AppendLine(".Newblog_NextPage h4{margin:0 15px;font-family:微软雅黑;font-size:14px}");
            txt.AppendLine("</style>");
            Page.Header.Controls.Add(new LiteralControl(txt.ToString()));
        }

        protected override void CreateChildControls()
        {
            AddStyle();
            InitControl();
            if (!Page.IsPostBack)
            {
                CurrentPage = 0;
                FillBlogContent();
                SetButtonState();
            }
        }
        private void InitControl()
        {
            StringBuilder txt = new StringBuilder();
            txt.AppendLine("<div class=\"NewBlogDiv\"  >");
            //txt.AppendLine("<div class=\"NewBlogDiv_Title\"><h1>最新博文</h1></div>");
            txt.AppendLine("<!-- 最新博文列表 -->");
            this.Controls.Add(new LiteralControl(txt.ToString()));
            divBlogContent = new HtmlGenericControl();
            this.Controls.Add(divBlogContent);
            this.Controls.Add(new LiteralControl("<div class=\"Newblog_NextPage\"><h4><span style=\"font-size:13px; font-weight:normal\">第"));
            lblPage = new Label();
            lblPage.ID = "lblPage";
            this.Controls.Add(lblPage);
            this.Controls.Add(new LiteralControl("页</span>&nbsp;&nbsp;"));
            btnPrev = new LinkButton();
            btnPrev.Text = "上一页";
            btnPrev.Click += btnPrev_Click;
            btnPrev.ID = "btnPrev";
            this.Controls.Add(btnPrev);
            this.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));
            btnNext = new LinkButton();
            btnNext.Text = "下一页";
            btnNext.ID = "btnNext";
            btnNext.Click += btnNext_Click;
            this.Controls.Add(btnNext);
            this.Controls.Add(new LiteralControl("</h4></div>"));

            this.Controls.Add(new LiteralControl("</div> "));
        }
        void btnNext_Click(object sender, EventArgs e)
        {
            NextPreview();
        }

        void btnPrev_Click(object sender, EventArgs e)
        {
            PrevPreview();
        }

        #region 控件定义
        LinkButton btnNext;
        LinkButton btnPrev;
        Label lblPage;
        HtmlGenericControl divBlogContent;
        #endregion
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
                    qry.RowLimit = TotalCount;
                    string titleQue = "";
                    if (CourseKeyword.Trim().Length > 0)
                    {
                        string[] titles = CourseKeyword.Split(';');
                        foreach (string titel in titles)
                        {
                            titleQue = titleQue + " title:\"" + titel + "\"";
                        }
                    }
                    string queryText = "ContentClass:STS_ListItem_Posts" + titleQue;
                    SPUser spUser = SPContext.Current.Web.CurrentUser;
                    if (LoginUser == 1 && spUser != null)
                    {
                        queryText = "author:" + spUser.Name + " " + queryText;
                    }
                    ////this.Controls.Add(new LiteralControl(queryText));
                    qry.QueryText = queryText;// 
                    qry.SelectProperties.AddRange(new string[] { "WorkId", "Title", "Author", "Created", "Path", "EditorOWSUSER", "HitHighlightedSummary", "ParentLink" });
                    qry.SortList.Add("Created", Microsoft.Office.Server.Search.Query.SortDirection.Descending);
                    SearchExecutor searchExecutor = new SearchExecutor();
                    ResultTableCollection resultTableCollection = searchExecutor.ExecuteQuery(qry);
                    IEnumerator<ResultTable> iResult = resultTableCollection.Filter("TableType", KnownTableTypes.RelevantResults).GetEnumerator();
                    iResult.MoveNext();
                    ResultTable resultTable = iResult.Current;
                    queryDataTable = resultTable.Table;

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
        private void FillBlogContent()
        {
            StringBuilder strContent = new StringBuilder();
            strContent.AppendLine("<ul  class=\"Newblog\">");
            DataTable dt = GetAllBlogs;
            int perPage=PageNumber;
            for (int i = 0; i <perPage  ; i++)
            {
                int rowIndex = CurrentPage * perPage + i;
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
            string photoUrl = GetPhotoUrlByUserProfile(dr["EditorOWSUSER"].ToString());
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
            strContent.AppendLine("<p class=\"Newblog_Time\">" + ((DateTime)dr["Created"]).AddHours(8).ToString("yyyy年MM月dd日 HH:mm:ss") + "</p>");
            strContent.AppendLine("</div>");
            strContent.AppendLine("</div>");
            strContent.AppendLine("</li>");
            return strContent.ToString();
        }
        #endregion

        #region 属性
        string courseKeyword = "HTML5;CSS3";
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("课程关键字")]
        [WebDescription("根据标题包含课程关键字")]
        public string CourseKeyword
        {
            get
            {
                return courseKeyword;
            }

            set
            {
                courseKeyword = value;
            }
        }
        int pageNumber = 15;
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("每页显示的博客数")]
        [WebDescription("每页显示的博客数")]
        public int PageNumber
        {
            get
            {
                return pageNumber;
            }
            set
            {
                pageNumber = value;
            }
        }
        int totalCount = 75;
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("显示的博客总数")]
        [WebDescription("")]
        public int TotalCount
        {
            get
            {
                return totalCount;
            }
            set
            {
                totalCount = value;
            }
        }
        int divWidth = 500;
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("显示的控件的宽度")]
        [WebDescription("")]
        public int ShowdivWidth
        {
            get
            {
                return divWidth;
            }
            set
            {
                divWidth = value;
            }
        }
        int loginUser=0;
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("是否只显示当前用户")]
        [WebDescription("1为当前用户；0为所有用户")]
        public int LoginUser
        {
            get
            {
                return loginUser;
            }
            set
            {
                loginUser = value;
            }
        }
        #endregion
    }
}
