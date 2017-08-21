namespace Sheng.SailingEase.Core.Development
{
    partial class FormEventEditor
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
            Sheng.SailingEase.Controls.SEComboSelectorTheme seComboSelectorTheme1 = new Sheng.SailingEase.Controls.SEComboSelectorTheme();
            Sheng.SailingEase.Controls.SEComboSelectorTheme seComboSelectorTheme2 = new Sheng.SailingEase.Controls.SEComboSelectorTheme();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.lblEventTime = new System.Windows.Forms.Label();
            this.lblEvent = new System.Windows.Forms.Label();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.treeViewParameter = new Sheng.SailingEase.Controls.SENavigationTreeView();
            this.imageListParameter = new System.Windows.Forms.ImageList(this.components);
            this.panelParameter = new Sheng.SailingEase.Controls.SEPanel();
            this.seLine1 = new Sheng.SailingEase.Controls.SELine();
            this.sePanel1 = new Sheng.SailingEase.Controls.SEPanel();
            this.availableEventTimes = new Sheng.SailingEase.Controls.SEComboSelector2();
            this.availableEvents = new Sheng.SailingEase.Controls.SEComboSelector2();
            this.label1 = new System.Windows.Forms.Label();
            this.sePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(623, 436);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "${FormEventEditor_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblEventTime
            // 
            this.lblEventTime.AutoSize = true;
            this.lblEventTime.Location = new System.Drawing.Point(408, 41);
            this.lblEventTime.Name = "lblEventTime";
            this.lblEventTime.Size = new System.Drawing.Size(209, 12);
            this.lblEventTime.TabIndex = 16;
            this.lblEventTime.Text = "${FormEventEditor_LabelEventTime}:";
            // 
            // lblEvent
            // 
            this.lblEvent.AutoSize = true;
            this.lblEvent.Location = new System.Drawing.Point(12, 41);
            this.lblEvent.Name = "lblEvent";
            this.lblEvent.Size = new System.Drawing.Size(185, 12);
            this.lblEvent.TabIndex = 15;
            this.lblEvent.Text = "${FormEventEditor_LabelEvent}:";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(542, 436);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "${FormEventEditor_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // treeViewParameter
            // 
            this.treeViewParameter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewParameter.AutoDockFill = true;
            this.treeViewParameter.HideSelection = false;
            this.treeViewParameter.ImageIndex = 0;
            this.treeViewParameter.ImageList = this.imageListParameter;
            this.treeViewParameter.Location = new System.Drawing.Point(12, 84);
            this.treeViewParameter.Name = "treeViewParameter";
            this.treeViewParameter.SelectedImageIndex = 0;
            this.treeViewParameter.ShowLines = false;
            this.treeViewParameter.ShowPlusMinus = false;
            this.treeViewParameter.ShowRootLines = false;
            this.treeViewParameter.Size = new System.Drawing.Size(150, 330);
            this.treeViewParameter.TabIndex = 3;
            this.treeViewParameter.OnAfterSelectNavigationNode += new Sheng.SailingEase.Controls.SENavigationTreeView.OnAfterSelectNavigationNodeHandler(this.treeViewParameter_OnAfterSelectNavigationNode);
            // 
            // imageListParameter
            // 
            this.imageListParameter.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListParameter.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListParameter.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // panelParameter
            // 
            this.panelParameter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelParameter.BorderColor = System.Drawing.Color.Black;
            this.panelParameter.CustomValidate = null;
            this.panelParameter.FillColorEnd = System.Drawing.Color.Empty;
            this.panelParameter.FillColorStart = System.Drawing.Color.Empty;
            this.panelParameter.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.panelParameter.FillStyle = Sheng.SailingEase.Controls.FillStyle.Solid;
            this.panelParameter.HighLight = false;
            this.panelParameter.Location = new System.Drawing.Point(171, 84);
            this.panelParameter.Name = "panelParameter";
            this.panelParameter.ShowBorder = false;
            this.panelParameter.Size = new System.Drawing.Size(527, 322);
            this.panelParameter.TabIndex = 4;
            this.panelParameter.Title = null;
            // 
            // seLine1
            // 
            this.seLine1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.seLine1.Location = new System.Drawing.Point(168, 412);
            this.seLine1.Name = "seLine1";
            this.seLine1.Size = new System.Drawing.Size(530, 2);
            this.seLine1.TabIndex = 22;
            this.seLine1.Text = "seLine1";
            // 
            // sePanel1
            // 
            this.sePanel1.BackColor = System.Drawing.Color.White;
            this.sePanel1.BorderColor = System.Drawing.Color.Black;
            this.sePanel1.Controls.Add(this.availableEventTimes);
            this.sePanel1.Controls.Add(this.availableEvents);
            this.sePanel1.Controls.Add(this.lblEvent);
            this.sePanel1.Controls.Add(this.lblEventTime);
            this.sePanel1.CustomValidate = null;
            this.sePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.sePanel1.FillColorEnd = System.Drawing.Color.Empty;
            this.sePanel1.FillColorStart = System.Drawing.Color.Empty;
            this.sePanel1.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.sePanel1.FillStyle = Sheng.SailingEase.Controls.FillStyle.Solid;
            this.sePanel1.HighLight = false;
            this.sePanel1.Location = new System.Drawing.Point(0, 0);
            this.sePanel1.Name = "sePanel1";
            this.sePanel1.ShowBorder = false;
            this.sePanel1.Size = new System.Drawing.Size(710, 78);
            this.sePanel1.TabIndex = 23;
            this.sePanel1.Title = null;
            // 
            // availableEventTimes
            // 
            this.availableEventTimes.AllowEmpty = false;
            this.availableEventTimes.BackColor = System.Drawing.Color.White;
            this.availableEventTimes.CustomValidate = null;
            this.availableEventTimes.DescriptionMember = null;
            this.availableEventTimes.DisplayMember = null;
            this.availableEventTimes.HighLight = true;
            this.availableEventTimes.LayoutMode = Sheng.SailingEase.Controls.ListViewLayoutMode.Descriptive;
            this.availableEventTimes.Location = new System.Drawing.Point(523, 34);
            this.availableEventTimes.MaxItem = 8;
            this.availableEventTimes.Name = "availableEventTimes";
            this.availableEventTimes.Padding = new System.Windows.Forms.Padding(5);
            this.availableEventTimes.ShowDescription = false;
            this.availableEventTimes.Size = new System.Drawing.Size(175, 26);
            this.availableEventTimes.TabIndex = 20;
            seComboSelectorTheme1.ArrowColorEnd = System.Drawing.Color.LightGray;
            seComboSelectorTheme1.ArrowColorStart = System.Drawing.Color.Gray;
            seComboSelectorTheme1.BackColor = System.Drawing.Color.Gray;
            seComboSelectorTheme1.BackgroundColor = System.Drawing.Color.White;
            seComboSelectorTheme1.BorderColor = System.Drawing.Color.LightGray;
            seComboSelectorTheme1.DescriptionTextColor = System.Drawing.SystemColors.GrayText;
            seComboSelectorTheme1.HoveredBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(228)))), ((int)(((byte)(134)))));
            seComboSelectorTheme1.HoveredBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(202)))), ((int)(((byte)(88)))));
            seComboSelectorTheme1.HoveredDescriptionColor = System.Drawing.SystemColors.GrayText;
            seComboSelectorTheme1.HoveredTextColor = System.Drawing.SystemColors.WindowText;
            seComboSelectorTheme1.SelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
            seComboSelectorTheme1.SelectedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(138)))), ((int)(((byte)(48)))));
            seComboSelectorTheme1.SelectedDescriptionTextColor = System.Drawing.SystemColors.GrayText;
            seComboSelectorTheme1.SelectedTextColor = System.Drawing.SystemColors.WindowText;
            seComboSelectorTheme1.TextColor = System.Drawing.SystemColors.WindowText;
            this.availableEventTimes.Theme = seComboSelectorTheme1;
            this.availableEventTimes.Title = null;
            // 
            // availableEvents
            // 
            this.availableEvents.AllowEmpty = false;
            this.availableEvents.BackColor = System.Drawing.Color.White;
            this.availableEvents.CustomValidate = null;
            this.availableEvents.DescriptionMember = null;
            this.availableEvents.DisplayMember = null;
            this.availableEvents.HighLight = true;
            this.availableEvents.LayoutMode = Sheng.SailingEase.Controls.ListViewLayoutMode.Descriptive;
            this.availableEvents.Location = new System.Drawing.Point(124, 34);
            this.availableEvents.MaxItem = 8;
            this.availableEvents.Name = "availableEvents";
            this.availableEvents.Padding = new System.Windows.Forms.Padding(5);
            this.availableEvents.ShowDescription = false;
            this.availableEvents.Size = new System.Drawing.Size(278, 26);
            this.availableEvents.TabIndex = 19;
            seComboSelectorTheme2.ArrowColorEnd = System.Drawing.Color.LightGray;
            seComboSelectorTheme2.ArrowColorStart = System.Drawing.Color.Gray;
            seComboSelectorTheme2.BackColor = System.Drawing.Color.Gray;
            seComboSelectorTheme2.BackgroundColor = System.Drawing.Color.White;
            seComboSelectorTheme2.BorderColor = System.Drawing.Color.LightGray;
            seComboSelectorTheme2.DescriptionTextColor = System.Drawing.SystemColors.GrayText;
            seComboSelectorTheme2.HoveredBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(228)))), ((int)(((byte)(134)))));
            seComboSelectorTheme2.HoveredBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(202)))), ((int)(((byte)(88)))));
            seComboSelectorTheme2.HoveredDescriptionColor = System.Drawing.SystemColors.GrayText;
            seComboSelectorTheme2.HoveredTextColor = System.Drawing.SystemColors.WindowText;
            seComboSelectorTheme2.SelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(107)))));
            seComboSelectorTheme2.SelectedBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(138)))), ((int)(((byte)(48)))));
            seComboSelectorTheme2.SelectedDescriptionTextColor = System.Drawing.SystemColors.GrayText;
            seComboSelectorTheme2.SelectedTextColor = System.Drawing.SystemColors.WindowText;
            seComboSelectorTheme2.TextColor = System.Drawing.SystemColors.WindowText;
            this.availableEvents.Theme = seComboSelectorTheme2;
            this.availableEvents.Title = null;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Silver;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(710, 1);
            this.label1.TabIndex = 25;
            this.label1.Text = "label1";
            // 
            // FormEventEditor
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(710, 471);
            this.Controls.Add(this.treeViewParameter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.seLine1);
            this.Controls.Add(this.panelParameter);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.sePanel1);
            this.MinimumSize = new System.Drawing.Size(651, 479);
            this.Name = "FormEventEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${FormEventEditor}";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormEventSet_FormClosed);
            this.Load += new System.EventHandler(this.FormEventSet_Load);
            this.sePanel1.ResumeLayout(false);
            this.sePanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private System.Windows.Forms.Label lblEventTime;
        private System.Windows.Forms.Label lblEvent;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private Sheng.SailingEase.Controls.SENavigationTreeView treeViewParameter;
        private Sheng.SailingEase.Controls.SEPanel panelParameter;
        private Sheng.SailingEase.Controls.SELine seLine1;
        private System.Windows.Forms.ImageList imageListParameter;
        private Sheng.SailingEase.Controls.SEPanel sePanel1;
        private System.Windows.Forms.Label label1;
        private Controls.SEComboSelector2 availableEventTimes;
        private Controls.SEComboSelector2 availableEvents;
    }
}