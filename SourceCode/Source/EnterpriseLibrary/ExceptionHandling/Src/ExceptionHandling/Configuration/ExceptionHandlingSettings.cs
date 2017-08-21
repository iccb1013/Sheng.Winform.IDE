/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    public class ExceptionHandlingSettings : SerializableConfigurationSection
    {
		public const string SectionName = "exceptionHandling";
        private const string policiesProperty = "exceptionPolicies";
        public static ExceptionHandlingSettings GetExceptionHandlingSettings(IConfigurationSource configurationSource)
		{
			return (ExceptionHandlingSettings)configurationSource.GetSection(SectionName);
		}
		public ExceptionHandlingSettings()
		{
			this[policiesProperty] = new NamedElementCollection<ExceptionPolicyData>();
		}
		[ConfigurationProperty(policiesProperty)]
		public NamedElementCollection<ExceptionPolicyData> ExceptionPolicies
		{
			get { return (NamedElementCollection<ExceptionPolicyData>)this[policiesProperty]; }
		}        
    }
}
