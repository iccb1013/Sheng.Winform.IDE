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
using Sheng.SailingEase.ComponentModel;
namespace Sheng.SailingEase.Core
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class NormalUIElementEntityProvideAttribute : CoreFormElementEntityProvideAttribute
    {
        public NormalUIElementEntityProvideAttribute(string name, int code)
            : base(name, code)
        {
        }
        public override bool Equals(object obj)
        {
            NormalUIElementEntityProvideAttribute attr = obj as NormalUIElementEntityProvideAttribute;
            if (attr == null)
                return false;
            return this.Code == attr.Code;
        }
    }
}
