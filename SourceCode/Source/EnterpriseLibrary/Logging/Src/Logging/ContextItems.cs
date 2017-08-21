/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Security;
namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	public class ContextItems
	{
		public const string CallContextSlotName = "EntLibLoggerContextItems";
		public ContextItems()
		{
		}
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public void SetContextItem(object key, object value)
		{
			Hashtable contextItems = (Hashtable)CallContext.GetData(CallContextSlotName);
			if (contextItems == null)
			{
				contextItems = new Hashtable();
			}
			contextItems[key] = value;
			CallContext.SetData(CallContextSlotName, contextItems);
		}
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public void FlushContextItems()
		{
			CallContext.FreeNamedDataSlot(CallContextSlotName);
		}
		public void ProcessContextItems(LogEntry log)
		{
			Hashtable contextItems = null;
			if (SecurityManager.IsGranted(new SecurityPermission(SecurityPermissionFlag.Infrastructure)))
			{
				try
				{
					contextItems = GetContextItems();
				}
				catch (SecurityException)
				{
				}
			}
			if (contextItems == null || contextItems.Count == 0)
			{
				return;
			}
			foreach (DictionaryEntry entry in contextItems)
			{
				string itemValue = GetContextItemValue(entry.Value);
				log.ExtendedProperties.Add(entry.Key.ToString(), itemValue);
			}
		}
		private static Hashtable GetContextItems()
		{
			return (Hashtable)CallContext.GetData(CallContextSlotName);
		}
		private string GetContextItemValue(object contextData)
		{
			string value = string.Empty;
			try
			{
				if (contextData.GetType() == typeof(byte[]))
				{
					value = Convert.ToBase64String((byte[])contextData);
				}
				else
				{
					value = contextData.ToString();
				}
			}
			catch
			{ /* ignore exceptions */
			}
			return value;
		}
	}
}
