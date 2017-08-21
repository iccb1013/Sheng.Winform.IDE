/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Install;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
	public abstract class AbstractInstallerBuilder
	{
		IList<Type> instrumentedTypes;
		public void Fill(Installer installer)
		{
			ICollection<Installer> installers = CreateInstallers(InstrumentedTypes);
			foreach (Installer childInstaller in installers)
			{
				installer.Installers.Add(childInstaller);
			}
		}
		protected IList<Type> InstrumentedTypes
		{
			get { return instrumentedTypes; }
			set { instrumentedTypes = value; }
		}
		protected AbstractInstallerBuilder(Type[] availableTypes, Type instrumentationAttributeType)
		{
			this.instrumentedTypes 
				= FindInstrumentedTypes(availableTypes, instrumentationAttributeType);
		}
		protected bool ConfirmAttributeExists(Type instrumentedType, Type attributeType)
		{
			object[] attributesFound = instrumentedType.GetCustomAttributes(attributeType, false);
			return attributesFound.Length == 0 ? false : true;
		}
		protected bool IsInstrumented(Type instrumentedType, Type instrumentedAttributeType)
		{
			if (instrumentedType == null) return false;
			Type attributeType = typeof(HasInstallableResourcesAttribute);
			if (ConfirmAttributeExists(instrumentedType, typeof(HasInstallableResourcesAttribute)) &&
			   (ConfirmAttributeExists(instrumentedType, instrumentedAttributeType)))
				return true;
			else
				return false;
		}
		private Type[] FindInstrumentedTypes(Type[] reflectableTypes, Type instrumentedAttributeType)
		{
			List<Type> instrumentedTypes = new List<Type>();
			foreach (Type type in reflectableTypes)
			{
				if (IsInstrumented(type, instrumentedAttributeType))
				{
					instrumentedTypes.Add(type);
				}
			}
			return instrumentedTypes.ToArray();
		}
		protected abstract ICollection<Installer> CreateInstallers(ICollection<Type> instrumentedTypes);
	}
}
