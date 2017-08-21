namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlEventEditorPanel_LoadDataToForm_Load
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnLoadAll = new Sheng.SailingEase.Controls.SEButton();
            this.btnDeleteLoad = new Sheng.SailingEase.Controls.SEButton();
            this.btnAddLoad = new Sheng.SailingEase.Controls.SEButton();
            this.dataGridViewLoadData = new System.Windows.Forms.DataGridView();
            this.lblLoadData = new System.Windows.Forms.Label();
            this.ColumnLoadDataDataItemEntityName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLoadDataLoadToName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLoadData)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoadAll
            // 
            this.btnLoadAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadAll.Location = new System.Drawing.Point(77, 274);
            this.btnLoadAll.Name = "btnLoadAll";
            this.btnLoadAll.Size = new System.Drawing.Size(68, 23);
            this.btnLoadAll.TabIndex = 3;
            this.btnLoadAll.Text = "${UserControlEventEditorPanel_LoadDataToForm_Load_ButtonLoadAll}";
            this.btnLoadAll.UseVisualStyleBackColor = true;
            this.btnLoadAll.Click += new System.EventHandler(this.btnLoadAll_Click);
            // 
            // btnDeleteLoad
            // 
            this.btnDeleteLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteLoad.Location = new System.Drawing.Point(40, 274);
            this.btnDeleteLoad.Name = "btnDeleteLoad";
            this.btnDeleteLoad.Size = new System.Drawing.Size(31, 23);
            this.btnDeleteLoad.TabIndex = 2;
            this.btnDeleteLoad.Text = "-";
            this.btnDeleteLoad.UseVisualStyleBackColor = true;
            this.btnDeleteLoad.Click += new System.EventHandler(this.btnDeleteLoad_Click);
            // 
            // btnAddLoad
            // 
            this.btnAddLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddLoad.Location = new System.Drawing.Point(3, 274);
            this.btnAddLoad.Name = "btnAddLoad";
            this.btnAddLoad.Size = new System.Drawing.Size(31, 23);
            this.btnAddLoad.TabIndex = 1;
            this.btnAddLoad.Text = "+";
            this.btnAddLoad.UseVisualStyleBackColor = true;
            this.btnAddLoad.Click += new System.EventHandler(this.btnAddLoad_Click);
            // 
            // dataGridViewLoadData
            // 
            this.dataGridViewLoadData.AllowUserToAddRows = false;
            this.dataGridViewLoadData.AllowUserToDeleteRows = false;
            this.dataGridViewLoadData.AllowUserToResizeRows = false;
            this.dataGridViewLoadData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewLoadData.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewLoadData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewLoadData.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewLoadData.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewLoadData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewLoadData.ColumnHeadersHeight = 21;
            this.dataGridViewLoadData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewLoadData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnLoadDataDataItemEntityName,
            this.ColumnLoadDataLoadToName});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewLoadData.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewLoadData.Location = new System.Drawing.Point(3, 50);
            this.dataGridViewLoadData.MultiSelect = false;
            this.dataGridViewLoadData.Name = "dataGridViewLoadData";
            this.dataGridViewLoadData.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewLoadData.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewLoadData.RowHeadersVisible = false;
            this.dataGridViewLoadData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewLoadData.RowTemplate.Height = 21;
            this.dataGridViewLoadData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewLoadData.Size = new System.Drawing.Size(454, 218);
            this.dataGridViewLoadData.StandardTab = true;
            this.dataGridViewLoadData.TabIndex = 0;
            this.dataGridViewLoadData.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridViewLoadData_DataBindingComplete);
            // 
            // lblLoadData
            // 
            this.lblLoadData.AutoSize = true;
            this.lblLoadData.Location = new System.Drawing.Point(3, 35);
            this.lblLoadData.Name = "lblLoadData";
            this.lblLoadData.Size = new System.Drawing.Size(395, 12);
            this.lblLoadData.TabIndex = 30;
            this.lblLoadData.Text = "${UserControlEventEditorPanel_LoadDataToForm_Load_LabelLoadData}:";
            // 
            // ColumnLoadDataDataItemEntityName
            // 
            this.ColumnLoadDataDataItemEntityName.DataPropertyName = "DataItemName";
            this.ColumnLoadDataDataItemEntityName.HeaderText = "${UserControlEventEditorPanel_LoadDataToForm_Load_ColumnLoadDataDataItemEntityNam" +
                "e}";
            this.ColumnLoadDataDataItemEntityName.Name = "ColumnLoadDataDataItemEntityName";
            this.ColumnLoadDataDataItemEntityName.ReadOnly = true;
            this.ColumnLoadDataDataItemEntityName.Width = 180;
            // 
            // ColumnLoadDataLoadToName
            // 
            this.ColumnLoadDataLoadToName.DataPropertyName = "SourceName";
            this.ColumnLoadDataLoadToName.HeaderText = "${UserControlEventEditorPanel_LoadDataToForm_Load_ColumnLoadDataLoadToName}";
            this.ColumnLoadDataLoadToName.Name = "ColumnLoadDataLoadToName";
            this.ColumnLoadDataLoadToName.ReadOnly = true;
            this.ColumnLoadDataLoadToName.Width = 260;
            // 
            // UserControlEventEditorPanel_LoadDataToForm_Load
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnLoadAll);
            this.Controls.Add(this.btnDeleteLoad);
            this.Controls.Add(this.btnAddLoad);
            this.Controls.Add(this.dataGridViewLoadData);
            this.Controls.Add(this.lblLoadData);
            this.Name = "UserControlEventEditorPanel_LoadDataToForm_Load";
            this.Controls.SetChildIndex(this.lblLoadData, 0);
            this.Controls.SetChildIndex(this.dataGridViewLoadData, 0);
            this.Controls.SetChildIndex(this.btnAddLoad, 0);
            this.Controls.SetChildIndex(this.btnDeleteLoad, 0);
            this.Controls.SetChildIndex(this.btnLoadAll, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLoadData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sheng.SailingEase.Controls.SEButton btnLoadAll;
        private Sheng.SailingEase.Controls.SEButton btnDeleteLoad;
        private Sheng.SailingEase.Controls.SEButton btnAddLoad;
        private System.Windows.Forms.DataGridView dataGridViewLoadData;
        private System.Windows.Forms.Label lblLoadData;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLoadDataDataItemEntityName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLoadDataLoadToName;
    }
}
