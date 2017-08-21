/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	[Assembler(typeof(PriorityFilterAssembler))]
	public class PriorityFilterData : LogFilterData
	{
		private const string minimumPriorityProperty = "minimumPriority";
		private const string maximumPriorityProperty = "maximumPriority";
		public PriorityFilterData()
		{
		}
		public PriorityFilterData(int minimumPriority)
			: this("priority", minimumPriority)
		{
		}
		public PriorityFilterData(string name, int minimumPriority)
			: base(name, typeof(PriorityFilter))
		{
			this.MinimumPriority = minimumPriority;
		}
		[ConfigurationProperty(minimumPriorityProperty)]
		public int MinimumPriority
		{
			get
			{
				return (int)this[minimumPriorityProperty];
			}
			set
			{
				this[minimumPriorityProperty] = value;
			}
		}
		[ConfigurationProperty(maximumPriorityProperty, DefaultValue = int.MaxValue)]
		public int MaximumPriority
		{
			get
			{
				return (int)this[maximumPriorityProperty];
			}
			set
			{
				this[maximumPriorityProperty] = value;
			}
		}
	}
	public class PriorityFilterAssembler : IAssembler<ILogFilter, LogFilterData>
	{
		public ILogFilter Assemble(IBuilderContext context, LogFilterData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			PriorityFilterData castedObjectConfiguration = (PriorityFilterData)objectConfiguration;
			ILogFilter createdObject
				= new PriorityFilter(
					castedObjectConfiguration.Name,
					castedObjectConfiguration.MinimumPriority,
					castedObjectConfiguration.MaximumPriority);
			return createdObject;
		}
	}
}
