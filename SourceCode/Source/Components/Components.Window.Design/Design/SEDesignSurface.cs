/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel.Design.Serialization;
namespace Sheng.SailingEase.Components.Window.DesignComponent
{
	class SEDesignSurface : DesignSurface
	{
		private BasicDesignerLoader _loader;
		public BasicDesignerLoader Loader
		{
			get
			{
				return _loader;
			}
			set
			{
				_loader = value;
			}
		}
		public IDesignerHost Host
		{
			get
			{
				return (IDesignerHost)GetService(typeof(IDesignerHost));
			}
		}
		public SEDesignSurface() : base()
		{
		}
		public SEDesignSurface(IServiceProvider parentProvider)
			: base(parentProvider)
		{
		}
		public IServiceContainer GetServiceContainer()
		{
			return this.GetService(typeof(IServiceContainer)) as IServiceContainer;
		}
		public void DoAction(CommandID commandId)
		{
			IMenuCommandService ims = this.GetService(typeof(IMenuCommandService)) as IMenuCommandService;
			try
			{
				ims.GlobalInvoke(commandId);
			}
			catch (Exception ex)
			{
				throw new Exception("::DoAction() - Exception: error in performing the action: " + commandId + "(see Inner Exception)", ex);
			}
		}
		public IComponent CreateRootComponent(Type controlType, Size controlSize)
		{
			try
			{
				IDesignerHost host = this.Host;
				if (null == host) return null;
				if (null != host.RootComponent) return null;
				this.BeginLoad(controlType);
				if (this.LoadErrors.Count > 0)
					throw new Exception("the BeginLoad() failed! Some error during " + controlType.ToString() + " loding");
				IDesignerHost ihost = this.Host;
				Control ctrl = null;
				Type hostType = host.RootComponent.GetType().BaseType;
				if (hostType == typeof(Form))
				{
					ctrl = this.View as Control;
					ctrl.BackColor = Color.LightGray;
					PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(ctrl);
					PropertyDescriptor pdS = pdc.Find("Size", false);
					if (null != pdS)
						pdS.SetValue(ihost.RootComponent, controlSize);
				}
				else if (hostType == typeof(UserControl))
				{
					ctrl = this.View as Control;
					ctrl.BackColor = Color.DarkGray;
					PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(ctrl);
					PropertyDescriptor pdS = pdc.Find("Size", false);
					if (null != pdS)
						pdS.SetValue(ihost.RootComponent, controlSize);
				}
				else if (hostType == typeof(Component))
				{
					ctrl = this.View as Control;
					ctrl.BackColor = Color.White;
				}
				else
				{
					throw new Exception("Undefined Host Type: " + hostType.ToString());
				}
				return ihost.RootComponent;
			}
			catch (Exception ex)
			{
				throw new Exception("::CreateRootComponent() - Exception: (see Inner Exception)", ex);
			}
		}
		public Control CreateControl(Type controlType, Size controlSize, Point controlLocation)
		{
			try
			{
				IDesignerHost host = this.Host;
				if (null == host) return null;
				if (null == host.RootComponent) return null;
				IComponent newComp = host.CreateComponent(controlType);
				if (null == newComp) return null;
				IDesigner designer = host.GetDesigner(newComp);
				if (null == designer) return null;
				if (designer is IComponentInitializer)
					((IComponentInitializer)designer).InitializeNewComponent(null);
				PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(newComp);
				PropertyDescriptor pdS = pdc.Find("Size", false);
				if (null != pdS)
					pdS.SetValue(newComp, controlSize);
				PropertyDescriptor pdL = pdc.Find("Location", false);
				if (null != pdL)
					pdL.SetValue(newComp, controlLocation);
				((Control)newComp).Parent = host.RootComponent as Control;
				return newComp as Control;
			}
			catch (Exception ex)
			{
				throw new Exception("::CreateControl() - Exception: (see Inner Exception)", ex);
			}
		}
		public void DestroyComponent(IComponent component)
		{
			try
			{
				IDesignerHost host = this.Host;
				if (null == host) return;
				if (null == host.RootComponent) return;
				host.DestroyComponent(component);
			}
			catch (Exception ex)
			{
				throw new Exception("::DestroyComponent() - Exception: (see Inner Exception)", ex);
			}
		}
		public Control GetView()
		{
			Control ctrl = this.View as Control;
			ctrl.Dock = DockStyle.Fill;
			return ctrl;
		}
	}
}
