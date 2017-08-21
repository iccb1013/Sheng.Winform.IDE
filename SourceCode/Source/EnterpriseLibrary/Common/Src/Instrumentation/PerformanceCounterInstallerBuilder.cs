/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
    public class PerformanceCounterInstallerBuilder : AbstractInstallerBuilder
    {
        public PerformanceCounterInstallerBuilder(Type[] availableTypes)
            : base(availableTypes, typeof(PerformanceCountersDefinitionAttribute)) {}
        void CollectPerformanceCounters(Type instrumentedType,
                                        PerformanceCounterInstaller installer)
        {
            foreach (FieldInfo field in instrumentedType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                object[] attributes = field.GetCustomAttributes(typeof(PerformanceCounterAttribute), false);
                if (attributes.Length == 1)
                {
                    PerformanceCounterAttribute attribute = (PerformanceCounterAttribute)attributes[0];
                    CounterCreationData counter = GetExistingCounter(installer, attribute.CounterName);
                    if (counter == null)
                    {
                        installer.Counters.Add(
                            new CounterCreationData(attribute.CounterName, GetCounterHelp(attribute.CounterHelp, instrumentedType.Assembly), attribute.CounterType));
                        if (attribute.HasBaseCounter())
                        {
                            installer.Counters.Add(
                                new CounterCreationData(attribute.BaseCounterName, GetCounterHelp(attribute.BaseCounterHelp, instrumentedType.Assembly), attribute.BaseCounterType));
                        }
                    }
                    else
                    {
                        if (counter.CounterType != attribute.CounterType || !counter.CounterHelp.Equals(GetCounterHelp(attribute.CounterHelp, instrumentedType.Assembly)))
                        {
                            throw new InvalidOperationException(
                                string.Format(
                                    Resources.Culture,
                                    Resources.ExceptionPerformanceCounterRedefined,
                                    counter.CounterName,
                                    installer.CategoryName,
                                    instrumentedType.FullName));
                        }
                    }
                }
            }
        }
        protected override ICollection<Installer> CreateInstallers(ICollection<Type> instrumentedTypes)
        {
            List<Installer> installers = new List<Installer>();
            foreach (Type instrumentedType in instrumentedTypes)
            {
                PerformanceCounterInstaller installer = GetOrCreateInstaller(instrumentedType, installers);
                CollectPerformanceCounters(instrumentedType, installer);
            }
            return installers;
        }
        public static string GetCategoryHelp(PerformanceCountersDefinitionAttribute attribute,
                                             Assembly originalAssembly)
        {
            return GetResourceString(attribute.CategoryHelp, originalAssembly);
        }
        public static string GetCounterHelp(string resourceName,
                                            Assembly originalAssembly)
        {
            return GetResourceString(resourceName, originalAssembly);
        }
        static CounterCreationData GetExistingCounter(PerformanceCounterInstaller installer,
                                               string counterName)
        {
            foreach (CounterCreationData counter in installer.Counters)
            {
                if (counter.CounterName.Equals(counterName, StringComparison.CurrentCulture))
                    return counter;
            }
            return null;
        }
        static PerformanceCounterInstaller GetExistingInstaller(string categoryName,
                                                         IEnumerable<Installer> installers)
        {
            foreach (PerformanceCounterInstaller installer in installers)
            {
                if (installer.CategoryName.Equals(categoryName, StringComparison.CurrentCulture))
                    return installer;
            }
            return null;
        }
        PerformanceCounterInstaller GetOrCreateInstaller(Type instrumentedType,
                                                         ICollection<Installer> installers)
        {
            PerformanceCountersDefinitionAttribute attribute
                = (PerformanceCountersDefinitionAttribute)instrumentedType.GetCustomAttributes(typeof(PerformanceCountersDefinitionAttribute), false)[0];
            PerformanceCounterInstaller installer = GetExistingInstaller(attribute.CategoryName, installers);
            if (installer == null)
            {
                installer = new PerformanceCounterInstaller();
                PopulateCounterCategoryData(attribute, instrumentedType.Assembly, installer);
                installers.Add(installer);
            }
            return installer;
        }
        static string GetResourceString(string name,
                                        Assembly originalAssembly)
        {
            string translatedHelpString = null;
            string[] resourceNames = originalAssembly.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; i++)
            {
                try
                {
                    int lastDotResourcesString = resourceNames[i].LastIndexOf(".resources");
                    string resourceName = resourceNames[i].Remove(lastDotResourcesString);
                    ResourceManager manager = new ResourceManager(resourceName, originalAssembly);
                    translatedHelpString = manager.GetString(name);
                }
                catch (Exception)
                {
                }
                if (!string.IsNullOrEmpty(translatedHelpString))
                    return translatedHelpString;
            }
            return "";
        }
        void PopulateCounterCategoryData(PerformanceCountersDefinitionAttribute attribute,
                                         Assembly originalAssembly,
                                         PerformanceCounterInstaller installer)
        {
            installer.CategoryName = attribute.CategoryName;
            installer.CategoryHelp = GetCategoryHelp(attribute, originalAssembly);
            installer.CategoryType = attribute.CategoryType;
        }
    }
}
