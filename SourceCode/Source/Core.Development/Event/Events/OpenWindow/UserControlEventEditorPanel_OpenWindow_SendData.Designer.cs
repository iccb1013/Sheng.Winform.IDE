namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlEventEditorPanel_OpenWindow_SendData
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
            this.dataGridViewSendData = new System.Windows.Forms.DataGridView();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDataSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSendData)).BeginInit();
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
            // dataGridViewSendData
            // 
            this.dataGridViewSendData.AllowUserToAddRows = false;
            this.dataGridViewSendData.AllowUserToDeleteRows = false;
            this.dataGridViewSendData.AllowUserToResizeRows = false;
            this.dataGridViewSendData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewSendData.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewSendData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewSendData.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewSendData.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewSendData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewSendData.ColumnHeadersHeight = 21;
            this.dataGridViewSendData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewSendData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.ColumnCode,
            this.ColumnDataSource});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewSendData.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewSendData.Location = new System.Drawing.Point(3, 35);
            this.dataGridViewSendData.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.dataGridViewSendData.MultiSelect = false;
            this.dataGridViewSendData.Name = "dataGridViewSendData";
            this.dataGridViewSendData.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewSendData.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewSendData.RowHeadersVisible = false;
            this.dataGridViewSendData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewSendData.RowTemplate.Height = 21;
            this.dataGridViewSendData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewSendData.Size = new System.Drawing.Size(454, 233);
            this.dataGridViewSendData.StandardTab = true;
            this.dataGridViewSendData.TabIndex = 0;
            this.dataGridViewSendData.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridViewSendData_DataBindingComplete);
            // 
            // ColumnName
            // 
            this.ColumnName.DataPropertyName = "DataName";
            this.ColumnName.HeaderText = "${UserControlEventEditorPanel_OpenForm_SendData_ColumnName}";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            // 
            // ColumnCode
            // 
            this.ColumnCode.DataPropertyName = "DataCode";
            this.ColumnCode.HeaderText = "${UserControlEventEditorPanel_OpenForm_SendData_ColumnCode}";
            this.ColumnCode.Name = "ColumnCode";
            this.ColumnCode.ReadOnly = true;
            // 
            // ColumnDataSource
            // 
            this.ColumnDataSource.DataPropertyName = "DataSourceName";
            this.ColumnDataSource.HeaderText = "${UserControlEventEditorPanel_OpenForm_SendData_ColumnDataSource}";
            this.ColumnDataSource.Name = "ColumnDataSource";
            this.ColumnDataSource.ReadOnly = true;
            this.ColumnDataSource.Width = 240;
            // 
            // UserControlEventEditorPanel_OpenForm_SendData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dataGridViewSendData);
            this.Name = "UserControlEventEditorPanel_OpenForm_SendData";
            this.Controls.SetChildIndex(this.dataGridViewSendData, 0);
            this.Controls.SetChildIndex(this.btnAdd, 0);
            this.Controls.SetChildIndex(this.btnDelete, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSendData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sheng.SailingEase.Controls.SEButton btnDelete;
        private Sheng.SailingEase.Controls.SEButton btnAdd;
        private System.Windows.Forms.DataGridView dataGridViewSendData;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDataSource;
    }
}
