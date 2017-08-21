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
using System.Xml.Linq;
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Core.Development
{
    public interface IEventEditorPanel
    {
        string PanelTitle { get; }
        bool DefaultPanel { get; }
        EventEditorAdapterAbstract HostAdapter { get; }
       
        List<XObject> GetXml();
        void SetParameter(EventBase even);
        bool ValidateParameter(out string validateMsg);
    }
}
