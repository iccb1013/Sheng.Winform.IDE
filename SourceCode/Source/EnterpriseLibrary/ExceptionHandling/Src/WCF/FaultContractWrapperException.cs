/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF
{
    public class FaultContractWrapperException : Exception 
    {
        private object faultContract;
        public FaultContractWrapperException(object faultContract) : this(faultContract, Guid.NewGuid(), null)
        {
        }
        public FaultContractWrapperException(object faultContract, Guid handlingInstanceId)
            : this(faultContract, handlingInstanceId, null)
        {
        }
        public FaultContractWrapperException(object faultContract, Guid handlingInstanceId, string exceptionMessage)
            : base(exceptionMessage ?? ExceptionUtility.FormatExceptionMessage(null, handlingInstanceId))
        {
            if (faultContract == null)
            {
                throw new ArgumentNullException("faultContract");
            }
            this.faultContract = faultContract;
        }
        public object FaultContract
        {
            get { return faultContract; }
            set { faultContract = value; }
        }
    }
}
