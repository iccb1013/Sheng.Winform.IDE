/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Windows.Forms.Development
{
    partial class FormSETextBoxExDevDataRule
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
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.seAdvLabel1 = new Sheng.SailingEase.Controls.SEAdvLabel();
            this.panel = new Sheng.SailingEase.Controls.SEPanel();
            this.SuspendLayout();
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(297, 247);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 19;
            this.btnOK.Text = "${FormSETextBoxExDevDataRule_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(378, 247);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "${FormSETextBoxExDevDataRule_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.seAdvLabel1.BorderColor = System.Drawing.Color.Empty;
            this.seAdvLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.seAdvLabel1.FillColorEnd = System.Drawing.Color.Empty;
            this.seAdvLabel1.FillColorStart = System.Drawing.Color.GhostWhite;
            this.seAdvLabel1.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.seAdvLabel1.FillStyle = Sheng.SailingEase.Controls.FillStyle.LinearGradient;
            this.seAdvLabel1.Location = new System.Drawing.Point(0, 0);
            this.seAdvLabel1.Name = "seAdvLabel1";
            this.seAdvLabel1.ShowBorder = false;
            this.seAdvLabel1.SingleLine = false;
            this.seAdvLabel1.Size = new System.Drawing.Size(465, 30);
            this.seAdvLabel1.TabIndex = 18;
            this.seAdvLabel1.Text = " ${FormSETextBoxExDevDataRule_LabelTitle}";
            this.seAdvLabel1.TextHorizontalCenter = false;
            this.seAdvLabel1.TextVerticalCenter = true;
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.BorderColor = System.Drawing.Color.Black;
            this.panel.FillColorEnd = System.Drawing.Color.Empty;
            this.panel.FillColorStart = System.Drawing.Color.Empty;
            this.panel.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.panel.FillStyle = Sheng.SailingEase.Controls.FillStyle.Solid;
            this.panel.HighLight = false;
            this.panel.Location = new System.Drawing.Point(12, 45);
            this.panel.Margin = new System.Windows.Forms.Padding(3, 12, 3, 12);
            this.panel.Name = "panel";
            this.panel.ShowBorder = false;
            this.panel.Size = new System.Drawing.Size(441, 187);
            this.panel.TabIndex = 21;
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(465, 282);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.seAdvLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSETextBoxExDevDataRule";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${FormSETextBoxExDevDataRule}";
            this.Load += new System.EventHandler(this.FormSETextBoxExDevDataRule_Load);
            this.ResumeLayout(false);
        }
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private Sheng.SailingEase.Controls.SEAdvLabel seAdvLabel1;
        private Sheng.SailingEase.Controls.SEPanel panel;
    }
}
