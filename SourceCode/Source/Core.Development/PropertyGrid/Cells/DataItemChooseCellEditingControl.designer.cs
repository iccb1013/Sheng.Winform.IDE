/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class DataItemChooseCellEditingControl
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
            this.txtDataItem = new System.Windows.Forms.TextBox();
            this.btnChoose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.txtDataItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDataItem.BackColor = System.Drawing.Color.White;
            this.txtDataItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDataItem.Location = new System.Drawing.Point(3, 3);
            this.txtDataItem.Name = "txtDataItem";
            this.txtDataItem.ReadOnly = true;
            this.txtDataItem.Size = new System.Drawing.Size(286, 14);
            this.txtDataItem.TabIndex = 0;
            this.btnChoose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChoose.Location = new System.Drawing.Point(295, 0);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(19, 19);
            this.btnChoose.TabIndex = 1;
            this.btnChoose.UseVisualStyleBackColor = true;
            this.btnChoose.Click += new System.EventHandler(this.btnChoose_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnChoose);
            this.Controls.Add(this.txtDataItem);
            this.Name = "DataItemChooseCellEditingControl";
            this.Size = new System.Drawing.Size(314, 87);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.TextBox txtDataItem;
        private System.Windows.Forms.Button btnChoose;
    }
}
