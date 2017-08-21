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
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.ComponentModel.Design.UndoEngine;
using Sheng.SailingEase.Core.Development.Localisation;
using Sheng.SailingEase.Core;
namespace Sheng.SailingEase.Core.Development
{
    public class SEUndoUnitEventEdit : SEUndoUnitCollectionEdit
    {
        public override string Name
        {
            get
            {
                string name = Language.Current.SEUndoUnitEventEdit_Name;
                EventBase eventBase = this.Value as EventBase;
                switch (this.EditType)
                {
                    case CollectionEditType.Add:
                        name = String.Format(Language.Current.SEUndoUnitEventEdit_Name_Add, eventBase.Name);
                        break;
                    case CollectionEditType.Edit:
                        name = String.Format(Language.Current.SEUndoUnitEventEdit_Name_Edit, eventBase.Name);
                        break;
                    case CollectionEditType.Delete:
                        if (this.Values.Count == 1)
                            name = String.Format(Language.Current.SEUndoUnitEventEdit_Name_Delete, (this.Values.ElementAt(0).Value as EventBase).Name);
                        else
                            name = String.Format(Language.Current.SEUndoUnitEventEdit_Name_MultiDelete, this.Values.Count);
                        break;
                    case CollectionEditType.Move:
                        name = String.Format(Language.Current.SEUndoUnitEventEdit_Name_Move, eventBase.Name);
                        break;
                }
                return name;
            }
        }
        public SEUndoUnitEventEdit(CollectionEditEventArgs collectionEditEventArgs)
            : base(String.Empty,collectionEditEventArgs)
        {
        }
    }
}
