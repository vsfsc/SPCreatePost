using System;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SPCreatePost.Layouts.SPCreatePost
{
    public partial class refresh : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text = DateTime.Now.ToString();
            this.Button1.Attributes.Add("onclick", "javascript:document.getElementById('doing').style.visibility='visible';");
            //this.Button1.OnClientClick = "javascript:document.getElementById('divSaveAs')innerHTML='正在进行，请稍等……'";

        }
        override protected void OnPreRender(EventArgs e)
        {
            doing.Style.Add("visibility", "hidden");
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            divSaveAs.InnerHtml = "正在进行，请稍等……";
            Label2.Text = DateTime.Now.ToString() + " 正在进行，请稍等……";
            DateTime t1 = DateTime.Now;
            ////TextBox1.Text = DateTime.Now.ToString();
            ////ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "alert", "alert('ok');", true);
            ////UpdatePanel1.Update();//使用Update()方法一定要将UpdateMode改为Conditional，否则报错&nbsp;
            System.Threading.Thread.Sleep(2000);
            Label2.Text = DateTime.Now.Subtract(t1).Seconds.ToString() + "共用时";
            divSaveAs.InnerHtml = "finish";
        }
        //在ajax中的UpdatePanel弹出对话窗，可以使用：

        //ScriptManager.RegisterStartupScript(ScriptManager, this.GetType(), "alert", "alert('更新成功!')", true);

//修改后跳到另一个页面中去时，可以使用：
//ScriptManager.RegisterStartupScript(UpdatePanel1, this.GetType(), "click", "location.replace('UserManger.aspx');", true);

//如果跳转前还有提示信息的话，则可以使用：

//ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "click", "alert('更新成功!');location.replace('UserManger.aspx');", true);

 

//例如：ScriptManager.RegisterStartupScript(this.UpdatePanel1,this.GetType(), "提示", "alert('购物车为空,请先购物!')", true);   
 

// protected void UpdatePanelAlert(string str_Message)
//        {
//            ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.GetType(), "提示", "alert('" + str_Message + "')", true);
//        }

// UpdatePanelAlert("无此代码");
    }
}
