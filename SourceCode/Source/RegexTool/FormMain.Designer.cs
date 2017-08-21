namespace Sheng.SailingEase.RegexTool
{
    partial class FormMain
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
            this.btnMatches = new System.Windows.Forms.Button();
            this.btnSplit = new System.Windows.Forms.Button();
            this.btnReplace = new System.Windows.Forms.Button();
            this.btnIsMatch = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkMultiLine = new System.Windows.Forms.CheckBox();
            this.chkExplicitCapture = new System.Windows.Forms.CheckBox();
            this.chkSingleLine = new System.Windows.Forms.CheckBox();
            this.chkIgnoreCase = new System.Windows.Forms.CheckBox();
            this.txtRegex = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtReplace = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.btnGroups = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblTitle = new SailingEase.Controls.SEAdvLabel();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnMatches
            // 
            this.btnMatches.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMatches.Location = new System.Drawing.Point(446, 487);
            this.btnMatches.Name = "btnMatches";
            this.btnMatches.Size = new System.Drawing.Size(100, 23);
            this.btnMatches.TabIndex = 7;
            this.btnMatches.Text = "Matches";
            this.btnMatches.Click += new System.EventHandler(this.btnMatches_Click);
            // 
            // btnSplit
            // 
            this.btnSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSplit.Location = new System.Drawing.Point(311, 487);
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(100, 23);
            this.btnSplit.TabIndex = 6;
            this.btnSplit.Text = "Split";
            this.btnSplit.Click += new System.EventHandler(this.btnSplit_Click);
            // 
            // btnReplace
            // 
            this.btnReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReplace.Location = new System.Drawing.Point(176, 487);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(100, 23);
            this.btnReplace.TabIndex = 5;
            this.btnReplace.Text = "Replace";
            this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
            // 
            // btnIsMatch
            // 
            this.btnIsMatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnIsMatch.Location = new System.Drawing.Point(41, 487);
            this.btnIsMatch.Name = "btnIsMatch";
            this.btnIsMatch.Size = new System.Drawing.Size(100, 23);
            this.btnIsMatch.TabIndex = 4;
            this.btnIsMatch.Text = "IsMatch";
            this.btnIsMatch.Click += new System.EventHandler(this.btnIsMatch_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 343);
            this.label4.Margin = new System.Windows.Forms.Padding(6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "匹配的结果：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 267);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "用于替换的文本：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkMultiLine);
            this.groupBox1.Controls.Add(this.chkExplicitCapture);
            this.groupBox1.Controls.Add(this.chkSingleLine);
            this.groupBox1.Controls.Add(this.chkIgnoreCase);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.groupBox1.Location = new System.Drawing.Point(41, 115);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(640, 55);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "正则表达式选项";
            // 
            // chkMultiLine
            // 
            this.chkMultiLine.Location = new System.Drawing.Point(463, 20);
            this.chkMultiLine.Name = "chkMultiLine";
            this.chkMultiLine.Size = new System.Drawing.Size(104, 24);
            this.chkMultiLine.TabIndex = 3;
            this.chkMultiLine.Text = "MultiLine";
            // 
            // chkExplicitCapture
            // 
            this.chkExplicitCapture.Location = new System.Drawing.Point(309, 20);
            this.chkExplicitCapture.Name = "chkExplicitCapture";
            this.chkExplicitCapture.Size = new System.Drawing.Size(120, 24);
            this.chkExplicitCapture.TabIndex = 2;
            this.chkExplicitCapture.Text = "ExplicitCapture";
            // 
            // chkSingleLine
            // 
            this.chkSingleLine.Location = new System.Drawing.Point(171, 20);
            this.chkSingleLine.Name = "chkSingleLine";
            this.chkSingleLine.Size = new System.Drawing.Size(104, 24);
            this.chkSingleLine.TabIndex = 1;
            this.chkSingleLine.Text = "SingleLine";
            // 
            // chkIgnoreCase
            // 
            this.chkIgnoreCase.Location = new System.Drawing.Point(33, 20);
            this.chkIgnoreCase.Name = "chkIgnoreCase";
            this.chkIgnoreCase.Size = new System.Drawing.Size(104, 24);
            this.chkIgnoreCase.TabIndex = 0;
            this.chkIgnoreCase.Text = "IgnoreCase";
            // 
            // txtRegex
            // 
            this.txtRegex.Location = new System.Drawing.Point(41, 61);
            this.txtRegex.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtRegex.Multiline = true;
            this.txtRegex.Name = "txtRegex";
            this.txtRegex.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRegex.Size = new System.Drawing.Size(640, 42);
            this.txtRegex.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "正则表达式：";
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(41, 206);
            this.txtInput.Margin = new System.Windows.Forms.Padding(6);
            this.txtInput.Multiline = true;
            this.txtInput.Name = "txtInput";
            this.txtInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtInput.Size = new System.Drawing.Size(640, 49);
            this.txtInput.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 182);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "用于匹配的文本：";
            // 
            // txtReplace
            // 
            this.txtReplace.Location = new System.Drawing.Point(41, 291);
            this.txtReplace.Margin = new System.Windows.Forms.Padding(6);
            this.txtReplace.Multiline = true;
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReplace.Size = new System.Drawing.Size(640, 40);
            this.txtReplace.TabIndex = 2;
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(41, 367);
            this.txtResult.Margin = new System.Windows.Forms.Padding(6);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(640, 96);
            this.txtResult.TabIndex = 3;
            // 
            // btnGroups
            // 
            this.btnGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGroups.Location = new System.Drawing.Point(581, 487);
            this.btnGroups.Name = "btnGroups";
            this.btnGroups.Size = new System.Drawing.Size(100, 23);
            this.btnGroups.TabIndex = 8;
            this.btnGroups.Text = "Groups";
            this.btnGroups.Click += new System.EventHandler(this.btnGroups_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(525, 32);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 9;
            this.btnBrowse.Text = "预置";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.BorderColor = System.Drawing.Color.Black;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.FillColorEnd = System.Drawing.Color.Empty;
            this.lblTitle.FillColorStart = System.Drawing.Color.White;
            this.lblTitle.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.lblTitle.FillStyle = Sheng.SailingEase.Controls.FillStyle.LinearGradient;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.ShowBorder = false;
            this.lblTitle.SingleLine = true;
            this.lblTitle.Size = new System.Drawing.Size(712, 23);
            this.lblTitle.TabIndex = 25;
            this.lblTitle.TextHorizontalCenter = false;
            this.lblTitle.TextVerticalCenter = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(606, 32);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 533);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnMatches);
            this.Controls.Add(this.btnSplit);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.btnIsMatch);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtRegex);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtReplace);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnGroups);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "正则表达式工具";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMatches;
        private System.Windows.Forms.Button btnSplit;
        private System.Windows.Forms.Button btnReplace;
        private System.Windows.Forms.Button btnIsMatch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkMultiLine;
        private System.Windows.Forms.CheckBox chkExplicitCapture;
        private System.Windows.Forms.CheckBox chkSingleLine;
        private System.Windows.Forms.CheckBox chkIgnoreCase;
        private System.Windows.Forms.TextBox txtRegex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtReplace;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button btnGroups;
        private System.Windows.Forms.Button btnBrowse;
        private Sheng.SailingEase.Controls.SEAdvLabel lblTitle;
        private System.Windows.Forms.Button btnOK;
    }
}

