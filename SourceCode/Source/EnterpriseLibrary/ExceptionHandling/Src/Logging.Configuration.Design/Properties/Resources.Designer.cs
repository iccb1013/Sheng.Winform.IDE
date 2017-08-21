/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Des" +
                            "ign.Properties.Resources", typeof(Resources).Assembly);
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
        internal static string CategoryGeneral {
            get {
                return ResourceManager.GetString("CategoryGeneral", resourceCulture);
            }
        }
        internal static string DefaultCategory {
            get {
                return ResourceManager.GetString("DefaultCategory", resourceCulture);
            }
        }
        internal static string DefaultEventIdDescription {
            get {
                return ResourceManager.GetString("DefaultEventIdDescription", resourceCulture);
            }
        }
        internal static string DefaultLogCategoryDescription {
            get {
                return ResourceManager.GetString("DefaultLogCategoryDescription", resourceCulture);
            }
        }
        internal static string DefaultSeverityDescription {
            get {
                return ResourceManager.GetString("DefaultSeverityDescription", resourceCulture);
            }
        }
        internal static string DefaultTitle {
            get {
                return ResourceManager.GetString("DefaultTitle", resourceCulture);
            }
        }
        internal static string DefaultTitleDescription {
            get {
                return ResourceManager.GetString("DefaultTitleDescription", resourceCulture);
            }
        }
        internal static string FormatterTypeNameDescription {
            get {
                return ResourceManager.GetString("FormatterTypeNameDescription", resourceCulture);
            }
        }
        internal static string GenericCreateStatusText {
            get {
                return ResourceManager.GetString("GenericCreateStatusText", resourceCulture);
            }
        }
        internal static string LoggingHandlerName {
            get {
                return ResourceManager.GetString("LoggingHandlerName", resourceCulture);
            }
        }
        internal static string MinimumPriorityDescription {
            get {
                return ResourceManager.GetString("MinimumPriorityDescription", resourceCulture);
            }
        }
    }
}
