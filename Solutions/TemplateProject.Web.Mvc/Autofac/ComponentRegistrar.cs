
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using MvpRestApiLib;
using SharpArch.Domain.Commands;
using SharpArch.NHibernate;
using TemplateProject.Infrastructure.NHibernateConfig;
using TemplateProject.Infrastructure.Queries;
using TemplateProject.Tasks;
using TemplateProject.Tasks.CommandHandlers;
using TemplateProject.Tasks.CustomContracts;
using TemplateProject.Web.Mvc.Wrappers;

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
            builder.RegisterModule<NHibernateModule>();
            builder.RegisterType<CommandProcessor>().As<ICommandProcessor>();
        }

        private static void AddCustomRepositoriesTo(ContainerBuilder builder)
        {
            builder.RegisterType<FormsAuthWrapper>().As<IAuthenticationTasks>();
            builder.RegisterType<MembershipWrapper>().As<IMembershipTasks>();
            builder.RegisterType<ReCaptchaWrapper>().As<ICaptchaTasks>();
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
            builder.RegisterType<XmlValueProviderFactory>().As<ValueProviderFactory>();
            builder.RegisterModelBinders(assembly);
            builder.RegisterModelBinderProvider();
            builder.RegisterControllers(assembly);
            builder.RegisterModule(new AutofacWebTypesModule());
        }
    }
}