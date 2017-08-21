/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation
{
	public interface IExtraInformationProvider
	{
		void PopulateDictionary(IDictionary<string, object> dict);
	}
}
