/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Windows.Forms.Development
{
    partial class FormSEComboBoxExDevDataRule
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
            this.panel = new Sheng.SailingEase.Controls.SEPanel();
            this.lblTitle = new Sheng.SailingEase.Controls.SEAdvLabel();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.btnClear = new Sheng.SailingEase.Controls.SEButton();
            this.SuspendLayout();
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
            this.panel.Size = new System.Drawing.Size(472, 228);
            this.panel.TabIndex = 0;
            this.lblTitle.BorderColor = System.Drawing.Color.Empty;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.FillColorEnd = System.Drawing.Color.Empty;
            this.lblTitle.FillColorStart = System.Drawing.Color.GhostWhite;
            this.lblTitle.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.lblTitle.FillStyle = Sheng.SailingEase.Controls.FillStyle.LinearGradient;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.ShowBorder = false;
            this.lblTitle.SingleLine = false;
            this.lblTitle.Size = new System.Drawing.Size(496, 30);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = " ${FormSEComboBoxExDevDataRule_LabelTitle}";
            this.lblTitle.TextHorizontalCenter = false;
            this.lblTitle.TextVerticalCenter = true;
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(328, 288);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "${FormSEComboBoxExDevDataRule_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(409, 288);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "${FormSEComboBoxExDevDataRule_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnClear.Location = new System.Drawing.Point(12, 288);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 16;
            this.btnClear.Text = "${FormSEComboBoxExDevDataRule_ButtonClear}";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(496, 323);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSEComboBoxExDevDataRule";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${FormSEComboBoxExDevDataRule}";
            this.Load += new System.EventHandler(this.FormSEComboBoxExDevDataRule_Load);
            this.ResumeLayout(false);
        }
        private Sheng.SailingEase.Controls.SEPanel panel;
        private Sheng.SailingEase.Controls.SEAdvLabel lblTitle;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private Sheng.SailingEase.Controls.SEButton btnClear;
    }
}
