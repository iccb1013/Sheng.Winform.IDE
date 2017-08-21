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
using Microsoft.Win32;
namespace Sheng.SailingEase.Controls
{
    class SEControlLicenseProvider : LicenseProvider
    {
        public bool IsValid
        {
            get
            {
                return true;
            }
        }
        public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
        {
            if (context.UsageMode == LicenseUsageMode.Runtime)
            {
                return new SEControlLicense(type);
            }
            else if (context.UsageMode == LicenseUsageMode.Designtime)
            {
                if (!IsValid)
                    throw new LicenseException(type);
                else
                    return new SEControlLicense(type);
            }
            return (null);
        }
    }
}
