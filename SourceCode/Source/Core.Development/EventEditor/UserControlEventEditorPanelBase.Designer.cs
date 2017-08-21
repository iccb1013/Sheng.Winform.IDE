/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlEventEditorPanelBase
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
            this.lblTitle = new Sheng.SailingEase.Controls.SEAdvLabel();
            this.SuspendLayout();
            this.lblTitle.BorderColor = System.Drawing.Color.Black;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.FillColorEnd = System.Drawing.Color.Empty;
            this.lblTitle.FillColorStart = System.Drawing.Color.LightGray;
            this.lblTitle.FillMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.lblTitle.FillStyle = Sheng.SailingEase.Controls.FillStyle.Solid;
            this.lblTitle.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.ShowBorder = false;
            this.lblTitle.SingleLine = true;
            this.lblTitle.Size = new System.Drawing.Size(460, 23);
            this.lblTitle.TabIndex = 24;
            this.lblTitle.Text = " LabelTitle";
            this.lblTitle.TextHorizontalCenter = false;
            this.lblTitle.TextVerticalCenter = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTitle);
            this.Name = "UserControlEventSetParameter";
            this.Size = new System.Drawing.Size(460, 300);
            this.ResumeLayout(false);
        }
        private Sheng.SailingEase.Controls.SEAdvLabel lblTitle;
    }
}
