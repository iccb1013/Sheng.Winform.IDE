/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Globalization;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm
{
	public class AdmPolicy
	{
		internal const String PolicyStartTemplate = "\tPOLICY \"{0}\"";
		internal const String KeyNameTemplate = "\t\t\tKEYNAME \"Software\\Policies\\{0}\"";
		internal const String ValueNameTemplate = "\t\t\tVALUENAME \"{0}\" VALUEON NUMERIC 1 VALUEOFF NUMERIC 0";
		internal const String PolicyEndTemplate = "\tEND POLICY";
		private List<AdmPart> parts;
		private String keyName;
		private String name;
		private String valueName;
		internal AdmPolicy(String name, String keyName, String valueName)
		{
			this.keyName = keyName;
			this.name = name;
			this.valueName = valueName;
			this.parts = new List<AdmPart>();
		}
		internal void AddPart(AdmPart part)
		{
			this.parts.Add(part);
		}
		internal void Write(TextWriter writer)
		{
			writer.WriteLine(String.Format(CultureInfo.InvariantCulture, PolicyStartTemplate, name));
			writer.WriteLine(String.Format(CultureInfo.InvariantCulture, KeyNameTemplate, keyName));
			if (valueName != null)
			{
				writer.WriteLine(String.Format(CultureInfo.InvariantCulture, ValueNameTemplate, valueName));
			}
			foreach (AdmPart part in parts)
			{
				part.Write(writer);
			}
			writer.WriteLine(PolicyEndTemplate);
		}
		public String KeyName
		{
			get { return keyName; }
		}
		public IEnumerable<AdmPart> Parts
		{
			get { return parts; }
		}
		public String Name
		{
			get { return name; }
		}
		public String ValueName
		{
			get { return valueName; }
		}
	}
}
