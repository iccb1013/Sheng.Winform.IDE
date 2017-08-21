/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
	public class DataInstrumentationListenerBinder : IExplicitInstrumentationBinder
    {
		public void Bind(object source, object listener)
        {
            DataInstrumentationListener castedListener = (DataInstrumentationListener)listener;
            DataInstrumentationProvider castedProvider = (DataInstrumentationProvider)source;
            castedProvider.commandExecuted += castedListener.CommandExecuted;
            castedProvider.commandFailed += castedListener.CommandFailed;
            castedProvider.connectionFailed += castedListener.ConnectionFailed;
            castedProvider.connectionOpened += castedListener.ConnectionOpened;
        }
    }
}
