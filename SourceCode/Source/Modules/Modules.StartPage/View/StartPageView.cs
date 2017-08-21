/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sheng.SailingEase.Infrastructure;
using Sheng.SailingEase.Controls;
using Microsoft.Practices.Composite.Events;
namespace Sheng.SailingEase.Modules.StartPageModule.View
{
   
    public partial class StartPageView :  WorkbenchViewBase
    {
        BrowserPane _browserPane = new BrowserPane();
        IEventAggregator _eventAggregator;
        public StartPageView(IEventAggregator eventAggregator, IWorkbenchService workbenchService)
        {
            _eventAggregator = eventAggregator;
            this.HideOnClose = false;
            this.TabText = "Browser";
            _browserPane.View.GetSchemeFunc = (sender, e) =>
            {
                if (e.SchemeName.Equals(StartPageScheme.SCHEMENAME, StringComparison.CurrentCultureIgnoreCase))
                    return StartPageScheme.Instance;
                else
                    return null;
            };
            _browserPane.View.StatusTextChanged = (e) => { workbenchService.SetStatusMessage(e); };
            _browserPane.View.TitleChanged = (e) => { this.TabText = e; };
            _browserPane.View.NewWindow += (sender, e) => { workbenchService.Show(new BrowserView(e.BrowserPane, e.Url)); };
            this.Controls.Add(_browserPane.View);
            _browserPane.Navigate(StartPageScheme.STARTPAGE_URI);
            this.Single = true;
            this.SingleKey = StartPageScheme.STARTPAGE_URI;
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
        }
        public override bool CompareSingleKey(object singleKey)
        {
            if (base.CompareSingleKey(singleKey) &&
                _browserPane.DummyUrl.ToString().Equals(StartPageScheme.STARTPAGE_URI, StringComparison.CurrentCultureIgnoreCase))
                return true;
            else
                return false;
        }
    }
}
