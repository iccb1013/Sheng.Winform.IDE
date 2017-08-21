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
using Sheng.SailingEase.Infrastructure;
using Microsoft.Practices.Unity;
using Sheng.SailingEase.Shell.View;
using Sheng.SailingEase.Controls.Extensions;
namespace Sheng.SailingEase.Shell
{
    class NavigationService : INavigationService
    {
        IUnityContainer _container;
        IShellView _shellView;
        public NavigationService(IUnityContainer container)
        {
            _container = container;
            _shellView = _container.Resolve<IShellView>();
        }
        public void RegisterMenu(string path, IToolStripItemCodon toolStripItem)
        {
            _shellView.RegisterMenu(path, toolStripItem);
        }
        public void RegisterToolStrip(string path, IToolStripItemCodon toolStripItem)
        {
            _shellView.RegisterToolStrip(path, toolStripItem);
        }
    }
}
