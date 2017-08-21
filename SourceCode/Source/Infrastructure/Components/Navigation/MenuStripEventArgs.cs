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
    public class MenuStripEventArgs
    {
        public MenuEntity Entity { get; set; }
        public MenuStripEventArgs(MenuEntity entity)
        {
            this.Entity = entity;
        }
    }
    public class MenuStripItemMoveBeforeEventArgs
    {
        private string _id;
        public string Id
        {
            get { return _id; }
        }
        private string _beforeId;
        public string BeforeId
        {
            get { return _beforeId; }
        }
        public MenuStripItemMoveBeforeEventArgs(string id, string beforeId)
        {
            _id = id;
            _beforeId = beforeId;
        }
    }
    public class MenuStripItemMoveAfterEventArgs
    {
        private string _id;
        public string Id
        {
            get { return _id; }
        }
        private string _afterId;
        public string AfterId
        {
            get { return _afterId; }
        }
        public MenuStripItemMoveAfterEventArgs(string id, string afterId)
        {
            _id = id;
            _afterId = afterId;
        }
    }
}
