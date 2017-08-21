/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Management.Instrumentation;
using System.Reflection;
using System.ServiceProcess;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Threading;
using System.Xml;
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor
{
    [DesignerCategory("Code")]
    [RunInstaller(true)]
	public class ProjectInstaller : DefaultManagementProjectInstaller
    {
        private ServiceProcessInstaller serviceProcessInstaller;
        private ServiceInstaller serviceInstaller;
        private const string ConfigurationFileName = "MsmqDistributor.exe.config";
        private string serviceName = string.Empty;
        private string serviceDependency = "Message Queuing";
        public ProjectInstaller()
        {
            SetName();
            InitializeComponent();
            this.serviceProcessInstaller.Account = ServiceAccount.User;
            this.serviceProcessInstaller.Username = null;
            this.serviceProcessInstaller.Password = null;
            InstallEventSource(this.serviceName, Resources.ApplicationLogName);
        }
        private void InstallEventSource(string sourceName, string logName)
        {
            EventLogInstaller defaultLogDestinationSinkNameInstaller = new EventLogInstaller();
            defaultLogDestinationSinkNameInstaller.Source = sourceName;
            defaultLogDestinationSinkNameInstaller.Log = logName;
            Installers.Add(defaultLogDestinationSinkNameInstaller);
        }
        private void SetName()
        {
            this.serviceName = DistributorService.DefaultApplicationName;
            string configurationFilepath = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, "exe.config");
            try
            {
                XmlDocument configurationDoc = new XmlDocument();
                configurationDoc.Load(configurationFilepath);
                XmlNode serviceNameNode = configurationDoc.SelectSingleNode("/configuration/msmqDistributorSettings/@serviceName");
                if (serviceNameNode != null && !string.IsNullOrEmpty(serviceNameNode.Value))
                {
                    this.serviceName = serviceNameNode.Value;
                }
            }
            catch(Exception ex)
            {
                throw new LoggingException(Resources.InstallerCannotReadServiceName, ex);
            }
        }
        private void InitializeComponent()
        {
            string[] dependencyArray = new string[] { this.serviceDependency };
            this.serviceProcessInstaller = new ServiceProcessInstaller();
            this.serviceInstaller = new ServiceInstaller();
            this.serviceInstaller.ServiceName = this.serviceName;
            this.serviceInstaller.ServicesDependedOn = dependencyArray;
            this.Installers.AddRange(new Installer[]
                {
                    this.serviceProcessInstaller,
                    this.serviceInstaller
                });
        }
    }
}
