/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Infrastructure
{
    partial class FormMultiSave
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
            this.listBoxSaveForms = new System.Windows.Forms.ListBox();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.btnNo = new Sheng.SailingEase.Controls.SEButton();
            this.btnYes = new Sheng.SailingEase.Controls.SEButton();
            this.lblNotice = new System.Windows.Forms.Label();
            this.SuspendLayout();
            this.listBoxSaveForms.DisplayMember = "SaveFormTitle";
            this.listBoxSaveForms.FormattingEnabled = true;
            this.listBoxSaveForms.ItemHeight = 12;
            this.listBoxSaveForms.Location = new System.Drawing.Point(12, 43);
            this.listBoxSaveForms.Name = "listBoxSaveForms";
            this.listBoxSaveForms.Size = new System.Drawing.Size(437, 232);
            this.listBoxSaveForms.TabIndex = 0;
            this.listBoxSaveForms.SelectedIndexChanged += new System.EventHandler(this.listBoxSaveForms_SelectedIndexChanged);
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(357, 291);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 32);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "${FormMultiSave_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            this.btnNo.Location = new System.Drawing.Point(259, 291);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(92, 32);
            this.btnNo.TabIndex = 2;
            this.btnNo.Text = "${FormMultiSave_ButtonNo}";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            this.btnYes.Location = new System.Drawing.Point(161, 291);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(92, 32);
            this.btnYes.TabIndex = 3;
            this.btnYes.Text = "${FormMultiSave_ButtonYes}";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            this.lblNotice.AutoSize = true;
            this.lblNotice.Location = new System.Drawing.Point(12, 21);
            this.lblNotice.Name = "lblNotice";
            this.lblNotice.Size = new System.Drawing.Size(173, 12);
            this.lblNotice.TabIndex = 4;
            this.lblNotice.Text = "${FormMultiSave_LabelNotice}";
            this.AcceptButton = this.btnYes;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(461, 335);
            this.Controls.Add(this.lblNotice);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.listBoxSaveForms);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMultiSave";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${FormMultiSave}";
            this.Load += new System.EventHandler(this.FormMultiSave_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.ListBox listBoxSaveForms;
        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private Sheng.SailingEase.Controls.SEButton btnNo;
        private Sheng.SailingEase.Controls.SEButton btnYes;
        private System.Windows.Forms.Label lblNotice;
    }
}
