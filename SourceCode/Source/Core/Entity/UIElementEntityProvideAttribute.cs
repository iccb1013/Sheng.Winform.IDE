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
namespace Sheng.SailingEase.Core
{
   
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class UIElementEntityProvideAttribute : Attribute
    {
        public virtual string Name
        {
            get;
            set;
        }
        public int Code
        {
            get;
            private set;
        }
        public UIElementEntityProvideAttribute(string name,  int code)
        {
            this.Name = name;
            this.Code = code;
        }
    }
}
