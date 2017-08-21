namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlEventEditorPanel_LoadDataToForm_DataEntity
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
            this.lblDataEntity = new System.Windows.Forms.Label();
            this.btnBrowseDataEntity = new Sheng.SailingEase.Controls.SEButton();
            this.txtDataEntityName = new Sheng.SailingEase.Controls.SETextBox();
            this.btnDeleteWhere = new Sheng.SailingEase.Controls.SEButton();
            this.btnAddWhere = new Sheng.SailingEase.Controls.SEButton();
            this.dataGridViewWhere = new System.Windows.Forms.DataGridView();
            this.lblWhere = new System.Windows.Forms.Label();
            this.ColumnWhereDataItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnWhereDataSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnWhereMatchType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWhere)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDataEntity
            // 
            this.lblDataEntity.AutoSize = true;
            this.lblDataEntity.Location = new System.Drawing.Point(4, 32);
            this.lblDataEntity.Name = "lblDataEntity";
            this.lblDataEntity.Size = new System.Drawing.Size(443, 12);
            this.lblDataEntity.TabIndex = 36;
            this.lblDataEntity.Text = "${UserControlEventEditorPanel_LoadDataToForm_DataEntity_LabelDataEntity}:";
            // 
            // btnBrowseDataEntity
            // 
            this.btnBrowseDataEntity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseDataEntity.Location = new System.Drawing.Point(426, 27);
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
            this.txtDataEntityName.Location = new System.Drawing.Point(90, 29);
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
            // btnDeleteWhere
            // 
            this.btnDeleteWhere.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteWhere.Location = new System.Drawing.Point(40, 274);
            this.btnDeleteWhere.Name = "btnDeleteWhere";
            this.btnDeleteWhere.Size = new System.Drawing.Size(31, 23);
            this.btnDeleteWhere.TabIndex = 4;
            this.btnDeleteWhere.Text = "-";
            this.btnDeleteWhere.UseVisualStyleBackColor = true;
            this.btnDeleteWhere.Click += new System.EventHandler(this.btnDeleteWhere_Click);
            // 
            // btnAddWhere
            // 
            this.btnAddWhere.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddWhere.Location = new System.Drawing.Point(3, 274);
            this.btnAddWhere.Name = "btnAddWhere";
            this.btnAddWhere.Size = new System.Drawing.Size(31, 23);
            this.btnAddWhere.TabIndex = 3;
            this.btnAddWhere.Text = "+";
            this.btnAddWhere.UseVisualStyleBackColor = true;
            this.btnAddWhere.Click += new System.EventHandler(this.btnAddWhere_Click);
            // 
            // dataGridViewWhere
            // 
            this.dataGridViewWhere.AllowUserToAddRows = false;
            this.dataGridViewWhere.AllowUserToDeleteRows = false;
            this.dataGridViewWhere.AllowUserToResizeRows = false;
            this.dataGridViewWhere.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewWhere.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewWhere.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewWhere.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewWhere.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewWhere.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewWhere.ColumnHeadersHeight = 21;
            this.dataGridViewWhere.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewWhere.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnWhereDataItemName,
            this.ColumnWhereDataSource,
            this.ColumnWhereMatchType});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewWhere.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewWhere.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewWhere.Location = new System.Drawing.Point(3, 74);
            this.dataGridViewWhere.MultiSelect = false;
            this.dataGridViewWhere.Name = "dataGridViewWhere";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewWhere.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewWhere.RowHeadersVisible = false;
            this.dataGridViewWhere.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewWhere.RowTemplate.Height = 21;
            this.dataGridViewWhere.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewWhere.Size = new System.Drawing.Size(454, 194);
            this.dataGridViewWhere.StandardTab = true;
            this.dataGridViewWhere.TabIndex = 2;
            this.dataGridViewWhere.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridViewWhere_DataBindingComplete);
            // 
            // lblWhere
            // 
            this.lblWhere.AutoSize = true;
            this.lblWhere.Location = new System.Drawing.Point(4, 59);
            this.lblWhere.Name = "lblWhere";
            this.lblWhere.Size = new System.Drawing.Size(413, 12);
            this.lblWhere.TabIndex = 30;
            this.lblWhere.Text = "${UserControlEventEditorPanel_LoadDataToForm_DataEntity_LabelWhere}:";
            // 
            // ColumnWhereDataItemName
            // 
            this.ColumnWhereDataItemName.DataPropertyName = "DataItemName";
            this.ColumnWhereDataItemName.HeaderText = "${UserControlEventEditorPanel_LoadDataToForm_DataEntity_ColumnWhereDataItemName}";
            this.ColumnWhereDataItemName.Name = "ColumnWhereDataItemName";
            this.ColumnWhereDataItemName.ReadOnly = true;
            this.ColumnWhereDataItemName.Width = 140;
            // 
            // ColumnWhereDataSource
            // 
            this.ColumnWhereDataSource.DataPropertyName = "SourceName";
            this.ColumnWhereDataSource.HeaderText = "${UserControlEventEditorPanel_LoadDataToForm_DataEntity_ColumnWhereDataSource}";
            this.ColumnWhereDataSource.Name = "ColumnWhereDataSource";
            this.ColumnWhereDataSource.ReadOnly = true;
            this.ColumnWhereDataSource.Width = 140;
            // 
            // ColumnWhereMatchType
            // 
            this.ColumnWhereMatchType.DataPropertyName = "MatchType";
            this.ColumnWhereMatchType.HeaderText = "${UserControlEventEditorPanel_LoadDataToForm_DataEntity_ColumnWhereMatchType}";
            this.ColumnWhereMatchType.Name = "ColumnWhereMatchType";
            this.ColumnWhereMatchType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnWhereMatchType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // UserControlEventEditorPanel_LoadDataToForm_DataEntity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtDataEntityName);
            this.Controls.Add(this.btnBrowseDataEntity);
            this.Controls.Add(this.btnDeleteWhere);
            this.Controls.Add(this.btnAddWhere);
            this.Controls.Add(this.dataGridViewWhere);
            this.Controls.Add(this.lblWhere);
            this.Controls.Add(this.lblDataEntity);
            this.Name = "UserControlEventEditorPanel_LoadDataToForm_DataEntity";
            this.Controls.SetChildIndex(this.lblDataEntity, 0);
            this.Controls.SetChildIndex(this.lblWhere, 0);
            this.Controls.SetChildIndex(this.dataGridViewWhere, 0);
            this.Controls.SetChildIndex(this.btnAddWhere, 0);
            this.Controls.SetChildIndex(this.btnDeleteWhere, 0);
            this.Controls.SetChildIndex(this.btnBrowseDataEntity, 0);
            this.Controls.SetChildIndex(this.txtDataEntityName, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWhere)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDataEntity;
        private Sheng.SailingEase.Controls.SEButton btnBrowseDataEntity;
        private Sheng.SailingEase.Controls.SETextBox txtDataEntityName;
        private Sheng.SailingEase.Controls.SEButton btnDeleteWhere;
        private Sheng.SailingEase.Controls.SEButton btnAddWhere;
        private System.Windows.Forms.DataGridView dataGridViewWhere;
        private System.Windows.Forms.Label lblWhere;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnWhereDataItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnWhereDataSource;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnWhereMatchType;
    }
}
