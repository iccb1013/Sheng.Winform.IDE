/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    partial class FormToolbox
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelToolbox = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(292, 1);
            this.panel1.TabIndex = 1;
            this.panelToolbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelToolbox.Location = new System.Drawing.Point(0, 1);
            this.panelToolbox.Name = "panelToolbox";
            this.panelToolbox.Size = new System.Drawing.Size(292, 265);
            this.panelToolbox.TabIndex = 2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.panelToolbox);
            this.Controls.Add(this.panel1);
            this.Name = "FormToolbox";
            this.TabText = "FormToolbox";
            this.Text = "FormToolbox";
            this.Load += new System.EventHandler(this.FormToolbox_Load);
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelToolbox;
    }
}
