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
namespace Sheng.SailingEase.Core.Development
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ToolStripItemEntityAttribute: Attribute
    {
        public string Name
        {
            get;
            private set;
        }
        public Type EntityType
        {
            get;
            private set;
        }
        public string Description
        {
            get;
            private set;
        }
        public ToolStripItemEntityAttribute(string name, Type entityType)
        {
            this.Name = name;
            this.EntityType = entityType;
        }
        public ToolStripItemEntityAttribute(string name, Type entityType, string description)
            : this(name, entityType)
        {
            this.Description = description;
        }
    }
}
