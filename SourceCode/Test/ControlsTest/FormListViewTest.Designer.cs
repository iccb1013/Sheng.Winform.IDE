namespace ControlsTest
{
    partial class FormListViewTest
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
            Sheng.SailingEase.Controls.SEListViewTheme seListViewTheme2 = new Sheng.SailingEase.Controls.SEListViewTheme();
            this.seListView1 = new Sheng.SailingEase.Controls.SEListView();
            this.SuspendLayout();
            // 
            // seListView1
            // 
            this.seListView1.AllowMultiSelection = false;
            this.seListView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.seListView1.LayoutMode = Sheng.SailingEase.Controls.ListViewLayoutMode.Standard;
            this.seListView1.Location = new System.Drawing.Point(38, 38);
            this.seListView1.Name = "seListView1";
            this.seListView1.Padding = new System.Windows.Forms.Padding(10);
            this.seListView1.Size = new System.Drawing.Size(255, 223);
            this.seListView1.TabIndex = 0;
            this.seListView1.Text = "seListView1";
            seListViewTheme2.BackColor = System.Drawing.SystemColors.Window;
            seListViewTheme2.HoverColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            seListViewTheme2.HoverColorStart = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            seListViewTheme2.ImageInnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            seListViewTheme2.ImageOuterBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            seListViewTheme2.ItemBackColor = System.Drawing.SystemColors.Window;
            seListViewTheme2.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            seListViewTheme2.ItemDescriptioniColor = System.Drawing.SystemColors.GrayText;
            seListViewTheme2.ItemHeaderColor = System.Drawing.SystemColors.WindowText;
            seListViewTheme2.ItemHeaderFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            seListViewTheme2.SelectedColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            seListViewTheme2.SelectedColorStart = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            seListViewTheme2.SelectionRectangleBorderColor = System.Drawing.SystemColors.Highlight;
            seListViewTheme2.SelectionRectangleColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            seListViewTheme2.UnFocusedColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            seListViewTheme2.UnFocusedColorStart = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.seListView1.Theme = seListViewTheme2;
            // 
            // FormListViewTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 327);
            this.Controls.Add(this.seListView1);
            this.Name = "FormListViewTest";
            this.Text = "FormListViewTest";
            this.Load += new System.EventHandler(this.FormListViewTest_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Sheng.SailingEase.Controls.SEListView seListView1;
    }
}