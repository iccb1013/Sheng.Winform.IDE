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
    public class WindowEventArgs
    {
        private WindowEntity _entity;
        public WindowEntity Entity
        {
            get { return _entity; }
            set { _entity = value; }
        }
        public WindowEventArgs(WindowEntity entity)
        {
            _entity = entity;
        }
    }
    public class WindowRemovedEventArgs
    {
        private string _id;
        public string Id
        {
            get { return _id; }
        }
        public WindowRemovedEventArgs(string id)
        {
            _id = id;
        }
    }
}
