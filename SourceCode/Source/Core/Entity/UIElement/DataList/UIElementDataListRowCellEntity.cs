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
    [Serializable]
    public class UIElementDataListRowCellEntity
    {
        public virtual object Value
        {
            get;
            set;
        }
        public UIElementDataListRowEntity OwningRow
        {
            get;
            set;
        }
        public UIElementDataListColumnEntityAbstract OwningColumn
        {
            get;
            set;
        }
        public UIElementDataListRowCellEntity(UIElementDataListRowEntity owningRow, UIElementDataListColumnEntityAbstract owningColumn)
        {
            OwningRow = owningRow;
            OwningColumn = owningColumn;
        }
    }
}
