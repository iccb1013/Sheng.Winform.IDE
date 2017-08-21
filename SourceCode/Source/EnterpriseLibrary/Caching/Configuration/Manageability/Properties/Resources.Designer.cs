/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability.Propert" +
                            "ies.Resources", typeof(Resources).Assembly);
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
        internal static string BackingStoreSettingsPartName {
            get {
                return ResourceManager.GetString("BackingStoreSettingsPartName", resourceCulture);
            }
        }
        internal static string CacheManagerExpirationPollFrequencyInSecondsPartName {
            get {
                return ResourceManager.GetString("CacheManagerExpirationPollFrequencyInSecondsPartName", resourceCulture);
            }
        }
        internal static string CacheManagerMaximumElementsInCacheBeforeScavengingPartName {
            get {
                return ResourceManager.GetString("CacheManagerMaximumElementsInCacheBeforeScavengingPartName", resourceCulture);
            }
        }
        internal static string CacheManagerNumberToRemoveWhenScavengingPartName {
            get {
                return ResourceManager.GetString("CacheManagerNumberToRemoveWhenScavengingPartName", resourceCulture);
            }
        }
        internal static string CacheManagerPolicyNameTemplate {
            get {
                return ResourceManager.GetString("CacheManagerPolicyNameTemplate", resourceCulture);
            }
        }
        internal static string CacheManagersCategoryName {
            get {
                return ResourceManager.GetString("CacheManagersCategoryName", resourceCulture);
            }
        }
        internal static string CacheManagerSettingsDefaultCacheManagerPartName {
            get {
                return ResourceManager.GetString("CacheManagerSettingsDefaultCacheManagerPartName", resourceCulture);
            }
        }
        internal static string CacheManagerSettingsPolicyName {
            get {
                return ResourceManager.GetString("CacheManagerSettingsPolicyName", resourceCulture);
            }
        }
        internal static string CachingSectionCategoryName {
            get {
                return ResourceManager.GetString("CachingSectionCategoryName", resourceCulture);
            }
        }
        internal static string CustomProviderAttributesPartName {
            get {
                return ResourceManager.GetString("CustomProviderAttributesPartName", resourceCulture);
            }
        }
        internal static string CustomProviderTypePartName {
            get {
                return ResourceManager.GetString("CustomProviderTypePartName", resourceCulture);
            }
        }
        internal static string IsolatedStorageCacheStorageDataPartitionNamePartName {
            get {
                return ResourceManager.GetString("IsolatedStorageCacheStorageDataPartitionNamePartName", resourceCulture);
            }
        }
        internal static string NullBackingStoreNoSettingsPartName {
            get {
                return ResourceManager.GetString("NullBackingStoreNoSettingsPartName", resourceCulture);
            }
        }
        internal static string StorageEncryptionProviderSettingsPartName {
            get {
                return ResourceManager.GetString("StorageEncryptionProviderSettingsPartName", resourceCulture);
            }
        }
    }
}
