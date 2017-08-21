namespace Sheng.SIMBE.IDE.UI.EventSet
{
    partial class UserControlEventSetParameter_DataListOperator_General
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
            this.txtCode = new Sheng.SIMBE.SEControl.SETextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtName = new Sheng.SIMBE.SEControl.SETextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtDataList = new Sheng.SIMBE.SEControl.SETextBox();
            this.ddlDataList = new Sheng.SIMBE.SEControl.SEComboBox();
            this.lblDataList = new System.Windows.Forms.Label();
            this.ddlOperatorType = new Sheng.SIMBE.SEControl.SEComboBox();
            this.lblOperatorType = new System.Windows.Forms.Label();
            this.ddlObjectForm = new Sheng.SIMBE.SEControl.SEComboBox();
            this.lblObjectForm = new System.Windows.Forms.Label();
            this.btnBrowse = new Sheng.SIMBE.SEControl.SEButton();
            this.SuspendLayout();
            // 
            // txtCode
            // 
            this.txtCode.AllowEmpty = false;
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
            this.lblCode.Size = new System.Drawing.Size(407, 12);
            this.lblCode.TabIndex = 26;
            this.lblCode.Text = "${UserControlEventSetParameter_DataListOperator_General_LabelCode}:";
            // 
            // txtName
            // 
            this.txtName.AllowEmpty = false;
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
            this.lblName.Size = new System.Drawing.Size(407, 12);
            this.lblName.TabIndex = 24;
            this.lblName.Text = "${UserControlEventSetParameter_DataListOperator_General_LabelName}:";
            // 
            // txtDataList
            // 
            this.txtDataList.AllowEmpty = true;
            this.txtDataList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDataList.HighLight = true;
            this.txtDataList.LimitMaxValue = false;
            this.txtDataList.Location = new System.Drawing.Point(357, 276);
            this.txtDataList.MaxLength = 100;
            this.txtDataList.MaxValue = ((long)(2147483647));
            this.txtDataList.Name = "txtDataList";
            this.txtDataList.Regex = "";
            this.txtDataList.RegexMsg = "RegexMsg_OnlyAllowMonogram";
            this.txtDataList.Size = new System.Drawing.Size(100, 21);
            this.txtDataList.TabIndex = 5;
            this.txtDataList.Title = "UserControlEventPar_DataListOperator_TextBoxDataSet";
            this.txtDataList.ValueCompareTo = null;
            this.txtDataList.Visible = false;
            this.txtDataList.WaterText = "";
            // 
            // ddlDataList
            // 
            this.ddlDataList.AllowEmpty = true;
            this.ddlDataList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlDataList.DisplayMember = "Name";
            this.ddlDataList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDataList.FormattingEnabled = true;
            this.ddlDataList.HighLight = true;
            this.ddlDataList.Location = new System.Drawing.Point(69, 111);
            this.ddlDataList.Name = "ddlDataList";
            this.ddlDataList.Size = new System.Drawing.Size(326, 20);
            this.ddlDataList.TabIndex = 4;
            this.ddlDataList.Title = "LabelDataList";
            this.ddlDataList.ValueMember = "Value";
            this.ddlDataList.WaterText = "";
            // 
            // lblDataList
            // 
            this.lblDataList.AutoSize = true;
            this.lblDataList.Location = new System.Drawing.Point(4, 114);
            this.lblDataList.Name = "lblDataList";
            this.lblDataList.Size = new System.Drawing.Size(431, 12);
            this.lblDataList.TabIndex = 39;
            this.lblDataList.Text = "${UserControlEventSetParameter_DataListOperator_General_LabelDataList}:";
            // 
            // ddlOperatorType
            // 
            this.ddlOperatorType.AllowEmpty = true;
            this.ddlOperatorType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlOperatorType.DisplayMember = "Text";
            this.ddlOperatorType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlOperatorType.FormattingEnabled = true;
            this.ddlOperatorType.HighLight = true;
            this.ddlOperatorType.Location = new System.Drawing.Point(69, 85);
            this.ddlOperatorType.Name = "ddlOperatorType";
            this.ddlOperatorType.Size = new System.Drawing.Size(326, 20);
            this.ddlOperatorType.TabIndex = 3;
            this.ddlOperatorType.Title = null;
            this.ddlOperatorType.ValueMember = "Value";
            this.ddlOperatorType.WaterText = "";
            // 
            // lblOperatorType
            // 
            this.lblOperatorType.AutoSize = true;
            this.lblOperatorType.Location = new System.Drawing.Point(3, 88);
            this.lblOperatorType.Name = "lblOperatorType";
            this.lblOperatorType.Size = new System.Drawing.Size(455, 12);
            this.lblOperatorType.TabIndex = 37;
            this.lblOperatorType.Text = "${UserControlEventSetParameter_DataListOperator_General_LabelOperatorType}:";
            // 
            // ddlObjectForm
            // 
            this.ddlObjectForm.AllowEmpty = true;
            this.ddlObjectForm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlObjectForm.DisplayMember = "Text";
            this.ddlObjectForm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlObjectForm.FormattingEnabled = true;
            this.ddlObjectForm.HighLight = true;
            this.ddlObjectForm.Location = new System.Drawing.Point(69, 59);
            this.ddlObjectForm.Name = "ddlObjectForm";
            this.ddlObjectForm.Size = new System.Drawing.Size(326, 20);
            this.ddlObjectForm.TabIndex = 2;
            this.ddlObjectForm.Title = null;
            this.ddlObjectForm.ValueMember = "Value";
            this.ddlObjectForm.WaterText = "";
            this.ddlObjectForm.SelectedIndexChanged += new System.EventHandler(this.ddlObjectForm_SelectedIndexChanged);
            // 
            // lblObjectForm
            // 
            this.lblObjectForm.AutoSize = true;
            this.lblObjectForm.Location = new System.Drawing.Point(3, 62);
            this.lblObjectForm.Name = "lblObjectForm";
            this.lblObjectForm.Size = new System.Drawing.Size(443, 12);
            this.lblObjectForm.TabIndex = 35;
            this.lblObjectForm.Text = "${UserControlEventSetParameter_DataListOperator_General_LabelObjectForm}:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(320, 137);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 58;
            this.btnBrowse.Text = "浏览";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Visible = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // UserControlEventSetParameter_DataListOperator_General
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtDataList);
            this.Controls.Add(this.ddlDataList);
            this.Controls.Add(this.ddlObjectForm);
            this.Controls.Add(this.ddlOperatorType);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblDataList);
            this.Controls.Add(this.lblObjectForm);
            this.Controls.Add(this.lblOperatorType);
            this.Name = "UserControlEventSetParameter_DataListOperator_General";
            this.Controls.SetChildIndex(this.lblOperatorType, 0);
            this.Controls.SetChildIndex(this.lblObjectForm, 0);
            this.Controls.SetChildIndex(this.lblDataList, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.lblCode, 0);
            this.Controls.SetChildIndex(this.txtCode, 0);
            this.Controls.SetChildIndex(this.ddlOperatorType, 0);
            this.Controls.SetChildIndex(this.ddlObjectForm, 0);
            this.Controls.SetChildIndex(this.ddlDataList, 0);
            this.Controls.SetChildIndex(this.txtDataList, 0);
            this.Controls.SetChildIndex(this.btnBrowse, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sheng.SIMBE.SEControl.SETextBox txtCode;
        private System.Windows.Forms.Label lblCode;
        private Sheng.SIMBE.SEControl.SETextBox txtName;
        private System.Windows.Forms.Label lblName;
        private Sheng.SIMBE.SEControl.SETextBox txtDataList;
        private Sheng.SIMBE.SEControl.SEComboBox ddlDataList;
        private System.Windows.Forms.Label lblDataList;
        private Sheng.SIMBE.SEControl.SEComboBox ddlOperatorType;
        private System.Windows.Forms.Label lblOperatorType;
        private Sheng.SIMBE.SEControl.SEComboBox ddlObjectForm;
        private System.Windows.Forms.Label lblObjectForm;
        private Sheng.SIMBE.SEControl.SEButton btnBrowse;
    }
}
