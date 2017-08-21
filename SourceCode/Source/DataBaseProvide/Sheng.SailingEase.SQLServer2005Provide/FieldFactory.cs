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
using Sheng.SailingEase.IDataBaseProvide;
using System.Reflection;
namespace Sheng.SailingEase.SQLServer2005Provide
{
    class FieldFactory : IFieldFactory
    {
        private FieldTypeCollection _fields;
        private object _objLock = new object();
        public FieldFactory()
        {
            LoadField();
        }
        private void LoadField()
        {
            if (_fields == null)
            {
                lock (_objLock)
                {
                    if (_fields == null)
                    {
                        _fields = new FieldTypeCollection();
                        Type[] exportedTypes = Assembly.GetExecutingAssembly().GetTypes();
                        for (int i = 0; i < exportedTypes.Length; i++)
                        {
                            if (exportedTypes[i].IsClass)
                            {
                                if (exportedTypes[i].GetInterfaces().Contains(typeof(IField)))
                                {
                                    _fields.Add(exportedTypes[i]);
                                }
                            }
                        }
                    }
                }
            }
        }
        public IField CreateInstance(int code)
        {
            return _fields.CreateInstance(code);
        }
        public IField CreateInstance(FieldProvideAttribute attribute)
        {
            return _fields.CreateInstance(attribute);
        }
        public FieldProvideAttribute GetProvideAttribute(int code)
        {
            return _fields.GetProvideAttribute(code);
        }
        public FieldProvideAttribute GetProvideAttribute(IField field)
        {
            return _fields.GetProvideAttribute(field);
        }
        public FieldProvideAttribute GetProvideAttribute(Type type)
        {
            return _fields.GetProvideAttribute(type);
        }
        public List<FieldProvideAttribute> GetProvideAttributeList()
        {
            return _fields.GetProvideAttributeList();
        }
        public string GetName(IField field)
        {
            return GetProvideAttribute(field).Name;
        }
        public List<Type> GetAvailableDataItems()
        {
            return _fields.ToList();
        }
    }
}
