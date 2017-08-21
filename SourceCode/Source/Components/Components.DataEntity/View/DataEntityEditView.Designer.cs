namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    partial class DataEntityEditView
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
            this.tabControl1 = new Sheng.SailingEase.Controls.SETabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.lblCode = new System.Windows.Forms.Label();
            this.lblDataEntityName = new System.Windows.Forms.Label();
            this.txtCode = new Sheng.SailingEase.Controls.SETextBox();
            this.txtName = new Sheng.SailingEase.Controls.SETextBox();
            this.tabPageRemark = new System.Windows.Forms.TabPage();
            this.txtRemark = new Sheng.SailingEase.Controls.SETextBox();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.tabControl1.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
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
            this.tabControl1.Size = new System.Drawing.Size(438, 215);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Title = null;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.lblCode);
            this.tabPageGeneral.Controls.Add(this.lblDataEntityName);
            this.tabPageGeneral.Controls.Add(this.txtCode);
            this.tabPageGeneral.Controls.Add(this.txtName);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(430, 189);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(17, 47);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(191, 12);
            this.lblCode.TabIndex = 7;
            this.lblCode.Text = "${DataEntityEditView_LabelCode}:";
            // 
            // lblDataEntityName
            // 
            this.lblDataEntityName.AutoSize = true;
            this.lblDataEntityName.Location = new System.Drawing.Point(17, 20);
            this.lblDataEntityName.Name = "lblDataEntityName";
            this.lblDataEntityName.Size = new System.Drawing.Size(251, 12);
            this.lblDataEntityName.TabIndex = 4;
            this.lblDataEntityName.Text = "${DataEntityEditView_LabelDataEntityName}:";
            // 
            // txtCode
            // 
            this.txtCode.AllowEmpty = false;
            this.txtCode.CustomValidate = null;
            this.txtCode.HighLight = true;
            this.txtCode.LimitMaxValue = false;
            this.txtCode.Location = new System.Drawing.Point(105, 44);
            this.txtCode.MaxLength = 100;
            this.txtCode.MaxValue = ((long)(2147483647));
            this.txtCode.Name = "txtCode";
            this.txtCode.Regex = "";
            this.txtCode.RegexMsg = "RegexMsg_OnlyAllowMonogram";
            this.txtCode.Size = new System.Drawing.Size(215, 21);
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
            this.txtName.Location = new System.Drawing.Point(105, 17);
            this.txtName.MaxLength = 100;
            this.txtName.MaxValue = ((long)(2147483647));
            this.txtName.Name = "txtName";
            this.txtName.Regex = "";
            this.txtName.RegexMsg = null;
            this.txtName.Size = new System.Drawing.Size(215, 21);
            this.txtName.TabIndex = 1;
            this.txtName.Title = "LabelDataEntityName";
            this.txtName.ValueCompareTo = null;
            this.txtName.WaterText = "";
            // 
            // tabPageRemark
            // 
            this.tabPageRemark.BackColor = System.Drawing.SystemColors.Window;
            this.tabPageRemark.Controls.Add(this.txtRemark);
            this.tabPageRemark.Location = new System.Drawing.Point(4, 22);
            this.tabPageRemark.Name = "tabPageRemark";
            this.tabPageRemark.Padding = new System.Windows.Forms.Padding(5);
            this.tabPageRemark.Size = new System.Drawing.Size(430, 189);
            this.tabPageRemark.TabIndex = 1;
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
            this.txtRemark.Size = new System.Drawing.Size(420, 180);
            this.txtRemark.TabIndex = 5;
            this.txtRemark.Title = null;
            this.txtRemark.ValueCompareTo = null;
            this.txtRemark.WaterText = "";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(375, 233);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "${DataEntityEditView_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(294, 233);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "${DataEntityEditView_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FormDataEntityAdd
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(462, 274);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDataEntityAdd";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${DataEntityAddView}";
            this.Load += new System.EventHandler(this.FormDataEntityAdd_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
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
        private System.Windows.Forms.Label lblDataEntityName;
        private Sheng.SailingEase.Controls.SETextBox txtCode;
        private Sheng.SailingEase.Controls.SETextBox txtName;
        private Sheng.SailingEase.Controls.SETextBox txtRemark;
    }
}