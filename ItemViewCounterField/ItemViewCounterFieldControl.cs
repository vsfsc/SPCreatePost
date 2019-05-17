using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Web.UI;

namespace ItemViewCounterField
{
    public class ItemViewCounterFieldControl : NumberField
    {
        // Methods
        protected override void Render(HtmlTextWriter output)
        {
            int cCounter;
            if (this.ItemFieldValue == null)
            {
                cCounter = 1;
            }
            else
            {
                cCounter = Convert.ToInt32(this.ItemFieldValue) + 1;
            }
            this.ItemFieldValue = cCounter.ToString();
            this.UpdateWithElevatedPrivileges();
            if (this.ItemFieldValue == null)
            {
                output.Write("0");
            }
            else
            {
                output.Write(this.ItemFieldValue.ToString());
            }
        }

        private void UpdateWithElevatedPrivileges()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        web.AllowUnsafeUpdates = true;
                        SPList list = web.Lists[base.ListId];
                        SPListItem item = list.GetItemById(base.ItemId);
                        if ((list.BaseType == SPBaseType.DocumentLibrary) || (item == null))
                        {
                            this.ItemFieldValue = (Convert.ToInt32(this.ItemFieldValue) - 1).ToString();
                        }
                        else
                        {
                            item[base.FieldName] = this.ItemFieldValue;
                            item.SystemUpdate();
                        }
                    }
                }
            });
        }

    }
}
