/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    partial class FormDesignSurfaceHosting
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
            this.SuspendLayout();
            this.panel.BackColor = System.Drawing.Color.White;
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(571, 328);
            this.panel.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 328);
            this.Controls.Add(this.panel);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FormDesignSurfaceHosting";
            this.Text = "FormDesinger";
            this.Load += new System.EventHandler(this.FormDesignSurfaceHosting_Load);
            this.Shown += new System.EventHandler(this.FormDesignSurfaceHosting_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormDesignSurfaceHosting_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDesignSurfaceHosting_FormClosing);
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.Panel panel;
    }
}
