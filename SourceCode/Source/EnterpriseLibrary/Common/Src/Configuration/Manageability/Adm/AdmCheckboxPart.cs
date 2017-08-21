/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm
{
	public class AdmCheckboxPart : AdmPart
	{
		internal const String CheckBoxTemplate = "\t\t\tCHECKBOX";
		internal const String CheckBoxCheckedTemplate = "\t\t\tCHECKBOX DEFCHECKED";
		internal const String DefaultCheckBoxOnTemplate = "\t\t\tVALUEON NUMERIC 1";
		internal const String DefaultCheckBoxOffTemplate = "\t\t\tVALUEOFF NUMERIC 0";
		private bool checkedByDefault;
		private bool valueForOn;
		private bool valueForOff;
		internal AdmCheckboxPart(String partName, String keyName, String valueName,
			bool checkedByDefault, bool valueForOn, bool valueForOff)
			: base(partName, keyName, valueName)
		{
			this.checkedByDefault = checkedByDefault;
			this.valueForOn = valueForOn;
			this.valueForOff = valueForOff;
		}
		protected override void WritePart(System.IO.TextWriter writer)
		{
			base.WritePart(writer);
			if (valueForOn)
			{
				writer.WriteLine(DefaultCheckBoxOnTemplate);
			}
			if (valueForOff)
			{
				writer.WriteLine(DefaultCheckBoxOffTemplate);
			}
		}
		public bool CheckedByDefault
		{
			get { return checkedByDefault; }
		}
		public bool ValueForOn
		{
			get { return valueForOn; }
		}
		public bool ValueForOff
		{
			get { return valueForOff; }
		}
		protected override string PartTypeTemplate
		{
			get
			{
				return checkedByDefault ? CheckBoxCheckedTemplate : CheckBoxTemplate;
			}
		}
	}
}
