using System.Reflection;
using Autofac;
using Autofac.Integration.Web;
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;
using SharpArch.NHibernate;
using TemplateProject.Infrastructure.NHibernateConfig;

namespace TemplateProject.Infrastructure.Quartz
{
    public class AutofacJobFactory : SimpleJobFactory
    {
        private readonly IContainerProvider _containerProvider;
        private readonly string _connectionString;

        public AutofacJobFactory(IContainerProvider containerProvider, string connectionString)
        {
            _containerProvider = containerProvider;
            _connectionString = connectionString;
        }

        public override IJob NewJob(TriggerFiredBundle bundle)
        {
            NHibernateInitializer.Instance().InitializeNHibernateOnce(InitialiseNHibernateSessions);

            var job = base.NewJob(bundle);
            _containerProvider.ApplicationContainer.InjectUnsetProperties(job);
            return job;
        }

        private void InitialiseNHibernateSessions()
        {
            var config = new NHibernateConfiguration(_connectionString);
            NHibernateSession.Init(new ThreadAndWebSessionStorage(null), new[] { Assembly.GetAssembly(GetType()).GetName().FullName },
                new AutoPersistenceModelGenerator().Generate(), null, null, null, config);
        }
    }
}
