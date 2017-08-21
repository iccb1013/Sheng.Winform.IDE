/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace Sheng.SailingEase.Kernal
{
    public class LocalisationHelper
    {
        private ResourceManager _resourceManager;
       
        private readonly static Regex patternInner = new Regex(@"(?<=\${)([^\}]*)?(?=\})",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);
        private readonly static Regex pattern = new Regex(@"\$\{[^\}]+\}",
           RegexOptions.Compiled | RegexOptions.CultureInvariant);
        public LocalisationHelper(ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }
        public string GetString(string name)
        {
            return _resourceManager.GetString(name);
        }
        public string Parse(string input)
        {
            if (input == null)
                return String.Empty;
            string result = input;
            string resString;
            MatchCollection matchs = pattern.Matches(input);
            foreach (Match match in matchs)
            {
                resString = GetString(patternInner.Match(match.Value).Value);
                if (resString != null)
                {
                    resString = resString.Replace("$NewLine$", System.Environment.NewLine);
                    result = result.Replace(match.Value, resString);
                }
                else
                {
                    result = result.Replace(match.Value, "/");
                }
            }
            return result;
        }
        public void ApplyResource(UserControl userControl)
        {
            userControl.Text = Parse(userControl.Text);
            foreach (Control control in userControl.Controls)
            {
                ApplyResource(control);
            }
        }
        public void ApplyResource(Form form)
        {
            form.Text = Parse(form.Text);
            foreach (Control control in form.Controls)
            {
                ApplyResource(control);
            }
        }
        public void ApplyResource(Control control)
        {
            control.Text = Parse(control.Text);
            if (control.Controls != null)
            {
                foreach (Control ctrl in control.Controls)
                {
                    ApplyResource(ctrl);
                }
            }
            if (control.ContextMenuStrip != null)
            {
                ContextMenuStrip contextMenuStrip = control.ContextMenuStrip as ContextMenuStrip;
                ApplyResource(contextMenuStrip);
            }
            if (control is DataGridView)
            {
                DataGridView dataGridView = control as DataGridView;
                ApplyResource(dataGridView);
            }
            if (control is ToolStrip)
            {
                ToolStrip toolStrip = control as ToolStrip;
                ApplyResource(toolStrip);
            }
        }
        public void ApplyResource(DataGridView dataGridView)
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.HeaderText = Parse(column.HeaderText);
            }
        }
        public void ApplyResource(ToolStrip toolStrip)
        {
             foreach (System.Windows.Forms.ToolStripItem item in toolStrip.Items)
            {
                item.Text = Parse(item.Text);
                if (item is ToolStripDropDownItem)
                {
                    ToolStripDropDownItem toolStripDropDownItem = item as ToolStripDropDownItem;
                    ApplyResource(toolStripDropDownItem.DropDownItems);
                }
            }
        }
        public void ApplyResource(ContextMenuStrip contextMenuStrip)
        {
            ApplyResource(contextMenuStrip.Items);
        }
        public void ApplyResource(ToolStripItemCollection items)
        {
            foreach (System.Windows.Forms.ToolStripItem item in items)
            {
                item.Text = Parse(item.Text);
                if (item is ToolStripMenuItem)
                {
                    ToolStripMenuItem toolStripMenuItem = item as ToolStripMenuItem;
                    ApplyResource(toolStripMenuItem.DropDownItems);
                }
            }
        }
    }
}
