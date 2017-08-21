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
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.RuntimeContract
{
    public interface IRuntimeForm
    {
        string SendDataXml { get; set; }
        WindowEntity Entity { get; set; }
        IRuntimeForm CallerForm { get; set; }
        bool IsDisposed { get; }
        IControlValue GetControlValue(string code);
        void SetControlValue(string code, object value);
        IRuntimeControl GetControl(string code);
        void Close();
    }
}
