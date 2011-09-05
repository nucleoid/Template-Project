
using Autofac;
using MbUnit.Framework;
using SharpArch.Domain.Commands;
using SharpArch.Domain.PersistenceSupport;
using SharpArch.NHibernate;
using SharpArch.NHibernate.Contracts.Repositories;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Tasks;
using TemplateProject.Infrastructure.Queries;
using TemplateProject.Tasks.Commands;
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
            Assert.IsTrue(container.IsRegistered<IProductTasks>());
            Assert.IsTrue(container.IsRegistered<ICategoryTasks>());
            Assert.IsTrue(container.IsRegistered(typeof(IProductsQuery)));
            Assert.IsTrue(container.IsRegistered<IEntityDuplicateChecker>());
            Assert.IsTrue(container.IsRegistered(typeof(INHibernateRepository<Product>)));
            //            Assert.IsTrue(container.IsRegistered(typeof(INHibernateRepositoryWithTypedId<,>))); //none yet
            Assert.IsTrue(container.IsRegistered<ISessionFactoryKeyProvider>());
            Assert.IsTrue(container.IsRegistered<ICommandProcessor>());
            Assert.IsTrue(container.IsRegistered<ICommandHandler<MassCategoryChangeCommand>>());
        }
    }
}
