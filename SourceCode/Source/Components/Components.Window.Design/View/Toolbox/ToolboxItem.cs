/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
    public class ToolboxItem
    {
        private string m_name = null;
        private Type m_type = null;
        public ToolboxItem()
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
        public Type Type
        {
            get
            {
                return m_type;
            }
            set
            {
                m_type = value;
            }
        }
    }
}
