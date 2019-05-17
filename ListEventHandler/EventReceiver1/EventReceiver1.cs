using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.Text.RegularExpressions;

namespace ListEventHandler.EventReceiver1
{
    struct Activity
    {
        public string name;
        public DateTime start;
        public int during;
        public string aType;
        public string aPosition;
        public string aObject;
        public string aDescption;
        public string aWorksUrl;
        public SPFieldUserValueCollection author;
        public SPFieldLookupValue task;
    };
    /// <summary>
    /// 列表项事件
    /// </summary>
    public class EventReceiver1 : SPItemEventReceiver
    {
        /// <summary>
        /// 数据写入活动列表，只写一次.
        /// </summary>
        private void WriteToList(Guid siteID, Guid webID, string lstTitle, Activity myActivity)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
          {
              using (SPWeb oWebsite = new SPSite(siteID).AllWebs[webID])
              {
                  oWebsite.AllowUnsafeUpdates = true;
                  try
                  {
                      SPList oList = oWebsite.Lists.TryGetList(lstTitle);
                      string fTaskID = oList.Fields.GetField("计划ID").InternalName;
                      if (ActivityIsExits(oList, fTaskID, myActivity.task.LookupValue) == 1) return;
                      string fType = oList.Fields.GetField("活动类型").InternalName;
                      string fDiDian = oList.Fields.GetField("活动地点").InternalName;
                      string fObject = oList.Fields.GetField("活动对象").InternalName;
                      string fStart = oList.Fields.GetField("开始时间").InternalName;
                      string fDuration = oList.Fields.GetField("持续时长").InternalName;
                      string fAuthor = oList.Fields.GetField("执行者").InternalName;
                      string fDesc = oList.Fields.GetField("活动描述").InternalName;

                      SPListItemCollection collItems = oList.Items;
                      SPListItem item = collItems.Add();
                      item["Title"] = myActivity.name;
                      item[fStart] = myActivity.start;
                      item[fDuration] = myActivity.during;
                      item[fDiDian] = myActivity.aPosition;
                      item[fType] = myActivity.aType;
                      item[fObject] = myActivity.aObject;
                      item[fDesc] = myActivity.aDescption;
                      item[fAuthor] = myActivity.author;
                      item[fTaskID] = myActivity.task;

                      item.Update();
                  }
                  catch
                  {
                  }
                  oWebsite.AllowUnsafeUpdates = false;
              }
          });
        }
        private int ActivityIsExits(SPList oList, string fName, string fValue)
        {
            SPQuery objSPQuery = new SPQuery();
            string queryString = String.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Lookup'>{1}</Value></Eq></Where>", fName, fValue);
            objSPQuery.Query = queryString;
            SPListItemCollection objItems = oList.GetItems(objSPQuery);
            return objItems.Count;

        }
        private void CreateActivityList(Guid siteID, Guid webID, string lstTitle, string desc, string lookupListTitle)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPWeb oWebsite = new SPSite(siteID).AllWebs[webID])
                {
                    oWebsite.AllowUnsafeUpdates = true;
                    try
                    {
                        oWebsite.Lists.Add(lstTitle, desc, SPListTemplateType.GenericList);

                        SPList list = oWebsite.Lists[lstTitle];

                        SPField myField = list.Fields.GetFieldByInternalName("Title");
                        myField.Title = "活动名称";
                        myField.Update();

                        list.Fields.Add("活动类型", SPFieldType.Text, false);

                        list.Fields.Add("活动对象", SPFieldType.Choice, false);
                        SPFieldChoice spChoice=list.Fields["活动对象"] as SPFieldChoice;
                        spChoice.FillInChoice = true;
                        spChoice.Choices.Add("教材");
                        spChoice.Choices.Add("编程");
                        spChoice.DefaultValue = spChoice.Choices[0]; 
                        spChoice.Update();

                        string fieldName = "计划ID"; //新增的Lookup类型字段的名字
                        SPList lookupList = oWebsite.Lists[lookupListTitle]; //设置这个Lookup类型字段要从哪个List中去取值
                        Guid lookupGuid = new Guid(lookupList.ID.ToString());// 取得这个Lookup数据源List的Guid
                        list.Fields.AddLookup(fieldName, lookupGuid, false);  //把上面取得的参数引入到AddLookup方法中，从而创建一个Lookup字段
                        SPFieldLookup splookup = list.Fields[fieldName] as SPFieldLookup; //绑定数据List到Lookup字段
                        splookup.LookupField = "Title";
                        splookup.Update();

                        SPView defaultView = list.Views[0];

                        defaultView.ViewFields.Add(list.Fields["活动类型"]);
                        defaultView.ViewFields.Add(list.Fields["活动对象"]);
                        
                        defaultView.ViewFields.Add(list.Fields["计划ID"]);
                        defaultView.Update();
                        list.Update();
                    }
                    catch
                    {
                    }
                    oWebsite.AllowUnsafeUpdates = false;
                }
            });

        }
     
        /// <summary>
        /// 如果当前网站下不存在活动这个列表，则不进行操作
        /// </summary>
        /// <param name="properties"></param>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
            SPList activ = properties.Web.Lists.TryGetList("活动1");
            if (activ == null)
            {
                CreateActivityList(properties.SiteId, properties.Web.ID, "活动1", "计划生成活动", properties.List.Title);
            }
            return;

            //获取字段内部名
            string nameField = properties.List.Fields.GetField("完成百分比").InternalName;

            string fType = properties.List.Fields.GetField("活动类型").InternalName;
            string fDiDian = properties.List.Fields.GetField("活动地点").InternalName;
            string fObject = properties.List.Fields.GetField("活动对象").InternalName;
            string fStart = properties.List.Fields.GetField("开始日期").InternalName;
            string fEnd = properties.List.Fields.GetField("截止日期").InternalName;
            string fAssigned = properties.List.Fields.GetField("分配对象").InternalName;
            string fDesc = properties.List.Fields.GetField("说明").InternalName;
            if (properties.ListItem["ParentID"] != null)
            {// sub level
                if (properties.ListItem[nameField].ToString() == "1")
                {
                    string taskName = properties.ListItem["Title"].ToString();
                    //外键
                    SPFieldLookupValue taskID = new SPFieldLookupValue(properties.ListItem.ID, taskName);

                    Activity myActivity = new Activity();
                    myActivity.name = taskName;
                    myActivity.aType = properties.ListItem[fType].ToString();
                    myActivity.aPosition = properties.ListItem[fDiDian].ToString();
                    myActivity.aObject = properties.ListItem[fObject].ToString();
                    myActivity.start = DateTime.Parse(properties.ListItem[fStart].ToString());
                    DateTime end = DateTime.Parse(properties.ListItem[fEnd].ToString());
                    myActivity.during = GetDuring(myActivity.start, end);
                    SPFieldUserValueCollection assignedTo = (SPFieldUserValueCollection)properties.ListItem[fAssigned];
                    myActivity.author = assignedTo;
                    myActivity.task = taskID;
                    myActivity.aDescption = properties.ListItem[fDesc].ToString();
                    WriteToList(properties.SiteId, properties.OpenWeb().ID, activ.Title, myActivity);
                }
            }
        }
        //返回持续时间
        /// <summary>
        /// 当前事件每到结束时间，用当前事件减去开始时间；否则用结束时间减去开始时间；取最短时间
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private int GetDuring(DateTime start, DateTime end)
        {
            if (DateTime.Now.Subtract(end).TotalMinutes < 0)
                return (int)DateTime.Now.Subtract(start).TotalMinutes;
            else
                return (int)end.Subtract(start).TotalMinutes;
        }
        /// <summary>
        /// 已更新项.
        /// </summary>
        public void ItemUpdatedbak(SPItemEventProperties properties)
        {
            string nameField = properties.List.Fields.GetField("完成百分比").InternalName;
            string fType = properties.List.Fields.GetField("活动类型").InternalName;
            string fDiDian = properties.List.Fields.GetField("活动地点").InternalName;
            string fObject = properties.List.Fields.GetField("活动对象").InternalName;
            string fStart = properties.List.Fields.GetField("开始日期").InternalName;
            string fEnd = properties.List.Fields.GetField("截止日期").InternalName;
            string fAssigned = properties.List.Fields.GetField("截止日期").InternalName;
            if (properties.ListItem["ParentID"] != null)
            {// sub level
                if (properties.ListItem[nameField].ToString() == "1")
                {
                    string taskName = properties.ListItem["Title"].ToString();
                    string typeName = properties.ListItem[fType].ToString();
                    string diDianName = properties.ListItem[fDiDian].ToString();
                    string objName = properties.ListItem[fObject].ToString();
                    DateTime start = DateTime.Parse(properties.ListItem[fStart].ToString());
                    DateTime end = DateTime.Parse(properties.ListItem[fEnd].ToString());
                    SPUser[] assignedTo = (SPUser[])properties.ListItem[fAssigned];
                    SPFieldLookupValue taskID = new SPFieldLookupValue(properties.ListItem.ID, taskName);
                }
            }
            string fDoc = properties.List.Fields.GetField("文档对象").InternalName;//查阅项

            SPField f1 = properties.List.Fields.GetFieldByInternalName(fDoc);

            SPFieldLookup field = f1 as SPFieldLookup;
            string lookList = field.LookupList;
            SPListItem item = properties.ListItem;
            SPFieldLookupValue fValue = new SPFieldLookupValue(item[fDoc].ToString());
            string[] choices = Regex.Split(fValue.ToString(), ";#");
            string docUrl = GetDocUrl(properties.WebUrl, field.LookupWebId, field.LookupList, fValue.LookupId.ToString());
            this.EventFiringEnabled = false;
            item["Title"] = properties.Web.Url;// +docUrl;// fValue.LookupId + "\\" + item[fDoc] + fValue.LookupValue;
            item.Update();
            this.EventFiringEnabled = true;
            //using (SPWeb targetSite = new SPSite(properties.WebUrl).AllWebs[field.LookupWebId])
            //{
            //    // Get the name of the list where this field gets information.
            //    SPList targetList = targetSite.Lists[new Guid(field.LookupList)];

            //    // Get the name of the field where this field gets information.
            //    SPField targetField = targetList.Fields.GetFieldByInternalName(field.LookupField);
            //    //this.EventFiringEnabled = false;
            //    //item["Title"] = targetList.Title + "\\" + item[fDoc] + targetField.Title;
            //    //item.Update();
            //    //this.EventFiringEnabled = true;
            //}
        }
        /// <summary>
        /// 获取查阅项的url链接
        /// </summary>
        /// <param name="siteURl">网站集url</param>
        /// <param name="lookupWebID">网站Id</param>
        /// <param name="lookupListID">列表Id</param>
        /// <param name="docID">列表项Id</param>
        /// <returns></returns>
        private string GetDocUrl(string siteURl, Guid lookupWebID, string lookupListID, string docID)
        {
            using (SPWeb targetSite = new SPSite(siteURl).AllWebs[lookupWebID])
            {
                // Get the name of the list where this field gets information.
                SPList oList = targetSite.Lists[new Guid(lookupListID)];
                SPQuery objSPQuery = new SPQuery();
                string queryString = String.Format("<Where><Eq><FieldRef Name='{0}' /><Value Type='Counter'>{1}</Value></Eq></Where>", "ID", docID);
                objSPQuery.Query = queryString;
                SPListItemCollection objItems = oList.GetItems(objSPQuery);
                if (objItems.Count > 0)
                    return objItems[0].Url;
                else
                    return "";
            }
        }
    }
}