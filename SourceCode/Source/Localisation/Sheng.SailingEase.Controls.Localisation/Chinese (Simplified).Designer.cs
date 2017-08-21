/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Controls.Localisation {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sheng.SailingEase.Controls.Localisation.Chinese (Simplified)", typeof(Chinese__Simplified_).Assembly);
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
        public string MessageBoxCaptiton_Message {
            get {
                return ResourceManager.GetString("MessageBoxCaptiton_Message", resourceCulture);
            }
        }
        public string SEPaginationDataGridView_EnumNavigationLocation_Bottom {
            get {
                return ResourceManager.GetString("SEPaginationDataGridView_EnumNavigationLocation_Bottom", resourceCulture);
            }
        }
        public string SEPaginationDataGridView_EnumNavigationLocation_Top {
            get {
                return ResourceManager.GetString("SEPaginationDataGridView_EnumNavigationLocation_Top", resourceCulture);
            }
        }
        public string SEToolStripImageSize_ExtraLarge {
            get {
                return ResourceManager.GetString("SEToolStripImageSize_ExtraLarge", resourceCulture);
            }
        }
        public string SEToolStripImageSize_Large {
            get {
                return ResourceManager.GetString("SEToolStripImageSize_Large", resourceCulture);
            }
        }
        public string SEToolStripImageSize_Medium {
            get {
                return ResourceManager.GetString("SEToolStripImageSize_Medium", resourceCulture);
            }
        }
        public string SEToolStripImageSize_Small {
            get {
                return ResourceManager.GetString("SEToolStripImageSize_Small", resourceCulture);
            }
        }
        public bool IsDefault {
            get {
                return false;
            }
        }
        public override string ToString() {
            return Culture.EnglishName;
        }
    }
}
