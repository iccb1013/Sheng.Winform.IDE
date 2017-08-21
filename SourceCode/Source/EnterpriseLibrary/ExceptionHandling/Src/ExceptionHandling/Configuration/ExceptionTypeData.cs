/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
	public class ExceptionTypeData : NamedConfigurationElement
    {
        private static AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();
		private const string typeProperty = "type";
		private const string postHandlingActionProperty = "postHandlingAction";
		private const string exceptionHandlersProperty = "exceptionHandlers";
		public ExceptionTypeData()
		{			
		}
		public ExceptionTypeData(string name, Type type, PostHandlingAction postHandlingAction)
            : this(name, typeConverter.ConvertToString(type), postHandlingAction)
		{					
		}
        public ExceptionTypeData(string name, string typeName, PostHandlingAction postHandlingAction)
            : base(name)
        {
            this.TypeName = typeName;
            this.PostHandlingAction = postHandlingAction;	
        }
        public Type Type
        {
            get { return (Type)typeConverter.ConvertFrom(TypeName); }
            set { TypeName = typeConverter.ConvertToString(value); }
        }
        [ConfigurationProperty(typeProperty, IsRequired = true)]
        public string TypeName
        {
            get { return (string)this[typeProperty]; }
            set { this[typeProperty] = value; }
        }
		[ConfigurationProperty(postHandlingActionProperty, IsRequired= true)]		
		public PostHandlingAction PostHandlingAction
		{
			get { return (PostHandlingAction)this[postHandlingActionProperty]; }
			set { this[postHandlingActionProperty] = value; }
		}
		[ConfigurationProperty(exceptionHandlersProperty)]
        public NameTypeConfigurationElementCollection<ExceptionHandlerData, CustomHandlerData> ExceptionHandlers
		{
			get
			{
                return (NameTypeConfigurationElementCollection<ExceptionHandlerData, CustomHandlerData>)this[exceptionHandlersProperty];
			}
		}
	}
}
