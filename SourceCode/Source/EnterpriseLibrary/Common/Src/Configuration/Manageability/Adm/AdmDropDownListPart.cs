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
	public class AdmDropDownListPart : AdmPart
	{
		public const String DropDownListTemplate = "\t\t\tDROPDOWNLIST";
        public const String ItemListStartTemplate = "\t\t\tITEMLIST";
        public const String ListItemTemplate = "\t\t\t\tNAME \"{0}\" VALUE \"{1}\"";
        public const String DefaultListItemTemplate = "\t\t\t\tNAME \"{0}\" VALUE \"{1}\" DEFAULT";
        public const String ItemListEndTemplate = "\t\t\tEND ITEMLIST";
		private IEnumerable<AdmDropDownListItem> items;
		private String defaultValue;
		public AdmDropDownListPart(String partName, String keyName, String valueName,
			IEnumerable<AdmDropDownListItem> items, String defaultValue)
			: base(partName, keyName, valueName)
		{
			this.items = items;
			this.defaultValue = defaultValue;
		}
		protected override void WritePart(System.IO.TextWriter writer)
		{
			base.WritePart(writer);
			writer.WriteLine(ItemListStartTemplate);
			foreach (AdmDropDownListItem item in items)
			{
				if (item.Name.Equals(defaultValue))
				{
					writer.WriteLine(String.Format(CultureInfo.InvariantCulture, DefaultListItemTemplate, item.Name, item.Value));
				}
				else
				{
					writer.WriteLine(String.Format(CultureInfo.InvariantCulture, ListItemTemplate, item.Name, item.Value));
				}
			}
			writer.WriteLine(ItemListEndTemplate);
		}
		public String DefaultValue
		{
			get { return defaultValue; }
		}
		public IEnumerable<AdmDropDownListItem> Items
		{
			get { return items; }
		}
		protected override string PartTypeTemplate
		{
			get { return DropDownListTemplate; }
		}
	}
}
