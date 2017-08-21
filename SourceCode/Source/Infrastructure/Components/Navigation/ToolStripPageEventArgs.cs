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
    public class ToolStripPageEventArgs
    {
        private ToolStripPageEntity _entity;
        public ToolStripPageEntity Entity
        {
            get { return _entity; }
        }
        public ToolStripPageEventArgs(ToolStripPageEntity page)
        {
            _entity = page;
        }
    }
    public class ToolStripPageMoveBeforeEventArgs
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
        public ToolStripPageMoveBeforeEventArgs(string id, string beforeId)
        {
            _id = id;
            _beforeId = beforeId;
        }
    }
    public class ToolStripPageMoveAfterEventArgs
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
        public ToolStripPageMoveAfterEventArgs(string id, string afterId)
        {
            _id = id;
            _afterId = afterId;
        }
    }
}
