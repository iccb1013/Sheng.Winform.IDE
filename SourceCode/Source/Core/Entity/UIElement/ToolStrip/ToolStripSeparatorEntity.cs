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
using System.Xml;
namespace Sheng.SailingEase.Core
{
    [ToolStripItemEntityProvide("ToolStripSeparatorEntity", 0x00006B)]
    public class ToolStripSeparatorEntity : ToolStripItemAbstract
    {
        public ToolStripSeparatorEntity()
            : base()
        {
        }
        public override void FromXml(string strXml)
        {
            base.FromXml(strXml);
        }
        public override string ToXml()
        {
            return base.ToXml();
        }
    }
}
