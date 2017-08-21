/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.ComponentModel.Design.Localisation {
    public interface ILanguage {
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        global::System.Resources.ResourceManager ResourceManager {
            get;
        }
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        global::System.Globalization.CultureInfo Culture {
            get;
        }
        string PropertyGrid_ErrorText_NullValueInefficacy {
            get;
        }
        string PropertyGridDataErrorView {
            get;
        }
        string PropertyGridDataErrorView_ButtonCancel {
            get;
        }
        string PropertyGridDataErrorView_ButtonOK {
            get;
        }
        string SEUndoUnit_Name_AddControl {
            get;
        }
        string SEUndoUnit_Name_MoveControl {
            get;
        }
        string SEUndoUnit_Name_RemoveControl {
            get;
        }
        string SEUndoUnit_Name_ResizeControl {
            get;
        }
        string SEUndoUnit_Name_SetProperty {
            get;
        }
        string SEUndoUnit_Set {
            get;
        }
        string SEUndoUnitCollectionEdit_Name {
            get;
        }
        string SEUndoUnitCollectionEdit_Name_Add {
            get;
        }
        string SEUndoUnitCollectionEdit_Name_Delete {
            get;
        }
        string SEUndoUnitCollectionEdit_Name_Edit {
            get;
        }
        string SEUndoUnitCollectionEdit_Name_Move {
            get;
        }
        string SEUndoUnitCollectionEdit_Name_MultiDelete {
            get;
        }
        string UndoEngine_StepList_RedoNotice {
            get;
        }
        string UndoEngine_StepList_UndoNotice {
            get;
        }
        bool IsDefault {
            get;
        }
    }
}
