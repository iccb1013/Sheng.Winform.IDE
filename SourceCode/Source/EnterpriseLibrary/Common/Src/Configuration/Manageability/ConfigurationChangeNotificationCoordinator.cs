/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    public class ConfigurationChangeNotificationCoordinator
    {
        readonly EventHandlerList eventHandlers = new EventHandlerList();
        readonly object eventHandlersLock = new object();
        public void AddSectionChangeHandler(string sectionName,
                                            ConfigurationChangedEventHandler handler)
        {
            lock (eventHandlersLock)
            {
                eventHandlers.AddHandler(CanonicalizeSectionName(sectionName), handler);
            }
        }
        static string CanonicalizeSectionName(string sectionName)
        {
            return String.Intern(sectionName);
        }
        public void NotifyUpdatedSections(IEnumerable<string> sectionsToNotify)
        {
            foreach (string rawSectionName in sectionsToNotify)
            {
                String sectionName = CanonicalizeSectionName(rawSectionName);
                Delegate[] invocationList = null;
                lock (eventHandlersLock)
                {
                    ConfigurationChangedEventHandler callbacks = (ConfigurationChangedEventHandler)eventHandlers[sectionName];
                    if (callbacks == null)
                    {
                        continue;
                    }
                    invocationList = callbacks.GetInvocationList();
                }
                ConfigurationChangedEventArgs eventData = new ConfigurationChangedEventArgs(sectionName);
                foreach (ConfigurationChangedEventHandler callback in invocationList)
                {
                    try
                    {
                        if (callback != null)
                        {
                            callback(this, eventData);
                        }
                    }
                    catch (Exception e)
                    {
                        ManageabilityExtensionsLogger.LogException(e,
                                                                   String.Format(Resources.Culture,
                                                                                 Resources.ExceptionErrorOnCallbackForSectionUpdate,
                                                                                 sectionName,
                                                                                 callback.ToString()));
                    }
                }
            }
        }
        public void RemoveSectionChangeHandler(string sectionName,
                                               ConfigurationChangedEventHandler handler)
        {
            lock (eventHandlersLock)
            {
                eventHandlers.RemoveHandler(CanonicalizeSectionName(sectionName), handler);
            }
        }
    }
}
