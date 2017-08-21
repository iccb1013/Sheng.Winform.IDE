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
namespace Sheng.SailingEase.ComponentModel.Design.UndoEngine
{
    public static class ExtensionMethods
    {
        public static void Inject(this SEUndoMemberCollection undoMemberCollection, List<ObjectCompareResult> compareResult)
        {
            foreach (ObjectCompareResult result in compareResult)
            {
                undoMemberCollection.Add(result.MemberName, result.SourceValue, result.CompareValue);
            }
        }
    }
}
