/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	public class ConfigurationManageabilityProviderAttributeRetriever
	{
		private ICollection<ConfigurationSectionManageabilityProviderAttribute> sectionManageabilityProviderAttributes;
		private ICollection<ConfigurationElementManageabilityProviderAttribute> elementManageabilityProviderAttributes;
		public ConfigurationManageabilityProviderAttributeRetriever()
			: this(AppDomain.CurrentDomain.BaseDirectory)
		{ }
		public ConfigurationManageabilityProviderAttributeRetriever(String baseDirectory)
			: this(GetAssemblyNames(baseDirectory))
		{ }
		public ConfigurationManageabilityProviderAttributeRetriever(IEnumerable<String> fileNames)
		{
			sectionManageabilityProviderAttributes
				= new List<ConfigurationSectionManageabilityProviderAttribute>();
			elementManageabilityProviderAttributes
				= new List<ConfigurationElementManageabilityProviderAttribute>();
			LoadRegisteredManageabilityProviders(fileNames, sectionManageabilityProviderAttributes, elementManageabilityProviderAttributes);
		}
		public IEnumerable<ConfigurationSectionManageabilityProviderAttribute> SectionManageabilityProviderAttributes
		{
			get { return sectionManageabilityProviderAttributes; }
		}
		public IEnumerable<ConfigurationElementManageabilityProviderAttribute> ElementManageabilityProviderAttributes
		{
			get { return elementManageabilityProviderAttributes; }
		}
		private static void LoadRegisteredManageabilityProviders(IEnumerable<string> fileNames, 
			ICollection<ConfigurationSectionManageabilityProviderAttribute> sectionManageabilityProviderAttributes, 
			ICollection<ConfigurationElementManageabilityProviderAttribute> elementManageabilityProviderAttributes)
		{
			foreach (string file in fileNames)
			{
				Assembly assembly = null;
				try
				{
					AssemblyName assemblyName = AssemblyName.GetAssemblyName(file);
					assembly = Assembly.Load(assemblyName);
				}
				catch (BadImageFormatException)
				{
				}
				if (assembly != null)
				{
					LoadAttributes<ConfigurationSectionManageabilityProviderAttribute>(assembly, sectionManageabilityProviderAttributes);
					LoadAttributes<ConfigurationElementManageabilityProviderAttribute>(assembly, elementManageabilityProviderAttributes);
				}
			}
		}
		private static IEnumerable<string> GetAssemblyNames(String baseDirectory)
		{
			return Directory.GetFiles(baseDirectory, "*.dll");
		}
		private static void LoadAttributes<T>(Assembly assembly, ICollection<T> manageabilityProviderAttributes)
		{
			foreach (T attribute in assembly.GetCustomAttributes(typeof(T), false))
			{
				manageabilityProviderAttributes.Add(attribute);
			}
		}
	}
}
