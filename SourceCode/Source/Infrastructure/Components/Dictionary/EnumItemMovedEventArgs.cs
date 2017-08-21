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
namespace Sheng.SailingEase.Infrastructure
{
    public class EnumItemMoveAlongEventArgs
    {
        public string EnumId { get; set; }
        public string Id { get; set; }
        public string BeforeId { get; set; }
        public EnumItemMoveAlongEventArgs(string enumId, string id, string beforeId)
        {
            EnumId = enumId;
            Id = id;
            BeforeId = beforeId;
        }
    }
}
