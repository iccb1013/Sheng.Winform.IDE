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
    public interface IHtmlViewSchemeExtension
    {
        void InterceptNavigate(HtmlViewPane pane, WebBrowserNavigatingEventArgs e);
        void DocumentCompleted(HtmlViewPane pane, WebBrowserDocumentCompletedEventArgs e);
        void GoHome(HtmlViewPane pane);
        void GoSearch(HtmlViewPane pane);
    }
}
