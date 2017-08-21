namespace Sheng.SailingEase.ComponentModel.Design
{
    partial class PropertyGridPad
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertyGridPad));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonShowDescription = new System.Windows.Forms.ToolStripButton();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.panelVerb = new Sheng.SailingEase.Controls.SEPanel();
            this.linkLabelVerb = new System.Windows.Forms.LinkLabel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.propertyGridDataGridView = new Sheng.SailingEase.ComponentModel.Design.PropertyGridDataGridView();
            this.toolStrip1.SuspendLayout();
            this.panelVerb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertyGridDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonShowDescription});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(432, 25);
            this.toolStrip1.TabIndex = 19;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonShowDescription
            // 
            this.toolStripButtonShowDescription.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonShowDescription.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonShowDescription.Image")));
            this.toolStripButtonShowDescription.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowDescription.Name = "toolStripButtonShowDescription";
            this.toolStripButtonShowDescription.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonShowDescription.Text = "显示注释";
            this.toolStripButtonShowDescription.Click += new System.EventHandler(this.toolStripButtonShowDescription_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtDescription.Location = new System.Drawing.Point(0, 317);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(432, 40);
            this.txtDescription.TabIndex = 20;
            this.txtDescription.Visible = false;
            // 
            // panelVerb
            // 
            this.panelVerb.BorderColor = System.Drawing.Color.Black;
            this.panelVerb.Controls.Add(this.linkLabelVerb);
            this.panelVerb.Controls.Add(this.textBox1);
            this.panelVerb.Controls.Add(this.panel1);
            this.panelVerb.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelVerb.FillColorEnd = System.Drawing.Color.Empty;
            this.panelVerb.FillColorStart = System.Drawing.Color.Empty;
            this.panelVerb.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.panelVerb.FillStyle = Sheng.SailingEase.Controls.FillStyle.Solid;
            this.panelVerb.HighLight = false;
            this.panelVerb.Location = new System.Drawing.Point(0, 275);
            this.panelVerb.Name = "panelVerb";
            this.panelVerb.ShowBorder = false;
            this.panelVerb.Size = new System.Drawing.Size(432, 42);
            this.panelVerb.TabIndex = 22;
            // 
            // linkLabelVerb
            // 
            this.linkLabelVerb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelVerb.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.linkLabelVerb.Location = new System.Drawing.Point(3, 3);
            this.linkLabelVerb.Name = "linkLabelVerb";
            this.linkLabelVerb.Size = new System.Drawing.Size(426, 34);
            this.linkLabelVerb.TabIndex = 2;
            this.linkLabelVerb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelVerb_LinkClicked);
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(432, 40);
            this.textBox1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 40);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(432, 2);
            this.panel1.TabIndex = 0;
            // 
            // propertyGridDataGridView
            // 
            this.propertyGridDataGridView.AllowUserToAddRows = false;
            this.propertyGridDataGridView.AllowUserToDeleteRows = false;
            this.propertyGridDataGridView.AllowUserToResizeRows = false;
            this.propertyGridDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.propertyGridDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.propertyGridDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.propertyGridDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.propertyGridDataGridView.ColumnHeadersVisible = false;
            this.propertyGridDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridDataGridView.EnableHeadersVisualStyles = false;
            this.propertyGridDataGridView.GridColor = System.Drawing.SystemColors.ControlLight;
            this.propertyGridDataGridView.Location = new System.Drawing.Point(0, 25);
            this.propertyGridDataGridView.MultiSelect = false;
            this.propertyGridDataGridView.Name = "propertyGridDataGridView";
            this.propertyGridDataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlLight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.propertyGridDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.propertyGridDataGridView.RowHeadersWidth = 20;
            this.propertyGridDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.propertyGridDataGridView.RowTemplate.Height = 23;
            this.propertyGridDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.propertyGridDataGridView.ShowEditingIcon = false;
            this.propertyGridDataGridView.Size = new System.Drawing.Size(432, 250);
            this.propertyGridDataGridView.TabIndex = 21;
            this.propertyGridDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewProperty_CellValueChanged);
            this.propertyGridDataGridView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewProperty_RowEnter);
            // 
            // UserControlPropertyGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.propertyGridDataGridView);
            this.Controls.Add(this.panelVerb);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.toolStrip1);
            this.Name = "UserControlPropertyGrid";
            this.Size = new System.Drawing.Size(432, 357);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panelVerb.ResumeLayout(false);
            this.panelVerb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.propertyGridDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TextBox txtDescription;
        private PropertyGridDataGridView propertyGridDataGridView;
        private System.Windows.Forms.ToolStripButton toolStripButtonShowDescription;
        private Sheng.SailingEase.Controls.SEPanel panelVerb;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel linkLabelVerb;
    }
}
