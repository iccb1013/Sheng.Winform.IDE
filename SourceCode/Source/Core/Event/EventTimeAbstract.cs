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
using System.Collections;
using System.Diagnostics;
namespace Sheng.SailingEase.Core
{
    public abstract class EventTimeAbstract
    {
        public abstract int Code
        {
            get;
        }
        public const string Property_Name = "Name";
        public abstract string Name
        {
            get;
        }
    }
}
