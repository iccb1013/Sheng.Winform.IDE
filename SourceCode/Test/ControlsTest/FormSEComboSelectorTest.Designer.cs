namespace ControlsTest
{
    partial class FormSEComboSelectorTest
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
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.seComboSelector21 = new Sheng.SailingEase.Controls.SEComboSelector2();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(87, 189);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(78, 16);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // seComboSelector21
            // 
            this.seComboSelector21.AllowEmpty = true;
            this.seComboSelector21.CustomValidate = null;
            this.seComboSelector21.DescriptionMember = null;
            this.seComboSelector21.DisplayMember = null;
            this.seComboSelector21.HighLight = true;
            this.seComboSelector21.Location = new System.Drawing.Point(22, 56);
            this.seComboSelector21.MaxItem = 5;
            this.seComboSelector21.Name = "seComboSelector21";
            this.seComboSelector21.Padding = new System.Windows.Forms.Padding(5);
            this.seComboSelector21.ShowDescription = false;
            this.seComboSelector21.Size = new System.Drawing.Size(204, 26);
            this.seComboSelector21.TabIndex = 0;
            this.seComboSelector21.Text = "seComboSelector21";
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
            this.seComboSelector21.Theme = seComboSelectorTheme1;
            this.seComboSelector21.Title = null;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(296, 76);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormSEComboSelectorTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 366);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.seComboSelector21);
            this.Name = "FormSEComboSelectorTest";
            this.Text = "FormSEComboSelectorTest";
            this.Load += new System.EventHandler(this.FormSEComboSelectorTest_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sheng.SailingEase.Controls.SEComboSelector2 seComboSelector21;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
    }
}