/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class ConfigurationElementTypeAttribute : Attribute
	{
		private Type configurationType;
		public ConfigurationElementTypeAttribute()
		{
		}
		public ConfigurationElementTypeAttribute(Type configurationType)
		{
			this.configurationType = configurationType;
		}
		public Type ConfigurationType
		{
			get { return configurationType; }			
		}
	}
}
