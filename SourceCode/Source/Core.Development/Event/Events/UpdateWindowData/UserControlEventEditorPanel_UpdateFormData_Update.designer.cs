namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlEventEditorPanel_UpdateFormData_Update
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
            this.btnUpdateAll = new Sheng.SailingEase.Controls.SEButton();
            this.btnDeleteUpdate = new Sheng.SailingEase.Controls.SEButton();
            this.btnAddUpdate = new Sheng.SailingEase.Controls.SEButton();
            this.dataGridViewUpdateData = new System.Windows.Forms.DataGridView();
            this.lblUpdateData = new System.Windows.Forms.Label();
            this.lblDataEntity = new System.Windows.Forms.Label();
            this.btnBrowseDataEntity = new Sheng.SailingEase.Controls.SEButton();
            this.txtDataEntityName = new Sheng.SailingEase.Controls.SETextBox();
            this.ColumnUpdateDataDataItemEntityName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnUpdateDataSourceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUpdateData)).BeginInit();
            this.SuspendLayout();
            // 
            // btnUpdateAll
            // 
            this.btnUpdateAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUpdateAll.Location = new System.Drawing.Point(77, 274);
            this.btnUpdateAll.Name = "btnUpdateAll";
            this.btnUpdateAll.Size = new System.Drawing.Size(68, 23);
            this.btnUpdateAll.TabIndex = 5;
            this.btnUpdateAll.Text = "${UserControlEventEditorPanel_UpdateFormData_Update_ButtonUpdateAll}";
            this.btnUpdateAll.UseVisualStyleBackColor = true;
            this.btnUpdateAll.Click += new System.EventHandler(this.btnUpdateAll_Click);
            // 
            // btnDeleteUpdate
            // 
            this.btnDeleteUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteUpdate.Location = new System.Drawing.Point(40, 274);
            this.btnDeleteUpdate.Name = "btnDeleteUpdate";
            this.btnDeleteUpdate.Size = new System.Drawing.Size(31, 23);
            this.btnDeleteUpdate.TabIndex = 4;
            this.btnDeleteUpdate.Text = "-";
            this.btnDeleteUpdate.UseVisualStyleBackColor = true;
            this.btnDeleteUpdate.Click += new System.EventHandler(this.btnDeleteUpdate_Click);
            // 
            // btnAddUpdate
            // 
            this.btnAddUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddUpdate.Location = new System.Drawing.Point(3, 274);
            this.btnAddUpdate.Name = "btnAddUpdate";
            this.btnAddUpdate.Size = new System.Drawing.Size(31, 23);
            this.btnAddUpdate.TabIndex = 3;
            this.btnAddUpdate.Text = "+";
            this.btnAddUpdate.UseVisualStyleBackColor = true;
            this.btnAddUpdate.Click += new System.EventHandler(this.btnAddUpdate_Click);
            // 
            // dataGridViewUpdateData
            // 
            this.dataGridViewUpdateData.AllowUserToAddRows = false;
            this.dataGridViewUpdateData.AllowUserToDeleteRows = false;
            this.dataGridViewUpdateData.AllowUserToResizeRows = false;
            this.dataGridViewUpdateData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewUpdateData.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewUpdateData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewUpdateData.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewUpdateData.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewUpdateData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewUpdateData.ColumnHeadersHeight = 21;
            this.dataGridViewUpdateData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewUpdateData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnUpdateDataDataItemEntityName,
            this.ColumnUpdateDataSourceName});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewUpdateData.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewUpdateData.Location = new System.Drawing.Point(6, 84);
            this.dataGridViewUpdateData.MultiSelect = false;
            this.dataGridViewUpdateData.Name = "dataGridViewUpdateData";
            this.dataGridViewUpdateData.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewUpdateData.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewUpdateData.RowHeadersVisible = false;
            this.dataGridViewUpdateData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewUpdateData.RowTemplate.Height = 21;
            this.dataGridViewUpdateData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewUpdateData.Size = new System.Drawing.Size(451, 184);
            this.dataGridViewUpdateData.StandardTab = true;
            this.dataGridViewUpdateData.TabIndex = 2;
            this.dataGridViewUpdateData.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridViewUpdateData_DataBindingComplete);
            // 
            // lblUpdateData
            // 
            this.lblUpdateData.AutoSize = true;
            this.lblUpdateData.Location = new System.Drawing.Point(4, 69);
            this.lblUpdateData.Name = "lblUpdateData";
            this.lblUpdateData.Size = new System.Drawing.Size(419, 12);
            this.lblUpdateData.TabIndex = 43;
            this.lblUpdateData.Text = "${UserControlEventEditorPanel_UpdateFormData_Update_LabelUpdateData}:";
            // 
            // lblDataEntity
            // 
            this.lblDataEntity.AutoSize = true;
            this.lblDataEntity.Location = new System.Drawing.Point(4, 38);
            this.lblDataEntity.Name = "lblDataEntity";
            this.lblDataEntity.Size = new System.Drawing.Size(419, 12);
            this.lblDataEntity.TabIndex = 42;
            this.lblDataEntity.Text = "${UserControlEventEditorPanel_UpdateFormData_Update_LabelDataEntity}:";
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
            this.txtDataEntityName.Location = new System.Drawing.Point(89, 35);
            this.txtDataEntityName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.txtDataEntityName.MaxValue = ((long)(2147483647));
            this.txtDataEntityName.Name = "txtDataEntityName";
            this.txtDataEntityName.ReadOnly = true;
            this.txtDataEntityName.Regex = "";
            this.txtDataEntityName.RegexMsg = null;
            this.txtDataEntityName.Size = new System.Drawing.Size(331, 21);
            this.txtDataEntityName.TabIndex = 0;
            this.txtDataEntityName.Title = "LabelDataEntity";
            this.txtDataEntityName.ValueCompareTo = null;
            this.txtDataEntityName.WaterText = "";
            // 
            // ColumnUpdateDataDataItemEntityName
            // 
            this.ColumnUpdateDataDataItemEntityName.DataPropertyName = "DataItemName";
            this.ColumnUpdateDataDataItemEntityName.HeaderText = "${UserControlEventEditorPanel_UpdateFormData_Update_ColumnUpdateDataDataItemEntit" +
                "yName}";
            this.ColumnUpdateDataDataItemEntityName.Name = "ColumnUpdateDataDataItemEntityName";
            this.ColumnUpdateDataDataItemEntityName.ReadOnly = true;
            this.ColumnUpdateDataDataItemEntityName.Width = 180;
            // 
            // ColumnUpdateDataSourceName
            // 
            this.ColumnUpdateDataSourceName.DataPropertyName = "SourceName";
            this.ColumnUpdateDataSourceName.HeaderText = "${UserControlEventEditorPanel_UpdateFormData_Update_ColumnUpdateDataSourceName}";
            this.ColumnUpdateDataSourceName.Name = "ColumnUpdateDataSourceName";
            this.ColumnUpdateDataSourceName.ReadOnly = true;
            this.ColumnUpdateDataSourceName.Width = 260;
            // 
            // UserControlEventEditorPanel_UpdateFormData_Update
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnUpdateAll);
            this.Controls.Add(this.btnDeleteUpdate);
            this.Controls.Add(this.btnAddUpdate);
            this.Controls.Add(this.dataGridViewUpdateData);
            this.Controls.Add(this.lblUpdateData);
            this.Controls.Add(this.txtDataEntityName);
            this.Controls.Add(this.btnBrowseDataEntity);
            this.Controls.Add(this.lblDataEntity);
            this.Name = "UserControlEventEditorPanel_UpdateFormData_Update";
            this.Controls.SetChildIndex(this.lblDataEntity, 0);
            this.Controls.SetChildIndex(this.btnBrowseDataEntity, 0);
            this.Controls.SetChildIndex(this.txtDataEntityName, 0);
            this.Controls.SetChildIndex(this.lblUpdateData, 0);
            this.Controls.SetChildIndex(this.dataGridViewUpdateData, 0);
            this.Controls.SetChildIndex(this.btnAddUpdate, 0);
            this.Controls.SetChildIndex(this.btnDeleteUpdate, 0);
            this.Controls.SetChildIndex(this.btnUpdateAll, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUpdateData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sheng.SailingEase.Controls.SEButton btnUpdateAll;
        private Sheng.SailingEase.Controls.SEButton btnDeleteUpdate;
        private Sheng.SailingEase.Controls.SEButton btnAddUpdate;
        private System.Windows.Forms.DataGridView dataGridViewUpdateData;
        private System.Windows.Forms.Label lblUpdateData;
        private System.Windows.Forms.Label lblDataEntity;
        private Sheng.SailingEase.Controls.SEButton btnBrowseDataEntity;
        private Sheng.SailingEase.Controls.SETextBox txtDataEntityName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnUpdateDataDataItemEntityName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnUpdateDataSourceName;
    }
}
