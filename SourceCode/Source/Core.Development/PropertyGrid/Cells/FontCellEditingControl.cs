/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.ComponentModel.Design;
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class FontCellEditingControl : UserControl, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;
        FontDialog fontDialog;
        private UIElementFont _elementFont;
        public UIElementFont ElementFont
        {
            get
            {
                return this._elementFont;
            }
            set
            {
                this._elementFont = value;
                this.txtDataItem.Text = value.ToString();
            }
        }
        public FontCellEditingControl()
        {
            InitializeComponent();
        }
        private void btnChoose_Click(object sender, EventArgs e)
        {
            if (fontDialog == null)
                fontDialog = new FontDialog();
            fontDialog.Font = ElementFont.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                UIElementFont font = new UIElementFont();
                font.FontFamily = fontDialog.Font.FontFamily.Name;
                font.Bold = fontDialog.Font.Bold;
                font.Italic = fontDialog.Font.Italic;
                font.Size = fontDialog.Font.Size;
                font.Underline = fontDialog.Font.Underline;
                this.ElementFont = font;
                PropertyGridRow propertyGirdRow = this.EditingControlDataGridView.Rows[rowIndex] as PropertyGridRow;
                propertyGirdRow.SetPropertyValue(font);
            }
        }
        public object EditingControlFormattedValue
        {
            get
            {
                return this.ElementFont.ToString();
            }
            set
            {
                UIElementFont newValue = value as UIElementFont;
                if (newValue != null)
                {
                    this.ElementFont = newValue;
                }
            }
        }
        public object GetEditingControlFormattedValue(
                DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }
        public void ApplyCellStyleToEditingControl(
                DataGridViewCellStyle dataGridViewCellStyle)
        {
        }
        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }
        public bool EditingControlWantsInputKey(
                Keys key, bool dataGridViewWantsInputKey)
        {
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return false;
            }
        }
        public void PrepareEditingControlForEdit(bool selectAll)
        {
        }
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }
        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }
    }
}
