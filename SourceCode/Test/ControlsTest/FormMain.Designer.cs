namespace ControlsTest
{
    partial class FormMain
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
            this.btnDockTest = new System.Windows.Forms.Button();
            this.btnListViewTest = new System.Windows.Forms.Button();
            this.btnComboSelectorTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDockTest
            // 
            this.btnDockTest.Location = new System.Drawing.Point(12, 12);
            this.btnDockTest.Name = "btnDockTest";
            this.btnDockTest.Size = new System.Drawing.Size(145, 23);
            this.btnDockTest.TabIndex = 0;
            this.btnDockTest.Text = "DockTest";
            this.btnDockTest.UseVisualStyleBackColor = true;
            this.btnDockTest.Click += new System.EventHandler(this.btnDockTest_Click);
            // 
            // btnListViewTest
            // 
            this.btnListViewTest.Location = new System.Drawing.Point(12, 41);
            this.btnListViewTest.Name = "btnListViewTest";
            this.btnListViewTest.Size = new System.Drawing.Size(145, 23);
            this.btnListViewTest.TabIndex = 1;
            this.btnListViewTest.Text = "ListViewTest";
            this.btnListViewTest.UseVisualStyleBackColor = true;
            this.btnListViewTest.Click += new System.EventHandler(this.btnListViewTest_Click);
            // 
            // btnComboSelectorTest
            // 
            this.btnComboSelectorTest.Location = new System.Drawing.Point(12, 70);
            this.btnComboSelectorTest.Name = "btnComboSelectorTest";
            this.btnComboSelectorTest.Size = new System.Drawing.Size(145, 23);
            this.btnComboSelectorTest.TabIndex = 2;
            this.btnComboSelectorTest.Text = "ComboSelectorTest";
            this.btnComboSelectorTest.UseVisualStyleBackColor = true;
            this.btnComboSelectorTest.Click += new System.EventHandler(this.btnComboSelectorTest_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.btnComboSelectorTest);
            this.Controls.Add(this.btnListViewTest);
            this.Controls.Add(this.btnDockTest);
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDockTest;
        private System.Windows.Forms.Button btnListViewTest;
        private System.Windows.Forms.Button btnComboSelectorTest;
    }
}