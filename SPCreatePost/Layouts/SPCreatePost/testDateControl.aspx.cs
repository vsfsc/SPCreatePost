using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SPCreatePost.Layouts.SPCreatePost
{
    public partial class testDateControl : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //BirthDatePicker.LocaleId = Convert.ToInt32(SPContext.Current.RegionalSettings.LocaleId);//设置日期选择器的区域类型，便于自动设置日期格式和语言

//PS:          DateTimeControl calDate =new DateTimeControl();

// calDate.ID ="WCal"+this.UniqueID; //设定一个唯一ID                                                                                                                                         calDate.LocaleId = (int)SPContext.Current.RegionalSettings.LocaleId;//设置日期选择器的区域类型，便于自动设置日期格式和语言                          calDate.DateOnly =true;//只需要选择日期，时间也可以选择，这个参数不设置，默认就带时间选择的。

//清空时间控件this.BirthDatePicker.ClearSelection();

//从GridView中获取时间赋值到时间控件上：

//ImageButton Btn = (ImageButton)e.CommandSource;

//this.BirthDatePicker.SelectedDate = Convert.ToDateTime((((GridViewRow)Btn.NamingContainer).FindControl("BirthDate") as Literal).Text.Trim());
        }

        protected void dtFrom_DateChanged(object sender, EventArgs e)
        {
           
        }
    }
}
