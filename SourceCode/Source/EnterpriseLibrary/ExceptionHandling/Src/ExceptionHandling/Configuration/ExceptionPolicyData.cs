/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
	public class ExceptionPolicyData : NamedConfigurationElement
    {
		private const string exceptionTypesProperty = "exceptionTypes";
        public ExceptionPolicyData() 
        {
        }
        public ExceptionPolicyData(string name) : base(name)
        {
			this[exceptionTypesProperty] = new NamedElementCollection<ExceptionTypeData>();
        }
		[ConfigurationProperty(exceptionTypesProperty)]		
		public NamedElementCollection<ExceptionTypeData> ExceptionTypes
		{
			get
			{
				return (NamedElementCollection<ExceptionTypeData>)this[exceptionTypesProperty];
			}
		}		
	}
}
