/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Sheng.SailingEase.RegexTool
{
    public partial class FormRegexLib : Form
    {
        public string Regex
        {
            get;
            private set;
        }
        public FormRegexLib()
        {
            InitializeComponent();
            foreach (RegexItem item in RegexLib.Items)
            {
                ListViewItem listViewItem = new ListViewItem(item.name);
                listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(listViewItem, item.regexString));
                listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(listViewItem, item.remark));
                this.listViewRegexLib.Items.Add(listViewItem);
            }
        }
        private void FormRegexLib_Load(object sender, EventArgs e)
        {
        }
        private void listViewRegexLib_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listViewRegexLib.SelectedItems.Count == 0)
                return;
            Regex = this.listViewRegexLib.SelectedItems[0].SubItems[1].Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
