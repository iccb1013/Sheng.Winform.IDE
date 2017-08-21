/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.DictionaryComponent.View
{
    partial class EnumEntityEditView
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
            this.lblCode = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtCode = new Sheng.SailingEase.Controls.SETextBox();
            this.txtName = new Sheng.SailingEase.Controls.SETextBox();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.SuspendLayout();
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(25, 50);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(185, 12);
            this.lblCode.TabIndex = 11;
            this.lblCode.Text = "${EnumEditView_LabelCode}:";
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(25, 23);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(185, 12);
            this.lblName.TabIndex = 10;
            this.lblName.Text = "${EnumEditView_LabelName}:";
            this.txtCode.AllowEmpty = false;
            this.txtCode.CustomValidate = null;
            this.txtCode.HighLight = true;
            this.txtCode.LimitMaxValue = false;
            this.txtCode.Location = new System.Drawing.Point(100, 47);
            this.txtCode.MaxLength = 100;
            this.txtCode.MaxValue = ((long)(2147483647));
            this.txtCode.Name = "txtCode";
            this.txtCode.Regex = "";
            this.txtCode.RegexMsg = "RegexMsg_OnlyAllowMonogram";
            this.txtCode.Size = new System.Drawing.Size(192, 21);
            this.txtCode.TabIndex = 9;
            this.txtCode.Title = "LabelCode";
            this.txtCode.ValueCompareTo = null;
            this.txtCode.WaterText = "";
            this.txtName.AllowEmpty = false;
            this.txtName.CustomValidate = null;
            this.txtName.HighLight = true;
            this.txtName.LimitMaxValue = false;
            this.txtName.Location = new System.Drawing.Point(100, 20);
            this.txtName.MaxLength = 100;
            this.txtName.MaxValue = ((long)(2147483647));
            this.txtName.Name = "txtName";
            this.txtName.Regex = "";
            this.txtName.RegexMsg = null;
            this.txtName.Size = new System.Drawing.Size(192, 21);
            this.txtName.TabIndex = 8;
            this.txtName.Title = "LabelName";
            this.txtName.ValueCompareTo = null;
            this.txtName.WaterText = "";
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(228, 113);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "${EnumEditView_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(309, 113);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "${EnumEditView_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(396, 148);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.txtName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EnumEntityEditView";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${EnumEditView}";
            this.Load += new System.EventHandler(this.EnumEditView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label lblName;
        private Sheng.SailingEase.Controls.SETextBox txtCode;
        private Sheng.SailingEase.Controls.SETextBox txtName;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private Sheng.SailingEase.Controls.SEButton btnCancel;
    }
}
