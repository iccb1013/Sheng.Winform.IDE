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
    public abstract class DataSource2
    {
        public abstract string Name { get; }
        public abstract string String { get; }
        public abstract object SourceItem { get; }
        public override string ToString()
        {
            return String;
        }
    }
}
