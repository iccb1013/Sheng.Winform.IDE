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
    public class KeyTypeMaps<KeyType>
    {
        private Dictionary<KeyType, Type> _types = new Dictionary<KeyType, Type>();
        public Type this[KeyType key]
        {
            get
            {
                Debug.Assert(key != null, "key 为空");
                return _types[key];
            }
        }
        public KeyTypeMaps()
        {
        }
        public void Add(KeyType key, Type type)
        {
            if (key == null || type == null)
            {
                Debug.Assert(false, "key 或 type 为空");
                return;
            }
            if (_types.ContainsKey(key))
            {
                Debug.Assert(false, "key 已存在");
                return;
            }
            _types.Add(key, type);
        }
        public bool ContainsKey(KeyType key)
        {
            if (key == null)
            {
                Debug.Assert(false, "key 为空");
                return false;
            }
            return _types.ContainsKey(key);
        }
        public object CreateInstance(KeyType key)
        {
            return CreateInstance(key, null);
        }
        public object CreateInstance(KeyType key, params object[] args)
        {
            Type type = this[key];
            return Activator.CreateInstance(type, args);
        }
    }
}
