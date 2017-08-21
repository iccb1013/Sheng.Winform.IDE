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
using Sheng.SailingEase.Controls;
namespace Sheng.SailingEase.Infrastructure
{
    public partial class BrowserView : WorkbenchViewBase
    {
        IWorkbenchService _workbenchService;
        public BrowserView(BrowserPane browserPane, Uri url)
        {
            InitializeComponent();
            this.HideOnClose = false;
            this.TabText = "Browser";
            _workbenchService = ServiceUnity.Container.Resolve<IWorkbenchService>();
            browserPane.View.StatusTextChanged = (e) => { _workbenchService.SetStatusMessage(e); };
            browserPane.View.TitleChanged = (e) => { this.TabText = e; };
            browserPane.View.NewWindow += (sender, e) => { _workbenchService.Show(new BrowserView(e.BrowserPane, e.Url)); };
            this.Controls.Add(browserPane.View);
             browserPane.Navigate(url.ToString());
        }
    }
}
