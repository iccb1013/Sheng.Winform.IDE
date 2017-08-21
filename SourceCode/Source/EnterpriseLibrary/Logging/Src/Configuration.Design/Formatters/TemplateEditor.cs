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
using System.Windows.Forms;
using System.Windows.Forms.Design;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters
{
    internal class TemplateEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            Debug.Assert(provider != null, "No service provider; we cannot edit the value");
            if (provider != null)
            {
                IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                Debug.Assert(edSvc != null, "No editor service; we cannot edit the value");
                if (edSvc != null)
                {
                    IWindowsFormsEditorService service = (IWindowsFormsEditorService) provider.GetService(typeof(IWindowsFormsEditorService));
                    TemplateEditorUI dialog = new TemplateEditorUI();
                    string originalText = ((Template)value).Text;
                    dialog.UserText = originalText;
                    DialogResult dialogResult = service.ShowDialog(dialog);
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return new Template(originalText);
                    }
                    return new Template(dialog.UserText);
                }
            }
            return value;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }
}
