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
using System.Windows.Forms;
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Core.Development
{
    class EventTreeNode:TreeNode
    {
        private EventBase _event;
        public EventBase Event
        {
            get { return _event; }
            set
            {
                _event = value;
                this.Text = _event.Name;
            }
        }
        public EventTreeNode(EventBase eventBase)
        {
            this.ImageIndex = this.SelectedImageIndex = EventTreeView.Images.Method;
            this.Event = eventBase;
        }
    }
}
