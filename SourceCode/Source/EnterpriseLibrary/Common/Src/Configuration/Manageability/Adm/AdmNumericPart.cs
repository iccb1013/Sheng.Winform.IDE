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
	public class AdmNumericPart : AdmPart
	{
		internal const String NumericTemplate = "\t\t\tNUMERIC";
		internal const String DefaultValueTemplate = "\t\t\tDEFAULT {0}";
		internal const String MinValueTemplate = "\t\t\tMIN {0}";
		internal const String MaxValueTemplate = "\t\t\tMAX {0}";
		private int? defaultValue;
		private int? maxValue;
		private int? minValue;
		internal AdmNumericPart(String partName, String keyName, String valueName,
			int? defaultValue, int? minValue, int? maxValue)
			: base(partName, keyName, valueName)
		{
			this.defaultValue = defaultValue;
			this.minValue = minValue;
			this.maxValue = maxValue;
		}
		protected override void WritePart(System.IO.TextWriter writer)
		{
			base.WritePart(writer);
			if (defaultValue != null)
			{
				writer.WriteLine(String.Format(CultureInfo.InvariantCulture, DefaultValueTemplate, this.defaultValue.Value));
			}
			if (minValue != null)
			{
				writer.WriteLine(String.Format(CultureInfo.InvariantCulture, MinValueTemplate, this.minValue.Value));
			}
			if (maxValue != null)
			{
				writer.WriteLine(String.Format(CultureInfo.InvariantCulture, MaxValueTemplate, this.maxValue.Value));
			}
		}
		public int? DefaultValue
		{
			get { return defaultValue; }
		}
		public int? MaxValue
		{
			get { return this.maxValue; }
		}
		public int? MinValue
		{
			get { return this.minValue; }
		}
		protected override string PartTypeTemplate
		{
			get { return NumericTemplate; }
		}
	}
}
