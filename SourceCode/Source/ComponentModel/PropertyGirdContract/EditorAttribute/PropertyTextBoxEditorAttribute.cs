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
    public class PropertyTextBoxEditorAttribute : PropertyEditorAttribute
    {
        private TypeCode _typeCode = TypeCode.String;
        public TypeCode TypeCode
        {
            get
            {
                return this._typeCode;
            }
            set
            {
                this._typeCode = value;
            }
        }
        public string Regex
        {
            get;
            set;
        }
        public virtual string RegexMsg
        {
            get;
            set;
        }
        private bool _allowEmpty = true;
        public bool AllowEmpty
        {
            get
            {
                return this._allowEmpty;
            }
            set
            {
                this._allowEmpty = value;
            }
        }
        private int _maxLength = 32767;
        public int MaxLength
        {
            get
            {
                return this._maxLength;
            }
            set
            {
                this._maxLength = value;
            }
        }
    }
}
