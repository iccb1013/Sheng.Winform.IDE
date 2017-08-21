/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class ImageResourceChooseCellEditingControl
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
            this.btnChoose = new System.Windows.Forms.Button();
            this.txtResourceName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            this.btnChoose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChoose.Location = new System.Drawing.Point(322, 0);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(19, 19);
            this.btnChoose.TabIndex = 3;
            this.btnChoose.UseVisualStyleBackColor = true;
            this.btnChoose.Click += new System.EventHandler(this.btnChoose_Click);
            this.txtResourceName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResourceName.BackColor = System.Drawing.Color.White;
            this.txtResourceName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtResourceName.Location = new System.Drawing.Point(3, 3);
            this.txtResourceName.Name = "txtResourceName";
            this.txtResourceName.ReadOnly = true;
            this.txtResourceName.Size = new System.Drawing.Size(313, 14);
            this.txtResourceName.TabIndex = 2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnChoose);
            this.Controls.Add(this.txtResourceName);
            this.Name = "ImageResourceChooseCellEditingControl";
            this.Size = new System.Drawing.Size(341, 107);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Button btnChoose;
        private System.Windows.Forms.TextBox txtResourceName;
    }
}
