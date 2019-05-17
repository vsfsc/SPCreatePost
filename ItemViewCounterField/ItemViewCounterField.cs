using System.Collections.Generic;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.WebControls;
using System.Security.Permissions;
using System.Collections.Specialized;

namespace ItemViewCounterField
{
   
    public class ItemViewCounterField : SPFieldNumber
    {
        // Methods
        public ItemViewCounterField(SPFieldCollection fields, string fieldName) : base(fields, fieldName)
        {
            this.Init();
        }

        public ItemViewCounterField(SPFieldCollection fields, string typeName, string displayName) : base(fields, typeName, displayName)
        {
            this.Init();
        }

        private void Init()
        {
            base.ShowInDisplayForm = true;
            base.ShowInEditForm = false;
            base.ShowInNewForm = false;
            base.ShowInViewForms = true;
        }

        // Properties
        public override BaseFieldControl FieldRenderingControl
        {
            [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
            get
            {
                return new ItemViewCounterFieldControl { FieldName = base.InternalName };
            }
        }
    }

}
