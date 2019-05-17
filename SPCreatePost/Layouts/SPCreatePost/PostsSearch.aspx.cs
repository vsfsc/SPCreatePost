using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.Office.Server;
using Microsoft.Office.Server.Search;
using Microsoft.Office.Server.Search.Query;
using Microsoft.Office.Server.Search.Administration;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
namespace SPCreatePost.Layouts.SPCreatePost
{
    public partial class PostsSearch : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindControl();
            if (!Page.IsPostBack)
            {
                ViewState["pageTitle"] = Page.Title; 
                //加载用户列表
                dateFrom.SelectedDate = DateTime.Today;
                DateTo.SelectedDate = DateTime.Today;
                BeginSearch();
            }
        }
        #region 使用的方法
        private void BeginSearch()
        {
            try
            {
                string txtFrom = dateFrom.SelectedDate.ToString("yyyy-MM-dd");
                string txtTo = DateTo.SelectedDate.AddDays(1).ToString("yyyy-MM-dd");
                if (dateFrom.SelectedDate.Year ==9999)
                {
                    //errlable.Text = "开始时间日期格式错误";
                    dateFrom.Focus();
                    return;
                }
                if (dateFrom.SelectedDate.Year == 9999)
                {
                    //errlable.Text = "截止时间日期格式错误";
                    DateTo.Focus();
                    return;
                }
                if (dateFrom.SelectedDate > DateTime.Today)
                {
                    dateFrom.SelectedDate = DateTime.Today;
                    return;
                }
                if (DateTo.SelectedDate > DateTime.Today)
                {
                    DateTo.SelectedDate = DateTime.Today;
                    DateTo.Focus();
                    return;
                }
                if (dateFrom.SelectedDate > DateTo.SelectedDate)
                {
                    errlable.Text = "开始时间不能大于截止时间";
                    dateFrom.Focus();
                    return;

                }
                FullQuery(txtFrom, txtTo);
                CaculatePageCount();
                GridView1.PagerSettings.LastPageText = ViewState["pageCount"].ToString();
                BindGrid();
            }
            catch
            {
                errlable.Text = "";
            }
        }
        /// <summary>
        /// 加载控件
        /// </summary>
        private void BindControl()
        {
            GridView1.AutoGenerateColumns = false;
            GridView1.CellPadding = 1;
            SetDataGridColumn();
            GridView1.AllowPaging = true;
            GridView1.ShowFooter = true;
            GridView1.PageSize = ListPageSize;
            GridView1.PagerSettings.Mode = PagerButtons.NumericFirstLast;
            GridView1.PagerStyle.HorizontalAlign = HorizontalAlign.Center;
            GridView1.PageIndexChanging += new GridViewPageEventHandler(DataGrid1_PageIndexChanging);
            GridView1.RowDataBound += new GridViewRowEventHandler(GridView1_RowDataBound);
            GridView1.PagerSettings.FirstPageText = "1";
            GridView1.HeaderStyle.Wrap = true;
            GridView1.RowStyle.Wrap = true;
            GridView1.HeaderStyle.BackColor = ColorTranslator.FromHtml("#3A81BF");
            GridView1.FooterStyle.BackColor = ColorTranslator.FromHtml("#3A81BF");
            GridView1.HeaderStyle.ForeColor = ColorTranslator.FromHtml("#ffffff");
            GridView1.RowStyle.BackColor = ColorTranslator.FromHtml("#E9FAFD");
            GridView1.AlternatingRowStyle.BackColor = ColorTranslator.FromHtml("#ffffff");
            errlable.Font.Size = 10;
            errlable.Font.Bold = true;
            btnSearch.BorderStyle = BorderStyle.Groove;
            btnSearch.Click += new EventHandler(btnSearch_Click);
           
        }
        void btnSearch_Click(object sender, EventArgs e)
        {

            BeginSearch();
            //Common.OpenWindow(Page, "MyNewsfeed.aspx");
        }

        private void FullQuery(string txtFrom ,string txtTo)
        {
                SPServiceContext context = SPServiceContext.Current;// ServerContext.Current;//ServerContext.GetContext
                SearchServiceApplicationProxy ssap = (SearchServiceApplicationProxy)SearchServiceApplicationProxy.GetProxy(SPServiceContext.GetContext(SPContext.Current.Site));
                using (KeywordQuery qry = new KeywordQuery(ssap))
                {
                    qry.EnableStemming = true;
                    qry.TrimDuplicates = true;
                    qry.RowLimit = 500;
                    string queryText = "";
                    //获取id和显示名称
                    if (userID.ResolvedEntities.Count > 0)
                    {
                        string name = ((PickerEntity)userID.ResolvedEntities[0]).DisplayText;
                        queryText = "Author:" + name + " "; ;// userID.CommaSeparatedAccounts.Replace(";", "") + " ";
                    }
                    else
                    {
                        queryText = "-Author:系统帐户 -Author:administrator ";
                    }
                    qry.QueryText = queryText + "Created:" + txtFrom + ".." + txtTo;
                    qry.SelectProperties.AddRange(new string[] { "WorkId", "Title", "Author", "Created", "Path", "ContentClass", "FileExtension" });
                    qry.SortList.Add("Created", Microsoft.Office.Server.Search.Query.SortDirection.Descending);
                    SearchExecutor searchExecutor = new SearchExecutor();
                    ResultTableCollection resultTableCollection = searchExecutor.ExecuteQuery(qry);
                    IEnumerator<ResultTable> iResult = resultTableCollection.Filter("TableType", KnownTableTypes.RelevantResults).GetEnumerator();
                    iResult.MoveNext();
                    ResultTable resultTable = iResult.Current;
                    DataTable queryDataTable = resultTable.Table;
                    DataTable distinctTable = queryDataTable.Clone();
                    foreach (DataRow dr in queryDataTable.Rows)
                    {
                        //小时加8
                        dr["Created"] = ((DateTime)dr["Created"]).AddHours(8);
                        switch (dr["ContentClass"].ToString())
                        {
                            case "STS_ListItem_TasksWithTimelineAndHierarchy":
                                dr["ContentClass"] = "任务";
                                break;
                            case "STS_ListItem_GenericList":
                                dr["ContentClass"] = "自定义列表";
                                break;
                            case "STS_ListItem_Posts":
                                dr["ContentClass"] = "博客文章";
                                break;
                            case "STS_ListItem_851"://Asset Library / Video Channel
                                dr["ContentClass"] = "资产库";
                                break;
                            case "STS_ListItem_DocumentLibrary":
                                dr["ContentClass"] = "文档库";
                                break;
                            case "STS_ListItem_MySiteDocumentLibrary":
                                dr["ContentClass"] = "个人网站文档库";
                                break;
                            case "STS_Site":
                                dr["ContentClass"] = "网站集";
                                break;
                            case "STS_ListItem_Announcements":
                                dr["ContentClass"] = "通知新闻";
                                break;
                            case "STS_ListItem_PictureLibrary":
                                dr["ContentClass"] = "图片库";
                                break;
                            case "STS_ListItem_Comments":
                                dr["ContentClass"] = "博客评论";
                                break;
                            case "STS_ListItem_Categories":
                                dr["ContentClass"] = "博客类别";
                                break;
                            case "STS_Web":
                                dr["ContentClass"] = "网站";
                                break;
                            case "STS_ListItem_544"://MicroBlogList (MicroFeed)
                                dr["ContentClass"] = "新闻源";
                                break;
                            case "STS_ListItem_Survey":
                                dr["ContentClass"] = "调查";
                                break;
                        }
                        string author = dr["Author"].ToString();
                        if (author.IndexOf(";") > 0)//多个作者
                        {
                            dr["Author"] = author.Substring(0, author.IndexOf(";"));
                        }
                        string url = dr["Path"].ToString();
                        try
                        {
                            dr["Path"] = url.Substring(url.IndexOf("/", 7));
                        }
                        catch { }
                        DataRow[] drs = distinctTable.Select("Path='"+dr["Path"]+"'");
                        if (drs.Length == 0)
                            distinctTable.Rows.Add(dr.ItemArray);
                    }
                    queryDataTable.AcceptChanges();
                    _gvtable = distinctTable.Copy() ;
                    errlable.Text = "本次查询共 " + _gvtable.Rows.Count + " 条数据！";
                }
        }
        /// <summary>
        /// 动态绑定项
        /// </summary>
        private void SetDataGridColumn()
        {
            GridView1.Columns.Clear();
            HyperLinkField lnkField = new HyperLinkField();
            lnkField.HeaderText = "标题";
            lnkField.DataTextField = "Title";
            lnkField.DataNavigateUrlFormatString = "{0}";
            lnkField.ItemStyle.Wrap = true;
            lnkField.DataNavigateUrlFields = new string[] { "Path" };
            GridView1.Columns.Add(lnkField);
            BoundField bindCol = new BoundField();
            bindCol = new BoundField();
            bindCol.ReadOnly = true;
            bindCol.HeaderText = "作者";
            bindCol.DataField = "Author";
            GridView1.Columns.Add(bindCol);

            bindCol = new BoundField();
            bindCol.ReadOnly = true;
            bindCol.HeaderText = "创建时间";
            bindCol.DataField = "Created";
            GridView1.Columns.Add(bindCol);

            bindCol = new BoundField();
            bindCol.ReadOnly = true;
            bindCol.HeaderText = "内容类型";
            bindCol.DataField = "ContentClass";
            GridView1.Columns.Add(bindCol);
        }
        /// <summary>
        /// 绑定数据，并设置当前页的索引
        /// </summary>
        private void BindGrid()
        {
            try
            {
                GridView1.DataSource = _gvtable;
                GridView1.DataKeyNames = new string[] { "WorkId" };
                GridView1.DataBind();
            }
            catch
            {
            }
        }
        private void CaculatePageCount()
        {
            int totalCount = _gvtable.Rows.Count;
            int pageCount = totalCount / this.GridView1.PageSize;
            if (totalCount % this.GridView1.PageSize > 0)
            {
                pageCount = pageCount + 1;
            }
            ViewState["pageCount"] = pageCount;
        }

        #endregion
        #region 事件
        void DataGrid1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Font.Size = 10;
                if (i == 0 && e.Row.RowType == DataControlRowType.DataRow)
                    e.Row.Cells[i].Wrap = true;
                else
                    e.Row.Cells[i].Wrap = false;
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[i].ForeColor = ColorTranslator.FromHtml("#ffffff");
                }

            }
        }
        #endregion
        #region 属性
        int pageSize = 20;
        public int ListPageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }
        int gWidth = 700;
        public int GridWidth
        {
            get { return gWidth; }
            set { gWidth = value; }
        }
        ///<summary>       
        ///保存GridView 数据源       
        /// </summary>  
        private static DataTable _gvtable;

        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }
        #endregion

        public SearchServiceApplicationProxy context { get; set; }
    }
}
