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
    public class EventProvideAttribute : Attribute
    {
        public const string Property_Name = "Name";
        public string Name
        {
            get;
            private set;
        }
        public int Code
        {
            get;
            private set;
        }
        public const string Property_Description = "Description";
        public string Description
        {
            get;
            private set;
        }
        public EventProvideAttribute(string name, int code)
        {
            this.Name = name;
            this.Code = code;
        }
        public EventProvideAttribute(string name, int code, string description)
            : this(name, code)
        {
            this.Description = description;
        }
    }
}
