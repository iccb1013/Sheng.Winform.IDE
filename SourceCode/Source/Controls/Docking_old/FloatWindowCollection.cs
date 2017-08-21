/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
namespace Sheng.SIMBE.SEControl.Docking
{
	public class FloatWindowCollection : ReadOnlyCollection<FloatWindow>
	{
		internal FloatWindowCollection()
            : base(new List<FloatWindow>())
		{
		}
		internal int Add(FloatWindow fw)
		{
			if (Items.Contains(fw))
				return Items.IndexOf(fw);
			Items.Add(fw);
            return Count - 1;
		}
		internal void Dispose()
		{
			for (int i=Count - 1; i>=0; i--)
				this[i].Close();
		}
		internal void Remove(FloatWindow fw)
		{
			Items.Remove(fw);
		}
		internal void BringWindowToFront(FloatWindow fw)
		{
			Items.Remove(fw);
			Items.Add(fw);
		}
	}
}
