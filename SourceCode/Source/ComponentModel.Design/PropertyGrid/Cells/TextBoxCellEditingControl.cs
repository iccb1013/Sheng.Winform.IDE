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
using System.ComponentModel;
namespace Sheng.SailingEase.ComponentModel.Design
{
    [ToolboxItem(false)]
    class TextBoxCellEditingControl : TextBox, IDataGridViewEditingControl
    {
        private DataGridView _dataGridView;
        private bool _valueChanged = false;
        private int _rowIndex;
        private PropertyRelatorAttribute propertyGirdAttribute;
        public PropertyRelatorAttribute PropertyGirdAttribute
        {
            get
            {
                return this.propertyGirdAttribute;
            }
            set
            {
                this.propertyGirdAttribute = value;
            }
        }
        public TextBoxCellEditingControl()
        {
        }
        public object EditingControlFormattedValue
        {
            get
            {
                return this.Text;
            }
            set
            {
                String newValue = value as String;
                if (newValue != null)
                {
                    this.Text = newValue;
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
                return _rowIndex;
            }
            set
            {
                _rowIndex = value;
            }
        }
        public bool EditingControlWantsInputKey(
                Keys key, bool dataGridViewWantsInputKey)
        {
            return true;
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
                return _dataGridView;
            }
            set
            {
                _dataGridView = value;
            }
        }
        public bool EditingControlValueChanged
        {
            get
            {
                return _valueChanged;
            }
            set
            {
                _valueChanged = value;
            }
        }
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }
        protected override void OnTextChanged(EventArgs e)
        {
            _valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnTextChanged(e);
        }
    }
}
