
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MbUnit.Framework;
using MvcContrib.Pagination;
using Rhino.Mocks;
using SharpArch.Domain.Commands;
using SharpArch.Testing;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Tasks;
using TemplateProject.Infrastructure.Queries;
using TemplateProject.Tasks.Commands;
using TemplateProject.Web.Mvc.Areas.Admin.Controllers;
using TemplateProject.Web.Mvc.Areas.Admin.Models;

namespace TemplateProject.Tests.Web.Mvc.Areas.Admin.Controllers
{
    [TestFixture]
    public class ProductsControllerTest
    {
        private IProductTasks _productTasks;
        private ICategoryTasks _categoryTasks;
        private IProductsQuery _productsQuery;
        private ICommandProcessor _commandProcessor;
        private ProductsController _controller;

        [SetUp]
        public void Setup()
        {
            _productTasks = MockRepository.GenerateMock<IProductTasks>();
            _categoryTasks = MockRepository.GenerateMock<ICategoryTasks>();
            _productsQuery = MockRepository.GenerateMock<IProductsQuery>();
            _commandProcessor = MockRepository.GenerateMock<ICommandProcessor>();
            _controller = new ProductsController(_productTasks, _categoryTasks, _productsQuery, _commandProcessor);
        }

        [Test]
        public void Index_Forwards_To_Index_Without_Page()
        {
            //Arrange
            _productsQuery.Expect(x => x.GetPagedList(1, 10)).Return(new CustomPagination<Product>(new List<Product> { new Product() }, 1, 10, 1));
            _categoryTasks.Expect(x => x.GetAll()).Return(new List<Category> { new Category() });

            //Act
            var result = _controller.Index(null) as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<CustomPagination<Product>>(result.Model);
            Assert.AreEqual(1, (result.Model as CustomPagination<Product>).PageNumber);
            Assert.AreEqual(1, (result.Model as CustomPagination<Product>).TotalItems);
            Assert.AreEqual(1, (result.ViewBag.Categories as Dictionary<int, string>).Count);
            _productsQuery.VerifyAllExpectations();
            _categoryTasks.VerifyAllExpectations();
        }

        [Test]
        public void Index_Forwards_To_Index_With_Page()
        {
            //Arrange
            _productsQuery.Expect(x => x.GetPagedList(2, 10)).Return(new CustomPagination<Product>(new List<Product> { new Product() }, 2, 10, 1));
            _categoryTasks.Expect(x => x.GetAll()).Return(new List<Category> { new Category() });

            //Act
            var result = _controller.Index(2) as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<CustomPagination<Product>>(result.Model);
            Assert.AreEqual(2, (result.Model as CustomPagination<Product>).PageNumber);
            Assert.AreEqual(1, (result.Model as CustomPagination<Product>).TotalItems);
            Assert.AreEqual(1, (result.ViewBag.Categories as Dictionary<int, string>).Count);
            _productsQuery.VerifyAllExpectations();
            _categoryTasks.VerifyAllExpectations();
        }

        [Test]
        public void Create_Forwards_To_Create_With_New_Product()
        {
            //Arrange
            _categoryTasks.Expect(x => x.GetAll()).Return(new List<Category> {new Category()});

            //Act
            var result = _controller.Create() as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<ProductEditViewModel>(result.Model);
            Assert.AreEqual(0, (result.Model as ProductEditViewModel).Product.Id);
            Assert.AreEqual(1, (result.Model as ProductEditViewModel).Categories.Count);
            Assert.AreEqual(0, (result.Model as ProductEditViewModel).SelectedCategoryId);
            _categoryTasks.VerifyAllExpectations();
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
            _categoryTasks.Expect(x => x.GetAll()).Return(new List<Category> {new Category()});

            //Act
            var result = _controller.Edit(new ProductEditViewModel {Product = new Product {Category = new Category()}}) as ViewResult;

            //Assert
            Assert.AreEqual("Create", result.ViewName);
            Assert.IsInstanceOfType<ProductEditViewModel>(result.Model);
            Assert.AreEqual(0, (result.Model as ProductEditViewModel).Product.Id);
            Assert.AreEqual(1, (result.Model as ProductEditViewModel).Categories.Count);
            _categoryTasks.VerifyAllExpectations();
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
            var product = new Product {Category = new Category()};
            product.SetIdTo(2);
            _categoryTasks.Expect(x => x.GetAll()).Return(new List<Category> { new Category() });

            //Act
            var result = _controller.Edit(new ProductEditViewModel {Product = product}) as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<ProductEditViewModel>(result.Model);
            Assert.AreEqual(2, (result.Model as ProductEditViewModel).Product.Id);
            Assert.AreEqual(1, (result.Model as ProductEditViewModel).Categories.Count);
            _categoryTasks.VerifyAllExpectations();
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
            _productTasks.Expect(x => x.CreateOrUpdate(Arg<Product>.Is.Anything)).Return(new Product());
            var product = new Product {Name = "blah", Category = new Category()};

            //Act
            var result = _controller.Edit(new ProductEditViewModel {Product = product}) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            _productTasks.VerifyAllExpectations();
        }

        [Test]
        public void Edit_Forwards_To_View_With_Product()
        {
            //Arrange
            var product = new Product {Category = new Category()};
            product.SetIdTo(3);
            _productTasks.Expect(x => x.Get(3)).Return(product);
            _categoryTasks.Expect(x => x.GetAll()).Return(new List<Category> {new Category()});

            //Act
            var result = _controller.Edit(3) as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<ProductEditViewModel>(result.Model);
            Assert.AreEqual(3, (result.Model as ProductEditViewModel).Product.Id);
            Assert.AreEqual(1, (result.Model as ProductEditViewModel).Categories.Count);
            _productTasks.VerifyAllExpectations();
        }

        [Test]
        public void Delete_Deletes_Product_And_Redirects_To_Index()
        {
            //Arrange
            var product = new Product();
            product.SetIdTo(3);
            _productTasks.Expect(x => x.Delete(3));

            //Act
            var result = _controller.Delete(3) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            _productTasks.VerifyAllExpectations();
        }

        [Test]
        public void ChangeCategory_Changes_Product_Categories()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            var form = new FormCollection {{"category", "3"}, {"1", "true,false"}, {"2", "false"}, {"3", "true,false"}};
            _controller.ValueProvider = form.ToValueProvider();
            var results = new CommandResults();
            results.AddResult(new MassCategoryChangeResult(true));
            _commandProcessor.Expect(x => x.Process(Arg<MassCategoryChangeCommand>.Matches(y =>
                y.CategoryId == 3 && y.ProductIds.Count() == 2))).Return(results);

            //Act
            var result = _controller.ChangeCategory(form) as ContentResult;

            //Assert
            Assert.AreEqual("Categories successfully changed! Refresh to see the changes.", result.Content);
            Assert.AreEqual("text/html", result.ContentType);
            _commandProcessor.VerifyAllExpectations();
        }

        [Test]
        public void ChangeCategory_With_ID_Not_Found()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            var form = new FormCollection { { "category", "3" }, { "1", "true,false" }, { "2", "false" }, { "3", "true,false" } };
            _controller.ValueProvider = form.ToValueProvider();
            var results = new CommandResults();
            results.AddResult(new MassCategoryChangeResult(false));
            _commandProcessor.Expect(x => x.Process(Arg<MassCategoryChangeCommand>.Matches(y =>
                y.CategoryId == 3 && y.ProductIds.Count() == 2))).Return(results);

            //Act
            var result = _controller.ChangeCategory(form) as ContentResult;

            //Assert
            Assert.AreEqual("One or more categories failed to change!", result.Content);
            Assert.AreEqual("text/html", result.ContentType);
            _commandProcessor.VerifyAllExpectations();
        }
    }
}
