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
using System.Diagnostics;
namespace Sheng.SailingEase.Kernal
{
    class TypeBinderTabPage : TabPage
    {
        private Type _boundType;
        public Type BoundType
        {
            get { return _boundType; }
        }
        private Control _view;
        public Control View
        {
            get { return _view; }
        }
        public TypeBinderTabPage(string text, Type boundType, Control view)
        {
            Text = text;
            _boundType = boundType;
            _view = view;
            Debug.Assert(view != null, "view为null");
            if (view != null)
            {
                view.Dock = DockStyle.Fill;
                this.Controls.Add(view);
            }
        }
    }
}
