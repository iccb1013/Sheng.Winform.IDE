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
namespace Sheng.SailingEase.ComponentModel
{
    [Serializable]
    public class PropertyComboBoxEditorAttribute : PropertyEditorAttribute
    {
        private Type _enum;
        public Type Enum
        {
            get
            {
                return this._enum;
            }
            set
            {
                if (value.IsEnum == false)
                {
                    throw new ArgumentException();
                }
                this._enum = value;
            }
        }
    }
}
