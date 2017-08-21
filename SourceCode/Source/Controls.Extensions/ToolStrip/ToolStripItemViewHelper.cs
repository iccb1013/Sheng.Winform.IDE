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
using System.Diagnostics;
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls.Extensions
{
    public static class ToolStripItemViewHelper
    {
        public static void Update(System.Windows.Forms.ToolStripItem toolStripItem, IToolStripItemCodon codon)
        {
            if (toolStripItem == null || codon == null)
            {
                Debug.Assert(false, "ToolStripItemDoozerHelper.Update , toolStripItem或codon为null");
                return;
            }
            toolStripItem.Text = codon.Text;
            toolStripItem.Image = codon.Image;
            toolStripItem.Alignment = codon.Alignment;
            toolStripItem.AutoSize = codon.AutoSize;
            toolStripItem.AutoToolTip = codon.AutoToolTip;
            toolStripItem.Available = codon.Available;
            toolStripItem.DisplayStyle = codon.DisplayStyle;
            if (codon.Width.HasValue)
                toolStripItem.Width = codon.Width.Value;
        }
        public static void Update(System.Windows.Forms.ToolStrip toolStrip, ToolStripItemCodonCollection codonCollection)
        {
            if (codonCollection == null || toolStrip == null)
            {
                Debug.Assert(false, "ToolStrip 或 CodonCollection为null");
                return;
            }
            toolStrip.Items.Clear();
            foreach (IToolStripItemCodon codon in codonCollection)
            {
                toolStrip.Items.Add((ToolStripItem)codon.View);
            }
        }
        public static void Update(System.Windows.Forms.ToolStripItemCollection toolStripItemCollection, 
            ToolStripItemCodonCollection codonCollection)
        {
            if (codonCollection == null || toolStripItemCollection == null)
            {
                Debug.Assert(false, "ToolStripItemCollection 或 CodonCollection为null");
                return;
            }
            toolStripItemCollection.Clear();
            foreach (IToolStripItemCodon codon in codonCollection)
            {
                toolStripItemCollection.Add((ToolStripItem)codon.View);
            }
        }
        public static void UpdateStatus(System.Windows.Forms.ToolStripItem toolStripItem)
        {
            IToolStripItemView view = toolStripItem as IToolStripItemView;
            if (view == null)
                return;
            toolStripItem.Visible = view.Codon.Visible;
            toolStripItem.Enabled = view.Codon.Enabled;
            toolStripItem.Text = view.Codon.Text;
        }
    }
}
