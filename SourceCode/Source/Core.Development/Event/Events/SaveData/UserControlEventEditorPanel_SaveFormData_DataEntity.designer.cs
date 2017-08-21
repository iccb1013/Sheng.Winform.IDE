namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlEventEditorPanel_SaveFormData_DataEntity
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
            this.btnAll = new Sheng.SailingEase.Controls.SEButton();
            this.btnDelete = new Sheng.SailingEase.Controls.SEButton();
            this.btnAdd = new Sheng.SailingEase.Controls.SEButton();
            this.dataGridViewSaveData = new System.Windows.Forms.DataGridView();
            this.lblSaveData = new System.Windows.Forms.Label();
            this.lblDataEntity = new System.Windows.Forms.Label();
            this.btnBrowseDataEntity = new Sheng.SailingEase.Controls.SEButton();
            this.txtDataEntityName = new Sheng.SailingEase.Controls.SETextBox();
            this.ColumnDataItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDataSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSaveData)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAll
            // 
            this.btnAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAll.Location = new System.Drawing.Point(77, 274);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(68, 23);
            this.btnAll.TabIndex = 5;
            this.btnAll.Text = "${UserControlEventEditorPanel_SaveFormData_DataEntity_ButtonAll}";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(40, 274);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(31, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "-";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(3, 274);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(31, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dataGridViewSaveData
            // 
            this.dataGridViewSaveData.AllowUserToAddRows = false;
            this.dataGridViewSaveData.AllowUserToDeleteRows = false;
            this.dataGridViewSaveData.AllowUserToResizeRows = false;
            this.dataGridViewSaveData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewSaveData.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewSaveData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewSaveData.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewSaveData.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewSaveData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewSaveData.ColumnHeadersHeight = 21;
            this.dataGridViewSaveData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewSaveData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnDataItemName,
            this.ColumnDataSource});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewSaveData.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewSaveData.Location = new System.Drawing.Point(3, 83);
            this.dataGridViewSaveData.MultiSelect = false;
            this.dataGridViewSaveData.Name = "dataGridViewSaveData";
            this.dataGridViewSaveData.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewSaveData.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewSaveData.RowHeadersVisible = false;
            this.dataGridViewSaveData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewSaveData.RowTemplate.Height = 21;
            this.dataGridViewSaveData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewSaveData.Size = new System.Drawing.Size(454, 185);
            this.dataGridViewSaveData.StandardTab = true;
            this.dataGridViewSaveData.TabIndex = 2;
            this.dataGridViewSaveData.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridViewSaveData_DataBindingComplete);
            // 
            // lblSaveData
            // 
            this.lblSaveData.AutoSize = true;
            this.lblSaveData.Location = new System.Drawing.Point(1, 65);
            this.lblSaveData.Name = "lblSaveData";
            this.lblSaveData.Size = new System.Drawing.Size(419, 12);
            this.lblSaveData.TabIndex = 33;
            this.lblSaveData.Text = "${UserControlEventEditorPanel_SaveFormData_DataEntity_LabelSaveData}:";
            // 
            // lblDataEntity
            // 
            this.lblDataEntity.AutoSize = true;
            this.lblDataEntity.Location = new System.Drawing.Point(1, 38);
            this.lblDataEntity.Name = "lblDataEntity";
            this.lblDataEntity.Size = new System.Drawing.Size(431, 12);
            this.lblDataEntity.TabIndex = 32;
            this.lblDataEntity.Text = "${UserControlEventEditorPanel_SaveFormData_DataEntity_LabelDataEntity}:";
            // 
            // btnBrowseDataEntity
            // 
            this.btnBrowseDataEntity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseDataEntity.Location = new System.Drawing.Point(426, 33);
            this.btnBrowseDataEntity.Name = "btnBrowseDataEntity";
            this.btnBrowseDataEntity.Size = new System.Drawing.Size(31, 23);
            this.btnBrowseDataEntity.TabIndex = 1;
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
            this.txtDataEntityName.Location = new System.Drawing.Point(90, 35);
            this.txtDataEntityName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.txtDataEntityName.MaxValue = ((long)(2147483647));
            this.txtDataEntityName.Name = "txtDataEntityName";
            this.txtDataEntityName.ReadOnly = true;
            this.txtDataEntityName.Regex = "";
            this.txtDataEntityName.RegexMsg = null;
            this.txtDataEntityName.Size = new System.Drawing.Size(330, 21);
            this.txtDataEntityName.TabIndex = 0;
            this.txtDataEntityName.Title = "LabelDataEntity";
            this.txtDataEntityName.ValueCompareTo = null;
            this.txtDataEntityName.WaterText = "";
            // 
            // ColumnDataItemName
            // 
            this.ColumnDataItemName.DataPropertyName = "DataItemName";
            this.ColumnDataItemName.HeaderText = "${UserControlEventEditorPanel_SaveFormData_DataEntity_ColumnDataItemName}";
            this.ColumnDataItemName.Name = "ColumnDataItemName";
            this.ColumnDataItemName.ReadOnly = true;
            this.ColumnDataItemName.Width = 180;
            // 
            // ColumnDataSource
            // 
            this.ColumnDataSource.DataPropertyName = "SourceName";
            this.ColumnDataSource.HeaderText = "${UserControlEventEditorPanel_SaveFormData_DataEntity_ColumnDataSource}";
            this.ColumnDataSource.Name = "ColumnDataSource";
            this.ColumnDataSource.ReadOnly = true;
            this.ColumnDataSource.Width = 260;
            // 
            // UserControlEventEditorPanel_SaveFormData_DataEntity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dataGridViewSaveData);
            this.Controls.Add(this.lblSaveData);
            this.Controls.Add(this.txtDataEntityName);
            this.Controls.Add(this.btnBrowseDataEntity);
            this.Controls.Add(this.lblDataEntity);
            this.Name = "UserControlEventEditorPanel_SaveFormData_DataEntity";
            this.Controls.SetChildIndex(this.lblDataEntity, 0);
            this.Controls.SetChildIndex(this.btnBrowseDataEntity, 0);
            this.Controls.SetChildIndex(this.txtDataEntityName, 0);
            this.Controls.SetChildIndex(this.lblSaveData, 0);
            this.Controls.SetChildIndex(this.dataGridViewSaveData, 0);
            this.Controls.SetChildIndex(this.btnAdd, 0);
            this.Controls.SetChildIndex(this.btnDelete, 0);
            this.Controls.SetChildIndex(this.btnAll, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSaveData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sheng.SailingEase.Controls.SEButton btnAll;
        private Sheng.SailingEase.Controls.SEButton btnDelete;
        private Sheng.SailingEase.Controls.SEButton btnAdd;
        private System.Windows.Forms.DataGridView dataGridViewSaveData;
        private System.Windows.Forms.Label lblSaveData;
        private System.Windows.Forms.Label lblDataEntity;
        private Sheng.SailingEase.Controls.SEButton btnBrowseDataEntity;
        private Sheng.SailingEase.Controls.SETextBox txtDataEntityName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDataItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDataSource;
    }
}
