/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.ResourceComponent.View
{
    partial class ResourceViewContainer
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
            this.viewNavigation = new Sheng.SailingEase.Controls.MozBar.MozPane();
            this.viewContainer = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.viewNavigation)).BeginInit();
            this.SuspendLayout();
            this.viewNavigation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.viewNavigation.ImageList = null;
            this.viewNavigation.Location = new System.Drawing.Point(3, 3);
            this.viewNavigation.Name = "viewNavigation";
            this.viewNavigation.Size = new System.Drawing.Size(134, 300);
            this.viewNavigation.TabIndex = 0;
            this.viewContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewContainer.Location = new System.Drawing.Point(143, 3);
            this.viewContainer.Name = "viewContainer";
            this.viewContainer.Size = new System.Drawing.Size(406, 300);
            this.viewContainer.TabIndex = 1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.viewContainer);
            this.Controls.Add(this.viewNavigation);
            this.Name = "ResourceViewContainer";
            this.Size = new System.Drawing.Size(552, 306);
            this.Load += new System.EventHandler(this.ResourceViewContainer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.viewNavigation)).EndInit();
            this.ResumeLayout(false);
        }
        private Controls.MozBar.MozPane viewNavigation;
        private System.Windows.Forms.Panel viewContainer;
    }
}
