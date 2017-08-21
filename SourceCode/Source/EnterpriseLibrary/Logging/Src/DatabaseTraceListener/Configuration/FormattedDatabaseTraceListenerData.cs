/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;
namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration
{
    [Assembler(typeof(FormattedDatabaseTraceListenerAssembler))]
    [ContainerPolicyCreator(typeof(FormattedDatabaseTraceListenerPolicyCreator))]
    public class FormattedDatabaseTraceListenerData : TraceListenerData
    {
        private const string writeLogStoredProcNameProperty = "writeLogStoredProcName";
        private const string addCategoryStoredProcNameProperty = "addCategoryStoredProcName";
        private const string databaseInstanceNameProperty = "databaseInstanceName";
        private const string formatterNameProperty = "formatter";
        public FormattedDatabaseTraceListenerData()
        {
        }
        public FormattedDatabaseTraceListenerData(string name,
                                                  string writeLogStoredProcName,
                                                  string addCategoryStoredProcName,
                                                  string databaseInstanceName,
                                                  string formatterName)
            : this(
                name,
                writeLogStoredProcName,
                addCategoryStoredProcName,
                databaseInstanceName,
                formatterName,
                TraceOptions.None,
                SourceLevels.All)
        {
        }
        public FormattedDatabaseTraceListenerData(string name,
                                                  string writeLogStoredProcName,
                                                  string addCategoryStoredProcName,
                                                  string databaseInstanceName,
                                                  string formatterName,
                                                  TraceOptions traceOutputOptions,
                                                  SourceLevels filter)
            : base(name, typeof(FormattedDatabaseTraceListener), traceOutputOptions, filter)
        {
            this.DatabaseInstanceName = databaseInstanceName;
            this.WriteLogStoredProcName = writeLogStoredProcName;
            this.AddCategoryStoredProcName = addCategoryStoredProcName;
            this.Formatter = formatterName;
        }
        [ConfigurationProperty(databaseInstanceNameProperty, IsRequired = false)]
        public string DatabaseInstanceName
        {
            get { return (string)base[databaseInstanceNameProperty]; }
            set { base[databaseInstanceNameProperty] = value; }
        }
        [ConfigurationProperty(writeLogStoredProcNameProperty, IsRequired = true)]
        public string WriteLogStoredProcName
        {
            get { return (string)base[writeLogStoredProcNameProperty]; }
            set { base[writeLogStoredProcNameProperty] = value; }
        }
        [ConfigurationProperty(addCategoryStoredProcNameProperty, IsRequired = true)]
        public string AddCategoryStoredProcName
        {
            get { return (string)base[addCategoryStoredProcNameProperty]; }
            set { base[addCategoryStoredProcNameProperty] = value; }
        }
        [ConfigurationProperty(formatterNameProperty, IsRequired = false)]
        public string Formatter
        {
            get { return (string)base[formatterNameProperty]; }
            set { base[formatterNameProperty] = value; }
        }
    }
    public class FormattedDatabaseTraceListenerAssembler : TraceListenerAsssembler
    {
        public override TraceListener Assemble(IBuilderContext context,
                                               TraceListenerData objectConfiguration,
                                               IConfigurationSource configurationSource,
                                               ConfigurationReflectionCache reflectionCache)
        {
            FormattedDatabaseTraceListenerData castedObjectConfiguration
                = (FormattedDatabaseTraceListenerData)objectConfiguration;
            IBuilderContext databaseContext
                = context.CloneForNewBuild(
                    NamedTypeBuildKey.Make<Data.Database>(castedObjectConfiguration.DatabaseInstanceName), null);
            Data.Database database
                = (Data.Database)databaseContext.Strategies.ExecuteBuildUp(databaseContext);
            ILogFormatter formatter
                = GetFormatter(context, castedObjectConfiguration.Formatter, configurationSource, reflectionCache);
            TraceListener createdObject
                = new FormattedDatabaseTraceListener(
                    database,
                    castedObjectConfiguration.WriteLogStoredProcName,
                    castedObjectConfiguration.AddCategoryStoredProcName,
                    formatter);
            return createdObject;
        }
    }
}
