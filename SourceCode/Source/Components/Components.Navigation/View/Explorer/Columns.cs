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
using Sheng.SailingEase.Components.NavigationComponent.Localisation;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Localisation;
namespace Sheng.SailingEase.Components.NavigationComponent.View
{
    class Columns
    {
        private List<DataGridViewColumn> _menuColumns;
        public List<DataGridViewColumn> MenuColumns
        {
            get
            {
                if (_menuColumns == null)
                {
                    _menuColumns = new List<DataGridViewColumn>();
                    DataGridViewImageBinderColumn iconColumn = new DataGridViewImageBinderColumn()
                    {
                        Width = 25
                    };
                    iconColumn.Mapping(typeof(MenuEntity),IconsLibrary.Menu,true);
                    _menuColumns.Add(iconColumn);
                    _menuColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Name",
                        HeaderText = Language.Current.Explorer_GridColumns_MenuColumns_Name,
                        Width = 180
                    });
                    _menuColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Code",
                        HeaderText = Language.Current.Explorer_GridColumns_MenuColumns_Code,
                        Width = 180
                    });
                }
                return _menuColumns;
            }
        }
        private List<DataGridViewColumn> _toolStripPageColumns;
        public List<DataGridViewColumn> ToolStripPageColumns
        {
            get
            {
                if (_toolStripPageColumns == null)
                {
                    _toolStripPageColumns = new List<DataGridViewColumn>();
                    _toolStripPageColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Name",
                        HeaderText = Language.Current.Explorer_GridColumns_ToolStripColumns_Name,
                        Width = 180
                    });
                    _toolStripPageColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Code",
                        HeaderText = Language.Current.Explorer_GridColumns_ToolStripColumns_Code,
                        Width = 180
                    });
                }
                return _toolStripPageColumns;
            }
        }
        private List<DataGridViewColumn> _toolStripGroupColumns;
        public List<DataGridViewColumn> ToolStripGroupColumns
        {
            get
            {
                if (_toolStripGroupColumns == null)
                {
                    _toolStripGroupColumns = new List<DataGridViewColumn>();
                    _toolStripGroupColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Name",
                        HeaderText = Language.Current.Explorer_GridColumns_ToolStripColumns_Name,
                        Width = 180
                    });
                    _toolStripGroupColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Code",
                        HeaderText = Language.Current.Explorer_GridColumns_ToolStripColumns_Code,
                        Width = 180
                    });
                }
                return _toolStripGroupColumns;
            }
        }
        private List<DataGridViewColumn> _toolStripItemColumns;
        public List<DataGridViewColumn> ToolStripItemColumns
        {
            get
            {
                if (_toolStripItemColumns == null)
                {
                    _toolStripItemColumns = new List<DataGridViewColumn>();
                    _toolStripItemColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Name",
                        HeaderText = Language.Current.Explorer_GridColumns_ToolStripColumns_Name,
                        Width = 180
                    });
                    _toolStripItemColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Code",
                        HeaderText = Language.Current.Explorer_GridColumns_ToolStripColumns_Code,
                        Width = 180
                    });
                }
                return _toolStripItemColumns;
            }
        }
    }
}
