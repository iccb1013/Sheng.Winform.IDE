/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using System.Management.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
	[ManagementEntity]
	public abstract partial class ConfigurationSetting
	{
		private string applicationName;
		private string sectionName;
		protected ConfigurationSetting()
		{ }
		protected ConfigurationSetting(ConfigurationElement sourceElement)
		{
			this.SourceElement = sourceElement;
		}
		[ManagementProbe]
		public virtual string ApplicationName
		{
			get { return applicationName; }
			set { applicationName = value; }
		}
		[ManagementProbe]
		public virtual string SectionName
		{
			get { return sectionName; }
			set { sectionName = value; }
		}
		public abstract void Publish();
		public abstract void Revoke();
		[ManagementCommit]
		public void Commit()
		{
			if (this.SourceElement != null
				&& SaveChanges(this.SourceElement)
				&& this.Changed != null)
			{
				this.Changed(this, new EventArgs());
			}
		}
		protected virtual bool SaveChanges(ConfigurationElement sourceElement)
		{
			return false;
		}
		public ConfigurationElement SourceElement { get; set; }
		public event EventHandler<EventArgs> Changed;
	}
}
