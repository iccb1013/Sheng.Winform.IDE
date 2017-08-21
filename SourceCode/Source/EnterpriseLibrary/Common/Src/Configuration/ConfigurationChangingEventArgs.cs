/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    [Serializable]
    public class ConfigurationChangingEventArgs : ConfigurationChangedEventArgs
    {
        private bool cancel;
        private readonly object newValue;
        private readonly object oldValue;
        public ConfigurationChangingEventArgs(string sectionName, object oldValue, object newValue) : base(sectionName)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }
        public object OldValue
        {
            get { return oldValue; }
        }
        public object NewValue
        {
            get { return newValue; }
        }
        public bool Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }
    }
    public delegate void ConfigurationChangingEventHandler(object sender, ConfigurationChangingEventArgs e);
}
