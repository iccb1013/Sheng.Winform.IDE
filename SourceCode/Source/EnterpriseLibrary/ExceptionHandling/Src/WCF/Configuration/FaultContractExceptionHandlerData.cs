/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration
{
	[Assembler(typeof(FaultContractHandlerAssembler))]
	[ContainerPolicyCreator(typeof(FaultContractExceptionHandlerPolicyCreator))]
	public class FaultContractExceptionHandlerData : ExceptionHandlerData
	{
		public const string ExceptionMessageAttributeName = "exceptionMessage";
		public const string FaultContractTypeAttributeName = "faultContractType";
		const string PropertyMappingsPropertyName = "mappings";
		public FaultContractExceptionHandlerData() { }
		public FaultContractExceptionHandlerData(string name)
			: this(name, string.Empty) { }
		public FaultContractExceptionHandlerData(string name,
												 string faultContractType)
			: base(name, typeof(FaultContractExceptionHandler))
		{
			FaultContractType = faultContractType;
		}
		public NameValueCollection Attributes
		{
			get
			{
				NameValueCollection result = new NameValueCollection();
				foreach (FaultContractExceptionHandlerMappingData mapping in PropertyMappings)
				{
					result.Add(mapping.Name, mapping.Source);
				}
				return result;
			}
		}
		[ConfigurationProperty(ExceptionMessageAttributeName, IsRequired = false)]
		public string ExceptionMessage
		{
			get { return this[ExceptionMessageAttributeName] as string; }
			set { this[ExceptionMessageAttributeName] = value; }
		}
		[ConfigurationProperty(FaultContractTypeAttributeName, IsRequired = true)]
		public string FaultContractType
		{
			get { return this[FaultContractTypeAttributeName] as string; }
			set { this[FaultContractTypeAttributeName] = value; }
		}
		[ConfigurationProperty(PropertyMappingsPropertyName)]
		public NamedElementCollection<FaultContractExceptionHandlerMappingData> PropertyMappings
		{
			get { return (NamedElementCollection<FaultContractExceptionHandlerMappingData>)this[PropertyMappingsPropertyName]; }
		}
	}
	public class FaultContractHandlerAssembler : IAssembler<IExceptionHandler, ExceptionHandlerData>
	{
		public IExceptionHandler Assemble(IBuilderContext context,
										  ExceptionHandlerData objectConfiguration,
										  IConfigurationSource configurationSource,
										  ConfigurationReflectionCache reflectionCache)
		{
			FaultContractExceptionHandlerData castedObjectConfiguration
				= (FaultContractExceptionHandlerData)objectConfiguration;
			FaultContractExceptionHandler createdObject
				= new FaultContractExceptionHandler(
					Type.GetType(castedObjectConfiguration.FaultContractType),
					castedObjectConfiguration.ExceptionMessage,
					castedObjectConfiguration.Attributes);
			return createdObject;
		}
	}
}
