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
namespace Sheng.SailingEase.Controls
{
    public delegate void OnHtmlViewPaneNewWindowHandler(object sender, HtmlViewPaneNewWindowEventArgs args);
    public class HtmlViewPaneNewWindowEventArgs : EventArgs
    {
        public HtmlViewPaneNewWindowEventArgs(BrowserPane browserPane,Uri url)
        {
            Url = url;
            BrowserPane = browserPane;
        }
        public Uri Url
        {
            get;
            private set;
        }
        public BrowserPane BrowserPane
        {
            get;
            private set;
        }
    }
    public class HtmlViewPaneGetSchemeEventArgs : EventArgs
    {
        public HtmlViewPaneGetSchemeEventArgs(string schemeName)
        {
            SchemeName = schemeName;
        }
        public string SchemeName
        {
            get;
            private set;
        }
    }
}
