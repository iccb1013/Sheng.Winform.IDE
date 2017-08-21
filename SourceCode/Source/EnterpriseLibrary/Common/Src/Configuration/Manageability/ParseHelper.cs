/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	public static class ParseHelper
	{
		public static T ParseEnum<T>(string value, bool throwIfInvalid)
		{
			try
			{
				return (T)Enum.Parse(typeof(T), value);
			}
			catch (ArgumentException)
			{
				if (throwIfInvalid)
					throw;
			}
			return default(T);
		}
        public static bool TryParseEnum<T>(string value, out T result)
	    {
            try
            {
                result = (T) Enum.Parse(typeof (T), value);
                return true;
            }
            catch(ArgumentException)
            {
                result = default(T);
                return false;
            }
	    }
		public static Type ParseType(string value, bool throwIfInvalid)
		{
			Type type = Type.GetType(value, false);
			if (type == null && throwIfInvalid)
			{
				throw new ArgumentException();
			}
			return type;
		}
	}
}
