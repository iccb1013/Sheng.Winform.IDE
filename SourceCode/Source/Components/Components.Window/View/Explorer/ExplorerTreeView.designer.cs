/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.WindowComponent.View
{
    partial class ExplorerTreeView
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
            this.navigationTree = new Sheng.SailingEase.Controls.SETreeView();
            this.SuspendLayout();
            this.navigationTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.navigationTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationTree.HotTracking = true;
            this.navigationTree.Location = new System.Drawing.Point(0, 0);
            this.navigationTree.Name = "navigationTree";
            this.navigationTree.ShowLines = false;
            this.navigationTree.Size = new System.Drawing.Size(284, 262);
            this.navigationTree.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.navigationTree);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "ExplorerTreeView";
            this.Text = "FormNavigationTree";
            this.Load += new System.EventHandler(this.FormNavigationTree_Load);
            this.ResumeLayout(false);
        }
        private Sheng.SailingEase.Controls.SETreeView navigationTree;
    }
}
