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
namespace Sheng.SailingEase.Infrastructure
{
    public class RuntimeControlDesignSupportAttribute : Attribute
    {
        private Type _entityType;
        public Type EntityType
        {
            get { return _entityType; }
        }
        private string _codeCreationPrefix;
        public string CodeCreationPrefix
        {
            get { return _codeCreationPrefix; }
        }
        public RuntimeControlDesignSupportAttribute(Type entityType, string codeCreationPrefix)
        {
            _entityType = entityType;
            _codeCreationPrefix = codeCreationPrefix;
        }
    }
}
