/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlAnchorEditingDropDown
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.scbRight = new Sheng.SailingEase.Controls.SESimpleCheckBox();
            this.scbLeft = new Sheng.SailingEase.Controls.SESimpleCheckBox();
            this.scbBottom = new Sheng.SailingEase.Controls.SESimpleCheckBox();
            this.scbTop = new Sheng.SailingEase.Controls.SESimpleCheckBox();
            this.btnAnchorAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            this.scbRight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.scbRight.Check = false;
            this.scbRight.Location = new System.Drawing.Point(95, 46);
            this.scbRight.Name = "scbRight";
            this.scbRight.Size = new System.Drawing.Size(25, 12);
            this.scbRight.TabIndex = 13;
            this.scbRight.Text = "seCheckBox4";
            this.scbLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.scbLeft.Check = false;
            this.scbLeft.Location = new System.Drawing.Point(13, 46);
            this.scbLeft.Name = "scbLeft";
            this.scbLeft.Size = new System.Drawing.Size(25, 12);
            this.scbLeft.TabIndex = 12;
            this.scbLeft.Text = "seCheckBox3";
            this.scbBottom.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.scbBottom.Check = false;
            this.scbBottom.Location = new System.Drawing.Point(60, 73);
            this.scbBottom.Name = "scbBottom";
            this.scbBottom.Size = new System.Drawing.Size(12, 25);
            this.scbBottom.TabIndex = 11;
            this.scbBottom.Text = "seCheckBox2";
            this.scbTop.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.scbTop.Check = false;
            this.scbTop.Location = new System.Drawing.Point(60, 6);
            this.scbTop.Name = "scbTop";
            this.scbTop.Size = new System.Drawing.Size(12, 25);
            this.scbTop.TabIndex = 10;
            this.scbTop.Text = "seCheckBox1";
            this.btnAnchorAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAnchorAll.Location = new System.Drawing.Point(44, 37);
            this.btnAnchorAll.Name = "btnAnchorAll";
            this.btnAnchorAll.Size = new System.Drawing.Size(45, 30);
            this.btnAnchorAll.TabIndex = 9;
            this.btnAnchorAll.UseVisualStyleBackColor = true;
            this.btnAnchorAll.Click += new System.EventHandler(this.btnAnchorAll_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scbRight);
            this.Controls.Add(this.scbLeft);
            this.Controls.Add(this.scbBottom);
            this.Controls.Add(this.scbTop);
            this.Controls.Add(this.btnAnchorAll);
            this.Name = "UserControlAnchorEditingDropDown";
            this.Size = new System.Drawing.Size(132, 104);
            this.ResumeLayout(false);
        }
        private Sheng.SailingEase.Controls.SESimpleCheckBox scbRight;
        private Sheng.SailingEase.Controls.SESimpleCheckBox scbLeft;
        private Sheng.SailingEase.Controls.SESimpleCheckBox scbBottom;
        private Sheng.SailingEase.Controls.SESimpleCheckBox scbTop;
        private System.Windows.Forms.Button btnAnchorAll;
    }
}
