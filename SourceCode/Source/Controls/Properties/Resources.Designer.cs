/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Sheng.SailingEase.Controls.Properties {
    using System;
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        private static global::System.Resources.ResourceManager resourceMan;
        private static global::System.Globalization.CultureInfo resourceCulture;
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sheng.SailingEase.Controls.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        internal static System.Drawing.Bitmap Check {
            get {
                object obj = ResourceManager.GetObject("Check", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        internal static System.Drawing.Bitmap Folder {
            get {
                object obj = ResourceManager.GetObject("Folder", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        internal static System.Drawing.Bitmap FolderClosed {
            get {
                object obj = ResourceManager.GetObject("FolderClosed", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        internal static System.Drawing.Bitmap Leaf {
            get {
                object obj = ResourceManager.GetObject("Leaf", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        internal static System.Drawing.Bitmap Minus2 {
            get {
                object obj = ResourceManager.GetObject("Minus2", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        internal static System.Drawing.Bitmap Plus2 {
            get {
                object obj = ResourceManager.GetObject("Plus2", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        internal static System.Drawing.Bitmap Uncheck {
            get {
                object obj = ResourceManager.GetObject("Uncheck", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        internal static System.Drawing.Bitmap Unknown {
            get {
                object obj = ResourceManager.GetObject("Unknown", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
    }
}
