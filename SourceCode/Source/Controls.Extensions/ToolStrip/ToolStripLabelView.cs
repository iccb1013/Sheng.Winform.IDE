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
    public class ToolStripLabelView : System.Windows.Forms.ToolStripLabel, IToolStripItemView
    {
        public ToolStripLabelView()
        {
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
