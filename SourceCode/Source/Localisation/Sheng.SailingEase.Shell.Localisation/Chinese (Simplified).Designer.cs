/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Shell.Localisation {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sheng.SailingEase.Shell.Localisation.Chinese (Simplified)", typeof(Chinese__Simplified_).Assembly);
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
        public string GlobalTabPageContextMenuStrip_Close {
            get {
                return ResourceManager.GetString("GlobalTabPageContextMenuStrip_Close", resourceCulture);
            }
        }
        public string GlobalTabPageContextMenuStrip_CloseAll {
            get {
                return ResourceManager.GetString("GlobalTabPageContextMenuStrip_CloseAll", resourceCulture);
            }
        }
        public string GlobalTabPageContextMenuStrip_CloseAllButThis {
            get {
                return ResourceManager.GetString("GlobalTabPageContextMenuStrip_CloseAllButThis", resourceCulture);
            }
        }
        public string Navigation_Menu_Build {
            get {
                return ResourceManager.GetString("Navigation_Menu_Build", resourceCulture);
            }
        }
        public string Navigation_Menu_Edit {
            get {
                return ResourceManager.GetString("Navigation_Menu_Edit", resourceCulture);
            }
        }
        public string Navigation_Menu_File {
            get {
                return ResourceManager.GetString("Navigation_Menu_File", resourceCulture);
            }
        }
        public string Navigation_Menu_File_Exit {
            get {
                return ResourceManager.GetString("Navigation_Menu_File_Exit", resourceCulture);
            }
        }
        public string Navigation_Menu_Help {
            get {
                return ResourceManager.GetString("Navigation_Menu_Help", resourceCulture);
            }
        }
        public string Navigation_Menu_Tool {
            get {
                return ResourceManager.GetString("Navigation_Menu_Tool", resourceCulture);
            }
        }
        public string Navigation_Menu_View {
            get {
                return ResourceManager.GetString("Navigation_Menu_View", resourceCulture);
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
