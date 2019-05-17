using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace SPLegalAmountField
{
    public class SPLegalAmountFieldEditor : UserControl, IFieldEditor
    {
        //定义编辑字段的两个控件
        protected DropDownList DrRelevanceListField = null;
        protected TextBox txtTextboxWidth = null;
        bool IFieldEditor.DisplayAsNewSection
        {
            get { return false; }
        }
        //编辑字段配置信息绑定到控件上
        void IFieldEditor.InitializeWithField(SPField field)
        {
            if (!Page.IsPostBack)
            {
                this.EnsureChildControls();
                BindSPListFieldData(SPContext.Current.ListId, DrRelevanceListField);
            }
            if (!Page.IsPostBack && field != null)
            {
                SPLegalAmountField fields = (SPLegalAmountField)field;
                this.DrRelevanceListField.SelectedValue = fields.SPLegalAmountFieldRelevanceListField;
                this.txtTextboxWidth.Text = fields.SPLegalAmountFieldTextboxWidth;
            }
        }
        //绑定当前列表的数字字段到下拉控件
        void BindSPListFieldData(Guid listid, DropDownList drlist)
        {
            using (SPSite site = new SPSite(SPContext.Current.Web.Site.ID))
            {
                using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                {
                    drlist.Items.Clear();
                    SPList splist = web.Lists[listid];
                    SPFieldCollection splistfield = splist.Fields;
                    foreach (SPField spfsitem in splistfield)
                    {
                        if (spfsitem.Reorderable)
                        {
                            if (spfsitem.Type == SPFieldType.Number || spfsitem.Type == SPFieldType.Currency)
                            {

                                string _text = spfsitem.Title;
                                string _value = spfsitem.InternalName;
                                System.Web.UI.WebControls.ListItem litem = new System.Web.UI.WebControls.ListItem(_text, _value);
                                drlist.Items.Add(litem);

                            }
                        }
                    }
                }
            }
        }
        //保存配置的值
        void IFieldEditor.OnSaveChange(SPField field, bool isNewField)
        {
            this.EnsureChildControls();
            if (field != null)
            {
                SPLegalAmountField CuserField = (SPLegalAmountField)field;

                CuserField.SPLegalAmountFieldRelevanceListField = DrRelevanceListField.SelectedValue;
                CuserField.SPLegalAmountFieldTextboxWidth = txtTextboxWidth.Text;
            }
        }

        }
    }
