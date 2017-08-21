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
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class DataItemChooseCellEditingControl : UserControl, IDataGridViewEditingControl
    {
        private DataGridView _dataGridView;
        private bool _valueChanged = false;
        private int _rowIndex;
        private DataEntityTreeChooseView _formDataEntityTreeChoose;
        private PropertyRelatorAttribute propertyGirdAttribute;
        [Obsolete]
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
        private bool _showDataItem = true;
        public bool ShowDataItem
        {
            get { return _showDataItem; }
            set
            {
                this._showDataItem = value;
            }
        }
        private string _dataItemId;
        public string DataItemId
        {
            get
            {
                return this._dataItemId;
            }
            set
            {
                this._dataItemId = value;
                if (value != String.Empty)
                {
                    string[] ids = value.Split('.');
                    DataEntity dataEntity = ServiceUnity.DataEntityComponentService.GetDataEntity(ids[0]);
                    if (dataEntity != null)
                    {
                        this.txtDataItem.Text = dataEntity.Name;
                        if (ids.Length == 2)
                        {
                            List<DataItemEntity> items =
                                (from c in dataEntity.Items.ToList() where c.Id.Equals(ids[1]) select c).ToList();
                            if (items.Count == 1)
                            {
                                DataItemEntity dataItemEntity = items[0];
                                this.txtDataItem.Text += "." + dataItemEntity.Name;
                            }
                            else
                            {
                                this.txtDataItem.Text = String.Empty;
                            }
                        }
                    }
                }
                else
                {
                    this.txtDataItem.Text = String.Empty;
                }
            }
        }
        public DataItemChooseCellEditingControl()
        {
            InitializeComponent();
        }
        private void btnChoose_Click(object sender, EventArgs e)
        {
            using (_formDataEntityTreeChoose = new DataEntityTreeChooseView(this.ShowDataItem))
            {
                _formDataEntityTreeChoose.SelectedId = _dataItemId;
                if (_formDataEntityTreeChoose.ShowDialog() != DialogResult.OK)
                    return;
                this._dataItemId = _formDataEntityTreeChoose.SelectedId;
                this.txtDataItem.Text = _formDataEntityTreeChoose.SelectedName;
            }
            this._valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
        }
        public object EditingControlFormattedValue
        {
            get
            {
                return this.DataItemId;
            }
            set
            {
                String newValue = value as String;
                if (newValue != null)
                {
                    this.DataItemId = newValue;
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
    }
}
