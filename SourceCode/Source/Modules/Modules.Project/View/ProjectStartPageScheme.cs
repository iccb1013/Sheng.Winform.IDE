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
using Sheng.SailingEase.Controls;
using Microsoft.Practices.Unity;
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Modules.ProjectModule.View
{
    class ProjectStartPageScheme : DefaultHtmlViewSchemeExtension
    {
        public const string PROJECT_STARTPAGE_URI = "project://start/";
        public const string SCHEMENAME = "project";
        private ProjectCodePage _page;
        private static InstanceLazy<ProjectStartPageScheme> _instance =
            new InstanceLazy<ProjectStartPageScheme>(() => new ProjectStartPageScheme());
        public static ProjectStartPageScheme Instance
        {
            get { return _instance.Value; }
        }
        private ProjectStartPageScheme()
        {
        }
        public override void InterceptNavigate(HtmlViewPane pane, WebBrowserNavigatingEventArgs e)
        {
            e.Cancel = true;
            if (_page == null)
            {
                _page = new ProjectCodePage();
            }
            string host = e.Url.Host;
            pane.WebBrowser.DocumentText = _page.Render(host);
        }
        public override void DocumentCompleted(HtmlViewPane pane, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlElement btn = null;
            btn = pane.WebBrowser.Document.GetElementById("btnOpenProject");
            if (btn != null)
            {
            }
            btn = pane.WebBrowser.Document.GetElementById("btnNewProject");
            if (btn != null)
            {
            }
            pane.WebBrowser.Navigating += pane_WebBrowser_Navigating;
            pane.WebBrowser.Navigated += pane_WebBrowser_Navigated;
        }
        public override void GoHome(HtmlViewPane pane)
        {
            pane.Navigate(PROJECT_STARTPAGE_URI);
        }
        void pane_WebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            try
            {
                WebBrowser webBrowser = (WebBrowser)sender;
                webBrowser.Navigating -= pane_WebBrowser_Navigating;
                webBrowser.Navigated -= pane_WebBrowser_Navigated;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void pane_WebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            try
            {
                if (e.Url.IsFile)
                {
                    e.Cancel = true;
                    string file = e.Url.LocalPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
