/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlEventEditorPanel_SaveFormData_SqlRegex
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
            this.lblNotice = new System.Windows.Forms.Label();
            this.btnGetSqlRegex = new Sheng.SailingEase.Controls.SEButton();
            this.txtSqlRegex = new SqlRegexIntelliSenseTextEditorControl();
            this.SuspendLayout();
            this.lblNotice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblNotice.AutoSize = true;
            this.lblNotice.Location = new System.Drawing.Point(3, 279);
            this.lblNotice.Name = "lblNotice";
            this.lblNotice.Size = new System.Drawing.Size(395, 12);
            this.lblNotice.TabIndex = 33;
            this.lblNotice.Text = "${UserControlEventEditorPanel_SaveFormData_SqlRegex_LabelNotice}";
            this.btnGetSqlRegex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetSqlRegex.Location = new System.Drawing.Point(362, 274);
            this.btnGetSqlRegex.Name = "btnGetSqlRegex";
            this.btnGetSqlRegex.Size = new System.Drawing.Size(95, 23);
            this.btnGetSqlRegex.TabIndex = 1;
            this.btnGetSqlRegex.Text = "${UserControlEventEditorPanel_SaveFormData_SqlRegex_ButtonGetSqlRegex}";
            this.btnGetSqlRegex.UseVisualStyleBackColor = true;
            this.btnGetSqlRegex.Click += new System.EventHandler(this.btnGetSqlRegex_Click);
            this.txtSqlRegex.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSqlRegex.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtSqlRegex.FormEntity = null;
            this.txtSqlRegex.IsReadOnly = false;
            this.txtSqlRegex.Location = new System.Drawing.Point(5, 29);
            this.txtSqlRegex.Name = "txtSqlRegex";
            this.txtSqlRegex.OwnerForm = null;
            this.txtSqlRegex.ShowLineNumbers = false;
            this.txtSqlRegex.ShowVRuler = false;
            this.txtSqlRegex.Size = new System.Drawing.Size(452, 239);
            this.txtSqlRegex.TabIndex = 0;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtSqlRegex);
            this.Controls.Add(this.btnGetSqlRegex);
            this.Controls.Add(this.lblNotice);
            this.Name = "UserControlEventEditorPanel_SaveFormData_SqlRegex";
            this.Controls.SetChildIndex(this.lblNotice, 0);
            this.Controls.SetChildIndex(this.btnGetSqlRegex, 0);
            this.Controls.SetChildIndex(this.txtSqlRegex, 0);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Label lblNotice;
        private Sheng.SailingEase.Controls.SEButton btnGetSqlRegex;
        private SqlRegexIntelliSenseTextEditorControl txtSqlRegex;
    }
}
