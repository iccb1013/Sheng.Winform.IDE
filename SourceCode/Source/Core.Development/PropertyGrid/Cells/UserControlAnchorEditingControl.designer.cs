/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlAnchorEditingControl
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.comboBoxAnchor = new Sheng.SailingEase.Controls.SEAdvComboBox();
            this.SuspendLayout();
            this.comboBoxAnchor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxAnchor.DropUserControl = null;
            this.comboBoxAnchor.Location = new System.Drawing.Point(0, 0);
            this.comboBoxAnchor.Name = "comboBoxAnchor";
            this.comboBoxAnchor.Size = new System.Drawing.Size(237, 23);
            this.comboBoxAnchor.TabIndex = 0;
            this.comboBoxAnchor.Value = null;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxAnchor);
            this.Name = "UserControlAnchorEditingControl";
            this.Size = new System.Drawing.Size(237, 46);
            this.ResumeLayout(false);
        }
        private Sheng.SailingEase.Controls.SEAdvComboBox comboBoxAnchor;
    }
}
