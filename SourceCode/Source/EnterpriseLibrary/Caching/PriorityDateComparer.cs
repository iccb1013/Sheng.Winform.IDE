/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections;
namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    public class PriorityDateComparer : IComparer
    {
        private Hashtable unsortedItems;
        public PriorityDateComparer(Hashtable unsortedItems)
        {
            this.unsortedItems = unsortedItems;
        }
        public int Compare(object x, object y)
        {
            CacheItem leftCacheItem = (CacheItem)unsortedItems[(string)x];
            CacheItem rightCacheItem = (CacheItem)unsortedItems[(string)y];
            lock (rightCacheItem)
            {
                lock (leftCacheItem)
                {
                    if (rightCacheItem == null && leftCacheItem == null)
                    {
                        return 0;
                    }
                    if (leftCacheItem == null)
                    {
                        return -1;
                    }
                    if (rightCacheItem == null)
                    {
                        return 1;
                    }
                    return leftCacheItem.ScavengingPriority == rightCacheItem.ScavengingPriority
                        ? leftCacheItem.LastAccessedTime.CompareTo(rightCacheItem.LastAccessedTime)
                        : leftCacheItem.ScavengingPriority - rightCacheItem.ScavengingPriority;
                }
            }
        }
    }
}
