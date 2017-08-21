/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
	[Image(typeof(ExceptionHandlingSettingsNode), "SettingsNode")]
    [SelectedImage(typeof(ExceptionHandlingSettingsNode), "SettingsNode")]
    public sealed class ExceptionHandlingSettingsNode : ConfigurationSectionNode
	{
		public ExceptionHandlingSettingsNode()
			: base(Resources.DefaultExceptionHandlingSettingsNodeName)
		{
		}
		[ReadOnly(true)]
		public override string Name
		{
			get
			{
				return base.Name;
			}
		}
	}
}
