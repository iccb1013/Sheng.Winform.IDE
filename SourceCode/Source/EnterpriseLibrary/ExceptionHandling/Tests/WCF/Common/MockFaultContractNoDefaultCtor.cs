/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests
{
    [DataContract(Namespace = "http://FaultContracts/2006/03/MockFaultContractNoDefaultCtor")]
    public class MockFaultContractNoDefaultCtor 
    {
        private string message;
        private IDictionary data;
        private Guid id;
        private double someNumber;
        public MockFaultContractNoDefaultCtor(string message)
        {
            this.message = message;
        }
        [DataMember]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        [DataMember]
        public IDictionary Data
        {
            get { return data; }
            set { data = value; }
        }
        [DataMember]
        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }
        [DataMember]
        public double SomeNumber
        {
            get { return someNumber; }
            set { someNumber = value; }
        }
    }
}
