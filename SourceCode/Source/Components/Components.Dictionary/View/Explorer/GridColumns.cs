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
using Sheng.SailingEase.Components.DictionaryComponent.Localisation;
namespace Sheng.SailingEase.Components.DictionaryComponent.View
{
    class GridColumns
    {
        public GridColumns()
        {
        }
        private List<DataGridViewColumn> _enumColumns;
        public List<DataGridViewColumn> EnumColumns
        {
            get
            {
                if (_enumColumns == null)
                {
                    _enumColumns = new List<DataGridViewColumn>();
                    _enumColumns.Add(new DataGridViewImageColumn()
                    {
                        DataPropertyName = "Icon",
                        Width = 25
                    });
                    _enumColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Id",
                        Visible = false
                    });
                    _enumColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Name",
                        HeaderText = Language.Current.Explorer_GridColumns_EnumColumns_Name,
                        Width = 180,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    });
                }
                return _enumColumns;
            }
        }
        private List<DataGridViewColumn> _enumItemColumns;
        public List<DataGridViewColumn> EnumItemColumns
        {
            get
            {
                if (_enumItemColumns == null)
                {
                    _enumItemColumns = new List<DataGridViewColumn>();
                    _enumItemColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Id",
                        Visible = false
                    });
                    _enumItemColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Text",
                        HeaderText = Language.Current.Explorer_GridColumns_EnumItemColumns_Text,
                        Width = 180
                    });
                    _enumItemColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Value",
                        HeaderText = Language.Current.Explorer_GridColumns_EnumItemColumns_Value,
                        Width = 180
                    });
                }
                return _enumItemColumns;
            }
        }
    }
}
