/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Windows.Forms;
using Sheng.SailingEase.Controls;
using Sheng.SailingEase.Kernal;
using Sheng.SailingEase.Infrastructure;
using System.IO;
namespace Sheng.SailingEase.Modules.StartPageModule.View
{
    class StartPageScheme : DefaultHtmlViewSchemeExtension
    {
        public const string SCHEMENAME = "startpage";
        public const string STARTPAGE_URI = "startpage://start/";
        public const string OPENPROJECT_URI = "startpage://project/";
        public const string OPENPROJECT_HOST = "project";
        public const string START_HOST = "start";
        public const string HELP_HOST = "help";
        private StartPageCodePage _page;
        private IProjectService _projectService;
        private static InstanceLazy<StartPageScheme> _instance =
            new InstanceLazy<StartPageScheme>(() => new StartPageScheme());
        public static StartPageScheme Instance
        {
            get { return _instance.Value; }
        }
        private StartPageScheme()
        {
            _projectService = ServiceUnity.Container.Resolve<IProjectService>();
        }
        public override void InterceptNavigate(HtmlViewPane pane, WebBrowserNavigatingEventArgs e)
        {
            e.Cancel = true;
            if (_page == null)
            {
                _page = new StartPageCodePage();
            }
            string host = e.Url.Host;
            if (host.Equals(OPENPROJECT_HOST, StringComparison.CurrentCultureIgnoreCase))
            {
                string projectFile = e.Url.LocalPath.TrimStart('/');
                if (String.IsNullOrEmpty(projectFile) == false)
                {
                    FileInfo fileInfo = new FileInfo(projectFile);
                    if (fileInfo.Exists)
                    {
                        _projectService.OpenProject(fileInfo.FullName);
                    }
                }
            }
            else
            {
                pane.WebBrowser.DocumentText = _page.Render(host);
            }
        }
        public override void DocumentCompleted(HtmlViewPane pane, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlElement btn = null;
            btn = pane.WebBrowser.Document.GetElementById("btnOpenProject");
            if (btn != null)
            {
                btn.Click += delegate { _projectService.OpenProject(); };
            }
            btn = pane.WebBrowser.Document.GetElementById("btnNewProject");
            if (btn != null)
            {
                btn.Click += delegate { _projectService.NewProject(); };
            }
            pane.WebBrowser.Navigating += pane_WebBrowser_Navigating;
            pane.WebBrowser.Navigated += pane_WebBrowser_Navigated;
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
        public override void GoHome(HtmlViewPane pane)
        {
            pane.Navigate(STARTPAGE_URI);
        }
    }
}
