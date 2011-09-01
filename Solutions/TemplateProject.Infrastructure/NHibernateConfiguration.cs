using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;


namespace TemplateProject.Infrastructure
{
    public class NHibernateConfiguration : IPersistenceConfigurer
    {
        public NHibernateConfiguration(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }

        public Configuration ConfigureProperties(Configuration nhibernateConfig)
        {
            var config = Fluently.Configure(nhibernateConfig)
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(ConnectionString))
                .ExposeConfiguration(c => c.SetProperty(NHibernate.Cfg.Environment.ConnectionProvider, "NHibernate.Connection.DriverConnectionProvider"))
                .ExposeConfiguration(c => c.SetProperty(NHibernate.Cfg.Environment.ConnectionDriver, "NHibernate.Driver.SqlClientDriver"))
                .ExposeConfiguration(c => c.SetProperty(NHibernate.Cfg.Environment.ShowSql, "true"))
                .ExposeConfiguration(c => c.SetProperty(NHibernate.Cfg.Environment.ReleaseConnections, "auto"))
                .ExposeConfiguration(c => c.SetProperty(NHibernate.Cfg.Environment.Hbm2ddlAuto, "create"))
                .ProxyFactoryFactory<NHibernate.ByteCode.Castle.ProxyFactoryFactory>()
                .BuildConfiguration();

            if (config == null)
                throw new Exception("Cannot build NHibernate configuration");
            return config;
        }
    }
}
