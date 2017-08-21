/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Manageability
{
    [ManagementEntity]
    public class FaultContractExceptionHandlerSetting : ExceptionHandlerSetting
    {
        string[] attributes;
        string exceptionMessage;
        string faultContractType;
        public FaultContractExceptionHandlerSetting(ConfigurationElement sourceElement,
                                                    string name,
                                                    string exceptionMessage,
                                                    string faultContractType,
                                                    string[] attributes)
            : base(sourceElement, name)
        {
            this.exceptionMessage = exceptionMessage;
            this.faultContractType = faultContractType;
            this.attributes = attributes;
        }
        [ManagementConfiguration]
        public string[] Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }
        [ManagementConfiguration]
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
            set { exceptionMessage = value; }
        }
        [ManagementConfiguration]
        public string FaultContractType
        {
            get { return faultContractType; }
            set { faultContractType = value; }
        }
        [ManagementBind]
        public static FaultContractExceptionHandlerSetting BindInstance(string applicationName,
                                                                        string sectionName,
                                                                        string policy,
                                                                        string exceptionType,
                                                                        string name)
        {
            return BindInstance<FaultContractExceptionHandlerSetting>(applicationName, sectionName, policy, exceptionType, name);
        }
        [ManagementEnumerator]
        public static IEnumerable<FaultContractExceptionHandlerSetting> GetInstances()
        {
            return GetInstances<FaultContractExceptionHandlerSetting>();
        }
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return FaultContractExceptionHandlerDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
