
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MbUnit.Framework;
using Rhino.Mocks;
using SharpArch.Testing;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Tasks;
using TemplateProject.Web.Mvc.Areas.Admin.Controllers;

namespace TemplateProject.Tests.Web.Mvc.Areas.Admin.Controllers
{
    [TestFixture]
    public class ProductsControllerTest
    {
        private IProductTasks _tasks;
        private ProductsController _controller;

        [SetUp]
        public void Setup()
        {
            _tasks = MockRepository.GenerateMock<IProductTasks>();
            _controller = new ProductsController(_tasks);
        }

        [Test]
        public void Index_Forwards_To_Index_With_Products()
        {
            //Arrange
            _tasks.Expect(x => x.GetAll()).Return(new List<Product> { new Product() });

            //Act
            var result = _controller.Index() as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<IList<Product>>(result.Model);
            Assert.AreEqual(1, (result.Model as IList<Product>).Count);
        }

        [Test]
        public void Create_Forwards_To_Create_With_New_Product()
        {
            //Act
            var result = _controller.Create() as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<Product>(result.Model);
            Assert.AreEqual(0, (result.Model as Product).Id);
        }

        [Test]
        public void Edit_Validates_Bad_Model_And_Forwards_To_Create()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();

            //Act
            var result = _controller.Edit(new Product()) as ViewResult;

            //Assert
            Assert.AreEqual("Create", result.ViewName);
            Assert.IsInstanceOfType<Product>(result.Model);
            Assert.AreEqual(0, (result.Model as Product).Id);
        }

        [Test]
        public void Edit_Validates_Bad_Model_And_Forwards_To_Edit()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();
            var product = new Product();
            product.SetIdTo(2);

            //Act
            var result = _controller.Edit(product) as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<Product>(result.Model);
            Assert.AreEqual(2, (result.Model as Product).Id);
        }

        [Test]
        public void Edit_Validates_Good_Model_And_Redirects_To_Index()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();
            _tasks.Expect(x => x.CreateOrUpdate(Arg<Product>.Is.Anything)).Return(new Product());

            //Act
            var result = _controller.Edit(new Product { Name = "blah" }) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            _tasks.VerifyAllExpectations();
        }

        [Test]
        public void Edit_Forwards_To_View_With_Product()
        {
            //Arrange
            var product = new Product();
            product.SetIdTo(3);
            _tasks.Expect(x => x.Get(3)).Return(product);

            //Act
            var result = _controller.Edit(3) as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<Product>(result.Model);
            Assert.AreEqual(3, (result.Model as Product).Id);
            _tasks.VerifyAllExpectations();
        }

        [Test]
        public void Details_Forwards_To_View_With_Product()
        {
            //Arrange
            var product = new Product();
            product.SetIdTo(3);
            _tasks.Expect(x => x.Get(3)).Return(product);

            //Act
            var result = _controller.Details(3) as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<Product>(result.Model);
            Assert.AreEqual(3, (result.Model as Product).Id);
            _tasks.VerifyAllExpectations();
        }

        [Test]
        public void Delete_Deletes_Product_And_Redirects_To_Index()
        {
            //Arrange
            var product = new Product();
            product.SetIdTo(3);
            _tasks.Expect(x => x.Delete(3));

            //Act
            var result = _controller.Delete(3) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            _tasks.VerifyAllExpectations();
        }
    }
}
