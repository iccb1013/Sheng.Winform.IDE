namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlEventEditorPanel_UpdateFormData_Where
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
            this.btnAddWhere = new Sheng.SailingEase.Controls.SEButton();
            this.dataGridViewWhere = new System.Windows.Forms.DataGridView();
            this.btnDeleteWhere = new Sheng.SailingEase.Controls.SEButton();
            this.ColumnWhereDataItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnWhereDataSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnWhereMatchType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWhere)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddWhere
            // 
            this.btnAddWhere.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddWhere.Location = new System.Drawing.Point(3, 274);
            this.btnAddWhere.Name = "btnAddWhere";
            this.btnAddWhere.Size = new System.Drawing.Size(31, 23);
            this.btnAddWhere.TabIndex = 1;
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
            this.dataGridViewWhere.Location = new System.Drawing.Point(3, 35);
            this.dataGridViewWhere.Margin = new System.Windows.Forms.Padding(6);
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
            this.dataGridViewWhere.Size = new System.Drawing.Size(451, 233);
            this.dataGridViewWhere.StandardTab = true;
            this.dataGridViewWhere.TabIndex = 0;
            this.dataGridViewWhere.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridViewWhere_DataBindingComplete);
            // 
            // btnDeleteWhere
            // 
            this.btnDeleteWhere.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteWhere.Location = new System.Drawing.Point(40, 274);
            this.btnDeleteWhere.Name = "btnDeleteWhere";
            this.btnDeleteWhere.Size = new System.Drawing.Size(31, 23);
            this.btnDeleteWhere.TabIndex = 2;
            this.btnDeleteWhere.Text = "-";
            this.btnDeleteWhere.UseVisualStyleBackColor = true;
            this.btnDeleteWhere.Click += new System.EventHandler(this.btnDeleteWhere_Click);
            // 
            // ColumnWhereDataItemName
            // 
            this.ColumnWhereDataItemName.DataPropertyName = "DataItemName";
            this.ColumnWhereDataItemName.HeaderText = "${UserControlEventEditorPanel_UpdateFormData_Where_ColumnWhereDataItemName}";
            this.ColumnWhereDataItemName.Name = "ColumnWhereDataItemName";
            this.ColumnWhereDataItemName.ReadOnly = true;
            this.ColumnWhereDataItemName.Width = 120;
            // 
            // ColumnWhereDataSource
            // 
            this.ColumnWhereDataSource.DataPropertyName = "SourceName";
            this.ColumnWhereDataSource.HeaderText = "${UserControlEventEditorPanel_UpdateFormData_Where_ColumnWhereDataSource}";
            this.ColumnWhereDataSource.Name = "ColumnWhereDataSource";
            this.ColumnWhereDataSource.ReadOnly = true;
            this.ColumnWhereDataSource.Width = 200;
            // 
            // ColumnWhereMatchType
            // 
            this.ColumnWhereMatchType.DataPropertyName = "MatchType";
            this.ColumnWhereMatchType.HeaderText = "${UserControlEventEditorPanel_UpdateFormData_Where_ColumnWhereMatchType}";
            this.ColumnWhereMatchType.Name = "ColumnWhereMatchType";
            this.ColumnWhereMatchType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnWhereMatchType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // UserControlEventEditorPanel_UpdateFormData_Where
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAddWhere);
            this.Controls.Add(this.dataGridViewWhere);
            this.Controls.Add(this.btnDeleteWhere);
            this.Name = "UserControlEventEditorPanel_UpdateFormData_Where";
            this.Controls.SetChildIndex(this.btnDeleteWhere, 0);
            this.Controls.SetChildIndex(this.dataGridViewWhere, 0);
            this.Controls.SetChildIndex(this.btnAddWhere, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWhere)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sheng.SailingEase.Controls.SEButton btnAddWhere;
        private System.Windows.Forms.DataGridView dataGridViewWhere;
        private Sheng.SailingEase.Controls.SEButton btnDeleteWhere;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnWhereDataItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnWhereDataSource;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnWhereMatchType;
    }
}
