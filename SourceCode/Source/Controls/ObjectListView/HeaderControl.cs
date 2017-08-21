/*
 * HeaderControl - A limited implementation of HeaderControl
 *
 * Author: Phillip Piper
 * Date: 25/11/2008 17:15 
 *
 * Change log:
 * 2009-10-03  JPP  - Handle when ListView.HeaderStyle is None
 * 2009-08-24  JPP  - Handle the header being destroyed
 * v2.2.1
 * 2009-08-16  JPP  - Correctly handle header themes
 * 2009-08-15  JPP  - Added formatting capabilities: font, color, word wrap
 * v2.2
 * 2009-06-01  JPP  - Use ToolTipControl
 * 2009-05-10  JPP  - Removed all unsafe code
 * 2008-11-25  JPP  - Initial version
 *
 * TO DO:
 *
 * Copyright (C) 2006-2009 Phillip Piper
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 * If you wish to use this code in a closed source application, please contact phillip_piper@bigfoot.com.
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;

namespace Sheng.SailingEase.Controls.ObjectListView
{
    /// <summary>
    /// Class used to capture window messages for the header of the list view
    /// control.
    /// </summary>
    public class HeaderControl : NativeWindow
    {
        public HeaderControl(ObjectListView olv) {
            this.ListView = olv;
            this.AssignHandle(NativeMethods.GetHeaderControl(olv));
        }

        #region Properties

        /// <summary>
        /// Return the index of the column under the current cursor position,
        /// or -1 if the cursor is not over a column
        /// </summary>
        /// <returns>Index of the column under the cursor, or -1</returns>
        public int ColumnIndexUnderCursor {
            get {
                Point pt = this.ListView.PointToClient(Cursor.Position);
                pt.X += NativeMethods.GetScrollPosition(this.ListView, true);
                return NativeMethods.GetColumnUnderPoint(this.Handle, pt);
            }
        }

        /// <summary>
        /// Return the Windows handle behind this control
        /// </summary>
        /// <remarks>
        /// When an ObjectListView is initialized as part of a UserControl, the
        /// GetHeaderControl() method returns 0 until the UserControl is
        /// completely initialized. So the AssignHandle() call in the constructor
        /// doesn't work. So we override the Handle property so value is always
        /// current.
        /// </remarks>
        public new IntPtr Handle {
            get { return NativeMethods.GetHeaderControl(this.ListView); }
        }
        //TODO: The Handle property may no longer be necessary. CHECK! 2008/11/28

        /// <summary>
        /// Gets or sets a style that should be applied to the font of the
        /// column's header text when the mouse is over that column
        /// </summary>
        /// <remarks>THIS IS EXPERIMENTAL. USE AT OWN RISK. August 2009</remarks>
        public FontStyle HotFontStyle {
            get { return this.fontStyle; }
            set { this.fontStyle = value; }
        }
        private FontStyle fontStyle = FontStyle.Regular;

        /// <summary>
        /// Gets whether the cursor is over a "locked" divider, i.e.
        /// one that cannot be dragged by the user.
        /// </summary>
        protected bool IsCursorOverLockedDivider {
            get {
                Point pt = this.ListView.PointToClient(Cursor.Position);
                pt.X += NativeMethods.GetScrollPosition(this.ListView, true);
                int dividerIndex = NativeMethods.GetDividerUnderPoint(this.Handle, pt);
                if (dividerIndex >= 0 && dividerIndex < this.ListView.Columns.Count) {
                    OLVColumn column = this.ListView.GetColumn(dividerIndex);
                    return column.IsFixedWidth || column.FillsFreeSpace;
                } else
                    return false;
            }
        }

        /// <summary>
        /// Gets or sets the listview that this header belongs to
        /// </summary>
        protected ObjectListView ListView {
            get { return this.listView; }
            set { this.listView = value; }
        }
        private ObjectListView listView;

        /// <summary>
        /// Get or set the ToolTip that shows tips for the header
        /// </summary>
        public ToolTipControl ToolTip {
            get {
                if (this.toolTip == null) {
                    this.CreateToolTip();
                }
                return this.toolTip;
            }
            protected set { this.toolTip = value; }
        }
        private ToolTipControl toolTip;

        /// <summary>
        /// Gets or sets whether the text in column headers should be word
        /// wrapped when it is too long to fit within the column
        /// </summary>
        public bool WordWrap {
            get { return this.wordWrap; }
            set { this.wordWrap = value; }
        }
        private bool wordWrap;
        
        #endregion

        #region Commands

        /// <summary>
        /// Calculate how height the header needs to be
        /// </summary>
        /// <returns>Height in pixels</returns>
        protected int CalculateHeight(Graphics g) {
            TextFormatFlags flags = this.TextFormatFlags;
            float height = 0.0f;
            for (int i = 0; i < this.ListView.Columns.Count; i++) {
                OLVColumn column = this.ListView.GetColumn(i);
                Font f = this.CalculateFont(column);
                if (this.WordWrap) {
                    Rectangle r = this.GetItemRect(i);
                    r.Width -= 6; // Match the "tweaking" done in CustomRender
                    if (this.HasNonThemedSortIndicator(column))
                        r.Width -= 16;
                    SizeF size = TextRenderer.MeasureText(g, column.Text, f, new Size(r.Width, 100), flags);
                    height = Math.Max(height, size.Height);
                } else {
                    height = Math.Max(height, f.Height);
                }
            }
            return 7 + (int)height; // 7 is a magic constant that makes it perfectly match XP behavior
        }

        /// <summary>
        /// Should the given column show a sort indicator?
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        protected bool HasSortIndicator(OLVColumn column) {
            return column == this.ListView.LastSortColumn && this.ListView.LastSortOrder != SortOrder.None;
        }

        /// <summary>
        /// Should the given column show a non-themed sort indicator?
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        protected bool HasNonThemedSortIndicator(OLVColumn column) {
            if (VisualStyleRenderer.IsSupported)
                return !VisualStyleRenderer.IsElementDefined(VisualStyleElement.Header.SortArrow.SortedUp) &&
                    this.HasSortIndicator(column);
            else
                return this.HasSortIndicator(column);
        }

        /// <summary>
        /// Return the bounds of the item with the given index
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <returns></returns>
        public Rectangle GetItemRect(int itemIndex) {
            const int HDM_FIRST = 0x1200;
            const int HDM_GETITEMRECT = HDM_FIRST + 7;
            NativeMethods.RECT r = new NativeMethods.RECT();
            NativeMethods.SendMessageRECT(this.Handle, HDM_GETITEMRECT, itemIndex, ref r);
            return Rectangle.FromLTRB(r.left, r.top, r.right, r.bottom);
        }

        /// <summary>
        /// Force the header to redraw by invalidating it
        /// </summary>
        public void Invalidate() {
            NativeMethods.InvalidateRect(this.Handle, 0, true);
        }

        #endregion

        #region Tooltip

        protected virtual void CreateToolTip() {
            this.ToolTip = new ToolTipControl();
            this.ToolTip.Create(this.Handle);
            this.ToolTip.AddTool(this);
            this.ToolTip.Showing += new EventHandler<ToolTipShowingEventArgs>(this.ListView.headerToolTip_Showing);
        }

        #endregion

        #region Windows messaging

        protected override void WndProc(ref Message m) {
            const int WM_DESTROY = 2;
            const int WM_SETCURSOR = 0x20;
            const int WM_NOTIFY = 0x4E;
            const int WM_MOUSEMOVE = 0x200;
            const int HDM_FIRST = 0x1200;
            const int HDM_LAYOUT = (HDM_FIRST + 5);

            switch (m.Msg) {
                case WM_SETCURSOR:
                    if (!this.HandleSetCursor(ref m))
                        return;
                    break;

                case WM_NOTIFY:
                    if (!this.HandleNotify(ref m))
                        return;
                    break;

                case WM_MOUSEMOVE:
                    if (!this.HandleMouseMove(ref m))
                        return;
                    break;

                case HDM_LAYOUT:
                    if (!this.HandleLayout(ref m))
                        return;
                    break;

                case WM_DESTROY:
                    if (!this.HandleDestroy(ref m))
                        return;
                    break;
            }

            base.WndProc(ref m);
        }

        protected bool HandleSetCursor(ref Message m) {
            if (this.IsCursorOverLockedDivider) {
                m.Result = (IntPtr)1;	// Don't change the cursor
                return false;
            }
            return true;
        }

        protected bool HandleMouseMove(ref Message m) {
            int columnIndex = this.ColumnIndexUnderCursor;

            // If the mouse has moved to a different header, pop the current tip (if any)
            if (columnIndex != this.columnShowingTip) {
                this.ToolTip.PopToolTip(this);
                this.columnShowingTip = columnIndex;
            }

            return true;
        }
        private int columnShowingTip = -1;

        protected bool HandleNotify(ref Message m) {
            // Can this ever happen? JPP 2009-05-22
            if (m.LParam == IntPtr.Zero)
                return false;

            NativeMethods.NMHDR nmhdr = (NativeMethods.NMHDR)m.GetLParam(typeof(NativeMethods.NMHDR));
            switch (nmhdr.code) {

                case ToolTipControl.TTN_SHOW:
                    //System.Diagnostics.Debug.WriteLine("hdr TTN_SHOW");
                    //System.Diagnostics.Trace.Assert(this.ToolTip.Handle == nmhdr.hwndFrom);
                    return this.ToolTip.HandleShow(ref m);

                case ToolTipControl.TTN_POP:
                    //System.Diagnostics.Debug.WriteLine("hdr TTN_POP");
                    //System.Diagnostics.Trace.Assert(this.ToolTip.Handle == nmhdr.hwndFrom);
                    return this.ToolTip.HandlePop(ref m);

                case ToolTipControl.TTN_GETDISPINFO:
                    //System.Diagnostics.Debug.WriteLine("hdr TTN_GETDISPINFO");
                    //System.Diagnostics.Trace.Assert(this.ToolTip.Handle == nmhdr.hwndFrom);
                    return this.ToolTip.HandleGetDispInfo(ref m);
            }

            return false;
        }

        internal virtual bool HandleHeaderCustomDraw(ref Message m) {
            const int CDRF_NEWFONT = 2;
            const int CDRF_SKIPDEFAULT = 4;
            const int CDRF_NOTIFYITEMDRAW = 0x20;

            const int CDDS_PREPAINT = 1;
            const int CDDS_ITEM = 0x00010000;
            const int CDDS_ITEMPREPAINT = (CDDS_ITEM | CDDS_PREPAINT);

            NativeMethods.NMCUSTOMDRAW nmcustomdraw = (NativeMethods.NMCUSTOMDRAW)m.GetLParam(typeof(NativeMethods.NMCUSTOMDRAW));
            //System.Diagnostics.Debug.WriteLine(String.Format("header cd: {0:x}, {1}, {2:x}", nmcustomdraw.dwDrawStage, nmcustomdraw.dwItemSpec, nmcustomdraw.uItemState));
            switch (nmcustomdraw.dwDrawStage) {
                case CDDS_PREPAINT:
                    this.cachedNeedsCustomDraw = this.NeedsCustomDraw;
                    m.Result = (IntPtr)CDRF_NOTIFYITEMDRAW;
                    return true;

                case CDDS_ITEMPREPAINT:
                    int columnIndex = nmcustomdraw.dwItemSpec.ToInt32();
                    OLVColumn column = this.ListView.GetColumn(columnIndex);

                    // These don't work when visual styles are enabled
                    //NativeMethods.SetBkColor(nmcustomdraw.hdc, ColorTranslator.ToWin32(Color.Red));
                    //NativeMethods.SetTextColor(nmcustomdraw.hdc, ColorTranslator.ToWin32(Color.Blue));
                    //m.Result = IntPtr.Zero;

                    if (this.cachedNeedsCustomDraw) {
                        using (Graphics g = Graphics.FromHdc(nmcustomdraw.hdc)) {
                            g.TextRenderingHint = ObjectListView.TextRendereringHint;
                            this.CustomDrawHeaderCell(g, columnIndex, nmcustomdraw.uItemState);
                        }
                        m.Result = (IntPtr)CDRF_SKIPDEFAULT;
                    } else {
                        Font f = column.HeaderFont ?? this.ListView.HeaderFont ?? this.ListView.Font;

                        if (this.HotFontStyle != FontStyle.Regular && columnIndex == this.ColumnIndexUnderCursor)
                            f = new Font(f, this.HotFontStyle);

                        NativeMethods.SelectObject(nmcustomdraw.hdc, f.ToHfont());

                        m.Result = (IntPtr)CDRF_NEWFONT;
                    }

                    return true;
            }

            return false;
        }
        bool cachedNeedsCustomDraw;

        /// <summary>
        /// The message divides a ListView's space between the header and the rows of the listview.
        /// The WINDOWPOS structure controls the headers bounds, the RECT controls the listview bounds.
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected bool HandleLayout(ref Message m) {
            if (this.ListView.HeaderStyle == ColumnHeaderStyle.None)
                return true;

            NativeMethods.HDLAYOUT hdlayout = (NativeMethods.HDLAYOUT)m.GetLParam(typeof(NativeMethods.HDLAYOUT));
            NativeMethods.RECT rect = (NativeMethods.RECT)Marshal.PtrToStructure(hdlayout.prc, typeof(NativeMethods.RECT));
            NativeMethods.WINDOWPOS wpos = (NativeMethods.WINDOWPOS)Marshal.PtrToStructure(hdlayout.pwpos, typeof(NativeMethods.WINDOWPOS));

            using (Graphics g = this.ListView.CreateGraphics()) {
                g.TextRenderingHint = ObjectListView.TextRendereringHint;
                int height = this.CalculateHeight(g);
                wpos.hwnd = this.Handle;
                wpos.hwndInsertAfter = IntPtr.Zero;
                wpos.flags = NativeMethods.SWP_FRAMECHANGED;
                wpos.x = rect.left;
                wpos.y = rect.top;
                wpos.cx = rect.right - rect.left;
                wpos.cy = height;

                rect.top = height;

                Marshal.StructureToPtr(rect, hdlayout.prc, false);
                Marshal.StructureToPtr(wpos, hdlayout.pwpos, false);
            }

            this.ListView.BeginInvoke((MethodInvoker)delegate {
                this.Invalidate();
                this.ListView.Invalidate();
            });
            return false;
        }

        /// <summary>
        /// Handle when the underlying header control is destroyed
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected bool HandleDestroy(ref Message m) {
            if (this.ToolTip != null) {
                this.ToolTip.Showing -= new EventHandler<ToolTipShowingEventArgs>(this.ListView.headerToolTip_Showing);
            }
            return false;
        }

        #endregion

        #region Rendering

        /// <summary>
        /// Does this header need to be custom drawn?
        /// </summary>
        /// <remarks>Word wrapping and colored text require custom drawning. Funnily enough, we
        /// can change the font natively.</remarks>
        protected bool NeedsCustomDraw {
            get {
                if (this.WordWrap)
                    return true;
                foreach (OLVColumn column in this.ListView.Columns) {
                    if (!column.HeaderForeColor.IsEmpty)
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Draw one cell of the header
        /// </summary>
        /// <param name="g"></param>
        /// <param name="columnIndex"></param>
        /// <param name="itemState"></param>
        protected void CustomDrawHeaderCell(Graphics g, int columnIndex, int itemState) {
            // TODO: This needs to be refactored
            const int CDIS_SELECTED = 1;
            Rectangle r = this.GetItemRect(columnIndex);
            OLVColumn column = this.ListView.GetColumn(columnIndex);
            int columnUnderCursor = this.ColumnIndexUnderCursor;

            // Draw the background
            if (VisualStyleRenderer.IsSupported &&
                VisualStyleRenderer.IsElementDefined(VisualStyleElement.Header.Item.Normal)) {
                int part = 1; // normal item
                if (columnIndex == 0 &&
                    VisualStyleRenderer.IsElementDefined(VisualStyleElement.Header.ItemLeft.Normal))
                    part = 2; // left item
                if (columnIndex == this.ListView.Columns.Count - 1 &&
                    VisualStyleRenderer.IsElementDefined(VisualStyleElement.Header.ItemRight.Normal))
                    part = 3; // right item

                int state = 1; // normal state
                if ((itemState & CDIS_SELECTED) == CDIS_SELECTED)
                    state = 3; // pressed
                else if (columnIndex == this.ColumnIndexUnderCursor)
                    state = 2; // hot

                VisualStyleRenderer renderer = new VisualStyleRenderer("HEADER", part, state);
                renderer.DrawBackground(g, r);
            } else {
                //g.FillRectangle(Brushes.LightGray, r);
                ControlPaint.DrawBorder3D(g, r, Border3DStyle.Raised);
            }

            // Draw the sort indicator if this column has one
            if (this.HasSortIndicator(column)) {
                if (VisualStyleRenderer.IsSupported && 
                    VisualStyleRenderer.IsElementDefined(VisualStyleElement.Header.SortArrow.SortedUp)) {
                    VisualStyleRenderer renderer2 = null;
                    if (this.ListView.LastSortOrder == SortOrder.Ascending)
                        renderer2 = new VisualStyleRenderer(VisualStyleElement.Header.SortArrow.SortedUp);
                    if (this.ListView.LastSortOrder == SortOrder.Descending)
                        renderer2 = new VisualStyleRenderer(VisualStyleElement.Header.SortArrow.SortedDown);
                    if (renderer2 != null) {
                        Size sz = renderer2.GetPartSize(g, ThemeSizeType.True);
                        Point pt = renderer2.GetPoint(PointProperty.Offset);
                        // GetPoint() should work, but if it doesn't, put the arrow in the top middle
                        if (pt.X == 0 && pt.Y == 0)
                            pt = new Point(r.X + (r.Width / 2) - (sz.Width / 2), r.Y);
                        renderer2.DrawBackground(g, new Rectangle(pt, sz));
                    }

                } else {
                    // No theme support for sort indicators. So, we draw a triangle at the right edge
                    // of the column header.
                    const int triangleHeight = 16;
                    const int triangleWidth = 16;
                    const int midX = triangleWidth / 2;
                    const int midY = (triangleHeight / 2) - 1;
                    const int deltaX = midX - 2;
                    const int deltaY = deltaX / 2;

                    Point triangleLocation = new Point(r.Right - triangleWidth - 2, r.Top + (r.Height - triangleHeight) / 2);
                    Point[] pts = new Point[] { triangleLocation, triangleLocation, triangleLocation };

                    if (this.ListView.LastSortOrder == SortOrder.Ascending) {
                        pts[0].Offset(midX - deltaX, midY + deltaY);
                        pts[1].Offset(midX, midY - deltaY - 1);
                        pts[2].Offset(midX + deltaX, midY + deltaY);
                    } else {
                        pts[0].Offset(midX - deltaX, midY - deltaY);
                        pts[1].Offset(midX, midY + deltaY);
                        pts[2].Offset(midX + deltaX, midY - deltaY);
                    }

                    g.FillPolygon(Brushes.SlateGray, pts);
                    r.Width = r.Width - triangleWidth;
                }
            }

            // Finally draw the text
            TextFormatFlags flags = this.TextFormatFlags;
            if (column.TextAlign == HorizontalAlignment.Center)
                flags |= TextFormatFlags.HorizontalCenter;
            if (column.TextAlign == HorizontalAlignment.Right)
                flags |= TextFormatFlags.Right;

            Font f = this.CalculateFont(column);
            Color color = column.HeaderForeColor;
            if (color.IsEmpty)
                color = this.ListView.ForeColor;

            // Tweak the text rectangle a little to improve aethestics
            r.Inflate(-3, 0); 
            r.Y -= 2;

            TextRenderer.DrawText(g, column.Text, f, r, color, Color.Transparent, flags);
        }

        private Font CalculateFont(OLVColumn column) {
            Font f = column.HeaderFont ?? this.ListView.HeaderFont ?? this.ListView.Font;
            if (this.HotFontStyle != FontStyle.Regular && 
                column.Index == this.ColumnIndexUnderCursor)
                f = new Font(f, this.HotFontStyle);
            return f;
        }

        private TextFormatFlags TextFormatFlags {
            get {
                TextFormatFlags flags = TextFormatFlags.EndEllipsis | 
                    TextFormatFlags.NoPrefix |
                    TextFormatFlags.WordEllipsis | 
                    TextFormatFlags.NoPadding |
                    TextFormatFlags.VerticalCenter | 
                    TextFormatFlags.PreserveGraphicsTranslateTransform;
                if (this.WordWrap)
                    flags |= TextFormatFlags.WordBreak;
                else
                    flags |= TextFormatFlags.SingleLine;
                if (this.ListView.RightToLeft == RightToLeft.Yes)
                    flags |= TextFormatFlags.RightToLeft;

                return flags;
            }
        }

        #endregion

    }
}
