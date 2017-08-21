namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlEventEditorPanel_DataListUpdateRow_Data
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
            this.dataGridViewDataSet = new Controls.SEDataGridView();
            this.ColumnDataColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSourceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnWhere = new System.Windows.Forms.DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(40, 274);
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
            this.btnAdd.Location = new System.Drawing.Point(3, 274);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(31, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dataGridViewDataSet
            // 
            this.dataGridViewDataSet.AllowUserToAddRows = false;
            this.dataGridViewDataSet.AllowUserToDeleteRows = false;
            this.dataGridViewDataSet.AllowUserToResizeRows = false;
            this.dataGridViewDataSet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewDataSet.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewDataSet.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewDataSet.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewDataSet.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewDataSet.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewDataSet.ColumnHeadersHeight = 21;
            this.dataGridViewDataSet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewDataSet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnDataColumnName,
            this.ColumnSourceName,
            this.ColumnWhere});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewDataSet.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewDataSet.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewDataSet.Location = new System.Drawing.Point(4, 35);
            this.dataGridViewDataSet.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.dataGridViewDataSet.MultiSelect = false;
            this.dataGridViewDataSet.Name = "dataGridViewDataSet";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewDataSet.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewDataSet.RowHeadersVisible = false;
            this.dataGridViewDataSet.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewDataSet.RowTemplate.Height = 21;
            this.dataGridViewDataSet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewDataSet.Size = new System.Drawing.Size(453, 233);
            this.dataGridViewDataSet.StandardTab = true;
            this.dataGridViewDataSet.TabIndex = 0;
            this.dataGridViewDataSet.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridViewDataSet_DataBindingComplete);
            // 
            // ColumnDataColumnName
            // 
            this.ColumnDataColumnName.DataPropertyName = "DataColumnName";
            this.ColumnDataColumnName.HeaderText = "${UserControlEventEditorPanel_DataListUpdateRow_Data_ColumnDataColumnName}";
            this.ColumnDataColumnName.Name = "ColumnDataColumnName";
            this.ColumnDataColumnName.ReadOnly = true;
            this.ColumnDataColumnName.Width = 180;
            // 
            // ColumnSourceName
            // 
            this.ColumnSourceName.DataPropertyName = "SourceName";
            this.ColumnSourceName.HeaderText = "${UserControlEventEditorPanel_DataListUpdateRow_Data_ColumnSourceName}";
            this.ColumnSourceName.Name = "ColumnSourceName";
            this.ColumnSourceName.ReadOnly = true;
            this.ColumnSourceName.Width = 180;
            // 
            // ColumnWhere
            // 
            this.ColumnWhere.DataPropertyName = "Where";
            this.ColumnWhere.HeaderText = "${UserControlEventEditorPanel_DataListUpdateRow_Data_ColumnWhere}";
            this.ColumnWhere.Name = "ColumnWhere";
            this.ColumnWhere.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnWhere.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // UserControlEventEditorPanel_DataListUpdateRow_Data
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dataGridViewDataSet);
            this.Name = "UserControlEventEditorPanel_DataListUpdateRow_Data";
            this.Controls.SetChildIndex(this.dataGridViewDataSet, 0);
            this.Controls.SetChildIndex(this.btnAdd, 0);
            this.Controls.SetChildIndex(this.btnDelete, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sheng.SailingEase.Controls.SEButton btnDelete;
        private Sheng.SailingEase.Controls.SEButton btnAdd;
        private Sheng.SailingEase.Controls.SEDataGridView dataGridViewDataSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDataColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSourceName;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnWhere;
    }
}
