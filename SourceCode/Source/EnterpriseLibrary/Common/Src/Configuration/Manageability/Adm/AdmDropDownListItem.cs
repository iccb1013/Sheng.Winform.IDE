/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm
{
	public struct AdmDropDownListItem
	{
		private String name;
		private String value;
		public AdmDropDownListItem(String name, String value)
		{
			this.name = name;
			this.value = value;
		}
		public String Name
		{
			get { return name; }
		}
		public String Value
		{
			get { return this.value; }
		}
	}
}
