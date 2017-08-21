/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
namespace Sheng.SailingEase.Infrastructure
{
    public static class UIHelper
    {
        public static void ProcessDataGridView(DataGridView dataGridView)
        {
            #if DEBUG
            dataGridView.DataError += new DataGridViewDataErrorEventHandler(dataGridView_DataError);
            #endif
            dataGridView.ReadOnly = true;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.BackgroundColor = DataGridViewBackColor;
            dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridView.ColumnHeadersHeight = 21;
            dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView.RowHeadersVisible = false;
            dataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dataGridView.RowTemplate.Height = 21;
            dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dataGridView.StandardTab = true;
            System.Windows.Forms.DataGridViewCellStyle columnHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            columnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            columnHeadersDefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
            columnHeadersDefaultCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            columnHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            columnHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            columnHeadersDefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dataGridView.ColumnHeadersDefaultCellStyle = columnHeadersDefaultCellStyle;
            System.Windows.Forms.DataGridViewCellStyle defaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            defaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            defaultCellStyle.BackColor = UIHelper.DataGridViewRowBackColorNormal;
            defaultCellStyle.ForeColor = UIHelper.DataGridViewRowForeColorNormal; 
            defaultCellStyle.SelectionBackColor = UIHelper.DataGridViewSelectedRowBackColorNormal;
            defaultCellStyle.SelectionForeColor = UIHelper.DataGridViewSelectedRowForeColorNormal;
            defaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            dataGridView.DefaultCellStyle = defaultCellStyle;
            System.Windows.Forms.DataGridViewCellStyle rowHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            rowHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            rowHeadersDefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
            rowHeadersDefaultCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            rowHeadersDefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            rowHeadersDefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            rowHeadersDefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dataGridView.RowHeadersDefaultCellStyle = rowHeadersDefaultCellStyle;
        }
        static void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Debug.Assert(false, e.Exception.Message);
        }
        public static Color DataGridViewBackColor
        {
            get { return Color.White; }
        }
        public static Color DataGridViewRowBackColorWarning
        {
            get
            {
                return SystemColors.Window;
            }
        }
        public static Color DataGridViewSelectedRowBackColorWarning
        {
            get
            {
                return SystemColors.Highlight;
            }
        }
        public static Color DataGridViewRowForeColorWarning
        {
            get
            {
                return Color.Red;
            }
        }
        public static Color DataGridViewSelectedRowForeColorWarning
        {
            get
            {
                return Color.Red;
            }
        }
        public static Color DataGridViewRowBackColorNormal
        {
            get
            {
                return SystemColors.Window;
            }
        }
        public static Color DataGridViewSelectedRowBackColorNormal
        {
            get
            {
                return SystemColors.Highlight;
            }
        }
        public static Color DataGridViewRowForeColorNormal
        {
            get
            {
                return SystemColors.ControlText;
            }
        }
        public static Color DataGridViewSelectedRowForeColorNormal
        {
            get
            {
                return SystemColors.HighlightText;
            }
        }
    }
}
