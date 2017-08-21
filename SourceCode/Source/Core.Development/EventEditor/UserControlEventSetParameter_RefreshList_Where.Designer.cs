namespace Sheng.SIMBE.IDE.UI.EventSet
{
    partial class UserControlEventSetParameter_RefreshList_Where
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
            this.btnDelete = new Sheng.SIMBE.SEControl.SEButton();
            this.btnAdd = new Sheng.SIMBE.SEControl.SEButton();
            this.dataGridViewWhere = new System.Windows.Forms.DataGridView();
            this.ColumnDataItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDataSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMatchType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.lblTitle = new Sheng.SIMBE.SEControl.SEAdvLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWhere)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(41, 274);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(31, 23);
            this.btnDelete.TabIndex = 19;
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
            this.btnAdd.TabIndex = 18;
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
            this.dataGridViewWhere.TabIndex = 17;
            this.dataGridViewWhere.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridViewWhere_DataBindingComplete);
            // 
            // ColumnDataItemName
            // 
            this.ColumnDataItemName.DataPropertyName = "DataItemName";
            this.ColumnDataItemName.HeaderText = "ColumnDataItemName";
            this.ColumnDataItemName.Name = "ColumnDataItemName";
            this.ColumnDataItemName.ReadOnly = true;
            this.ColumnDataItemName.Width = 80;
            // 
            // ColumnDataSource
            // 
            this.ColumnDataSource.DataPropertyName = "DataSourceName";
            this.ColumnDataSource.HeaderText = "ColumnDataSource";
            this.ColumnDataSource.Name = "ColumnDataSource";
            this.ColumnDataSource.ReadOnly = true;
            this.ColumnDataSource.Width = 80;
            // 
            // ColumnMatchType
            // 
            this.ColumnMatchType.DataPropertyName = "MatchType";
            this.ColumnMatchType.HeaderText = "ColumnMatchType";
            this.ColumnMatchType.Name = "ColumnMatchType";
            this.ColumnMatchType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnMatchType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // lblTitle
            // 
            this.lblTitle.BorderColor = System.Drawing.Color.Black;
            this.lblTitle.FillColorEnd = System.Drawing.Color.Empty;
            this.lblTitle.FillColorStart = System.Drawing.Color.White;
            this.lblTitle.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.lblTitle.FillStyle = Sheng.SIMBE.SEControl.FillStyle.LinearGradient;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.ShowBorder = false;
            this.lblTitle.SingleLine = true;
            this.lblTitle.Size = new System.Drawing.Size(460, 23);
            this.lblTitle.TabIndex = 20;
            this.lblTitle.Text = " LabelWhere";
            this.lblTitle.TextHorizontalCenter = false;
            this.lblTitle.TextVerticalCenter = true;
            // 
            // UserControlEventSetParameter_RefreshList_Where
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dataGridViewWhere);
            this.Name = "UserControlEventSetParameter_RefreshList_Where";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWhere)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sheng.SIMBE.SEControl.SEButton btnDelete;
        private Sheng.SIMBE.SEControl.SEButton btnAdd;
        private System.Windows.Forms.DataGridView dataGridViewWhere;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDataItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDataSource;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColumnMatchType;
        private Sheng.SIMBE.SEControl.SEAdvLabel lblTitle;
    }
}
