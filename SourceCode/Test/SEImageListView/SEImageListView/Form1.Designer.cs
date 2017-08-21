namespace SEImageListView
{
    partial class Form1
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            SEImageListView.ImageListViewTheme imageListViewTheme1 = new SEImageListView.ImageListViewTheme();
            this.lblFirstPartiallyVisible = new System.Windows.Forms.Label();
            this.lblLastPartiallyVisible = new System.Windows.Forms.Label();
            this.lblFirstVisible = new System.Windows.Forms.Label();
            this.lblLastVisible = new System.Windows.Forms.Label();
            this.btnShowDebugInfo = new System.Windows.Forms.Button();
            this.lblLastMouseDownLocation = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.aToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAdd = new System.Windows.Forms.Button();
            this.seImageListView1 = new SEImageListView.ImageListView();
            this.btnRemove = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFirstPartiallyVisible
            // 
            this.lblFirstPartiallyVisible.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFirstPartiallyVisible.AutoSize = true;
            this.lblFirstPartiallyVisible.Location = new System.Drawing.Point(587, 55);
            this.lblFirstPartiallyVisible.Name = "lblFirstPartiallyVisible";
            this.lblFirstPartiallyVisible.Size = new System.Drawing.Size(137, 12);
            this.lblFirstPartiallyVisible.TabIndex = 1;
            this.lblFirstPartiallyVisible.Text = "FirstPartiallyVisible:";
            // 
            // lblLastPartiallyVisible
            // 
            this.lblLastPartiallyVisible.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLastPartiallyVisible.AutoSize = true;
            this.lblLastPartiallyVisible.Location = new System.Drawing.Point(587, 79);
            this.lblLastPartiallyVisible.Name = "lblLastPartiallyVisible";
            this.lblLastPartiallyVisible.Size = new System.Drawing.Size(131, 12);
            this.lblLastPartiallyVisible.TabIndex = 2;
            this.lblLastPartiallyVisible.Text = "LastPartiallyVisible:";
            // 
            // lblFirstVisible
            // 
            this.lblFirstVisible.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFirstVisible.AutoSize = true;
            this.lblFirstVisible.Location = new System.Drawing.Point(587, 105);
            this.lblFirstVisible.Name = "lblFirstVisible";
            this.lblFirstVisible.Size = new System.Drawing.Size(83, 12);
            this.lblFirstVisible.TabIndex = 3;
            this.lblFirstVisible.Text = "FirstVisible:";
            // 
            // lblLastVisible
            // 
            this.lblLastVisible.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLastVisible.AutoSize = true;
            this.lblLastVisible.Location = new System.Drawing.Point(587, 128);
            this.lblLastVisible.Name = "lblLastVisible";
            this.lblLastVisible.Size = new System.Drawing.Size(77, 12);
            this.lblLastVisible.TabIndex = 4;
            this.lblLastVisible.Text = "LastVisible:";
            // 
            // btnShowDebugInfo
            // 
            this.btnShowDebugInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowDebugInfo.Location = new System.Drawing.Point(589, 21);
            this.btnShowDebugInfo.Name = "btnShowDebugInfo";
            this.btnShowDebugInfo.Size = new System.Drawing.Size(135, 23);
            this.btnShowDebugInfo.TabIndex = 5;
            this.btnShowDebugInfo.Text = "ShowDebugInfo";
            this.btnShowDebugInfo.UseVisualStyleBackColor = true;
            this.btnShowDebugInfo.Click += new System.EventHandler(this.btnShowDebugInfo_Click);
            // 
            // lblLastMouseDownLocation
            // 
            this.lblLastMouseDownLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLastMouseDownLocation.AutoSize = true;
            this.lblLastMouseDownLocation.Location = new System.Drawing.Point(587, 155);
            this.lblLastMouseDownLocation.Name = "lblLastMouseDownLocation";
            this.lblLastMouseDownLocation.Size = new System.Drawing.Size(137, 12);
            this.lblLastMouseDownLocation.TabIndex = 6;
            this.lblLastMouseDownLocation.Text = "LastMouseDownLocation:";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aToolStripMenuItem,
            this.cToolStripMenuItem,
            this.dToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(85, 70);
            // 
            // aToolStripMenuItem
            // 
            this.aToolStripMenuItem.Name = "aToolStripMenuItem";
            this.aToolStripMenuItem.Size = new System.Drawing.Size(84, 22);
            this.aToolStripMenuItem.Text = "a";
            // 
            // cToolStripMenuItem
            // 
            this.cToolStripMenuItem.Name = "cToolStripMenuItem";
            this.cToolStripMenuItem.Size = new System.Drawing.Size(84, 22);
            this.cToolStripMenuItem.Text = "c";
            // 
            // dToolStripMenuItem
            // 
            this.dToolStripMenuItem.Name = "dToolStripMenuItem";
            this.dToolStripMenuItem.Size = new System.Drawing.Size(84, 22);
            this.dToolStripMenuItem.Text = "d";
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(587, 205);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(116, 23);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // seImageListView1
            // 
            this.seImageListView1.AllowMultiSelection = false;
            this.seImageListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.seImageListView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.seImageListView1.ContextMenuStrip = this.contextMenuStrip1;
            this.seImageListView1.Location = new System.Drawing.Point(12, 21);
            this.seImageListView1.Name = "seImageListView1";
            this.seImageListView1.Padding = new System.Windows.Forms.Padding(10);
            this.seImageListView1.Size = new System.Drawing.Size(569, 448);
            this.seImageListView1.TabIndex = 0;
            this.seImageListView1.Text = "seImageListView1";
            imageListViewTheme1.BackColor = System.Drawing.SystemColors.Window;
            imageListViewTheme1.HoverColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            imageListViewTheme1.HoverColorStart = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            imageListViewTheme1.ImageInnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            imageListViewTheme1.ImageOuterBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            imageListViewTheme1.ItemBackColor = System.Drawing.SystemColors.Window;
            imageListViewTheme1.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            imageListViewTheme1.ItemHeaderColor = System.Drawing.SystemColors.WindowText;
            imageListViewTheme1.ItemHeaderFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            imageListViewTheme1.SelectedColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            imageListViewTheme1.SelectedColorStart = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            imageListViewTheme1.SelectionRectangleBorderColor = System.Drawing.SystemColors.Highlight;
            imageListViewTheme1.SelectionRectangleColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            imageListViewTheme1.UnFocusedColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            imageListViewTheme1.UnFocusedColorStart = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.seImageListView1.Theme = imageListViewTheme1;
            this.seImageListView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.seImageListView1_MouseDown);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Location = new System.Drawing.Point(587, 234);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(116, 23);
            this.btnRemove.TabIndex = 8;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 481);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblLastMouseDownLocation);
            this.Controls.Add(this.btnShowDebugInfo);
            this.Controls.Add(this.lblLastVisible);
            this.Controls.Add(this.lblFirstVisible);
            this.Controls.Add(this.lblLastPartiallyVisible);
            this.Controls.Add(this.lblFirstPartiallyVisible);
            this.Controls.Add(this.seImageListView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ImageListView seImageListView1;
        private System.Windows.Forms.Label lblFirstPartiallyVisible;
        private System.Windows.Forms.Label lblLastPartiallyVisible;
        private System.Windows.Forms.Label lblFirstVisible;
        private System.Windows.Forms.Label lblLastVisible;
        private System.Windows.Forms.Button btnShowDebugInfo;
        private System.Windows.Forms.Label lblLastMouseDownLocation;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem aToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dToolStripMenuItem;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
    }
}

