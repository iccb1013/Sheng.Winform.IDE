/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;
namespace Sheng.SailingEase.Controls
{
    [LicenseProvider(typeof(SEControlLicenseProvider))]
    public partial class SEStackView : UserControl
    {
        public SEStackView()
        {
            LicenseManager.Validate(typeof(SEStackView)); 
            InitializeComponent();
            this.stackStrip.Renderer = new SEStackRenderer();
        }
        internal static Bitmap DeserializeFromBase64(string data)
        {
            MemoryStream stream =
                new MemoryStream(Convert.FromBase64String(data));
            Bitmap b = new Bitmap(stream);
            return b;
        }
    }
    internal class SEStackRenderer : ToolStripProfessionalRenderer
    {
        private static Bitmap titleBarGripBmp;
        private static string titleBarGripEnc = @"Qk16AQAAAAAAADYAAAAoAAAAIwAAAAMAAAABABgAAAAAAAAAAADEDgAAxA4AAAAAAAAAAAAAuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5uGMyuGMy+/n5+/n5ANj+RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5wm8/RzIomHRh+/n5ANj+RzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMzHtMRzIoRzIozHtMANj+";
        private static Color titlebarColor1 = Color.FromArgb(89, 135, 214);
        private static Color titlebarColor2 = Color.FromArgb(76, 123, 204);
        private static Color titlebarColor3 = Color.FromArgb(63, 111, 194);
        private static Color titlebarColor4 = Color.FromArgb(50, 99, 184);
        private static Color titlebarColor5 = Color.FromArgb(38, 88, 174);
        private static Color titlebarColor6 = Color.FromArgb(25, 76, 164);
        private static Color titlebarColor7 = Color.FromArgb(12, 64, 154);
        private static Color borderColor = Color.FromArgb(0, 0, 128);
        static SEStackRenderer()
        {
            titleBarGripBmp = SEStackView.DeserializeFromBase64(titleBarGripEnc);
        }
        public SEStackRenderer()
        {
        }
        private void DrawTitleBar(Graphics g, Rectangle rect)
        {
            Image titlebarGrip = titleBarGripBmp;
            g.DrawImage(
                titlebarGrip,
                new Point(rect.X + ((rect.Width / 2) - (titlebarGrip.Width / 2)),
                rect.Y + 1));
        }
        protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
        {
            DrawTitleBar(
                e.Graphics,
                new Rectangle(0, 0, e.ToolStrip.Width, 7));
        }
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            DrawTitleBar(
                e.Graphics,
                new Rectangle(0, 0, e.ToolStrip.Width, 7));
        }
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(Point.Empty, e.Item.Size);
            Color gradientBegin = Color.FromArgb(203, 225, 252);
            Color gradientEnd = Color.FromArgb(125, 165, 224);
            ToolStripButton button = e.Item as ToolStripButton;
            if (button.Pressed || button.Checked)
            {
                gradientBegin = Color.FromArgb(254, 128, 62);
                gradientEnd = Color.FromArgb(255, 223, 154);
            }
            else if (button.Selected)
            {
                gradientBegin = Color.FromArgb(255, 255, 222);
                gradientEnd = Color.FromArgb(255, 203, 136);
            }
            using (Brush b = new LinearGradientBrush(
                bounds,
                gradientBegin,
                gradientEnd,
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(b, bounds);
            }
            e.Graphics.DrawRectangle(
                SystemPens.ControlDarkDark,
                bounds);
            g.DrawLine(
                SystemPens.ControlDarkDark,
                bounds.X,
                bounds.Y,
                bounds.Width - 1,
                bounds.Y);
            g.DrawLine(
                SystemPens.ControlDarkDark,
                bounds.X,
                bounds.Y,
                bounds.X,
                bounds.Height - 1);
            ToolStrip toolStrip = button.Owner;
            ToolStripButton nextItem = button.Owner.GetItemAt(
                button.Bounds.X,
                button.Bounds.Bottom + 1) as ToolStripButton;
            if (nextItem == null)
            {
                g.DrawLine(
                    SystemPens.ControlDarkDark,
                    bounds.X,
                    bounds.Height - 1,
                    bounds.X + bounds.Width - 1,
                    bounds.Height - 1);
            }
        }
    }
}
