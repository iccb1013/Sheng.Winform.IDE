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
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Core
{
    public interface IDataItemDataSourceRelationship
    {
        string DataItem { get; }
        DataSource Source { get; }
        EnumMatchType MatchType { get; }
    }
}
