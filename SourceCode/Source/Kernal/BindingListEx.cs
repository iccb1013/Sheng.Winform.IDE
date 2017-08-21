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
using System.ComponentModel;
namespace Sheng.SailingEase.Kernal
{
    public interface IBindingListEx : IBindingList
    {
        void ResetItem(int position);
    }
    public class BindingListEx<T> : BindingList<T>, IBindingListEx
    {
        public BindingListEx()
        {
        }
        public BindingListEx(IList<T> list):
            base(list)
        {
        }
    }
}
