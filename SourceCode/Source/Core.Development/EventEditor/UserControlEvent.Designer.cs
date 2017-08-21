namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlEvent
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlEvent));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAddEvent = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEditEvent = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDeleteEvent = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonUpEvent = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDownEvent = new System.Windows.Forms.ToolStripButton();
            this.dataGridViewEvent = new Sheng.SailingEase.Controls.SEDataGridView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemUp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDown = new System.Windows.Forms.ToolStripMenuItem();
            this.ColumnEventEventName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEventTimeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEventName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEventCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEvent)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddEvent,
            this.toolStripButtonEditEvent,
            this.toolStripButtonDeleteEvent,
            this.toolStripSeparator1,
            this.toolStripButtonUpEvent,
            this.toolStripButtonDownEvent});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip.Size = new System.Drawing.Size(528, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.TabStop = true;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripButtonAddEvent
            // 
            this.toolStripButtonAddEvent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAddEvent.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddEvent.Image")));
            this.toolStripButtonAddEvent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddEvent.Name = "toolStripButtonAddEvent";
            this.toolStripButtonAddEvent.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonAddEvent.Text = "${UserControlEvent_ToolStripButtonAddEvent}";
            this.toolStripButtonAddEvent.Click += new System.EventHandler(this.toolStripButtonAddEvent_Click);
            // 
            // toolStripButtonEditEvent
            // 
            this.toolStripButtonEditEvent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonEditEvent.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEditEvent.Image")));
            this.toolStripButtonEditEvent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEditEvent.Name = "toolStripButtonEditEvent";
            this.toolStripButtonEditEvent.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonEditEvent.Text = "${UserControlEvent_ToolStripButtonEditEvent}";
            this.toolStripButtonEditEvent.Click += new System.EventHandler(this.toolStripButtonEditEvent_Click);
            // 
            // toolStripButtonDeleteEvent
            // 
            this.toolStripButtonDeleteEvent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDeleteEvent.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDeleteEvent.Image")));
            this.toolStripButtonDeleteEvent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDeleteEvent.Name = "toolStripButtonDeleteEvent";
            this.toolStripButtonDeleteEvent.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDeleteEvent.Text = "${UserControlEvent_ToolStripButtonDeleteEvent}";
            this.toolStripButtonDeleteEvent.Click += new System.EventHandler(this.toolStripButtonDeleteEvent_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonUpEvent
            // 
            this.toolStripButtonUpEvent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonUpEvent.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonUpEvent.Image")));
            this.toolStripButtonUpEvent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUpEvent.Name = "toolStripButtonUpEvent";
            this.toolStripButtonUpEvent.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonUpEvent.Text = "${UserControlEvent_ToolStripButtonUpEvent}";
            this.toolStripButtonUpEvent.Click += new System.EventHandler(this.toolStripButtonUpEvent_Click);
            // 
            // toolStripButtonDownEvent
            // 
            this.toolStripButtonDownEvent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDownEvent.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDownEvent.Image")));
            this.toolStripButtonDownEvent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDownEvent.Name = "toolStripButtonDownEvent";
            this.toolStripButtonDownEvent.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDownEvent.Text = "${UserControlEvent_ToolStripButtonDownEvent}";
            this.toolStripButtonDownEvent.Click += new System.EventHandler(this.toolStripButtonDownEvent_Click);
            // 
            // dataGridViewEvent
            // 
            this.dataGridViewEvent.AllowUserToAddRows = false;
            this.dataGridViewEvent.AllowUserToDeleteRows = false;
            this.dataGridViewEvent.AllowUserToResizeRows = false;
            this.dataGridViewEvent.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewEvent.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewEvent.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridViewEvent.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewEvent.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewEvent.ColumnHeadersHeight = 21;
            this.dataGridViewEvent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewEvent.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnEventEventName,
            this.ColumnEventTimeName,
            this.ColumnEventName,
            this.ColumnEventCode});
            this.dataGridViewEvent.ContextMenuStrip = this.contextMenuStrip;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewEvent.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewEvent.Location = new System.Drawing.Point(0, 25);
            this.dataGridViewEvent.Name = "dataGridViewEvent";
            this.dataGridViewEvent.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewEvent.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewEvent.RowHeadersVisible = false;
            this.dataGridViewEvent.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewEvent.RowTemplate.Height = 21;
            this.dataGridViewEvent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewEvent.Size = new System.Drawing.Size(528, 273);
            this.dataGridViewEvent.StandardTab = true;
            this.dataGridViewEvent.TabIndex = 0;
            this.dataGridViewEvent.WaterText = "";
            this.dataGridViewEvent.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewEvent_CellMouseDoubleClick);
            this.dataGridViewEvent.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridViewEvent_DataBindingComplete);
            this.dataGridViewEvent.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dataGridViewEvent_RowStateChanged);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAdd,
            this.toolStripMenuItemEdit,
            this.toolStripMenuItemDelete,
            this.toolStripMenuItem1,
            this.toolStripMenuItemUp,
            this.toolStripMenuItemDown});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(347, 120);
            // 
            // toolStripMenuItemAdd
            // 
            this.toolStripMenuItemAdd.Name = "toolStripMenuItemAdd";
            this.toolStripMenuItemAdd.Size = new System.Drawing.Size(346, 22);
            this.toolStripMenuItemAdd.Text = "${UserControlEvent_ToolStripMenuItemAdd}";
            this.toolStripMenuItemAdd.Click += new System.EventHandler(this.toolStripMenuItemAdd_Click);
            // 
            // toolStripMenuItemEdit
            // 
            this.toolStripMenuItemEdit.Name = "toolStripMenuItemEdit";
            this.toolStripMenuItemEdit.Size = new System.Drawing.Size(346, 22);
            this.toolStripMenuItemEdit.Text = "${UserControlEvent_ToolStripMenuItemEdit}";
            this.toolStripMenuItemEdit.Click += new System.EventHandler(this.toolStripMenuItemEdit_Click);
            // 
            // toolStripMenuItemDelete
            // 
            this.toolStripMenuItemDelete.Name = "toolStripMenuItemDelete";
            this.toolStripMenuItemDelete.Size = new System.Drawing.Size(346, 22);
            this.toolStripMenuItemDelete.Text = "${UserControlEvent_ToolStripMenuItemDelete}";
            this.toolStripMenuItemDelete.Click += new System.EventHandler(this.toolStripMenuItemDelete_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(343, 6);
            // 
            // toolStripMenuItemUp
            // 
            this.toolStripMenuItemUp.Name = "toolStripMenuItemUp";
            this.toolStripMenuItemUp.Size = new System.Drawing.Size(346, 22);
            this.toolStripMenuItemUp.Text = "${UserControlEvent_ToolStripMenuItemUp}";
            this.toolStripMenuItemUp.Click += new System.EventHandler(this.toolStripMenuItemUp_Click);
            // 
            // toolStripMenuItemDown
            // 
            this.toolStripMenuItemDown.Name = "toolStripMenuItemDown";
            this.toolStripMenuItemDown.Size = new System.Drawing.Size(346, 22);
            this.toolStripMenuItemDown.Text = "${UserControlEvent_ToolStripMenuItemDown}";
            this.toolStripMenuItemDown.Click += new System.EventHandler(this.toolStripMenuItemDown_Click);
            // 
            // ColumnEventEventName
            // 
            this.ColumnEventEventName.DataPropertyName = "EventName";
            this.ColumnEventEventName.HeaderText = "${UserControlEvent_ColumnEventEventName}";
            this.ColumnEventEventName.Name = "ColumnEventEventName";
            this.ColumnEventEventName.ReadOnly = true;
            this.ColumnEventEventName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnEventTimeName
            // 
            this.ColumnEventTimeName.DataPropertyName = "EventTimeName";
            this.ColumnEventTimeName.HeaderText = "${UserControlEvent_ColumnEventTimeName}";
            this.ColumnEventTimeName.Name = "ColumnEventTimeName";
            this.ColumnEventTimeName.ReadOnly = true;
            this.ColumnEventTimeName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnEventName
            // 
            this.ColumnEventName.DataPropertyName = "Name";
            this.ColumnEventName.HeaderText = "${UserControlEvent_ColumnEventName}";
            this.ColumnEventName.Name = "ColumnEventName";
            this.ColumnEventName.ReadOnly = true;
            this.ColumnEventName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnEventCode
            // 
            this.ColumnEventCode.DataPropertyName = "Code";
            this.ColumnEventCode.HeaderText = "${UserControlEvent_ColumnEventCode}";
            this.ColumnEventCode.Name = "ColumnEventCode";
            this.ColumnEventCode.ReadOnly = true;
            this.ColumnEventCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // UserControlEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridViewEvent);
            this.Controls.Add(this.toolStrip);
            this.Name = "UserControlEvent";
            this.Size = new System.Drawing.Size(528, 298);
            this.Load += new System.EventHandler(this.UserControlEvent_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEvent)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddEvent;
        private System.Windows.Forms.ToolStripButton toolStripButtonEditEvent;
        private System.Windows.Forms.ToolStripButton toolStripButtonDeleteEvent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonUpEvent;
        private System.Windows.Forms.ToolStripButton toolStripButtonDownEvent;
        private Sheng.SailingEase.Controls.SEDataGridView dataGridViewEvent;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAdd;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEdit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemUp;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDown;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEventEventName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEventTimeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEventName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEventCode;
    }
}
