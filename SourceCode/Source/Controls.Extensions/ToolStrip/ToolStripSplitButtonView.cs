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
    public class ToolStripSplitButtonView : System.Windows.Forms.ToolStripSplitButton, IToolStripItemView
    {
        private ToolStripSplitButtonCodon<ToolStripSplitButtonView> SplitButtonCodon
        {
            get { return Codon as ToolStripSplitButtonCodon<ToolStripSplitButtonView>; }
        }
        public ToolStripSplitButtonView()
        {
            this.DropDownOpening += new EventHandler(ToolStripSplitButtonDoozer_DropDownOpening);
        }
        void ToolStripSplitButtonDoozer_DropDownOpening(object sender, EventArgs e)
        {
            if (SplitButtonCodon != null && SplitButtonCodon.DropDownOpeningAction != null)
            {
                SplitButtonCodon.DropDownOpeningAction(this,new ToolStripItemCodonEventArgs( this.Codon));
            }
        }
        protected override void OnButtonClick(EventArgs e)
        {
            if (Codon != null && Codon.Action != null)
            {
                Codon.Action(this, new ToolStripItemCodonEventArgs(this.Codon));
            }
            base.OnButtonClick(e);
        }
        private IToolStripItemCodon _codon;
        public IToolStripItemCodon Codon
        {
            get { return _codon; }
            set
            {
                _codon = value;
                ToolStripItemViewHelper.Update(this, _codon);
                ToolStripItemViewHelper.Update(this.DropDownItems, SplitButtonCodon.Items);
            }
        }
        public void UpdateSataus()
        {
            ToolStripItemViewHelper.UpdateStatus(this);
        }
    }
}
