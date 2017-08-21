/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development.View
{
    partial class DataSourceSelector
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
            this.linkLabelDataSource = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            this.linkLabelDataSource.BackColor = System.Drawing.SystemColors.Control;
            this.linkLabelDataSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.linkLabelDataSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkLabelDataSource.Location = new System.Drawing.Point(0, 0);
            this.linkLabelDataSource.Margin = new System.Windows.Forms.Padding(0);
            this.linkLabelDataSource.Name = "linkLabelDataSource";
            this.linkLabelDataSource.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.linkLabelDataSource.Size = new System.Drawing.Size(517, 35);
            this.linkLabelDataSource.TabIndex = 0;
            this.linkLabelDataSource.TabStop = true;
            this.linkLabelDataSource.Text = "${DataSourceSelector_linkLabelDataSource}";
            this.linkLabelDataSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabelDataSource.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDataSource_LinkClicked);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkLabelDataSource);
            this.Name = "DataSourceSelector";
            this.Size = new System.Drawing.Size(517, 35);
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.LinkLabel linkLabelDataSource;
    }
}
