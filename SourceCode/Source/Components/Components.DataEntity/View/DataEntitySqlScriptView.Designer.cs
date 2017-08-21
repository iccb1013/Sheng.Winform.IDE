/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    partial class DataEntitySqlScriptView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataEntitySqlScriptView));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.txtSql = new Sheng.SailingEase.Controls.TextEditor.TextEditorControl();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(555, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSave.Text = "保存";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            this.txtSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSql.IsReadOnly = false;
            this.txtSql.Location = new System.Drawing.Point(0, 25);
            this.txtSql.Name = "txtSql";
            this.txtSql.ShowVRuler = false;
            this.txtSql.Size = new System.Drawing.Size(555, 370);
            this.txtSql.TabIndex = 1;
            this.saveFileDialog.DefaultExt = "sql";
            this.saveFileDialog.Filter = "Sql 文件|*.sql|所有文件|*.*";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtSql);
            this.Controls.Add(this.toolStrip1);
            this.Name = "UserControlDataEntityCreateSql";
            this.Size = new System.Drawing.Size(555, 395);
            this.Load += new System.EventHandler(this.UserControlDataEntityCreateSql_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.ToolStrip toolStrip1;
        private Sheng.SailingEase.Controls.TextEditor.TextEditorControl txtSql;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}
