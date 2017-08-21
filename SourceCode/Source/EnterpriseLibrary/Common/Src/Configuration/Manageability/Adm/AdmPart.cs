/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm
{
	public abstract class AdmPart
	{
		public const String PartStartTemplate = "\t\tPART \"{0}\"";
        public const String PartEndTemplate = "\t\tEND PART";
        public const String ValueNameTemplate = "\t\t\tVALUENAME \"{0}\"";
        public const String KeyNameTemplate = "\t\t\tKEYNAME \"Software\\Policies\\{0}\"";
		private String keyName;
		private String partName;
		private String valueName;
		protected AdmPart(String partName, String keyName, String valueName)
		{
			this.keyName = keyName;
			this.partName = partName;
			this.valueName = valueName;
		}
		public void Write(TextWriter writer)
		{
			writer.WriteLine(String.Format(CultureInfo.InvariantCulture, PartStartTemplate, partName));
			WritePart(writer);
			writer.WriteLine(PartEndTemplate);
		}
		protected virtual void WritePart(TextWriter writer)
		{
			writer.WriteLine(PartTypeTemplate);
			if (valueName != null)
			{
				writer.WriteLine(String.Format(CultureInfo.InvariantCulture, ValueNameTemplate, valueName));
			}
			if (keyName != null)
			{
				writer.WriteLine(String.Format(CultureInfo.InvariantCulture, KeyNameTemplate, keyName));
			}
		}
		public String KeyName
		{
			get { return keyName; }
		}
		public String PartName
		{
			get { return partName; }
		}
		public String ValueName
		{
			get { return valueName; }
		}
		protected abstract String PartTypeTemplate
		{
			get;
		}
	}
}
