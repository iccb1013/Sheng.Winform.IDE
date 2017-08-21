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
namespace Sheng.SailingEase.Controls.Extensions
{
    public class ToolStripButtonView : System.Windows.Forms.ToolStripButton, IToolStripItemView
    {
        public ToolStripButtonView()
        {
        }
        protected override void OnClick(EventArgs e)
        {
            if (Codon != null && Codon.Action != null)
            {
                Codon.Action(this,new ToolStripItemCodonEventArgs(this.Codon));
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
            }
        }
        public void UpdateSataus()
        {
            ToolStripItemViewHelper.UpdateStatus(this);
        }
    }
}
