
using Autofac;
using MbUnit.Framework;
using SharpArch.Domain.PersistenceSupport;
using SharpArch.NHibernate;
using SharpArch.NHibernate.Contracts.Repositories;
using TemplateProject.Domain;
using TemplateProject.Tasks;
using TemplateProject.Web.Mvc.Autofac;

namespace TemplateProject.Tests.Web.Mvc.Autofac
{
    [TestFixture]
    public class ComponentRegistrarTest
    {
        [Test]
        public void AddComponentsTo_Adds_All_Components()
        {
            //Arrange
            var builder = new ContainerBuilder();

            //Act
            ComponentRegistrar.AddComponentsTo(builder);
            var container = builder.Build();

            //Assert
            Assert.IsNotNull(container.Resolve<ProductTasks>());
//            Assert.IsNotNull(container.Resolve(typeof(IQuery<>))); //none yet
            Assert.IsNotNull(container.Resolve<IEntityDuplicateChecker>());
            Assert.IsNotNull(container.Resolve(typeof(INHibernateRepository<Product>)));
//            Assert.IsNotNull(container.Resolve(typeof(INHibernateRepositoryWithTypedId<,>))); //none yet
            Assert.IsNotNull(container.Resolve<ISessionFactoryKeyProvider>());
            Assert.IsNotNull(container.Resolve<SharpArch.Domain.Commands.ICommandProcessor>());
        }
    }
}
