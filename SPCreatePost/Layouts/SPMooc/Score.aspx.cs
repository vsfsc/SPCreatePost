using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Text;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SPCreatePost.Layouts.SPMooc
{
    public partial class Score:AppBasePage 
    {        
        #region  属性
        private DataSet GetCurrentWorks
        {
            get
            {
                if (ViewState["dsWorks"] == null)
                {
                    DataSet ds = null;// DAL.Works.GetFinalsExpertWillScore(GroupID, LoginID); 
                    ViewState["dsWorks"] = ds;
                }
                return (DataSet)ViewState["dsWorks"];
            }
        }
       
        private DataSet GetFinalsScoreStandard
        {
            get
            {
                if (ViewState["dsStandard"] == null)
                {
                    DataSet ds = null;// DAL.Works.GetFinalsScoreStandard();
                    ViewState["dsStandard"] = ds;
                }
                return (DataSet)ViewState["dsStandard"];
            }
        }
        private int GroupID
        {
            get
            {
                if (ViewState["groupID"] == null)
                {
                    DataTable dt = null;// DAL.Group.GetExpertGroup(ContestID).Tables[0];
                    long  loginID =LoginID;
                    DataRow[] drs = dt.Select("ExpertID=" + loginID); 
                    int groupID=0;
                    if (drs.Length >0)
                        groupID = (int)drs[0]["GroupID"];
                    ViewState["groupID"] = groupID;

                }
                return int.Parse(ViewState["groupID"].ToString() );
            }
      
        }
        #endregion
        #region 事件
        protected void Page_Load(object sender, EventArgs e)
        {
            //可以在页面加载时设置页面的缓存为“SetNoStore()”，即无缓存  

            Response.Cache.SetNoStore();

            //Session中存储的变量“IsSubmit”是标记是否提交成功的  

            if (Convert.ToBoolean(Session["IsSubmit"]))
            {

                //如果表单数据提交成功，就设“Session["IsSubmit"]”为false  

                Session["IsSubmit"] = false;

                //显示提交成功信息  


            }
            if (!Page.IsPostBack)
            {
                string url = SPWeb == null ? "test.aspx" : SPWeb.Url;
                try
                {
                    //专家评分操作
                    if (!IsWebAdmin && !LoginRole.Contains(3))
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script defer>alert('没有权限');top.location.href='" + url  + "'</script>");
                        return;
                    }
                }
                catch
                {
                }
                DataTable dt = GetCurrentWorks.Tables[0];
                if (dt.Rows.Count == 0)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script defer>alert('评分已经结束');top.location.href='" +url  + "'</script>");
                    return;

                }
                ddlNumber.DataSource = dt.DefaultView ;
                ddlNumber.DataTextField = "Number";
                ddlNumber.DataValueField = "WorksID";
                ddlNumber.DataBind();
                ddlNumber_SelectedIndexChanged(null, null);
            }
            ddlNumber.SelectedIndexChanged += new EventHandler(ddlNumber_SelectedIndexChanged);
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
            if (HiddenField1.Value != "")
                lblTotalScore.InnerText = HiddenField1.Value;
            InitControl();
        }

        void ddlNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlNumber.SelectedValue == "") return;
            DataSet ds = null;// DAL.Works.GetWorksByWorksID(long.Parse(ddlNumber.SelectedValue));  
            DataRow dr=ds.Tables[0].Rows[0];
            lblWorksName.Text = dr["WorksName"].ToString() ;
            lblWorksNum.Text = dr["WorksCode"].ToString();
            ddlWorksType.Text = dr["WorksTypeName"].ToString();
        }
        //刷新会提交两次数据
        void btnSubmit_Click(object sender, EventArgs e)
        {
            int total = int.Parse(HiddenField1.Value);
            if (total == 0) return;
            bool result = false;// BLL.Works.InsertWorksExpert(ddlNumber.SelectedValue, new List<object> { LoginID }, new object[] { total, 3 });
            if (result)
            {
                ddlNumber.Items.RemoveAt(ddlNumber.SelectedIndex);
                //ddlNumber_SelectedIndexChanged(null, null);
                //lblTotalScore.InnerText = "";
                //foreach (Control subCtr in divContent.Controls)
                //{
                //    if (subCtr.GetType() == typeof(HtmlInputText))
                //    {
                //        HtmlInputText txtBox = (HtmlInputText)subCtr;
                //        txtBox.Value = "";
                //    }
                //}
                HiddenField1.Value ="0";
                Session["IsSubmit"] = true;

                Response.Redirect("Score.aspx"); 
            }
            else
                Common.ShowMessage(Page, this.GetType(), "提交失败！"); 
        }
        #endregion
        #region 方法
        private void InitControl()
        {
            divContent.Controls.Clear();
            StringBuilder txt;
            DataSet dsStandard = GetFinalsScoreStandard;
            HtmlInputText txtScore;
            foreach (DataRow dr in dsStandard.Tables[0].Rows)
            {
                txt = new StringBuilder();
                txt.AppendLine("<div class=\"mainDiv\"><div class=\"fl\" style=\"margin-left:150px;width:500px\"><p class=\"ptitle\">");
                txt.AppendLine(dr["StandardName"].ToString() + "（" + dr["Score"].ToString() + "）");
                txt.AppendLine("</p><p class=\"pin\" >");
                txt.AppendLine(dr["Description"].ToString());
                txt.AppendLine("</p></div>");
                divContent.Controls.Add(new LiteralControl(txt.ToString()));
                txtScore = new HtmlInputText();
                txtScore.ID = "txtFen" + dr["StandardID"] + "_" + dr["Score"];
                txtScore.Attributes.Add("class", "keyboardInput mt h30");
                txtScore.Attributes.Add("onchange", "CaculateScore()");
                //<input type=\"text\" value=\"\" =\\" />);
                divContent.Controls.Add(txtScore);
                divContent.Controls.Add(new LiteralControl("</div><div style=\"height:20px;\">&nbsp;</div>"));
            }
        }
        #endregion
    }
}
