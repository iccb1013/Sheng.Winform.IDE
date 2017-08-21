namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    partial class ToolStripItemEditView
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
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.ddlControlType = new Sheng.SailingEase.Controls.SEComboBox();
            this.lblControlType = new System.Windows.Forms.Label();
            this.txtCode = new Sheng.SailingEase.Controls.SETextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblCode = new System.Windows.Forms.Label();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.txtName = new Sheng.SailingEase.Controls.SETextBox();
            this.tabPageDetail = new System.Windows.Forms.TabPage();
            this.propertyGrid = new Sheng.SailingEase.ComponentModel.Design.PropertyGridPad();
            this.tabPageEvent = new System.Windows.Forms.TabPage();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.tabControl1 = new Sheng.SailingEase.Controls.SETabControl();
            this.tabPageGeneral.SuspendLayout();
            this.tabPageDetail.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(332, 375);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "${ToolStripItemEditView_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ddlControlType
            // 
            this.ddlControlType.AllowEmpty = false;
            this.ddlControlType.CustomValidate = null;
            this.ddlControlType.DisplayMember = "Name";
            this.ddlControlType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlControlType.FormattingEnabled = true;
            this.ddlControlType.HighLight = true;
            this.ddlControlType.Location = new System.Drawing.Point(100, 73);
            this.ddlControlType.Name = "ddlControlType";
            this.ddlControlType.Size = new System.Drawing.Size(212, 20);
            this.ddlControlType.TabIndex = 9;
            this.ddlControlType.Title = "LabelControlType";
            this.ddlControlType.WaterText = "";
            // 
            // lblControlType
            // 
            this.lblControlType.AutoSize = true;
            this.lblControlType.Location = new System.Drawing.Point(14, 76);
            this.lblControlType.Name = "lblControlType";
            this.lblControlType.Size = new System.Drawing.Size(257, 12);
            this.lblControlType.TabIndex = 8;
            this.lblControlType.Text = "${ToolStripItemEditView_LabelControlType}:";
            // 
            // txtCode
            // 
            this.txtCode.AllowEmpty = false;
            this.txtCode.CustomValidate = null;
            this.txtCode.HighLight = true;
            this.txtCode.LimitMaxValue = false;
            this.txtCode.Location = new System.Drawing.Point(100, 46);
            this.txtCode.MaxLength = 100;
            this.txtCode.MaxValue = ((long)(2147483647));
            this.txtCode.Name = "txtCode";
            this.txtCode.Regex = "";
            this.txtCode.RegexMsg = "";
            this.txtCode.Size = new System.Drawing.Size(212, 21);
            this.txtCode.TabIndex = 7;
            this.txtCode.Title = "";
            this.txtCode.ValueCompareTo = null;
            this.txtCode.WaterText = "";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(14, 22);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(215, 12);
            this.lblName.TabIndex = 4;
            this.lblName.Text = "${ToolStripItemEditView_LabelName}:";
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(14, 49);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(215, 12);
            this.lblCode.TabIndex = 6;
            this.lblCode.Text = "${ToolStripItemEditView_LabelCode}:";
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.lblControlType);
            this.tabPageGeneral.Controls.Add(this.lblName);
            this.tabPageGeneral.Controls.Add(this.lblCode);
            this.tabPageGeneral.Controls.Add(this.ddlControlType);
            this.tabPageGeneral.Controls.Add(this.txtCode);
            this.tabPageGeneral.Controls.Add(this.txtName);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 21);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(467, 326);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // txtName
            // 
            this.txtName.AllowEmpty = false;
            this.txtName.CustomValidate = null;
            this.txtName.HighLight = true;
            this.txtName.LimitMaxValue = false;
            this.txtName.Location = new System.Drawing.Point(100, 19);
            this.txtName.MaxLength = 100;
            this.txtName.MaxValue = ((long)(2147483647));
            this.txtName.Name = "txtName";
            this.txtName.Regex = "";
            this.txtName.RegexMsg = null;
            this.txtName.Size = new System.Drawing.Size(212, 21);
            this.txtName.TabIndex = 5;
            this.txtName.Title = "";
            this.txtName.ValueCompareTo = null;
            this.txtName.WaterText = "";
            // 
            // tabPageDetail
            // 
            this.tabPageDetail.Controls.Add(this.propertyGrid);
            this.tabPageDetail.Location = new System.Drawing.Point(4, 21);
            this.tabPageDetail.Name = "tabPageDetail";
            this.tabPageDetail.Size = new System.Drawing.Size(467, 326);
            this.tabPageDetail.TabIndex = 2;
            this.tabPageDetail.Text = "Detail";
            this.tabPageDetail.UseVisualStyleBackColor = true;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.ReadOnly = false;
            this.propertyGrid.SelectedObjects = null;
            this.propertyGrid.ShowDescription = false;
            this.propertyGrid.Size = new System.Drawing.Size(467, 326);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.Verbs = null;
            // 
            // tabPageEvent
            // 
            this.tabPageEvent.Location = new System.Drawing.Point(4, 21);
            this.tabPageEvent.Name = "tabPageEvent";
            this.tabPageEvent.Size = new System.Drawing.Size(467, 326);
            this.tabPageEvent.TabIndex = 4;
            this.tabPageEvent.Text = "Event";
            this.tabPageEvent.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(413, 375);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "${ToolStripItemEditView_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageGeneral);
            this.tabControl1.Controls.Add(this.tabPageDetail);
            this.tabControl1.Controls.Add(this.tabPageEvent);
            this.tabControl1.CustomValidate = null;
            this.tabControl1.HighLight = false;
            this.tabControl1.Location = new System.Drawing.Point(13, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(475, 351);
            this.tabControl1.TabIndex = 6;
            this.tabControl1.Title = null;
            // 
            // ToolStripItemEditView
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(500, 410);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ToolStripItemEditView";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${ToolStripItemEditView}";
            this.Load += new System.EventHandler(this.ToolStripItemEditView_Load);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.tabPageDetail.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sheng.SailingEase.Controls.SEButton btnOK;
        private Sheng.SailingEase.Controls.SEComboBox ddlControlType;
        private System.Windows.Forms.Label lblControlType;
        private Sheng.SailingEase.Controls.SETextBox txtCode;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private Sheng.SailingEase.Controls.SETextBox txtName;
        private System.Windows.Forms.TabPage tabPageDetail;
        private System.Windows.Forms.TabPage tabPageEvent;
        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private Sheng.SailingEase.Controls.SETabControl tabControl1;
        private Sheng.SailingEase.ComponentModel.Design.PropertyGridPad propertyGrid;
    }
}