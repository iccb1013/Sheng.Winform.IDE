/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using System.Diagnostics;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners
{
    public class EmailTraceListenerNode: TraceListenerNode
    {
        private string formatterName;
        private FormatterNode formatterNode;
		private string toAddress;
		private string fromAddress;
		private string subjectLineStarter;
		private string subjectLineEnder;
		private string smtpServer;
		private int smtpPort;
        public EmailTraceListenerNode()
            :this(new EmailTraceListenerData(Resources.EmailTraceListenerNode, DefaultValues.EmailListenerToAddress, DefaultValues.EmailListenerFromAddress, string.Empty, string.Empty, DefaultValues.EmailListenerSmtpAddress, DefaultValues.EmailListenerSmtpPort, string.Empty))
        {
        }
        public EmailTraceListenerNode(EmailTraceListenerData emailTraceListenerData)
        {
			if (null == emailTraceListenerData) throw new ArgumentNullException("emailTraceListenerData");
			Rename(emailTraceListenerData.Name);
			TraceOutputOptions = emailTraceListenerData.TraceOutputOptions;
            Filter = emailTraceListenerData.Filter;
            this.formatterName = emailTraceListenerData.Formatter;
			this.toAddress = emailTraceListenerData.ToAddress;
			this.fromAddress = emailTraceListenerData.FromAddress;
			this.subjectLineStarter = emailTraceListenerData.SubjectLineStarter;
			this.subjectLineEnder = emailTraceListenerData.SubjectLineEnder;
			this.smtpServer = emailTraceListenerData.SmtpServer;
			this.smtpPort = emailTraceListenerData.SmtpPort;
        }
        [Required]
        [SRDescription("EmailSinkToAddressDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string ToAddress
        {
            get { return toAddress; }
            set { toAddress = value; }
        }
        [Required]
        [SRDescription("EmailTraceListenerFromAddressDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string FromAddress
        {
            get { return fromAddress; }
            set { fromAddress = value; }
        }
        [SRDescription("EmailSinkSubjectLineStarterDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string SubjectLineStarter
        {
            get { return subjectLineStarter; }
            set { subjectLineStarter = value; }
        }
        [SRDescription("EmailTraceListenerSubjectLineEnderDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string SubjectLineEnder
        {
            get { return subjectLineEnder; }
            set { subjectLineEnder = value; }
        }
        [Required]
        [SRDescription("EmailTraceListenerSmtpServerDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string SmtpServer
        {
            get { return smtpServer; }
            set { smtpServer = value; }
        }
        [Required]
        [SRDescription("EmailTraceListenerSmtpPortDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public int SmtpPort
        {
            get { return smtpPort; }
            set { smtpPort = value; }
        }
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
				EmailTraceListenerData data = new EmailTraceListenerData(Name, toAddress, fromAddress, subjectLineStarter, 
					subjectLineEnder, smtpServer, smtpPort, formatterName);
				data.TraceOutputOptions = TraceOutputOptions;
				return data;
			}
		}
		protected override void SetFormatterReference(ConfigurationNode formatterNodeReference)
		{
			if (formatterName == formatterNodeReference.Name) Formatter = (FormatterNode)formatterNodeReference;
		}		
        private void OnFormatterNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            formatterNode = null;
        }
        private void OnFormatterNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
            formatterName = e.Node.Name;
        }
    }
}
