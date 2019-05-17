using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLegalAmountField
{
    public class SPLegalAmountFieldValue : SPFieldMultiColumnValue
    {
        private const int numberOfFields = 2;
        public SPLegalAmountFieldValue() : base(numberOfFields) { }
        public SPLegalAmountFieldValue(string value) : base(value) { }
        public string AmountCapital
        {
            get { if (this != null && this.Count > 0) return this[0]; else return ""; }
            set { if (value != null) this[0] = value; else this[0] = ""; }
        }
        public string AmountNumber
        {
            get { if (this != null && this.Count > 1) return this[1]; else return ""; }
            set { if (value != null) this[1] = value; else this[1] = ""; }
        }

    }
}
