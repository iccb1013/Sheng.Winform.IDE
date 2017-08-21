/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests.Mocks
{
	public class MockRegistryKey : RegistryKeyBase, IRegistryKey
	{
		private bool open;
		private IDictionary<String, Object> values;
		private IDictionary<String, MockRegistryKey> subKeys;
		private List<String> requests;
		public MockRegistryKey(bool open)
		{
			this.open = open;
			this.values = new Dictionary<String, Object>();
			this.subKeys = new Dictionary<String, MockRegistryKey>();
			this.requests = new List<String>();
		}
		public static bool CheckAllClosed(params MockRegistryKey[] keys)
		{
			foreach (MockRegistryKey key in keys)
			{
				if (key.open)
					return false;
			}
			return true;
		}
		public void AddIntValue(String name, Int32 value)
		{
			values.Add(name, value);
		}
		public void AddStringValue(String name, String value)
		{
			values.Add(name, value);
		}
		public void AddBooleanValue(String name, Boolean value)
		{
			values.Add(name, value ? 1 : 0);
		}
		public void AddEnumValue<T>(String name, T value)
			where T : struct
		{
			AddStringValue(name, value.ToString());
		}
		public void AddSubKey(String name, MockRegistryKey subKey)
		{
			subKeys.Add(name, subKey);
		}
		public ICollection<String> GetRequests()
		{
			return requests;
		}
		protected override object DoGetValue(string name)
		{
			CheckOpen();
			Object value = values.ContainsKey(name) ? value = values[name] : null;
			requests.Add("value: " + name + " - " + value);
			return value;
		}
		public override void Close()
		{
			CheckOpen();
			open = false;
		}
		public override string[] GetValueNames()
		{
			CheckOpen();
			return new List<string>(values.Keys).ToArray();
		}
		public override IRegistryKey DoOpenSubKey(string name)
		{
			CheckOpen();
			MockRegistryKey subKey = subKeys.ContainsKey(name) ? subKeys[name] : null;
			requests.Add("key: " + name + " - " + subKey);
			if (subKey != null)
				subKey.SetOpen();
			return subKey;
		}
		public void Dispose()
		{
			Close();
		}
		private void CheckOpen()
		{
			if (!open)
				throw new InvalidOperationException("Registry key already closed!");
		}
		private void SetOpen()
		{
			if (open)
				throw new InvalidOperationException("Registry key already opened!");
			open = true;
		}
		public bool IsOpen
		{
			get { return open; }
		}
		public override string Name
		{
			get { return "mock"; }
		}
	}
}
