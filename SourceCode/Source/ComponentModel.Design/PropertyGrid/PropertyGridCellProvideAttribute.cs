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
namespace Sheng.SailingEase.ComponentModel.Design
{
    public class PropertyGridCellProvideAttribute : Attribute
    {
        private Type _editorAttribute;
        public Type EditorAttribut
        {
            get { return _editorAttribute; }
        }
        public PropertyGridCellProvideAttribute(Type editorAttribute)
        {
            _editorAttribute = editorAttribute;
        }
    }
}
