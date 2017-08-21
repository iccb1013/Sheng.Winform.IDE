/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageabili" +
                            "ty.Properties.Resources", typeof(Resources).Assembly);
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
        internal static string CustomHandlerAttributesPartName {
            get {
                return ResourceManager.GetString("CustomHandlerAttributesPartName", resourceCulture);
            }
        }
        internal static string CustomHandlerTypePartName {
            get {
                return ResourceManager.GetString("CustomHandlerTypePartName", resourceCulture);
            }
        }
        internal static string ExceptionTypeHandlersPartName {
            get {
                return ResourceManager.GetString("ExceptionTypeHandlersPartName", resourceCulture);
            }
        }
        internal static string ExceptionTypePolicyNameTemplate {
            get {
                return ResourceManager.GetString("ExceptionTypePolicyNameTemplate", resourceCulture);
            }
        }
        internal static string ExceptionTypePostHandlingActionPartName {
            get {
                return ResourceManager.GetString("ExceptionTypePostHandlingActionPartName", resourceCulture);
            }
        }
        internal static string HandlerPartNameTemplate {
            get {
                return ResourceManager.GetString("HandlerPartNameTemplate", resourceCulture);
            }
        }
        internal static string ReplaceHandlerExceptionMessagePartName {
            get {
                return ResourceManager.GetString("ReplaceHandlerExceptionMessagePartName", resourceCulture);
            }
        }
        internal static string ReplaceHandlerExceptionTypePartName {
            get {
                return ResourceManager.GetString("ReplaceHandlerExceptionTypePartName", resourceCulture);
            }
        }
        internal static string SectionCategoryName {
            get {
                return ResourceManager.GetString("SectionCategoryName", resourceCulture);
            }
        }
        internal static string SectionPolicyName {
            get {
                return ResourceManager.GetString("SectionPolicyName", resourceCulture);
            }
        }
        internal static string WrapHandlerExceptionMessagePartName {
            get {
                return ResourceManager.GetString("WrapHandlerExceptionMessagePartName", resourceCulture);
            }
        }
        internal static string WrapHandlerExceptionTypePartName {
            get {
                return ResourceManager.GetString("WrapHandlerExceptionTypePartName", resourceCulture);
            }
        }
    }
}
