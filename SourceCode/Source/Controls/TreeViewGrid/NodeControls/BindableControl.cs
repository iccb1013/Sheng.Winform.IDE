/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
namespace Sheng.SailingEase.Controls.TreeViewGrid.NodeControls
{
	public abstract class BindableControl: NodeControl
	{
		private string _propertyName = "";
		[DefaultValue("")]
		public string DataPropertyName
		{
			get { return _propertyName; }
			set 
			{
				if (_propertyName == null)
					_propertyName = string.Empty;
				_propertyName = value; 
			}
		}
		public object GetValue(TreeNodeAdv node)
		{
			PropertyInfo pi = GetPropertyInfo(node);
			if (pi != null && pi.CanRead)
				return pi.GetValue(node.Tag, null);
			else
				return null;
		}
		public void SetValue(TreeNodeAdv node, object value)
		{
			PropertyInfo pi = GetPropertyInfo(node);
			if (pi != null && pi.CanWrite)
			{
				try
				{
					pi.SetValue(node.Tag, value, null);
				}
				catch (TargetInvocationException ex)
				{
					if (ex.InnerException != null)
						throw new ArgumentException(ex.InnerException.Message, ex.InnerException);
					else
						throw new ArgumentException(ex.Message);
				}
			}
		}
		public Type GetPropertyType(TreeNodeAdv node)
		{
			if (node.Tag != null && !string.IsNullOrEmpty(DataPropertyName))
			{
				Type type = node.Tag.GetType();
				PropertyInfo pi = type.GetProperty(DataPropertyName);
				if (pi != null)
					return pi.PropertyType;
			}
			return null;
		}
		private PropertyInfo GetPropertyInfo(TreeNodeAdv node)
		{
			if (node.Tag != null && !string.IsNullOrEmpty(DataPropertyName))
			{
				Type type = node.Tag.GetType();
				return type.GetProperty(DataPropertyName);
			}
			return null;
		}
		public override string ToString()
		{
			if (string.IsNullOrEmpty(DataPropertyName))
				return GetType().Name;
			else
				return string.Format("{0} ({1})", GetType().Name, DataPropertyName);
		}
	}
}
