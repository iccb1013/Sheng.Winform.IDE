// ImageListView - A listview control for image files
// Copyright (C) 2009 Ozgur Ozcitak
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Ozgur Ozcitak (ozcitak@yahoo.com)

#if DEBUG
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    /// <summary>
    /// Represents a renderer meant to be used for debugging purposes.
    /// Included in the debug build only.
    /// </summary>
    public class DebugRenderer : ImageListView.ImageListViewRenderer
    {
        private long baseMem;

        /// <summary>
        /// Initializes a new instance of the DebugRenderer class.
        /// </summary>
        public DebugRenderer()
        {
            Process p = Process.GetCurrentProcess();
            p.Refresh();
            baseMem = p.PrivateMemorySize64;
        }
        /// <summary>
        /// Draws the specified item on the given graphics.
        /// </summary>
        /// <param name="g">The System.Drawing.Graphics to draw on.</param>
        /// <param name="item">The ImageListViewItem to draw.</param>
        /// <param name="state">The current view state of item.</param>
        /// <param name="bounds">The bounding rectangle of item in client coordinates.</param>
        public override void DrawItem(Graphics g, ImageListViewItem item, ItemState state, Rectangle bounds)
        {
            if (item.Index == mImageListView.layoutManager.FirstPartiallyVisible ||
                item.Index == mImageListView.layoutManager.LastPartiallyVisible)
            {
                using (Brush b = new HatchBrush(HatchStyle.BackwardDiagonal, Color.Green, Color.Transparent))
                {
                    g.FillRectangle(b, bounds);
                }
            }
            if (item.Index == mImageListView.layoutManager.FirstVisible ||
                item.Index == mImageListView.layoutManager.LastVisible)
            {
                using (Brush b = new HatchBrush(HatchStyle.ForwardDiagonal, Color.Red, Color.Transparent))
                {
                    g.FillRectangle(b, bounds);
                }
            }

            base.DrawItem(g, item, state, bounds);
        }
        /// <summary>
        /// Draws an overlay image over the client area.
        /// </summary>
        /// <param name="g">The System.Drawing.Graphics to draw on.</param>
        /// <param name="bounds">The bounding rectangle of the client area.</param>
        public override void DrawOverlay(Graphics g, Rectangle bounds)
        {
            // Refresh process info
            Process p = Process.GetCurrentProcess();
            p.Refresh();
            long mem = Math.Max(0, p.PrivateMemorySize64 - baseMem);

            // Display memory stats
            string s = string.Format("Total: {0}\r\nCache: {1}\r\nCache*: {2}", Utility.FormatSize(baseMem), Utility.FormatSize(mem), Utility.FormatSize(mImageListView.cacheManager.MemoryUsed));
            SizeF sz = g.MeasureString(s, ImageListView.Font);
            Rectangle r = new Rectangle(ItemAreaBounds.Right - 120, ItemAreaBounds.Top + 5, 115, (int)sz.Height);
            using (Brush b = new SolidBrush(Color.FromArgb(220, Color.LightGray)))
            {
                g.FillRectangle(b, r);
            }
            using (Pen pen = new Pen(Color.FromArgb(128, Color.Gray)))
            {
                g.DrawRectangle(pen, r);
            }
            g.DrawString(s, ImageListView.Font, Brushes.Black, r.Location);

            // Display navigation parameters
            r = new Rectangle(ItemAreaBounds.Right - 120, ItemAreaBounds.Top + 5 + (int)sz.Height + 10, 115, 125);
            using (Brush b = new SolidBrush(Color.FromArgb(220, Color.LightGray)))
            {
                g.FillRectangle(b, r);
            }
            using (Pen pen = new Pen(Color.FromArgb(128, Color.Gray)))
            {
                g.DrawRectangle(pen, r);
            }

            // Is left button down?
            r = new Rectangle(r.Left + 5, r.Top + 5, 15, 15);
            if (mImageListView.navigationManager.LeftButton)
            {
                g.FillRectangle(Brushes.DarkGray, r);
            }
            g.DrawRectangle(Pens.Black, r);
            r.Offset(15, 0);
            r.Offset(15, 0);

            // Is right button down?
            if (mImageListView.navigationManager.RightButton)
            {
                g.FillRectangle(Brushes.DarkGray, r);
            }
            g.DrawRectangle(Pens.Black, r);
            r.Offset(-30, 22);

            // Is shift key down?
            Color tColor = Color.Gray;
            if (mImageListView.navigationManager.ShiftKey)
                tColor = Color.Black;
            using (Brush b = new SolidBrush(tColor))
            {
                g.DrawString("Shift", mImageListView.Font, b, r.Location);
            }
            r.Offset(0, 12);

            // Is control key down?
            tColor = Color.Gray;
            if (mImageListView.navigationManager.ControlKey)
                tColor = Color.Black;
            using (Brush b = new SolidBrush(tColor))
            {
                g.DrawString("Control", mImageListView.Font, b, r.Location);
            }
            r.Offset(0, 20);

            // Display hit test details for item area
            ImageListView.HitInfo h = null;
            mImageListView.HitTest(mImageListView.PointToClient(Control.MousePosition), out h);

            tColor = Color.Gray;
            if (h.InItemArea)
                tColor = Color.Black;
            using (Brush b = new SolidBrush(tColor))
            {
                g.DrawString("InItemArea (" + h.ItemIndex.ToString() + ")", mImageListView.Font, b, r.Location);
            }
            r.Offset(0, 12);

            // Display hit test details for column header area
            tColor = Color.Gray;
            if (h.InHeaderArea)
                tColor = Color.Black;
            using (Brush b = new SolidBrush(tColor))
            {
                g.DrawString("InHeaderArea (" + h.ColumnIndex.ToString() + ")", mImageListView.Font, b, r.Location);
            }
            r.Offset(0, 12);

            // Display hit test details for pane area
            tColor = Color.Gray;
            if (h.InPaneArea)
                tColor = Color.Black;
            using (Brush b = new SolidBrush(tColor))
            {
                g.DrawString("InPaneArea " + (h.PaneBorder ? " (Border)" : ""), mImageListView.Font, b, r.Location);
            }
            r.Offset(0, 12);
        }
    }
}
#endif