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
namespace Sheng.SailingEase.RuntimeContract
{
    public class ControlValue : IControlValue
    {
        private object _value = null;
        public object Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
            }
        }
        private bool _error = false;
        public bool Error
        {
            get
            {
                return this._error;
            }
            set
            {
                this._error = value;
            }
        }
        private string _errorMsg = String.Empty;
        public string ErrorMsg
        {
            get
            {
                return this._errorMsg;
            }
            set
            {
                this._errorMsg = value;
            }
        }
        public bool IsEmpty
        {
            get
            {
                if (this.Value == null
                            || this.Value == DBNull.Value
                            || this.Value.Equals(String.Empty))
                    return true;
                else
                    return false;
            }
        }
        public override string ToString()
        {
            if (this.Value != null)
            {
                return this.Value.ToString();
            }
            return String.Empty;
        }
        public virtual void ShowError()
        {
        }
    }
}
