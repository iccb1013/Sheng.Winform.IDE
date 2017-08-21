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
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Diagnostics;
namespace Sheng.SailingEase.Controls.Extensions
{
    public class ToolStripView : System.Windows.Forms.ToolStrip
    {
        public ToolStripCodon Codon
        {
            get;
            private set;
        }
        public ToolStripView(ToolStripCodon codon)
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
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            IToolStripItemView toolStripItemDoozer;
            foreach (ToolStripItem item in this.Items)
            {
                if (item.IsOnOverflow)
                    continue;
                toolStripItemDoozer = item as IToolStripItemView;
                if (toolStripItemDoozer == null)
                    continue;
                if (toolStripItemDoozer.Codon.Owner != this.Codon)
                {
                    e.Graphics.FillRectangle(new LinearGradientBrush(new Rectangle(item.Bounds.X, 0, item.Bounds.Width, this.Height),
                        Color.Transparent, Color.FromArgb(255, 255, 156), LinearGradientMode.Vertical),
                        new Rectangle(item.Bounds.X, 0, item.Bounds.Width, this.Height));
                }
            }
        }
        public void RegisterItem(string path, IToolStripItemCodon toolStripItem)
        {
            RegisterItem(new ToolStripPath(path), toolStripItem);
        }
        public void RegisterItem(ToolStripPath path, IToolStripItemCodon toolStripItem)
        {
            if (path.PathPoints.Count > 1)
            {
                Debug.Assert(false, "工具栏暂不支持多级注册项");
                throw new NotImplementedException("工具栏暂不支持多级注册项");
            }
            toolStripItem.Owner = this.Codon;
            ToolStripItem item = toolStripItem.View as ToolStripItem;
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
            this.Visible = Codon.Visible;
            if (this.Visible)
            {
                IToolStripItemView itemDoozer;
                foreach (ToolStripItem item in this.Items)
                {
                    itemDoozer = item as IToolStripItemView;
                    if (itemDoozer != null)
                        itemDoozer.UpdateSataus();
                }
            }
        }
    }
}
