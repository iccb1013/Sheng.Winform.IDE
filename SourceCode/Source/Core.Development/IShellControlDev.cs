/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Sheng.SailingEase.Core.Development
{
    public interface IShellControlDev
    {
       
        EntityBase Entity { get; set; }
        void InitializationEntity(EntityBase entity);
        bool ViewUpdating { get; set; }
        void UpdateView();
        void UpdateEntity();
        string GetCode();
        string GetName();
        string GetControlTypeName();
        void ClearEntity();
        string GetText();
        EventCollection GetEvents();
    }
}
