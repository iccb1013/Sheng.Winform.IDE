namespace Sheng.SailingEase.Core.Development
{
    partial class DataSourceEditView
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
            Sheng.SailingEase.Controls.SEComboSelectorTheme seComboSelectorTheme1 = new Sheng.SailingEase.Controls.SEComboSelectorTheme();
            Sheng.SailingEase.Controls.SEComboSelectorTheme seComboSelectorTheme2 = new Sheng.SailingEase.Controls.SEComboSelectorTheme();
            this.btnOK = new Sheng.SailingEase.Controls.SEButton();
            this.btnCancel = new Sheng.SailingEase.Controls.SEButton();
            this.provideSelector = new Sheng.SailingEase.Controls.SEComboSelector2();
            this.dataSourceSelector = new Sheng.SailingEase.Controls.SEComboSelector2();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(298, 180);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 51;
            this.btnOK.Text = "${DataSourceEditView_ButtonOK}";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(379, 180);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 52;
            this.btnCancel.Text = "${DataSourceEditView_ButtonCancel}";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // provideSelector
            // 
            this.provideSelector.AllowEmpty = false;
            this.provideSelector.BackColor = System.Drawing.Color.White;
            this.provideSelector.CustomValidate = null;
            this.provideSelector.DescriptionMember = null;
            this.provideSelector.DisplayMember = "Name";
            this.provideSelector.HighLight = true;
            this.provideSelector.LayoutMode = Sheng.SailingEase.Controls.ListViewLayoutMode.Standard;
            this.provideSelector.Location = new System.Drawing.Point(30, 47);
            this.provideSelector.MaxItem = 5;
            this.provideSelector.Name = "provideSelector";
            this.provideSelector.Padding = new System.Windows.Forms.Padding(5);
            this.provideSelector.ShowDescription = false;
            this.provideSelector.Size = new System.Drawing.Size(379, 26);
            this.provideSelector.TabIndex = 53;
            this.provideSelector.Text = "seComboSelector21";
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
            this.provideSelector.Theme = seComboSelectorTheme1;
            this.provideSelector.Title = null;
            this.provideSelector.SelectedValueChanged += new System.EventHandler<Sheng.SailingEase.Controls.SEComboSelector2.OnSelectedValueChangedEventArgs>(this.provideSelector_SelectedValueChanged);
            // 
            // dataSourceSelector
            // 
            this.dataSourceSelector.AllowEmpty = false;
            this.dataSourceSelector.BackColor = System.Drawing.Color.White;
            this.dataSourceSelector.CustomValidate = null;
            this.dataSourceSelector.DescriptionMember = null;
            this.dataSourceSelector.DisplayMember = "Name";
            this.dataSourceSelector.HighLight = true;
            this.dataSourceSelector.LayoutMode = Sheng.SailingEase.Controls.ListViewLayoutMode.Standard;
            this.dataSourceSelector.Location = new System.Drawing.Point(30, 111);
            this.dataSourceSelector.MaxItem = 5;
            this.dataSourceSelector.Name = "dataSourceSelector";
            this.dataSourceSelector.Padding = new System.Windows.Forms.Padding(5);
            this.dataSourceSelector.ShowDescription = false;
            this.dataSourceSelector.Size = new System.Drawing.Size(379, 26);
            this.dataSourceSelector.TabIndex = 54;
            this.dataSourceSelector.Text = "seComboSelector22";
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
            this.dataSourceSelector.Theme = seComboSelectorTheme2;
            this.dataSourceSelector.Title = null;
            this.dataSourceSelector.ItemTextGetting += new System.EventHandler<Sheng.SailingEase.Controls.SEComboSelector2.ItemTextGettingEventArgs>(this.dataSourceSelector_ItemTextGetting);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(209, 12);
            this.label1.TabIndex = 55;
            this.label1.Text = "${DataSourceEditView_LabelProvide}";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(227, 12);
            this.label2.TabIndex = 56;
            this.label2.Text = "${DataSourceEditView_LabelDataSource}";
            // 
            // DataSourceEditView
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(466, 215);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataSourceSelector);
            this.Controls.Add(this.provideSelector);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataSourceEditView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "${DataSourceEditView}";
            this.Load += new System.EventHandler(this.DataSourceEditView_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.SEButton btnOK;
        private Controls.SEButton btnCancel;
        private Controls.SEComboSelector2 provideSelector;
        private Controls.SEComboSelector2 dataSourceSelector;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}