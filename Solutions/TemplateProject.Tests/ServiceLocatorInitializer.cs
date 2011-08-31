using Autofac;
using AutofacContrib.CommonServiceLocator;
using Microsoft.Practices.ServiceLocation;
using SharpArch.Domain.PersistenceSupport;
using SharpArch.NHibernate;

namespace TemplateProject.Tests
{
    public class ServiceLocatorInitializer
    {
        public static void Init() 
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<EntityDuplicateChecker>().As<IEntityDuplicateChecker>();

            var container = builder.Build();
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
        }
    }
}
