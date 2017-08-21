namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    partial class MenuEditView
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
            this.lblName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtText = new Sheng.SailingEase.Controls.SETextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtCode = new Sheng.SailingEase.Controls.SETextBox();
            this.txtName = new Sheng.SailingEase.Controls.SETextBox();
            this.tabPageEvent = new System.Windows.Forms.TabPage();
            this.panelEvent = new Sheng.SailingEase.Controls.SEPanel();
            this.tabPageRemark = new System.Windows.Forms.TabPage();
            this.txtRemark = new Sheng.SailingEase.Controls.SETextBox();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.tabControl1.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabPageEvent.SuspendLayout();
            this.tabPageRemark.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageGeneral);
            this.tabControl1.Controls.Add(this.tabPageEvent);
            this.tabControl1.Controls.Add(this.tabPageRemark);
            this.tabControl1.HighLight = false;
            this.tabControl1.Location = new System.Drawing.Point(10, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(440, 281);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.lblName);
            this.tabPageGeneral.Controls.Add(this.label1);
            this.tabPageGeneral.Controls.Add(this.txtText);
            this.tabPageGeneral.Controls.Add(this.lblCode);
            this.tabPageGeneral.Controls.Add(this.txtCode);
            this.tabPageGeneral.Controls.Add(this.txtName);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 21);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(12);
            this.tabPageGeneral.Size = new System.Drawing.Size(432, 256);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(15, 18);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(179, 12);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "${MenuEditView_LabelName}:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "${MenuEditView_LabelText}:";
            // 
            // txtText
            // 
            this.txtText.AllowEmpty = true;
            this.txtText.HighLight = true;
            this.txtText.LimitMaxValue = false;
            this.txtText.Location = new System.Drawing.Point(82, 69);
            this.txtText.MaxValue = ((long)(2147483647));
            this.txtText.Name = "txtText";
            this.txtText.Regex = "";
            this.txtText.RegexMsg = "";
            this.txtText.Size = new System.Drawing.Size(169, 21);
            this.txtText.TabIndex = 4;
            this.txtText.Title = "";
            this.txtText.ValueCompareTo = null;
            this.txtText.WaterText = "";
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(15, 45);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(179, 12);
            this.lblCode.TabIndex = 3;
            this.lblCode.Text = "${MenuEditView_LabelCode}:";
            // 
            // txtCode
            // 
            this.txtCode.AllowEmpty = false;
            this.txtCode.HighLight = true;
            this.txtCode.LimitMaxValue = false;
            this.txtCode.Location = new System.Drawing.Point(82, 42);
            this.txtCode.MaxLength = 100;
            this.txtCode.MaxValue = ((long)(2147483647));
            this.txtCode.Name = "txtCode";
            this.txtCode.Regex = "";
            this.txtCode.RegexMsg = "RegexMsg_OnlyAllowMonogram";
            this.txtCode.Size = new System.Drawing.Size(169, 21);
            this.txtCode.TabIndex = 2;
            this.txtCode.Title = "LabelCode";
            this.txtCode.ValueCompareTo = null;
            this.txtCode.WaterText = "";
            // 
            // txtName
            // 
            this.txtName.AllowEmpty = false;
            this.txtName.HighLight = true;
            this.txtName.LimitMaxValue = false;
            this.txtName.Location = new System.Drawing.Point(82, 15);
            this.txtName.MaxLength = 100;
            this.txtName.MaxValue = ((long)(2147483647));
            this.txtName.Name = "txtName";
            this.txtName.Regex = "";
            this.txtName.RegexMsg = null;
            this.txtName.Size = new System.Drawing.Size(169, 21);
            this.txtName.TabIndex = 1;
            this.txtName.Title = "LabelName";
            this.txtName.ValueCompareTo = null;
            this.txtName.WaterText = "";
            // 
            // tabPageEvent
            // 
            this.tabPageEvent.Controls.Add(this.panelEvent);
            this.tabPageEvent.Location = new System.Drawing.Point(4, 21);
            this.tabPageEvent.Name = "tabPageEvent";
            this.tabPageEvent.Size = new System.Drawing.Size(432, 256);
            this.tabPageEvent.TabIndex = 2;
            this.tabPageEvent.Text = "Event";
            this.tabPageEvent.UseVisualStyleBackColor = true;
            // 
            // panelEvent
            // 
            this.panelEvent.BorderColor = System.Drawing.Color.Black;
            this.panelEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEvent.FillColorEnd = System.Drawing.Color.Empty;
            this.panelEvent.FillColorStart = System.Drawing.Color.Empty;
            this.panelEvent.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.panelEvent.FillStyle = Sheng.SailingEase.Controls.FillStyle.Solid;
            this.panelEvent.HighLight = false;
            this.panelEvent.Location = new System.Drawing.Point(0, 0);
            this.panelEvent.Name = "panelEvent";
            this.panelEvent.ShowBorder = false;
            this.panelEvent.Size = new System.Drawing.Size(432, 256);
            this.panelEvent.TabIndex = 6;
            // 
            // tabPageRemark
            // 
            this.tabPageRemark.Controls.Add(this.txtRemark);
            this.tabPageRemark.Location = new System.Drawing.Point(4, 21);
            this.tabPageRemark.Name = "tabPageRemark";
            this.tabPageRemark.Padding = new System.Windows.Forms.Padding(5);
            this.tabPageRemark.Size = new System.Drawing.Size(432, 256);
            this.tabPageRemark.TabIndex = 1;
            this.tabPageRemark.Text = "Remark";
            this.tabPageRemark.UseVisualStyleBackColor = true;
            // 
            // txtRemark
            // 
            this.txtRemark.AllowEmpty = true;
            this.txtRemark.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRemark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemark.HighLight = true;
            this.txtRemark.LimitMaxValue = false;
            this.txtRemark.Location = new System.Drawing.Point(5, 5);
            this.txtRemark.MaxValue = ((long)(2147483647));
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Regex = "";
            this.txtRemark.RegexMsg = null;
            this.txtRemark.Size = new System.Drawing.Size(422, 246);
            this.txtRemark.TabIndex = 1;
            this.txtRemark.Title = null;
            this.txtRemark.ValueCompareTo = null;
            this.txtRemark.WaterText = "";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(375, 299);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "${MenuEditView_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(294, 299);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "${MenuEditView_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FormMainMenuAdd
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(456, 334);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMainMenuAdd";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${MenuEditView}";
            this.Load += new System.EventHandler(this.MenuEditView_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.tabPageEvent.ResumeLayout(false);
            this.tabPageRemark.ResumeLayout(false);
            this.tabPageRemark.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Sheng.SailingEase.Controls.SETabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageRemark;
        private Sheng.SailingEase.Controls.SETextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblCode;
        private Sheng.SailingEase.Controls.SETextBox txtCode;
        private Sheng.SailingEase.Controls.SETextBox txtRemark;
        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private System.Windows.Forms.TabPage tabPageEvent;
        private Sheng.SailingEase.Controls.SEPanel panelEvent;
        private System.Windows.Forms.Label label1;
        private Sheng.SailingEase.Controls.SETextBox txtText;
    }
}