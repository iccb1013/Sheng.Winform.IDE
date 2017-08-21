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
    class NumericUpDownCellEditingControl : NumericUpDown, IDataGridViewEditingControl
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
        public NumericUpDownCellEditingControl()
        {
        }
        public object EditingControlFormattedValue
        {
            get
            {
                return this.Value.ToString();
            }
            set
            {
                String newValue = value as String;
                if (newValue != null)
                {
                    this.Value = decimal.Parse(newValue);
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
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
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
        protected override void OnValueChanged(EventArgs eventargs)
        {
            _valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(eventargs);
        }
    }
}
