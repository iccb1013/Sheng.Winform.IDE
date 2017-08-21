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
    class EventTimeTreeNode : TreeNode
    {
         private EventTimeAbstract _eventTime;
        public EventTimeAbstract EventTime
        {
            get { return _eventTime; }
            set
            {
                _eventTime = value;
                this.Text = _eventTime.Name;
            }
        }
        public EventTimeTreeNode()
        {
        }
        public EventTimeTreeNode(EventTimeAbstract eventTime)
        {
            this.ImageIndex = this.SelectedImageIndex = EventTreeView.Images.Event;
            this.EventTime = eventTime;
        }
    }
}
