using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApplication2
{
    class RichToolStripMenuItem : ToolStripControlHost
    {
        public RichToolStripMenuItem()
            : base(new RichToolStripMenuItemControl2())
        {
            
        }
    }
}
