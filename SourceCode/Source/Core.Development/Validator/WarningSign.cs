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
namespace Sheng.SailingEase.Core.Development
{
    public class WarningSign
    {
        private IWarningable _host;
        public bool ExistWarning
        {
            get { return _warnings.Count > 0; }
        }
        public string Name
        {
            get { return this._host.WarningSignName; }
        }
        private string _message = String.Empty;
        public string Message
        {
            get { return _message; }
            private set { _message = value; }
        }
        private WarningSignCollection _warnings = new WarningSignCollection();
        public WarningSignCollection Warnings
        {
            get { return _warnings; }
            set { _warnings = value; }
        }
        public int WarningCount
        {
            get
            {
                if (_warnings.Count == 0)
                    return 0;
                int count = 0;
                foreach (WarningSign sign in _warnings)
                {
                    if (String.IsNullOrEmpty(sign.Message) == false)
                    {
                        count++;
                    }
                    count += sign.WarningCount;
                }
                return count;
            }
        }
        public WarningSign(IWarningable  host)
        {
            _host = host;
        }
        public void Clear()
        {
            _message = String.Empty;
            if (_warnings != null && _warnings.Count > 0)
            {
                _warnings.Clear();
            }
        }
        public void AddWarningSign(WarningSign warning)
        {
            _warnings.Add(warning);
        }
        public void AddWarningSign(WarningSignCollection warnings)
        {
            _warnings.AddRange(warnings);
        }
        public void AddWarningSign(IWarningable target, string message)
        {
            WarningSign warningSign = new WarningSign(target)
            {
                Message = message
            };
            _warnings.Add(warningSign);
        }
    }
}
