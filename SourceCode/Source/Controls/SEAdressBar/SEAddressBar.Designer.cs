/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Controls.SEAdressBar
{
    partial class SEAddressBar
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
            this.addressBarStrip = new Sheng.SailingEase.Controls.SEAdressBar.SEAddressBarStrip();
            this.SuspendLayout();
            this.addressBarStrip.CurrentNode = null;
            this.addressBarStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addressBarStrip.DropDownRenderer = null;
            this.addressBarStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.addressBarStrip.Location = new System.Drawing.Point(0, 0);
            this.addressBarStrip.Name = "addressBarStrip";
            this.addressBarStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.addressBarStrip.RootNode = null;
            this.addressBarStrip.Size = new System.Drawing.Size(466, 28);
            this.addressBarStrip.TabIndex = 0;
            this.addressBarStrip.Text = "seAddressBarStrip1";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.addressBarStrip);
            this.Name = "SEAddressBar";
            this.Size = new System.Drawing.Size(466, 28);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private SEAddressBarStrip addressBarStrip;
    }
}
