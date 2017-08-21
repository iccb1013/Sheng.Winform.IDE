/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
	public interface IEventLogEntryFormatter
	{
		string GetEntryText(string message, params string[] extraInformation);
		string GetEntryText(string message, Exception exception, params string[] extraInformation);
	}
}
