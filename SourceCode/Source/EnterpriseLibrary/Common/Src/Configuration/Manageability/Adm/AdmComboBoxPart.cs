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
	public class AdmComboBoxPart : AdmEditTextPart
	{
		internal const String ComboBoxTemplate = "\t\t\tCOMBOBOX";
		internal const String SuggestionsStartTemplate = "\t\t\tSUGGESTIONS";
		internal const String SuggestionsEndTemplate = "\t\t\tEND SUGGESTIONS";
		IEnumerable<String> suggestions;
		internal AdmComboBoxPart(String partName, String keyName, String valueName,
			String defaultValue, IEnumerable<String> suggestions, int maxlen, bool required)
			: base(partName, keyName, valueName, defaultValue, maxlen, required)
		{
			this.suggestions = suggestions;
		}
		protected override void WritePart(System.IO.TextWriter writer)
		{
			base.WritePart(writer);
			writer.Write(SuggestionsStartTemplate);
			foreach (String suggestion in this.suggestions)
			{
				writer.Write(String.Format(CultureInfo.InvariantCulture, " \"{0}\"", suggestion));
			}
			writer.Write(writer.NewLine);
			writer.WriteLine(SuggestionsEndTemplate);
		}
		public IEnumerable<String> Suggestions
		{
			get { return suggestions; }
		}
		protected override string PartTypeTemplate
		{
			get
			{
				return ComboBoxTemplate;
			}
		}
	}
}
