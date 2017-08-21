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
namespace Sheng.SailingEase.Controls.Extensions
{
    public class ContextMenuStripView : System.Windows.Forms.ContextMenuStrip
    {
        public ContextMenuStripCodon Codon
        {
            get;
            private set;
        }
        public ContextMenuStripView(ContextMenuStripCodon codon)
        {
            this.Renderer = UIHelper.ToolStripRenders.ControlToControlLight;
            Codon = codon;
            codon.View = this;
            if (codon != null && codon.Items != null)
            {
                foreach (IToolStripItemCodon toolStripItem in codon.Items)
                {
                    System.Windows.Forms.ToolStripItem item =
                        toolStripItem.View as System.Windows.Forms.ToolStripItem;
                    Debug.Assert(item != null, "item 为 null");
                    if (item != null)
                        this.Items.Add(item);
                }
            }
        }
        public void RegisterItem(ToolStripPath path, IToolStripItemCodon toolStripItem)
        {
            if (path.PathPoints.Count > 1)
            {
                throw new NotImplementedException("工具栏暂不支持多级注册项");
            }
            toolStripItem.Owner = this.Codon;
            ToolStripMenuItemView item = toolStripItem.View as ToolStripMenuItemView;
            if (path.PathPoints[0].Index.HasValue)
            {
                this.Items.Insert(path.PathPoints[0].Index.Value, item);
            }
            else
            {
                this.Items.Add(item);
            }
        }
        public void UpdateStatus()
        {
            IToolStripItemView itemDoozer;
            foreach (System.Windows.Forms.ToolStripItem item in this.Items)
            {
                itemDoozer = item as IToolStripItemView;
                if (itemDoozer != null)
                    itemDoozer.UpdateSataus();
            }
        }
        protected override void OnOpening(System.ComponentModel.CancelEventArgs e)
        {
            this.UpdateStatus();
            if (Codon != null && Codon.OnOpeningAction != null)
            {
                Codon.OnOpeningAction(this, this.Codon);
            }
            base.OnOpening(e);
        }
    }
}
