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
namespace Sheng.SailingEase.IDataBaseProvide
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class FieldProvideAttribute : Attribute
    {
        public const string NameProperty = "Name";
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
        public const string DescriptionProperty = "Description";
        public string Description
        {
            get;
            private set;
        }
        public FieldProvideAttribute(string name, int code)
            : this(name, String.Empty, code)
        {
        }
        public FieldProvideAttribute(string name, string description, int code)
        {
            this.Name = name;
            this.Description = description;
            this.Code = code;
        }
    }
}
