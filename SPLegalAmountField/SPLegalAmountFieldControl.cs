using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SPLegalAmountField
{
    public class SPLegalAmountFieldControl : BaseFieldControl
    {
        //定义表单呈现状态时的控件
        protected TextBox txtSPLegalAmountField;
        protected HiddenField hidSPLegalAmountField;
        protected HiddenField hidSPLegalAmountFieldPropery;
        protected Label LabSPLegalAmountField;
        protected HiddenField LabhidSPLegalAmountField;




        protected override string DefaultTemplateName
        {
            get
            {
                //下面的用户控件名,<SharePoint:RenderingTemplate> 控件的ID 需要等于这个值 
                return "SPLegalAmountFieldControl";
            }
        }
        public override string DisplayTemplateName
        {
            get
            {
                return "SPLegalAmountFieldControlDisplay";
            }

        }

        //取值与赋值
        public override object Value
        {
            get
            {
                EnsureChildControls();
                SPLegalAmountFieldValue fieldValue = new SPLegalAmountFieldValue();



                SPLegalAmountField field = (SPLegalAmountField)base.Field;
                FormField txtAmountLower = GetCurrentFormFieldControl((Control)this.Page, field.SPLegalAmountFieldRelevanceListField);

                if (txtAmountLower == null || txtAmountLower.Value == null)
                {
                    if (SPContext.Current.Item[field.SPLegalAmountFieldRelevanceListField] != null)
                    {
                        SPFieldType filetype = SPContext.Current.Item.Fields.GetFieldByInternalName(field.SPLegalAmountFieldRelevanceListField).Type;
                        SPFieldCalculated filecal = (SPFieldCalculated)SPContext.Current.Item.Fields.GetFieldByInternalName(field.SPLegalAmountFieldRelevanceListField);
                        string fieldCalculatedValue = filecal.GetFieldValueAsText(SPContext.Current.Item[field.SPLegalAmountFieldRelevanceListField]);

                        double amount = Convert.ToDouble(fieldCalculatedValue);

                        fieldValue.AmountCapital = new RMBCapitalization().RMBAmount(amount);
                        fieldValue.AmountNumber = fieldCalculatedValue;
                    }
                    else
                    {
                        fieldValue.AmountCapital = txtSPLegalAmountField.Text;
                        fieldValue.AmountNumber = hidSPLegalAmountField.Value;
                    }

                }
                else
                {
                    fieldValue.AmountCapital = txtSPLegalAmountField.Text;
                    fieldValue.AmountNumber = hidSPLegalAmountField.Value;
                }


                return fieldValue;
            }
            set
            {
                EnsureChildControls();
                SPLegalAmountFieldValue fieldValue = (SPLegalAmountFieldValue)value;
                if (LabSPLegalAmountField != null && fieldValue != null)
                {
                    LabSPLegalAmountField.Text = fieldValue.AmountCapital;
                    LabhidSPLegalAmountField.Value = fieldValue.AmountNumber;



                }
                else if (txtSPLegalAmountField != null && fieldValue != null)
                {
                    txtSPLegalAmountField.Text = fieldValue.AmountCapital;
                    hidSPLegalAmountField.Value = fieldValue.AmountNumber;

                    SPLegalAmountField field = (SPLegalAmountField)Field;
                    hidSPLegalAmountFieldPropery.Value = field.SPLegalAmountFieldRelevanceListField;
                }
                base.Value = fieldValue;
            }
        }

       
        public override void Focus()
        {

            EnsureChildControls();
            // txtCurrentUserDepart.Focus();
        }

        protected override void CreateChildControls()
        {
            if (Field == null) return;

            if (this.ControlMode == SPControlMode.Display)
            {
                this.TemplateName = this.DisplayTemplateName;
            }
            base.CreateChildControls();
            if (ControlMode == SPControlMode.Display)
            {
                LabSPLegalAmountField = (Label)TemplateContainer.FindControl("LabSPLegalAmountField");
                if (LabSPLegalAmountField == null)
                    throw new ArgumentException("未找到LabSPLegalAmountField控件");
                LabhidSPLegalAmountField = (HiddenField)TemplateContainer.FindControl("LabhidSPLegalAmountField");
                if (LabhidSPLegalAmountField == null)
                    throw new ArgumentException("未找到LabhidSPLegalAmountField控件");


                SPLegalAmountFieldValue fieldValue = (SPLegalAmountFieldValue)this.ItemFieldValue;
                if (fieldValue != null)
                {
                    LabSPLegalAmountField.Text = fieldValue.AmountCapital;
                    LabhidSPLegalAmountField.Value = fieldValue.AmountNumber;
                }

            }
            else
            {
                txtSPLegalAmountField = (TextBox)TemplateContainer.FindControl("txtSPLegalAmountField");
                hidSPLegalAmountField = (HiddenField)TemplateContainer.FindControl("hidSPLegalAmountField");
                hidSPLegalAmountFieldPropery = (HiddenField)TemplateContainer.FindControl("hidSPLegalAmountFieldPropery");

                if (txtSPLegalAmountField == null)
                    throw new ArgumentException("未找到txtSPLegalAmountField控件");
                if (hidSPLegalAmountField == null)
                    throw new ArgumentException("未找到hidSPLegalAmountField控件");
                if (hidSPLegalAmountFieldPropery == null)
                    throw new ArgumentException("未找到hidSPLegalAmountFieldPropery控件");


                SPLegalAmountField field = (SPLegalAmountField)base.Field;
                if (!string.IsNullOrEmpty(field.SPLegalAmountFieldTextboxWidth))
                    txtSPLegalAmountField.Width = Convert.ToInt32(field.SPLegalAmountFieldTextboxWidth.Trim());


                FormField txtAmountLower = GetCurrentFormFieldControl((Control)this.Page, field.SPLegalAmountFieldRelevanceListField);

                if (txtAmountLower != null)
                {
                    string _txtAmountLowerId = txtAmountLower.ClientID + txtAmountLower.ClientID.Replace(txtAmountLower.ClientID, "") + "_TextField";


                    StringBuilder jsstr = new StringBuilder();
                    jsstr.AppendLine("<script type=\"text/javascript\">");
                    jsstr.AppendLine("function convertAmount" + _txtAmountLowerId + "(){");
                    jsstr.AppendLine("var _this=document.getElementById('" + _txtAmountLowerId + "');");
                    jsstr.AppendLine("var _amountlegal = convert(_this.value,_this);");
                    jsstr.AppendLine("document.getElementById('" + txtSPLegalAmountField.ClientID + "').value = _amountlegal;");
                    jsstr.AppendLine("document.getElementById('" + hidSPLegalAmountField.ClientID + "').value = _this.value;");
                    jsstr.AppendLine("}");
                    jsstr.AppendLine("if(/msie/i.test(navigator.userAgent)){");
                    jsstr.AppendLine("document.getElementById('" + _txtAmountLowerId + "').onpropertychange = convertAmount" + _txtAmountLowerId + "");
                    jsstr.AppendLine("}");
                    jsstr.AppendLine("else");
                    jsstr.AppendLine("{");
                    jsstr.AppendLine("document.getElementById('" + _txtAmountLowerId + "').addEventListener(\"input\",convertAmount" + _txtAmountLowerId + ",false);");
                    jsstr.AppendLine("}");
                    jsstr.AppendLine("convertAmount" + _txtAmountLowerId + "();");
                    jsstr.AppendLine("</script>");

                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "Amountchangejs" + field.SPLegalAmountFieldRelevanceListField, jsstr.ToString());
                }
            }

        }
        public FormField GetCurrentFormFieldControl(Control control, string fieldName)
        {
            FormField result = null;
            if (control is FormField)
            {
                if (((FormField)control).Field.InternalName == fieldName)
                {
                    result = (FormField)control;
                }
            }
            else
            {
                foreach (Control c in control.Controls)
                {
                    result = GetCurrentFormFieldControl(c, fieldName);
                    if (result != null)
                    {
                        break;
                    }
                }
            }

            return result;
        }
    }
                }
