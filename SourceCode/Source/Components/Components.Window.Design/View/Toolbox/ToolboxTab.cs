/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    public class ToolboxTab
    {
        private string m_name = null;
        private ToolboxItemCollection m_toolboxItemCollection = null;
        public ToolboxTab()
        {
        }
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                m_name = value;
            }
        }
        public ToolboxItemCollection ToolboxItems
        {
            get
            {
                return m_toolboxItemCollection;
            }
            set
            {
                m_toolboxItemCollection = value;
            }
        }
    }
}
