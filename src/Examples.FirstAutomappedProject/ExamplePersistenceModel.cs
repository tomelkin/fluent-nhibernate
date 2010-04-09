using System.IO;
using FluentNHibernate;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Examples.FirstAutomappedProject
{
    public class ExamplePersistenceModel : PersistenceModel
    {
        public ExamplePersistenceModel()
        {
            AutoMap.ThisAssembly()
                .UsingConfiguration<ExampleAutomappingConfiguration>();

            Database(SQLiteConfiguration.Standard.UsingFile(DbFile));
            PostConfigure(BuildSchema);
        }

        void BuildSchema(Configuration cfg)
        {
            // delete the existing db on each run
            if (File.Exists(DbFile))
                File.Delete(DbFile);

            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(cfg)
                .Create(false, true);
        }

        const string DbFile = "firstProgram.db";
    }
}