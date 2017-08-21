/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.ComponentModel.Design.UndoEngine
{
    partial class StepList
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
            this.panel = new System.Windows.Forms.Panel();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblNotice = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(274, 250);
            this.panel.TabIndex = 0;
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.Location = new System.Drawing.Point(274, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(17, 250);
            this.vScrollBar.TabIndex = 0;
            this.panel1.Controls.Add(this.lblNotice);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 250);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(291, 23);
            this.panel1.TabIndex = 1;
            this.lblNotice.AutoSize = true;
            this.lblNotice.Location = new System.Drawing.Point(16, 5);
            this.lblNotice.Name = "lblNotice";
            this.lblNotice.Size = new System.Drawing.Size(59, 12);
            this.lblNotice.TabIndex = 0;
            this.lblNotice.Text = "lblNotice";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.Controls.Add(this.vScrollBar);
            this.Controls.Add(this.panel1);
            this.Name = "StepList";
            this.Size = new System.Drawing.Size(291, 273);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.VScrollBar vScrollBar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblNotice;
    }
}
