/*********************************************        
作者：曹旭升              
QQ：279060597
访问博客了解详细介绍及更多内容：   
http://blog.shengxunwei.com
**********************************************/
using System;
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design
{
    [Image(typeof(ProviderMappingNode))]
	public sealed class ProviderMappingNode : ConfigurationNode, IDatabaseProviderName
    {
        private string databaseTypeName;
        public ProviderMappingNode() : this(new DbProviderMapping("System.Data.SqlClient", typeof(SqlDatabase)))
        {
        }
		public ProviderMappingNode(DbProviderMapping dbProviderMapping)
			: base(dbProviderMapping == null ? "System.Data.SqlClient" : dbProviderMapping.Name)
        {
			if (dbProviderMapping == null)
            {
				throw new ArgumentNullException("dbProviderMapping");
            }
            this.databaseTypeName = dbProviderMapping.DatabaseTypeName;
        }
		[Editor(typeof(ProviderEditor), typeof(UITypeEditor))]
		public override string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				base.Name = value;
			}
		}
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(Database))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("DatabaseTypeFullyQualifedNameDescription", typeof(Resources))]
        public string TypeName
        {
            get { return databaseTypeName; }
            set { databaseTypeName = value; }
        }
        [Browsable(false)]
        public DbProviderMapping ProviderMapping
        {
            get { return new DbProviderMapping(Name, databaseTypeName); }
        }
		[Browsable(false)]
		string IDatabaseProviderName.DatabaseProviderName
		{
			get { return Name; }			
		}
    }
}
