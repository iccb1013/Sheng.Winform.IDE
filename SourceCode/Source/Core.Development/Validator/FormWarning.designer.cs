/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class FormWarning
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWarning));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonWarning = new System.Windows.Forms.ToolStripButton();
            this.userControlWarningView = new UserControlWarningView();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonWarning});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(645, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStripButtonWarning.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonWarning.Image")));
            this.toolStripButtonWarning.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonWarning.Name = "toolStripButtonWarning";
            this.toolStripButtonWarning.Size = new System.Drawing.Size(267, 22);
            this.toolStripButtonWarning.Text = "${FormWarning_ToolStripButtonWarning}";
            this.userControlWarningView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlWarningView.HighLight = false;
            this.userControlWarningView.Location = new System.Drawing.Point(0, 25);
            this.userControlWarningView.Name = "userControlWarningView";
            this.userControlWarningView.Size = new System.Drawing.Size(645, 321);
            this.userControlWarningView.TabIndex = 8;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 346);
            this.Controls.Add(this.userControlWarningView);
            this.Controls.Add(this.toolStrip1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormWarning";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${FormWarning}";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonWarning;
        private UserControlWarningView userControlWarningView;
    }
}
