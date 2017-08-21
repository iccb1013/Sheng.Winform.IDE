/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    partial class DataBaseCreateWizard_Account
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
            this.txtPassword = new Sheng.SailingEase.Controls.SETextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUserId = new System.Windows.Forms.Label();
            this.txtLoginName = new Sheng.SailingEase.Controls.SETextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNotice = new Sheng.SailingEase.Controls.SEAdvLabel();
            this.cbNoPasswordChar = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            this.txtPassword.AllowEmpty = true;
            this.txtPassword.HighLight = true;
            this.txtPassword.LimitMaxValue = false;
            this.txtPassword.Location = new System.Drawing.Point(91, 51);
            this.txtPassword.MaxValue = ((long)(2147483647));
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Regex = "";
            this.txtPassword.RegexMsg = null;
            this.txtPassword.Size = new System.Drawing.Size(238, 21);
            this.txtPassword.TabIndex = 18;
            this.txtPassword.Title = "密码";
            this.txtPassword.ValueCompareTo = null;
            this.txtPassword.WaterText = "";
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(28, 54);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(365, 12);
            this.lblPassword.TabIndex = 17;
            this.lblPassword.Text = "${UserControlCreateDataBaseWizardStepAccount_LabelPassword}:";
            this.lblUserId.AutoSize = true;
            this.lblUserId.Location = new System.Drawing.Point(28, 27);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(359, 12);
            this.lblUserId.TabIndex = 16;
            this.lblUserId.Text = "${UserControlCreateDataBaseWizardStepAccount_LabelAccount}:";
            this.txtLoginName.AllowEmpty = false;
            this.txtLoginName.HighLight = true;
            this.txtLoginName.LimitMaxValue = false;
            this.txtLoginName.Location = new System.Drawing.Point(91, 24);
            this.txtLoginName.MaxValue = ((long)(2147483647));
            this.txtLoginName.Name = "txtLoginName";
            this.txtLoginName.Regex = "";
            this.txtLoginName.RegexMsg = null;
            this.txtLoginName.Size = new System.Drawing.Size(238, 21);
            this.txtLoginName.TabIndex = 15;
            this.txtLoginName.Text = "Administrator";
            this.txtLoginName.Title = "帐户";
            this.txtLoginName.ValueCompareTo = null;
            this.txtLoginName.WaterText = "";
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(431, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "${UserControlCreateDataBaseWizardStepAccount_LabelDefaultAdminAccount}:";
            this.lblNotice.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblNotice.FillColorEnd = System.Drawing.Color.Empty;
            this.lblNotice.FillColorStart = System.Drawing.SystemColors.Info;
            this.lblNotice.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.lblNotice.FillStyle = Sheng.SailingEase.Controls.FillStyle.Solid;
            this.lblNotice.Location = new System.Drawing.Point(0, 207);
            this.lblNotice.Name = "lblNotice";
            this.lblNotice.ShowBorder = true;
            this.lblNotice.SingleLine = false;
            this.lblNotice.Size = new System.Drawing.Size(505, 43);
            this.lblNotice.TabIndex = 33;
            this.lblNotice.Text = " ${UserControlCreateDataBaseWizardStepAccount_Description}";
            this.lblNotice.TextHorizontalCenter = false;
            this.lblNotice.TextVerticalCenter = true;
            this.cbNoPasswordChar.AutoSize = true;
            this.cbNoPasswordChar.Location = new System.Drawing.Point(91, 78);
            this.cbNoPasswordChar.Name = "cbNoPasswordChar";
            this.cbNoPasswordChar.Size = new System.Drawing.Size(432, 16);
            this.cbNoPasswordChar.TabIndex = 34;
            this.cbNoPasswordChar.Text = "${UserControlCreateDataBaseWizardStepAccount_CheckBoxNoPasswordChar}";
            this.cbNoPasswordChar.UseVisualStyleBackColor = true;
            this.cbNoPasswordChar.CheckedChanged += new System.EventHandler(this.cbNoPasswordChar_CheckedChanged);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbNoPasswordChar);
            this.Controls.Add(this.lblNotice);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtLoginName);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUserId);
            this.Name = "UserControlCreateDataBaseWizardStepAccount";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Sheng.SailingEase.Controls.SETextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUserId;
        private Sheng.SailingEase.Controls.SETextBox txtLoginName;
        private System.Windows.Forms.Label label1;
        private Sheng.SailingEase.Controls.SEAdvLabel lblNotice;
        private System.Windows.Forms.CheckBox cbNoPasswordChar;
    }
}
