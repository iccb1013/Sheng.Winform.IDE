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
using Sheng.SailingEase.Controls;
using System.Windows.Forms;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Composite.Events;
namespace Sheng.SailingEase.Modules.ProjectModule.View
{
    class ProjectStartPageView : WorkbenchViewBase
    {
        BrowserPane _browserPane = new BrowserPane();
        IEventAggregator _eventAggregator;
        public ProjectStartPageView(IEventAggregator eventAggregator, IWorkbenchService workbenchService)
        {
            _eventAggregator = eventAggregator;
            this.HideOnClose = false;
            this.TabText = "Browser";
            _browserPane.View.GetSchemeFunc = (sender, e) =>
            {
                if (e.SchemeName.Equals(ProjectStartPageScheme.SCHEMENAME, StringComparison.CurrentCultureIgnoreCase))
                    return ProjectStartPageScheme.Instance;
                else
                    return null;
            };
            _browserPane.View.StatusTextChanged = (e) => { workbenchService.SetStatusMessage(e); };
            _browserPane.View.TitleChanged = (e) => { this.TabText = e; };
            this.Controls.Add(_browserPane.View);
            _browserPane.Navigate(ProjectStartPageScheme.PROJECT_STARTPAGE_URI);
            this.Single = true;
            this.SingleKey = ProjectStartPageScheme.PROJECT_STARTPAGE_URI;
            this.HideOnClose = true;
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            SubscriptionToken projectSavedEventToken = _eventAggregator.GetEvent<ProjectSavedEvent>().Subscribe((args) =>
             {
                 if (_browserPane.Url.Scheme.Equals(ProjectStartPageScheme.SCHEMENAME, StringComparison.CurrentCultureIgnoreCase))
                 {
                     _browserPane.Navigate(ProjectStartPageScheme.PROJECT_STARTPAGE_URI);
                 }
             });
            this.FormClosed += (sender, e) =>
            {
                _eventAggregator.GetEvent<ProjectSavedEvent>().Unsubscribe(projectSavedEventToken);
            };
        }
        public override bool CompareSingleKey(object singleKey)
        {
            if (base.CompareSingleKey(singleKey) &&
                _browserPane.Url.ToString().Equals(ProjectStartPageScheme.PROJECT_STARTPAGE_URI, StringComparison.CurrentCultureIgnoreCase))
                return true;
            else
                return false;
        }
    }
}
