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
using Sheng.SailingEase.Components.DataEntityComponent.Localisation;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Components.DataEntityComponent.View
{
    class GridColumns
    {
        public GridColumns()
        {
        }
        private List<DataGridViewColumn> _dataEntityColumns;
        public List<DataGridViewColumn> DataEntityColumns
        {
            get
            {
                if (_dataEntityColumns == null)
                {
                    _dataEntityColumns = new List<DataGridViewColumn>();
                    _dataEntityColumns.Add(new DataGridViewImageColumn()
                    {
                        DataPropertyName = "Icon",
                        Width = 25
                    });
                    _dataEntityColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Id",
                        Visible = false
                    });
                    _dataEntityColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Name",
                        HeaderText = Language.Current.GridViewController_DataEntityColumns_Name,
                        Width = 180
                    });
                    _dataEntityColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Code",
                        HeaderText = Language.Current.GridViewController_DataEntityColumns_Code,
                        Width = 180
                    });
                }
                return _dataEntityColumns;
            }
        }
        private List<DataGridViewColumn> _dataItemEntityColumns;
        public List<DataGridViewColumn> DataItemEntityColumns
        {
            get
            {
                if (_dataItemEntityColumns == null)
                {
                    _dataItemEntityColumns = new List<DataGridViewColumn>();
                    _dataItemEntityColumns.Add(new DataGridViewImageColumn()
                    {
                        DataPropertyName = "Icon",
                        Width = 25
                    });
                    _dataItemEntityColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Id",
                        Visible = false
                    });
                    _dataItemEntityColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Name",
                        HeaderText = Language.Current.GridViewController_DataItemEntityColumns_Name,
                        Width = 180
                    });
                    _dataItemEntityColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Code",
                        HeaderText = Language.Current.GridViewController_DataItemEntityColumns_Code,
                        Width = 180
                    });
                    _dataItemEntityColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "TypeName",
                        HeaderText = Language.Current.GridViewController_DataItemEntityColumns_TypeName,
                        Width = 130
                    });
                    _dataItemEntityColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "DefaultValue",
                        HeaderText = Language.Current.GridViewController_DataItemEntityColumns_DefaultValue,
                        Width = 100
                    });
                    _dataItemEntityColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Length",
                        HeaderText = Language.Current.GridViewController_DataItemEntityColumns_Length,
                        Width = 100
                    });
                    _dataItemEntityColumns.Add(new SEDataGridViewCheckBoxColumn()
                    {
                        DataPropertyName = "AllowEmpty",
                        HeaderText = Language.Current.GridViewController_DataItemEntityColumns_AllowEmpty,
                        Width = 50
                    });
                }
                return _dataItemEntityColumns;
            }
        }
    }
}
