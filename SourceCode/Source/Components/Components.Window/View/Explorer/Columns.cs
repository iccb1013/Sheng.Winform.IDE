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
using Sheng.SailingEase.Components.WindowComponent.Localisation;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Core;
using Sheng.SailingEase.Localisation;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Components.WindowComponent.View
{
    class Columns
    {
        public Columns()
        {
        }
        private List<DataGridViewColumn> _gridColumns;
        public List<DataGridViewColumn> GridColumns
        {
            get
            {
                if (_gridColumns == null)
                {
                    _gridColumns = new List<DataGridViewColumn>();
                    DataGridViewImageBinderColumn iconColumn = new DataGridViewImageBinderColumn()
                    {
                        Width = 25
                    };
                    iconColumn.Mapping(typeof(FolderEntityIndex), IconsLibrary.Folder, true);
                    iconColumn.Mapping(typeof(WindowEntityIndex), IconsLibrary.Form, true);
                    _gridColumns.Add(iconColumn);
                    _gridColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Name",
                        HeaderText = Language.Current.Explorer_GridColumns_Name,
                        Width = 180
                    });
                    _gridColumns.Add(new DataGridViewTextBoxColumn()
                    {
                        DataPropertyName = "Code",
                        HeaderText = Language.Current.Explorer_GridColumns_Code,
                        Width = 180
                    });
                }
                return _gridColumns;
            }
        }
    }
}
