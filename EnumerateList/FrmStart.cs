using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SharePoint;

namespace EnumerateList
{
    public partial class frmStart : Form
    {
        public frmStart()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            string txt = "初审记录.xlsx";
            using (SPSite mySite = new SPSite(txtiteUrl.Text))
            {
                foreach (SPWeb subWeb in mySite.AllWebs)
                    sb.AppendLine(subWeb.Name + "    " + subWeb.ID);
                //using (SPWeb myWeb = mySite.OpenWeb(txtWebUrl.Text))
                //{
                //    foreach (SPList mylist in myWeb.Lists)
                //    {
                //        try
                //        {
                //            sb.AppendLine(mylist.Title + "    " + mylist.TemplateFeatureId + "    " + mylist.BaseTemplate.ToString());

                //        }
                //        catch
                //        {
                //            sb.AppendLine("Error " + mylist.Title);
                //        }

                //    }

                //}
            }
            txtResult.Text = sb.ToString();
        }

        private void frmStart_Load(object sender, EventArgs e)
        {
            SPSite site = new SPSite("http://xqx2012");
            SPWeb mWeb = site.OpenWeb("blog/blog1");
            SPList sList = mWeb.Lists.TryGetList("个人学习助手结果");
            SPQuery qry = new SPQuery();
            SPListItemCollection docITems = null;
            //当前用户创建的文档
            //qry.ViewFields = "<FieldRef Name='ID' /><FieldRef Name='Author' /><FieldRef Name='FileRef' /><FieldRef Name='Title' />";
            //qry.Query = @"<Where><Eq><FieldRef Name='Author' LookupId='True' /><Value Type='Lookup'>" + user.ID + "</Value></Eq></Where>";
            docITems = sList.GetItems(qry);
            DataTable dt = docITems.GetDataTable();
            foreach (SPWeb sWeb in mWeb.Webs )
                Console.Write(sWeb.ServerRelativeUrl );

        }
    }
}
