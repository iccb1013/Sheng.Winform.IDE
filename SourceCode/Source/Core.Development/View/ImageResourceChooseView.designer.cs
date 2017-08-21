/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class ImageResourceChooseView
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
            this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
            this.seGroupBox1 = new Sheng.SailingEase.Controls.SEGroupBox();
            this.btnImport = new Sheng.SailingEase.Controls.SEButton();
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxImageResourceList = new System.Windows.Forms.ListBox();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
            this.seGroupBox1.SuspendLayout();
            this.SuspendLayout();
            this.pictureBoxPreview.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pictureBoxPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxPreview.Location = new System.Drawing.Point(260, 12);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new System.Drawing.Size(231, 207);
            this.pictureBoxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxPreview.TabIndex = 0;
            this.pictureBoxPreview.TabStop = false;
            this.seGroupBox1.Controls.Add(this.btnImport);
            this.seGroupBox1.Controls.Add(this.label1);
            this.seGroupBox1.Controls.Add(this.listBoxImageResourceList);
            this.seGroupBox1.HighLight = false;
            this.seGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.seGroupBox1.Name = "seGroupBox1";
            this.seGroupBox1.Size = new System.Drawing.Size(242, 207);
            this.seGroupBox1.TabIndex = 0;
            this.seGroupBox1.TabStop = false;
            this.seGroupBox1.Text = "${ImageResourceChooseView_GroupBoxResource}";
            this.btnImport.Location = new System.Drawing.Point(8, 171);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "${ImageResourceChooseView_ButtonImport}";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "${ImageResourceChooseView_LabelProjectResource}:";
            this.listBoxImageResourceList.FormattingEnabled = true;
            this.listBoxImageResourceList.ItemHeight = 12;
            this.listBoxImageResourceList.Location = new System.Drawing.Point(8, 41);
            this.listBoxImageResourceList.Name = "listBoxImageResourceList";
            this.listBoxImageResourceList.Size = new System.Drawing.Size(230, 124);
            this.listBoxImageResourceList.TabIndex = 1;
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(416, 237);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "${ImageResourceChooseView_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(335, 237);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "${ImageResourceChooseView_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(503, 272);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.seGroupBox1);
            this.Controls.Add(this.pictureBoxPreview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormImageResourceChoose";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "${ImageResourceChooseView}";
            this.Load += new System.EventHandler(this.FormImageResourceChoose_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
            this.seGroupBox1.ResumeLayout(false);
            this.seGroupBox1.PerformLayout();
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.PictureBox pictureBoxPreview;
        private Sheng.SailingEase.Controls.SEGroupBox seGroupBox1;
        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxImageResourceList;
        private Sheng.SailingEase.Controls.SEButton btnImport;
    }
}
