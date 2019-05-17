using System;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.Office.Server.Search.Administration;

namespace SPCreatePost.Layouts.SPMooc
{
    public partial class test : System.Web.UI.Page
    {
        protected Button Button1;
        protected void Page_Load(object sender, EventArgs e)
        {
//            SPSecurity.RunWithElevatedPrivileges(delegate()
//{
//    SearchService searchService = SearchService.Service;

//    SearchServiceApplication searchApp = searchService.SearchApplications.GetValue<SearchServiceApplication>("Search Service 应用程序 1");
//    searchApp.MaxRowLimit = 10000;

//    searchApp.Update(true);
//});
            Button1.Click += Button1_Click;


        }

        void Button1_Click(object sender, EventArgs e)
        {
            Common.OpenWindow(Page, "MyNewsfeed.aspx");
        }
    }
}
