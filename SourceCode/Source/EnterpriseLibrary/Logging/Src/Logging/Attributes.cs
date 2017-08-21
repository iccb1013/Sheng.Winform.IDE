/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Management.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public sealed class InternalAttribute : IgnoreMemberAttribute 
    {
    }
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
	public sealed class InstrumentationTypeAttribute : System.Management.Instrumentation.InstrumentationClassAttribute
	{
		public InstrumentationTypeAttribute()
			: base(InstrumentationType.Event)
		{
		}
	}
}
