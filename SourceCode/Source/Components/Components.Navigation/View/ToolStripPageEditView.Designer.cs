/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    partial class ToolStripPageEditView
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
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.lblName = new System.Windows.Forms.Label();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtCode = new Sheng.SailingEase.Controls.SETextBox();
            this.txtName = new Sheng.SailingEase.Controls.SETextBox();
            this.SuspendLayout();
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(320, 148);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "${ToolStripPageEditView_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(239, 148);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "${ToolStripPageEditView_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(22, 28);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(215, 12);
            this.lblName.TabIndex = 8;
            this.lblName.Text = "${ToolStripPageEditView_LabelName}:";
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(22, 55);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(215, 12);
            this.lblCode.TabIndex = 10;
            this.lblCode.Text = "${ToolStripPageEditView_LabelCode}:";
            this.txtCode.AllowEmpty = false;
            this.txtCode.CustomValidate = null;
            this.txtCode.HighLight = true;
            this.txtCode.LimitMaxValue = false;
            this.txtCode.Location = new System.Drawing.Point(108, 52);
            this.txtCode.MaxLength = 100;
            this.txtCode.MaxValue = ((long)(2147483647));
            this.txtCode.Name = "txtCode";
            this.txtCode.Regex = "";
            this.txtCode.RegexMsg = "";
            this.txtCode.Size = new System.Drawing.Size(212, 21);
            this.txtCode.TabIndex = 11;
            this.txtCode.Title = "";
            this.txtCode.ValueCompareTo = null;
            this.txtCode.WaterText = "";
            this.txtName.AllowEmpty = false;
            this.txtName.CustomValidate = null;
            this.txtName.HighLight = true;
            this.txtName.LimitMaxValue = false;
            this.txtName.Location = new System.Drawing.Point(108, 25);
            this.txtName.MaxLength = 100;
            this.txtName.MaxValue = ((long)(2147483647));
            this.txtName.Name = "txtName";
            this.txtName.Regex = "";
            this.txtName.RegexMsg = null;
            this.txtName.Size = new System.Drawing.Size(212, 21);
            this.txtName.TabIndex = 9;
            this.txtName.Title = "";
            this.txtName.ValueCompareTo = null;
            this.txtName.WaterText = "";
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(407, 183);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ToolStripPageEditView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${ToolStripPageEditView}";
            this.Load += new System.EventHandler(this.ToolStripPageEditView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Controls.SEButton btnCancel;
        private Controls.SEButton btnOK;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblCode;
        private Controls.SETextBox txtCode;
        private Controls.SETextBox txtName;
    }
}
