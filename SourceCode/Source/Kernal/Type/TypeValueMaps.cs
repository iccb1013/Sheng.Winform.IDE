/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Diagnostics;
namespace Sheng.SailingEase.Kernal
{
    public class TypeValueMaps<T>
    {
        private Dictionary<Type, T> _types = new Dictionary<Type, T>();
        private bool _actOnSub = false;
        public bool ActOnSub
        {
            get { return _actOnSub; }
            set { _actOnSub = value; }
        }
        public TypeValueMaps()
        {
        }
        public bool ContainsKey(Type key)
        {
            if (key == null)
            {
                Debug.Assert(false, "key 为空");
                return false;
            }
            if (_actOnSub == false)
                return _types.ContainsKey(key);
            else
            {
                foreach (KeyValuePair<Type, T> item in _types)
                {
                    if (item.Key.Equals(key))
                        return true;
                    else
                    {
                        if (item.Key.IsInterface)
                        {
                            Type interfaceType = key.GetInterface(item.Key.Name, false);
                            if (interfaceType != null)
                                return true;
                        }
                        else
                        {
                            if (key.IsSubclassOf(item.Key))
                                return true;
                        }
                    }
                }
                return false;
            }
        }
        public void Add(Type key, T value)
        {
            if (key == null || value == null)
            {
                Debug.Assert(false, "key 或 type 为空");
                return;
            }
            if (_types.ContainsKey(key))
            {
                Debug.Assert(false, "key 已存在");
                return;
            }
            _types.Add(key, value);
        }
        public T GetValue(Type key)
        {
            Debug.Assert(key != null, "key 为空");
            if (_actOnSub)
            {
                foreach (KeyValuePair<Type,T> item in _types)
                {
                    if (item.Key.Equals(key))
                        return item.Value;
                    else
                    {
                        if (item.Key.IsInterface)
                        {
                            Type interfaceType = key.GetInterface(item.Key.Name, false);
                            if (interfaceType != null)
                                return item.Value;
                        }
                        else
                        {
                            if (key.IsSubclassOf(item.Key))
                                return item.Value;
                        }
                    }
                }
                return default(T);
            }
            else
            {
                if (_types.ContainsKey(key) == false)
                    return default(T);
                else
                    return _types[key];
            }
        }
    }
    public class TypeTypeMaps : TypeValueMaps<Type>
    {
        public object CreateInstance(Type key)
        {
            return CreateInstance(key, null);
        }
        public object CreateInstance(Type key, params object[] args)
        {
            if (ContainsKey(key))
            {
                Type valueType = GetValue(key);
                return Activator.CreateInstance(valueType, args);
            }
            else
            {
                Debug.Assert(false, "指定的 key 不存在:" + key.ToString());
                return null;
            }
        }
    }
}
