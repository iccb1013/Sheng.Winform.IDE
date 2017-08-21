/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlEventEditorPanel_ValidateFormData_General
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
            this.ddlMode = new Sheng.SailingEase.Controls.SEComboBox();
            this.lblMode = new System.Windows.Forms.Label();
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
            this.lblCode.Size = new System.Drawing.Size(407, 12);
            this.lblCode.TabIndex = 26;
            this.lblCode.Text = "${UserControlEventEditorPanel_ValidateFormData_General_LabelCode}:";
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
            this.lblName.Size = new System.Drawing.Size(407, 12);
            this.lblName.TabIndex = 24;
            this.lblName.Text = "${UserControlEventEditorPanel_ValidateFormData_General_LabelName}:";
            this.ddlMode.AllowEmpty = false;
            this.ddlMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlMode.DisplayMember = "Text";
            this.ddlMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlMode.FormattingEnabled = true;
            this.ddlMode.HighLight = true;
            this.ddlMode.Location = new System.Drawing.Point(69, 59);
            this.ddlMode.Name = "ddlMode";
            this.ddlMode.Size = new System.Drawing.Size(326, 20);
            this.ddlMode.TabIndex = 2;
            this.ddlMode.Title = "LabelMode";
            this.ddlMode.ValueMember = "Value";
            this.lblMode.AutoSize = true;
            this.lblMode.Location = new System.Drawing.Point(4, 62);
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(407, 12);
            this.lblMode.TabIndex = 32;
            this.lblMode.Text = "${UserControlEventEditorPanel_ValidateFormData_General_LabelMode}:";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ddlMode);
            this.Controls.Add(this.lblMode);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Name = "UserControlEventEditorPanel_ValidateFormData_General";
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.lblCode, 0);
            this.Controls.SetChildIndex(this.txtCode, 0);
            this.Controls.SetChildIndex(this.lblMode, 0);
            this.Controls.SetChildIndex(this.ddlMode, 0);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Sheng.SailingEase.Controls.SETextBox txtCode;
        private System.Windows.Forms.Label lblCode;
        private Sheng.SailingEase.Controls.SETextBox txtName;
        private System.Windows.Forms.Label lblName;
        private Sheng.SailingEase.Controls.SEComboBox ddlMode;
        private System.Windows.Forms.Label lblMode;
    }
}
