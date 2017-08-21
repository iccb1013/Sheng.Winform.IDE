/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters
{
	[ConfigurationElementType(typeof(CategoryFilterData))]
	public class CategoryFilter : LogFilter
	{
		private ICollection<string> categoryFilters;
		private CategoryFilterMode categoryFilterMode;
		public CategoryFilter(string name, ICollection<string> categoryFilters, CategoryFilterMode categoryFilterMode)
			: base(name)
		{
			this.categoryFilters = categoryFilters;
			this.categoryFilterMode = categoryFilterMode;
		}
		public override bool Filter(LogEntry log)
		{
			return ShouldLog(log.Categories);
		}
		public bool ShouldLog(IEnumerable<string> categories)
		{
			bool matchDetected = false;
			foreach (string category in categories)
			{
				if (this.CategoryFilters.Contains(category))
				{
					matchDetected = true;
					break;
				}
			}
			return ((CategoryFilterMode.AllowAllExceptDenied == this.CategoryFilterMode) && !matchDetected)
				|| ((CategoryFilterMode.DenyAllExceptAllowed == this.CategoryFilterMode) && matchDetected);
		}
		public bool ShouldLog(string category)
		{
			return ShouldLog(new string[] { category });
		}
		public ICollection<string> CategoryFilters
		{
			get { return categoryFilters; }
		}
		public CategoryFilterMode CategoryFilterMode
		{
			get { return categoryFilterMode; }
			set { categoryFilterMode = value; }
		}
	}
}
