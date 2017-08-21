/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	internal class CustomTraceListenerDataHelper
		: BasicCustomTraceListenerDataHelper
	{
		private static readonly ConfigurationProperty formatterProperty =
			new ConfigurationProperty(CustomTraceListenerData.formatterNameProperty,
										typeof(string),
										null,   
										null,   
										null,	
										ConfigurationPropertyOptions.None);
		internal CustomTraceListenerDataHelper(CustomTraceListenerData helpedCustomProviderData)
			: base(helpedCustomProviderData)
		{
			propertiesCollection.Add(formatterProperty);
		}
		protected override bool IsKnownPropertyName(string propertyName)
		{
			return base.IsKnownPropertyName(propertyName)
				|| CustomTraceListenerData.formatterNameProperty.Equals(propertyName);
		}
	}
}
