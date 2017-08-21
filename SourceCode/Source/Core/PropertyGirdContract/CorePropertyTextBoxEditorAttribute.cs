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
using Sheng.SailingEase.ComponentModel;
using Sheng.SailingEase.Core.Localisation;
namespace Sheng.SailingEase.Core
{
    class CorePropertyTextBoxEditorAttribute : PropertyTextBoxEditorAttribute
    {
        private bool _regexMsgInitialized = false;
        private string _regexMsg;
        public override string RegexMsg
        {
            get
            {
                if (_regexMsgInitialized == false)
                {
                    _regexMsg = Language.GetString(_regexMsg);
                    if (_regexMsg == null)
                        _regexMsg = String.Empty;
                    _regexMsgInitialized = true;
                }
                return _regexMsg;
            }
            set
            {
                _regexMsg = value;
                _regexMsgInitialized = false;
            }
        }
    }
}
