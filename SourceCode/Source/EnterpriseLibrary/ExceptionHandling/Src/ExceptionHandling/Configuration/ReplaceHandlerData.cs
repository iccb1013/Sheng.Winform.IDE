/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Unity;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    [Assembler(typeof(ReplaceHandlerAssembler))]
    [ContainerPolicyCreator(typeof(ReplaceHandlerPolicyCreator))]
    public class ReplaceHandlerData : ExceptionHandlerData
    {
        private static AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();
        private const string exceptionMessageProperty = "exceptionMessage";
        private const string replaceExceptionTypeProperty = "replaceExceptionType";
        private const string ExceptionMessageResourceTypeNameProperty = "exceptionMessageResourceType";
        private const string ExceptionMessageResourceNameProperty = "exceptionMessageResourceName";
        public ReplaceHandlerData()
        {
        }
        public ReplaceHandlerData(string name, string exceptionMessage, string replaceExceptionTypeName)
            : base(name, typeof(ReplaceHandler))
        {
            this.ExceptionMessage = exceptionMessage;
            this.ReplaceExceptionTypeName = replaceExceptionTypeName;
        }
        [ConfigurationProperty(exceptionMessageProperty, IsRequired = false)]
        public string ExceptionMessage
        {
            get { return (string)this[exceptionMessageProperty]; }
            set { this[exceptionMessageProperty] = value; }
        }
        [ConfigurationProperty(ExceptionMessageResourceNameProperty)]
        public string ExceptionMessageResourceName
        {
            get { return (string)this[ExceptionMessageResourceNameProperty]; }
            set { this[ExceptionMessageResourceNameProperty] = value; }
        }
        [ConfigurationProperty(ExceptionMessageResourceTypeNameProperty)]
        public string ExceptionMessageResourceType
        {
            get { return (string)this[ExceptionMessageResourceTypeNameProperty]; }
            set { this[ExceptionMessageResourceTypeNameProperty] = value; }
        }
        public Type ReplaceExceptionType
        {
            get { return (Type)typeConverter.ConvertFrom(ReplaceExceptionTypeName); }
            set { ReplaceExceptionTypeName = typeConverter.ConvertToString(value); }
        }
        [ConfigurationProperty(replaceExceptionTypeProperty, IsRequired = true)]
        public string ReplaceExceptionTypeName
        {
            get { return (string)this[replaceExceptionTypeProperty]; }
            set { this[replaceExceptionTypeProperty] = value; }
        }
    }
    public class ReplaceHandlerAssembler : IAssembler<IExceptionHandler, ExceptionHandlerData>
    {
        public IExceptionHandler Assemble(IBuilderContext context, ExceptionHandlerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            ReplaceHandlerData castedObjectConfiguration
                = (ReplaceHandlerData)objectConfiguration;
            IStringResolver exceptionMessageResolver
                = new ResourceStringResolver(
                    castedObjectConfiguration.ExceptionMessageResourceType,
                    castedObjectConfiguration.ExceptionMessageResourceName,
                    castedObjectConfiguration.ExceptionMessage);
            ReplaceHandler createdObject 
                = new ReplaceHandler(exceptionMessageResolver, castedObjectConfiguration.ReplaceExceptionType);
            return createdObject;
        }
    }
}
