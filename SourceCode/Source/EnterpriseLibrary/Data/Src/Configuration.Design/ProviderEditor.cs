/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{	
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, Name="FullTrust")]
    [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name="FullTrust")]
    internal class ProviderEditor : UITypeEditor
    {
       public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
			if (null == context) return value;
			if (null == provider) return value;
            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));	
			if (null == service) return null;
			string currentProvider = ((IDatabaseProviderName)context.Instance).DatabaseProviderName;
			ProviderEditorUI control = new ProviderEditorUI(service, currentProvider);
			service.DropDownControl(control);
            if (control.SelectedProvider != null)
            {
				return control.SelectedProvider;
            }
			return value; 
        }
       public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }		
    }
}
