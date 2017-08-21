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
using Sheng.SailingEase.Controls.Extensions;
using Sheng.SailingEase.Shell.View;
using Microsoft.Practices.Composite.Events;
using Sheng.SailingEase.Infrastructure;
namespace Sheng.SailingEase.Shell.View
{
    class ShellViewPresenter
    {
        IShellView _view;
        IEventAggregator _eventAggregator;
        public ShellViewPresenter(IShellView view)
        {
            _view = view;
            _eventAggregator = ServiceUnity.Container.Resolve<IEventAggregator>();
        }
        public void ApplicationRun()
        {
            _eventAggregator.GetEvent<ApplicationRunEvent>().Publish(new NullArgs());
        }
    }
}
