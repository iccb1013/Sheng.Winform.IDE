/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration
{
	public class FaultContractExceptionHandlerMappingData : NamedConfigurationElement
	{
		public FaultContractExceptionHandlerMappingData()
		{ }
		public FaultContractExceptionHandlerMappingData(string name, string source)
		{
			this.Name = name;
			this.Source = source;
		}
		private const string SourcePropertyName = "source";
		[ConfigurationProperty(SourcePropertyName)]
		public string Source
		{
			get { return (string)this[SourcePropertyName]; }
			set { this[SourcePropertyName] = value; }
		}
	}
}
