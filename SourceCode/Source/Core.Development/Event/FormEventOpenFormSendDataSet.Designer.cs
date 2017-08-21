/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class FormEventOpenFormSendDataSet
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
            this.lblDataName = new System.Windows.Forms.Label();
            this.txtDataName = new Sheng.SailingEase.Controls.SETextBox();
            this.txtDataCode = new Sheng.SailingEase.Controls.SETextBox();
            this.lblDataCode = new System.Windows.Forms.Label();
            this.lblDataSource = new System.Windows.Forms.Label();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.ddlFormElement = new Sheng.SailingEase.Core.Development.FormElementComboBox();
            this.SuspendLayout();
            this.lblDataName.AutoSize = true;
            this.lblDataName.Location = new System.Drawing.Point(12, 20);
            this.lblDataName.Name = "lblDataName";
            this.lblDataName.Size = new System.Drawing.Size(281, 12);
            this.lblDataName.TabIndex = 20;
            this.lblDataName.Text = "${FormEventOpenFormSendDataSet_LabelDataName}:";
            this.txtDataName.AllowEmpty = false;
            this.txtDataName.CustomValidate = null;
            this.txtDataName.HighLight = true;
            this.txtDataName.LimitMaxValue = false;
            this.txtDataName.Location = new System.Drawing.Point(78, 17);
            this.txtDataName.MaxLength = 100;
            this.txtDataName.MaxValue = ((long)(2147483647));
            this.txtDataName.Name = "txtDataName";
            this.txtDataName.Regex = "";
            this.txtDataName.RegexMsg = null;
            this.txtDataName.Size = new System.Drawing.Size(245, 21);
            this.txtDataName.TabIndex = 21;
            this.txtDataName.Title = "LabelDataName";
            this.txtDataName.ValueCompareTo = null;
            this.txtDataName.WaterText = "";
            this.txtDataCode.AllowEmpty = false;
            this.txtDataCode.CustomValidate = null;
            this.txtDataCode.HighLight = true;
            this.txtDataCode.LimitMaxValue = false;
            this.txtDataCode.Location = new System.Drawing.Point(78, 44);
            this.txtDataCode.MaxLength = 100;
            this.txtDataCode.MaxValue = ((long)(2147483647));
            this.txtDataCode.Name = "txtDataCode";
            this.txtDataCode.Regex = "";
            this.txtDataCode.RegexMsg = "";
            this.txtDataCode.Size = new System.Drawing.Size(245, 21);
            this.txtDataCode.TabIndex = 23;
            this.txtDataCode.Title = "";
            this.txtDataCode.ValueCompareTo = null;
            this.txtDataCode.WaterText = "";
            this.lblDataCode.AutoSize = true;
            this.lblDataCode.Location = new System.Drawing.Point(12, 47);
            this.lblDataCode.Name = "lblDataCode";
            this.lblDataCode.Size = new System.Drawing.Size(281, 12);
            this.lblDataCode.TabIndex = 22;
            this.lblDataCode.Text = "${FormEventOpenFormSendDataSet_LabelDataCode}:";
            this.lblDataSource.AutoSize = true;
            this.lblDataSource.Location = new System.Drawing.Point(12, 74);
            this.lblDataSource.Name = "lblDataSource";
            this.lblDataSource.Size = new System.Drawing.Size(293, 12);
            this.lblDataSource.TabIndex = 29;
            this.lblDataSource.Text = "${FormEventOpenFormSendDataSet_LabelDataSource}:";
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(346, 140);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 34;
            this.btnOK.Text = "${FormEventOpenFormSendDataSet_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(427, 140);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 35;
            this.btnCancel.Text = "${FormEventOpenFormSendDataSet_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.ddlFormElement.AllowDataSource = Sheng.SailingEase.Core.EnumEventDataSource.FormElement;
            this.ddlFormElement.AllowEmpty = false;
            this.ddlFormElement.CustomValidate = null;
            this.ddlFormElement.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ddlFormElement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlFormElement.FormattingEnabled = true;
            this.ddlFormElement.FormEntity = null;
            this.ddlFormElement.HighLight = true;
            this.ddlFormElement.Location = new System.Drawing.Point(78, 71);
            this.ddlFormElement.MaxDropDownItems = 16;
            this.ddlFormElement.Name = "ddlFormElement";
            this.ddlFormElement.SelectedFormElementId = "";
            this.ddlFormElement.Size = new System.Drawing.Size(382, 22);
            this.ddlFormElement.TabIndex = 36;
            this.ddlFormElement.Title = "FormEventOpenFormSendDataSet_ComboBoxFormElement";
            this.ddlFormElement.WaterText = "";
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(514, 175);
            this.Controls.Add(this.lblDataSource);
            this.Controls.Add(this.lblDataCode);
            this.Controls.Add(this.lblDataName);
            this.Controls.Add(this.ddlFormElement);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtDataCode);
            this.Controls.Add(this.txtDataName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEventOpenFormSendDataSet";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "${FormEventOpenFormSendDataSet}";
            this.Load += new System.EventHandler(this.FormEventOpenFormSendDataSet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Label lblDataName;
        private Sheng.SailingEase.Controls.SETextBox txtDataName;
        private Sheng.SailingEase.Controls.SETextBox txtDataCode;
        private System.Windows.Forms.Label lblDataCode;
        private System.Windows.Forms.Label lblDataSource;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private FormElementComboBox ddlFormElement;
    }
}
