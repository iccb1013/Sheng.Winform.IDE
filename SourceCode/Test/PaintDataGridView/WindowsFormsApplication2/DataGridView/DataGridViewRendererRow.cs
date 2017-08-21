using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    class DataGridViewRendererRow : DataGridViewRow
    {
        private bool _hovered = false;
        public bool Hovered
        {
            get { return _hovered; }
            set { _hovered = value; }
        }
    }
}
