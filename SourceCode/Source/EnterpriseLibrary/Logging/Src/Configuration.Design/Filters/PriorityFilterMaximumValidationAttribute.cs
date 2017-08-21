/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using System.Globalization;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class PriorityFilterMaximumPriorityValidationAttribute : ValidationAttribute
    {
        protected override void ValidateCore(object instance, System.Reflection.PropertyInfo propertyInfo, IList<ValidationError> errors)
        {
			PriorityFilterNode node = instance as PriorityFilterNode;
			if (node != null)
			{
				if (node.MaximumPriority != null && node.MinimumPriority >= node.MaximumPriority)
				{
					string errorMessage = string.Format(CultureInfo.CurrentUICulture, Resources.MaxPrioShouldBeGreaterThanMinPrioError);
                    errors.Add(new ValidationError(node, propertyInfo.Name, errorMessage));
				}
			}
        }
    }
}
