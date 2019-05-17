using System;
using Microsoft.SharePoint;

namespace BlogReply.BlogReplyEventReceiver
{
    /// <summary>
    /// 列表项事件
    /// </summary>
    public class BlogReplyEventReceiver : SPItemEventReceiver
    {
        /// <summary>
        /// 正在添加项.

        public override void ItemAdded(SPItemEventProperties properties)
        {
            //base.ItemAdded(properties);
            SPWeb web = properties.OpenWeb();
            int authorID = 0;
            try
            {
                authorID = web.Author.ID;
                if (web.CurrentUser.ID == web.Author.ID) return;
            }
            catch
            {
                //未找到用户
                if (authorID == 0)
                {
                    foreach (SPUser sUser in web.Users)
                        if (web.CurrentUser.ID == sUser.ID && web.DoesUserHavePermissions(sUser.LoginName, SPBasePermissions.FullMask))
                            return;
                }
            }
            SPListItem item = properties.ListItem;
            SPList lstNewPost = CheckList(web);
            string fieldRefName = "Title";
            foreach (SPField fid in lstNewPost.Fields)
            {
                if (fid.Title == "文章ID")
                {
                    fieldRefName = fid.InternalName;// +";" + fid.StaticName + ";";
                    break;
                }
            }
            SPSecurity.RunWithElevatedPrivileges(delegate()
               {
                   using (SPWeb web1 = new SPSite(web.Site.ID).OpenWeb(web.ID))
                   {
                       SPList lst = web1.Lists[lstNewPost.ID];
                       SPListItem lstItem = null;

                       string id = item["文章 ID"].ToString();
                       id = id.Substring(0, id.IndexOf(";#"));
                       SPQuery query = new SPQuery();
                       query.ViewAttributes = "Scope='RecursiveAll'";
                       query.Query = "<Where><Eq><FieldRef Name='" + fieldRefName + "'/><Value Type='Text'>" + id + "</Value></Eq></Where>";
                       SPListItemCollection items = lstNewPost.GetItems(query);
                       if (items.Count == 0)
                       {
                           lstItem = lst.Items.Add();
                           lstItem["标题"] = item.ID;
                           lstItem["文章ID"] = id;

                       }
                       else
                           lstItem = items[0];

                       lstItem["新回复数"] = Convert.ToInt32(lstItem["新回复数"]) + 1;
                       lstItem.Update();
                   }
               });
        }
        string photoLibaryName = "PostBlog";
        private SPList CheckList(SPWeb web)
        {
            web.AllowUnsafeUpdates = true;
            SPList list = null;
            try
            {
                list = web.Lists[photoLibaryName];
            }
            catch (Exception)
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
    {

        using (SPWeb web1 = new SPSite(web.Site.ID).OpenWeb(web.ID))
        {
            web1.Lists.Add(photoLibaryName, "存放博客新回复", SPListTemplateType.GenericList);
            list = web1.Lists[photoLibaryName];
            list.Fields.Add("文章ID", SPFieldType.Integer, false);
            list.Fields.Add("新回复数", SPFieldType.Integer, false);
            list.Update();
        }
    });
            }
            return list;
        }

    }
}