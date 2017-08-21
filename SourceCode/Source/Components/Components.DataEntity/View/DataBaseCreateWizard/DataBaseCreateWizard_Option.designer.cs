/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    partial class DataBaseCreateWizard_Option
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbNoCreateDataBase = new System.Windows.Forms.CheckBox();
            this.cbInsertEnum = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblNotice = new Sheng.SailingEase.Controls.SEAdvLabel();
            this.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(347, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "${UserControlCreateDataBaseWizardStepOption_LabelOption}:";
            this.cbNoCreateDataBase.AutoSize = true;
            this.cbNoCreateDataBase.Location = new System.Drawing.Point(31, 24);
            this.cbNoCreateDataBase.Name = "cbNoCreateDataBase";
            this.cbNoCreateDataBase.Size = new System.Drawing.Size(438, 16);
            this.cbNoCreateDataBase.TabIndex = 1;
            this.cbNoCreateDataBase.Text = "${UserControlCreateDataBaseWizardStepOption_CheckBoxNoCreateDataBase}";
            this.cbNoCreateDataBase.UseVisualStyleBackColor = true;
            this.cbInsertEnum.AutoSize = true;
            this.cbInsertEnum.Checked = true;
            this.cbInsertEnum.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbInsertEnum.Location = new System.Drawing.Point(31, 66);
            this.cbInsertEnum.Name = "cbInsertEnum";
            this.cbInsertEnum.Size = new System.Drawing.Size(402, 16);
            this.cbInsertEnum.TabIndex = 2;
            this.cbInsertEnum.Text = "${UserControlCreateDataBaseWizardStepOption_CheckBoxInsertEnum}";
            this.cbInsertEnum.UseVisualStyleBackColor = true;
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(467, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "${UserControlCreateDataBaseWizardStepOption_LabelNoCreateDataBaseDescription}";
            this.lblNotice.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblNotice.FillColorEnd = System.Drawing.Color.Empty;
            this.lblNotice.FillColorStart = System.Drawing.SystemColors.Info;
            this.lblNotice.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.lblNotice.FillStyle = Sheng.SailingEase.Controls.FillStyle.Solid;
            this.lblNotice.Location = new System.Drawing.Point(0, 212);
            this.lblNotice.Name = "lblNotice";
            this.lblNotice.ShowBorder = true;
            this.lblNotice.SingleLine = false;
            this.lblNotice.Size = new System.Drawing.Size(505, 38);
            this.lblNotice.TabIndex = 32;
            this.lblNotice.Text = " ${UserControlCreateDataBaseWizardStepOption_Description}";
            this.lblNotice.TextHorizontalCenter = false;
            this.lblNotice.TextVerticalCenter = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblNotice);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbInsertEnum);
            this.Controls.Add(this.cbNoCreateDataBase);
            this.Controls.Add(this.label1);
            this.Name = "UserControlCreateDataBaseWizardStepOption";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbNoCreateDataBase;
        private System.Windows.Forms.CheckBox cbInsertEnum;
        private System.Windows.Forms.Label label3;
        private Sheng.SailingEase.Controls.SEAdvLabel lblNotice;
    }
}
