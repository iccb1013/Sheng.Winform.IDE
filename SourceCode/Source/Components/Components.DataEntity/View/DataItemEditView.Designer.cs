namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    partial class DataItemEditView
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            Sheng.SailingEase.Controls.SEComboSelectorTheme seComboSelectorTheme1 = new Sheng.SailingEase.Controls.SEComboSelectorTheme();
            this.tabControl1 = new Sheng.SailingEase.Controls.SETabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.availableDataItems = new Sheng.SailingEase.Controls.SEComboSelector2();
            this.txtLength = new Sheng.SailingEase.Controls.SEComboBox();
            this.txtDecimalDigits = new Sheng.SailingEase.Controls.SENumericUpDown();
            this.cbExclusive = new System.Windows.Forms.CheckBox();
            this.cbAllowEmpty = new System.Windows.Forms.CheckBox();
            this.txtDefaultValue = new Sheng.SailingEase.Controls.SETextBox();
            this.lblDefaultValue = new System.Windows.Forms.Label();
            this.lblDecimalDigits = new System.Windows.Forms.Label();
            this.lblLength = new System.Windows.Forms.Label();
            this.lblDataType = new System.Windows.Forms.Label();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtCode = new Sheng.SailingEase.Controls.SETextBox();
            this.txtName = new Sheng.SailingEase.Controls.SETextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tabPageRemark = new System.Windows.Forms.TabPage();
            this.txtRemark = new Sheng.SailingEase.Controls.SETextBox();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.tabControl1.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDecimalDigits)).BeginInit();
            this.tabPageRemark.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageGeneral);
            this.tabControl1.Controls.Add(this.tabPageRemark);
            this.tabControl1.CustomValidate = null;
            this.tabControl1.HighLight = false;
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(425, 287);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Title = null;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.availableDataItems);
            this.tabPageGeneral.Controls.Add(this.txtLength);
            this.tabPageGeneral.Controls.Add(this.txtDecimalDigits);
            this.tabPageGeneral.Controls.Add(this.cbExclusive);
            this.tabPageGeneral.Controls.Add(this.cbAllowEmpty);
            this.tabPageGeneral.Controls.Add(this.txtDefaultValue);
            this.tabPageGeneral.Controls.Add(this.lblDefaultValue);
            this.tabPageGeneral.Controls.Add(this.lblDecimalDigits);
            this.tabPageGeneral.Controls.Add(this.lblLength);
            this.tabPageGeneral.Controls.Add(this.lblDataType);
            this.tabPageGeneral.Controls.Add(this.lblCode);
            this.tabPageGeneral.Controls.Add(this.txtCode);
            this.tabPageGeneral.Controls.Add(this.txtName);
            this.tabPageGeneral.Controls.Add(this.lblName);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(417, 261);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "TabPageGeneral";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // availableDataItems
            // 
            this.availableDataItems.AllowEmpty = false;
            this.availableDataItems.BackColor = System.Drawing.Color.White;
            this.availableDataItems.CustomValidate = null;
            this.availableDataItems.DescriptionMember = null;
            this.availableDataItems.DisplayMember = null;
            this.availableDataItems.HighLight = true;
            this.availableDataItems.Location = new System.Drawing.Point(87, 84);
            this.availableDataItems.MaxItem = 8;
            this.availableDataItems.Name = "availableDataItems";
            this.availableDataItems.Padding = new System.Windows.Forms.Padding(5);
            this.availableDataItems.ShowDescription = false;
            this.availableDataItems.Size = new System.Drawing.Size(308, 26);
            this.availableDataItems.TabIndex = 17;
            this.availableDataItems.Text = "seComboSelector21";
            seComboSelectorTheme1.ArrowColorEnd = System.Drawing.Color.LightGray;
            seComboSelectorTheme1.ArrowColorStart = System.Drawing.Color.Gray;
            seComboSelectorTheme1.BackColor = System.Drawing.Color.Gray;
            seComboSelectorTheme1.BackgroundColor = System.Drawing.Color.White;
            seComboSelectorTheme1.BorderColor = System.Drawing.Color.LightGray;
            seComboSelectorTheme1.DescriptionTextColor = System.Drawing.SystemColors.GrayText;
            seComboSelectorTheme1.HoveredBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(228)))), ((int)(((byte)(134)))));
            seComboSelectorTheme1.HoveredBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(202)))), ((int)(((byte)(88)))));
            seComboSelectorTheme1.HoveredDescriptionColor = System.Drawing.SystemColors.GrayText;
            seComboSelectorTheme1.HoveredTextColor = System.Drawing.SystemColors.WindowText;
            seComboSelectorTheme1.SelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
            seComboSelectorTheme1.SelectedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(138)))), ((int)(((byte)(48)))));
            seComboSelectorTheme1.SelectedDescriptionTextColor = System.Drawing.SystemColors.GrayText;
            seComboSelectorTheme1.SelectedTextColor = System.Drawing.SystemColors.WindowText;
            seComboSelectorTheme1.TextColor = System.Drawing.SystemColors.WindowText;
            this.availableDataItems.Theme = seComboSelectorTheme1;
            this.availableDataItems.Title = null;
            this.availableDataItems.SelectedValueChanged += new System.EventHandler<Sheng.SailingEase.Controls.SEComboSelector2.OnSelectedValueChangedEventArgs>(this.availableDataItems_SelectedValueChanged);
            // 
            // txtLength
            // 
            this.txtLength.AllowEmpty = false;
            this.txtLength.CustomValidate = null;
            this.txtLength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.txtLength.FormattingEnabled = true;
            this.txtLength.HighLight = true;
            this.txtLength.Items.AddRange(new object[] {
            "MAX"});
            this.txtLength.Location = new System.Drawing.Point(87, 117);
            this.txtLength.MaxLength = 4;
            this.txtLength.Name = "txtLength";
            this.txtLength.Size = new System.Drawing.Size(100, 20);
            this.txtLength.TabIndex = 16;
            this.txtLength.Title = null;
            this.txtLength.WaterText = "";
            // 
            // txtDecimalDigits
            // 
            this.txtDecimalDigits.Location = new System.Drawing.Point(285, 116);
            this.txtDecimalDigits.Name = "txtDecimalDigits";
            this.txtDecimalDigits.Size = new System.Drawing.Size(110, 21);
            this.txtDecimalDigits.TabIndex = 5;
            // 
            // cbExclusive
            // 
            this.cbExclusive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbExclusive.AutoSize = true;
            this.cbExclusive.Location = new System.Drawing.Point(109, 224);
            this.cbExclusive.Name = "cbExclusive";
            this.cbExclusive.Size = new System.Drawing.Size(246, 16);
            this.cbExclusive.TabIndex = 8;
            this.cbExclusive.Text = "${DataItemEditView_CheckBoxExclusive}";
            this.cbExclusive.UseVisualStyleBackColor = true;
            this.cbExclusive.Visible = false;
            // 
            // cbAllowEmpty
            // 
            this.cbAllowEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbAllowEmpty.AutoSize = true;
            this.cbAllowEmpty.Checked = true;
            this.cbAllowEmpty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAllowEmpty.Location = new System.Drawing.Point(22, 224);
            this.cbAllowEmpty.Name = "cbAllowEmpty";
            this.cbAllowEmpty.Size = new System.Drawing.Size(252, 16);
            this.cbAllowEmpty.TabIndex = 7;
            this.cbAllowEmpty.Text = "${DataItemEditView_CheckBoxAllowEmpty}";
            this.cbAllowEmpty.UseVisualStyleBackColor = true;
            // 
            // txtDefaultValue
            // 
            this.txtDefaultValue.AllowEmpty = true;
            this.txtDefaultValue.CustomValidate = null;
            this.helpProvider1.SetHelpString(this.txtDefaultValue, "默认值可以是计算结果为常量的任何值。字符和日期常量要放在单引号 (\') 内；货币、整数和浮点常量不需要引号。二进制数据必须以 0x 开头，货币数据必须以美元符号 " +
                    "($) 开头。默认值必须与列数据类型兼容。");
            this.txtDefaultValue.HighLight = true;
            this.txtDefaultValue.LimitMaxValue = false;
            this.txtDefaultValue.Location = new System.Drawing.Point(87, 143);
            this.txtDefaultValue.MaxLength = 255;
            this.txtDefaultValue.MaxValue = ((long)(2147483647));
            this.txtDefaultValue.Name = "txtDefaultValue";
            this.txtDefaultValue.Regex = "";
            this.txtDefaultValue.RegexMsg = null;
            this.helpProvider1.SetShowHelp(this.txtDefaultValue, true);
            this.txtDefaultValue.Size = new System.Drawing.Size(100, 21);
            this.txtDefaultValue.TabIndex = 6;
            this.txtDefaultValue.Title = null;
            this.txtDefaultValue.ValueCompareTo = null;
            this.txtDefaultValue.WaterText = "";
            // 
            // lblDefaultValue
            // 
            this.lblDefaultValue.AutoSize = true;
            this.lblDefaultValue.Location = new System.Drawing.Point(20, 146);
            this.lblDefaultValue.Name = "lblDefaultValue";
            this.lblDefaultValue.Size = new System.Drawing.Size(233, 12);
            this.lblDefaultValue.TabIndex = 14;
            this.lblDefaultValue.Text = "${DataItemEditView_LabelDefaultValue}:";
            // 
            // lblDecimalDigits
            // 
            this.lblDecimalDigits.AutoSize = true;
            this.lblDecimalDigits.Location = new System.Drawing.Point(217, 120);
            this.lblDecimalDigits.Name = "lblDecimalDigits";
            this.lblDecimalDigits.Size = new System.Drawing.Size(239, 12);
            this.lblDecimalDigits.TabIndex = 12;
            this.lblDecimalDigits.Text = "${DataItemEditView_LabelDecimalDigits}:";
            // 
            // lblLength
            // 
            this.lblLength.AutoSize = true;
            this.lblLength.Location = new System.Drawing.Point(20, 120);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(197, 12);
            this.lblLength.TabIndex = 10;
            this.lblLength.Text = "${DataItemEditView_LabelLength}:";
            // 
            // lblDataType
            // 
            this.lblDataType.AutoSize = true;
            this.lblDataType.Location = new System.Drawing.Point(20, 93);
            this.lblDataType.Name = "lblDataType";
            this.lblDataType.Size = new System.Drawing.Size(209, 12);
            this.lblDataType.TabIndex = 9;
            this.lblDataType.Text = "${DataItemEditView_LabelDataType}:";
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(217, 24);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(185, 12);
            this.lblCode.TabIndex = 7;
            this.lblCode.Text = "${DataItemEditView_LabelCode}:";
            // 
            // txtCode
            // 
            this.txtCode.AllowEmpty = false;
            this.txtCode.CustomValidate = null;
            this.txtCode.HighLight = true;
            this.txtCode.LimitMaxValue = false;
            this.txtCode.Location = new System.Drawing.Point(285, 21);
            this.txtCode.MaxLength = 100;
            this.txtCode.MaxValue = ((long)(2147483647));
            this.txtCode.Name = "txtCode";
            this.txtCode.Regex = "";
            this.txtCode.RegexMsg = "RegexMsg_OnlyAllowMonogram";
            this.txtCode.Size = new System.Drawing.Size(110, 21);
            this.txtCode.TabIndex = 2;
            this.txtCode.Title = "LabelCode";
            this.txtCode.ValueCompareTo = null;
            this.txtCode.WaterText = "";
            // 
            // txtName
            // 
            this.txtName.AllowEmpty = false;
            this.txtName.CustomValidate = null;
            this.txtName.HighLight = true;
            this.txtName.LimitMaxValue = false;
            this.txtName.Location = new System.Drawing.Point(87, 21);
            this.txtName.MaxLength = 100;
            this.txtName.MaxValue = ((long)(2147483647));
            this.txtName.Name = "txtName";
            this.txtName.Regex = "";
            this.txtName.RegexMsg = null;
            this.txtName.Size = new System.Drawing.Size(98, 21);
            this.txtName.TabIndex = 1;
            this.txtName.Title = "LabelName";
            this.txtName.ValueCompareTo = null;
            this.txtName.WaterText = "";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(20, 24);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(185, 12);
            this.lblName.TabIndex = 4;
            this.lblName.Text = "${DataItemEditView_LabelName}:";
            // 
            // tabPageRemark
            // 
            this.tabPageRemark.Controls.Add(this.txtRemark);
            this.tabPageRemark.Location = new System.Drawing.Point(4, 22);
            this.tabPageRemark.Name = "tabPageRemark";
            this.tabPageRemark.Padding = new System.Windows.Forms.Padding(5);
            this.tabPageRemark.Size = new System.Drawing.Size(417, 261);
            this.tabPageRemark.TabIndex = 1;
            this.tabPageRemark.Text = "TabPageRemark";
            this.tabPageRemark.UseVisualStyleBackColor = true;
            // 
            // txtRemark
            // 
            this.txtRemark.AcceptsReturn = true;
            this.txtRemark.AllowEmpty = true;
            this.txtRemark.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRemark.CustomValidate = null;
            this.txtRemark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemark.HighLight = true;
            this.txtRemark.LimitMaxValue = false;
            this.txtRemark.Location = new System.Drawing.Point(5, 5);
            this.txtRemark.MaxLength = 1000;
            this.txtRemark.MaxValue = ((long)(2147483647));
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Regex = "";
            this.txtRemark.RegexMsg = null;
            this.txtRemark.Size = new System.Drawing.Size(407, 251);
            this.txtRemark.TabIndex = 1;
            this.txtRemark.Title = null;
            this.txtRemark.ValueCompareTo = null;
            this.txtRemark.WaterText = "";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(362, 305);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "${DataItemEditView_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(281, 305);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "${DataItemEditView_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // DataItemEditView
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(449, 337);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataItemEditView";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${FormDataItemAdd}";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DataItemEditView_FormClosed);
            this.Load += new System.EventHandler(this.DataItemEditView_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDecimalDigits)).EndInit();
            this.tabPageRemark.ResumeLayout(false);
            this.tabPageRemark.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Sheng.SailingEase.Controls.SETabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageRemark;
        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private System.Windows.Forms.Label lblCode;
        private Sheng.SailingEase.Controls.SETextBox txtCode;
        private Sheng.SailingEase.Controls.SETextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDataType;
        private System.Windows.Forms.Label lblLength;
        private System.Windows.Forms.Label lblDecimalDigits;
        private Sheng.SailingEase.Controls.SETextBox txtDefaultValue;
        private System.Windows.Forms.Label lblDefaultValue;
        private Sheng.SailingEase.Controls.SETextBox txtRemark;
        private System.Windows.Forms.CheckBox cbAllowEmpty;
        private System.Windows.Forms.CheckBox cbExclusive;
        private Sheng.SailingEase.Controls.SENumericUpDown txtDecimalDigits;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private Sheng.SailingEase.Controls.SEComboBox txtLength;
        private Controls.SEComboSelector2 availableDataItems;
    }
}