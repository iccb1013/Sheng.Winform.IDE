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
namespace Sheng.SailingEase.Kernal
{
    public class SEDataTable : DataTable
    {
        public enum DefaultColumns
        {
            TextValue = 0
        }
        public SEDataTable()
        {
        }
        public SEDataTable(DefaultColumns defaultColumns)
        {
            switch (defaultColumns)
            {
                case DefaultColumns.TextValue:
                    AddColumns("Text", "Value");
                    break;
            }
        }
        public SEDataTable(params string[] columnNames)
        {
            AddColumns(columnNames);
        }
        public SEDataTable(string [] columnNames,Type [] types)
        {
            if (columnNames.Length != types.Length)
            {
                throw new ArgumentOutOfRangeException();
            }
            for (int i = 0; i < columnNames.Length; i++)
            {
                this.Columns.Add(columnNames[i], types[i]);
            }
        }
        public void AddColumns(params string[] columnNames)
        {
            foreach (string name in columnNames)
            {
                this.Columns.Add(name);
            }
        }
        public void AddRow(params object[] values)
        {
            if (values.Length != this.Columns.Count)
            {
                throw new ArgumentOutOfRangeException();
            }
            DataRow dr = this.NewRow();
            for (int i = 0; i < this.Columns.Count; i++)
            {
                dr[i] = values[i];
            }
            this.Rows.Add(dr);
        }
    }
}
