/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class FormEventReturnDataToCallerFormSet
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
            this.lblFormElementCode = new System.Windows.Forms.Label();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.txtFormElementCode = new Sheng.SailingEase.Controls.SETextBox();
            this.lblDataSource = new System.Windows.Forms.Label();
            this.ddlFormElement = new FormElementComboBox();
            this.btnBrowse = new Sheng.SailingEase.Controls.SEButton();
            this.SuspendLayout();
            this.lblFormElementCode.AutoSize = true;
            this.lblFormElementCode.Location = new System.Drawing.Point(12, 29);
            this.lblFormElementCode.Name = "lblFormElementCode";
            this.lblFormElementCode.Size = new System.Drawing.Size(353, 12);
            this.lblFormElementCode.TabIndex = 0;
            this.lblFormElementCode.Text = "${FormEventReturnDataToCallerFormSet_LabelFormElementCode}";
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(380, 119);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 33;
            this.btnOK.Text = "${FormEventReturnDataToCallerFormSet_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(461, 119);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 34;
            this.btnCancel.Text = "${FormEventReturnDataToCallerFormSet_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.txtFormElementCode.AllowEmpty = false;
            this.txtFormElementCode.HighLight = true;
            this.txtFormElementCode.LimitMaxValue = false;
            this.txtFormElementCode.Location = new System.Drawing.Point(114, 26);
            this.txtFormElementCode.MaxLength = 100;
            this.txtFormElementCode.MaxValue = ((long)(2147483647));
            this.txtFormElementCode.Name = "txtFormElementCode";
            this.txtFormElementCode.Regex = "";
            this.txtFormElementCode.RegexMsg = "";
            this.txtFormElementCode.Size = new System.Drawing.Size(325, 21);
            this.txtFormElementCode.TabIndex = 35;
            this.txtFormElementCode.Title = "";
            this.txtFormElementCode.ValueCompareTo = null;
            this.txtFormElementCode.WaterText = "";
            this.lblDataSource.AutoSize = true;
            this.lblDataSource.Location = new System.Drawing.Point(12, 56);
            this.lblDataSource.Name = "lblDataSource";
            this.lblDataSource.Size = new System.Drawing.Size(323, 12);
            this.lblDataSource.TabIndex = 36;
            this.lblDataSource.Text = "${FormEventReturnDataToCallerFormSet_LabelDataSource}";
            this.ddlFormElement.AllowDataSource = EnumEventDataSource.FormElement;
            this.ddlFormElement.AllowEmpty = false;
            this.ddlFormElement.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ddlFormElement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlFormElement.FormattingEnabled = true;
            this.ddlFormElement.FormEntity = null;
            this.ddlFormElement.HighLight = true;
            this.ddlFormElement.Location = new System.Drawing.Point(113, 53);
            this.ddlFormElement.MaxDropDownItems = 16;
            this.ddlFormElement.Name = "ddlFormElement";
            this.ddlFormElement.SelectedFormElementId = "";
            this.ddlFormElement.Size = new System.Drawing.Size(326, 22);
            this.ddlFormElement.TabIndex = 39;
            this.ddlFormElement.Title = null;
            this.ddlFormElement.WaterText = "";
            this.btnBrowse.Location = new System.Drawing.Point(445, 24);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(59, 23);
            this.btnBrowse.TabIndex = 40;
            this.btnBrowse.Text = "${FormEventReturnDataToCallerFormSet_ButtonBrowse}";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(548, 154);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.ddlFormElement);
            this.Controls.Add(this.lblDataSource);
            this.Controls.Add(this.txtFormElementCode);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblFormElementCode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormEventReturnDataToCallerFormSet";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${FormEventReturnDataToCallerFormSet}";
            this.Load += new System.EventHandler(this.FormEventReturnDataToCallerFormSet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Label lblFormElementCode;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private Sheng.SailingEase.Controls.SETextBox txtFormElementCode;
        private System.Windows.Forms.Label lblDataSource;
        private FormElementComboBox ddlFormElement;
        private Sheng.SailingEase.Controls.SEButton btnBrowse;
    }
}
