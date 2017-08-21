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
using System.Diagnostics;
namespace Sheng.SailingEase.Core.Development
{
    [ToolboxItem(false)]
    partial class ImageResourceChooseCellEditingControl : UserControl, IDataGridViewEditingControl
    {
        public string ResourceName
        {
            get
            {
                return this.txtResourceName.Text;
            }
            set
            {
                this.txtResourceName.Text = value;
            }
        }
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;
        public ImageResourceChooseCellEditingControl()
        {
            InitializeComponent();
        }
        private void btnChoose_Click(object sender, EventArgs e)
        {
            ImageResourceInfo imageResource;
            if (DialogUnity.ImageResourceChoose(out imageResource))
            {
                if (imageResource != null)
                    this.txtResourceName.Text = imageResource.Name;
                else
                    this.txtResourceName.Text = String.Empty;
                valueChanged = true;
                this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            }
        }
        public object EditingControlFormattedValue
        {
            get
            {
                return ResourceName;
            }
            set
            {
                String newValue = value as String;
                if (newValue != null)
                {
                    ResourceName = newValue;
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
