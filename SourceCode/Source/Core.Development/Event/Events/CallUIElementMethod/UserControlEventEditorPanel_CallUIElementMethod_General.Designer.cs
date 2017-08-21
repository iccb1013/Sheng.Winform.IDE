namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlEventEditorPanel_CallUIElementMethod_General
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
            Sheng.SailingEase.Controls.SEComboSelectorTheme seComboSelectorTheme1 = new Sheng.SailingEase.Controls.SEComboSelectorTheme();
            this.txtCode = new Sheng.SailingEase.Controls.SETextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtName = new Sheng.SailingEase.Controls.SETextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblFormElement = new System.Windows.Forms.Label();
            this.lblMethod = new System.Windows.Forms.Label();
            this.ddlFormElement = new Sheng.SailingEase.Core.Development.FormElementComboBox();
            this.ddlObjectForm = new Sheng.SailingEase.Controls.SEComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowse = new Sheng.SailingEase.Controls.SEButton();
            this.txtFormElement = new Sheng.SailingEase.Controls.SETextBox();
            this.availabilityEvent = new Sheng.SailingEase.Controls.SEComboSelector2();
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
            this.lblCode.Size = new System.Drawing.Size(401, 12);
            this.lblCode.TabIndex = 26;
            this.lblCode.Text = "${UserControlEventEditorPanel_CallEntityMethod_General_LabelCode}:";
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
            this.lblName.Size = new System.Drawing.Size(401, 12);
            this.lblName.TabIndex = 24;
            this.lblName.Text = "${UserControlEventEditorPanel_CallEntityMethod_General_LabelName}:";
            // 
            // lblFormElement
            // 
            this.lblFormElement.AutoSize = true;
            this.lblFormElement.Location = new System.Drawing.Point(4, 62);
            this.lblFormElement.Name = "lblFormElement";
            this.lblFormElement.Size = new System.Drawing.Size(443, 12);
            this.lblFormElement.TabIndex = 32;
            this.lblFormElement.Text = "${UserControlEventEditorPanel_CallEntityMethod_General_LabelFormElement}:";
            // 
            // lblMethod
            // 
            this.lblMethod.AutoSize = true;
            this.lblMethod.Location = new System.Drawing.Point(4, 92);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(413, 12);
            this.lblMethod.TabIndex = 34;
            this.lblMethod.Text = "${UserControlEventEditorPanel_CallEntityMethod_General_LabelMethod}:";
            // 
            // ddlFormElement
            // 
            this.ddlFormElement.AllowDataSource = Sheng.SailingEase.Core.EnumEventDataSource.FormElement;
            this.ddlFormElement.AllowEmpty = true;
            this.ddlFormElement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlFormElement.CustomValidate = null;
            this.ddlFormElement.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ddlFormElement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlFormElement.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ddlFormElement.FormattingEnabled = true;
            this.ddlFormElement.FormEntity = null;
            this.ddlFormElement.HighLight = true;
            this.ddlFormElement.Location = new System.Drawing.Point(70, 59);
            this.ddlFormElement.MaxDropDownItems = 16;
            this.ddlFormElement.Name = "ddlFormElement";
            this.ddlFormElement.SelectedFormElementId = "";
            this.ddlFormElement.Size = new System.Drawing.Size(326, 22);
            this.ddlFormElement.TabIndex = 35;
            this.ddlFormElement.Title = "";
            this.ddlFormElement.WaterText = "";
            this.ddlFormElement.SelectedIndexChanged += new System.EventHandler(this.ddlFormElement_SelectedIndexChanged);
            // 
            // ddlObjectForm
            // 
            this.ddlObjectForm.AllowEmpty = true;
            this.ddlObjectForm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlObjectForm.CustomValidate = null;
            this.ddlObjectForm.DisplayMember = "Text";
            this.ddlObjectForm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlObjectForm.Enabled = false;
            this.ddlObjectForm.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ddlObjectForm.FormattingEnabled = true;
            this.ddlObjectForm.HighLight = true;
            this.ddlObjectForm.Location = new System.Drawing.Point(69, 238);
            this.ddlObjectForm.Name = "ddlObjectForm";
            this.ddlObjectForm.Size = new System.Drawing.Size(246, 20);
            this.ddlObjectForm.TabIndex = 36;
            this.ddlObjectForm.Title = null;
            this.ddlObjectForm.ValueMember = "Value";
            this.ddlObjectForm.Visible = false;
            this.ddlObjectForm.WaterText = "";
            this.ddlObjectForm.SelectedIndexChanged += new System.EventHandler(this.ddlObjectForm_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 241);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(437, 12);
            this.label1.TabIndex = 37;
            this.label1.Text = "${UserControlEventEditorPanel_CallEntityMethod_General_LabelTargetForm}:";
            this.label1.Visible = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Enabled = false;
            this.btnBrowse.Location = new System.Drawing.Point(321, 236);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 58;
            this.btnBrowse.Text = "${UserControlEventEditorPanel_CallEntityMethod_General_ButtonBrowse}";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Visible = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFormElement
            // 
            this.txtFormElement.AllowEmpty = true;
            this.txtFormElement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFormElement.CustomValidate = null;
            this.txtFormElement.HighLight = true;
            this.txtFormElement.LimitMaxValue = false;
            this.txtFormElement.Location = new System.Drawing.Point(5, 276);
            this.txtFormElement.MaxLength = 100;
            this.txtFormElement.MaxValue = ((long)(2147483647));
            this.txtFormElement.Name = "txtFormElement";
            this.txtFormElement.ReadOnly = true;
            this.txtFormElement.Regex = "";
            this.txtFormElement.RegexMsg = "只允许字母组合";
            this.txtFormElement.Size = new System.Drawing.Size(100, 21);
            this.txtFormElement.TabIndex = 59;
            this.txtFormElement.Title = "对象";
            this.txtFormElement.ValueCompareTo = null;
            this.txtFormElement.Visible = false;
            this.txtFormElement.WaterText = "";
            // 
            // availabilityEvent
            // 
            this.availabilityEvent.AllowEmpty = true;
            this.availabilityEvent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.availabilityEvent.BackColor = System.Drawing.Color.White;
            this.availabilityEvent.CustomValidate = null;
            this.availabilityEvent.DescriptionMember = "Description";
            this.availabilityEvent.DisplayMember = "Name";
            this.availabilityEvent.HighLight = true;
            this.availabilityEvent.LayoutMode = Sheng.SailingEase.Controls.ListViewLayoutMode.Descriptive;
            this.availabilityEvent.Location = new System.Drawing.Point(69, 87);
            this.availabilityEvent.MaxItem = 5;
            this.availabilityEvent.Name = "availabilityEvent";
            this.availabilityEvent.Padding = new System.Windows.Forms.Padding(5);
            this.availabilityEvent.ShowDescription = false;
            this.availabilityEvent.Size = new System.Drawing.Size(327, 26);
            this.availabilityEvent.TabIndex = 61;
            this.availabilityEvent.Text = "seComboSelector21";
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
            this.availabilityEvent.Theme = seComboSelectorTheme1;
            this.availabilityEvent.Title = null;
            // 
            // UserControlEventEditorPanel_CallUIElementMethod_General
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CausesValidation = false;
            this.Controls.Add(this.availabilityEvent);
            this.Controls.Add(this.txtFormElement);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.ddlObjectForm);
            this.Controls.Add(this.ddlFormElement);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblMethod);
            this.Controls.Add(this.lblFormElement);
            this.Controls.Add(this.label1);
            this.Name = "UserControlEventEditorPanel_CallUIElementMethod_General";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.lblFormElement, 0);
            this.Controls.SetChildIndex(this.lblMethod, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.lblCode, 0);
            this.Controls.SetChildIndex(this.txtCode, 0);
            this.Controls.SetChildIndex(this.ddlFormElement, 0);
            this.Controls.SetChildIndex(this.ddlObjectForm, 0);
            this.Controls.SetChildIndex(this.btnBrowse, 0);
            this.Controls.SetChildIndex(this.txtFormElement, 0);
            this.Controls.SetChildIndex(this.availabilityEvent, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sheng.SailingEase.Controls.SETextBox txtCode;
        private System.Windows.Forms.Label lblCode;
        private Sheng.SailingEase.Controls.SETextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblFormElement;
        private System.Windows.Forms.Label lblMethod;
        private FormElementComboBox ddlFormElement;
        private Sheng.SailingEase.Controls.SEComboBox ddlObjectForm;
        private System.Windows.Forms.Label label1;
        private Sheng.SailingEase.Controls.SEButton btnBrowse;
        private Sheng.SailingEase.Controls.SETextBox txtFormElement;
        private Controls.SEComboSelector2 availabilityEvent;
    }
}
