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
namespace Sheng.SailingEase.ComponentModel.Design
{
    class PropertyGridRowCache
    {
        private List<PropertyGridRowCacheItem> _items = new List<PropertyGridRowCacheItem>();
        public void Cache(PropertyGridRowCacheItem item)
        {
            if (IsCached(item) == false)
            {
                _items.Add(item);
            }
        }
        public bool IsCached(PropertyGridRowCacheItem item)
        {
            if (_items.Contains(item) )
            {
                return true;
            }
            var items = from c in _items where c.Type.Equals(item.Type) && c.PropertyName == item.PropertyName select c;
            if (items.ToList().Count > 0)
                return true;
            else
                return false;
        }
        public bool IsCached(Type type)
        {
            var items = from c in _items where c.Type.Equals(type) select c;
            if ( items.ToList().Count > 0)
                return true;
            else
                return false;
        }
        public PropertyGridRow[] GetPropertyGridRow(Type type)
        {
            var items = from c in _items where c.Type.Equals(type) select c;
            PropertyGridRowCacheItem[] cachedItems = items.ToArray();
            PropertyGridRow[] rows = new PropertyGridRow[cachedItems.Length];
            for (int i = 0; i < cachedItems.Length; i++)
            {
                rows[i] = cachedItems[i].PropertyGridRow;
            }
            return rows;
        }
    }
}
