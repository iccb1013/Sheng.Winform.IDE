/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Configuration.Install;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
	public class EventLogInstallerBuilder  : AbstractInstallerBuilder
	{
		public EventLogInstallerBuilder(Type[] potentialTypes)
			: base(potentialTypes, typeof(EventLogDefinitionAttribute))
		{
		}
		protected override ICollection<Installer> CreateInstallers(ICollection<Type> instrumentedTypes)
		{
			IList<Installer> installers	= new List<Installer>();
			foreach (Type instrumentedType in instrumentedTypes)
			{
				EventLogDefinitionAttribute attribute 
					= (EventLogDefinitionAttribute)instrumentedType.GetCustomAttributes(typeof(EventLogDefinitionAttribute), false)[0];
				EventLogInstaller installer = new EventLogInstaller();
				installer.Log = attribute.LogName;
				installer.Source = attribute.SourceName;
				installer.CategoryCount = attribute.CategoryCount;
				if (attribute.CategoryResourceFile != null) installer.CategoryResourceFile = attribute.CategoryResourceFile;
				if (attribute.MessageResourceFile != null) installer.MessageResourceFile = attribute.MessageResourceFile;
				if (attribute.ParameterResourceFile != null) installer.ParameterResourceFile = attribute.ParameterResourceFile;
				installers.Add(installer);
			}
			return installers;
		}
	}
}
