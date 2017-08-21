namespace Sheng.SailingEase.Core.Development
{
    partial class FormGlobalFormElementChoose
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Sheng.SailingEase.Controls.TreeViewGrid.TreeColumn treeColumn1 = new Sheng.SailingEase.Controls.TreeViewGrid.TreeColumn();
            Sheng.SailingEase.Controls.TreeViewGrid.TreeColumn treeColumn2 = new Sheng.SailingEase.Controls.TreeViewGrid.TreeColumn();
            Sheng.SailingEase.Controls.TreeViewGrid.TreeColumn treeColumn3 = new Sheng.SailingEase.Controls.TreeViewGrid.TreeColumn();
            this.treeViewGridFormElement = new Sheng.SailingEase.Controls.TreeViewGrid.SETreeViewGrid();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemShowAll = new System.Windows.Forms.ToolStripMenuItem();
            this.nodeIcon1 = new Sheng.SailingEase.Controls.TreeViewGrid.NodeControls.NodeIcon();
            this.nodeTextBox1 = new Sheng.SailingEase.Controls.TreeViewGrid.NodeControls.NodeTextBox();
            this.nodeTextBox2 = new Sheng.SailingEase.Controls.TreeViewGrid.NodeControls.NodeTextBox();
            this.nodeTextBox3 = new Sheng.SailingEase.Controls.TreeViewGrid.NodeControls.NodeTextBox();
            this.lblTitle = new Sheng.SailingEase.Controls.SEAdvLabel();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabelFormElement = new System.Windows.Forms.LinkLabel();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewGridFormElement
            // 
            this.treeViewGridFormElement.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewGridFormElement.BackColor = System.Drawing.SystemColors.Window;
            this.treeViewGridFormElement.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            treeColumn1.Header = "";
            treeColumn1.Width = 180;
            treeColumn2.Header = "";
            treeColumn2.Width = 160;
            treeColumn3.Header = "";
            treeColumn3.Width = 120;
            this.treeViewGridFormElement.Columns.Add(treeColumn1);
            this.treeViewGridFormElement.Columns.Add(treeColumn2);
            this.treeViewGridFormElement.Columns.Add(treeColumn3);
            this.treeViewGridFormElement.ContextMenuStrip = this.contextMenuStrip;
            this.treeViewGridFormElement.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeViewGridFormElement.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeViewGridFormElement.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeViewGridFormElement.Location = new System.Drawing.Point(12, 87);
            this.treeViewGridFormElement.Margin = new System.Windows.Forms.Padding(3, 12, 3, 12);
            this.treeViewGridFormElement.Model = null;
            this.treeViewGridFormElement.Name = "treeViewGridFormElement";
            this.treeViewGridFormElement.NodeControls.Add(this.nodeIcon1);
            this.treeViewGridFormElement.NodeControls.Add(this.nodeTextBox1);
            this.treeViewGridFormElement.NodeControls.Add(this.nodeTextBox2);
            this.treeViewGridFormElement.NodeControls.Add(this.nodeTextBox3);
            this.treeViewGridFormElement.SelectedNode = null;
            this.treeViewGridFormElement.Size = new System.Drawing.Size(489, 250);
            this.treeViewGridFormElement.TabIndex = 0;
            this.treeViewGridFormElement.Text = "seTreeViewGrid1";
            this.treeViewGridFormElement.UseColumns = true;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSelect,
            this.toolStripMenuItem1,
            this.toolStripMenuItemShowAll});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(442, 54);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // toolStripMenuItemSelect
            // 
            this.toolStripMenuItemSelect.Name = "toolStripMenuItemSelect";
            this.toolStripMenuItemSelect.Size = new System.Drawing.Size(441, 22);
            this.toolStripMenuItemSelect.Text = "${FormGlobalFormElementChoose_ToolStripMenuItemSelect}";
            this.toolStripMenuItemSelect.Click += new System.EventHandler(this.toolStripMenuItemSelect_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(438, 6);
            // 
            // toolStripMenuItemShowAll
            // 
            this.toolStripMenuItemShowAll.Name = "toolStripMenuItemShowAll";
            this.toolStripMenuItemShowAll.Size = new System.Drawing.Size(441, 22);
            this.toolStripMenuItemShowAll.Text = "${FormGlobalFormElementChoose_ToolStripMenuItemShowAll}";
            this.toolStripMenuItemShowAll.Click += new System.EventHandler(this.toolStripMenuItemShowAll_Click);
            // 
            // nodeIcon1
            // 
            this.nodeIcon1.DataPropertyName = "Icon";
            this.nodeIcon1.DrawActive = false;
            // 
            // nodeTextBox1
            // 
            this.nodeTextBox1.DataPropertyName = "Name";
            // 
            // nodeTextBox2
            // 
            this.nodeTextBox2.Column = 1;
            this.nodeTextBox2.DataPropertyName = "Code";
            this.nodeTextBox2.DrawActive = false;
            // 
            // nodeTextBox3
            // 
            this.nodeTextBox3.Column = 2;
            this.nodeTextBox3.DataPropertyName = "ControlTypeName";
            this.nodeTextBox3.DrawActive = false;
            // 
            // lblTitle
            // 
            this.lblTitle.BorderColor = System.Drawing.Color.Black;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.FillColorEnd = System.Drawing.Color.Empty;
            this.lblTitle.FillColorStart = System.Drawing.Color.White;
            this.lblTitle.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.lblTitle.FillStyle = Sheng.SailingEase.Controls.FillStyle.LinearGradient;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.ShowBorder = false;
            this.lblTitle.SingleLine = true;
            this.lblTitle.Size = new System.Drawing.Size(513, 23);
            this.lblTitle.TabIndex = 25;
            this.lblTitle.Text = " ${FormGlobalFormElementChoose_LabelTitle}";
            this.lblTitle.TextHorizontalCenter = false;
            this.lblTitle.TextVerticalCenter = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(426, 407);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "${FormGlobalFormElementChoose_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(345, 407);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 27;
            this.btnOK.Text = "${FormGlobalFormElementChoose_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(489, 40);
            this.label1.TabIndex = 28;
            this.label1.Text = "${FormGlobalFormElementChoose_LabelDescription}";
            // 
            // linkLabelFormElement
            // 
            this.linkLabelFormElement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelFormElement.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.linkLabelFormElement.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.linkLabelFormElement.Location = new System.Drawing.Point(10, 349);
            this.linkLabelFormElement.Name = "linkLabelFormElement";
            this.linkLabelFormElement.Size = new System.Drawing.Size(491, 46);
            this.linkLabelFormElement.TabIndex = 29;
            this.linkLabelFormElement.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelFormElement_LinkClicked);
            // 
            // FormGlobalFormElementChoose
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(513, 442);
            this.Controls.Add(this.linkLabelFormElement);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.treeViewGridFormElement);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGlobalFormElementChoose";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${FormGlobalFormElementChoose}";
            this.Load += new System.EventHandler(this.FormGlobalFormElementChoose_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sheng.SailingEase.Controls.TreeViewGrid.SETreeViewGrid treeViewGridFormElement;
        private Sheng.SailingEase.Controls.SEAdvLabel lblTitle;
        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private Sheng.SailingEase.Controls.TreeViewGrid.NodeControls.NodeTextBox nodeTextBox1;
        private Sheng.SailingEase.Controls.TreeViewGrid.NodeControls.NodeTextBox nodeTextBox2;
        private Sheng.SailingEase.Controls.TreeViewGrid.NodeControls.NodeTextBox nodeTextBox3;
        private Sheng.SailingEase.Controls.TreeViewGrid.NodeControls.NodeIcon nodeIcon1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabelFormElement;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShowAll;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSelect;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    }
}