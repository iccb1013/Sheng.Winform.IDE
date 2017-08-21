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
    public class ToolStripComboBoxView : System.Windows.Forms.ToolStripComboBox, IToolStripItemView
    {
        private ToolStripComboBoxCodon<ToolStripComboBoxView> ComboBoxCodon
        {
            get { return Codon as ToolStripComboBoxCodon<ToolStripComboBoxView>; }
        }
        public ToolStripComboBoxView()
        {
            this.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
        }
        void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxCodon != null && ComboBoxCodon.SelectedItemChangedAction != null)
            {
                ToolStripComboBoxCodonSelectedItemChangedEventArgs args =
                    new ToolStripComboBoxCodonSelectedItemChangedEventArgs(this.Codon);
                args.SelectedItem = this.SelectedItem;
                ComboBoxCodon.SelectedItemChangedAction(this, args);
            }
        }
        private IToolStripItemCodon _codon;
        public IToolStripItemCodon Codon
        {
            get { return _codon; }
            set
            {
                _codon = value;
                ToolStripItemViewHelper.Update(this, _codon);
                this.ComboBox.DataSource = ComboBoxCodon.DataSource;
                if (ComboBoxCodon.DisplayMember != null)
                    this.ComboBox.DisplayMember = ComboBoxCodon.DisplayMember;
                if (ComboBoxCodon.ValueMember != null)
                    this.ComboBox.ValueMember = ComboBoxCodon.ValueMember;
            }
        }
        public void UpdateSataus()
        {
            if (this.Focused)
                return;
            ToolStripItemViewHelper.UpdateStatus(this);
            if (ComboBoxCodon.GetSelectedValue != null)
            {
                if (String.IsNullOrEmpty(this.ComboBox.ValueMember))
                {
                    this.ComboBox.SelectedItem = ComboBoxCodon.GetSelectedValue();
                }
                else
                {
                    this.ComboBox.SelectedValue = ComboBoxCodon.GetSelectedValue();
                }
            }
        }
    }
}
