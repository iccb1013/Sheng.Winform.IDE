/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests.Properties.Re" +
                            "sources", typeof(Resources).Assembly);
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
        internal static string InitializeComponentCompleted {
            get {
                return ResourceManager.GetString("InitializeComponentCompleted", resourceCulture);
            }
        }
        internal static string InitializeComponentStarted {
            get {
                return ResourceManager.GetString("InitializeComponentStarted", resourceCulture);
            }
        }
        internal static string MsmqAccessDenied {
            get {
                return ResourceManager.GetString("MsmqAccessDenied", resourceCulture);
            }
        }
        internal static string ServicePausedSuccess {
            get {
                return ResourceManager.GetString("ServicePausedSuccess", resourceCulture);
            }
        }
        internal static string ServicePauseError {
            get {
                return ResourceManager.GetString("ServicePauseError", resourceCulture);
            }
        }
        internal static string ServicePauseWarning {
            get {
                return ResourceManager.GetString("ServicePauseWarning", resourceCulture);
            }
        }
        internal static string ServiceResumeComplete {
            get {
                return ResourceManager.GetString("ServiceResumeComplete", resourceCulture);
            }
        }
        internal static string ServiceResumeError {
            get {
                return ResourceManager.GetString("ServiceResumeError", resourceCulture);
            }
        }
        internal static string ServiceStartComplete {
            get {
                return ResourceManager.GetString("ServiceStartComplete", resourceCulture);
            }
        }
        internal static string ServiceStartError {
            get {
                return ResourceManager.GetString("ServiceStartError", resourceCulture);
            }
        }
        internal static string ServiceStopComplete {
            get {
                return ResourceManager.GetString("ServiceStopComplete", resourceCulture);
            }
        }
        internal static string ServiceStopError {
            get {
                return ResourceManager.GetString("ServiceStopError", resourceCulture);
            }
        }
        internal static string ServiceStopWarning {
            get {
                return ResourceManager.GetString("ServiceStopWarning", resourceCulture);
            }
        }
        internal static string ValidationComplete {
            get {
                return ResourceManager.GetString("ValidationComplete", resourceCulture);
            }
        }
        internal static string ValidationError {
            get {
                return ResourceManager.GetString("ValidationError", resourceCulture);
            }
        }
        internal static string ValidationStarted {
            get {
                return ResourceManager.GetString("ValidationStarted", resourceCulture);
            }
        }
    }
}
