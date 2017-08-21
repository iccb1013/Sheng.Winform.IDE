/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Filters
{
    [ManagementEntity]
    public partial class CustomFilterSetting : LogFilterSetting
    {
        string[] attributes;
        string filterType;
        public CustomFilterSetting(CustomLogFilterData sourceElement,
                                   string name,
                                   string filterType,
                                   string[] attributes)
            : base(sourceElement, name)
        {
            this.filterType = filterType;
            this.attributes = attributes;
        }
        [ManagementConfiguration]
        public string[] Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }
        [ManagementConfiguration]
        public string FilterType
        {
            get { return filterType; }
            set { filterType = value; }
        }
        [ManagementBind]
        public static CustomFilterSetting BindInstance(string ApplicationName,
                                                       string SectionName,
                                                       string Name)
        {
            return BindInstance<CustomFilterSetting>(ApplicationName, SectionName, Name);
        }
        [ManagementEnumerator]
        public static IEnumerable<CustomFilterSetting> GetInstances()
        {
            return GetInstances<CustomFilterSetting>();
        }
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return CustomLogFilterDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
