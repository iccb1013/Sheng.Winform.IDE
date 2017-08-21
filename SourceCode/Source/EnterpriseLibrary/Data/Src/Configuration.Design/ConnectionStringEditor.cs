/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.Windows.Forms;
using Microsoft.Data.ConnectionUI;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Windows.Forms.Design;
using System.IO;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
	class ConnectionStringEditor : UITypeEditor
	{
		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (null == context) return value;
			if (null == provider) return value;
			object newValue;
			if (this.AttemptToEditValueWithCustomDialog(context, provider, value, out newValue))
			{
				return newValue;
			}
			else
			{
				return new MultilineStringEditor().EditValue(context, provider, value);
			}
		}
		public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
		private bool AttemptToEditValueWithCustomDialog(ITypeDescriptorContext context, IServiceProvider provider, object value, out object newValue)
		{
			string providerName = GetProviderName(context.Instance);
			newValue = null;
			if (string.IsNullOrEmpty(providerName))
			{
				return false;
			}
			try
			{
				return DoEditValueWithCcustomDialog(provider, providerName, value, out newValue);
			}
			catch (FileNotFoundException)
			{
				return false;
			}
		}
		private bool DoEditValueWithCcustomDialog(IServiceProvider provider, string providerName, object value, out object newValue)
		{
			newValue = value;
			DataSource dataSource;
			DataProvider dataProvider;
			if (!GetKnownDataSourceAndProviderForProviderName(providerName, out dataSource, out dataProvider))
			{
				return false;
			}
			DataConnectionDialog dlg = new DataConnectionDialog();
			dlg.DataSources.Add(dataSource);
			dlg.SelectedDataSource = dataSource;
			dlg.SelectedDataProvider = dataProvider;
			dlg.ConnectionString = value as string;
			System.Windows.Forms.Design.IUIService uisvc
				= (System.Windows.Forms.Design.IUIService)provider.GetService(typeof(System.Windows.Forms.Design.IUIService));
			IWin32Window owner = uisvc != null ? uisvc.GetDialogOwnerWindow() : null;
			if (DataConnectionDialog.Show(dlg, owner) == DialogResult.OK)
			{
				newValue = dlg.ConnectionString;
			}
			return true;
		}
		private bool GetKnownDataSourceAndProviderForProviderName(string providerName,
			out DataSource dataSource,
            out DataProvider dataProvider)
		{
			dataSource = null;
			dataProvider = null;
			switch (providerName)
			{
				case "System.Data.SqlClient":
					dataProvider = DataProvider.SqlDataProvider;
					break;
				case "System.Data.OracleClient":
					dataProvider = DataProvider.OracleDataProvider;
					break;
				case "System.Data.Odbc":
					dataProvider = DataProvider.OdbcDataProvider;
					break;
				case "System.Data.OleDb":
					dataProvider = DataProvider.OleDBDataProvider;
					break;
			}
			if (dataProvider != null)
			{
				dataSource = new DataSource(dataProvider.Name, dataProvider.DisplayName);
				dataSource.Providers.Add(dataProvider);
				return true;
			}
			else
			{
				return false;
			}
		}
		private string GetProviderName(object context)
		{
			return ((IDatabaseProviderName)context).DatabaseProviderName;
		}
	}
}
