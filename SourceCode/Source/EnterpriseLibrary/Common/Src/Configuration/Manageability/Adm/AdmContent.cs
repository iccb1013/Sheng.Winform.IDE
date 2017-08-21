/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.IO;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm
{
    public class AdmContent
    {
        List<AdmCategory> categories;
        public AdmContent()
        {
            categories = new List<AdmCategory>();
        }
        public IEnumerable<AdmCategory> Categories
        {
            get { return categories; }
        }
        public void AddCategory(AdmCategory category)
        {
            categories.Add(category);
        }
        public void Write(TextWriter writer)
        {
            foreach (AdmCategory category in categories)
            {
                category.Write(writer);
            }
        }
    }
}
