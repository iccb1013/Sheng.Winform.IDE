/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm
{
    public class AdmCategory
    {
        internal const String CategoryEndTemplate = "END CATEGORY\t; \"{0}\"";
        internal const String CategoryStartTemplate = "CATEGORY \"{0}\"";
        List<AdmCategory> categories;
        String name;
        List<AdmPolicy> policies;
        public AdmCategory(String categoryName)
        {
            name = categoryName;
            categories = new List<AdmCategory>();
            policies = new List<AdmPolicy>();
        }
        public IEnumerable<AdmCategory> Categories
        {
            get { return categories; }
        }
        public String Name
        {
            get { return name; }
        }
        public IEnumerable<AdmPolicy> Policies
        {
            get { return policies; }
        }
        internal void AddCategory(AdmCategory category)
        {
            categories.Add(category);
        }
        internal void AddPolicy(AdmPolicy policy)
        {
            policies.Add(policy);
        }
        internal void Write(TextWriter writer)
        {
            writer.WriteLine(String.Format(CultureInfo.InvariantCulture, CategoryStartTemplate, name));
            foreach (AdmCategory category in categories)
            {
                category.Write(writer);
            }
            foreach (AdmPolicy policy in policies)
            {
                policy.Write(writer);
            }
            writer.WriteLine(String.Format(CultureInfo.InvariantCulture, CategoryEndTemplate, name));
        }
    }
}
