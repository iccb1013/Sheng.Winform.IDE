/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	[Assembler(typeof(CategoryFilterAssembler))]
	[ContainerPolicyCreator(typeof(CategoryFilterPolicyCreator))]
	public class CategoryFilterData : LogFilterData
	{
		private const string categoryFilterModeProperty = "categoryFilterMode";
		private const string categoryFiltersProperty = "categoryFilters";
		public CategoryFilterData()
		{
		}
		public CategoryFilterData(NamedElementCollection<CategoryFilterEntry> categoryFilters, CategoryFilterMode categoryFilterMode)
			: this("category", categoryFilters, categoryFilterMode)
		{
		}
		public CategoryFilterData(string name, NamedElementCollection<CategoryFilterEntry> categoryFilters, CategoryFilterMode categoryFilterMode)
			: base(name, typeof(CategoryFilter))
		{
			this.CategoryFilters = categoryFilters;
			this.CategoryFilterMode = categoryFilterMode;
		}
		[ConfigurationProperty(categoryFilterModeProperty)]
		public CategoryFilterMode CategoryFilterMode
		{
			get
			{
				return (CategoryFilterMode)this[categoryFilterModeProperty];
			}
			set
			{
				this[categoryFilterModeProperty] = value;
			}
		}
		[ConfigurationProperty(categoryFiltersProperty)]
		public NamedElementCollection<CategoryFilterEntry> CategoryFilters
		{
			get
			{
				return (NamedElementCollection<CategoryFilterEntry>)base[categoryFiltersProperty];
			}
			private set
			{
				base[categoryFiltersProperty] = value;
			}
		}
	}
	public class CategoryFilterAssembler : IAssembler<ILogFilter, LogFilterData>
	{
		public ILogFilter Assemble(IBuilderContext context, LogFilterData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			CategoryFilterData castedObjectConfiguration = (CategoryFilterData)objectConfiguration;
			ICollection<string> categoryFilters = new List<string>();
			foreach (CategoryFilterEntry entry in castedObjectConfiguration.CategoryFilters)
			{
				categoryFilters.Add(entry.Name);
			}
			ILogFilter createdObject
				= new CategoryFilter(
					castedObjectConfiguration.Name,
					categoryFilters,
					castedObjectConfiguration.CategoryFilterMode);
			return createdObject;
		}
	}
}
