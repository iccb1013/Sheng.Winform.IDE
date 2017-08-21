/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design.Properties;
namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design
{
    public class FaultContractExceptionHandlerNode : ExceptionHandlerNode
    {
        string exceptionMessage;
        string faultContractType;
        List<FaultContractPropertyMapping> propertyMappings = new List<FaultContractPropertyMapping>();
        public FaultContractExceptionHandlerNode()
            : this(new FaultContractExceptionHandlerData(Resources.FaultContractExceptionHandlerNodeName)) {}
        public FaultContractExceptionHandlerNode(FaultContractExceptionHandlerData data)
        {
            if (null == data) throw new ArgumentNullException("data");
            Rename(data.Name);
            faultContractType = data.FaultContractType;
            exceptionMessage = data.ExceptionMessage;
            foreach (FaultContractExceptionHandlerMappingData mappingData in data.PropertyMappings)
            {
                FaultContractPropertyMapping mapping = new FaultContractPropertyMapping();
                mapping.Name = mappingData.Name;
                mapping.Source = mappingData.Source;
                PropertyMappings.Add(mapping);
            }
        }
        [Browsable(false)]
        public override ExceptionHandlerData ExceptionHandlerData
        {
            get
            {
                FaultContractExceptionHandlerData data = new FaultContractExceptionHandlerData(Name, faultContractType);
                data.ExceptionMessage = exceptionMessage;
                foreach (FaultContractPropertyMapping mapping in propertyMappings)
                {
                    data.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData(mapping.Name, mapping.Source));
                }
                return data;
            }
        }
        [SRDescription("ExceptionMessageDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
            set { exceptionMessage = value; }
        }
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(object))]
        [SRDescription("FaultContractTypeDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string FaultContractType
        {
            get { return faultContractType; }
            set { faultContractType = value; }
        }
        [SRDescription("PropertyMappingsMessageDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [PropertyMappingsValidation]
        public List<FaultContractPropertyMapping> PropertyMappings
        {
            get { return propertyMappings; }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) {}
            base.Dispose(disposing);
        }
        class PropertyMappingsValidationAttribute : ValidationAttribute
        {
            protected override void ValidateCore(object instance,
                                                 PropertyInfo propertyInfo,
                                                 IList<ValidationError> errors)
            {
                List<FaultContractPropertyMapping> mappings = propertyInfo.GetValue(instance, new object[0]) as List<FaultContractPropertyMapping>;
                if (mappings != null)
                {
                    int position = -1;
                    List<string> keys = new List<string>();
                    foreach (FaultContractPropertyMapping item in mappings)
                    {
                        position++;
                        if (string.IsNullOrEmpty(item.Name))
                        {
                            string errorMessage = string.Format(Resources.Culture, Resources.PropertyMappingNameNullError, position);
                            errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, errorMessage));
                            continue;
                        }
                        if (keys.Contains(item.Name))
                        {
                            string errorMessage = string.Format(Resources.Culture, Resources.PropertyMappingDuplicateNameError, item.Name);
                            errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, errorMessage));
                            continue;
                        }
                        keys.Add(item.Name);
                    }
                }
            }
        }
    }
}
