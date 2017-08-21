/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	public class ImplementationKeyComparer : IEqualityComparer<ImplementationKey>
	{
        public bool Equals(ImplementationKey x, ImplementationKey y)
		{
			if (x.FileName != y.FileName
				&& (x.FileName == null || !x.FileName.Equals(y.FileName, StringComparison.OrdinalIgnoreCase)))
				return false;
			if (x.ApplicationName != y.ApplicationName
				&& (x.ApplicationName == null || !x.ApplicationName.Equals(y.ApplicationName, StringComparison.OrdinalIgnoreCase)))
				return false;
			if (x.EnableGroupPolicies != y.EnableGroupPolicies)
				return false;
			return true;
		}
        public int GetHashCode(ImplementationKey obj)
		{
			return (obj.FileName == null ? 0 : obj.FileName.GetHashCode())
				^ (obj.ApplicationName == null ? 0 : obj.ApplicationName.GetHashCode())
				^ obj.EnableGroupPolicies.GetHashCode();
		}
	}
}
