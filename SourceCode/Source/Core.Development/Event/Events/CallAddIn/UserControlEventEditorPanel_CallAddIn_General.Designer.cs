/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlEventEditorPanel_CallAddIn_General
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
            this.txtCode = new Sheng.SailingEase.Controls.SETextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtName = new Sheng.SailingEase.Controls.SETextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblAddInFullName = new System.Windows.Forms.Label();
            this.txtAddInFullName = new Sheng.SailingEase.Controls.SETextBox();
            this.SuspendLayout();
            this.txtCode.AllowEmpty = false;
            this.txtCode.HighLight = true;
            this.txtCode.Location = new System.Drawing.Point(275, 32);
            this.txtCode.MaxLength = 100;
            this.txtCode.Name = "txtCode";
            this.txtCode.Regex = "";
            this.txtCode.RegexMsg = "RegexMsg_OnlyAllowMonogram";
            this.txtCode.Size = new System.Drawing.Size(120, 21);
            this.txtCode.TabIndex = 1;
            this.txtCode.Title = "LabelCode";
            this.txtCode.ValueCompareTo = null;
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(210, 35);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(359, 12);
            this.lblCode.TabIndex = 19;
            this.lblCode.Text = "${UserControlEventEditorPanel_CallAddIn_General_LabelCode}";
            this.txtName.AllowEmpty = false;
            this.txtName.HighLight = true;
            this.txtName.Location = new System.Drawing.Point(69, 32);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.txtName.MaxLength = 100;
            this.txtName.Name = "txtName";
            this.txtName.Regex = "";
            this.txtName.RegexMsg = null;
            this.txtName.Size = new System.Drawing.Size(120, 21);
            this.txtName.TabIndex = 0;
            this.txtName.Title = "LabelName";
            this.txtName.ValueCompareTo = null;
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(3, 35);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(365, 12);
            this.lblName.TabIndex = 17;
            this.lblName.Text = "${UserControlEventEditorPanel_CallAddIn_General_LabelName}:";
            this.lblAddInFullName.AutoSize = true;
            this.lblAddInFullName.Location = new System.Drawing.Point(3, 67);
            this.lblAddInFullName.Name = "lblAddInFullName";
            this.lblAddInFullName.Size = new System.Drawing.Size(419, 12);
            this.lblAddInFullName.TabIndex = 24;
            this.lblAddInFullName.Text = "${UserControlEventEditorPanel_CallAddIn_General_LabelAddInFullName}:";
            this.txtAddInFullName.AllowEmpty = false;
            this.txtAddInFullName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddInFullName.HighLight = true;
            this.txtAddInFullName.Location = new System.Drawing.Point(69, 86);
            this.txtAddInFullName.MaxLength = 1000;
            this.txtAddInFullName.Multiline = true;
            this.txtAddInFullName.Name = "txtAddInFullName";
            this.txtAddInFullName.Regex = "";
            this.txtAddInFullName.RegexMsg = null;
            this.txtAddInFullName.Size = new System.Drawing.Size(376, 75);
            this.txtAddInFullName.TabIndex = 2;
            this.txtAddInFullName.Title = "LabelAddInFullName";
            this.txtAddInFullName.ValueCompareTo = null;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblAddInFullName);
            this.Controls.Add(this.txtAddInFullName);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Name = "UserControlEventEditorPanel_CallAddIn_General";
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.lblCode, 0);
            this.Controls.SetChildIndex(this.txtCode, 0);
            this.Controls.SetChildIndex(this.txtAddInFullName, 0);
            this.Controls.SetChildIndex(this.lblAddInFullName, 0);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Sheng.SailingEase.Controls.SETextBox txtCode;
        private System.Windows.Forms.Label lblCode;
        private Sheng.SailingEase.Controls.SETextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblAddInFullName;
        private Sheng.SailingEase.Controls.SETextBox txtAddInFullName;
    }
}
