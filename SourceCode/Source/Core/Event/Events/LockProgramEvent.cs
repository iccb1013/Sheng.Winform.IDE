/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
namespace Sheng.SailingEase.Core
{
    [Serializable]
    [EventProvide("��������", 0x000067,"����Ӧ�ó������û��������������")]
    public class LockProgramEvent:EventBase
    {
        public LockProgramEvent()
        {
        }
    }
}
