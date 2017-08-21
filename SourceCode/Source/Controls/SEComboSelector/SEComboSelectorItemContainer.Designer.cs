/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Controls
{
    partial class SEComboSelectorItemContainer
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
            this.panelItem = new System.Windows.Forms.Panel();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.SuspendLayout();
            this.panelItem.AutoScroll = true;
            this.panelItem.BackColor = System.Drawing.SystemColors.Window;
            this.panelItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelItem.Location = new System.Drawing.Point(0, 0);
            this.panelItem.Name = "panelItem";
            this.panelItem.Size = new System.Drawing.Size(193, 189);
            this.panelItem.TabIndex = 0;
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.Location = new System.Drawing.Point(193, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(17, 189);
            this.vScrollBar.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelItem);
            this.Controls.Add(this.vScrollBar);
            this.Name = "SEComboSelectorItemContainer";
            this.Size = new System.Drawing.Size(210, 189);
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.Panel panelItem;
        private System.Windows.Forms.VScrollBar vScrollBar;
    }
}
