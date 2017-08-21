/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Drawing.Design;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters
{
    public sealed class CategoryFilterNode : LogFilterNode
    {
		private CategoryFilterSettings categoryFilterSettings;		
        public CategoryFilterNode()
            : this(new CategoryFilterData(Resources.CategoryLogFilterNode, new NamedElementCollection<CategoryFilterEntry>(), CategoryFilterMode.DenyAllExceptAllowed))
        {
        }
        public CategoryFilterNode(CategoryFilterData categoryLogFilterData)
            : base(null == categoryLogFilterData ? string.Empty : categoryLogFilterData.Name)
        {
			if (null == categoryLogFilterData) throw new ArgumentNullException("categoryLogFilterData");
			categoryFilterSettings = new CategoryFilterSettings(categoryLogFilterData.CategoryFilterMode, new NamedElementCollection<CategoryFilterEntry>());
			foreach (CategoryFilterEntry filterEntry in categoryLogFilterData.CategoryFilters)
			{
				this.categoryFilterSettings.CategoryFilters.Add(filterEntry);
			}            
        }
        [Editor(typeof(CategoryFilterEditor), typeof(UITypeEditor))]
        [SRDescription("CategoryFilterExpressionDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public CategoryFilterSettings CategoryFilterExpression
        {
            get { return new CategoryFilterSettings(categoryFilterSettings.CategoryFilterMode, categoryFilterSettings.CategoryFilters); }
            set
            {
                categoryFilterSettings.CategoryFilters.Clear();
                foreach (CategoryFilterEntry filterEntry in value.CategoryFilters)
                {
                    categoryFilterSettings.CategoryFilters.Add(filterEntry);
                }
                categoryFilterSettings.CategoryFilterMode = value.CategoryFilterMode;
            }
        }
		public override LogFilterData LogFilterData
		{
			get 
			{ 
				NamedElementCollection<CategoryFilterEntry> entries = new NamedElementCollection<CategoryFilterEntry>();
				foreach (CategoryFilterEntry filterEntry in categoryFilterSettings.CategoryFilters)
				{
					entries.Add(filterEntry);
				}
				return new CategoryFilterData(Name, entries ,categoryFilterSettings.CategoryFilterMode); 
			}
		}
    }
}
