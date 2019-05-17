using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using System.Drawing;
using System.IO;

namespace WorkEvaluate.Layouts.WorkEvaluate
{
    public partial class test : UnsecuredLayoutsPageBase
    {
        protected HtmlGenericControl divUsers;
        protected HtmlGenericControl divTest;
        
        protected Button Button1;
        protected HtmlGenericControl showMsg;
        protected TextBox TextBox1;
        protected GridView GridView1;
        protected PeopleEditor userID;
        protected ListBox ListBox1;
      

        private void ShowNewsField()
        {
            SPWeb web= SPContext.Current.Web;
            SPList myNews =web.Lists["讨论列表"];
            foreach (SPListItem item in myNews.Items)
            {
                SPAttachmentCollection attach = item.Attachments;

                for (int i = 0; i < attach.Count; i++)
                {

                    String url = attach.UrlPrefix + attach[i];

                    Console.WriteLine("正在下载{0}...", url);

                    SPFile file = web.GetFile(url);

                    FileStream fs = new FileStream(file.Name, FileMode.Create);

                    byte[] content = file.OpenBinary();

                    fs.Write(content, 0, content.Length);

                    fs.Close();

                }
            }
            //获取第一个discussion的主题
            foreach (SPListItem myFoleder in myNews.Folders)
            {
               
                Response.Write("Sethem:" +myFoleder.Name);
                Response.Write(myFoleder["Created"] + "---" + myFoleder["Author"] + "<br/>");
                foreach (SPListItem myItem in myFoleder.ListItems)
                {
                    Response.Write("Replay:"+myItem.Name+"--" +myItem["Title"] +  "<br/>");
                    Response.Write(myItem["Created"] + "---" + myItem["Author"] + "<br/>");
                }
            }
            //第一个主题，第一个回复的内容
            //Response.Write(myNews.Items[1]);
            //foreach (SPListItem myItem in myNews.Items)
            //{
            //    Response.Write(myItem["Created"] + "---" + myItem["Author"] + "<br/>");
            //    Response.Write(myItem["Title"] + "---" + myItem["LastReplyBy"] + "<br/>");
            //}
            foreach (SPField myField in myNews.Fields)
            {
                if (myField.InternalName =="Author")
                Response.Write(myField.InternalName + "<--->"+ myField.TypeDisplayName+ "---" +myField.TypeAsString +"----" +myField.Title);
            }
            //SPListItemCollection collListItems = myNews.Items;
            //GridView1.DataSource = collListItems.GetDataTable();
            //GridView1.DataBind();
        }
        private void SetDataGridColumn()
        {
            GridView1.Columns.Clear();
            HyperLinkField lnkField = new HyperLinkField();
            lnkField.HeaderText = "标题";
            lnkField.DataTextField = "Title";
            lnkField.DataNavigateUrlFormatString = "{0}";
            lnkField.ItemStyle.Wrap = true;
            //lnkField.DataNavigateUrlFields = new string[] { "Path" };
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
        private void BindControl()
        {
            GridView1.AutoGenerateColumns = false;
            GridView1.CellPadding = 1;
            GridView1.Width = 800;
            SetDataGridColumn();
            GridView1.PageSize = ListPageSize;
            GridView1.AllowPaging = true;
            GridView1.ShowFooter = true;
            GridView1.PagerSettings.Mode = PagerButtons.NumericFirstLast;
            GridView1.PagerStyle.HorizontalAlign = HorizontalAlign.Center;
            GridView1.PageIndexChanging += GridView1_PageIndexChanging;
            GridView1.RowDataBound += GridView1_RowDataBound;
            GridView1.PagerSettings.FirstPageText = "1";
            GridView1.HeaderStyle.Wrap = true;
            GridView1.RowStyle.Wrap = true;
            GridView1.HeaderStyle.BackColor = ColorTranslator.FromHtml("#3A81BF");
            GridView1.FooterStyle.BackColor = ColorTranslator.FromHtml("#3A81BF");
            GridView1.HeaderStyle.ForeColor = ColorTranslator.FromHtml("#ffffff");
            GridView1.RowStyle.BackColor = ColorTranslator.FromHtml("#E9FAFD");
            GridView1.AlternatingRowStyle.BackColor = ColorTranslator.FromHtml("#ffffff");

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
        private void BindDiv()
        {
            string txt = "<ul><li>通过动态加按钮来实现编辑和删除</li></ul>";
            divTest.Controls.Add(new LiteralControl(txt));
            Button btn = new Button();
            btn.Text = "动态生成删除";
            btn.Click += btn_Click;
            btn.CommandArgument = "id";
            divTest.Controls.Add(btn);
            divTest.Controls.Add(new LiteralControl("动态删除数据测试")); 

            
        }
        protected override bool AllowAnonymousAccess
        {
            get
            {
                return true;
            }
        }
        void btn_Click(object sender, EventArgs e)
        {
            //Response.Redirect("ChangePassword.aspx");//  跳转到新增活动页面

            divTest.Controls.RemoveAt(1);
        }
        private void testUrl()
        {
            string str = "http%3A%2F%2Fxqx2012%2Fblog%2FLists%2FList32%2FAllItems%2Easpx&ContentTypeId=0x0100588905E4EF91BF428BDE39647168CE34";
            string res = Server.UrlDecode(str);

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            testUrl();
            BindDiv();
            //BindControl();
            //ShowNewsField();
            //return;
            ////    Response.Write(SPContext.Current.Web.Url  );
            ////    return;
            //CaculatePageCount();
            //GridView1.PagerSettings.FirstPageText = "1";
            //GridView1.PagerSettings.LastPageText = ViewState["pageCount"].ToString();

            //BindGridview();
            //TextBox1.Text = GridView1.PageCount.ToString();
            ////Response.Write(dr["id"].ToString()  + dt.Rows.Count);

            //dt.Rows.Add(dr);
            //Response.Write("qqqq"+dt.Rows.Count);

            Button1.Click += Button1_Click;
            //HtmlIframe iframe = new HtmlIframe();
            //iframe.Src = "http://va.neu.edu.cn/_layouts/15/WopiFrame.aspx?sourcedoc=/Shared%20Documents/%e6%95%b0%e5%ad%97%e5%8c%96%e5%ad%a6%e4%b9%a0.ppt";
            //iframe.Style.Add("Width", "1000");
            //iframe.Style.Add("Height", "500");

            //////iframe.Style.Add("Width", "500");
            ////this.Controls.Add(iframe);
            //divUsers.Controls.Add(iframe);
            //string txt="<iframe src='http://va.neu.edu.cn/_layouts/15/WopiFrame.aspx?sourcedoc=/Shared%20Documents/%e6%95%b0%e5%ad%97%e5%8c%96%e5%ad%";
            //txt += "a6%e4%b9%a0.ppt' width='600px' height='470px' frameborder='0'>这是嵌入 <a target='_blank' href='http://office.com'>Microsoft ";
            //txt+="Office</a> 文档，";
            //txt+="由 <a target='_blank' href='http://office.com/webapps'>Office Web Apps</a> 支持。</iframe>";
            //string txt = "<iframe src='http://www.baidu.com'  width='600px' height='470px'></iframe>";
            //divUsers.Controls.Add(new LiteralControl(txt));

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

        private DataTable gridTable;
        private DataTable GetAllBindData
        {
            get
            {

                if (ViewState["dt"] != null)
                {
                    gridTable = (DataTable)ViewState["dt"];
                }
                else
                {
                    DataTable dt = new DataTable();
                    SPList myNews = SPContext.Current.Web.Lists["86的通知"];
                    //foreach (SPField myField in myNews.Fields)
                    //{
                    //    Response.Write(myField.InternalName + "     " + myNews.Items[0][myField.InternalName]);
                    //}
                    SPListItemCollection collListItems = myNews.Items;
                    dt = collListItems.GetDataTable();
                    ViewState["dt"] = dt;
                    //dt.Columns.Add("id");
                    //DataRow dr;
                    //for (int i = 0; i < 200; i++)
                    //{
                    //    dr = dt.NewRow();
                    //    dr["id"] = i;
                    //    dt.Rows.Add(dr);

                    //}

                    gridTable = dt;
                }
                return gridTable;
            }
        }
        private void BindGridview()
        {
            GridView1.DataSource = GetAllBindData;
            GridView1.DataBind();

        }
        void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindGridview();
        }

        void Button1_Click(object sender, EventArgs e)
        {
            Response.Write(TextBox1.Text.Length);
            Response.Write(TextBox1.Text.IndexOf("\n"));
            showMsg.InnerText = TextBox1.Text.Replace("\n", "<br/>");
            return;
            //string Content = TextBox1.Text.Replace("\n", "<br>");//将回车替换成html中的换行标记

            //Content = Content.Replace(" ", "&nbsp;&nbsp;");//将空格替换了，为了保证空格在中文状态下显示正常，用了两个&nbsp;
            showMsg.InnerText = userID.Accounts[0].ToString();
            //showMsg.InnerText += "<br>" + userID.Entities[0] 
            showMsg.InnerHtml += "<br/>" + ((PickerEntity)userID.Entities[0]).EntityData.Keys;
            showMsg.InnerHtml += "<br/>DisplayText:" + ((PickerEntity)userID.ResolvedEntities[0]).DisplayText;
            showMsg.InnerHtml += "<br/>Key:" + ((PickerEntity)userID.ResolvedEntities[0]).Key;
            Hashtable key1 = ((PickerEntity)userID.Entities[0]).EntityData;
            IEnumerator myEnumerator = key1.Keys.GetEnumerator();
            int i = 1;
            while (myEnumerator.MoveNext())
            {
                object obj = myEnumerator.Current;
                showMsg.InnerHtml += "<br/>" + i.ToString() + ": " + obj.ToString();
                i += 1;
                //if (obj.GetType().FullName == "System.Web.UI.WebControls.Panel")
                //{
                //    //Panel pnl = (Panel)obj;
                //    //DropDownList ddl = (DropDownList)pnl.FindControl(pnl.ID + "_ddlLM");
                //    //if (ddl == null) continue;
                //    //Response.Write(pnl.ID + "-" + ddl.SelectedValue + "<br>");
                //}
            }
            i = 1;
            myEnumerator = key1.Values.GetEnumerator();
            while (myEnumerator.MoveNext())
            {
                object obj = myEnumerator.Current;
                showMsg.InnerHtml += "<br/>" + i.ToString() + ": " + obj.ToString();
                i += 1;
            }
        }
        //        DisplayText:薛清侠
        //Key:i:0#.w|ccc\xueqingxia

        //        1: Title
        //2: MobilePhone
        //3: SIPAddress     
        //4: Department
        //5: Email
        //1: 
        //2: 
        //3: xueqingxia@CCC.NEU.EDU.CN
        //4: 
        //5: xueqingxia@ccc.neu.edu.cn
        private void InitUserControls(int ctrCount)
        {

        }
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

        protected void Button2_Click(object sender, EventArgs e)
        {
            SPSite site = new SPSite("http://xqx2012");
            SPWeb web = site.AllWebs["ComputerCompetitive"];//
            SPList myList = web.Lists["mytask"];
            //注册事件处理程序
            myList.EventReceivers.Add( SPEventReceiverType.ItemUpdated, "TaskEventHandler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=4f5a25a745ef7f27", "TaskEventHandler.TaskEventReceiver");
         
            myList.Update();
           
            ListBox1.Items.Clear();
            ListBox1.Items.Add("baseType-" + myList.BaseType.ToString());
            ListBox1.Items.Add("GetType-" + myList.GetType().ToString());
            foreach (SPEventReceiverDefinition myEvet in myList.EventReceivers)
                ListBox1.Items.Add(myEvet.Name);
        }

    }
}
