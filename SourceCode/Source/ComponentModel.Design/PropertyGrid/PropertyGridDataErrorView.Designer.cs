/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.ComponentModel.Design
{
    partial class PropertyGridDataErrorView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyGridDataErrorView));
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.txtErrorMsg = new Sheng.SailingEase.Controls.SETextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(286, 97);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "${PropertyGridDataErrorView_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(205, 97);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "${PropertyGridDataErrorView_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(22, 31);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(32, 32);
            this.pictureBox.TabIndex = 3;
            this.pictureBox.TabStop = false;
            this.txtErrorMsg.AllowEmpty = true;
            this.txtErrorMsg.BackColor = System.Drawing.SystemColors.Control;
            this.txtErrorMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtErrorMsg.CustomValidate = null;
            this.txtErrorMsg.HighLight = true;
            this.txtErrorMsg.LimitMaxValue = false;
            this.txtErrorMsg.Location = new System.Drawing.Point(73, 31);
            this.txtErrorMsg.MaxValue = ((long)(2147483647));
            this.txtErrorMsg.Multiline = true;
            this.txtErrorMsg.Name = "txtErrorMsg";
            this.txtErrorMsg.ReadOnly = true;
            this.txtErrorMsg.Regex = "";
            this.txtErrorMsg.RegexMsg = null;
            this.txtErrorMsg.Size = new System.Drawing.Size(288, 60);
            this.txtErrorMsg.TabIndex = 2;
            this.txtErrorMsg.Text = "Message";
            this.txtErrorMsg.Title = null;
            this.txtErrorMsg.ValueCompareTo = null;
            this.txtErrorMsg.WaterText = "";
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(373, 132);
            this.Controls.Add(this.txtErrorMsg);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPropertyGridDataError";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "${PropertyGridDataErrorView}";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private System.Windows.Forms.PictureBox pictureBox;
        private Sheng.SailingEase.Controls.SETextBox txtErrorMsg;
    }
}
