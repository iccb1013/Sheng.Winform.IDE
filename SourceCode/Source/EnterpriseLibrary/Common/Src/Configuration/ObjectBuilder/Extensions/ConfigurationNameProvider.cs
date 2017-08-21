/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	public static class ConfigurationNameProvider
	{
		private const string nameSuffix = "___";
		public static string MakeUpName()
		{
			return Guid.NewGuid().ToString() + nameSuffix;
		}
		public static bool IsMadeUpName(string name)
		{
			if (name == null) return false;
			return name.EndsWith(nameSuffix);
		}	
	}
}
