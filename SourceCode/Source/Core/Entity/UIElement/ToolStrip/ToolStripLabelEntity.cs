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
    [ToolStripItemEntityProvide("ToolStripLabelEntity", 0x00006D)]
    public class ToolStripLabelEntity : ToolStripItemAbstract
    {
        public ToolStripLabelEntity()
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
