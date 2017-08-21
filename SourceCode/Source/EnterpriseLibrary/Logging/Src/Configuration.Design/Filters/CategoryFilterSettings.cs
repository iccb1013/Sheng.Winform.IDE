/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters
{
    public class CategoryFilterSettings
    {
        private CategoryFilterMode categoryFilterMode;
        private NamedElementCollection<CategoryFilterEntry> categoryFilters;
        public CategoryFilterMode CategoryFilterMode
        {
            get { return categoryFilterMode; }
            set { categoryFilterMode = value; }
        }
        public NamedElementCollection<CategoryFilterEntry> CategoryFilters
        {
            get { return categoryFilters; }            
        }
        public CategoryFilterSettings(CategoryFilterMode categoryFilterMode, NamedElementCollection<CategoryFilterEntry> categoryFilters)
        {
            this.categoryFilterMode = categoryFilterMode;
            this.categoryFilters = categoryFilters;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (CategoryFilterMode == CategoryFilterMode.AllowAllExceptDenied)
            {
                sb.Append(Resources.CategoryFilterSummaryAllow);
            }
            else
            {
                sb.Append(Resources.CategoryFilterSummaryDeny);
            }
            sb.Append(": ");
            bool first = true;
            foreach (CategoryFilterEntry categoryFilter in CategoryFilters)
            {
                if (!first)
                {
                    sb.Append(", ");
                }
                sb.Append(categoryFilter.Name);
                first = false;
            }
            return sb.ToString();
        }
    }
}
