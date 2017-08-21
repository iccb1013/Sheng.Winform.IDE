/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Core.Development
{
    partial class UserControlWarningView
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
            this.components = new System.ComponentModel.Container();
            this.treeListView = new Sheng.SailingEase.Controls.ObjectListView.TreeListView();
            this.treeColumnName = new Sheng.SailingEase.Controls.ObjectListView.OLVColumn();
            this.treeColumnIcon = new Sheng.SailingEase.Controls.ObjectListView.OLVColumn();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.treeListView)).BeginInit();
            this.SuspendLayout();
            this.treeListView.AllColumns.Add(this.treeColumnName);
            this.treeListView.AllColumns.Add(this.treeColumnIcon);
            this.treeListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.treeColumnName,
            this.treeColumnIcon});
            this.treeListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListView.GridLines = true;
            this.treeListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.treeListView.HideSelection = false;
            this.treeListView.Location = new System.Drawing.Point(0, 0);
            this.treeListView.MultiSelect = false;
            this.treeListView.Name = "treeListView";
            this.treeListView.OwnerDraw = true;
            this.treeListView.ShowGroups = false;
            this.treeListView.Size = new System.Drawing.Size(594, 344);
            this.treeListView.SmallImageList = this.imageList;
            this.treeListView.TabIndex = 0;
            this.treeListView.UseCompatibleStateImageBehavior = false;
            this.treeListView.View = System.Windows.Forms.View.Details;
            this.treeListView.VirtualMode = true;
            this.treeColumnName.AspectName = "";
            this.treeColumnName.FillsFreeSpace = true;
            this.treeColumnName.HeaderFont = null;
            this.treeColumnIcon.HeaderFont = null;
            this.treeColumnIcon.Width = 25;
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeListView);
            this.Name = "UserControlWarningView";
            this.Size = new System.Drawing.Size(594, 344);
            ((System.ComponentModel.ISupportInitialize)(this.treeListView)).EndInit();
            this.ResumeLayout(false);
        }
        private Sheng.SailingEase.Controls.ObjectListView.TreeListView treeListView;
        private Sheng.SailingEase.Controls.ObjectListView.OLVColumn treeColumnName;
        private System.Windows.Forms.ImageList imageList;
        private Sheng.SailingEase.Controls.ObjectListView.OLVColumn treeColumnIcon;
    }
}
