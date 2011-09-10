
using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using SharpArch.Domain.Commands;
using SharpArch.Domain.PersistenceSupport;
using SharpArch.NHibernate;
using SharpArch.NHibernate.Contracts.Repositories;
using TemplateProject.Infrastructure.Queries;
using TemplateProject.Tasks;
using TemplateProject.Tasks.CommandHandlers;

namespace TemplateProject.Web.Mvc.Autofac
{
    public class ComponentRegistrar
    {
        public static void AddComponentsTo(ContainerBuilder builder, Assembly executingAssembly)
        {
            AddGenericRepositoriesTo(builder);
            AddCustomRepositoriesTo(builder);
            AddQueryObjectsTo(builder);
            AddTasksTo(builder);
            AddCommandsTo(builder);
            AddMVCObjects(builder, executingAssembly);
        }

        private static void AddGenericRepositoriesTo(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(NHibernateQuery<>)).As(typeof(IQuery<>));
            builder.RegisterType<EntityDuplicateChecker>().As<IEntityDuplicateChecker>();
            builder.RegisterGeneric(typeof(NHibernateRepository<>)).As(typeof(INHibernateRepository<>));
            builder.RegisterGeneric(typeof(NHibernateRepositoryWithTypedId<,>)).As(typeof(INHibernateRepositoryWithTypedId<,>));
            builder.RegisterType<DefaultSessionFactoryKeyProvider>().As<ISessionFactoryKeyProvider>();
            builder.RegisterType<CommandProcessor>().As<ICommandProcessor>();
        }

        private static void AddCustomRepositoriesTo(ContainerBuilder builder)
        {
            //none yet
        }

        private static void AddQueryObjectsTo(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ProductsQuery))).AssignableTo<NHibernateQuery>().
                AsImplementedInterfaces();
        }

        private static void AddTasksTo(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ProductTasks))).Where(x => x.Name.EndsWith("Tasks")).AsImplementedInterfaces();
        }

        private static void AddCommandsTo(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(MassCategoryChangeHandler))).InNamespaceOf<MassCategoryChangeHandler>()
                .AsImplementedInterfaces();
        }

        private static void AddMVCObjects(ContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterModelBinders(assembly);
            builder.RegisterModelBinderProvider();
            builder.RegisterControllers(assembly);
            builder.RegisterModule(new AutofacWebTypesModule());
        }
    }
}