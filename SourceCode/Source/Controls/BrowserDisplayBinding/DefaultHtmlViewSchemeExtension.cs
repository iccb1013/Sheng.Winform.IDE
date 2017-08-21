/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Windows.Forms;
namespace Sheng.SailingEase.Controls
{
   public  class DefaultHtmlViewSchemeExtension : IHtmlViewSchemeExtension
    {
        public virtual void InterceptNavigate(HtmlViewPane pane, WebBrowserNavigatingEventArgs e) { }
        public virtual void DocumentCompleted(HtmlViewPane pane, WebBrowserDocumentCompletedEventArgs e) { }
        public virtual void GoHome(HtmlViewPane pane)
        {
            pane.Navigate(HtmlViewPane.DefaultHomepage);
        }
        public virtual void GoSearch(HtmlViewPane pane)
        {
            pane.Navigate(HtmlViewPane.DefaultSearchUrl);
        }
    }
}
