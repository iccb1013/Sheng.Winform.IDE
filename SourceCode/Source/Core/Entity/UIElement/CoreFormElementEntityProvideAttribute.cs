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
    [Serializable]
    public class CoreFormElementEntityProvideAttribute : UIElementEntityProvideAttribute
    {
        private bool _nameInitialized = false;
        private string _name;
        public override string Name
        {
            get
            {
                if (_nameInitialized == false)
                {
                    _name = Language.GetString(_name);
                    if (_name == null)
                        _name = String.Empty;
                    _nameInitialized = true;
                }
                return _name;
            }
            set
            {
                _name = value;
                _nameInitialized = false;
            }
        }
        public CoreFormElementEntityProvideAttribute(string name, int code)
            : base(name, code)
        {
        }
    }
}
