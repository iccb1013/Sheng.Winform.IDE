/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.ComponentModel.Design.Localisation {
    using System;
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Chinese__Simplified_ : ILanguage {
        private global::System.Resources.ResourceManager resourceMan;
        private global::System.Globalization.CultureInfo resourceCulture;
        public Chinese__Simplified_() {
        }
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sheng.SailingEase.ComponentModel.Design.Localisation.Chinese (Simplified)", typeof(Chinese__Simplified_).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public global::System.Globalization.CultureInfo Culture {
            get {
                if (object.ReferenceEquals(resourceCulture, null)) {
                    global::System.Globalization.CultureInfo temp = new global::System.Globalization.CultureInfo("zh-CHS");
                    resourceCulture = temp;
                }
                return resourceCulture;
            }
        }
        public string PropertyGrid_ErrorText_NullValueInefficacy {
            get {
                return ResourceManager.GetString("PropertyGrid_ErrorText_NullValueInefficacy", resourceCulture);
            }
        }
        public string PropertyGridDataErrorView {
            get {
                return ResourceManager.GetString("PropertyGridDataErrorView", resourceCulture);
            }
        }
        public string PropertyGridDataErrorView_ButtonCancel {
            get {
                return ResourceManager.GetString("PropertyGridDataErrorView_ButtonCancel", resourceCulture);
            }
        }
        public string PropertyGridDataErrorView_ButtonOK {
            get {
                return ResourceManager.GetString("PropertyGridDataErrorView_ButtonOK", resourceCulture);
            }
        }
        public string SEUndoUnit_Name_AddControl {
            get {
                return ResourceManager.GetString("SEUndoUnit_Name_AddControl", resourceCulture);
            }
        }
        public string SEUndoUnit_Name_MoveControl {
            get {
                return ResourceManager.GetString("SEUndoUnit_Name_MoveControl", resourceCulture);
            }
        }
        public string SEUndoUnit_Name_RemoveControl {
            get {
                return ResourceManager.GetString("SEUndoUnit_Name_RemoveControl", resourceCulture);
            }
        }
        public string SEUndoUnit_Name_ResizeControl {
            get {
                return ResourceManager.GetString("SEUndoUnit_Name_ResizeControl", resourceCulture);
            }
        }
        public string SEUndoUnit_Name_SetProperty {
            get {
                return ResourceManager.GetString("SEUndoUnit_Name_SetProperty", resourceCulture);
            }
        }
        public string SEUndoUnit_Set {
            get {
                return ResourceManager.GetString("SEUndoUnit_Set", resourceCulture);
            }
        }
        public string SEUndoUnitCollectionEdit_Name {
            get {
                return ResourceManager.GetString("SEUndoUnitCollectionEdit_Name", resourceCulture);
            }
        }
        public string SEUndoUnitCollectionEdit_Name_Add {
            get {
                return ResourceManager.GetString("SEUndoUnitCollectionEdit_Name_Add", resourceCulture);
            }
        }
        public string SEUndoUnitCollectionEdit_Name_Delete {
            get {
                return ResourceManager.GetString("SEUndoUnitCollectionEdit_Name_Delete", resourceCulture);
            }
        }
        public string SEUndoUnitCollectionEdit_Name_Edit {
            get {
                return ResourceManager.GetString("SEUndoUnitCollectionEdit_Name_Edit", resourceCulture);
            }
        }
        public string SEUndoUnitCollectionEdit_Name_Move {
            get {
                return ResourceManager.GetString("SEUndoUnitCollectionEdit_Name_Move", resourceCulture);
            }
        }
        public string SEUndoUnitCollectionEdit_Name_MultiDelete {
            get {
                return ResourceManager.GetString("SEUndoUnitCollectionEdit_Name_MultiDelete", resourceCulture);
            }
        }
        public string UndoEngine_StepList_RedoNotice {
            get {
                return ResourceManager.GetString("UndoEngine_StepList_RedoNotice", resourceCulture);
            }
        }
        public string UndoEngine_StepList_UndoNotice {
            get {
                return ResourceManager.GetString("UndoEngine_StepList_UndoNotice", resourceCulture);
            }
        }
        public bool IsDefault {
            get {
                return true;
            }
        }
        public override string ToString() {
            return Culture.EnglishName;
        }
    }
}
