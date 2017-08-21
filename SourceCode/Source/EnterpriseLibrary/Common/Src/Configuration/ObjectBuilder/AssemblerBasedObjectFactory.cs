/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	public abstract class AssemblerBasedObjectFactory<TObject, TConfiguration>
		where TObject : class
		where TConfiguration : class
	{
		private IDictionary<Type, IAssembler<TObject, TConfiguration>> assemblersMapping 
			= new Dictionary<Type, IAssembler<TObject, TConfiguration>>();
		private object assemblersMappingLock = new object();
		ConfigurationReflectionCache reflectionCache = new ConfigurationReflectionCache();
		public virtual TObject Create(IBuilderContext context, TConfiguration objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			IAssembler<TObject, TConfiguration> assembler = GetAssembler(objectConfiguration);
			TObject createdObject = assembler.Assemble(context, objectConfiguration, configurationSource, reflectionCache);
			return createdObject;
		}
		private IAssembler<TObject, TConfiguration> GetAssembler(TConfiguration objectConfiguration)
		{
			bool exists = false;
			Type type = objectConfiguration.GetType();
			IAssembler<TObject, TConfiguration> assembler;
			lock (assemblersMappingLock)
			{
				exists = assemblersMapping.TryGetValue(type, out assembler);
			}
			if (exists)
			{
				return assembler;
			}
			AssemblerAttribute assemblerAttribute = GetAssemblerAttribute(type);
			if (!typeof(IAssembler<TObject, TConfiguration>).IsAssignableFrom(assemblerAttribute.AssemblerType))
			{
				throw new InvalidOperationException(
					string.Format(
						Resources.Culture,
						Resources.ExceptionAssemblerTypeNotCompatible,
						objectConfiguration.GetType().FullName,
						typeof(IAssembler<TObject, TConfiguration>),
						assemblerAttribute.AssemblerType.FullName));
			}
			assembler = (IAssembler<TObject, TConfiguration>)Activator.CreateInstance(assemblerAttribute.AssemblerType);
			lock (assemblersMappingLock)
			{
				assemblersMapping[type] = assembler;
			}
			return assembler;
		}
		private AssemblerAttribute GetAssemblerAttribute(Type type)
		{
			AssemblerAttribute assemblerAttribute 
				= Attribute.GetCustomAttribute(type, typeof(AssemblerAttribute)) as AssemblerAttribute;
			if (assemblerAttribute == null)
			{ 
				throw new InvalidOperationException(
					string.Format(
						Resources.Culture,
						Resources.ExceptionAssemblerAttributeNotSet,
						type.FullName));
			}
			return assemblerAttribute;
		}
	}
}
