using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SharePoint;
namespace RegisterEventHandler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SPSite site = new SPSite(txtSIte.Text );
            SPWeb web=site.AllWebs[txtWeb.Text ];//
            SPList myList = web.Lists[txtList.Text];
            //注册事件处理程序
            //myList.EventReceivers.Add(SPEventReceiverType.ItemUpdated, "TaskEventHandler, Version=1.1.0.0, Culture=neutral, PublicKeyToken=4f5a25a745ef7f27", "");

            foreach (SPEventReceiverDefinition myEvet in myList.EventReceivers)
                Console.WriteLine(myEvet.Name );
        }
    }
}
