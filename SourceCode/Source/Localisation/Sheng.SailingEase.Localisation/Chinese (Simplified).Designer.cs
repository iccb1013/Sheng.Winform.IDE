/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Localisation {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sheng.SailingEase.Localisation.Chinese (Simplified)", typeof(Chinese__Simplified_).Assembly);
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
        public string BooleanFalse {
            get {
                return ResourceManager.GetString("BooleanFalse", resourceCulture);
            }
        }
        public string BooleanTrue {
            get {
                return ResourceManager.GetString("BooleanTrue", resourceCulture);
            }
        }
        public string ConfirmDelete {
            get {
                return ResourceManager.GetString("ConfirmDelete", resourceCulture);
            }
        }
        public string FolderNotExist {
            get {
                return ResourceManager.GetString("FolderNotExist", resourceCulture);
            }
        }
        public string MessageBoxButton_Cancel {
            get {
                return ResourceManager.GetString("MessageBoxButton_Cancel", resourceCulture);
            }
        }
        public string MessageBoxButton_No {
            get {
                return ResourceManager.GetString("MessageBoxButton_No", resourceCulture);
            }
        }
        public string MessageBoxButton_Yes {
            get {
                return ResourceManager.GetString("MessageBoxButton_Yes", resourceCulture);
            }
        }
        public string MessageCaption_Error {
            get {
                return ResourceManager.GetString("MessageCaption_Error", resourceCulture);
            }
        }
        public string MessageCaption_Infomation {
            get {
                return ResourceManager.GetString("MessageCaption_Infomation", resourceCulture);
            }
        }
        public string MessageCaption_Notice {
            get {
                return ResourceManager.GetString("MessageCaption_Notice", resourceCulture);
            }
        }
        public string MessageCaption_Warning {
            get {
                return ResourceManager.GetString("MessageCaption_Warning", resourceCulture);
            }
        }
        public string RegexMsg_EntityCode {
            get {
                return ResourceManager.GetString("RegexMsg_EntityCode", resourceCulture);
            }
        }
        public string SystemItemNoDeleteAllowed {
            get {
                return ResourceManager.GetString("SystemItemNoDeleteAllowed", resourceCulture);
            }
        }
        public string SystemItemNoEditAllowed {
            get {
                return ResourceManager.GetString("SystemItemNoEditAllowed", resourceCulture);
            }
        }
        public string ValueInefficacyUseKeywords {
            get {
                return ResourceManager.GetString("ValueInefficacyUseKeywords", resourceCulture);
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
