using Autofac;
using SharpArch.Domain.PersistenceSupport;
using SharpArch.NHibernate;
using SharpArch.NHibernate.Contracts.Repositories;

namespace TemplateProject.Infrastructure.NHibernateConfig
{
    public class NHibernateModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(NHibernateQuery<>)).As(typeof(IQuery<>));
            builder.RegisterType<EntityDuplicateChecker>().As<IEntityDuplicateChecker>();
            builder.RegisterGeneric(typeof(NHibernateRepository<>)).As(typeof(INHibernateRepository<>));
            builder.RegisterGeneric(typeof(NHibernateRepositoryWithTypedId<,>)).As(typeof(INHibernateRepositoryWithTypedId<,>));
            builder.RegisterType<DefaultSessionFactoryKeyProvider>().As<ISessionFactoryKeyProvider>();
        }
    }
}
