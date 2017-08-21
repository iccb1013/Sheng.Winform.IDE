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
    public class UIElementTextBoxEventTimes : EventTimesAbstract
    {
        public UIElementTextBoxEventTimes()
        {
            _times = new List<EventTimeAbstract>();
            _times.Add(new Click());
            _times.Add(new Enter());
            _times.Add(new Leave());
            _times.Add(new TextChanged());
        }
        public class Click : EventTimeAbstract
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
                get { return "单击"; }
            }
        }
        public class Enter : EventTimeAbstract
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
                get { return "获得焦点"; }
            }
        }
        public class Leave : EventTimeAbstract
        {
            public static int XCode
            {
                get { return 3; }
            }
            public override int Code
            {
                get { return XCode; }
            }
            public override string Name
            {
                get { return "失去焦点"; }
            }
        }
        public class TextChanged : EventTimeAbstract
        {
            public static int XCode
            {
                get { return 4; }
            }
            public override int Code
            {
                get { return XCode; }
            }
            public override string Name
            {
                get { return "更改文本内容"; }
            }
        }
    }
}
