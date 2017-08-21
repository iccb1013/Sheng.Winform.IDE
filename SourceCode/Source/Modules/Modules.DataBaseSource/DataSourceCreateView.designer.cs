namespace Sheng.SailingEase.Modules.DataBaseSourceModule
{
    partial class DataSourceCreateView
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
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.lblDataSourceType = new System.Windows.Forms.Label();
            this.txtDataSourceType = new Sheng.SailingEase.Controls.SETextBox();
            this.lblDataSourceName = new System.Windows.Forms.Label();
            this.ddlDataSource = new Sheng.SailingEase.Controls.SEComboBox();
            this.btnRefreshDataSource = new Sheng.SailingEase.Controls.SEButton();
            this.groupBoxLoginOption = new Sheng.SailingEase.Controls.SEGroupBox();
            this.txtPassword = new Sheng.SailingEase.Controls.SETextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUserId = new Sheng.SailingEase.Controls.SETextBox();
            this.lblUserId = new System.Windows.Forms.Label();
            this.radioButtonNoIntegratedSecurity = new System.Windows.Forms.RadioButton();
            this.radioButtonIntegratedSecurity = new System.Windows.Forms.RadioButton();
            this.lblNotice = new System.Windows.Forms.Label();
            this.groupBoxLoginOption.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(282, 339);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "${DataSourceCreateView_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(201, 339);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "${DataSourceCreateView_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblDataSourceType
            // 
            this.lblDataSourceType.AutoSize = true;
            this.lblDataSourceType.Location = new System.Drawing.Point(21, 52);
            this.lblDataSourceType.Name = "lblDataSourceType";
            this.lblDataSourceType.Size = new System.Drawing.Size(269, 12);
            this.lblDataSourceType.TabIndex = 6;
            this.lblDataSourceType.Text = "${DataSourceCreateView_LabelDataSourceType}:";
            // 
            // txtDataSourceType
            // 
            this.txtDataSourceType.AllowEmpty = true;
            this.txtDataSourceType.Enabled = false;
            this.txtDataSourceType.HighLight = true;
            this.txtDataSourceType.Location = new System.Drawing.Point(23, 67);
            this.txtDataSourceType.Name = "txtDataSourceType";
            this.txtDataSourceType.Regex = "";
            this.txtDataSourceType.RegexMsg = null;
            this.txtDataSourceType.Size = new System.Drawing.Size(244, 21);
            this.txtDataSourceType.TabIndex = 7;
            this.txtDataSourceType.Text = "SqlClient";
            this.txtDataSourceType.Title = null;
            this.txtDataSourceType.ValueCompareTo = null;
            // 
            // lblDataSourceName
            // 
            this.lblDataSourceName.AutoSize = true;
            this.lblDataSourceName.Location = new System.Drawing.Point(21, 103);
            this.lblDataSourceName.Name = "lblDataSourceName";
            this.lblDataSourceName.Size = new System.Drawing.Size(269, 12);
            this.lblDataSourceName.TabIndex = 8;
            this.lblDataSourceName.Text = "${DataSourceCreateView_LabelDataSourceName}:";
            // 
            // ddlDataSource
            // 
            this.ddlDataSource.AllowEmpty = false;
            this.ddlDataSource.DisplayMember = "ServerName";
            this.ddlDataSource.FormattingEnabled = true;
            this.ddlDataSource.HighLight = true;
            this.ddlDataSource.Location = new System.Drawing.Point(23, 118);
            this.ddlDataSource.Name = "ddlDataSource";
            this.ddlDataSource.Size = new System.Drawing.Size(244, 20);
            this.ddlDataSource.TabIndex = 9;
            this.ddlDataSource.Title = null;
            this.ddlDataSource.ValueMember = "ServerName";
            // 
            // btnRefreshDataSource
            // 
            this.btnRefreshDataSource.Location = new System.Drawing.Point(273, 116);
            this.btnRefreshDataSource.Name = "btnRefreshDataSource";
            this.btnRefreshDataSource.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshDataSource.TabIndex = 10;
            this.btnRefreshDataSource.Text = "${DataSourceCreateView_ButtonRefreshDataSource}";
            this.btnRefreshDataSource.UseVisualStyleBackColor = true;
            this.btnRefreshDataSource.Click += new System.EventHandler(this.btnRefreshDataSource_Click);
            // 
            // groupBoxLoginOption
            // 
            this.groupBoxLoginOption.Controls.Add(this.txtPassword);
            this.groupBoxLoginOption.Controls.Add(this.lblPassword);
            this.groupBoxLoginOption.Controls.Add(this.txtUserId);
            this.groupBoxLoginOption.Controls.Add(this.lblUserId);
            this.groupBoxLoginOption.Controls.Add(this.radioButtonNoIntegratedSecurity);
            this.groupBoxLoginOption.Controls.Add(this.radioButtonIntegratedSecurity);
            this.groupBoxLoginOption.HighLight = false;
            this.groupBoxLoginOption.Location = new System.Drawing.Point(23, 156);
            this.groupBoxLoginOption.Name = "groupBoxLoginOption";
            this.groupBoxLoginOption.Size = new System.Drawing.Size(325, 147);
            this.groupBoxLoginOption.TabIndex = 11;
            this.groupBoxLoginOption.TabStop = false;
            this.groupBoxLoginOption.Text = "${DataSourceCreateView_GroupBoxLoginOption}";
            // 
            // txtPassword
            // 
            this.txtPassword.AllowEmpty = true;
            this.txtPassword.Enabled = false;
            this.txtPassword.HighLight = true;
            this.txtPassword.Location = new System.Drawing.Point(81, 100);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Regex = "";
            this.txtPassword.RegexMsg = null;
            this.txtPassword.Size = new System.Drawing.Size(238, 21);
            this.txtPassword.TabIndex = 14;
            this.txtPassword.Title = "密码";
            this.txtPassword.ValueCompareTo = null;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(18, 103);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(233, 12);
            this.lblPassword.TabIndex = 13;
            this.lblPassword.Text = "${DataSourceCreateView_LabelPassword}:";
            // 
            // txtUserId
            // 
            this.txtUserId.AllowEmpty = true;
            this.txtUserId.Enabled = false;
            this.txtUserId.HighLight = true;
            this.txtUserId.Location = new System.Drawing.Point(81, 73);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Regex = "";
            this.txtUserId.RegexMsg = null;
            this.txtUserId.Size = new System.Drawing.Size(238, 21);
            this.txtUserId.TabIndex = 12;
            this.txtUserId.Title = "用户名";
            this.txtUserId.ValueCompareTo = null;
            // 
            // lblUserId
            // 
            this.lblUserId.AutoSize = true;
            this.lblUserId.Location = new System.Drawing.Point(18, 76);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(221, 12);
            this.lblUserId.TabIndex = 12;
            this.lblUserId.Text = "${DataSourceCreateView_LabelUserId}:";
            // 
            // radioButtonNoIntegratedSecurity
            // 
            this.radioButtonNoIntegratedSecurity.AutoSize = true;
            this.radioButtonNoIntegratedSecurity.Location = new System.Drawing.Point(6, 45);
            this.radioButtonNoIntegratedSecurity.Name = "radioButtonNoIntegratedSecurity";
            this.radioButtonNoIntegratedSecurity.Size = new System.Drawing.Size(353, 16);
            this.radioButtonNoIntegratedSecurity.TabIndex = 12;
            this.radioButtonNoIntegratedSecurity.Text = "${DataSourceCreateView_RadioButtonNoIntegratedSecurity}";
            this.radioButtonNoIntegratedSecurity.UseVisualStyleBackColor = true;
            this.radioButtonNoIntegratedSecurity.CheckedChanged += new System.EventHandler(this.RadioButtonCheckedChanged);
            // 
            // radioButtonIntegratedSecurity
            // 
            this.radioButtonIntegratedSecurity.AutoSize = true;
            this.radioButtonIntegratedSecurity.Checked = true;
            this.radioButtonIntegratedSecurity.Location = new System.Drawing.Point(6, 23);
            this.radioButtonIntegratedSecurity.Name = "radioButtonIntegratedSecurity";
            this.radioButtonIntegratedSecurity.Size = new System.Drawing.Size(341, 16);
            this.radioButtonIntegratedSecurity.TabIndex = 12;
            this.radioButtonIntegratedSecurity.TabStop = true;
            this.radioButtonIntegratedSecurity.Text = "${DataSourceCreateView_RadioButtonIntegratedSecurity}";
            this.radioButtonIntegratedSecurity.UseVisualStyleBackColor = true;
            this.radioButtonIntegratedSecurity.CheckedChanged += new System.EventHandler(this.RadioButtonCheckedChanged);
            // 
            // lblNotice
            // 
            this.lblNotice.Location = new System.Drawing.Point(23, 14);
            this.lblNotice.Name = "lblNotice";
            this.lblNotice.Size = new System.Drawing.Size(325, 28);
            this.lblNotice.TabIndex = 12;
            this.lblNotice.Text = "${DataSourceCreateView_LabelNotice}";
            // 
            // FormDataSourceCreate
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(369, 374);
            this.Controls.Add(this.lblNotice);
            this.Controls.Add(this.groupBoxLoginOption);
            this.Controls.Add(this.btnRefreshDataSource);
            this.Controls.Add(this.ddlDataSource);
            this.Controls.Add(this.lblDataSourceName);
            this.Controls.Add(this.txtDataSourceType);
            this.Controls.Add(this.lblDataSourceType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDataSourceCreate";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${DataSourceCreateView}";
            this.groupBoxLoginOption.ResumeLayout(false);
            this.groupBoxLoginOption.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private System.Windows.Forms.Label lblDataSourceType;
        private Sheng.SailingEase.Controls.SETextBox txtDataSourceType;
        private System.Windows.Forms.Label lblDataSourceName;
        private Sheng.SailingEase.Controls.SEComboBox ddlDataSource;
        private Sheng.SailingEase.Controls.SEButton btnRefreshDataSource;
        private Sheng.SailingEase.Controls.SEGroupBox groupBoxLoginOption;
        private System.Windows.Forms.RadioButton radioButtonNoIntegratedSecurity;
        private System.Windows.Forms.RadioButton radioButtonIntegratedSecurity;
        private Sheng.SailingEase.Controls.SETextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private Sheng.SailingEase.Controls.SETextBox txtUserId;
        private System.Windows.Forms.Label lblUserId;
        private System.Windows.Forms.Label lblNotice;
    }
}