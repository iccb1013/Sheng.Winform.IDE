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
namespace Sheng.SailingEase.ComponentModel.Design
{
    class CatalogRow : DataGridViewRow, ICollapseRow
    {
        private string _catalogName;
        public string CatalogName
        {
            get { return this._catalogName; }
        }
        private List<PropertyGridRow> _subRows;
        public List<PropertyGridRow> SubRows
        {
            get
            {
                return _subRows;
            }
            private set
            {
                _subRows = value;
            }
        }
        private bool _isCollapse = true;
        public bool IsCollapse
        {
            get
            {
                return _isCollapse;
            }
            set
            {
                _isCollapse = value;
                if (value)
                {
                    HideSubRow();
                }
                else
                {
                    ShowSubRow();
                }
            }
        }
        public CatalogRow(string catalogName)
        {
            this._catalogName = catalogName;
            this.SubRows = new List<PropertyGridRow>();
            DataGridViewCell dcName = new DataGridViewTextBoxCell();
            dcName.Value = catalogName;
            this.Cells.Add(dcName);
            this.Cells.Add(new DataGridViewTextBoxCell());
            this.Cells.Add(new DataGridViewTextBoxCell());
            this.Cells.Add(new DataGridViewTextBoxCell());
            for (int i = 0; i < this.Cells.Count; i++)
            {
                this.Cells[i].ReadOnly = true;
                this.Cells[i].Style.BackColor = SystemColors.ControlLight;
                this.Cells[i].Style.ForeColor = Color.Black;
                this.Cells[i].Style.SelectionBackColor = SystemColors.ControlLight;
                this.Cells[i].Style.SelectionForeColor = Color.Black;
            }
            this.Height = 19;
        }
        private void ShowSubRow()
        {
            if (this.SubRows == null || this.SubRows.Count == 0)
            {
                return;
            }
            this.DataGridView.SuspendLayout();
            for (int i = 0; i < this.SubRows.Count; i++)
            {
                this.DataGridView.Rows.Insert(this.Index + 1 + i, this.SubRows[i]);
            }
            this.DataGridView.ResumeLayout();
        }
        private void HideSubRow()
        {
            if (this.SubRows == null || this.SubRows.Count == 0)
            {
                return;
            }
            this.DataGridView.SuspendLayout();
            for (int i = 0; i < this.SubRows.Count; i++)
            {
                if (this.SubRows[i].DataGridView == this.DataGridView)
                {
                    ICollapseRow collapseRow = this.SubRows[i] as ICollapseRow;
                    if (collapseRow != null)
                        collapseRow.IsCollapse = true;
                    this.DataGridView.Rows.Remove(this.SubRows[i]);
                }
            }
            this.DataGridView.ResumeLayout();
        }
    }
}
