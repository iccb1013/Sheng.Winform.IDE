/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Management.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	public static class ManagementEntityTypesRegistrar
	{
		private static IDictionary<Type, Type> registeredTypes = new Dictionary<Type, Type>();
		private static object registeredTypesLock = new object();
		public static void SafelyRegisterTypes(params Type[] types)
		{
			lock (registeredTypesLock)
			{
				foreach (Type type in types)
				{
					if (!registeredTypes.ContainsKey(type))
					{
						DoRegisterType(type);
						registeredTypes.Add(type, type);
					}
				}
			}
		}
		public static void SafelyUnregisterTypes(params Type[] types)
		{
			lock (registeredTypesLock)
			{
				foreach (Type type in types)
				{
					if (registeredTypes.ContainsKey(type))
					{
						DoUnregisterType(type);
						registeredTypes.Remove(type);
					}
				}
			}
		}
		public static void UnregisterAll()
		{
			lock (registeredTypesLock)
			{
				foreach (Type type in registeredTypes.Keys)
				{
					DoUnregisterType(type);
				}
				registeredTypes.Clear();
			}
		}
		private static void DoRegisterType(Type type)
		{
			try
			{
				InstrumentationManager.RegisterType(type);
			}
			catch (NullReferenceException)
			{
			}
		}
		private static void DoUnregisterType(Type type)
		{
			try
			{
				InstrumentationManager.UnregisterType(type);
			}
			catch (NullReferenceException)
			{
			}
		}
	}
}
