using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.Office.Server;
using Microsoft.Office.Server.Search;
using Microsoft.Office.Server.Search.Query;
using Microsoft.Office.Server.Search.Administration  ;

namespace SpWebpart.WebStatistics
{
    [ToolboxItemAttribute(false)]
    public class WebStatistics : WebPart
    {
        #region  事件  
        protected override void CreateChildControls()
        {

            base.CreateChildControls();
            if (!Page.IsPostBack)
            {
                CreateAccessTotal();
                CreateAccessDetail();
            }
            if (CurrentSelectedUser != "匿名用户")
                SaveUsageData();
            StatisticCount();
            InitControl();
        }
        #endregion
        #region 属性
        private string CurrentSelectedUser
        {
            get
            {
                // look for current selected user in ViewState
                object currentSelectedUser = this.ViewState["_CurrentSelectedUser"];
                if (currentSelectedUser == null)
                {
                    string name = Context.User.Identity.Name;
                    if ( string.IsNullOrEmpty( name))
                        name = "匿名用户";
                    else
                    {
                        SPUser spUser = SPControl.GetContextWeb(Context).CurrentUser;
                        name = spUser.Name;
                    }
                    this.ViewState["_CurrentSelectedUser"] = name;
                    return name;	// default to showing the first page
                }
                else
                    return (string)currentSelectedUser;
            }

            set
            {
                this.ViewState["_CurrentSelectedUser"] = value;
            }
        }
        private string CurrentIP
        {
            get
            {
                // look for current selected user in ViewState
                object currentIP = this.ViewState["_CurrentIP"];
                if (currentIP == null)
                {
                    this.ViewState["_CurrentIP"] = HttpContext.Current.Request.UserHostAddress;
                    return HttpContext.Current.Request.UserHostAddress;	// default to showing the first page
                }
                else
                    return (string)currentIP;
            }

            set
            {
                this.ViewState["_CurrentIP"] = value;
            }
        }
       
        #endregion
        
        #region 方法
        private void InitControl()
        {
            Table tbl = new Table();
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            tc.Text = "总访问次数：";
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = TotalCount.ToString();
            tr.Cells.Add(tc);
            tbl.Rows.Add(tr);

            tr = new TableRow();
            tc = new TableCell();
            tc.Text = "当天访问次数：";
            tr.Cells.Add(tc);
            tc = new TableCell();
            tc.Text = TodayCount.ToString();
            tr.Cells.Add(tc);
            tbl.Rows.Add(tr);

            tr = new TableRow();
            tc = new TableCell();
            tc.Text = "最近七天发贴量：";
            tr.Cells.Add(tc);
            tc = new TableCell();
            int count = FullQuery(DateTime.Today.AddDays(-6).ToString("yyyy-MM-dd"), DateTime.Today.ToString("yyyy-MM-dd"));
            tc.Text = count.ToString();
            tr.Cells.Add(tc);
            tbl.Rows.Add(tr);

            tr = new TableRow();
            tc = new TableCell();
            tc.Text = "当天发贴量：";
            tr.Cells.Add(tc);
            tc = new TableCell();
            count = FullQuery(DateTime.Today.ToString("yyyy-MM-dd"), DateTime.Today.ToString("yyyy-MM-dd"));
            tc.Text = count.ToString();
            tr.Cells.Add(tc);
            tbl.Rows.Add(tr);
            this.Controls.Add(tbl);

        }
        private int FullQuery(string txtFrom, string txtTo)
        {
            SPServiceContext context = SPServiceContext.Current;// ServerContext.Current;//ServerContext.GetContext
            SearchServiceApplicationProxy ssap = (SearchServiceApplicationProxy)SearchServiceApplicationProxy.GetProxy(SPServiceContext.GetContext(SPContext.Current.Site));
            using (KeywordQuery qry = new KeywordQuery(ssap))
            {
                qry.EnableStemming = true;
                qry.TrimDuplicates = true;
                qry.RowLimit = 10000;
                string queryText = "";
                //queryText = "-Author:系统帐户 -Author:administrator ";
                qry.QueryText = queryText + "Created:" + txtFrom + ".." + txtTo;
                qry.SelectProperties.AddRange(new string[] { "WorkId", "Title", "Author", "Created", "Path", "ContentClass", "FileExtension" });
                qry.SortList.Add("Created", Microsoft.Office.Server.Search.Query.SortDirection.Descending);
                SearchExecutor searchExecutor = new SearchExecutor();
                ResultTableCollection resultTableCollection = searchExecutor.ExecuteQuery(qry);
                IEnumerator<ResultTable> iResult = resultTableCollection.Filter("TableType", KnownTableTypes.RelevantResults).GetEnumerator();
                iResult.MoveNext();
                ResultTable resultTable = iResult.Current;
                DataTable queryDataTable = resultTable.Table;
                return queryDataTable.Rows.Count;

            }
        }
        /// <summary>
        /// 统计次数
        /// </summary>
        private void StatisticCount()
        {
            //string queryStr = "SELECT totalCount,todayCount FROM " + accessTotal ;
            Guid webID = SPContext.Current.Web.ID;

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite mySite = new SPSite(SPContext.Current.Site.Url))
                {

                    using (SPWeb thisWeb = mySite.AllWebs[webID])
                    {
                        try
                        {
                            SPList list = thisWeb.Lists[accessTotal ];
                            SPQuery query = new SPQuery();
                            SPListItemCollection items = list.GetItems(query); 
                            TotalCount = int.Parse (items[0]["TotalCount"].ToString());
                            TodayCount = int.Parse (items[0]["TodayCount"].ToString());
                        }
                        catch
                        {
                        }
                    }
                }
            });
          
        }
        /// <summary>
        /// 保存站点统计信息
        /// </summary>
        private void SaveUsageData()
        {
            Guid webID = SPContext.Current.Web.ID;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite mySite = new SPSite(SPContext.Current.Site.Url))
                {

                    using (SPWeb thisWeb = mySite.AllWebs[webID])
                    {
                        thisWeb.AllowUnsafeUpdates = true;
                        try
                        {
                            try
                            {
                                SPList list = thisWeb.Lists[accessDetail];

                                SPListItem lstItem;
                                int lstID = VoteExist();
                                //IP和时间进行访问
                                if (lstID == 0)
                                {
                                    lstItem = list.Items.Add();
                                    lstItem["Title"] = CurrentSelectedUser;
                                    lstItem["AccessIP"] = CurrentIP;
                                    lstItem["AccessTime"] = DateTime.Today.ToString("yyyy-MM-dd");
                                    lstItem.Update();
                                    //'更新汇总
                                    list = thisWeb.Lists[accessTotal];
                                    if (list.ItemCount == 0)
                                    {
                                        lstItem = list.Items.Add();
                                        lstItem["Title"] = DateTime.Today.ToString("yyyy-MM-dd");
                                        lstItem["TodayCount"] = 1;
                                        lstItem["TotalCount"] = 1;
                                    }
                                    else
                                    {
                                        lstItem = list.Items[0];
                                        string time = lstItem["Title"].ToString();
                                        if (DateTime.Today.ToString("yyyy-MM-dd") == time)
                                            lstItem["TodayCount"] = Convert.ToInt32(lstItem["TodayCount"]) + 1;
                                        else
                                        {
                                            lstItem["Title"] = DateTime.Today.ToString("yyyy-MM-dd");
                                            lstItem["TodayCount"] = 1;
                                        }
                                        lstItem["TotalCount"] = Convert.ToInt32(lstItem["TotalCount"]) + 1;
                                    }
                                    lstItem.Update();
                                }
                            }
                            catch
                            {
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int VoteExist()
        {
            SPWeb web = SPContext.Current.Web;
            //string queryStr = "SELECT ID, 用户名,访问IP,创建时间,访问时间 FROM " + HistoryList + " where 访问IP='" + CurrentIP + "' and 访问时间='" + DateTime.Today.ToString("yyyy-MM-dd") + "' and 用户名='"+CurrentSelectedUser  +"'";
            SPList list = web.Lists[accessDetail];
            SPQuery query=new SPQuery ();
            query.ViewAttributes = "Scope='RecursiveAll'";
            query.Query = "<Where><And><And><Eq><FieldRef Name='Title'/><Value Type='Text'>" + CurrentSelectedUser + "</Value></Eq><Eq><FieldRef Name='AccessIP'/><Value Type='Text'>" + CurrentIP + "</Value></Eq></And><Eq><FieldRef Name='AccessTime'/><Value Type='Text'>" + DateTime.Today.ToString("yyyy-MM-dd") + "</Value></Eq></And></Where>";
            SPListItemCollection items = list.GetItems(query); 
            if (items.Count > 0)
                return items.Count;//items[0].ID;
            else
                return 0;
        }
        private void CreateAccessDetail()
        {
            Guid webID = SPContext.Current.Web.ID;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite mySite = new SPSite(SPContext.Current.Site.Url))
                {

                    using (SPWeb web = mySite.AllWebs[webID])
                    {
                        web.AllowUnsafeUpdates = true;
                        SPList list = web.Lists.TryGetList(accessDetail);
                        if (list == null)
                        {
                            Guid newListGuid = web.Lists.Add(accessDetail, "访问明细", SPListTemplateType.GenericList);
                            list = web.Lists[newListGuid]; //取得刚才添加的List.
                            //给TestSaleStaff这个List添加一个普通文本类型的字段.
                            SPField fldName = (SPFieldText)list.Fields.CreateNewField(SPFieldType.Text.ToString(), "AccessIP");
                            fldName.Description = "AccessIP";
                            fldName.Required = true; //在新建项目时,此字段是否是必填的.
                            list.Fields.Add(fldName);

                            fldName = (SPFieldText)list.Fields.CreateNewField(SPFieldType.Text.ToString(), "AccessTime");
                            fldName.Description = "AccessTime";
                            fldName.Required = true; //在新建项目时,此字段是否是必填的.
                            list.Fields.Add(fldName);

                            list.Update();
                            web.AllowUnsafeUpdates = false;
                        }

                    }
                }
            });
         }
        
        private void CreateAccessTotal()
        {
            Guid webID = SPContext.Current.Web.ID;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite mySite = new SPSite(SPContext.Current.Site.Url))
                {

                    using (SPWeb web = mySite.AllWebs[webID])
                    {
                        SPList newList = web.Lists.TryGetList(accessTotal);
                        if (newList == null)
                        {
                            web.AllowUnsafeUpdates = true;
                            Guid newListGuid = web.Lists.Add(accessTotal, "访问汇总的内容", SPListTemplateType.GenericList);
                            newList = web.Lists[newListGuid]; //取得刚才添加的List.
                            //给TestSaleStaff这个List添加一个普通文本类型的字段.
                            SPField fldName = (SPFieldNumber)newList.Fields.CreateNewField(SPFieldType.Number.ToString(), "TotalCount");
                            fldName.Description = "TotalCount";
                            fldName.Required = true; //在新建项目时,此字段是否是必填的.
                            newList.Fields.Add(fldName);

                            fldName = (SPFieldNumber)newList.Fields.CreateNewField(SPFieldType.Number.ToString(), "TodayCount");
                            fldName.Description = "TodayCount";
                            fldName.Required = true; //在新建项目时,此字段是否是必填的.
                            newList.Fields.Add(fldName);
                            newList.Update();
                            web.AllowUnsafeUpdates = false;
                        }
                    }
                }

            });
        }
        #endregion
        #region 属性
        /// <summary>
        /// 网站的总访问次数
        /// </summary>
        private int totalCount=0;
        private int TotalCount
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
        /// <summary>
        /// 网站的当天访问次数
        /// </summary>
        private int todayCount=0;
        private int TodayCount
        {
            get
            {
                return todayCount;
            }
            set
            {
                todayCount = value;
            }
        }
        //列表名称
        string accessDetail = "访问明细";
        //[Personalizable]
        //[WebBrowsable]
        //[WebDisplayName("明细")]
        //[WebDescription("用来存放明细")]
        //public string AccessDetail
        //{
        //    get { return accessDetail; }
        //    set { accessDetail = value; }
        //}
        //列表名称
        string accessTotal = "统计汇总";
        //[Personalizable]
        //[WebBrowsable]
        //[WebDisplayName("站点统计的汇总信息")]
        //[WebDescription("用来存放统计信息")]
        //public string HistoryDetail
        //{
        //    get { return historyDetail; }
        //    set { historyDetail = value; }
        //}
        #endregion
      
    }
}
