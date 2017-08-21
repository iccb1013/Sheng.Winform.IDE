namespace Sheng.SailingEase.Core.Development
{
    partial class FormEventDataListDataSet
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
            this.txtDataColumn = new Sheng.SailingEase.Controls.SETextBox();
            this.ddlDataColumn = new Sheng.SailingEase.Controls.SEComboBox();
            this.ddlDataSourceType = new Sheng.SailingEase.Controls.SEComboBox();
            this.lblDataSource = new System.Windows.Forms.Label();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.lblDataColumn = new System.Windows.Forms.Label();
            this.ddlFormElement = new FormElementComboBox();
            this.btnBrowse = new Sheng.SailingEase.Controls.SEButton();
            this.SuspendLayout();
            // 
            // txtDataColumn
            // 
            this.txtDataColumn.AllowEmpty = true;
            this.txtDataColumn.HighLight = true;
            this.txtDataColumn.LimitMaxValue = false;
            this.txtDataColumn.Location = new System.Drawing.Point(7, 158);
            this.txtDataColumn.MaxLength = 100;
            this.txtDataColumn.MaxValue = ((long)(2147483647));
            this.txtDataColumn.Name = "txtDataColumn";
            this.txtDataColumn.Regex = "";
            this.txtDataColumn.RegexMsg = "";
            this.txtDataColumn.Size = new System.Drawing.Size(100, 21);
            this.txtDataColumn.TabIndex = 55;
            this.txtDataColumn.Title = "数据列代码";
            this.txtDataColumn.ValueCompareTo = null;
            this.txtDataColumn.Visible = false;
            this.txtDataColumn.WaterText = "";
            // 
            // ddlDataColumn
            // 
            this.ddlDataColumn.AllowEmpty = true;
            this.ddlDataColumn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlDataColumn.DisplayMember = "Text";
            this.ddlDataColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDataColumn.FormattingEnabled = true;
            this.ddlDataColumn.HighLight = true;
            this.ddlDataColumn.Location = new System.Drawing.Point(101, 26);
            this.ddlDataColumn.Name = "ddlDataColumn";
            this.ddlDataColumn.Size = new System.Drawing.Size(385, 20);
            this.ddlDataColumn.TabIndex = 54;
            this.ddlDataColumn.Title = "LabelDataColumn";
            this.ddlDataColumn.ValueMember = "Value";
            this.ddlDataColumn.WaterText = "";
            // 
            // ddlDataSourceType
            // 
            this.ddlDataSourceType.AllowEmpty = false;
            this.ddlDataSourceType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlDataSourceType.DisplayMember = "Text";
            this.ddlDataSourceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDataSourceType.FormattingEnabled = true;
            this.ddlDataSourceType.HighLight = true;
            this.ddlDataSourceType.Location = new System.Drawing.Point(101, 53);
            this.ddlDataSourceType.Name = "ddlDataSourceType";
            this.ddlDataSourceType.Size = new System.Drawing.Size(385, 20);
            this.ddlDataSourceType.TabIndex = 52;
            this.ddlDataSourceType.Title = "LabelDataSource";
            this.ddlDataSourceType.ValueMember = "Value";
            this.ddlDataSourceType.WaterText = "";
            this.ddlDataSourceType.SelectedIndexChanged += new System.EventHandler(this.ddlDataSourceType_SelectedIndexChanged);
            // 
            // lblDataSource
            // 
            this.lblDataSource.AutoSize = true;
            this.lblDataSource.Location = new System.Drawing.Point(12, 56);
            this.lblDataSource.Name = "lblDataSource";
            this.lblDataSource.Size = new System.Drawing.Size(269, 12);
            this.lblDataSource.TabIndex = 51;
            this.lblDataSource.Text = "${FormEventDataListDataSet_LabelDataSource}:";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(418, 147);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 49;
            this.btnOK.Text = "${FormEventDataListDataSet_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(499, 147);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 50;
            this.btnCancel.Text = "${FormEventDataListDataSet_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblDataColumn
            // 
            this.lblDataColumn.AutoSize = true;
            this.lblDataColumn.Location = new System.Drawing.Point(12, 29);
            this.lblDataColumn.Name = "lblDataColumn";
            this.lblDataColumn.Size = new System.Drawing.Size(269, 12);
            this.lblDataColumn.TabIndex = 48;
            this.lblDataColumn.Text = "${FormEventDataListDataSet_LabelDataColumn}:";
            // 
            // ddlFormElement
            // 
            this.ddlFormElement.AllowDataSource = EnumEventDataSource.FormElement;
            this.ddlFormElement.AllowEmpty = false;
            this.ddlFormElement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlFormElement.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ddlFormElement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlFormElement.FormattingEnabled = true;
            this.ddlFormElement.FormEntity = null;
            this.ddlFormElement.HighLight = true;
            this.ddlFormElement.Location = new System.Drawing.Point(101, 79);
            this.ddlFormElement.MaxDropDownItems = 16;
            this.ddlFormElement.Name = "ddlFormElement";
            this.ddlFormElement.SelectedFormElementId = "";
            this.ddlFormElement.Size = new System.Drawing.Size(385, 22);
            this.ddlFormElement.TabIndex = 56;
            this.ddlFormElement.Title = "LabelDataSource";
            this.ddlFormElement.WaterText = "";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(492, 24);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 57;
            this.btnBrowse.Text = "${FormEventDataListDataSet_ButtonBrowse}";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Visible = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // FormEventDataListDataSet
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(586, 182);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.ddlFormElement);
            this.Controls.Add(this.txtDataColumn);
            this.Controls.Add(this.ddlDataColumn);
            this.Controls.Add(this.ddlDataSourceType);
            this.Controls.Add(this.lblDataSource);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblDataColumn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEventDataListDataSet";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${FormEventDataListDataSet}";
            this.Load += new System.EventHandler(this.FormEventDataListDataSet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sheng.SailingEase.Controls.SETextBox txtDataColumn;
        private Sheng.SailingEase.Controls.SEComboBox ddlDataColumn;
        private Sheng.SailingEase.Controls.SEComboBox ddlDataSourceType;
        private System.Windows.Forms.Label lblDataSource;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private System.Windows.Forms.Label lblDataColumn;
        private FormElementComboBox ddlFormElement;
        private Sheng.SailingEase.Controls.SEButton btnBrowse;
    }
}