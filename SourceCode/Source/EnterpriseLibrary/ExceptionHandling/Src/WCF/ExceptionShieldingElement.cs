/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Configuration;
using System.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF
{
    public class ExceptionShieldingElement : BehaviorExtensionElement
    {
        public const string ExceptionPolicyNameAttributeName = "exceptionPolicyName";
        public ExceptionShieldingElement()
        {
            this.ExceptionPolicyName = ExceptionShielding.DefaultExceptionPolicy;
        }
        [ConfigurationProperty(
            ExceptionShieldingElement.ExceptionPolicyNameAttributeName, 
            DefaultValue = ExceptionShielding.DefaultExceptionPolicy, 
            IsRequired=false)]
        public string ExceptionPolicyName
        {
            get { return this[ExceptionPolicyNameAttributeName] as string; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = ExceptionShielding.DefaultExceptionPolicy;
                }
                this[ExceptionPolicyNameAttributeName] = value; 
            }
        }
        public override void CopyFrom(ServiceModelExtensionElement from)
        {
            base.CopyFrom(from);
            ExceptionShieldingElement element = (ExceptionShieldingElement)from;
            this.ExceptionPolicyName = element.ExceptionPolicyName;
        }
        public override Type BehaviorType
        {
            get { return typeof(ExceptionShieldingBehavior); }
        }
        protected override object CreateBehavior()
        {
            return new ExceptionShieldingBehavior(this.ExceptionPolicyName);
        }
    }
}
