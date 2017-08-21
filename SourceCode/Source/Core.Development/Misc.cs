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
using System.Data;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Core.Development.Localisation;
namespace Sheng.SailingEase.Core.Development
{
    static class Misc
    {
        private static SEDataTable _trueFalseTable;
        public static DataTable TrueFalseTable
        {
            get
            {
                if (_trueFalseTable == null)
                {
                    _trueFalseTable = new SEDataTable(new string[] { "Text", "Value" }, new Type[] { typeof(string), typeof(bool) });
                    _trueFalseTable.AddRow(Language.Current.TrueFalseTable_False, false);
                    _trueFalseTable.AddRow(Language.Current.TrueFalseTable_True, true);
                }
                return _trueFalseTable;
            }
        }
    }
}
