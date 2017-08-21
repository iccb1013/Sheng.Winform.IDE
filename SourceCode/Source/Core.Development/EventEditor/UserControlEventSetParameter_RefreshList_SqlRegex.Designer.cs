/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SIMBE.IDE.UI.EventSet
{
    partial class UserControlEventSetParameter_RefreshList_SqlRegex
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
            this.lblTitle = new Sheng.SIMBE.SEControl.SEAdvLabel();
            this.txtSqlRegex = new Sheng.SIMBE.SEControl.TextEditor.TextEditorControl();
            this.lblNotice = new System.Windows.Forms.Label();
            this.btnGetSqlRegex = new Sheng.SIMBE.SEControl.SEButton();
            this.SuspendLayout();
            this.lblTitle.BorderColor = System.Drawing.Color.Black;
            this.lblTitle.FillColorEnd = System.Drawing.Color.Empty;
            this.lblTitle.FillColorStart = System.Drawing.Color.White;
            this.lblTitle.FillMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.lblTitle.FillStyle = Sheng.SIMBE.SEControl.FillStyle.LinearGradient;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.ShowBorder = false;
            this.lblTitle.SingleLine = true;
            this.lblTitle.Size = new System.Drawing.Size(460, 23);
            this.lblTitle.TabIndex = 21;
            this.lblTitle.Text = " LabelTitle";
            this.lblTitle.TextHorizontalCenter = false;
            this.lblTitle.TextVerticalCenter = true;
            this.txtSqlRegex.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSqlRegex.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtSqlRegex.IsReadOnly = false;
            this.txtSqlRegex.Location = new System.Drawing.Point(3, 35);
            this.txtSqlRegex.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtSqlRegex.Name = "txtSqlRegex";
            this.txtSqlRegex.Size = new System.Drawing.Size(454, 230);
            this.txtSqlRegex.TabIndex = 22;
            this.lblNotice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblNotice.AutoSize = true;
            this.lblNotice.Location = new System.Drawing.Point(3, 279);
            this.lblNotice.Name = "lblNotice";
            this.lblNotice.Size = new System.Drawing.Size(71, 12);
            this.lblNotice.TabIndex = 24;
            this.lblNotice.Text = "LabelNotice";
            this.btnGetSqlRegex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetSqlRegex.Location = new System.Drawing.Point(362, 274);
            this.btnGetSqlRegex.Name = "btnGetSqlRegex";
            this.btnGetSqlRegex.Size = new System.Drawing.Size(95, 23);
            this.btnGetSqlRegex.TabIndex = 23;
            this.btnGetSqlRegex.Text = "ButtonGetSqlRegex";
            this.btnGetSqlRegex.UseVisualStyleBackColor = true;
            this.btnGetSqlRegex.Click += new System.EventHandler(this.btnGetSqlRegex_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblNotice);
            this.Controls.Add(this.btnGetSqlRegex);
            this.Controls.Add(this.txtSqlRegex);
            this.Controls.Add(this.lblTitle);
            this.Name = "UserControlEventSetParameter_RefreshList_SqlRegex";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private Sheng.SIMBE.SEControl.SEAdvLabel lblTitle;
        private Sheng.SIMBE.SEControl.TextEditor.TextEditorControl txtSqlRegex;
        private System.Windows.Forms.Label lblNotice;
        private Sheng.SIMBE.SEControl.SEButton btnGetSqlRegex;
    }
}
