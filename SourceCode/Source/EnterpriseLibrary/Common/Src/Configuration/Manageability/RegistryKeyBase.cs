/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	public abstract class RegistryKeyBase
	{
		public const String PolicyValueName = "Available";
		public abstract void Close();
		protected abstract object DoGetValue(String valueName);
		public abstract IRegistryKey DoOpenSubKey(String name);
		public bool? GetBoolValue(String valueName)
		{
			int? value = GetIntValue(valueName);
			return value.Value == 1;
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004", Justification="The generic parameter is used to drive the type of the return value.")]
		public T? GetEnumValue<T>(String valueName)
			where T : struct
		{
			String value = GetStringValue(valueName);
			try
			{
				return (T)Enum.Parse(typeof(T), value);
			}
			catch (ArgumentException)
			{
				throw new RegistryAccessException(
					String.Format(Resources.Culture,
									Resources.ExceptionRegistryValueNotEnumValue,
									this.Name,
									valueName,
									typeof(T).Name,
									value));
			}
		}
		public int? GetIntValue(String valueName)
		{
			object value = GetValue(valueName);
			try
			{
				return (int)value;
			}
			catch (InvalidCastException)
			{
				throw new RegistryAccessException(
					String.Format(Resources.Culture,
									Resources.ExceptionRegistryValueOfWrongType,
									this.Name,
									valueName,
									typeof(int).Name,
									value.GetType().Name));
			}
		}
		public String GetStringValue(String valueName)
		{
			object value = GetValue(valueName);
			try
			{
				return (String)value;
			}
			catch (InvalidCastException)
			{
				throw new RegistryAccessException(
					String.Format(Resources.Culture,
									Resources.ExceptionRegistryValueOfWrongType,
									this.Name,
									valueName,
									typeof(String).Name,
									value.GetType().Name));
			}
		}
		public Type GetTypeValue(String valueName)
		{
			String value = GetStringValue(valueName);
			Type type = Type.GetType(value, false);
			if (type == null)
			{
				throw new RegistryAccessException(
					String.Format(Resources.Culture,
									Resources.ExceptionRegistryValueNotTypeName,
									this.Name,
									valueName,
									value));
			}
			return type;
		}
		public abstract string[] GetValueNames();
		public IRegistryKey OpenSubKey(String name)
		{
			CheckValidName(name, "name");
			return DoOpenSubKey(name);
		}
		public Boolean IsPolicyKey
		{
			get
			{
				return DoGetValue(PolicyValueName) != null;
			}
		}
		public abstract String Name { get; }
		private object GetValue(String valueName)
		{
			CheckValidName(valueName, "valueName");
			object value = DoGetValue(valueName);
			if (value == null)
			{
				throw new RegistryAccessException(
					String.Format(Resources.Culture,
						Resources.ExceptionMissingRegistryValue,
						this.Name,
						valueName));
			}
			return value;
		}
		private static void CheckValidName(String name, String argumentName)
		{
			if (name == null)
			{
				throw new ArgumentNullException(argumentName);
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(String.Format(Resources.Culture,
						Resources.ExceptionArgumentEmpty, 
						argumentName));
			}
		}
	}
}
