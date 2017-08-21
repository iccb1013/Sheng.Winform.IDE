/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.DataEntityComponent.View
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
            this.dataEntityTree = new Sheng.SailingEase.Controls.SETreeView();
            this.SuspendLayout();
            this.dataEntityTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataEntityTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataEntityTree.HotTracking = true;
            this.dataEntityTree.Location = new System.Drawing.Point(0, 0);
            this.dataEntityTree.Name = "dataEntityTree";
            this.dataEntityTree.ShowLines = false;
            this.dataEntityTree.Size = new System.Drawing.Size(284, 262);
            this.dataEntityTree.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.dataEntityTree);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FormNavigationTree";
            this.Text = "FormNavigationTree";
            this.Load += new System.EventHandler(this.FormNavigationTree_Load);
            this.ResumeLayout(false);
        }
        private Sheng.SailingEase.Controls.SETreeView dataEntityTree;
    }
}
