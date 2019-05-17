using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using System.Drawing;

namespace ShowVANews.ShowVANews
{
    [ToolboxItemAttribute(false)]
    public class ShowVANews : WebPart
    {
        #region 事件
        protected override void CreateChildControls()
        {
            if (GetAllBindData != null)
            {
                BindControl();
                if (!Page.IsPostBack)
                    BindGridview();
            }
        }
        void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                //e.Row.Cells[i].Font.Size = 10;
                //if (i == 0 && e.Row.RowType == DataControlRowType.DataRow)
                //    e.Row.Cells[i].Wrap = true;
                //else
                //    e.Row.Cells[i].Wrap = false;
                //if (e.Row.RowType == DataControlRowType.Header)
                //{
                //    e.Row.Cells[i].ForeColor = ColorTranslator.FromHtml("#ffffff");
                //}
            }
        }
        void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                LinkButton Button_IndexNext = new LinkButton();
                LinkButton Button_IndexPrevious = new LinkButton();

                Button_IndexNext.Text = " 下一页";
                Button_IndexNext.CommandName = "next";
                Button_IndexNext.ForeColor = Color.Blue;

                Button_IndexNext.Click += new EventHandler(PageButtonClick);

                Button_IndexPrevious.Text = "上一页 ";
                Button_IndexPrevious.CommandName = "previous";
                Button_IndexPrevious.ForeColor = Color.Blue;
                Button_IndexPrevious.Click += new EventHandler(PageButtonClick);
                //e.Row.Controls[0].Controls[0].Controls[0].Controls[0].Controls.AddAt(0, (Button_IndexFirst));
                e.Row.Controls[0].Controls[0].Controls[0].Controls[0].Controls.AddAt(0, (Button_IndexPrevious));

                int controlTmp = e.Row.Controls[0].Controls[0].Controls[0].Controls.Count - 1;
                e.Row.Controls[0].Controls[0].Controls[0].Controls[controlTmp].Controls.Add(Button_IndexNext);
                //e.Row.Controls[0].Controls[0].Controls[0].Controls[controlTmp].Controls.Add(Button_IndexLast);
                //TableCell tCell = e.Row.Controls[0] as TableCell ;
                //TableCell pagerTCell = tCell.Controls[0].Controls[0].Controls[0] as TableCell ; 
                //foreach (Control ctr in tCell.Controls[0].Controls )
                //    Page.Response.Write(ctr.ID + "   " + ctr.GetType().ToString());
            }
        }
        void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindGridview();
        }

        #endregion
        #region 控件定义
        protected GridView GridView1;
        protected UpdatePanel updatePanel1;
        static SPList myNews;
        #endregion
        #region 方法
        private DataTable GetAllBindData
        {
            get
            {
                if (ViewState["dt"] != null)
                {
                    _gvtable = (DataTable)ViewState["dt"];
                }
                else
                {
                    myNews = SPContext.Current.Web.Lists.TryGetList(NewsListName);
                    if (myNews != null)
                    {
                        SPListItemCollection collListItems = myNews.Items;
                        _gvtable = collListItems.GetDataTable();
                        _gvtable.DefaultView.Sort = "Created desc";
                        ViewState["dt"] = _gvtable;
                    }
                    else
                        _gvtable = null;
                }
                return _gvtable;
            }
        }
        private void SetDataGridColumn()
        {
            GridView1.Columns.Clear();
            ButtonField lnkField = new ButtonField();
            lnkField.ButtonType = ButtonType.Link;
            lnkField.HeaderText = "标题";
            lnkField.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
            lnkField.HeaderStyle.Font.Bold = false;
            lnkField.DataTextField = "Title";
            lnkField.ItemStyle.Wrap = true;
            lnkField.CommandName = "ShowDetails";
            GridView1.Columns.Add(lnkField);
            BoundField bindCol = new BoundField();
           
            if (ShowAdditionFields.Length > 0)
            {
                string[] addFields = showAdditionFields.Split(';');
                foreach (string showName in addFields)
                {
                    string[] interNames = GetInterNameByShowName(showName);
                    if (interNames.Length > 1)
                    {
                        bindCol = new BoundField();
                        bindCol.ReadOnly = true;
                        bindCol.HeaderText = interNames[1];
                        bindCol.DataField = interNames[0];
                        bindCol.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
                        bindCol.HeaderStyle.Font.Bold = false;
                        if (showName == "创建时间")
                            bindCol.DataFormatString = "{0:yyyy-MM-dd HH:mm}";

                        GridView1.Columns.Add(bindCol);
                    }
                }
            }
            GridView1.DataKeyNames = new string[] { "ID" };
        }
        private void BindControl()
        {
            GridView1 = new GridView();
            GridView1.ID = "GridView1";
           
            GridView1.AutoGenerateColumns = false;
            GridView1.GridLines = GridLines.None;
            GridView1.EmptyDataText = "没有要显示的数据！";
            GridView1.CellPadding = 3;
            GridView1.Width = GridWidth ;
            SetDataGridColumn();
            GridView1.PageSize = ListPageSize;
            GridView1.AllowPaging = true;
            GridView1.ShowFooter = false;
            GridView1.PagerSettings.Mode = PagerButtons.NumericFirstLast;
            GridView1.PagerStyle.HorizontalAlign = HorizontalAlign.Center;
            GridView1.PageIndexChanging += GridView1_PageIndexChanging;
            GridView1.RowDataBound += GridView1_RowDataBound;
            GridView1.RowCommand += GridView1_RowCommand;
            GridView1.RowCreated += GridView1_RowCreated;
            CaculatePageCount();
            GridView1.PagerSettings.FirstPageText = "1";
            GridView1.PagerSettings.LastPageText = ViewState["pageCount"].ToString();
            GridView1.HeaderStyle.Wrap = true;
            GridView1.HeaderStyle.Font.Bold = false;
            GridView1.RowStyle.BackColor = ColorTranslator.FromHtml("#E9FAFD");
            GridView1.AlternatingRowStyle.BackColor = ColorTranslator.FromHtml("#ffffff");
            updatePanel1 = new UpdatePanel();
            updatePanel1.ID = "updatePanel1";
            updatePanel1.UpdateMode = UpdatePanelUpdateMode.Conditional;
            updatePanel1.ChildrenAsTriggers = false;
            AsyncPostBackTrigger t = new AsyncPostBackTrigger();
            //t.EventName = "PageIndexChanging";
            t.ControlID = GridView1.ID;
            updatePanel1.Triggers.Add(t);
            updatePanel1.ContentTemplateContainer.Controls.Add(GridView1);
            this.Controls.Add(updatePanel1);
        }
        protected void PageButtonClick(object sender, EventArgs e)
        {
            LinkButton clickedButton = ((LinkButton)sender);
            if (clickedButton.CommandName == "first")
            {
                GridView1.PageIndex = 0;
            }
            else if (clickedButton.CommandName == "next")
            {
                if (GridView1.PageIndex < GridView1.PageCount - 1)
                {
                    GridView1.PageIndex += 1;
                }
            }
            else if (clickedButton.CommandName == "previous")
            {
                if (GridView1.PageIndex >= 1)
                {
                    GridView1.PageIndex -= 1;
                }
            }
            else if (clickedButton.CommandName == "last")
            {
                GridView1.PageIndex = GridView1.PageCount - 1;
            }
            BindGridview();

        } 
        void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ShowDetails")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int listID = Convert.ToInt32(GridView1.DataKeys[index].Value);
                SPListItem myItem = myNews.Items.GetItemById(listID);
                string listitem = myItem.Url.ToString();
                int urlnum = listitem.LastIndexOf("/");
                string urln = listitem.Substring(0, urlnum + 1);
                string url = SPContext.Current.Web.Url + "/" + urln + "DispForm.aspx?ID="+listID ;
                Page.Response.Redirect(url);
            }
        }
        private void CaculatePageCount()
        {
            int totalCount = GetAllBindData.Rows.Count;
            int pageCount = totalCount / this.GridView1.PageSize;
            if (totalCount % this.GridView1.PageSize > 0)
            {
                pageCount = pageCount + 1;
            }
            ViewState["pageCount"] = pageCount;
        }
        private void BindGridview()
        {
            DataView dView=new DataView (GetAllBindData);
            dView.Sort = "Created desc";
            GridView1.DataSource = dView;
            GridView1.DataBind();
            Page.Title = SPContext.Current.Web.Title;
        }
        private string[] GetInterNameByShowName(string showName)
        {
            try
            {
                SPField myField = myNews.Fields.GetField(showName);
                return new string[] { myField.InternalName, myField.Title };
            }
            catch
            {
                return new string[] { "0" };
            }
        }
        #endregion
        #region 属性
        //显示的其它字段名称
        string showAdditionFields = "创建时间;新闻类别;赞数目";
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("显示的其它字段名称")]
        [WebDescription("")]
        public string ShowAdditionFields
        {
            get { return showAdditionFields; }
            set { showAdditionFields = value; }
        }

        //列表名称
        string newsListName = "86的通知";
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("新闻所在的列表名称")]
        [WebDescription("")]
        public string NewsListName
        {
            get { return newsListName; }
            set { newsListName = value; }
        }
//
        int listPageSize = 20;
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("每页显示数目")]
        [WebDescription("")]       
        public int ListPageSize
        {
            get { return listPageSize; }
            set { listPageSize = value; }
        }
        int gWidth = 700;
        [Personalizable]
        [WebBrowsable]
        [WebDisplayName("整个显示部件的宽度")]
        [WebDescription("")]       

        public int GridWidth
        {
            get { return gWidth; }
            set { gWidth = value; }
        }
        ///<summary>       
        ///保存GridView 数据源       
        /// </summary>  
        private DataTable _gvtable;


        #endregion
    }
}
