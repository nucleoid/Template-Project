using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using AutofacContrib.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;
using SharpArch.Domain.PersistenceSupport;
using SharpArch.NHibernate;
using TemplateProject.Web.Mvc.Autofac;

namespace TemplateProject.Tests
{
    public class ServiceLocatorInitializer
    {
        public static void Init() 
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<EntityDuplicateChecker>().As<IEntityDuplicateChecker>();

            builder.RegisterModelBinders(Assembly.GetAssembly(typeof(ComponentRegistrar)));
            builder.RegisterModelBinderProvider();
            builder.RegisterControllers(Assembly.GetAssembly(typeof(ComponentRegistrar)));
            builder.RegisterModule(new AutofacWebTypesModule());

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
        }
    }
}
