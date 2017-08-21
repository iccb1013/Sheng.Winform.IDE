/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Components.ResourceComponent.Localisation {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sheng.SailingEase.Components.ResourceComponent.Localisation.Chinese (Simplified)", typeof(Chinese__Simplified_).Assembly);
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
        public string Explorer_ImageListViewContextMenuStrip_Add {
            get {
                return ResourceManager.GetString("Explorer_ImageListViewContextMenuStrip_Add", resourceCulture);
            }
        }
        public string Explorer_ImageListViewContextMenuStrip_Delete {
            get {
                return ResourceManager.GetString("Explorer_ImageListViewContextMenuStrip_Delete", resourceCulture);
            }
        }
        public string ExplorerView_TabText {
            get {
                return ResourceManager.GetString("ExplorerView_TabText", resourceCulture);
            }
        }
        public string Navigation_Menu_Resource {
            get {
                return ResourceManager.GetString("Navigation_Menu_Resource", resourceCulture);
            }
        }
        public string Navigation_ToolStrip_Resource {
            get {
                return ResourceManager.GetString("Navigation_ToolStrip_Resource", resourceCulture);
            }
        }
        public string RemoveImageResourceCommand_ConfirmDelete {
            get {
                return ResourceManager.GetString("RemoveImageResourceCommand_ConfirmDelete", resourceCulture);
            }
        }
        public string ResourceComponentService_OverrideConfirm {
            get {
                return ResourceManager.GetString("ResourceComponentService_OverrideConfirm", resourceCulture);
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
