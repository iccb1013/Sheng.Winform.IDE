using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.VisualStyles;
using System.Diagnostics;
using Sheng.SailingEase.Drawing;

namespace WindowsFormsApplication2
{
    class DataGridViewImageCellRenderer : IDataGridViewCellRenderer
    {
        #region IDataGridViewCellRenderer 成员

        private Type _renderCellType = typeof(DataGridViewCheckBoxCell);
        public Type RenderCellType
        {
            get { return _renderCellType; }
        }

        public void Paint(System.Drawing.Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates elementState, object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle)
        {
            if (value == null)
                return;

            if ((value is Image) == false)
            {
                Debug.Assert(false, "value 不是 Image");
                return;
            }

            int imageWidth = cellBounds.Width - 2;
            int imageHeight = cellBounds.Height - 2;

            Image image = (Image)value;
            image = DrawingTool.GetScaleImage(image, imageWidth, imageHeight);

            Point drawInPoint = new Point(cellBounds.X + cellBounds.Width / 2 - image.Width / 2,
                cellBounds.Y + cellBounds.Height / 2 - image.Height / 2);

            graphics.DrawImage(image, drawInPoint);
        }

        #endregion
    }
}
