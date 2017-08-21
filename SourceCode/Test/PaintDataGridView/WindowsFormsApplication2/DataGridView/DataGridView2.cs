using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApplication2
{
    class DataGridView2 : DataGridView
    {
        public DataGridView2()
        {
            this.ReadOnly = true;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.RowHeadersVisible = false;
            this.AllowUserToAddRows = false;
            this.AllowUserToResizeRows = false;

            //this.BackgroundColor = Color.White;
            this.CellBorderStyle = DataGridViewCellBorderStyle.None;

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            DataGridViewRenderer renderer = new DataGridViewRenderer(this);

        }
    }
}
