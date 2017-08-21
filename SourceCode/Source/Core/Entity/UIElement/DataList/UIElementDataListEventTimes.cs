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
namespace Sheng.SailingEase.Core
{
    public class UIElementDataListEventTimes : EventTimesAbstract
    {
        public UIElementDataListEventTimes()
        {
            _times = new List<EventTimeAbstract>();
            _times.Add(new RowStateChanged());
            _times.Add(new CellMouseDoubleClick());
        }
        public class RowStateChanged : EventTimeAbstract
        {
            public static int XCode
            {
                get { return 1; }
            }
            public override int Code
            {
                get { return XCode; }
            }
            public override string Name
            {
                get { return "行状态改变"; }
            }
        }
        public class CellMouseDoubleClick : EventTimeAbstract
        {
            public static int XCode
            {
                get { return 2; }
            }
            public override int Code
            {
                get { return XCode; }
            }
            public override string Name
            {
                get { return "双击单元格"; }
            }
        }
    }
}
