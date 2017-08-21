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
using System.ComponentModel;
namespace Sheng.SailingEase.Controls
{
    class SEControlLicense : License
    {
        private Type _Type;
        public SEControlLicense(Type type)
        {
            if (type == null)
            {
                throw (new NullReferenceException());
            }
            _Type = type;
        }
        public override void Dispose()
        {
        }
        public override string LicenseKey
        {
            get { return (_Type.GUID.ToString()); }
        }
    }
}
