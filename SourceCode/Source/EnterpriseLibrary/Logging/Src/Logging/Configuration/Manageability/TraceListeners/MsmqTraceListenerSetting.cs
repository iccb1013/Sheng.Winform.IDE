/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
    [ManagementEntity]
    public partial class MsmqTraceListenerSetting : TraceListenerSetting
    {
        string formatter;
        string messagePriority;
        string queuePath;
        bool recoverable;
        string timeToBeReceived;
        string timeToReachQueue;
        string transactionType;
        bool useAuthentication;
        bool useDeadLetterQueue;
        bool useEncryption;
		public MsmqTraceListenerSetting(MsmqTraceListenerData sourceElement,
                                          string name,
                                          string formatter,
                                          string messagePriority,
                                          string queuePath,
                                          bool recoverable,
                                          string timeToBeReceived,
                                          string timeToReachQueue,
                                          string traceOutputOptions,
                                          string transactionType,
                                          bool useAuthentication,
                                          bool useDeadLetterQueue,
                                          bool useEncryption,
										  string filter)
            : base(sourceElement, name, traceOutputOptions, filter)
        {
            this.formatter = formatter;
            this.messagePriority = messagePriority;
            this.queuePath = queuePath;
            this.recoverable = recoverable;
            this.timeToBeReceived = timeToBeReceived;
            this.timeToReachQueue = timeToReachQueue;
            this.transactionType = transactionType;
            this.useAuthentication = useAuthentication;
            this.useDeadLetterQueue = useDeadLetterQueue;
            this.useEncryption = useEncryption;
        }
        [ManagementConfiguration]
        public string Formatter
        {
            get { return formatter; }
            set { formatter = value; }
        }
        [ManagementConfiguration]
        public string MessagePriority
        {
            get { return messagePriority; }
            set { messagePriority = value; }
        }
        [ManagementConfiguration]
        public string QueuePath
        {
            get { return queuePath; }
            set { queuePath = value; }
        }
        [ManagementConfiguration]
        public bool Recoverable
        {
            get { return recoverable; }
            set { recoverable = value; }
        }
        [ManagementConfiguration]
        public string TimeToBeReceived
        {
            get { return timeToBeReceived; }
            set { timeToBeReceived = value; }
        }
        [ManagementConfiguration]
        public string TimeToReachQueue
        {
            get { return timeToReachQueue; }
            set { timeToReachQueue = value; }
        }
        [ManagementConfiguration]
        public string TransactionType
        {
            get { return transactionType; }
            set { transactionType = value; }
        }
        [ManagementConfiguration]
        public bool UseAuthentication
        {
            get { return useAuthentication; }
            set { useAuthentication = value; }
        }
        [ManagementConfiguration]
        public bool UseDeadLetterQueue
        {
            get { return useDeadLetterQueue; }
            set { useDeadLetterQueue = value; }
        }
        [ManagementConfiguration]
        public bool UseEncryption
        {
            get { return useEncryption; }
            set { useEncryption = value; }
        }
        [ManagementBind]
        public static MsmqTraceListenerSetting BindInstance(string ApplicationName,
                                                            string SectionName,
                                                            string Name)
        {
            return BindInstance<MsmqTraceListenerSetting>(ApplicationName, SectionName, Name);
        }
        [ManagementEnumerator]
        public static IEnumerable<MsmqTraceListenerSetting> GetInstances()
        {
            return GetInstances<MsmqTraceListenerSetting>();
        }
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return MsmqTraceListenerDataWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}
