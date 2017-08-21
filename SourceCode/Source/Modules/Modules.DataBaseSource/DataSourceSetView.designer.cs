/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Modules.DataBaseSourceModule
{
    partial class DataSourceSetView
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
            this.sePanel1 = new Sheng.SailingEase.Controls.SEPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblConnectionString = new System.Windows.Forms.Label();
            this.txtConnectionString = new Sheng.SailingEase.Controls.SETextBox();
            this.linkLabelCreateDataSource = new System.Windows.Forms.LinkLabel();
            this.sePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(463, 307);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "${DataSourceSetView_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(382, 307);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "${DataSourceSetView_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.sePanel1.BackColor = System.Drawing.Color.White;
            this.sePanel1.Controls.Add(this.lblTitle);
            this.sePanel1.Controls.Add(this.pictureBox1);
            this.sePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.sePanel1.HighLight = false;
            this.sePanel1.Location = new System.Drawing.Point(0, 0);
            this.sePanel1.Name = "sePanel1";
            this.sePanel1.Size = new System.Drawing.Size(550, 80);
            this.sePanel1.TabIndex = 6;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.Location = new System.Drawing.Point(83, 39);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(255, 14);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "${DataSourceSetView_LabelTitle}";
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(55, 52);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.lblConnectionString.AutoSize = true;
            this.lblConnectionString.Location = new System.Drawing.Point(35, 109);
            this.lblConnectionString.Name = "lblConnectionString";
            this.lblConnectionString.Size = new System.Drawing.Size(263, 12);
            this.lblConnectionString.TabIndex = 7;
            this.lblConnectionString.Text = "${DataSourceSetView_LabelConnectionString}:";
            this.txtConnectionString.AllowEmpty = true;
            this.txtConnectionString.HighLight = true;
            this.txtConnectionString.Location = new System.Drawing.Point(37, 132);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.Regex = "";
            this.txtConnectionString.RegexMsg = null;
            this.txtConnectionString.Size = new System.Drawing.Size(476, 147);
            this.txtConnectionString.TabIndex = 1;
            this.txtConnectionString.Title = null;
            this.txtConnectionString.ValueCompareTo = null;
            this.linkLabelCreateDataSource.Location = new System.Drawing.Point(319, 109);
            this.linkLabelCreateDataSource.Name = "linkLabelCreateDataSource";
            this.linkLabelCreateDataSource.Size = new System.Drawing.Size(194, 12);
            this.linkLabelCreateDataSource.TabIndex = 8;
            this.linkLabelCreateDataSource.TabStop = true;
            this.linkLabelCreateDataSource.Text = "${DataSourceSetView_LinkLabelCreateDataSource}";
            this.linkLabelCreateDataSource.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.linkLabelCreateDataSource.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelCreateDataSource_LinkClicked);
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(550, 342);
            this.Controls.Add(this.linkLabelCreateDataSource);
            this.Controls.Add(this.txtConnectionString);
            this.Controls.Add(this.lblConnectionString);
            this.Controls.Add(this.sePanel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDataSourceSet";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${DataSourceSetView}";
            this.Load += new System.EventHandler(this.DataSourceSetView_Load);
            this.sePanel1.ResumeLayout(false);
            this.sePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private Sheng.SailingEase.Controls.SEPanel sePanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblConnectionString;
        private Sheng.SailingEase.Controls.SETextBox txtConnectionString;
        private System.Windows.Forms.LinkLabel linkLabelCreateDataSource;
    }
}
