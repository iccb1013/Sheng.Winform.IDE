/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    public class FileConfigurationSourceElement : ConfigurationSourceElement
    {
        private const string filePathProperty = "filePath";
        public FileConfigurationSourceElement() : this(Resources.FileConfigurationSourceName, string.Empty)
        {
        }
        public FileConfigurationSourceElement(string name, string filePath)
            : base(name, typeof(FileConfigurationSource))
		{
            this.FilePath = filePath;
        }
        [ConfigurationProperty(filePathProperty, IsRequired = true)]
        public string FilePath
        {
            get { return (string)this[filePathProperty]; }
            set { this[filePathProperty] = value; }
        }
		public override IConfigurationSource CreateSource()
		{
			IConfigurationSource createdObject = new FileConfigurationSource(FilePath);
			return createdObject;
		}
    }
}
