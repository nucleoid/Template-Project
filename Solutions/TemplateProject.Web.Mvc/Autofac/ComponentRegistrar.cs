
using System.Reflection;
using Autofac;
using SharpArch.Domain.PersistenceSupport;
using SharpArch.NHibernate;
using SharpArch.NHibernate.Contracts.Repositories;
using TemplateProject.Domain.Contracts.Tasks;
using TemplateProject.Tasks;

namespace TemplateProject.Web.Mvc.Autofac
{
    public class ComponentRegistrar
    {
        public static void AddComponentsTo(ContainerBuilder builder)
        {
            AddGenericRepositoriesTo(builder);
            AddCustomRepositoriesTo(builder);
            AddQueryObjectsTo(builder);
            AddTasksTo(builder);
            AddCommandsTo(builder);
        }

        private static void AddTasksTo(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ProductTasks))).AsImplementedInterfaces();
        }

        private static void AddCustomRepositoriesTo(ContainerBuilder builder)
        {
            //none yet
        }

        private static void AddGenericRepositoriesTo(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(NHibernateQuery<>)).As(typeof(IQuery<>));
            builder.RegisterType<EntityDuplicateChecker>().As<IEntityDuplicateChecker>();
            builder.RegisterGeneric(typeof(NHibernateRepository<>)).As(typeof(INHibernateRepository<>));
            builder.RegisterGeneric(typeof(NHibernateRepositoryWithTypedId<,>)).As(typeof(INHibernateRepositoryWithTypedId<,>));
            builder.RegisterType<DefaultSessionFactoryKeyProvider>().As<ISessionFactoryKeyProvider>();
            builder.RegisterType<SharpArch.Domain.Commands.CommandProcessor>().As<SharpArch.Domain.Commands.ICommandProcessor>();

        }

        private static void AddQueryObjectsTo(ContainerBuilder builder)
        {
            //none yet
        }

        private static void AddCommandsTo(ContainerBuilder builder)
        {
            //none yet
        }
    }
}