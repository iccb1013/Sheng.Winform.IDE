namespace Sheng.SailingEase.Modules.ProjectModule.View
{
    partial class ProjectPropertyView
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
            this.txtAboutImg = new Sheng.SailingEase.Controls.SETextBox();
            this.tabControl1 = new Sheng.SailingEase.Controls.SETabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.lblUseDataBase = new System.Windows.Forms.Label();
            this.lblCode = new System.Windows.Forms.Label();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.cbUserSubsequent = new System.Windows.Forms.CheckBox();
            this.cbUserPopedomModel = new System.Windows.Forms.CheckBox();
            this.cbUserModel = new System.Windows.Forms.CheckBox();
            this.ddlDataBase = new Sheng.SailingEase.Controls.SEComboBox();
            this.txtPrjCode = new Sheng.SailingEase.Controls.SETextBox();
            this.txtPrjName = new Sheng.SailingEase.Controls.SETextBox();
            this.tabPageAbout = new System.Windows.Forms.TabPage();
            this.lblSplashImg = new System.Windows.Forms.Label();
            this.lblBackImg = new System.Windows.Forms.Label();
            this.lblAboutImg = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblSummary = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblCompany = new System.Windows.Forms.Label();
            this.btnBrowseAboutImg = new Sheng.SailingEase.Controls.SEButton();
            this.btnBrowseBackImg = new Sheng.SailingEase.Controls.SEButton();
            this.btnBrowseSplashImg = new Sheng.SailingEase.Controls.SEButton();
            this.txtSplashImg = new Sheng.SailingEase.Controls.SETextBox();
            this.txtBackImg = new Sheng.SailingEase.Controls.SETextBox();
            this.txtCopyright = new Sheng.SailingEase.Controls.SETextBox();
            this.txtSummary = new Sheng.SailingEase.Controls.SETextBox();
            this.txtVersion = new Sheng.SailingEase.Controls.SETextBox();
            this.txtCompany = new Sheng.SailingEase.Controls.SETextBox();
            this.tabPageRemark = new System.Windows.Forms.TabPage();
            this.txtRemark = new Sheng.SailingEase.Controls.SETextBox();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.openFileDialogImg = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabPageAbout.SuspendLayout();
            this.tabPageRemark.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtAboutImg
            // 
            this.txtAboutImg.AllowEmpty = true;
            this.txtAboutImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtAboutImg.CustomValidate = null;
            this.txtAboutImg.HighLight = true;
            this.txtAboutImg.LimitMaxValue = false;
            this.txtAboutImg.Location = new System.Drawing.Point(103, 369);
            this.txtAboutImg.MaxValue = ((long)(2147483647));
            this.txtAboutImg.Name = "txtAboutImg";
            this.txtAboutImg.Regex = "";
            this.txtAboutImg.RegexMsg = null;
            this.txtAboutImg.Size = new System.Drawing.Size(294, 21);
            this.txtAboutImg.TabIndex = 9;
            this.txtAboutImg.Title = null;
            this.txtAboutImg.ValueCompareTo = null;
            this.txtAboutImg.WaterText = "";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageGeneral);
            this.tabControl1.Controls.Add(this.tabPageAbout);
            this.tabControl1.Controls.Add(this.tabPageRemark);
            this.tabControl1.CustomValidate = null;
            this.tabControl1.HighLight = false;
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(479, 433);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Title = null;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.lblUseDataBase);
            this.tabPageGeneral.Controls.Add(this.lblCode);
            this.tabPageGeneral.Controls.Add(this.lblProjectName);
            this.tabPageGeneral.Controls.Add(this.cbUserSubsequent);
            this.tabPageGeneral.Controls.Add(this.cbUserPopedomModel);
            this.tabPageGeneral.Controls.Add(this.cbUserModel);
            this.tabPageGeneral.Controls.Add(this.ddlDataBase);
            this.tabPageGeneral.Controls.Add(this.txtPrjCode);
            this.tabPageGeneral.Controls.Add(this.txtPrjName);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(471, 407);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // lblUseDataBase
            // 
            this.lblUseDataBase.AutoSize = true;
            this.lblUseDataBase.Location = new System.Drawing.Point(21, 76);
            this.lblUseDataBase.Name = "lblUseDataBase";
            this.lblUseDataBase.Size = new System.Drawing.Size(245, 12);
            this.lblUseDataBase.TabIndex = 4;
            this.lblUseDataBase.Text = "${ProjectPropertyView_LabelUseDataBase}:";
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(21, 49);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(203, 12);
            this.lblCode.TabIndex = 2;
            this.lblCode.Text = "${ProjectPropertyView_LabelCode}:";
            // 
            // lblProjectName
            // 
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.Location = new System.Drawing.Point(21, 22);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(215, 12);
            this.lblProjectName.TabIndex = 0;
            this.lblProjectName.Text = "${ProjectPropertyView_ProjectName}:";
            // 
            // cbUserSubsequent
            // 
            this.cbUserSubsequent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbUserSubsequent.AutoSize = true;
            this.cbUserSubsequent.Enabled = false;
            this.cbUserSubsequent.Location = new System.Drawing.Point(23, 352);
            this.cbUserSubsequent.Name = "cbUserSubsequent";
            this.cbUserSubsequent.Size = new System.Drawing.Size(294, 16);
            this.cbUserSubsequent.TabIndex = 5;
            this.cbUserSubsequent.Text = "${ProjectPropertyView_CheckBoxUserSubsequent}";
            this.cbUserSubsequent.UseVisualStyleBackColor = true;
            this.cbUserSubsequent.Visible = false;
            // 
            // cbUserPopedomModel
            // 
            this.cbUserPopedomModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbUserPopedomModel.AutoSize = true;
            this.cbUserPopedomModel.Enabled = false;
            this.cbUserPopedomModel.Location = new System.Drawing.Point(23, 374);
            this.cbUserPopedomModel.Name = "cbUserPopedomModel";
            this.cbUserPopedomModel.Size = new System.Drawing.Size(306, 16);
            this.cbUserPopedomModel.TabIndex = 6;
            this.cbUserPopedomModel.Text = "${ProjectPropertyView_CheckBoxUserPopedomModel}";
            this.cbUserPopedomModel.UseVisualStyleBackColor = true;
            this.cbUserPopedomModel.Visible = false;
            // 
            // cbUserModel
            // 
            this.cbUserModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbUserModel.AutoSize = true;
            this.cbUserModel.Checked = true;
            this.cbUserModel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUserModel.Enabled = false;
            this.cbUserModel.Location = new System.Drawing.Point(23, 330);
            this.cbUserModel.Name = "cbUserModel";
            this.cbUserModel.Size = new System.Drawing.Size(264, 16);
            this.cbUserModel.TabIndex = 4;
            this.cbUserModel.Text = "${ProjectPropertyView_CheckBoxUserModel}";
            this.cbUserModel.UseVisualStyleBackColor = true;
            this.cbUserModel.Visible = false;
            // 
            // ddlDataBase
            // 
            this.ddlDataBase.AllowEmpty = true;
            this.ddlDataBase.CustomValidate = null;
            this.ddlDataBase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDataBase.FormattingEnabled = true;
            this.ddlDataBase.HighLight = true;
            this.ddlDataBase.Location = new System.Drawing.Point(113, 73);
            this.ddlDataBase.Name = "ddlDataBase";
            this.ddlDataBase.Size = new System.Drawing.Size(274, 20);
            this.ddlDataBase.TabIndex = 3;
            this.ddlDataBase.Title = null;
            this.ddlDataBase.WaterText = "";
            // 
            // txtPrjCode
            // 
            this.txtPrjCode.AllowEmpty = false;
            this.txtPrjCode.CustomValidate = null;
            this.txtPrjCode.HighLight = true;
            this.txtPrjCode.LimitMaxValue = false;
            this.txtPrjCode.Location = new System.Drawing.Point(113, 46);
            this.txtPrjCode.MaxLength = 100;
            this.txtPrjCode.MaxValue = ((long)(2147483647));
            this.txtPrjCode.Name = "txtPrjCode";
            this.txtPrjCode.Regex = "";
            this.txtPrjCode.RegexMsg = "RegexMsg_OnlyAllowMonogram";
            this.txtPrjCode.Size = new System.Drawing.Size(274, 21);
            this.txtPrjCode.TabIndex = 2;
            this.txtPrjCode.Title = "LabelCode";
            this.txtPrjCode.ValueCompareTo = null;
            this.txtPrjCode.WaterText = "";
            // 
            // txtPrjName
            // 
            this.txtPrjName.AllowEmpty = false;
            this.txtPrjName.CustomValidate = null;
            this.txtPrjName.HighLight = true;
            this.txtPrjName.LimitMaxValue = false;
            this.txtPrjName.Location = new System.Drawing.Point(113, 19);
            this.txtPrjName.MaxLength = 200;
            this.txtPrjName.MaxValue = ((long)(2147483647));
            this.txtPrjName.Name = "txtPrjName";
            this.txtPrjName.Regex = "";
            this.txtPrjName.RegexMsg = null;
            this.txtPrjName.Size = new System.Drawing.Size(274, 21);
            this.txtPrjName.TabIndex = 1;
            this.txtPrjName.Title = "LabelProjectName";
            this.txtPrjName.ValueCompareTo = null;
            this.txtPrjName.WaterText = "";
            // 
            // tabPageAbout
            // 
            this.tabPageAbout.Controls.Add(this.lblSplashImg);
            this.tabPageAbout.Controls.Add(this.lblBackImg);
            this.tabPageAbout.Controls.Add(this.lblAboutImg);
            this.tabPageAbout.Controls.Add(this.lblCopyright);
            this.tabPageAbout.Controls.Add(this.lblSummary);
            this.tabPageAbout.Controls.Add(this.lblVersion);
            this.tabPageAbout.Controls.Add(this.lblCompany);
            this.tabPageAbout.Controls.Add(this.btnBrowseAboutImg);
            this.tabPageAbout.Controls.Add(this.btnBrowseBackImg);
            this.tabPageAbout.Controls.Add(this.btnBrowseSplashImg);
            this.tabPageAbout.Controls.Add(this.txtSplashImg);
            this.tabPageAbout.Controls.Add(this.txtBackImg);
            this.tabPageAbout.Controls.Add(this.txtAboutImg);
            this.tabPageAbout.Controls.Add(this.txtCopyright);
            this.tabPageAbout.Controls.Add(this.txtSummary);
            this.tabPageAbout.Controls.Add(this.txtVersion);
            this.tabPageAbout.Controls.Add(this.txtCompany);
            this.tabPageAbout.Location = new System.Drawing.Point(4, 22);
            this.tabPageAbout.Name = "tabPageAbout";
            this.tabPageAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAbout.Size = new System.Drawing.Size(471, 407);
            this.tabPageAbout.TabIndex = 1;
            this.tabPageAbout.Text = "About";
            this.tabPageAbout.UseVisualStyleBackColor = true;
            // 
            // lblSplashImg
            // 
            this.lblSplashImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSplashImg.AutoSize = true;
            this.lblSplashImg.Location = new System.Drawing.Point(21, 320);
            this.lblSplashImg.Name = "lblSplashImg";
            this.lblSplashImg.Size = new System.Drawing.Size(203, 12);
            this.lblSplashImg.TabIndex = 12;
            this.lblSplashImg.Text = "${ProjectPropertyView_SplashImg}:";
            // 
            // lblBackImg
            // 
            this.lblBackImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblBackImg.AutoSize = true;
            this.lblBackImg.Location = new System.Drawing.Point(21, 347);
            this.lblBackImg.Name = "lblBackImg";
            this.lblBackImg.Size = new System.Drawing.Size(221, 12);
            this.lblBackImg.TabIndex = 10;
            this.lblBackImg.Text = "${ProjectPropertyView_LabelBackImg}:";
            // 
            // lblAboutImg
            // 
            this.lblAboutImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAboutImg.AutoSize = true;
            this.lblAboutImg.Location = new System.Drawing.Point(21, 374);
            this.lblAboutImg.Name = "lblAboutImg";
            this.lblAboutImg.Size = new System.Drawing.Size(227, 12);
            this.lblAboutImg.TabIndex = 8;
            this.lblAboutImg.Text = "${ProjectPropertyView_LabelAboutImg}:";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Location = new System.Drawing.Point(21, 104);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(233, 12);
            this.lblCopyright.TabIndex = 6;
            this.lblCopyright.Text = "${ProjectPropertyView_LabelCopyright}:";
            // 
            // lblSummary
            // 
            this.lblSummary.AutoSize = true;
            this.lblSummary.Location = new System.Drawing.Point(21, 77);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(221, 12);
            this.lblSummary.TabIndex = 4;
            this.lblSummary.Text = "${ProjectPropertyView_LabelSummary}:";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(21, 50);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(221, 12);
            this.lblVersion.TabIndex = 2;
            this.lblVersion.Text = "${ProjectPropertyView_LabelVersion}:";
            // 
            // lblCompany
            // 
            this.lblCompany.AutoSize = true;
            this.lblCompany.Location = new System.Drawing.Point(21, 23);
            this.lblCompany.Name = "lblCompany";
            this.lblCompany.Size = new System.Drawing.Size(221, 12);
            this.lblCompany.TabIndex = 0;
            this.lblCompany.Text = "${ProjectPropertyView_LabelCompany}:";
            // 
            // btnBrowseAboutImg
            // 
            this.btnBrowseAboutImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseAboutImg.Location = new System.Drawing.Point(403, 368);
            this.btnBrowseAboutImg.Name = "btnBrowseAboutImg";
            this.btnBrowseAboutImg.Size = new System.Drawing.Size(33, 23);
            this.btnBrowseAboutImg.TabIndex = 10;
            this.btnBrowseAboutImg.Text = "...";
            this.btnBrowseAboutImg.UseVisualStyleBackColor = true;
            this.btnBrowseAboutImg.Click += new System.EventHandler(this.btnBrowseAboutImg_Click);
            // 
            // btnBrowseBackImg
            // 
            this.btnBrowseBackImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseBackImg.Location = new System.Drawing.Point(403, 341);
            this.btnBrowseBackImg.Name = "btnBrowseBackImg";
            this.btnBrowseBackImg.Size = new System.Drawing.Size(33, 23);
            this.btnBrowseBackImg.TabIndex = 8;
            this.btnBrowseBackImg.Text = "...";
            this.btnBrowseBackImg.UseVisualStyleBackColor = true;
            this.btnBrowseBackImg.Click += new System.EventHandler(this.btnBrowseBackImg_Click);
            // 
            // btnBrowseSplashImg
            // 
            this.btnBrowseSplashImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseSplashImg.Enabled = false;
            this.btnBrowseSplashImg.Location = new System.Drawing.Point(403, 312);
            this.btnBrowseSplashImg.Name = "btnBrowseSplashImg";
            this.btnBrowseSplashImg.Size = new System.Drawing.Size(33, 23);
            this.btnBrowseSplashImg.TabIndex = 6;
            this.btnBrowseSplashImg.Text = "...";
            this.btnBrowseSplashImg.UseVisualStyleBackColor = true;
            this.btnBrowseSplashImg.Click += new System.EventHandler(this.btnBrowseSplashImg_Click);
            // 
            // txtSplashImg
            // 
            this.txtSplashImg.AllowEmpty = true;
            this.txtSplashImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSplashImg.CustomValidate = null;
            this.txtSplashImg.Enabled = false;
            this.txtSplashImg.HighLight = true;
            this.txtSplashImg.LimitMaxValue = false;
            this.txtSplashImg.Location = new System.Drawing.Point(103, 315);
            this.txtSplashImg.MaxValue = ((long)(2147483647));
            this.txtSplashImg.Name = "txtSplashImg";
            this.txtSplashImg.Regex = "";
            this.txtSplashImg.RegexMsg = null;
            this.txtSplashImg.Size = new System.Drawing.Size(294, 21);
            this.txtSplashImg.TabIndex = 5;
            this.txtSplashImg.Text = "Resources\\Splash.JPG";
            this.txtSplashImg.Title = null;
            this.txtSplashImg.ValueCompareTo = null;
            this.txtSplashImg.WaterText = "";
            // 
            // txtBackImg
            // 
            this.txtBackImg.AllowEmpty = true;
            this.txtBackImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtBackImg.CustomValidate = null;
            this.txtBackImg.HighLight = true;
            this.txtBackImg.LimitMaxValue = false;
            this.txtBackImg.Location = new System.Drawing.Point(103, 342);
            this.txtBackImg.MaxValue = ((long)(2147483647));
            this.txtBackImg.Name = "txtBackImg";
            this.txtBackImg.Regex = "";
            this.txtBackImg.RegexMsg = null;
            this.txtBackImg.Size = new System.Drawing.Size(294, 21);
            this.txtBackImg.TabIndex = 7;
            this.txtBackImg.Title = null;
            this.txtBackImg.ValueCompareTo = null;
            this.txtBackImg.WaterText = "";
            // 
            // txtCopyright
            // 
            this.txtCopyright.AllowEmpty = true;
            this.txtCopyright.CustomValidate = null;
            this.txtCopyright.HighLight = true;
            this.txtCopyright.LimitMaxValue = false;
            this.txtCopyright.Location = new System.Drawing.Point(103, 101);
            this.txtCopyright.MaxLength = 500;
            this.txtCopyright.MaxValue = ((long)(2147483647));
            this.txtCopyright.Name = "txtCopyright";
            this.txtCopyright.Regex = "";
            this.txtCopyright.RegexMsg = null;
            this.txtCopyright.Size = new System.Drawing.Size(291, 21);
            this.txtCopyright.TabIndex = 4;
            this.txtCopyright.Title = null;
            this.txtCopyright.ValueCompareTo = null;
            this.txtCopyright.WaterText = "";
            // 
            // txtSummary
            // 
            this.txtSummary.AllowEmpty = true;
            this.txtSummary.CustomValidate = null;
            this.txtSummary.HighLight = true;
            this.txtSummary.LimitMaxValue = false;
            this.txtSummary.Location = new System.Drawing.Point(103, 74);
            this.txtSummary.MaxLength = 500;
            this.txtSummary.MaxValue = ((long)(2147483647));
            this.txtSummary.Name = "txtSummary";
            this.txtSummary.Regex = "";
            this.txtSummary.RegexMsg = null;
            this.txtSummary.Size = new System.Drawing.Size(291, 21);
            this.txtSummary.TabIndex = 3;
            this.txtSummary.Title = null;
            this.txtSummary.ValueCompareTo = null;
            this.txtSummary.WaterText = "";
            // 
            // txtVersion
            // 
            this.txtVersion.AllowEmpty = true;
            this.txtVersion.CustomValidate = null;
            this.txtVersion.HighLight = true;
            this.txtVersion.LimitMaxValue = false;
            this.txtVersion.Location = new System.Drawing.Point(103, 47);
            this.txtVersion.MaxLength = 100;
            this.txtVersion.MaxValue = ((long)(2147483647));
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Regex = "";
            this.txtVersion.RegexMsg = null;
            this.txtVersion.Size = new System.Drawing.Size(291, 21);
            this.txtVersion.TabIndex = 2;
            this.txtVersion.Title = null;
            this.txtVersion.ValueCompareTo = null;
            this.txtVersion.WaterText = "";
            // 
            // txtCompany
            // 
            this.txtCompany.AllowEmpty = true;
            this.txtCompany.CustomValidate = null;
            this.txtCompany.HighLight = true;
            this.txtCompany.LimitMaxValue = false;
            this.txtCompany.Location = new System.Drawing.Point(103, 20);
            this.txtCompany.MaxLength = 200;
            this.txtCompany.MaxValue = ((long)(2147483647));
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.Regex = "";
            this.txtCompany.RegexMsg = null;
            this.txtCompany.Size = new System.Drawing.Size(291, 21);
            this.txtCompany.TabIndex = 1;
            this.txtCompany.Title = null;
            this.txtCompany.ValueCompareTo = null;
            this.txtCompany.WaterText = "";
            // 
            // tabPageRemark
            // 
            this.tabPageRemark.Controls.Add(this.txtRemark);
            this.tabPageRemark.Location = new System.Drawing.Point(4, 22);
            this.tabPageRemark.Name = "tabPageRemark";
            this.tabPageRemark.Padding = new System.Windows.Forms.Padding(5);
            this.tabPageRemark.Size = new System.Drawing.Size(471, 407);
            this.tabPageRemark.TabIndex = 2;
            this.tabPageRemark.Text = "Remark";
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
            this.txtRemark.Size = new System.Drawing.Size(461, 397);
            this.txtRemark.TabIndex = 0;
            this.txtRemark.Title = null;
            this.txtRemark.ValueCompareTo = null;
            this.txtRemark.WaterText = "";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(335, 451);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "${ProjectPropertyView_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(416, 451);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "${ProjectPropertyView_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // openFileDialogImg
            // 
            this.openFileDialogImg.FileName = "openFileDialog1";
            this.openFileDialogImg.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            // 
            // ProjectPropertyView
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(503, 486);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectPropertyView";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${ProjectPropertyView}";
            this.Load += new System.EventHandler(this.ProjectPropertyView_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.tabPageAbout.ResumeLayout(false);
            this.tabPageAbout.PerformLayout();
            this.tabPageRemark.ResumeLayout(false);
            this.tabPageRemark.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Sheng.SailingEase.Controls.SETabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageAbout;
        private System.Windows.Forms.TabPage tabPageRemark;
        private Sheng.SailingEase.Controls.SETextBox txtPrjCode;
        private System.Windows.Forms.Label lblCode;
        private Sheng.SailingEase.Controls.SETextBox txtPrjName;
        private System.Windows.Forms.Label lblProjectName;
        private Sheng.SailingEase.Controls.SEComboBox ddlDataBase;
        private System.Windows.Forms.Label lblUseDataBase;
        private System.Windows.Forms.CheckBox cbUserSubsequent;
        private System.Windows.Forms.CheckBox cbUserPopedomModel;
        private System.Windows.Forms.CheckBox cbUserModel;
        private Sheng.SailingEase.Controls.SETextBox txtRemark;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private Sheng.SailingEase.Controls.SETextBox txtVersion;
        private System.Windows.Forms.Label lblVersion;
        private Sheng.SailingEase.Controls.SETextBox txtCompany;
        private System.Windows.Forms.Label lblCompany;
        private Sheng.SailingEase.Controls.SETextBox txtCopyright;
        private System.Windows.Forms.Label lblCopyright;
        private Sheng.SailingEase.Controls.SETextBox txtSummary;
        private System.Windows.Forms.Label lblSummary;
        private Sheng.SailingEase.Controls.SETextBox txtSplashImg;
        private System.Windows.Forms.Label lblSplashImg;
        private Sheng.SailingEase.Controls.SETextBox txtBackImg;
        private System.Windows.Forms.Label lblBackImg;
        private System.Windows.Forms.Label lblAboutImg;
        private Sheng.SailingEase.Controls.SEButton btnBrowseAboutImg;
        private Sheng.SailingEase.Controls.SEButton btnBrowseBackImg;
        private Sheng.SailingEase.Controls.SEButton btnBrowseSplashImg;
        private Sheng.SailingEase.Controls.SETextBox txtAboutImg;
        private System.Windows.Forms.OpenFileDialog openFileDialogImg;
    }
}