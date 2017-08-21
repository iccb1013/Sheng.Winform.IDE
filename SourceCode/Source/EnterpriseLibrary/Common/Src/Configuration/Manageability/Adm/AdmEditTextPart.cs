/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm
{
	public class AdmEditTextPart : AdmPart
	{
		internal const String EditTextTemplate = "\t\t\tEDITTEXT";
		internal const String DefaultValueTemplate = "\t\t\tDEFAULT \"{0}\"";
		internal const String MaxLengthTemplate = "\t\t\tMAXLEN {0}";
		internal const String RequiredTemplate = "\t\t\tREQUIRED";
		private String defaultValue;
		private int maxlen;
		private bool required;
		protected internal AdmEditTextPart(String partName, String keyName, String valueName,
			String defaultValue, int maxlen, bool required)
			: base(partName, keyName, valueName)
		{
			this.defaultValue = defaultValue;
			this.maxlen = maxlen;
			this.required = required;
		}
		protected override void WritePart(System.IO.TextWriter writer)
		{
			base.WritePart(writer);
			if (maxlen > 0)
				writer.WriteLine(String.Format(CultureInfo.InvariantCulture, MaxLengthTemplate, maxlen));
			if (required)
				writer.WriteLine(RequiredTemplate);
			if (defaultValue != null)
			{
				writer.WriteLine(String.Format(CultureInfo.InvariantCulture, DefaultValueTemplate, this.defaultValue));
			}
		}
		public String DefaultValue
		{
			get { return defaultValue; }
		}
		public int Maxlen
		{
			get { return this.maxlen; }
		}
		public bool Required
		{
			get { return this.required; }
		}
		protected override String PartTypeTemplate
		{
			get { return EditTextTemplate; }
		}
	}
}
