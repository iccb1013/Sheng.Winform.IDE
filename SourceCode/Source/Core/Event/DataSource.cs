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
namespace Sheng.SailingEase.Core
{
    [Serializable]
    public class DataSource
    {
        public DataSource()
        {
        }
        public DataSource(string dataSource)
        {
            string[] source = dataSource.Split('.');
            this.Type = (EnumEventDataSource)Enum.Parse(typeof(EnumEventDataSource), source[0]);
            this.Source = source[1];
        }
        public EnumEventDataSource Type
        {
            get;
            set;
        }
        public string Source
        {
            get;
            set;
        }
        public override string ToString()
        {
            string str = "{0}.{1}";
            str = String.Format(str, this.Type.ToString(), this.Source);
            return str;
        }
    }
}
