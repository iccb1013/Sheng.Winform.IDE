/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Properties {
    using System;
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Practices.EnterpriseLibrary.Common.Tests.Properties.Resources", typeof(Resources).Assembly);
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
        internal static string CategoryHelp {
            get {
                return ResourceManager.GetString("CategoryHelp", resourceCulture);
            }
        }
        internal static string FooLabel {
            get {
                return ResourceManager.GetString("FooLabel", resourceCulture);
            }
        }
        internal static string HelpBillHelpResource {
            get {
                return ResourceManager.GetString("HelpBillHelpResource", resourceCulture);
            }
        }
        internal static string HelpTomHelpResource {
            get {
                return ResourceManager.GetString("HelpTomHelpResource", resourceCulture);
            }
        }
        internal static string Hi {
            get {
                return ResourceManager.GetString("Hi", resourceCulture);
            }
        }
    }
}
