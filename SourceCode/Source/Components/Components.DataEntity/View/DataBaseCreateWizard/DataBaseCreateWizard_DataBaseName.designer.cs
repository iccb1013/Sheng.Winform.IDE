/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    partial class DataBaseCreateWizard_DataBaseName
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
            this.txtDataBaseName = new Sheng.SailingEase.Controls.SETextBox();
            this.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(449, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "${UserControlCreateDataBaseWizardStepEnterDataBaseName_LabelDataBaseName}:";
            this.txtDataBaseName.AllowEmpty = false;
            this.txtDataBaseName.HighLight = true;
            this.txtDataBaseName.LimitMaxValue = false;
            this.txtDataBaseName.Location = new System.Drawing.Point(28, 22);
            this.txtDataBaseName.MaxValue = ((long)(2147483647));
            this.txtDataBaseName.Name = "txtDataBaseName";
            this.txtDataBaseName.Regex = "";
            this.txtDataBaseName.RegexMsg = null;
            this.txtDataBaseName.Size = new System.Drawing.Size(346, 21);
            this.txtDataBaseName.TabIndex = 1;
            this.txtDataBaseName.Title = "数据库名";
            this.txtDataBaseName.ValueCompareTo = null;
            this.txtDataBaseName.WaterText = "";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtDataBaseName);
            this.Controls.Add(this.label1);
            this.Name = "UserControlCreateDataBaseWizardStepEnterDataBaseName";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Label label1;
        private Sheng.SailingEase.Controls.SETextBox txtDataBaseName;
    }
}
