using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.Office.Server.Search.Administration;

namespace SetRowLimit
{
    class Program
    {
        static void Main(string[] args)
        {
            //SetMaxRowLimit();
            //ReadDoc();
            FilterDuplicate();
        }
        private void ShowField()
        {
            SPSite mySite = new SPSite("http://xqx2012");
        }
        static  void FilterDuplicate()
         {
              DataTable dt = new DataTable("person");
              dt.Columns.Add("name", typeof(string));
              dt.Columns.Add("age", typeof(string));
              dt.Columns.Add("sex", typeof(string));
  
              dt.Rows.Add("makan", "28", "男");
              dt.Rows.Add("makan", "28", "男");
             dt.Rows.Add("zhengrui", "28", "男");
 
             Console.WriteLine("Source DataTable Info");
             outputDt(dt);
 
             string[] distinctcols = new string[(dt.Columns.Count)];
             foreach (DataColumn dc in dt.Columns)
             {
                 distinctcols[dc.Ordinal] = dc.ColumnName;
             }
 
             DataTable dtfd = new DataTable("personFilterDup");
             DataView mydataview = new DataView(dt);
             dtfd = mydataview.ToTable(true, distinctcols);
 
             Console.WriteLine("FilterDuplicate DataTable Info");
            outputDt(dtfd);
            Console.ReadKey();
        }

        static  void outputDt(DataTable dt)
         {
             Console.WriteLine("DataTable Name="+dt.TableName);
            foreach (DataRow dr in dt.Rows)
            {
               foreach (DataColumn dc in dt.Columns)
                 {
                    Console.Write(dc.ColumnName + "=" + dr[dc.Ordinal].ToString() + "\t");              }
                Console.WriteLine("");
           }
        }

        private static void ReadDoc()
        {
            using (SPSite site = new SPSite("http://xqx2012"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList lst = web.Lists["新联"];
                    foreach (SPField myField in lst.Fields)
                    {
                        Console.WriteLine(myField.FieldValueType.ToString() );
                    }
                    //SPDocumentLibrary lstDoc = (SPDocumentLibrary)lst;
                    //SPFile file = lstDoc.RootFolder.Files[0];
                    //string url = file.Url;
                    //SPFile file = web.GetFile(url);
                    //Response.Write(file.Name );
                    //SPFolder folder = web.Folders[file.ParentFolder.Name];
                    //file = folder.Files[file.Name];

                    //var context = WebOperationContext.Current.OutgoingResponse;
                    //context.ContentType = "application/octet-stream";
                    //context.Headers.Add("Content-Disposition", "attachment; filename=" + file.Name  );
                    //Response.Write("header");

                }
            }
        }
        public static void SetMaxRowLimit()
        {
            //SPFarm farm = SPFarm.Local;
            //SearchServiceApplication searchApp = (SearchServiceApplication)farm.Services.
            //    GetValue<SearchQueryAndSiteSettingsService>().Applications.GetValue<SearchServiceApplication>("Search Service 应用程序 1");
            SearchService searchService = SearchService.Service;

            SearchServiceApplication searchApp = searchService.SearchApplications.GetValue<SearchServiceApplication>(new Guid("Search Service 应用程序 1"));
            searchApp.MaxRowLimit = 10000;
            searchApp.Update(); 

        }
    }
}
