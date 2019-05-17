using System;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.WebControls;
using System.Security.Permissions;
using System.Collections.Specialized;

namespace SPLegalAmountField
{

    public class SPLegalAmountField : SPFieldMultiColumn
    {
        //创建字段保存创建配置属性：关联的小写字段
        const string SPLegalAmountField_RELEVANCELISTFIELD = "SPLegalAmountFieldRelevanceListField";
        //创建字段保存创建配置属性：呈现出来的大小金额文本框长度
        const string SPLegalAmountField_TEXTBOXWIDTH = "SPLegalAmountFieldTextboxWidth";

        public SPLegalAmountField(SPFieldCollection fields, string fieldname)
            : base(fields, fieldname)
        {
            _splegalAmountFieldRelevanceListField = "" + base.GetCustomProperty(SPLegalAmountField_RELEVANCELISTFIELD);
            _splegalAmountFieldTextboxWidth = "" + base.GetCustomProperty(SPLegalAmountField_TEXTBOXWIDTH);

        }
        public SPLegalAmountField(SPFieldCollection fields, string typeName, string displaydname)
            : base(fields, typeName, displaydname)
        {
            _splegalAmountFieldRelevanceListField = "" + base.GetCustomProperty(SPLegalAmountField_RELEVANCELISTFIELD);
            _splegalAmountFieldTextboxWidth = "" + base.GetCustomProperty(SPLegalAmountField_TEXTBOXWIDTH);

        }
        public override object GetFieldValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;
            return new SPLegalAmountFieldValue(value);
        }


        private string _splegalAmountFieldRelevanceListField;
        private string _splegalAmountFieldTextboxWidth;

        public string SPLegalAmountFieldRelevanceListField
        {
            get
            {
                return _splegalAmountFieldRelevanceListField;
            }
            set
            {
                _splegalAmountFieldRelevanceListField = value;
                this.SetCustomPropertytoCache(SPLegalAmountField_RELEVANCELISTFIELD, value);
            }
        }
        public string SPLegalAmountFieldTextboxWidth
        {
            get
            {
                return _splegalAmountFieldTextboxWidth;
            }
            set
            {
                _splegalAmountFieldTextboxWidth = value;
                this.SetCustomPropertytoCache(SPLegalAmountField_TEXTBOXWIDTH, value);
            }
        }

        private static readonly Dictionary<string, StringDictionary>
            CustomPropertiesCache = new Dictionary<string, StringDictionary>();
        private string ContextKey
        {
            get
            {
                return this.ParentList.ID.ToString() + "_" + System.Web.HttpContext.Current.GetHashCode();
            }
        }
        protected void SetCustomPropertytoCache(string key, string value)
        {
            StringDictionary plist = null;
            if (CustomPropertiesCache.ContainsKey(ContextKey))
            {
                plist = CustomPropertiesCache[ContextKey];
            }
            else
            {
                plist = new StringDictionary();
                CustomPropertiesCache.Add(ContextKey, plist);
            }
            if (plist.ContainsKey(key))
            {
                plist[key] = value;
            }
            else
            {
                plist.Add(key, value);
            }
        }
        protected string GetCustomPropertyFromCache(string key)
        {
            if (CustomPropertiesCache.ContainsKey(ContextKey))
            {
                StringDictionary plist = CustomPropertiesCache[ContextKey];
                if (plist.ContainsKey(key))
                    return plist[key];
                else
                    return "";
            }
            else
            {
                return "";
            }
        }
        public override void OnAdded(SPAddFieldOptions op)
        {
            base.OnAdded(op);
            Update();
        }
        public override void Update()
        {

            base.SetCustomProperty(SPLegalAmountField_RELEVANCELISTFIELD, this.GetCustomPropertyFromCache(SPLegalAmountField_RELEVANCELISTFIELD));
            base.SetCustomProperty(SPLegalAmountField_TEXTBOXWIDTH, this.GetCustomPropertyFromCache(SPLegalAmountField_TEXTBOXWIDTH));

            base.Update();
        }


        public override BaseFieldControl FieldRenderingControl
        {
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]

            get
            {
                BaseFieldControl _renderingControl = new SPLegalAmountFieldControl();
                _renderingControl.FieldName = InternalName;
                return _renderingControl;

            }
        }
        /// <summary>
        /// 提交表单时候的验证数据类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string GetValidatedString(object value)
        {
            string strValue = "" + value;
            if (Required && strValue == "")
            {
                throw new SPFieldValidationException(System.Web.HttpContext.GetGlobalResourceObject("FlowMan.WebControls", "SPLegalAmountField_Required").ToString());
            }

            return base.GetValidatedString(value);
        }
    }
}
