/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Properties.Resour" +
                            "ces", typeof(Resources).Assembly);
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
        internal static string HandlerPartNameTemplate {
            get {
                return ResourceManager.GetString("HandlerPartNameTemplate", resourceCulture);
            }
        }
        internal static string LoggingHandlerCategoryPartName {
            get {
                return ResourceManager.GetString("LoggingHandlerCategoryPartName", resourceCulture);
            }
        }
        internal static string LoggingHandlerEventIdPartName {
            get {
                return ResourceManager.GetString("LoggingHandlerEventIdPartName", resourceCulture);
            }
        }
        internal static string LoggingHandlerFormatterPartName {
            get {
                return ResourceManager.GetString("LoggingHandlerFormatterPartName", resourceCulture);
            }
        }
        internal static string LoggingHandlerPriorityPartName {
            get {
                return ResourceManager.GetString("LoggingHandlerPriorityPartName", resourceCulture);
            }
        }
        internal static string LoggingHandlerSeverityPartName {
            get {
                return ResourceManager.GetString("LoggingHandlerSeverityPartName", resourceCulture);
            }
        }
        internal static string LoggingHandlerTitlePartName {
            get {
                return ResourceManager.GetString("LoggingHandlerTitlePartName", resourceCulture);
            }
        }
        internal static string MissingConstructor {
            get {
                return ResourceManager.GetString("MissingConstructor", resourceCulture);
            }
        }
    }
}
