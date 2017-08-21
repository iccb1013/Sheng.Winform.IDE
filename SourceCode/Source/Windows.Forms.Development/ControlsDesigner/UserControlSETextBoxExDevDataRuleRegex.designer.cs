/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Windows.Forms.Development
{
    partial class UserControlSETextBoxExDevDataRuleRegex
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
            this.txtRegexMsg = new Sheng.SailingEase.Controls.SETextBox();
            this.lblRegexMsg = new System.Windows.Forms.Label();
            this.txtRegex = new Sheng.SailingEase.Controls.SETextBox();
            this.lblRegex = new System.Windows.Forms.Label();
            this.btnTool = new Sheng.SailingEase.Controls.SEButton();
            this.btnRegexLib = new Sheng.SailingEase.Controls.SEButton();
            this.SuspendLayout();
            this.txtRegexMsg.AllowEmpty = true;
            this.txtRegexMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRegexMsg.HighLight = true;
            this.txtRegexMsg.LimitMaxValue = false;
            this.txtRegexMsg.Location = new System.Drawing.Point(28, 98);
            this.txtRegexMsg.MaxLength = 255;
            this.txtRegexMsg.MaxValue = ((long)(2147483647));
            this.txtRegexMsg.Name = "txtRegexMsg";
            this.txtRegexMsg.Regex = "";
            this.txtRegexMsg.RegexMsg = null;
            this.txtRegexMsg.Size = new System.Drawing.Size(403, 21);
            this.txtRegexMsg.TabIndex = 1;
            this.txtRegexMsg.Title = null;
            this.txtRegexMsg.ValueCompareTo = null;
            this.txtRegexMsg.WaterText = "";
            this.lblRegexMsg.AutoSize = true;
            this.lblRegexMsg.Location = new System.Drawing.Point(3, 80);
            this.lblRegexMsg.Name = "lblRegexMsg";
            this.lblRegexMsg.Size = new System.Drawing.Size(341, 12);
            this.lblRegexMsg.TabIndex = 8;
            this.lblRegexMsg.Text = "${UserControlSETextBoxExDevDataRuleRegex_LabelRegexMsg}:";
            this.txtRegex.AllowEmpty = true;
            this.txtRegex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRegex.HighLight = true;
            this.txtRegex.LimitMaxValue = false;
            this.txtRegex.Location = new System.Drawing.Point(28, 25);
            this.txtRegex.MaxValue = ((long)(2147483647));
            this.txtRegex.Name = "txtRegex";
            this.txtRegex.Regex = "";
            this.txtRegex.RegexMsg = null;
            this.txtRegex.Size = new System.Drawing.Size(403, 21);
            this.txtRegex.TabIndex = 0;
            this.txtRegex.Title = null;
            this.txtRegex.ValueCompareTo = null;
            this.txtRegex.WaterText = "";
            this.lblRegex.AutoSize = true;
            this.lblRegex.Location = new System.Drawing.Point(3, 6);
            this.lblRegex.Name = "lblRegex";
            this.lblRegex.Size = new System.Drawing.Size(323, 12);
            this.lblRegex.TabIndex = 4;
            this.lblRegex.Text = "${UserControlSETextBoxExDevDataRuleRegex_LabelRegex}:";
            this.btnTool.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTool.Location = new System.Drawing.Point(356, 52);
            this.btnTool.Name = "btnTool";
            this.btnTool.Size = new System.Drawing.Size(75, 23);
            this.btnTool.TabIndex = 3;
            this.btnTool.Text = "${UserControlSETextBoxExDevDataRuleRegex_Tool}";
            this.btnTool.UseVisualStyleBackColor = true;
            this.btnTool.Click += new System.EventHandler(this.btnTool_Click);
            this.btnRegexLib.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegexLib.Location = new System.Drawing.Point(275, 52);
            this.btnRegexLib.Name = "btnRegexLib";
            this.btnRegexLib.Size = new System.Drawing.Size(75, 23);
            this.btnRegexLib.TabIndex = 2;
            this.btnRegexLib.Text = "${UserControlSETextBoxExDevDataRuleRegex_RegexLib}";
            this.btnRegexLib.UseVisualStyleBackColor = true;
            this.btnRegexLib.Click += new System.EventHandler(this.btnRegexLib_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRegexLib);
            this.Controls.Add(this.btnTool);
            this.Controls.Add(this.txtRegexMsg);
            this.Controls.Add(this.lblRegexMsg);
            this.Controls.Add(this.txtRegex);
            this.Controls.Add(this.lblRegex);
            this.Name = "UserControlSETextBoxExDevDataRuleRegex";
            this.Size = new System.Drawing.Size(434, 265);
            this.Load += new System.EventHandler(this.UserControlSETextBoxExDevDataRuleRegex_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Sheng.SailingEase.Controls.SETextBox txtRegexMsg;
        private System.Windows.Forms.Label lblRegexMsg;
        private Sheng.SailingEase.Controls.SETextBox txtRegex;
        private System.Windows.Forms.Label lblRegex;
        private Sheng.SailingEase.Controls.SEButton btnTool;
        private Sheng.SailingEase.Controls.SEButton btnRegexLib;
    }
}
