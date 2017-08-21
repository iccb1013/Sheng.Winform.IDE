/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm
{
    public class AdmTextPart : AdmPart
    {
        internal const String TextTemplate = "\t\t\tTEXT";
        public AdmTextPart(String partName)
            : base(partName, null, null) {}
        protected override string PartTypeTemplate
        {
            get { return TextTemplate; }
        }
    }
}
