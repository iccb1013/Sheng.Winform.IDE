namespace Sheng.SailingEase.Windows.Forms.Development
{
    partial class UserControlSEComboBoxExDevDataRule_DataEntity
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlSEComboBoxExDevDataRule_DataEntity));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonEquals = new System.Windows.Forms.ToolStripButton();
            this.ddlValueDataItem = new Sheng.SailingEase.Controls.SEComboBox();
            this.lblValueDataItem = new System.Windows.Forms.Label();
            this.ddlTextDataItem = new Sheng.SailingEase.Controls.SEComboBox();
            this.lblTextDataItem = new System.Windows.Forms.Label();
            this.btnBrowseDataEntity = new Sheng.SailingEase.Controls.SEButton();
            this.txtDataEntityName = new Sheng.SailingEase.Controls.SETextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonEquals});
            this.toolStrip1.Location = new System.Drawing.Point(378, 93);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(26, 25);
            this.toolStrip1.TabIndex = 12;
            this.toolStrip1.TabStop = true;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonEquals
            // 
            this.toolStripButtonEquals.Checked = true;
            this.toolStripButtonEquals.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonEquals.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonEquals.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEquals.Image")));
            this.toolStripButtonEquals.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEquals.Name = "toolStripButtonEquals";
            this.toolStripButtonEquals.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonEquals.Text = "=";
            this.toolStripButtonEquals.Click += new System.EventHandler(this.toolStripButtonEquals_Click);
            // 
            // ddlValueDataItem
            // 
            this.ddlValueDataItem.AllowEmpty = false;
            this.ddlValueDataItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlValueDataItem.DisplayMember = "Name";
            this.ddlValueDataItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlValueDataItem.Enabled = false;
            this.ddlValueDataItem.FormattingEnabled = true;
            this.ddlValueDataItem.HighLight = true;
            this.ddlValueDataItem.Location = new System.Drawing.Point(5, 98);
            this.ddlValueDataItem.Name = "ddlValueDataItem";
            this.ddlValueDataItem.Size = new System.Drawing.Size(367, 20);
            this.ddlValueDataItem.TabIndex = 10;
            this.ddlValueDataItem.Title = "LabelValueDataItem";
            this.ddlValueDataItem.ValueMember = "Id";
            this.ddlValueDataItem.WaterText = "";
            // 
            // lblValueDataItem
            // 
            this.lblValueDataItem.AutoSize = true;
            this.lblValueDataItem.Location = new System.Drawing.Point(3, 83);
            this.lblValueDataItem.Name = "lblValueDataItem";
            this.lblValueDataItem.Size = new System.Drawing.Size(413, 12);
            this.lblValueDataItem.TabIndex = 13;
            this.lblValueDataItem.Text = "${UserControlSEComboBoxExDevDataRule_DataEntity_LabelValueDataItem}:";
            // 
            // ddlTextDataItem
            // 
            this.ddlTextDataItem.AllowEmpty = false;
            this.ddlTextDataItem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ddlTextDataItem.DisplayMember = "Name";
            this.ddlTextDataItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlTextDataItem.FormattingEnabled = true;
            this.ddlTextDataItem.HighLight = true;
            this.ddlTextDataItem.Location = new System.Drawing.Point(5, 48);
            this.ddlTextDataItem.Name = "ddlTextDataItem";
            this.ddlTextDataItem.Size = new System.Drawing.Size(367, 20);
            this.ddlTextDataItem.TabIndex = 9;
            this.ddlTextDataItem.Title = "LabelTextDataItem";
            this.ddlTextDataItem.ValueMember = "Id";
            this.ddlTextDataItem.WaterText = "";
            this.ddlTextDataItem.SelectedIndexChanged += new System.EventHandler(this.ddlTextDataItem_SelectedIndexChanged);
            // 
            // lblTextDataItem
            // 
            this.lblTextDataItem.AutoSize = true;
            this.lblTextDataItem.Location = new System.Drawing.Point(3, 33);
            this.lblTextDataItem.Name = "lblTextDataItem";
            this.lblTextDataItem.Size = new System.Drawing.Size(407, 12);
            this.lblTextDataItem.TabIndex = 11;
            this.lblTextDataItem.Text = "${UserControlSEComboBoxExDevDataRule_DataEntity_LabelTextDataItem}:";
            // 
            // btnBrowseDataEntity
            // 
            this.btnBrowseDataEntity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseDataEntity.Location = new System.Drawing.Point(378, 1);
            this.btnBrowseDataEntity.Name = "btnBrowseDataEntity";
            this.btnBrowseDataEntity.Size = new System.Drawing.Size(31, 23);
            this.btnBrowseDataEntity.TabIndex = 8;
            this.btnBrowseDataEntity.Text = "...";
            this.btnBrowseDataEntity.UseVisualStyleBackColor = true;
            this.btnBrowseDataEntity.Click += new System.EventHandler(this.btnBrowseDataEntity_Click);
            // 
            // txtDataEntityName
            // 
            this.txtDataEntityName.AllowEmpty = false;
            this.txtDataEntityName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDataEntityName.HighLight = true;
            this.txtDataEntityName.LimitMaxValue = false;
            this.txtDataEntityName.Location = new System.Drawing.Point(3, 3);
            this.txtDataEntityName.MaxValue = ((long)(2147483647));
            this.txtDataEntityName.Name = "txtDataEntityName";
            this.txtDataEntityName.ReadOnly = true;
            this.txtDataEntityName.Regex = "";
            this.txtDataEntityName.RegexMsg = null;
            this.txtDataEntityName.Size = new System.Drawing.Size(369, 21);
            this.txtDataEntityName.TabIndex = 7;
            this.txtDataEntityName.Title = "TextBoxDataEntityNameTitle";
            this.txtDataEntityName.ValueCompareTo = null;
            this.txtDataEntityName.WaterText = "";
            // 
            // UserControlSEComboBoxExDevDataRule_DataEntity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.ddlValueDataItem);
            this.Controls.Add(this.lblValueDataItem);
            this.Controls.Add(this.ddlTextDataItem);
            this.Controls.Add(this.lblTextDataItem);
            this.Controls.Add(this.btnBrowseDataEntity);
            this.Controls.Add(this.txtDataEntityName);
            this.Name = "UserControlSEComboBoxExDevDataRule_DataEntity";
            this.Size = new System.Drawing.Size(412, 226);
            this.Load += new System.EventHandler(this.SEComboBoxExDevDataRule_DataEntity_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonEquals;
        private Sheng.SailingEase.Controls.SEComboBox ddlValueDataItem;
        private System.Windows.Forms.Label lblValueDataItem;
        private Sheng.SailingEase.Controls.SEComboBox ddlTextDataItem;
        private System.Windows.Forms.Label lblTextDataItem;
        private Sheng.SailingEase.Controls.SEButton btnBrowseDataEntity;
        private Sheng.SailingEase.Controls.SETextBox txtDataEntityName;
    }
}
