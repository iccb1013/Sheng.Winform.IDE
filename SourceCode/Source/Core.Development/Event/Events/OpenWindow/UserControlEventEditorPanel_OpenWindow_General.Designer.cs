namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlEventEditorPanel_OpenWindow_General
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtCode = new Sheng.SailingEase.Controls.SETextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtName = new Sheng.SailingEase.Controls.SETextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblIfOpend = new System.Windows.Forms.Label();
            this.ddlIfOpend = new Sheng.SailingEase.Controls.SEComboBox();
            this.cbIsDiablog = new System.Windows.Forms.CheckBox();
            this.txtFormName = new Sheng.SailingEase.Controls.SETextBox();
            this.lblFormName = new System.Windows.Forms.Label();
            this.btnBrowseForm = new Sheng.SailingEase.Controls.SEButton();
            this.SuspendLayout();
            // 
            // txtCode
            // 
            this.txtCode.AllowEmpty = false;
            this.txtCode.CustomValidate = null;
            this.txtCode.HighLight = true;
            this.txtCode.LimitMaxValue = false;
            this.txtCode.Location = new System.Drawing.Point(275, 32);
            this.txtCode.MaxLength = 100;
            this.txtCode.MaxValue = ((long)(2147483647));
            this.txtCode.Name = "txtCode";
            this.txtCode.Regex = "";
            this.txtCode.RegexMsg = "RegexMsg_OnlyAllowMonogram";
            this.txtCode.Size = new System.Drawing.Size(120, 21);
            this.txtCode.TabIndex = 1;
            this.txtCode.Title = "LabelCode";
            this.txtCode.ValueCompareTo = null;
            this.txtCode.WaterText = "";
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(210, 35);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(353, 12);
            this.lblCode.TabIndex = 26;
            this.lblCode.Text = "${UserControlEventEditorPanel_OpenForm_General_LabelCode}:";
            // 
            // txtName
            // 
            this.txtName.AllowEmpty = false;
            this.txtName.CustomValidate = null;
            this.txtName.HighLight = true;
            this.txtName.LimitMaxValue = false;
            this.txtName.Location = new System.Drawing.Point(69, 32);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.txtName.MaxLength = 100;
            this.txtName.MaxValue = ((long)(2147483647));
            this.txtName.Name = "txtName";
            this.txtName.Regex = "";
            this.txtName.RegexMsg = null;
            this.txtName.Size = new System.Drawing.Size(120, 21);
            this.txtName.TabIndex = 0;
            this.txtName.Title = "LabelName";
            this.txtName.ValueCompareTo = null;
            this.txtName.WaterText = "";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(3, 35);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(353, 12);
            this.lblName.TabIndex = 24;
            this.lblName.Text = "${UserControlEventEditorPanel_OpenForm_General_LabelName}:";
            // 
            // lblIfOpend
            // 
            this.lblIfOpend.AutoSize = true;
            this.lblIfOpend.Location = new System.Drawing.Point(4, 89);
            this.lblIfOpend.Name = "lblIfOpend";
            this.lblIfOpend.Size = new System.Drawing.Size(341, 12);
            this.lblIfOpend.TabIndex = 34;
            this.lblIfOpend.Text = "${UserControlEventEditorPanel_OpenForm_General_IfOpend}:";
            // 
            // ddlIfOpend
            // 
            this.ddlIfOpend.AllowEmpty = true;
            this.ddlIfOpend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlIfOpend.CustomValidate = null;
            this.ddlIfOpend.DisplayMember = "Text";
            this.ddlIfOpend.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlIfOpend.FormattingEnabled = true;
            this.ddlIfOpend.HighLight = true;
            this.ddlIfOpend.Location = new System.Drawing.Point(69, 86);
            this.ddlIfOpend.Name = "ddlIfOpend";
            this.ddlIfOpend.Size = new System.Drawing.Size(324, 20);
            this.ddlIfOpend.TabIndex = 4;
            this.ddlIfOpend.Title = null;
            this.ddlIfOpend.ValueMember = "Value";
            this.ddlIfOpend.WaterText = "";
            // 
            // cbIsDiablog
            // 
            this.cbIsDiablog.AutoSize = true;
            this.cbIsDiablog.Location = new System.Drawing.Point(6, 112);
            this.cbIsDiablog.Name = "cbIsDiablog";
            this.cbIsDiablog.Size = new System.Drawing.Size(414, 16);
            this.cbIsDiablog.TabIndex = 5;
            this.cbIsDiablog.Text = "${UserControlEventEditorPanel_OpenForm_General_CheckBoxIsDiablog}";
            this.cbIsDiablog.UseVisualStyleBackColor = true;
            // 
            // txtFormName
            // 
            this.txtFormName.AllowEmpty = false;
            this.txtFormName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFormName.CustomValidate = null;
            this.txtFormName.HighLight = true;
            this.txtFormName.LimitMaxValue = false;
            this.txtFormName.Location = new System.Drawing.Point(69, 59);
            this.txtFormName.MaxValue = ((long)(2147483647));
            this.txtFormName.Name = "txtFormName";
            this.txtFormName.ReadOnly = true;
            this.txtFormName.Regex = "";
            this.txtFormName.RegexMsg = null;
            this.txtFormName.Size = new System.Drawing.Size(324, 21);
            this.txtFormName.TabIndex = 2;
            this.txtFormName.Title = "LabelFormName";
            this.txtFormName.ValueCompareTo = null;
            this.txtFormName.WaterText = "";
            // 
            // lblFormName
            // 
            this.lblFormName.AutoSize = true;
            this.lblFormName.Location = new System.Drawing.Point(4, 62);
            this.lblFormName.Name = "lblFormName";
            this.lblFormName.Size = new System.Drawing.Size(377, 12);
            this.lblFormName.TabIndex = 29;
            this.lblFormName.Text = "${UserControlEventEditorPanel_OpenForm_General_LabelFormName}:";
            // 
            // btnBrowseForm
            // 
            this.btnBrowseForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseForm.Location = new System.Drawing.Point(399, 57);
            this.btnBrowseForm.Name = "btnBrowseForm";
            this.btnBrowseForm.Size = new System.Drawing.Size(31, 23);
            this.btnBrowseForm.TabIndex = 3;
            this.btnBrowseForm.Text = "...";
            this.btnBrowseForm.UseVisualStyleBackColor = true;
            this.btnBrowseForm.Click += new System.EventHandler(this.btnBrowseForm_Click);
            // 
            // UserControlEventEditorPanel_OpenForm_General
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ddlIfOpend);
            this.Controls.Add(this.cbIsDiablog);
            this.Controls.Add(this.txtFormName);
            this.Controls.Add(this.lblFormName);
            this.Controls.Add(this.btnBrowseForm);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblIfOpend);
            this.Name = "UserControlEventEditorPanel_OpenForm_General";
            this.Controls.SetChildIndex(this.lblIfOpend, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.lblCode, 0);
            this.Controls.SetChildIndex(this.txtCode, 0);
            this.Controls.SetChildIndex(this.btnBrowseForm, 0);
            this.Controls.SetChildIndex(this.lblFormName, 0);
            this.Controls.SetChildIndex(this.txtFormName, 0);
            this.Controls.SetChildIndex(this.cbIsDiablog, 0);
            this.Controls.SetChildIndex(this.ddlIfOpend, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sheng.SailingEase.Controls.SETextBox txtCode;
        private System.Windows.Forms.Label lblCode;
        private Sheng.SailingEase.Controls.SETextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblIfOpend;
        private Sheng.SailingEase.Controls.SEComboBox ddlIfOpend;
        private System.Windows.Forms.CheckBox cbIsDiablog;
        private Sheng.SailingEase.Controls.SETextBox txtFormName;
        private System.Windows.Forms.Label lblFormName;
        private Sheng.SailingEase.Controls.SEButton btnBrowseForm;
    }
}
