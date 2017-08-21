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
using System.Drawing;
using System.Diagnostics;
namespace Sheng.SailingEase.Kernal
{
    public class ResourceMapping
    {
        private Dictionary<Type, Image> _mapping = new Dictionary<Type, Image>();
        public ResourceMapping()
        {
        }
        public void Mapping(Type type, Image image)
        {
            if (_mapping.Keys.Contains(type))
            {
                _mapping[type] = image;
            }
            else
            {
                _mapping.Add(type, image);
            }
        }
        public bool HasResource(Type type)
        {
            return _mapping.Keys.Contains(type);
        }
        public Image Get(Type type)
        {
            if (HasResource(type) == false)
            {
                Debug.Assert(false,"ResourceMapping 中没有指定类型的资源:" + type.Name);
                return null;
            }
            return _mapping[type];
        }
    }
}
