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
namespace Sheng.SailingEase.Controls
{
    public class BrowserPane : IDisposable
    {
        private HtmlViewPane _htmlViewPane;
        public HtmlViewPane View
        {
            get
            {
                return _htmlViewPane;
            }
        }
        public Uri Url
        {
            get
            {
                return _htmlViewPane.Url;
            }
        }
        public string DummyUrl
        {
            get { return _htmlViewPane.DummyUrl; }
        }
        protected BrowserPane(bool showNavigation)
        {
            _htmlViewPane = new HtmlViewPane(showNavigation);
            _htmlViewPane.Dock = DockStyle.Fill;
        }
        public BrowserPane()
            : this(true)
        {
        }
        public void Dispose()
        {
            _htmlViewPane.Dispose();
        }
        public void Navigate(string url)
        {
            _htmlViewPane.Navigate(url);
        }
    }
}
