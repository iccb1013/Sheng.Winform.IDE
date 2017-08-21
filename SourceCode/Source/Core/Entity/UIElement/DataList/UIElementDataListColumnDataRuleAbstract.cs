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
using Sheng.SailingEase.Kernal;
namespace Sheng.SailingEase.Core
{
    [Serializable]
    public abstract class UIElementDataListColumnDataRuleAbstract : IXmlable
    {
        public abstract void FromXml(string strXml);
        public abstract string ToXml();
        public virtual object GetFormattedValue(UIElementDataListRowEntity row, UIElementDataListRowCellEntity cell)
        {
            return cell.Value;
        }
    }
}
