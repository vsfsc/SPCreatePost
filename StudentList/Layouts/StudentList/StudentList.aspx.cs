using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.DirectoryServices;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using Microsoft.Office.Server.Search.Query;
using Microsoft.Office.Server.Search.Administration;
using System.Collections.Generic;

namespace StudentList.Layouts.StudentList
{
    public partial class StudentList : LayoutsPageBase
    {
        #region 事件
        protected void Page_Load(object sender, EventArgs e)
        {
            SPUser loginUser=SPContext.Current.Web.CurrentUser;
            string listName = "课表";
            trCondition.Style.Add("display", "none");
            if ( loginUser == null)
                errlable.Text = "请先登录";
            else 
            {
                string dispName = loginUser.Name;
                SPList oList = SPContext.Current.Web.Lists[listName];
                SPQuery oQuery = new SPQuery();
                oQuery.ViewAttributes = "Scope='RecursiveAll'";
                oQuery.Query = "<Where><Eq><FieldRef Name='Title'/><Value Type='Text'>" + dispName + "</Value></Eq></Where>";
                SPListItemCollection lstItems = oList.GetItems(oQuery);
                if (lstItems.Count > 0)
                {
                    errlable.Text = "";
                    trCondition.Style.Add("display", "");
                    bjLists= lstItems[0]["上课班级"].ToString ();
                    ViewState["OUName"] = lstItems[0]["OUName"];
                    dateFrom.SelectedDate = DateTime.Today.AddHours(-24);
                    DateTo.SelectedDate = DateTime.Today;
                    BindControl(GridView1);
                    BindControl(GridView2);
                    SetDataGridColumn(GridView2);
                    BindGrid();
                }
                else
                    errlable.Text = "对不起！您没权限查看学生学习情况";

            }
        }
        void DataGrid1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex;
            BindGrid2();
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
        #region 数据绑定
        private void BindGrid()
        {
            DataTable dt=DTStudents.Copy();
            GridView1.DataSource =dt.Copy ()  ;
            GridView1.DataBind();
            startQuery();
        }
        private void startQuery()
        {
            try
            {
                string txtFrom = dateFrom.SelectedDate.ToString("yyyy-MM-dd");
                string txtTo = DateTo.SelectedDate.ToString("yyyy-MM-dd");//.AddDays(1)
                if (dateFrom.SelectedDate.Year == 9999)
                {
                    dateFrom.Focus();
                    return;
                }
                if (dateFrom.SelectedDate.Year == 9999)
                {
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
                GetStudentUpdate(txtFrom , txtTo );
                CaculatePageCount();
                GridView2.PagerSettings.LastPageText = ViewState["pageCount"].ToString();
                BindGrid2();
            }
            catch (Exception ex)
            {
                errlable.Text = ex.ToString ();
            }
        }
        private void BindGrid2()
        {
            GridView2.DataSource = _gvtable.Copy() ;
            GridView2.DataKeyNames = new string[] { "WorkId" };
            GridView2.DataBind();
        }
        private void BindControl(GridView GridView1)
        {
            GridView1.AutoGenerateColumns = false;
            GridView1.CellPadding = 1;
            GridView1.ShowFooter = true;
            GridView1.RowDataBound += new GridViewRowEventHandler(GridView1_RowDataBound);
            GridView1.HeaderStyle.BackColor = ColorTranslator.FromHtml("#3A81BF");
            GridView1.FooterStyle.BackColor = ColorTranslator.FromHtml("#3A81BF");
            GridView1.HeaderStyle.ForeColor = ColorTranslator.FromHtml("#ffffff");
            GridView1.RowStyle.BackColor = ColorTranslator.FromHtml("#E9FAFD");
            GridView1.AlternatingRowStyle.BackColor = ColorTranslator.FromHtml("#ffffff");
            if (GridView1.ID == "GridView2")
            {
                GridView1.AllowPaging = true;
                GridView1.PageSize = ListPageSize;
                GridView1.PagerSettings.Mode = PagerButtons.NumericFirstLast;
                GridView1.PagerStyle.HorizontalAlign = HorizontalAlign.Center;
                GridView1.PageIndexChanging += new GridViewPageEventHandler(DataGrid1_PageIndexChanging);
                GridView1.RowDataBound += new GridViewRowEventHandler(GridView1_RowDataBound);
                GridView1.PagerSettings.FirstPageText = "1";

                errlable.Font.Size = 10;
                errlable.Font.Bold = true;
                btnSearch.BorderStyle = BorderStyle.Groove;
                btnSearch.Click += new EventHandler(btnSearch_Click);
            }
        }
       void btnSearch_Click(object sender, EventArgs e)
        {
            startQuery();
        }        
        #endregion
 
        #region 方法
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
        string bjLists ;
        string ouName;
        private string[] getBanJi()
        {
            bjLists = bjLists.Replace("；", ";");
            bjLists = bjLists.Replace(" ","");
            if (bjLists.EndsWith(";"))
                bjLists = bjLists.Substring(0, bjLists.Length - 1);
            string[] bjs = bjLists.Split(';');
            return bjs;
        }
        private DataTable GetAllStudents()
        {
            string _subADPath = "LDAP://OU=东北大学本科生,DC=ccc,DC=neu,DC=edu,DC=cn";
            if (ViewState["OUName"] == null)
                _subADPath = "LDAP://OU=东北大学本科生,DC=ccc,DC=neu,DC=edu,DC=cn";
            else
                _subADPath = ViewState["OUName"].ToString();
            string[] bjs = getBanJi();
            DataTable dt = new DataTable();
            HyperLinkField lnkField;
            lstBanJi.Items.Clear();
            lstBanJi.Items.Add("全部");
            foreach (string subBj in bjs)
            {
                lstBanJi.Items.Add(subBj);
                dt.Columns.Add(subBj);
                dt.Columns.Add(subBj+"blog");
                lnkField = new HyperLinkField();
                lnkField.HeaderText = subBj;
                lnkField.DataTextField = subBj;
                lnkField.DataNavigateUrlFormatString = "{0}";
                lnkField.DataNavigateUrlFields = new string[] { subBj + "blog" };
                GridView1.Columns.Add(lnkField);
            }
            int totalStudents = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                    string bjAdPath;
                    foreach (string subBj in bjs)
                    {
                        bjAdPath = GetClassEntry(_subADPath, subBj);
                        if (bjAdPath != "")
                        { 
                           totalStudents += GetStudentByClass(bjAdPath, ref dt, subBj);
                        }
                    }
            });
            StudentCount = totalStudents;
            return dt;
        }
        //班级下面的学生,返回班级人数
        private int GetStudentByClass(string adPath,ref DataTable dt,string bjName)
        {
            using (DirectoryEntry de = new DirectoryEntry(adPath))
            {
                using (DirectorySearcher search = new DirectorySearcher())
                {
                    search.SearchRoot = de;
                    search.Filter = "objectClass=user";
                    search.SearchScope = SearchScope.Subtree;
                    SearchResultCollection results = search.FindAll();
                    string userName;
                    string xueHao;
                    string colName;
                    int i = 0;
                    int dtCount = dt.Rows.Count;
                    foreach (SearchResult sr in results)
                    {
                        userName = sr.Properties["displayName"][0].ToString();
                        xueHao = sr.Properties["sAMAccountName"][0].ToString();
                        DataRow dr;
                        if (dtCount > i)
                            dr = dt.Rows[i];//[bjName] = userName;
                        else
                        {
                            dr = dt.NewRow();
                            dt.Rows.Add(dr);
                        }
                        dr[bjName] = userName;
                        colName = bjName + "blog";
                        dr[colName] = "/personal/" + xueHao+"/Blog";
                        i = i + 1;
                    }
                    dt.AcceptChanges();
                    return results.Count;
                }
            }
        }
        /// <summary>
        /// 返回班级路径
        /// </summary>
        /// <param name="ouName"></param>
        /// <returns></returns>
        private string GetClassEntry(string adPath, string className)
        {
            using (DirectoryEntry de = new DirectoryEntry(adPath))
            {
                using (DirectorySearcher search = new DirectorySearcher())
                {
                    search.SearchRoot = de;
                    search.Filter = "(&(objectClass=organizationalUnit)(OU=" + className + "))";
                    search.SearchScope = SearchScope.Subtree;
                    SearchResult results = search.FindOne();
                    if (results == null)
                        return "";
                    else
                        return (results.Path);
                }
            }
        }
        #endregion
        #region 属性
        private DataTable DTStudents
        {
            get
            {
                if (ViewState["dtcourse"] == null)
                {
                    ViewState["dtcourse"] = GetAllStudents();
                }
                return (DataTable)ViewState["dtcourse"];
            }
        }
        #endregion
        #region 当天更新
        private static int totalStudents;

        public static int StudentCount
        {
            set { totalStudents = value; }
            get { return totalStudents; }
        }
        int pageSize =20;
        public int ListPageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }
        private void GetStudentUpdate(string txtFrom, string txtTo)
        {
            DataTable queryDataTable = FullQuery(txtFrom, txtTo);
            DataTable distinctTable = queryDataTable;// QueryByBanJi(ref queryDataTable);
            if (distinctTable.Rows.Count > 0)
            {
                DataRow[] drs = distinctTable.Select("", "Author");
                DataSet ds = new DataSet();
                ds.Merge(drs);
                _gvtable = ds.Tables[0].Copy();
            }
            else
                _gvtable = distinctTable;
            errlable.Text = "共有学生" + StudentCount.ToString () + "人，共更新 " + _gvtable.Rows.Count + " 条记录！";
        }
        ///保存GridView 数据源       
        /// </summary>  
        private static DataTable _gvtable;
        private string GetFullQueryString()
        {
            DataTable dtStudent = DTStudents;
            string sql = "";
            foreach (DataRow dr in dtStudent.Rows)
            {
                int j = 0;
                if (lstBanJi.SelectedIndex == 0)//全部班级
                {
                    for (int i = 1; i <= dtStudent.Columns.Count / 2; i++)
                    {
                        if (!dr.IsNull(j) && dr[j].ToString() != "")
                        {
                            if (sql != "")
                                sql = sql + " Author:" + dr[j];
                            else
                                sql = "Author:" + dr[j];
                        }
                        j += 2;
                    }
                }
                else//按指定班级查
                {
                    if (!dr.IsNull(lstBanJi.SelectedItem.Text) && dr[lstBanJi.SelectedItem.Text].ToString() != "")
                    {
                        if (sql != "")
                            sql = sql + " Author:" + dr[lstBanJi.SelectedItem.Text];
                        else
                            sql = "Author:" + dr[lstBanJi.SelectedItem.Text];
                    }
                }
            }
            return sql;
        }
        private DataTable FullQuery(string txtFrom, string txtTo)
        {
            SPServiceContext context = SPServiceContext.Current;
            SearchServiceApplicationProxy ssap = (SearchServiceApplicationProxy)SearchServiceApplicationProxy.GetProxy(SPServiceContext.GetContext(SPContext.Current.Site));
            using (KeywordQuery qry = new KeywordQuery(ssap))
            {
                qry.EnableStemming = true;
                qry.TrimDuplicates = true;
                qry.RowLimit = 10000;
                string queryText = GetFullQueryString();
                string txtTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                txtTime = txtTime.Substring(txtTime.IndexOf(" "));
                qry.QueryText =   "Created:" + txtFrom + ".." + txtTo+" " +queryText;
                qry.SelectProperties.AddRange(new string[] { "WorkId", "Title", "Author", "Created", "Path", "ContentClass", "FileExtension" });
                qry.SortList.Add("Created", Microsoft.Office.Server.Search.Query.SortDirection.Descending);
                SearchExecutor searchExecutor = new SearchExecutor();
                ResultTableCollection resultTableCollection = searchExecutor.ExecuteQuery(qry);
                IEnumerator<ResultTable> iResult = resultTableCollection.Filter("TableType", KnownTableTypes.RelevantResults).GetEnumerator();
                iResult.MoveNext();
                ResultTable resultTable = iResult.Current;
                DataTable queryDataTable = resultTable.Table;
                if (queryDataTable.Rows.Count > 0)
                {
                   foreach (DataRow dr in queryDataTable.Rows)
                    {
                        //小时加8
                        dr["Created"] = ((DateTime)dr["Created"]).AddHours(8);
                        string author = dr["Author"].ToString();
                        if (author.IndexOf(";") > 0)//多个作者，修改者也加到了里面
                        {
                            dr["Author"] = author.Substring(0, author.IndexOf(";"));
                        }
                    }
                    queryDataTable.AcceptChanges();
                    //当天查询减去24小时
                    if (dateFrom.SelectedDate == DateTime.Today.AddDays(-1) && DateTo.SelectedDate == DateTime.Today)
                    {
                        DataRow[] drs = queryDataTable.Select("Created>='" + txtFrom + txtTime + "' and Created<='" + DateTo.SelectedDate.ToString("yyyy-MM-dd") + txtTime + "'", "Created desc");
                        DataSet ds = new DataSet();
                        DataTable dt = queryDataTable.Clone();
                        ds.Tables.Add(dt);
                        ds.Merge(drs);
                        queryDataTable = ds.Tables[0];
                    }
                }
                return queryDataTable;
            }
        }
        private DataTable QueryByBanJi(ref DataTable queryDataTable)
        {
            DataTable distinctTable = queryDataTable.Clone();
            DataTable dtStudent = DTStudents;
            errlable.Text = "";
            if (queryDataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in dtStudent.Rows)
                {
                    int j = 1;
                    string sql = "";
                    string strTj;
                    if (lstBanJi.SelectedIndex == 0)//全部班级
                    {
                        for (int i = 1; i <= dtStudent.Columns.Count / 2; i++)
                        {
                            if (!dr.IsNull(j) && dr[j].ToString() != "")
                            {
                                strTj = dr[j].ToString();
                                if (sql != "")
                                    sql = sql + " or Path like '%" + strTj + "%'";
                                else
                                    sql = "Path like '%" + strTj + "%'";
                            }
                            j += 2;
                        }
                    }
                    else//按指定班级查
                    {
                        if (!dr.IsNull(lstBanJi.SelectedItem.Text) && dr[lstBanJi.SelectedItem.Text].ToString() != "")
                        {
                            strTj = dr[lstBanJi.SelectedItem.Text + "Blog"].ToString();
                            if (sql != "")
                                sql = sql + " or Path like '%" + strTj + "%'";
                            else
                                sql = "Path like '%" + strTj + "%'";
                        }
                        else
                            sql = "";
                    }
                    if (sql != "")
                    {
                        DataRow[] drs = queryDataTable.Select(sql);
                        errlable.Text += sql + "<br>";
                        foreach (DataRow dr1 in drs)
                        {
                            dr1["Created"] = ((DateTime)dr1["Created"]).AddHours(8);
                            distinctTable.Rows.Add(dr1.ItemArray);
                        }
                    }
                }
            }
            queryDataTable.AcceptChanges();
            return distinctTable;
        }
        /// <summary>
        /// 动态绑定项
        /// </summary>
        private void SetDataGridColumn(GridView GridView1)
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
        }

        #endregion
    }
}
