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
using System.IO;
namespace Sheng.SailingEase.Core
{
   
    public abstract class ResourceInfo
    {
        public virtual string Name
        {
            get;
            set;
        }
        public virtual Stream ResourceStream
        {
            get;
            set;
        }
        public override string ToString()
        {
            return Name;
        }
        public override bool Equals(object obj)
        {
            if ((obj is ResourceInfo) == false)
                return false;
            ResourceInfo resourceInfo = obj as ResourceInfo;
            return this.Name == resourceInfo.Name;
        }
    }
}
