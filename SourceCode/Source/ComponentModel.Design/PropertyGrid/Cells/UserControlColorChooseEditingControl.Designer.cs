/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.ComponentModel.Design
{
    partial class UserControlColorChooseEditingControl
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
            this.colorChooseComboBox = new Controls.SEColorChooseComboBox();
            this.SuspendLayout();
            this.colorChooseComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.colorChooseComboBox.Location = new System.Drawing.Point(0, 0);
            this.colorChooseComboBox.Name = "colorChooseComboBox";
            this.colorChooseComboBox.Size = new System.Drawing.Size(252, 24);
            this.colorChooseComboBox.TabIndex = 0;
            this.colorChooseComboBox.Value = "";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.colorChooseComboBox);
            this.Name = "UserControlColorChooseEditingControl";
            this.Size = new System.Drawing.Size(252, 41);
            this.ResumeLayout(false);
        }
        private Sheng.SailingEase.Controls.SEColorChooseComboBox colorChooseComboBox;
    }
}
