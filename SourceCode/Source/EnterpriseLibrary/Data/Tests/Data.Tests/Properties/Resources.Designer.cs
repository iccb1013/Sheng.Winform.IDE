/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Practices.EnterpriseLibrary.Data.Tests.Properties.Resources", typeof(Resources).Assembly);
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
        internal static string CounterCategory {
            get {
                return ResourceManager.GetString("CounterCategory", resourceCulture);
            }
        }
        internal static string ExceptionMessageUpdateDataSetRowFailure {
            get {
                return ResourceManager.GetString("ExceptionMessageUpdateDataSetRowFailure", resourceCulture);
            }
        }
        internal static string NumCmdsFailPerSec {
            get {
                return ResourceManager.GetString("NumCmdsFailPerSec", resourceCulture);
            }
        }
        internal static string NumCmdsPerSec {
            get {
                return ResourceManager.GetString("NumCmdsPerSec", resourceCulture);
            }
        }
        internal static string NumConnFailPerSec {
            get {
                return ResourceManager.GetString("NumConnFailPerSec", resourceCulture);
            }
        }
        internal static string NumConnPerSec {
            get {
                return ResourceManager.GetString("NumConnPerSec", resourceCulture);
            }
        }
        internal static string NumTransAbortPerSec {
            get {
                return ResourceManager.GetString("NumTransAbortPerSec", resourceCulture);
            }
        }
        internal static string NumTransCommitPerSec {
            get {
                return ResourceManager.GetString("NumTransCommitPerSec", resourceCulture);
            }
        }
        internal static string NumTransFailPerSec {
            get {
                return ResourceManager.GetString("NumTransFailPerSec", resourceCulture);
            }
        }
        internal static string NumTransOpenPerSec {
            get {
                return ResourceManager.GetString("NumTransOpenPerSec", resourceCulture);
            }
        }
    }
}
