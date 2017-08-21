namespace Sheng.SailingEase.Core.Development
{
    partial class FormFormChoose
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.ToolStripSystemRenderer toolStripSystemRenderer1 = new System.Windows.Forms.ToolStripSystemRenderer();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.dataGridViewForms = new Sheng.SailingEase.Controls.SEDataGridView();
            this.folderAddressBar = new Sheng.SailingEase.Controls.SEAdressBar.SEAddressBar();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewForms)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(432, 349);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "${FormFormChoose_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(351, 349);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "${FormFormChoose_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // dataGridViewForms
            // 
            this.dataGridViewForms.AllowUserToAddRows = false;
            this.dataGridViewForms.AllowUserToDeleteRows = false;
            this.dataGridViewForms.AllowUserToResizeRows = false;
            this.dataGridViewForms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewForms.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewForms.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewForms.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewForms.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewForms.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewForms.ColumnHeadersHeight = 21;
            this.dataGridViewForms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewForms.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.ColumnCode});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewForms.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewForms.Location = new System.Drawing.Point(12, 50);
            this.dataGridViewForms.MultiSelect = false;
            this.dataGridViewForms.Name = "dataGridViewForms";
            this.dataGridViewForms.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewForms.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewForms.RowHeadersVisible = false;
            this.dataGridViewForms.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewForms.RowTemplate.Height = 21;
            this.dataGridViewForms.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewForms.Size = new System.Drawing.Size(495, 293);
            this.dataGridViewForms.StandardTab = true;
            this.dataGridViewForms.TabIndex = 18;
            this.dataGridViewForms.WaterText = "";
            this.dataGridViewForms.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewForms_CellMouseDoubleClick);
            // 
            // folderAddressBar
            // 
            this.folderAddressBar.CurrentNode = null;
            this.folderAddressBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.folderAddressBar.DropDownRenderer = null;
            this.folderAddressBar.Location = new System.Drawing.Point(0, 0);
            this.folderAddressBar.Name = "folderAddressBar";
            this.folderAddressBar.Renderer = toolStripSystemRenderer1;
            this.folderAddressBar.Size = new System.Drawing.Size(519, 25);
            this.folderAddressBar.TabIndex = 20;
            this.folderAddressBar.SelectionChange += new Sheng.SailingEase.Controls.SEAdressBar.SEAddressBarStrip.SelectionChanged(this.folderAddressBar_SelectionChange);
            // 
            // ColumnName
            // 
            this.ColumnName.DataPropertyName = "Name";
            this.ColumnName.HeaderText = "${FormFormChoose_ColumnName}";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            this.ColumnName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnName.Width = 180;
            // 
            // ColumnCode
            // 
            this.ColumnCode.DataPropertyName = "Code";
            this.ColumnCode.HeaderText = "${FormFormChoose_ColumnCode}";
            this.ColumnCode.Name = "ColumnCode";
            this.ColumnCode.ReadOnly = true;
            this.ColumnCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnCode.Width = 180;
            // 
            // FormFormChoose
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(519, 384);
            this.Controls.Add(this.folderAddressBar);
            this.Controls.Add(this.dataGridViewForms);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFormChoose";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${FormFormChoose}";
            this.Load += new System.EventHandler(this.FormFormChoose_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewForms)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private Sheng.SailingEase.Controls.SEDataGridView dataGridViewForms;
        private Sheng.SailingEase.Controls.SEAdressBar.SEAddressBar folderAddressBar;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCode;
    }
}