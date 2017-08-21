namespace Sheng.SailingEase.Modules.ProjectModule.View
{
    partial class CreateProjectView
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
            this.treeViewProjectType = new System.Windows.Forms.TreeView();
            this.listViewTemplate = new System.Windows.Forms.ListView();
            this.seLine1 = new Sheng.SailingEase.Controls.SELine();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.txtFolder = new Sheng.SailingEase.Controls.SETextBox();
            this.btnBrowse = new Sheng.SailingEase.Controls.SEButton();
            this.label3 = new System.Windows.Forms.Label();
            this.txtProjectFileName = new Sheng.SailingEase.Controls.SETextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDescription = new Sheng.SailingEase.Controls.SETextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // treeViewProjectType
            // 
            this.treeViewProjectType.HideSelection = false;
            this.treeViewProjectType.Location = new System.Drawing.Point(12, 36);
            this.treeViewProjectType.Name = "treeViewProjectType";
            this.treeViewProjectType.Size = new System.Drawing.Size(156, 235);
            this.treeViewProjectType.TabIndex = 0;
            this.treeViewProjectType.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewProjectType_AfterSelect);
            // 
            // listViewTemplate
            // 
            this.listViewTemplate.HideSelection = false;
            this.listViewTemplate.Location = new System.Drawing.Point(174, 36);
            this.listViewTemplate.MultiSelect = false;
            this.listViewTemplate.Name = "listViewTemplate";
            this.listViewTemplate.Size = new System.Drawing.Size(409, 235);
            this.listViewTemplate.TabIndex = 1;
            this.listViewTemplate.UseCompatibleStateImageBehavior = false;
            this.listViewTemplate.SelectedIndexChanged += new System.EventHandler(this.listViewTemplate_SelectedIndexChanged);
            // 
            // seLine1
            // 
            this.seLine1.Location = new System.Drawing.Point(13, 357);
            this.seLine1.Name = "seLine1";
            this.seLine1.Size = new System.Drawing.Size(571, 2);
            this.seLine1.TabIndex = 2;
            this.seLine1.Text = "seLine1";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(508, 376);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "${CreateProjectView_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(427, 376);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "${CreateProjectView_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.AllowEmpty = false;
            this.txtFolder.HighLight = true;
            this.txtFolder.LimitMaxValue = false;
            this.txtFolder.Location = new System.Drawing.Point(76, 330);
            this.txtFolder.MaxValue = ((long)(2147483647));
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Regex = "";
            this.txtFolder.RegexMsg = "请确认路径输入是否有效";
            this.txtFolder.Size = new System.Drawing.Size(426, 21);
            this.txtFolder.TabIndex = 7;
            this.txtFolder.Title = "位置";
            this.txtFolder.ValueCompareTo = null;
            this.txtFolder.WaterText = "";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(508, 328);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 8;
            this.btnBrowse.Text = "${CreateProjectView_ButtonBrowse}";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 334);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(203, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "${CreateProjectView_LabelFolder}:";
            // 
            // txtProjectFileName
            // 
            this.txtProjectFileName.AllowEmpty = false;
            this.txtProjectFileName.HighLight = true;
            this.txtProjectFileName.LimitMaxValue = false;
            this.txtProjectFileName.Location = new System.Drawing.Point(76, 304);
            this.txtProjectFileName.MaxLength = 200;
            this.txtProjectFileName.MaxValue = ((long)(2147483647));
            this.txtProjectFileName.Name = "txtProjectFileName";
            this.txtProjectFileName.Regex = "";
            this.txtProjectFileName.RegexMsg = null;
            this.txtProjectFileName.Size = new System.Drawing.Size(426, 21);
            this.txtProjectFileName.TabIndex = 5;
            this.txtProjectFileName.Title = "名称";
            this.txtProjectFileName.ValueCompareTo = null;
            this.txtProjectFileName.WaterText = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 307);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(215, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "${CreateProjectView_LabelFileName}:";
            // 
            // txtDescription
            // 
            this.txtDescription.AllowEmpty = true;
            this.txtDescription.HighLight = true;
            this.txtDescription.LimitMaxValue = false;
            this.txtDescription.Location = new System.Drawing.Point(12, 277);
            this.txtDescription.MaxValue = ((long)(2147483647));
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Regex = "";
            this.txtDescription.RegexMsg = null;
            this.txtDescription.Size = new System.Drawing.Size(571, 21);
            this.txtDescription.TabIndex = 10;
            this.txtDescription.Title = null;
            this.txtDescription.ValueCompareTo = null;
            this.txtDescription.WaterText = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "${CreateProjectView_LabelProjectType}:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(172, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(215, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "${CreateProjectView_LabelTemplate}:";
            // 
            // FormCreateProject
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(595, 411);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtProjectFileName);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.seLine1);
            this.Controls.Add(this.listViewTemplate);
            this.Controls.Add(this.treeViewProjectType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCreateProject";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${CreateProjectView}";
            this.Load += new System.EventHandler(this.CreateProjectView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewProjectType;
        private System.Windows.Forms.ListView listViewTemplate;
        private Sheng.SailingEase.Controls.SELine seLine1;
        private Sheng.SailingEase.Controls.SEButton btnCancel;
        private Sheng.SailingEase.Controls.SEButton btnOK;
        private Sheng.SailingEase.Controls.SETextBox txtFolder;
        private Sheng.SailingEase.Controls.SEButton btnBrowse;
        private System.Windows.Forms.Label label3;
        private Sheng.SailingEase.Controls.SETextBox txtProjectFileName;
        private System.Windows.Forms.Label label2;
        private Sheng.SailingEase.Controls.SETextBox txtDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
    }
}