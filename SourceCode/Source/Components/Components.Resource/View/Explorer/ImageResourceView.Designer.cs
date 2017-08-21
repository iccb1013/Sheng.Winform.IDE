/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.ResourceComponent.View
{
    partial class ImageResourceView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageResourceView));
            this.imageListView = new Controls.ImageListView();
            this.topPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            this.imageListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.imageListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageListView.Location = new System.Drawing.Point(0, 28);
            this.imageListView.Name = "imageListView";
            this.imageListView.Size = new System.Drawing.Size(379, 236);
            this.imageListView.TabIndex = 0;
            this.imageListView.Text = "";
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(379, 28);
            this.topPanel.TabIndex = 1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.imageListView);
            this.Controls.Add(this.topPanel);
            this.Name = "ImageResourceView";
            this.Size = new System.Drawing.Size(379, 264);
            this.Load += new System.EventHandler(this.ImageResourceView_Load);
            this.ResumeLayout(false);
        }
        private Controls.ImageListView imageListView;
        private System.Windows.Forms.Panel topPanel;
    }
}
