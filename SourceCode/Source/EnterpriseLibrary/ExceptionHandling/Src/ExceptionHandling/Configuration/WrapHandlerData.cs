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
    [Assembler(typeof(WrapHandlerAssembler))]
    [ContainerPolicyCreator(typeof(WrapHandlerPolicyCreator))]
    public class WrapHandlerData : ExceptionHandlerData
    {
        private static AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();
        private const string exceptionMessageProperty = "exceptionMessage";
        private const string wrapExceptionTypeProperty = "wrapExceptionType";
        private const string ExceptionMessageResourceTypeNameProperty = "exceptionMessageResourceType";
        private const string ExceptionMessageResourceNameProperty = "exceptionMessageResourceName";
        public WrapHandlerData()
        {
        }
        public WrapHandlerData(string name, string exceptionMessage, string wrapExceptionTypeName)
            : base(name, typeof(WrapHandler))
        {
            this.ExceptionMessage = exceptionMessage;
            this.WrapExceptionTypeName = wrapExceptionTypeName;
        }
        [ConfigurationProperty(exceptionMessageProperty, IsRequired = false)]
        public string ExceptionMessage
        {
            get { return (string)this[exceptionMessageProperty]; }
            set { this[exceptionMessageProperty] = value; }
        }
        [ConfigurationProperty(ExceptionMessageResourceTypeNameProperty)]
        public string ExceptionMessageResourceType
        {
            get { return (string)this[ExceptionMessageResourceTypeNameProperty]; }
            set { this[ExceptionMessageResourceTypeNameProperty] = value; }
        }
        [ConfigurationProperty(ExceptionMessageResourceNameProperty)]
        public string ExceptionMessageResourceName
        {
            get { return (string)this[ExceptionMessageResourceNameProperty]; }
            set { this[ExceptionMessageResourceNameProperty] = value; }
        }
        public Type WrapExceptionType
        {
            get { return (Type)typeConverter.ConvertFrom(WrapExceptionTypeName); }
            set { WrapExceptionTypeName = typeConverter.ConvertToString(value); }
        }
        [ConfigurationProperty(wrapExceptionTypeProperty, IsRequired = true)]
        public string WrapExceptionTypeName
        {
            get { return (string)this[wrapExceptionTypeProperty]; }
            set { this[wrapExceptionTypeProperty] = value; }
        }
    }
    public class WrapHandlerAssembler : IAssembler<IExceptionHandler, ExceptionHandlerData>
    {
        public IExceptionHandler Assemble(IBuilderContext context, ExceptionHandlerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            WrapHandlerData castedObjectConfiguration
                = (WrapHandlerData)objectConfiguration;
            IStringResolver exceptionMessageResolver
                = new ResourceStringResolver(
                    castedObjectConfiguration.ExceptionMessageResourceType,
                    castedObjectConfiguration.ExceptionMessageResourceName,
                    castedObjectConfiguration.ExceptionMessage);
            WrapHandler createdObject = new WrapHandler(exceptionMessageResolver, castedObjectConfiguration.WrapExceptionType);
            return createdObject;
        }
    }
}
