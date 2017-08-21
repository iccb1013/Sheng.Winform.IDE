/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
namespace Sheng.SailingEase.Controls.Docking
{
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class LocalizedDescriptionAttribute : DescriptionAttribute
	{
		private bool m_initialized = false;
		public LocalizedDescriptionAttribute(string key) : base(key)
		{
		}
		public override string Description
		{
			get
			{	
				if (!m_initialized)
				{
					string key = base.Description;
					DescriptionValue = ResourceHelper.GetString(key);
					if (DescriptionValue == null)
						DescriptionValue = String.Empty;
					m_initialized = true;
				}
				return DescriptionValue;
			}
		}
	}
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class LocalizedCategoryAttribute : CategoryAttribute
	{
		public LocalizedCategoryAttribute(string key) : base(key)
		{
		}
		protected override string GetLocalizedString(string key)
		{
			return ResourceHelper.GetString(key);
		}
	}
}
