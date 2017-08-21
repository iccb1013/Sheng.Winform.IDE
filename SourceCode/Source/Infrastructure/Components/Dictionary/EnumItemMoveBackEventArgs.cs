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
namespace Sheng.SailingEase.Infrastructure
{
    public class EnumItemMoveBackEventArgs
    {
        public string EnumId { get; set; }
        public string Id { get; set; }
        public string AfterId { get; set; }
        public EnumItemMoveBackEventArgs(string enumId, string id, string afterId)
        {
            EnumId = enumId;
            Id = id;
            AfterId = afterId;
        }
    }
}
