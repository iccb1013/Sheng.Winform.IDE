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
using Sheng.SailingEase.IDataBaseProvide;
namespace Sheng.SailingEase.Core
{
    public static class DataBaseProvide
    {
        private static IDataBase _current = new SQLServer2005Provide.SQLServer2005();
        public static IDataBase Current
        {
            get { return _current; }
        }
    }
}
