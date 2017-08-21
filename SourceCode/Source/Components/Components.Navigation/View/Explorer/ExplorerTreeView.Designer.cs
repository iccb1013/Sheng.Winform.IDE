/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.NavigationComponent.View
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
            this.tabControl = new Sheng.SailingEase.Controls.SETabControl();
            this.SuspendLayout();
            this.tabControl.CustomValidate = null;
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.HighLight = false;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(284, 262);
            this.tabControl.TabIndex = 2;
            this.tabControl.Title = null;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "ExplorerTreeView";
            this.Text = "NavigationTreeView";
            this.Load += new System.EventHandler(this.ExplorerTreeView_Load);
            this.ResumeLayout(false);
        }
        private Controls.SETabControl tabControl;
    }
}
