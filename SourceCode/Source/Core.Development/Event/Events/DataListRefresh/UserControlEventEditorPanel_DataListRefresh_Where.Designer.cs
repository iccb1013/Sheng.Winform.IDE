namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlEventEditorPanel_DataListRefresh_Where
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
            this.btnDelete = new Sheng.SailingEase.Controls.SEButton();
            this.btnAdd = new Sheng.SailingEase.Controls.SEButton();
            this.dataGridViewWhere = new System.Windows.Forms.DataGridView();
            this.ColumnDataItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDataSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMatchType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWhere)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(41, 274);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(31, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "-";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(4, 274);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(31, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
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
            this.ColumnDataItemName,
            this.ColumnDataSource,
            this.ColumnMatchType});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewWhere.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewWhere.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewWhere.Location = new System.Drawing.Point(4, 35);
            this.dataGridViewWhere.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
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
            this.dataGridViewWhere.Size = new System.Drawing.Size(453, 233);
            this.dataGridViewWhere.StandardTab = true;
            this.dataGridViewWhere.TabIndex = 0;
            // 
            // ColumnDataItemName
            // 
            this.ColumnDataItemName.DataPropertyName = "DataItemName";
            this.ColumnDataItemName.HeaderText = "${UserControlEventEditorPanel_DataListRefresh_Where_ColumnDataItemName}";
            this.ColumnDataItemName.Name = "ColumnDataItemName";
            this.ColumnDataItemName.ReadOnly = true;
            // 
            // ColumnDataSource
            // 
            this.ColumnDataSource.DataPropertyName = "SourceName";
            this.ColumnDataSource.HeaderText = "${UserControlEventEditorPanel_DataListRefresh_Where_ColumnDataSource}";
            this.ColumnDataSource.Name = "ColumnDataSource";
            this.ColumnDataSource.ReadOnly = true;
            this.ColumnDataSource.Width = 230;
            // 
            // ColumnMatchType
            // 
            this.ColumnMatchType.DataPropertyName = "MatchType";
            this.ColumnMatchType.HeaderText = "${UserControlEventEditorPanel_DataListRefresh_Where_ColumnMatchType}";
            this.ColumnMatchType.Name = "ColumnMatchType";
            this.ColumnMatchType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnMatchType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // UserControlEventEditorPanel_DataListRefresh_Where
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dataGridViewWhere);
            this.Name = "UserControlEventEditorPanel_DataListRefresh_Where";
            this.Controls.SetChildIndex(this.dataGridViewWhere, 0);
            this.Controls.SetChildIndex(this.btnAdd, 0);
            this.Controls.SetChildIndex(this.btnDelete, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWhere)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sheng.SailingEase.Controls.SEButton btnDelete;
        private Sheng.SailingEase.Controls.SEButton btnAdd;
        private System.Windows.Forms.DataGridView dataGridViewWhere;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDataItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDataSource;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnMatchType;
    }
}
