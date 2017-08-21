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
    public interface IWizardView
    {
        bool CloseButtonEnabled { set; }
        bool BackButtonEnabled { get; set; }
        bool NextButtonEnabled { get; set; }
        bool FinishButtonEnabled { get; set; }
        void NextPanel();
        void SetData(string name, object data);
        object GetData(string name);
        void SetOptionInstance<T>(T option) where T : class;
        T GetOptionInstance<T>() where T : class;
    }
}
