
using System.Web;
using System.Web.Mvc;
using Autofac;
using MbUnit.Framework;
using SharpArch.Domain.Commands;
using SharpArch.Domain.PersistenceSupport;
using SharpArch.NHibernate;
using SharpArch.NHibernate.Contracts.Repositories;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Queries;
using TemplateProject.Domain.Contracts.Tasks;
using TemplateProject.Infrastructure.Queries;
using TemplateProject.Tasks.Commands;
using TemplateProject.Tasks.CustomContracts;
using TemplateProject.Web.Mvc.Autofac;
using TemplateProject.Web.Mvc.Controllers;

namespace TemplateProject.Tests.Web.Mvc.Autofac
{
    [TestFixture]
    public class ComponentRegistrarTest
    {
        private IContainer _container;

        [FixtureSetUp]
        public void FixtureSetup()
        {
            //Arrange
            var builder = new ContainerBuilder();
            var assembly = typeof(HomeController).Assembly;

            //Act
            ComponentRegistrar.AddComponentsTo(builder, assembly);
            _container = builder.Build();
        }

        [Test]
        public void AddComponentsTo_Adds_Generic_Repositories()
        {
            //Assert
            Assert.IsTrue(_container.IsRegistered<IEntityDuplicateChecker>());
            Assert.IsTrue(_container.IsRegistered(typeof(INHibernateRepository<Product>)));
            Assert.IsTrue(_container.IsRegistered<ISessionFactoryKeyProvider>());
            Assert.IsTrue(_container.IsRegistered<ICommandProcessor>());
        }

        [Test]
        public void AddComponentsTo_Adds_Custom_Repositories()
        {
            Assert.IsTrue(_container.IsRegistered<IAuthenticationTasks>());
            Assert.IsTrue(_container.IsRegistered<IMembershipTasks>());
            Assert.IsTrue(_container.IsRegistered<ICaptchaTasks>());
        }

        [Test]
        public void AddComponentsTo_Adds_Query_Objects()
        {
            Assert.IsTrue(_container.IsRegistered(typeof(IProductsQuery)));
            Assert.IsTrue(_container.IsRegistered(typeof(ICategoriesQuery)));
        }

        [Test]
        public void AddComponentsTo_Adds_Tasks()
        {
            //Assert
            Assert.IsTrue(_container.IsRegistered<IProductTasks>());
            Assert.IsTrue(_container.IsRegistered<ICategoryTasks>());
        }

        [Test]
        public void AddComponentsTo_Adds_Commands()
        {
            //Assert
            Assert.IsTrue(_container.IsRegistered<ICommandHandler<MassCategoryChangeCommand>>());
        }

        [Test]
        public void AddComponentsTo_Adds_MVC_Objects()
        {
            //Assert
            Assert.IsTrue(_container.IsRegistered<IModelBinder>());
            Assert.IsTrue(_container.IsRegistered<IModelBinderProvider>());
            Assert.IsTrue(_container.IsRegistered<HomeController>());
            Assert.IsTrue(_container.IsRegistered<HttpContextBase>());
            Assert.IsTrue(_container.IsRegistered<ValueProviderFactory>());
        }
    }
}
