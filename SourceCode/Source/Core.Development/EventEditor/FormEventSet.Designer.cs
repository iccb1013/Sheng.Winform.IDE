namespace Sheng.SIMBE.IDE.UI.EventSet
{
    partial class FormEventSet
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
            this.btnCancel = new Sheng.SIMBE.SEControl.SEButton();
            this.ddlEvent = new Sheng.SIMBE.SEControl.SEComboBox();
            this.ddlEventTime = new Sheng.SIMBE.SEControl.SEComboBox();
            this.lblEventTime = new System.Windows.Forms.Label();
            this.lblEvent = new System.Windows.Forms.Label();
            this.btnOK = new Sheng.SIMBE.SEControl.SEButton();
            this.treeViewParameter = new System.Windows.Forms.TreeView();
            this.imageListParameter = new System.Windows.Forms.ImageList(this.components);
            this.panelParameter = new Sheng.SIMBE.SEControl.SEPanel();
            this.seLine1 = new Sheng.SIMBE.SEControl.SELine();
            this.sePanel1 = new Sheng.SIMBE.SEControl.SEPanel();
            this.seAdvLabel1 = new Sheng.SIMBE.SEControl.SEAdvLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.sePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(556, 410);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "${FormEventSet_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // ddlEvent
            // 
            this.ddlEvent.AllowEmpty = false;
            this.ddlEvent.DisplayMember = "Text";
            this.ddlEvent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlEvent.FormattingEnabled = true;
            this.ddlEvent.HighLight = true;
            this.ddlEvent.Location = new System.Drawing.Point(125, 22);
            this.ddlEvent.MaxDropDownItems = 16;
            this.ddlEvent.Name = "ddlEvent";
            this.ddlEvent.Size = new System.Drawing.Size(214, 20);
            this.ddlEvent.TabIndex = 1;
            this.ddlEvent.Title = "LabelEvent";
            this.ddlEvent.ValueMember = "Value";
            this.ddlEvent.WaterText = "";
            this.ddlEvent.SelectedIndexChanged += new System.EventHandler(this.ddlEvent_SelectedIndexChanged);
            // 
            // ddlEventTime
            // 
            this.ddlEventTime.AllowEmpty = false;
            this.ddlEventTime.DisplayMember = "Text";
            this.ddlEventTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlEventTime.FormattingEnabled = true;
            this.ddlEventTime.HighLight = true;
            this.ddlEventTime.Location = new System.Drawing.Point(469, 22);
            this.ddlEventTime.Name = "ddlEventTime";
            this.ddlEventTime.Size = new System.Drawing.Size(136, 20);
            this.ddlEventTime.TabIndex = 2;
            this.ddlEventTime.Title = "LabelEventTime";
            this.ddlEventTime.ValueMember = "Value";
            this.ddlEventTime.WaterText = "";
            // 
            // lblEventTime
            // 
            this.lblEventTime.AutoSize = true;
            this.lblEventTime.Location = new System.Drawing.Point(388, 25);
            this.lblEventTime.Name = "lblEventTime";
            this.lblEventTime.Size = new System.Drawing.Size(191, 12);
            this.lblEventTime.TabIndex = 16;
            this.lblEventTime.Text = "${FormEventSet_LabelEventTime}:";
            // 
            // lblEvent
            // 
            this.lblEvent.AutoSize = true;
            this.lblEvent.Location = new System.Drawing.Point(37, 25);
            this.lblEvent.Name = "lblEvent";
            this.lblEvent.Size = new System.Drawing.Size(167, 12);
            this.lblEvent.TabIndex = 15;
            this.lblEvent.Text = "${FormEventSet_LabelEvent}:";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(475, 410);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "${FormEventSet_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // treeViewParameter
            // 
            this.treeViewParameter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewParameter.HideSelection = false;
            this.treeViewParameter.ImageIndex = 0;
            this.treeViewParameter.ImageList = this.imageListParameter;
            this.treeViewParameter.Location = new System.Drawing.Point(12, 80);
            this.treeViewParameter.Name = "treeViewParameter";
            this.treeViewParameter.SelectedImageIndex = 0;
            this.treeViewParameter.ShowLines = false;
            this.treeViewParameter.ShowPlusMinus = false;
            this.treeViewParameter.ShowRootLines = false;
            this.treeViewParameter.Size = new System.Drawing.Size(150, 308);
            this.treeViewParameter.TabIndex = 3;
            this.treeViewParameter.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewParameter_AfterSelect);
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
            this.panelParameter.HighLight = false;
            this.panelParameter.Location = new System.Drawing.Point(171, 80);
            this.panelParameter.Name = "panelParameter";
            this.panelParameter.Size = new System.Drawing.Size(460, 300);
            this.panelParameter.TabIndex = 4;
            // 
            // seLine1
            // 
            this.seLine1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.seLine1.Location = new System.Drawing.Point(168, 386);
            this.seLine1.Name = "seLine1";
            this.seLine1.Size = new System.Drawing.Size(463, 2);
            this.seLine1.TabIndex = 22;
            this.seLine1.Text = "seLine1";
            // 
            // sePanel1
            // 
            this.sePanel1.BackColor = System.Drawing.Color.White;
            this.sePanel1.Controls.Add(this.lblEvent);
            this.sePanel1.Controls.Add(this.lblEventTime);
            this.sePanel1.Controls.Add(this.ddlEvent);
            this.sePanel1.Controls.Add(this.ddlEventTime);
            this.sePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.sePanel1.HighLight = false;
            this.sePanel1.Location = new System.Drawing.Point(0, 0);
            this.sePanel1.Name = "sePanel1";
            this.sePanel1.Size = new System.Drawing.Size(643, 45);
            this.sePanel1.TabIndex = 23;
            // 
            // seAdvLabel1
            // 
            this.seAdvLabel1.BorderColor = System.Drawing.Color.Black;
            this.seAdvLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.seAdvLabel1.FillColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.seAdvLabel1.FillColorStart = System.Drawing.Color.White;
            this.seAdvLabel1.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.seAdvLabel1.FillStyle = Sheng.SIMBE.SEControl.FillStyle.LinearGradient;
            this.seAdvLabel1.Location = new System.Drawing.Point(0, 45);
            this.seAdvLabel1.Name = "seAdvLabel1";
            this.seAdvLabel1.ShowBorder = false;
            this.seAdvLabel1.SingleLine = false;
            this.seAdvLabel1.Size = new System.Drawing.Size(643, 20);
            this.seAdvLabel1.TabIndex = 24;
            this.seAdvLabel1.TextHorizontalCenter = false;
            this.seAdvLabel1.TextVerticalCenter = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Silver;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(643, 1);
            this.label1.TabIndex = 25;
            this.label1.Text = "label1";
            // 
            // FormEventSet
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(643, 445);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.seAdvLabel1);
            this.Controls.Add(this.seLine1);
            this.Controls.Add(this.panelParameter);
            this.Controls.Add(this.treeViewParameter);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.sePanel1);
            this.MinimumSize = new System.Drawing.Size(651, 479);
            this.Name = "FormEventSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${FormEventSet}";
            this.Load += new System.EventHandler(this.FormEventSet_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormEventSet_FormClosed);
            this.sePanel1.ResumeLayout(false);
            this.sePanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Sheng.SIMBE.SEControl.SEButton btnCancel;
        private Sheng.SIMBE.SEControl.SEComboBox ddlEvent;
        private Sheng.SIMBE.SEControl.SEComboBox ddlEventTime;
        private System.Windows.Forms.Label lblEventTime;
        private System.Windows.Forms.Label lblEvent;
        private Sheng.SIMBE.SEControl.SEButton btnOK;
        private System.Windows.Forms.TreeView treeViewParameter;
        private Sheng.SIMBE.SEControl.SEPanel panelParameter;
        private Sheng.SIMBE.SEControl.SELine seLine1;
        private System.Windows.Forms.ImageList imageListParameter;
        private Sheng.SIMBE.SEControl.SEPanel sePanel1;
        private Sheng.SIMBE.SEControl.SEAdvLabel seAdvLabel1;
        private System.Windows.Forms.Label label1;
    }
}