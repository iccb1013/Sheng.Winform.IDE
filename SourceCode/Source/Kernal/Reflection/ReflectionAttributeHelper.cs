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
using System.Reflection;
namespace Sheng.SailingEase.Kernal
{
    public static class ReflectionAttributeHelper
    {
        public static List<T> GetCustomAttributes<T>(Assembly assembly, bool inherit) where T : Attribute
        {
            List<T> list = new List<T>();
            Type[] exportedTypes = assembly.GetTypes();
            for (int i = 0; i < exportedTypes.Length; i++)
            {
                if (exportedTypes[i].IsClass)
                {
                    object[] attObjs =
                        exportedTypes[i].GetCustomAttributes(typeof(T), inherit);
                    if (attObjs.Length > 0)
                    {
                        list.AddRange(attObjs.Cast<T>());
                    }
                }
            }
            return list;
        }
        public static List<Type> GetCompriseCustomAttributeTypes<T>(Assembly assembly, bool inherit) where T : Attribute
        {
            List<Type> list = new List<Type>();
            Type[] exportedTypes = assembly.GetTypes();
            for (int i = 0; i < exportedTypes.Length; i++)
            {
                if (exportedTypes[i].IsClass)
                {
                    object[] attObjs =
                        exportedTypes[i].GetCustomAttributes(typeof(T), inherit);
                    if (attObjs.Length > 0)
                    {
                        list.Add(exportedTypes[i]);
                    }
                }
            }
            return list;
        }
        public static List<AttributeAndTypeRelation> GetAttributeAndTypeRelation<T>(Assembly assembly, bool inherit) where T : Attribute
        {
            List<AttributeAndTypeRelation> list = new List<AttributeAndTypeRelation>();
            Type[] exportedTypes = assembly.GetTypes();
            for (int i = 0; i < exportedTypes.Length; i++)
            {
                if (exportedTypes[i].IsClass)
                {
                    object[] attObjs =
                        exportedTypes[i].GetCustomAttributes(typeof(T), inherit);
                    foreach (Attribute item in attObjs)
                    {
                        list.Add(new AttributeAndTypeRelation(item, exportedTypes[i]));
                    }
                }
            }
            return list;
        }
        public static List<Type> GetImplementInterfaceTypes<T>(Assembly assembly)
        {
            string interfaceName = typeof(T).Name;
            List<Type> list = new List<Type>();
            Type[] exportedTypes = assembly.GetTypes();
            for (int i = 0; i < exportedTypes.Length; i++)
            {
                if (exportedTypes[i].IsClass)
                {
                    if(exportedTypes[i].GetInterface(interfaceName) != null)
                    {
                        list.Add(exportedTypes[i]);
                    }
                }
            }
            return list;
        }
    }
    public class AttributeAndTypeRelation
    {
        private Attribute _attribute;
        public Attribute Attribute
        {
            get { return _attribute; }
        }
        private Type _type;
        public Type Type
        {
            get { return _type; }
        }
        public AttributeAndTypeRelation(Attribute attribute, Type type)
        {
            _attribute = attribute;
            _type = type;
        }
    }
}
