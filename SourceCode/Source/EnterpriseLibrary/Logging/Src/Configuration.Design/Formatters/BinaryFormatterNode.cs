/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters
{
    public sealed class BinaryFormatterNode : FormatterNode
    {
        public BinaryFormatterNode()
            : this(new BinaryLogFormatterData(Resources.BinaryFormatterNode))
        {
        }
        public BinaryFormatterNode(BinaryLogFormatterData binaryLogFormatter)
            : base()
        {
			if (null == binaryLogFormatter) throw new ArgumentNullException("binaryLogFormatter");
			Rename(binaryLogFormatter.Name);
        }
		public override FormatterData FormatterData
		{
			get { return new BinaryLogFormatterData(Name); }
		}
    }
}
