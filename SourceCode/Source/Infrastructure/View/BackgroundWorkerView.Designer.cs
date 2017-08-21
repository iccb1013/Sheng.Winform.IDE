/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Infrastructure
{
    partial class BackgroundWorkerView
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblNotice = new System.Windows.Forms.Label();
            this.SuspendLayout();
            this.progressBar1.Location = new System.Drawing.Point(12, 65);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(501, 23);
            this.progressBar1.TabIndex = 0;
            this.lblNotice.AutoSize = true;
            this.lblNotice.Location = new System.Drawing.Point(12, 29);
            this.lblNotice.Name = "lblNotice";
            this.lblNotice.Size = new System.Drawing.Size(215, 12);
            this.lblNotice.TabIndex = 1;
            this.lblNotice.Text = "${BackgroundWorkerView_LabelNotice}";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 151);
            this.Controls.Add(this.lblNotice);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BackgroundWorkerView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${BackgroundWorkerView}";
            this.Load += new System.EventHandler(this.BackgroundWorkerView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblNotice;
    }
}
