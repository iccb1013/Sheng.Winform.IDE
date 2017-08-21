using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Controls.Docking;

namespace ControlsTest
{
    public partial class FormDockTest : Form
    {
        public FormDockTest()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DockContent dc = new DockContent();
            dc.TabText = "DockContent";
            dc.Show(this.dockPanel1);

            //dc = new DockContent();
            //dc.TabText = "DockContent";
            //dc.Show(this.dockPanel1);

            //dc = new DockContent();
            //dc.TabText = "DockContent";
            //dc.Show(this.dockPanel1);
        }
    }
}
