/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	public interface IRegistryKey : IDisposable
	{
		void Close();
		bool? GetBoolValue(String valueName);
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004")]
		T? GetEnumValue<T>(String valueName) where T : struct;
		int? GetIntValue(String valueName);
		String GetStringValue(String valueName);
		Type GetTypeValue(String valueName);
		string[] GetValueNames();
		IRegistryKey OpenSubKey(string name);
		Boolean IsPolicyKey { get; }
		String Name { get; }
	}
}
