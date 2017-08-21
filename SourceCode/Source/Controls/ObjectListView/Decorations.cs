/*
 * Decorations - Images, text or other things that can be rendered onto an ObjectListView
 *
 * Author: Phillip Piper
 * Date: 19/08/2009 10:56 PM
 *
 * Change log:
 * 2009-09-23   JPP  - Added LeftColumn and RightColumn to RowBorderDecoration
 * 2009-08-23   JPP  - Added LightBoxDecoration
 * 2009-08-19   JPP  - Initial version. Separated from Overlays.cs
 *
 * To do:
 *
 * Copyright (C) 2009 Phillip Piper
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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Sheng.SailingEase.Controls.ObjectListView
{
    /// <summary>
    /// A decoration is an overlay that draws itself in relation to a given row or cell.
    /// Decorations scroll when the listview scrolls.
    /// </summary>
    public interface IDecoration : IOverlay
    {
        /// <summary>
        /// Gets or sets the row that is to be decorated
        /// </summary>
        OLVListItem ListItem { get; set; }

        /// <summary>
        /// Gets or sets the subitem that is to be decorated
        /// </summary>
        OLVListSubItem SubItem { get; set; }
    }

    /// <summary>
    /// An AbstractDecoration is a safe do-nothing implementation of the IDecoration interface
    /// </summary>
    public class AbstractDecoration : IDecoration
    {
        #region IDecoration Members

        public OLVListItem ListItem {
            get { return listItem; }
            set { listItem = value; }
        }
        private OLVListItem listItem;

        public OLVListSubItem SubItem {
            get { return subItem; }
            set { subItem = value; }
        }
        private OLVListSubItem subItem;

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the bounds of the decorations row
        /// </summary>
        public Rectangle RowBounds {
            get {
                if (this.ListItem == null)
                    return Rectangle.Empty;
                else
                    return this.ListItem.Bounds;
            }
        }

        /// <summary>
        /// Get the bounds of the decorations cell
        /// </summary>
        public Rectangle CellBounds {
            get {
                if (this.ListItem == null || this.SubItem == null)
                    return Rectangle.Empty;
                else
                    return this.ListItem.GetSubItemBounds(this.ListItem.SubItems.IndexOf(this.SubItem));
            }
        }

        #endregion

        #region IOverlay Members

        /// <summary>
        /// Draw the decoration
        /// </summary>
        /// <param name="olv"></param>
        /// <param name="g"></param>
        /// <param name="r"></param>
        public virtual void Draw(ObjectListView olv, Graphics g, Rectangle r) {
        }

        #endregion
    }

    /// <summary>
    /// This decoration draws a slight tint over a column of the
    /// owning listview. If no column is explicitly set, the selected
    /// column in the listview will be used.
    /// The selected column is normally the sort column, but does not have to be.
    /// </summary>
    public class TintedColumnDecoration : AbstractDecoration
    {
        #region Constructors

        public TintedColumnDecoration() {
            this.Tint = Color.FromArgb(15, Color.Blue);
        }

        public TintedColumnDecoration(OLVColumn column)
            : this() {
            this.ColumnToTint = column;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the column that will be tinted
        /// </summary>
        public OLVColumn ColumnToTint {
            get { return this.columnToTint; }
            set { this.columnToTint = value; }
        }
        private OLVColumn columnToTint;

        /// <summary>
        /// Gets or sets the color that will be 'tinted' over the selected column
        /// </summary>
        public Color Tint {
            get { return this.tint; }
            set {
                if (this.tint == value)
                    return;

                if (this.tintBrush != null) {
                    this.tintBrush.Dispose();
                    this.tintBrush = null;
                }

                this.tint = value;
                this.tintBrush = new SolidBrush(this.tint);
            }
        }
        private Color tint;
        private SolidBrush tintBrush;

        #endregion

        #region IOverlay Members

        public override void Draw(ObjectListView olv, Graphics g, Rectangle r) {

            // This overlay only works when:
            // - the list is in Details view
            // - there is at least one row
            // - there is a selected column
            if (olv.View != System.Windows.Forms.View.Details)
                return;

            if (olv.GetItemCount() == 0)
                return;

            OLVColumn column = this.ColumnToTint ?? olv.SelectedColumn;
            if (column == null)
                return;

            Point sides = NativeMethods.GetScrolledColumnSides(olv, column.Index);
            if (sides.X == -1)
                return;

            Rectangle columnBounds = new Rectangle(sides.X, r.Top, sides.Y - sides.X, r.Bottom);

            // Find the bottom of the last item. The tinting should extend only to there.
            OLVListItem lastItem = olv.GetLastItemInDisplayOrder();
            if (lastItem != null) {
                Rectangle lastItemBounds = lastItem.Bounds;
                if (!lastItemBounds.IsEmpty && lastItemBounds.Bottom < columnBounds.Bottom)
                    columnBounds.Height = lastItemBounds.Bottom - columnBounds.Top;
            }
            g.FillRectangle(this.tintBrush, columnBounds);
        }

        #endregion
    }

    /// <summary>
    /// This decoration draws an optionally filled border around a rectangle.
    /// Subclasses must override CalculateBounds().
    /// </summary>
    public class BorderDecoration : AbstractDecoration
    {
        #region Constructors

        public BorderDecoration()
            : this(new Pen(Color.FromArgb(64, Color.Blue), 1)) {
        }

        public BorderDecoration(Pen borderPen) {
            this.BorderPen = borderPen;
        }

        public BorderDecoration(Pen borderPen, Brush fill) {
            this.BorderPen = borderPen;
            this.FillBrush = fill;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the pen that will be used to draw the border
        /// </summary>
        public Pen BorderPen {
            get { return this.borderPen; }
            set { this.borderPen = value; }
        }
        private Pen borderPen;

        /// <summary>
        /// Gets or sets the padding that will be added to the bounds of the item
        /// before drawing the border and fill.
        /// </summary>
        public Size BoundsPadding {
            get { return this.boundsPadding; }
            set { this.boundsPadding = value; }
        }
        private Size boundsPadding = new Size(-1, 2);

        /// <summary>
        /// How rounded should the corners of the border be? 0 means no rounding.
        /// </summary>
        /// <remarks>If this value is too large, the edges of the border will appear odd.</remarks>
        public float CornerRounding {
            get { return this.cornerRounding; }
            set { this.cornerRounding = value; }
        }
        private float cornerRounding = 16.0f;

        /// <summary>
        /// Gets or sets the brush that will be used to fill the border
        /// </summary>
        public Brush FillBrush {
            get { return this.fillBrush; }
            set { this.fillBrush = value; }
        }
        private Brush fillBrush = new SolidBrush(Color.FromArgb(64, Color.Blue));

        #endregion

        #region IOverlay Members

        public override void Draw(ObjectListView olv, Graphics g, Rectangle r) {
            Rectangle bounds = this.CalculateBounds();
            if (!bounds.IsEmpty)
                this.DrawFilledBorder(g, bounds);
        }
        
        #endregion

        #region Subclass responsibility

        /// <summary>
        /// Subclasses should override this to say where the border should be drawn
        /// </summary>
        /// <returns></returns>
        protected virtual Rectangle CalculateBounds() {
            return Rectangle.Empty;
        }

        #endregion

        #region Implementation utlities

        protected void DrawFilledBorder(Graphics g, Rectangle bounds) {
            bounds.Inflate(this.BoundsPadding);
            GraphicsPath path = this.GetRoundedRect(bounds, this.CornerRounding);
            if (this.FillBrush != null)
                g.FillPath(this.FillBrush, path);
            if (this.BorderPen != null)
                g.DrawPath(this.BorderPen, path);
        }

        protected GraphicsPath GetRoundedRect(RectangleF rect, float diameter) {
            GraphicsPath path = new GraphicsPath();

            if (diameter <= 0.0f) {
                path.AddRectangle(rect);
            } else {
                RectangleF arc = new RectangleF(rect.X, rect.Y, diameter, diameter);
                path.AddArc(arc, 180, 90);
                arc.X = rect.Right - diameter;
                path.AddArc(arc, 270, 90);
                arc.Y = rect.Bottom - diameter;
                path.AddArc(arc, 0, 90);
                arc.X = rect.Left;
                path.AddArc(arc, 90, 90);
                path.CloseFigure();
            }

            return path;
        }

        #endregion
    }

    /// <summary>
    /// Instances of this class draw a border around the decorated row
    /// </summary>
    public class RowBorderDecoration : BorderDecoration
    {
        public int LeftColumn {
            get { return leftColumn; }
            set { leftColumn = value; }
        }
        private int leftColumn = -1;

        public int RightColumn {
            get { return rightColumn; }
            set { rightColumn = value; }
        }
        private int rightColumn = -1;

        protected override Rectangle CalculateBounds() {
            Rectangle bounds = this.RowBounds;
            if (this.ListItem == null)
                return bounds;

            if (this.LeftColumn >= 0) {
                Rectangle leftCellBounds = this.ListItem.GetSubItemBounds(this.LeftColumn);
                if (!leftCellBounds.IsEmpty) {
                    bounds.Width = bounds.Right - leftCellBounds.Left;
                    bounds.X = leftCellBounds.Left;
                }
            }

            if (this.RightColumn >= 0) {
                Rectangle rightCellBounds = this.ListItem.GetSubItemBounds(this.RightColumn);
                if (!rightCellBounds.IsEmpty) {
                    bounds.Width = rightCellBounds.Right - bounds.Left;
                }
            }

            return bounds;
        }
    }

    /// <summary>
    /// Instances of this class draw a border around the decorated subitem.
    /// </summary>
    public class CellBorderDecoration : BorderDecoration
    {
        protected override Rectangle CalculateBounds() {
            return this.CellBounds;
        }
    }

    /// <summary>
    /// This decoration causes everything except the decoration row to be overpainted
    /// with a tint. The dark and more opaque the fill color, the more obvious the
    /// decorated row becomes.
    /// </summary>
    public class LightBoxDecoration : BorderDecoration
    {
        public LightBoxDecoration() {
            this.BoundsPadding = new Size(-1, 4);
            this.CornerRounding = 8.0f;
            this.FillBrush = new SolidBrush(Color.FromArgb(48, Color.Black));
        }

        public override void Draw(ObjectListView olv, Graphics g, Rectangle r) {
            if (!r.Contains(olv.PointToClient(Cursor.Position)))
                return;

            using (Region newClip = new Region(r)) {
                Rectangle bounds = this.RowBounds;
                bounds.Inflate(this.BoundsPadding);
                newClip.Exclude(this.GetRoundedRect(bounds, this.CornerRounding));
                Region originalClip = g.Clip;
                g.Clip = newClip;
                g.FillRectangle(this.FillBrush, r);
                g.Clip = originalClip;
            }
        }
    }

    /// <summary>
    /// Instances of this class put an Image over the row/cell that it is decorating
    /// </summary>
    public class ImageDecoration : ImageAdornment, IDecoration
    {
        #region Constructors

        public ImageDecoration() {
            this.Alignment = ContentAlignment.MiddleRight;
        }

        public ImageDecoration(Image image)
            : this() {
            this.Image = image;
        }

        public ImageDecoration(Image image, int transparency)
            : this() {
            this.Image = image;
            this.Transparency = transparency;
        }

        public ImageDecoration(Image image, ContentAlignment alignment)
            : this() {
            this.Image = image;
            this.Alignment = alignment;
        }

        public ImageDecoration(Image image, int transparency, ContentAlignment alignment)
            : this() {
            this.Image = image;
            this.Transparency = transparency;
            this.Alignment = alignment;
        }

        #endregion

        #region IDecoration Members

        public OLVListItem ListItem {
            get { return listItem; }
            set { listItem = value; }
        }
        private OLVListItem listItem;

        public OLVListSubItem SubItem {
            get { return subItem; }
            set { subItem = value; }
        }
        private OLVListSubItem subItem;

        #endregion

        #region Commands

        /// <summary>
        /// Draw this overlay
        /// </summary>
        /// <param name="olv">The ObjectListView being decorated</param>
        /// <param name="g">The Graphics used for drawing</param>
        /// <param name="r">The bounds of the rendering</param>
        public virtual void Draw(ObjectListView olv, Graphics g, Rectangle r) {
            this.DrawImage(g, this.CalculateItemBounds(this.ListItem, this.SubItem));
        }

        #endregion
    }

    /// <summary>
    /// Instances of this class draw some text over the row/cell that they are decorating
    /// </summary>
    public class TextDecoration : TextAdornment, IDecoration
    {
        #region Constructors

        public TextDecoration() {
            this.Alignment = ContentAlignment.MiddleRight;
        }

        public TextDecoration(string text)
            : this() {
            this.Text = text;
        }
        
        public TextDecoration(string text, int transparency)
            : this() {
            this.Text = text;
            this.Transparency = transparency;
        }

        public TextDecoration(string text, ContentAlignment alignment)
            : this() {
            this.Text = text;
            this.Alignment = alignment;
        }

        public TextDecoration(string text, int transparency, ContentAlignment alignment)
            : this() {
            this.Text = text;
            this.Transparency = transparency;
            this.Alignment = alignment;
        }

        #endregion

        #region IDecoration Members

        public OLVListItem ListItem {
            get { return listItem; }
            set { listItem = value; }
        }
        private OLVListItem listItem;

        public OLVListSubItem SubItem {
            get { return subItem; }
            set { subItem = value; }
        }
        private OLVListSubItem subItem;

        #endregion

        #region Commands

        /// <summary>
        /// Draw this overlay
        /// </summary>
        /// <param name="olv">The ObjectListView being decorated</param>
        /// <param name="g">The Graphics used for drawing</param>
        /// <param name="r">The bounds of the rendering</param>
        public virtual void Draw(ObjectListView olv, Graphics g, Rectangle r) {
            this.DrawText(g, this.CalculateItemBounds(this.ListItem, this.SubItem));
        }

        #endregion
    }
}
