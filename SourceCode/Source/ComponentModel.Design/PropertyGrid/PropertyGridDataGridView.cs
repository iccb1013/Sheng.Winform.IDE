/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.ComponentModel.Design
{
    [ToolboxItem(false)]
    class PropertyGridDataGridView : DataGridView
    {
        private bool _selectedObjectSeting = false;
        internal bool SelectedObjectSeting
        {
            get { return _selectedObjectSeting; }
        }
        public PropertyGridDataGridView()
            : base()
        {
            SetStyle(ControlStyles.DoubleBuffer |
                ControlStyles.AllPaintingInWmPaint | 
                ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
            this.RowHeadersVisible = true;
            this.RowHeadersWidth = 20;
            this.Columns.Add("PropertyName", "PropertyName");  
            this.Columns.Add("Description", "Description");
            this.Columns.Add("PropertyValue", "PropertyValue");
            this.Columns.Add("Property", "Property"); 
            this.Columns["PropertyName"].ReadOnly = true;
            this.Columns["Description"].Visible = false;
            this.Columns["PropertyValue"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            this.Columns["Property"].Visible = false;
        }
        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
        {
            base.OnRowPostPaint(e);
            Rectangle cellRectangle = new Rectangle
                (e.RowBounds.Location.X, e.RowBounds.Location.Y, this.RowHeadersWidth, this.Rows[e.RowIndex].Height);
            using (SolidBrush brush = new SolidBrush(SystemColors.ControlLight))
            {
                e.Graphics.FillRectangle(brush, cellRectangle);
            }
            ICollapseRow row = this.Rows[e.RowIndex] as ICollapseRow;
            if (row == null)
                return;
            if (row.SubRows != null && row.SubRows.Count != 0)
            {
                Rectangle rect = new Rectangle(e.RowBounds.Location.X + 4, e.RowBounds.Location.Y + 5, 9, 9);
                Image img = null;
                if (row.IsCollapse)
                {
                    img = IconsLibrary.Plus;
                }
                else
                {
                    img = IconsLibrary.Minus;
                }
                e.Graphics.DrawImage(img, rect);
            }
        }
        protected override void OnRowHeaderMouseClick(DataGridViewCellMouseEventArgs e)
        {
            base.OnRowHeaderMouseClick(e);
            ICollapseRow row = this.Rows[e.RowIndex] as ICollapseRow;
            if (row != null)
            {
                row.IsCollapse = !row.IsCollapse;
            }
        }
        protected override void OnCellMouseDoubleClick(DataGridViewCellMouseEventArgs e)
        {
            base.OnCellMouseDoubleClick(e);
            ICollapseRow row = this.Rows[e.RowIndex] as ICollapseRow;
            if (row != null)
            {
                row.IsCollapse = !row.IsCollapse;
            }
        }
        protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
        {
            base.OnRowsAdded(e);
            if (this.Rows[e.RowIndex] is CatalogRow)
            {
                CatalogRow row = (CatalogRow)this.Rows[e.RowIndex];
                row.IsCollapse = false;
            }
        }
        internal void BeginSelectedObject()
        {
            _selectedObjectSeting = true;
        }
        internal void EndSelectedObject()
        {
            _selectedObjectSeting = false;
        }
        public DataGridViewCell GetDescriptionCell(int rowIndex)
        {
            return this.Rows[rowIndex].Cells["Description"];
        }
        public DataGridViewCell GetPropertyNameCell(int rowIndex)
        {
            return this.Rows[rowIndex].Cells["PropertyName"];
        }
        public DataGridViewCell GetPropertyValueCell(int rowIndex)
        {
            return this.Rows[rowIndex].Cells["PropertyValue"];
        }
        public DataGridViewCell GetPropertyCell(int rowIndex)
        {
            return this.Rows[rowIndex].Cells["Property"];
        }
    }
}
