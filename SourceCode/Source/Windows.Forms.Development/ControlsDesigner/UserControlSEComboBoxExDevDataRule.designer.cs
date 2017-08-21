/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Windows.Forms.Development
{
    partial class UserControlSEComboBoxExDevDataRule
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
            this.lblDataSourceMode = new System.Windows.Forms.Label();
            this.ddlDataSourceMode = new Sheng.SailingEase.Controls.SEComboBox();
            this.panel = new Sheng.SailingEase.Controls.SEPanel();
            this.seAdvLabel1 = new Sheng.SailingEase.Controls.SEAdvLabel();
            this.SuspendLayout();
            this.lblDataSourceMode.AutoSize = true;
            this.lblDataSourceMode.Location = new System.Drawing.Point(3, 6);
            this.lblDataSourceMode.Name = "lblDataSourceMode";
            this.lblDataSourceMode.Size = new System.Drawing.Size(353, 12);
            this.lblDataSourceMode.TabIndex = 1;
            this.lblDataSourceMode.Text = "${UserControlSEComboBoxExDevDataRule_LabelDataSourceMode}:";
            this.ddlDataSourceMode.AllowEmpty = true;
            this.ddlDataSourceMode.DisplayMember = "Text";
            this.ddlDataSourceMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDataSourceMode.FormattingEnabled = true;
            this.ddlDataSourceMode.HighLight = true;
            this.ddlDataSourceMode.Location = new System.Drawing.Point(104, 3);
            this.ddlDataSourceMode.Name = "ddlDataSourceMode";
            this.ddlDataSourceMode.Size = new System.Drawing.Size(216, 20);
            this.ddlDataSourceMode.TabIndex = 2;
            this.ddlDataSourceMode.Title = null;
            this.ddlDataSourceMode.ValueMember = "Value";
            this.ddlDataSourceMode.WaterText = "";
            this.ddlDataSourceMode.SelectedIndexChanged += new System.EventHandler(this.ddlDataSourceMode_SelectedIndexChanged);
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.BorderColor = System.Drawing.Color.Black;
            this.panel.FillColorEnd = System.Drawing.Color.Empty;
            this.panel.FillColorStart = System.Drawing.Color.Empty;
            this.panel.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.panel.FillStyle = Sheng.SailingEase.Controls.FillStyle.Solid;
            this.panel.HighLight = false;
            this.panel.Location = new System.Drawing.Point(5, 36);
            this.panel.Name = "panel";
            this.panel.ShowBorder = false;
            this.panel.Size = new System.Drawing.Size(412, 219);
            this.panel.TabIndex = 4;
            this.seAdvLabel1.BorderColor = System.Drawing.Color.Black;
            this.seAdvLabel1.FillColorEnd = System.Drawing.Color.Empty;
            this.seAdvLabel1.FillColorStart = System.Drawing.SystemColors.ActiveCaption;
            this.seAdvLabel1.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.seAdvLabel1.FillStyle = Sheng.SailingEase.Controls.FillStyle.LinearGradient;
            this.seAdvLabel1.Location = new System.Drawing.Point(5, 29);
            this.seAdvLabel1.Name = "seAdvLabel1";
            this.seAdvLabel1.ShowBorder = false;
            this.seAdvLabel1.SingleLine = false;
            this.seAdvLabel1.Size = new System.Drawing.Size(250, 1);
            this.seAdvLabel1.TabIndex = 5;
            this.seAdvLabel1.TextHorizontalCenter = false;
            this.seAdvLabel1.TextVerticalCenter = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.seAdvLabel1);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.ddlDataSourceMode);
            this.Controls.Add(this.lblDataSourceMode);
            this.Name = "UserControlSEComboBoxExDevDataRule";
            this.Size = new System.Drawing.Size(420, 258);
            this.Load += new System.EventHandler(this.SEComboBoxExDevDataRule_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Label lblDataSourceMode;
        private Sheng.SailingEase.Controls.SEComboBox ddlDataSourceMode;
        private Sheng.SailingEase.Controls.SEPanel panel;
        private Sheng.SailingEase.Controls.SEAdvLabel seAdvLabel1;
    }
}
