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
    public class ToolStripGroupEventArgs
    {
        private ToolStripGroupEntity _entity;
        public ToolStripGroupEntity Entity
        {
            get { return _entity; }
        }
        public ToolStripGroupEventArgs(ToolStripGroupEntity group)
        {
            _entity = group;
        }
    }
    public class ToolStripGroupMoveBeforeEventArgs
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
        public ToolStripGroupMoveBeforeEventArgs(string id, string beforeId)
        {
            _id = id;
            _beforeId = beforeId;
        }
    }
    public class ToolStripGroupMoveAfterEventArgs
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
        public ToolStripGroupMoveAfterEventArgs(string id, string afterId)
        {
            _id = id;
            _afterId = afterId;
        }
    }
}
