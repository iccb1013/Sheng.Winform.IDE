using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ControlsTest
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnDockTest_Click(object sender, EventArgs e)
        {
            FormDockTest form = new FormDockTest();
            form.Show();
        }

        private void btnListViewTest_Click(object sender, EventArgs e)
        {
            FormListViewTest form = new FormListViewTest();
            form.Show();
        }

        private void btnComboSelectorTest_Click(object sender, EventArgs e)
        {
            FormSEComboSelectorTest form = new FormSEComboSelectorTest();
            form.Show();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Point _Left1 = new Point(0, 0);
            Point _Left2 = new Point(this.Width / 2, 0);
            Point _Left3 = new Point(this.Width / 2, this.Height);
            Point _Left4 = new Point(0, this.Height);

            Point[] _Point = new Point[] { _Left1, _Left2, _Left3, _Left4 };
            PathGradientBrush _SetBruhs = new PathGradientBrush(_Point, WrapMode.TileFlipX);
            _SetBruhs.CenterPoint = new PointF(0, 0);
            _SetBruhs.FocusScales = new PointF(0, this.Height);
            _SetBruhs.CenterColor = Color.Red;
            _SetBruhs.SurroundColors = new Color[] { Color.Blue };

            e.Graphics.FillRectangle(_SetBruhs, new Rectangle(0, 0, this.Width, this.Height));

            base.OnPaint(e);
        }
    }
}
