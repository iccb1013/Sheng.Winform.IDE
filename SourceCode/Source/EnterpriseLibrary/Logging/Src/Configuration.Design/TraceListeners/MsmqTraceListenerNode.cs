/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Messaging;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Diagnostics;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners
{
    public class MsmqTraceListenerNode : TraceListenerNode
    {
		private FormatterNode formatterNode;
		private bool useEncryption;
		private bool useDeadLetterQueue;
		private bool useAuthentication;
		private MessageQueueTransactionType transactionType;
		private MessagePriority messagePriority;
		private TimeSpan timeToReachQueue;
		private TimeSpan timeToBeReceived;
		private bool recoverable;
		private string queuePath;
		private string formatterName;
        public MsmqTraceListenerNode()
			: this(new MsmqTraceListenerData(Resources.MsmqTraceListenerNode, DefaultValues.MsmqQueuePath, string.Empty))
        {            
        }
        public MsmqTraceListenerNode(MsmqTraceListenerData msmqTraceListenerData)
        {
			if (null == msmqTraceListenerData) throw new ArgumentNullException("msmqTraceListenerData");
			Rename(msmqTraceListenerData.Name);
            Filter = msmqTraceListenerData.Filter;
			TraceOutputOptions = msmqTraceListenerData.TraceOutputOptions;
			this.useEncryption = msmqTraceListenerData.UseEncryption;
			this.useDeadLetterQueue = msmqTraceListenerData.UseDeadLetterQueue;
			this.useAuthentication = msmqTraceListenerData.UseAuthentication;
			this.transactionType = msmqTraceListenerData.TransactionType;
			this.messagePriority = msmqTraceListenerData.MessagePriority;
			this.timeToReachQueue = msmqTraceListenerData.TimeToReachQueue;
			this.timeToBeReceived = msmqTraceListenerData.TimeToBeReceived;
			this.recoverable = msmqTraceListenerData.Recoverable;
			this.queuePath = msmqTraceListenerData.QueuePath;
			this.formatterName = msmqTraceListenerData.Formatter;
        }
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("QueuePathDescription", typeof(Resources))]
        [Required]
        public string QueuePath
        {
            get { return queuePath; }
            set { queuePath = value; }
        }
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("RecoverableDescription", typeof(Resources))]
        public bool Recoverable
        {
            get { return recoverable; }
            set { recoverable = value; }
        }
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("TimeToBeReceivedDescription", typeof(Resources))]
        public TimeSpan TimeToBeReceived
        {
            get { return timeToBeReceived; }
            set { timeToBeReceived = value; }
        }
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("TimeToReachQueueDescription", typeof(Resources))]
        public TimeSpan TimeToReachQueue
        {
            get { return timeToReachQueue; }
            set { timeToReachQueue = value; }
        }
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("MessagePriorityDescription", typeof(Resources))]
        public MessagePriority MessagePriority
        {
            get { return messagePriority; }
            set { messagePriority = value; }
        }
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("TransactionTypeDescription", typeof(Resources))]
        public MessageQueueTransactionType TransactionType
        {
            get { return transactionType; }
            set { transactionType = value; }
        }
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("UseAuthenticationDescription", typeof(Resources))]
        public bool UseAuthentication
        {
            get { return useAuthentication; }
            set { useAuthentication = value; }
        }
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("UseDeadLetterQueueDescription", typeof(Resources))]
        public bool UseDeadLetterQueue
        {
            get { return useDeadLetterQueue; }
            set { useDeadLetterQueue = value; }
        }
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("UseEncryptionDescription", typeof(Resources))]
        public bool UseEncryption
        {
            get { return useEncryption; }
            set { useEncryption = value; }
        }
        [Required]
		[Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(FormatterNode))]
        [SRDescription("FormatDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public FormatterNode Formatter
        {
            get { return formatterNode; }
            set
            {
                formatterNode = LinkNodeHelper.CreateReference<FormatterNode>(formatterNode,
                    value,
                    OnFormatterNodeRemoved,
                    OnFormatterNodeRenamed);
                formatterName = formatterNode == null ? String.Empty : formatterNode.Name;
            }
        }
		public override TraceListenerData TraceListenerData
		{
			get 
			{
				MsmqTraceListenerData data = new MsmqTraceListenerData(Name, queuePath, formatterName, messagePriority,
					recoverable, timeToReachQueue, timeToBeReceived, useAuthentication,
					useDeadLetterQueue, useEncryption, transactionType);
				data.TraceOutputOptions = TraceOutputOptions;
                data.Filter = Filter;
				return data;
			}
		}
		protected override void SetFormatterReference(ConfigurationNode formatterNodeReference)
		{
			if (formatterNodeReference.Name == formatterName) Formatter = (FormatterNode)formatterNodeReference;
		}
		private void OnFormatterNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.formatterNode = null;
        }
        private void OnFormatterNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.formatterName = e.Node.Name;
        }
    }
}
