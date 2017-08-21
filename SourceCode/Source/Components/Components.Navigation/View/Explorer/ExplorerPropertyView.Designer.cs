/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    partial class ExplorerPropertyView
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
            this.propertyGrid = new Sheng.SailingEase.ComponentModel.Design.PropertyGridPad();
            this.SuspendLayout();
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.ReadOnly = false;
            this.propertyGrid.SelectedObjects = null;
            this.propertyGrid.ShowDescription = false;
            this.propertyGrid.Size = new System.Drawing.Size(284, 262);
            this.propertyGrid.TabIndex = 1;
            this.propertyGrid.Verbs = null;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.propertyGrid);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "ExplorerPropertyView";
            this.Text = "ExplorerPropertyView";
            this.ResumeLayout(false);
        }
        private Sheng.SailingEase.ComponentModel.Design.PropertyGridPad propertyGrid;
    }
}
