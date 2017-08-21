/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Windows.Forms.Development
{
    partial class UserControlSEComboBoxExDevDataRule_Enum
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
            this.btnBrowseEnum = new Sheng.SailingEase.Controls.SEButton();
            this.txtEnumName = new Sheng.SailingEase.Controls.SETextBox();
            this.SuspendLayout();
            this.btnBrowseEnum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseEnum.Location = new System.Drawing.Point(378, 3);
            this.btnBrowseEnum.Name = "btnBrowseEnum";
            this.btnBrowseEnum.Size = new System.Drawing.Size(31, 23);
            this.btnBrowseEnum.TabIndex = 3;
            this.btnBrowseEnum.Text = "...";
            this.btnBrowseEnum.UseVisualStyleBackColor = true;
            this.btnBrowseEnum.Click += new System.EventHandler(this.btnBrowseEnum_Click);
            this.txtEnumName.AllowEmpty = false;
            this.txtEnumName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEnumName.HighLight = true;
            this.txtEnumName.Location = new System.Drawing.Point(3, 3);
            this.txtEnumName.Name = "txtEnumName";
            this.txtEnumName.ReadOnly = true;
            this.txtEnumName.Regex = "";
            this.txtEnumName.RegexMsg = null;
            this.txtEnumName.Size = new System.Drawing.Size(369, 21);
            this.txtEnumName.TabIndex = 2;
            this.txtEnumName.Title = "TextBoxEnumNameTitle";
            this.txtEnumName.ValueCompareTo = null;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnBrowseEnum);
            this.Controls.Add(this.txtEnumName);
            this.Name = "SEComboBoxExDevDataRule_Enum";
            this.Size = new System.Drawing.Size(412, 226);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Sheng.SailingEase.Controls.SEButton btnBrowseEnum;
        private Sheng.SailingEase.Controls.SETextBox txtEnumName;
    }
}
