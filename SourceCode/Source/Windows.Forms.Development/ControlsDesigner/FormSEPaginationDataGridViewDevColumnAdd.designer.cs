namespace Sheng.SailingEase.Windows.Forms.Development
{
    partial class FormSEPaginationDataGridViewDevColumnAdd
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtText = new Sheng.SailingEase.Controls.SETextBox();
            this.lblHeaderText = new System.Windows.Forms.Label();
            this.cbVisible = new System.Windows.Forms.CheckBox();
            this.txtDataPropertyName = new Sheng.SailingEase.Controls.SETextBox();
            this.lblDataPropertyName = new System.Windows.Forms.Label();
            this.lblDataItem = new System.Windows.Forms.Label();
            this.ddlDataItem = new Sheng.SailingEase.Controls.SEComboBox();
            this.radioButtonUnBind = new System.Windows.Forms.RadioButton();
            this.radioButtonBind = new System.Windows.Forms.RadioButton();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.txtCode = new Sheng.SailingEase.Controls.SETextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtName = new Sheng.SailingEase.Controls.SETextBox();
            this.seGroupBox2 = new Sheng.SailingEase.Controls.SEGroupBox();
            this.panelDataRule = new Sheng.SailingEase.Controls.SEPanel();
            this.ddlDataRule = new Sheng.SailingEase.Controls.SEComboBox();
            this.seTabControl1 = new Sheng.SailingEase.Controls.SETabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnClear = new Sheng.SailingEase.Controls.SEButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.seGroupBox2.SuspendLayout();
            this.seTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtText
            // 
            this.txtText.AllowEmpty = true;
            this.txtText.HighLight = true;
            this.txtText.LimitMaxValue = false;
            this.txtText.Location = new System.Drawing.Point(118, 69);
            this.txtText.MaxLength = 200;
            this.txtText.MaxValue = ((long)(2147483647));
            this.txtText.Name = "txtText";
            this.txtText.Regex = "";
            this.txtText.RegexMsg = "";
            this.txtText.Size = new System.Drawing.Size(154, 21);
            this.txtText.TabIndex = 34;
            this.txtText.Title = "";
            this.txtText.ValueCompareTo = null;
            this.txtText.WaterText = "";
            // 
            // lblHeaderText
            // 
            this.lblHeaderText.AutoSize = true;
            this.lblHeaderText.Location = new System.Drawing.Point(19, 72);
            this.lblHeaderText.Name = "lblHeaderText";
            this.lblHeaderText.Size = new System.Drawing.Size(365, 12);
            this.lblHeaderText.TabIndex = 35;
            this.lblHeaderText.Text = "${FormSEPaginationDataGridViewDevColumnAdd_LabelHeaderText}:";
            // 
            // cbVisible
            // 
            this.cbVisible.AutoSize = true;
            this.cbVisible.Checked = true;
            this.cbVisible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbVisible.Location = new System.Drawing.Point(21, 232);
            this.cbVisible.Name = "cbVisible";
            this.cbVisible.Size = new System.Drawing.Size(378, 16);
            this.cbVisible.TabIndex = 27;
            this.cbVisible.Text = "${FormSEPaginationDataGridViewDevColumnAdd_CheckBoxVisible}";
            this.cbVisible.UseVisualStyleBackColor = true;
            // 
            // txtDataPropertyName
            // 
            this.txtDataPropertyName.AllowEmpty = true;
            this.txtDataPropertyName.HighLight = true;
            this.txtDataPropertyName.LimitMaxValue = false;
            this.txtDataPropertyName.Location = new System.Drawing.Point(104, 40);
            this.txtDataPropertyName.MaxLength = 100;
            this.txtDataPropertyName.MaxValue = ((long)(2147483647));
            this.txtDataPropertyName.Name = "txtDataPropertyName";
            this.txtDataPropertyName.Regex = "";
            this.txtDataPropertyName.RegexMsg = "RegexMsg_OnlyAllowMonogram";
            this.txtDataPropertyName.Size = new System.Drawing.Size(210, 21);
            this.txtDataPropertyName.TabIndex = 26;
            this.txtDataPropertyName.Title = "";
            this.txtDataPropertyName.ValueCompareTo = null;
            this.txtDataPropertyName.WaterText = "";
            // 
            // lblDataPropertyName
            // 
            this.lblDataPropertyName.AutoSize = true;
            this.lblDataPropertyName.Location = new System.Drawing.Point(19, 44);
            this.lblDataPropertyName.Name = "lblDataPropertyName";
            this.lblDataPropertyName.Size = new System.Drawing.Size(371, 12);
            this.lblDataPropertyName.TabIndex = 33;
            this.lblDataPropertyName.Text = "${FormSEPaginationDataGridViewDevColumnAdd_DataPropertyName}:";
            // 
            // lblDataItem
            // 
            this.lblDataItem.AutoSize = true;
            this.lblDataItem.Location = new System.Drawing.Point(19, 44);
            this.lblDataItem.Name = "lblDataItem";
            this.lblDataItem.Size = new System.Drawing.Size(323, 12);
            this.lblDataItem.TabIndex = 32;
            this.lblDataItem.Text = "${FormSEPaginationDataGridViewDevColumnAdd_DataItem}:";
            // 
            // ddlDataItem
            // 
            this.ddlDataItem.AllowEmpty = true;
            this.ddlDataItem.DisplayMember = "Name";
            this.ddlDataItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDataItem.FormattingEnabled = true;
            this.ddlDataItem.HighLight = true;
            this.ddlDataItem.Location = new System.Drawing.Point(104, 40);
            this.ddlDataItem.Name = "ddlDataItem";
            this.ddlDataItem.Size = new System.Drawing.Size(210, 20);
            this.ddlDataItem.TabIndex = 24;
            this.ddlDataItem.Title = "";
            this.ddlDataItem.ValueMember = "Id";
            this.ddlDataItem.WaterText = "";
            this.ddlDataItem.SelectedIndexChanged += new System.EventHandler(this.ddlDataItem_SelectedIndexChanged);
            // 
            // radioButtonUnBind
            // 
            this.radioButtonUnBind.AutoSize = true;
            this.radioButtonUnBind.Location = new System.Drawing.Point(126, 20);
            this.radioButtonUnBind.Name = "radioButtonUnBind";
            this.radioButtonUnBind.Size = new System.Drawing.Size(389, 16);
            this.radioButtonUnBind.TabIndex = 25;
            this.radioButtonUnBind.TabStop = true;
            this.radioButtonUnBind.Text = "${FormSEPaginationDataGridViewDevColumnAdd_RadioButtonUnBind}";
            this.radioButtonUnBind.UseVisualStyleBackColor = true;
            // 
            // radioButtonBind
            // 
            this.radioButtonBind.AutoSize = true;
            this.radioButtonBind.Location = new System.Drawing.Point(7, 20);
            this.radioButtonBind.Name = "radioButtonBind";
            this.radioButtonBind.Size = new System.Drawing.Size(377, 16);
            this.radioButtonBind.TabIndex = 23;
            this.radioButtonBind.TabStop = true;
            this.radioButtonBind.Text = "${FormSEPaginationDataGridViewDevColumnAdd_RadioButtonBind}";
            this.radioButtonBind.UseVisualStyleBackColor = true;
            this.radioButtonBind.CheckedChanged += new System.EventHandler(this.radioButtonBind_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(362, 312);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 29;
            this.btnOK.Text = "${FormSEPaginationDataGridViewDevColumnAdd_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(443, 312);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "${FormSEPaginationDataGridViewDevColumnAdd_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtCode
            // 
            this.txtCode.AllowEmpty = false;
            this.txtCode.HighLight = true;
            this.txtCode.LimitMaxValue = false;
            this.txtCode.Location = new System.Drawing.Point(118, 42);
            this.txtCode.MaxLength = 100;
            this.txtCode.MaxValue = ((long)(2147483647));
            this.txtCode.Name = "txtCode";
            this.txtCode.Regex = "";
            this.txtCode.RegexMsg = "RegexMsg_OnlyAllowMonogram";
            this.txtCode.Size = new System.Drawing.Size(154, 21);
            this.txtCode.TabIndex = 22;
            this.txtCode.Title = "";
            this.txtCode.ValueCompareTo = null;
            this.txtCode.WaterText = "";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(19, 18);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(329, 12);
            this.lblName.TabIndex = 28;
            this.lblName.Text = "${FormSEPaginationDataGridViewDevColumnAdd_LabelName}:";
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(19, 45);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(329, 12);
            this.lblCode.TabIndex = 31;
            this.lblCode.Text = "${FormSEPaginationDataGridViewDevColumnAdd_LabelCode}:";
            // 
            // txtName
            // 
            this.txtName.AllowEmpty = false;
            this.txtName.HighLight = true;
            this.txtName.LimitMaxValue = false;
            this.txtName.Location = new System.Drawing.Point(118, 15);
            this.txtName.MaxLength = 100;
            this.txtName.MaxValue = ((long)(2147483647));
            this.txtName.Name = "txtName";
            this.txtName.Regex = "";
            this.txtName.RegexMsg = null;
            this.txtName.Size = new System.Drawing.Size(154, 21);
            this.txtName.TabIndex = 21;
            this.txtName.Title = "";
            this.txtName.ValueCompareTo = null;
            this.txtName.WaterText = "";
            // 
            // seGroupBox2
            // 
            this.seGroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.seGroupBox2.Controls.Add(this.radioButtonUnBind);
            this.seGroupBox2.Controls.Add(this.radioButtonBind);
            this.seGroupBox2.Controls.Add(this.ddlDataItem);
            this.seGroupBox2.Controls.Add(this.txtDataPropertyName);
            this.seGroupBox2.Controls.Add(this.lblDataItem);
            this.seGroupBox2.Controls.Add(this.lblDataPropertyName);
            this.seGroupBox2.HighLight = false;
            this.seGroupBox2.Location = new System.Drawing.Point(21, 106);
            this.seGroupBox2.Name = "seGroupBox2";
            this.seGroupBox2.Size = new System.Drawing.Size(458, 76);
            this.seGroupBox2.TabIndex = 28;
            this.seGroupBox2.TabStop = false;
            this.seGroupBox2.Text = "绑定";
            // 
            // panelDataRule
            // 
            this.panelDataRule.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDataRule.BorderColor = System.Drawing.Color.Black;
            this.panelDataRule.FillColorEnd = System.Drawing.Color.Empty;
            this.panelDataRule.FillColorStart = System.Drawing.Color.Empty;
            this.panelDataRule.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.panelDataRule.FillStyle = Sheng.SailingEase.Controls.FillStyle.Solid;
            this.panelDataRule.HighLight = false;
            this.panelDataRule.Location = new System.Drawing.Point(19, 52);
            this.panelDataRule.Name = "panelDataRule";
            this.panelDataRule.ShowBorder = false;
            this.panelDataRule.Size = new System.Drawing.Size(456, 191);
            this.panelDataRule.TabIndex = 39;
            // 
            // ddlDataRule
            // 
            this.ddlDataRule.AllowEmpty = true;
            this.ddlDataRule.DisplayMember = "Name";
            this.ddlDataRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDataRule.FormattingEnabled = true;
            this.ddlDataRule.HighLight = true;
            this.ddlDataRule.Location = new System.Drawing.Point(19, 16);
            this.ddlDataRule.Name = "ddlDataRule";
            this.ddlDataRule.Size = new System.Drawing.Size(290, 20);
            this.ddlDataRule.TabIndex = 39;
            this.ddlDataRule.Title = null;
            this.ddlDataRule.WaterText = "";
            this.ddlDataRule.SelectedIndexChanged += new System.EventHandler(this.ddlDataRule_SelectedIndexChanged);
            // 
            // seTabControl1
            // 
            this.seTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.seTabControl1.Controls.Add(this.tabPage1);
            this.seTabControl1.Controls.Add(this.tabPage2);
            this.seTabControl1.HighLight = false;
            this.seTabControl1.Location = new System.Drawing.Point(12, 12);
            this.seTabControl1.Name = "seTabControl1";
            this.seTabControl1.SelectedIndex = 0;
            this.seTabControl1.Size = new System.Drawing.Size(506, 294);
            this.seTabControl1.TabIndex = 39;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cbVisible);
            this.tabPage1.Controls.Add(this.btnClear);
            this.tabPage1.Controls.Add(this.lblName);
            this.tabPage1.Controls.Add(this.txtName);
            this.tabPage1.Controls.Add(this.seGroupBox2);
            this.tabPage1.Controls.Add(this.lblCode);
            this.tabPage1.Controls.Add(this.txtCode);
            this.tabPage1.Controls.Add(this.lblHeaderText);
            this.tabPage1.Controls.Add(this.txtText);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(498, 268);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "首要";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(286, 67);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(26, 23);
            this.btnClear.TabIndex = 38;
            this.btnClear.Text = "X";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panelDataRule);
            this.tabPage2.Controls.Add(this.ddlDataRule);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(498, 268);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "数据规则";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // FormSEPaginationDataGridViewDevColumnAdd
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(530, 347);
            this.Controls.Add(this.seTabControl1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSEPaginationDataGridViewDevColumnAdd";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "${FormSEPaginationDataGridViewDevColumnAdd}";
            this.Load += new System.EventHandler(this.FormSEPaginationDataGridViewDevColumnAdd_Load);
            this.seGroupBox2.ResumeLayout(false);
            this.seGroupBox2.PerformLayout();
            this.seTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sheng.SailingEase.Controls.SETextBox txtText;
        private System.Windows.Forms.Label lblHeaderText;
        private System.Windows.Forms.CheckBox cbVisible;
        private Sheng.SailingEase.Controls.SETextBox txtDataPropertyName;
        private System.Windows.Forms.Label lblDataPropertyName;
        private System.Windows.Forms.Label lblDataItem;
        private Sheng.SailingEase.Controls.SEComboBox ddlDataItem;
        private System.Windows.Forms.RadioButton radioButtonUnBind;
        private System.Windows.Forms.RadioButton radioButtonBind;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private Sheng.SailingEase.Controls.SETextBox txtCode;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblCode;
        private Sheng.SailingEase.Controls.SETextBox txtName;
        private Sheng.SailingEase.Controls.SEGroupBox seGroupBox2;
        private Sheng.SailingEase.Controls.SEPanel panelDataRule;
        private Sheng.SailingEase.Controls.SEComboBox ddlDataRule;
        private Sheng.SailingEase.Controls.SETabControl seTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Sheng.SailingEase.Controls.SEButton btnClear;

    }
}