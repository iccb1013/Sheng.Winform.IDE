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
using Microsoft.Practices.Unity;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Shell.View;
using Sheng.SailingEase.Controls.Extensions;
using System.Drawing;
namespace Sheng.SailingEase.Shell
{
    class WorkbenchService : IWorkbenchService
    {
        private IUnityContainer _container;
        private IShellView _shellView;
        public WorkbenchService(IUnityContainer container)
        {
            _container = container;
            _shellView = _container.Resolve<IShellView>();
        }
        public System.Windows.Forms.Form Form
        {
            get { return _shellView.Form; }
        }
        public Point ToolStripPanelLocation
        {
            get { return _shellView.ToolStripPanelLocation; }
        }
        public Point PointToScreen(Point point)
        {
            return _shellView.PointToScreen(point);
        }
        public void Show<T>() where T : IView
        {
            _shellView.Show<T>();
        }
        public void Show<T>(object singleKey) where T : IView
        {
            _shellView.Show<T>(singleKey);
        }
        public void Show(IView view)
        {
            _shellView.Show(view);
        }
        public void Show(Func<IView> func, object singleKey)
        {
            _shellView.Show(func, singleKey);
        }
        public void CloseView(IView view)
        {
            _shellView.CloseView(view);
        }
        public void SetStatusMessage(string msg)
        {
            _shellView.SetStatusMessage(msg);
        }
        public void ActivateToolStrip(ToolStripCodon toolStripCodon)
        {
            _shellView.ActivateToolStrip(toolStripCodon);
        }
        public void DeactiveToolStrip(ToolStripCodon toolStripCodon)
        {
            _shellView.DeactiveToolStrip(toolStripCodon);
        }
        public void DestroyToolStrip(List<ToolStripCodon> toolStripCodonList)
        {
            _shellView.DestroyToolStrip(toolStripCodonList);
        }
        public T GetView<T>() where T : IView
        {
            return _shellView.GetView<T>();
        }
        public void CloseAllViewButThis()
        {
            _shellView.CloseAllViewButThis();
        }
        public void CloseAllView()
        {
            _shellView.CloseAllView();
        }
        public void CloseView()
        {
            _shellView.CloseView();
        }
    }
}
