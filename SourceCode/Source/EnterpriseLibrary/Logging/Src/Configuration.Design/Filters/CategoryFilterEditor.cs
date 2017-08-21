/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Sources;
using System.Collections.Generic;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters
{
    internal class CategoryFilterEditor : UITypeEditor
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
                    CategoryFilterEditorUI dialog = new CategoryFilterEditorUI();
                    CategoryFilterNode currentSettings = (CategoryFilterNode)context.Instance;
                    InitializeDialog(dialog, currentSettings);
                    if (DialogResult.OK == service.ShowDialog(dialog))
                    {
                        CategoryFilterMode categoryFilterMode = dialog.FilterMode;
                        NamedElementCollection<CategoryFilterEntry> categoryFilters = new NamedElementCollection<CategoryFilterEntry>();
                        foreach (string category in dialog.SelectedCategoryNames)
                        {
                            CategoryFilterEntry categoryFilter = new CategoryFilterEntry();
                            categoryFilter.Name = category;
                            categoryFilters.Add(categoryFilter);
                        }
                        return new CategoryFilterSettings(categoryFilterMode, categoryFilters);
                    }
                }
            }
            return value;
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        private static void InitializeDialog(CategoryFilterEditorUI dialog, CategoryFilterNode node)
        {
            dialog.FilterMode = node.CategoryFilterExpression.CategoryFilterMode;
            SetCategoryFilterSettings(dialog, node);
            SetAvailableCategoryNames(dialog, node);
        }
        private static void SetCategoryFilterSettings(CategoryFilterEditorUI dialog, CategoryFilterNode node)
        {
            ArrayList selectedCategoryNames = new ArrayList();
            foreach (CategoryFilterEntry categoryFilter in node.CategoryFilterExpression.CategoryFilters)
            {
                selectedCategoryNames.Add(categoryFilter.Name);
            }
            dialog.SetSelectedCategoryNames(selectedCategoryNames);
        }
        private static void SetAvailableCategoryNames(CategoryFilterEditorUI dialog, ConfigurationNode node)
        {
            List<ConfigurationNode> nodes = new List<ConfigurationNode>(node.Hierarchy.FindNodesByType(typeof(CategoryTraceSourceNode)));
            IList<string> categoryNames = new List<string>();
            if (nodes != null)
            {
                ConfigurationNode[] sortedNodes = nodes.ToArray();
                Array.Sort(sortedNodes);
                foreach (ConfigurationNode categoryNode in sortedNodes)
                {
                    categoryNames.Add(categoryNode.Name);
                }
            }
            dialog.UpdateAvailableCategoryNames(categoryNames);
        }
    }
}
