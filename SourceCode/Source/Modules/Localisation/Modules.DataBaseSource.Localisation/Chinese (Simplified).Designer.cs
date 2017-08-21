/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Modules.DataBaseSourceModule.Localisation {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sheng.SailingEase.Modules.DataBaseSourceModule.Localisation.Chinese (Simplified)", typeof(Chinese__Simplified_).Assembly);
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
        public string DataSourceCreateView {
            get {
                return ResourceManager.GetString("DataSourceCreateView", resourceCulture);
            }
        }
        public string DataSourceCreateView_ButtonCancel {
            get {
                return ResourceManager.GetString("DataSourceCreateView_ButtonCancel", resourceCulture);
            }
        }
        public string DataSourceCreateView_ButtonOK {
            get {
                return ResourceManager.GetString("DataSourceCreateView_ButtonOK", resourceCulture);
            }
        }
        public string DataSourceCreateView_ButtonRefreshDataSource {
            get {
                return ResourceManager.GetString("DataSourceCreateView_ButtonRefreshDataSource", resourceCulture);
            }
        }
        public string DataSourceCreateView_GroupBoxLoginOption {
            get {
                return ResourceManager.GetString("DataSourceCreateView_GroupBoxLoginOption", resourceCulture);
            }
        }
        public string DataSourceCreateView_LabelDataSourceName {
            get {
                return ResourceManager.GetString("DataSourceCreateView_LabelDataSourceName", resourceCulture);
            }
        }
        public string DataSourceCreateView_LabelDataSourceType {
            get {
                return ResourceManager.GetString("DataSourceCreateView_LabelDataSourceType", resourceCulture);
            }
        }
        public string DataSourceCreateView_LabelNotice {
            get {
                return ResourceManager.GetString("DataSourceCreateView_LabelNotice", resourceCulture);
            }
        }
        public string DataSourceCreateView_LabelPassword {
            get {
                return ResourceManager.GetString("DataSourceCreateView_LabelPassword", resourceCulture);
            }
        }
        public string DataSourceCreateView_LabelUserId {
            get {
                return ResourceManager.GetString("DataSourceCreateView_LabelUserId", resourceCulture);
            }
        }
        public string DataSourceCreateView_RadioButtonIntegratedSecurity {
            get {
                return ResourceManager.GetString("DataSourceCreateView_RadioButtonIntegratedSecurity", resourceCulture);
            }
        }
        public string DataSourceCreateView_RadioButtonNoIntegratedSecurity {
            get {
                return ResourceManager.GetString("DataSourceCreateView_RadioButtonNoIntegratedSecurity", resourceCulture);
            }
        }
        public string DataSourceSetView {
            get {
                return ResourceManager.GetString("DataSourceSetView", resourceCulture);
            }
        }
        public string DataSourceSetView_ButtonCancel {
            get {
                return ResourceManager.GetString("DataSourceSetView_ButtonCancel", resourceCulture);
            }
        }
        public string DataSourceSetView_ButtonOK {
            get {
                return ResourceManager.GetString("DataSourceSetView_ButtonOK", resourceCulture);
            }
        }
        public string DataSourceSetView_LabelConnectionString {
            get {
                return ResourceManager.GetString("DataSourceSetView_LabelConnectionString", resourceCulture);
            }
        }
        public string DataSourceSetView_LabelTitle {
            get {
                return ResourceManager.GetString("DataSourceSetView_LabelTitle", resourceCulture);
            }
        }
        public string DataSourceSetView_LinkLabelCreateDataSource {
            get {
                return ResourceManager.GetString("DataSourceSetView_LinkLabelCreateDataSource", resourceCulture);
            }
        }
        public string Navigation_Menu_DataSourceSet {
            get {
                return ResourceManager.GetString("Navigation_Menu_DataSourceSet", resourceCulture);
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
