/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    partial class Toolbox
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
            this.toolboxTabButton1 = new Sheng.SailingEase.Components.Window.DesignComponent.ToolboxTabButton();
            this.SuspendLayout();
            this.toolboxTabButton1.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolboxTabButton1.ItemContainer = null;
            this.toolboxTabButton1.Location = new System.Drawing.Point(0, 0);
            this.toolboxTabButton1.Name = "toolboxTabButton1";
            this.toolboxTabButton1.Selected = false;
            this.toolboxTabButton1.Size = new System.Drawing.Size(150, 23);
            this.toolboxTabButton1.TabIndex = 0;
            this.toolboxTabButton1.Text = "TabButton";
            this.toolboxTabButton1.UseVisualStyleBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.toolboxTabButton1);
            this.Name = "Toolbox";
            this.ResumeLayout(false);
        }
        private ToolboxTabButton toolboxTabButton1;
    }
}
