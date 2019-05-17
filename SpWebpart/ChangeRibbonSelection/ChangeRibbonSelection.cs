using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SpWebpart.ChangeRibbonSelection
{
    [ToolboxItemAttribute(false)]
    public class ChangeRibbonSelection : WebPart
    {
        protected override void CreateChildControls()
        {
            //SPSecurity.RunWithElevatedPrivileges(delegate()
            //{
                SPRibbon current = SPRibbon.GetCurrent(this.Page);
                if (current != null)
                {
                    current.MakeTabAvailable("Ribbon.Read");
                    current.InitialTabId = "Ribbon.Read";
                    current.Minimized = true;
                }
            //});
        }
                //current.TrimById("Ribbon.ListForm.Display"); 
                //SPRibbonScriptManager manager = new SPRibbonScriptManager();
                //List<IRibbonCommand> commands = new List<IRibbonCommand>();
                //bool admin = base.GlobalAdmin.IsCurrentUserMachineAdmin();
                //commands.Add(new SPRibbonCommand("WebAppTab"));
//        <script type='txt/javascript'>
//_spBodyOnLoadFunctionNames.push("InitTab");
//function InitTab()
//{
//InitializeTab("Ribbon.Document");
//}
//</script>
    }
}
