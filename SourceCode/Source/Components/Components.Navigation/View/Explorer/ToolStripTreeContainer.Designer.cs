/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    partial class ToolStripTreeContainer
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
            this.treeView = new Sheng.SailingEase.Controls.SETreeView();
            this.SuspendLayout();
            this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.HotTracking = true;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.ShowLines = false;
            this.treeView.Size = new System.Drawing.Size(150, 150);
            this.treeView.TabIndex = 3;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeView);
            this.Name = "ToolStripTreeContainer";
            this.Load += new System.EventHandler(this.ToolStripTreeContainer_Load);
            this.ResumeLayout(false);
        }
        private Controls.SETreeView treeView;
    }
}
