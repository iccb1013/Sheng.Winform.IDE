/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Runtime.Serialization;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	[Serializable]
	public class RegistryAccessException : Exception, ISerializable
	{
		public RegistryAccessException()
			: base()
		{ }
		public RegistryAccessException(String message)
			: base(message)
		{ }
		public RegistryAccessException(String message, Exception inner)
			: base(message, inner)
		{ }
		protected RegistryAccessException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
	}
}
