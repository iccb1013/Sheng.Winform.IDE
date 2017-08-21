/*
 * DragSource.cs - Add drag source functionality to an ObjectListView
 *
 * UNFINISHED
 * 
 * Author: Phillip Piper
 * Date: 2009-03-17 5:15 PM
 *
 * Change log:
 * 2009-07-06   JPP  - Make sure Link is acceptable as an drop effect by default
 *                     (since MS didn't make it part of the 'All' value)
 * v2.2
 * 2009-04-15   JPP  - Separated DragSource.cs into DropSink.cs
 * 2009-03-17   JPP  - Initial version
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Sheng.SailingEase.Controls.ObjectListView
{
    public interface IDragSource
    {
        Object StartDrag(ObjectListView olv, MouseButtons button, OLVListItem item);
        DragDropEffects GetAllowedEffects(Object dragObject);
        void EndDrag(Object dragObject, DragDropEffects effect);
    }

    public class AbstractDragSource : IDragSource
    {
        #region IDragSource Members

        public virtual Object StartDrag(ObjectListView olv, MouseButtons button, OLVListItem item) {
            return null;
        }

        public virtual DragDropEffects GetAllowedEffects(Object data) {
            return DragDropEffects.None;
        }

        public virtual void EndDrag(Object dragObject, DragDropEffects effect) {
        }

        #endregion
    }

    public class SimpleDragSource : IDragSource
    {
        #region Constructors

        public SimpleDragSource() {
        }

        public SimpleDragSource(bool refreshAfterDrop) {
            this.RefreshAfterDrop = refreshAfterDrop;
        }

        #endregion

        #region Public properties

        public bool RefreshAfterDrop {
            get { return refreshAfterDrop; }
            set { refreshAfterDrop = value;  }
        }
        private bool refreshAfterDrop;

        #endregion

        #region IDragSource Members

        public virtual Object StartDrag(ObjectListView olv, MouseButtons button, OLVListItem item) {
            // We only drag on left mouse
            if (button != MouseButtons.Left)
                return null;

            return this.CreateDataObject(olv);
        }

        public virtual DragDropEffects GetAllowedEffects(Object data) {
            return DragDropEffects.All | DragDropEffects.Link; // why didn't MS include 'Link' in 'All'??
        }

        public virtual void EndDrag(Object dragObject, DragDropEffects effect) {
            OLVDataObject data = dragObject as OLVDataObject;
            if (data == null)
                return;

            if (this.RefreshAfterDrop)
                data.ListView.RefreshObjects(data.ModelObjects);
        }

        /// <summary>
        /// Create a data object that will be used to as the data object
        /// for the drag operation.
        /// </summary>
        /// <remarks>
        /// Subclasses can override this method add new formats to the data object.
        /// </remarks>
        /// <param name="olv">The ObjectListView that is the source of the drag</param>
        /// <returns>A data object for the drag</returns>
        protected virtual object CreateDataObject(ObjectListView olv) {
            OLVDataObject data = new OLVDataObject(olv);
            data.CreateTextFormats();
            return data;
        }

        #endregion
    }

    public class OLVDataObject : DataObject
    {
        public OLVDataObject(ObjectListView olv) : this(olv, olv.SelectedObjects) {

        }

        public OLVDataObject(ObjectListView olv, IList modelObjects) {
            this.objectListView = olv;
            this.modelObjects = modelObjects;
        }

        public ObjectListView ListView {
            get { return objectListView; }
        }
        private ObjectListView objectListView;

        public IList ModelObjects {
            get { return modelObjects; }
        }
        private IList modelObjects = new ArrayList();

        public void CreateTextFormats() {
            List<OLVColumn> columns = this.ListView.ColumnsInDisplayOrder;

            // Build text and html versions of the selection
            StringBuilder sbText = new StringBuilder();
            StringBuilder sbHtml = new StringBuilder("<table>");

            foreach (object modelObject in this.ModelObjects) {
                sbHtml.Append("<tr><td>");
                foreach (OLVColumn col in columns) {
                    if (col != columns[0]) {
                        sbText.Append("\t");
                        sbHtml.Append("</td><td>");
                    }
                    string strValue = col.GetStringValue(modelObject);
                    sbText.Append(strValue);
                    sbHtml.Append(strValue); //TODO: Should encode the string value
                }
                sbText.AppendLine();
                sbHtml.AppendLine("</td></tr>");
            }
            sbHtml.AppendLine("</table>");

            // Put both the text and html versions onto the clipboard.
            // For some reason, SetText() with UnicodeText doesn't set the basic CF_TEXT format,
            // but using SetData() does.
            //this.SetText(sbText.ToString(), TextDataFormat.UnicodeText);
            this.SetData(sbText.ToString());
            this.SetText(ConvertToHtmlFragment(sbHtml.ToString()), TextDataFormat.Html);
        }

        /// <summary>
        /// Convert the fragment of HTML into the Clipboards HTML format.
        /// </summary>
        /// <remarks>The HTML format is found here http://msdn2.microsoft.com/en-us/library/aa767917.aspx
        /// </remarks>
        /// <param name="fragment">The HTML to put onto the clipboard. It must be valid HTML!</param>
        /// <returns>A string that can be put onto the clipboard and will be recognized as HTML</returns>
        private string ConvertToHtmlFragment(string fragment) {
            // Minimal implementation of HTML clipboard format
            string source = "http://www.codeproject.com/KB/list/ObjectListView.aspx";

            const String MARKER_BLOCK =
                "Version:1.0\r\n" +
                "StartHTML:{0,8}\r\n" +
                "EndHTML:{1,8}\r\n" +
                "StartFragment:{2,8}\r\n" +
                "EndFragment:{3,8}\r\n" +
                "StartSelection:{2,8}\r\n" +
                "EndSelection:{3,8}\r\n" +
                "SourceURL:{4}\r\n" +
                "{5}";

            int prefixLength = String.Format(MARKER_BLOCK, 0, 0, 0, 0, source, "").Length;

            const String DEFAULT_HTML_BODY =
                "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.0 Transitional//EN\">" +
                "<HTML><HEAD></HEAD><BODY><!--StartFragment-->{0}<!--EndFragment--></BODY></HTML>";

            string html = String.Format(DEFAULT_HTML_BODY, fragment);
            int startFragment = prefixLength + html.IndexOf(fragment);
            int endFragment = startFragment + fragment.Length;

            return String.Format(MARKER_BLOCK, prefixLength, prefixLength + html.Length, startFragment, endFragment, source, html);
        }
    }
}
