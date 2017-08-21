/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design
{
    public class FaultContractPropertyMapping
    {
        string name;
        string source;
        [Required]
        [SRDescription("FaultContractMappingNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        [Required]
        [SRDescription("FaultContractMappingSourceDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string Source
        {
            get { return source; }
            set { source = value; }
        }
    }
}
