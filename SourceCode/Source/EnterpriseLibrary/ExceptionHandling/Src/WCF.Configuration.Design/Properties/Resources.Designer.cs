/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design." +
                            "Properties.Resources", typeof(Resources).Assembly);
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
        internal static string AddWCF {
            get {
                return ResourceManager.GetString("AddWCF", resourceCulture);
            }
        }
        internal static string ExceptionMessageDescription {
            get {
                return ResourceManager.GetString("ExceptionMessageDescription", resourceCulture);
            }
        }
        internal static string FaultContractExceptionHandlerNodeName {
            get {
                return ResourceManager.GetString("FaultContractExceptionHandlerNodeName", resourceCulture);
            }
        }
        internal static string FaultContractExceptionHandlerNodeUICommandLongText {
            get {
                return ResourceManager.GetString("FaultContractExceptionHandlerNodeUICommandLongText", resourceCulture);
            }
        }
        internal static string FaultContractExceptionHandlerNodeUICommandText {
            get {
                return ResourceManager.GetString("FaultContractExceptionHandlerNodeUICommandText", resourceCulture);
            }
        }
        internal static string FaultContractMappingNameDescription {
            get {
                return ResourceManager.GetString("FaultContractMappingNameDescription", resourceCulture);
            }
        }
        internal static string FaultContractMappingSourceDescription {
            get {
                return ResourceManager.GetString("FaultContractMappingSourceDescription", resourceCulture);
            }
        }
        internal static string FaultContractTypeDescription {
            get {
                return ResourceManager.GetString("FaultContractTypeDescription", resourceCulture);
            }
        }
        internal static string PropertyMappingDuplicateNameError {
            get {
                return ResourceManager.GetString("PropertyMappingDuplicateNameError", resourceCulture);
            }
        }
        internal static string PropertyMappingEditorFormat {
            get {
                return ResourceManager.GetString("PropertyMappingEditorFormat", resourceCulture);
            }
        }
        internal static string PropertyMappingNameNullError {
            get {
                return ResourceManager.GetString("PropertyMappingNameNullError", resourceCulture);
            }
        }
        internal static string PropertyMappingsMessageDescription {
            get {
                return ResourceManager.GetString("PropertyMappingsMessageDescription", resourceCulture);
            }
        }
        internal static string WCF {
            get {
                return ResourceManager.GetString("WCF", resourceCulture);
            }
        }
    }
}
