/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.Configuration.Install;
using System.Reflection;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
	public class ReflectionInstaller<TInstallerBuilder> : Installer
		where TInstallerBuilder : AbstractInstallerBuilder
	{
		public override void Install(IDictionary stateSaver)
		{
			PrepareInstaller();
			base.Install(stateSaver);
		}
		public override void Uninstall(IDictionary stateSaver)
		{
			PrepareInstaller();
			base.Uninstall(stateSaver);
		}
		private void PrepareInstaller()
		{
			string assemblyName = this.Context.Parameters["assemblypath"];
			Type[] types = Assembly.LoadFile(assemblyName).GetTypes();
			TInstallerBuilder builder = (TInstallerBuilder)Activator.CreateInstance(typeof(TInstallerBuilder), new object[] { types });
			builder.Fill(this);
		}
	}
}
