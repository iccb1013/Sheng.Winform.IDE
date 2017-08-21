/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.RegexTool
{
    partial class FormRegexLib
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
            this.listViewRegexLib = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            this.listViewRegexLib.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewRegexLib.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewRegexLib.FullRowSelect = true;
            this.listViewRegexLib.Location = new System.Drawing.Point(12, 12);
            this.listViewRegexLib.MultiSelect = false;
            this.listViewRegexLib.Name = "listViewRegexLib";
            this.listViewRegexLib.Size = new System.Drawing.Size(528, 342);
            this.listViewRegexLib.TabIndex = 0;
            this.listViewRegexLib.UseCompatibleStateImageBehavior = false;
            this.listViewRegexLib.View = System.Windows.Forms.View.Details;
            this.listViewRegexLib.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewRegexLib_MouseDoubleClick);
            this.columnHeader1.Text = "名称";
            this.columnHeader1.Width = 90;
            this.columnHeader2.Text = "表达式";
            this.columnHeader2.Width = 200;
            this.columnHeader3.Text = "备注";
            this.columnHeader3.Width = 200;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 366);
            this.Controls.Add(this.listViewRegexLib);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRegexLib";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormRegexLib";
            this.Load += new System.EventHandler(this.FormRegexLib_Load);
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.ListView listViewRegexLib;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}
