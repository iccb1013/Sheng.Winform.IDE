/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class FormEventReceiveDataSet
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
            this.txtDataCode = new Sheng.SailingEase.Controls.SETextBox();
            this.lblDataCode = new System.Windows.Forms.Label();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.lblReceiveTo = new System.Windows.Forms.Label();
            this.ddlFormElement = new FormElementComboBox();
            this.SuspendLayout();
            this.txtDataCode.AllowEmpty = false;
            this.txtDataCode.HighLight = true;
            this.txtDataCode.LimitMaxValue = false;
            this.txtDataCode.Location = new System.Drawing.Point(103, 19);
            this.txtDataCode.MaxLength = 100;
            this.txtDataCode.MaxValue = ((long)(2147483647));
            this.txtDataCode.Name = "txtDataCode";
            this.txtDataCode.Regex = "";
            this.txtDataCode.RegexMsg = "";
            this.txtDataCode.Size = new System.Drawing.Size(322, 21);
            this.txtDataCode.TabIndex = 25;
            this.txtDataCode.Title = "";
            this.txtDataCode.ValueCompareTo = null;
            this.txtDataCode.WaterText = "";
            this.lblDataCode.AutoSize = true;
            this.lblDataCode.Location = new System.Drawing.Point(12, 22);
            this.lblDataCode.Name = "lblDataCode";
            this.lblDataCode.Size = new System.Drawing.Size(251, 12);
            this.lblDataCode.TabIndex = 24;
            this.lblDataCode.Text = "${FormEventReceiveDataSet_LabelDataCode}:";
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(351, 119);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 36;
            this.btnOK.Text = "${FormEventReceiveDataSet_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(432, 119);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 37;
            this.btnCancel.Text = "${FormEventReceiveDataSet_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.lblReceiveTo.AutoSize = true;
            this.lblReceiveTo.Location = new System.Drawing.Point(12, 49);
            this.lblReceiveTo.Name = "lblReceiveTo";
            this.lblReceiveTo.Size = new System.Drawing.Size(257, 12);
            this.lblReceiveTo.TabIndex = 39;
            this.lblReceiveTo.Text = "${FormEventReceiveDataSet_LabelReceiveTo}:";
            this.ddlFormElement.AllowDataSource = EnumEventDataSource.FormElement;
            this.ddlFormElement.AllowEmpty = false;
            this.ddlFormElement.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ddlFormElement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlFormElement.FormattingEnabled = true;
            this.ddlFormElement.FormEntity = null;
            this.ddlFormElement.HighLight = true;
            this.ddlFormElement.Location = new System.Drawing.Point(103, 46);
            this.ddlFormElement.MaxDropDownItems = 16;
            this.ddlFormElement.Name = "ddlFormElement";
            this.ddlFormElement.SelectedFormElementId = "";
            this.ddlFormElement.Size = new System.Drawing.Size(322, 22);
            this.ddlFormElement.TabIndex = 40;
            this.ddlFormElement.Title = null;
            this.ddlFormElement.WaterText = "";
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(519, 154);
            this.Controls.Add(this.ddlFormElement);
            this.Controls.Add(this.lblReceiveTo);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtDataCode);
            this.Controls.Add(this.lblDataCode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEventReceiveDataSet";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${FormEventReceiveDataSet}";
            this.Load += new System.EventHandler(this.FormEventReceiveDataSet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Sheng.SailingEase.Controls.SETextBox txtDataCode;
        private System.Windows.Forms.Label lblDataCode;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private System.Windows.Forms.Label lblReceiveTo;
        private FormElementComboBox ddlFormElement;
    }
}
