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
namespace Sheng.SailingEase.Controls.Extensions
{
    public class ToolStripMenuItemView : System.Windows.Forms.ToolStripMenuItem, IToolStripItemView
    {
        public ToolStripMenuItemView()
        {
        }
        protected override void OnClick(EventArgs e)
        {
            if (Codon != null && Codon.Action != null)
            {
                Codon.Action(this, new ToolStripItemCodonEventArgs(this.Codon));
            }
            base.OnClick(e);
        }
        private IToolStripItemCodon _codon;
        public IToolStripItemCodon Codon
        {
            get { return _codon; }
            set
            {
                _codon = value;
                ToolStripItemViewHelper.Update(this, _codon);
                ToolStripMenuItemCodon<ToolStripMenuItemView> menuItemCodon = (ToolStripMenuItemCodon<ToolStripMenuItemView>)_codon;
                foreach (IToolStripItemCodon itemCodon in menuItemCodon.Items)
                {
                    this.DropDownItems.Add((ToolStripItem)_codon.View);
                }
            }
        }
        public void UpdateSataus()
        {
            ToolStripItemViewHelper.UpdateStatus(this);
            IToolStripItemView itemDoozer;
            foreach (System.Windows.Forms.ToolStripItem item in this.DropDownItems)
            {
                itemDoozer = item as IToolStripItemView;
                if (itemDoozer != null)
                    itemDoozer.UpdateSataus();
            }
        }
    }
}
