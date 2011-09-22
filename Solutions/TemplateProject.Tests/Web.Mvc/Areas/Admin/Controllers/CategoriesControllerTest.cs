using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MbUnit.Framework;
using MvcContrib.Pagination;
using Rhino.Mocks;
using SharpArch.Testing;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Queries;
using TemplateProject.Domain.Contracts.Tasks;
using TemplateProject.Web.Mvc.Areas.Admin.Controllers;
using TemplateProject.Web.Mvc.Areas.Admin.Models;

namespace TemplateProject.Tests.Web.Mvc.Areas.Admin.Controllers
{
    [TestFixture]
    public class CategoriesControllerTest
    {
        private ICategoryTasks _tasks;
        private ICategoriesQuery _categoriesQuery;
        private CategoriesController _controller;

        [SetUp]
        public void Setup()
        {
            _tasks = MockRepository.GenerateMock<ICategoryTasks>();
            _categoriesQuery = MockRepository.GenerateMock<ICategoriesQuery>();
            _controller = new CategoriesController(_tasks, _categoriesQuery);
        }

        [Test]
        public void Index_Forwards_To_Index_Without_Page()
        {
            //Arrange
            _categoriesQuery.Expect(x => x.GetPagedList(1, 10)).Return(new CustomPagination<Category>(new List<Category> { new Category() }, 1, 10, 1));

            //Act
            var result = _controller.Index(null) as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<CategoriesViewModel>(result.Model);
            Assert.AreEqual(1, (result.Model as CategoriesViewModel).Categories.PageNumber);
            Assert.AreEqual(1, (result.Model as CategoriesViewModel).Categories.TotalItems);
            _tasks.VerifyAllExpectations();
        }

        [Test]
        public void Index_Forwards_To_Index_With_Page()
        {
            //Arrange
            _categoriesQuery.Expect(x => x.GetPagedList(2, 10)).Return(new CustomPagination<Category>(new List<Category> { new Category() }, 2, 10, 1));

            //Act
            var result = _controller.Index(2) as ViewResult;

            //Assert
            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<CategoriesViewModel>(result.Model);
            Assert.AreEqual(2, (result.Model as CategoriesViewModel).Categories.PageNumber);
            Assert.AreEqual(1, (result.Model as CategoriesViewModel).Categories.TotalItems);
            _categoriesQuery.VerifyAllExpectations();
        }

        [Test]
        public void Create_Forwards_To_Create_With_New_Category()
        {
            //Act
            var result = _controller.Create() as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<Category>(result.Model);
            Assert.AreEqual(0, (result.Model as Category).Id);
        }

        [Test]
        public void Save_Validates_Bad_Model_And_Forwards_To_Create()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();

            //Act
            var result = _controller.Save(new Category()) as ViewResult;

            //Assert
            Assert.AreEqual("Create", result.ViewName);
            Assert.IsInstanceOfType<Category>(result.Model);
            Assert.AreEqual(0, (result.Model as Category).Id);
        }

        [Test]
        public void Save_Validates_Bad_Model_And_Forwards_To_Edit()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();
            var category = new Category();
            category.SetIdTo(2);

            //Act
            var result = _controller.Save(category) as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<Category>(result.Model);
            Assert.AreEqual(2, (result.Model as Category).Id);
        }

        [Test]
        public void Save_Validates_Good_Model_And_Redirects_To_Index()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();
            _tasks.Expect(x => x.CreateOrUpdate(Arg<Category>.Is.Anything)).Return(new Category());

            //Act
            var result = _controller.Save(new Category { Name = "blah" }) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            _tasks.VerifyAllExpectations();
        }

        [Test]
        public void Edit_Forwards_To_View_With_Category()
        {
            //Arrange
            var category = new Category();
            category.SetIdTo(3);
            _tasks.Expect(x => x.Get(3)).Return(category);

            //Act
            var result = _controller.Edit(3) as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<Category>(result.Model);
            Assert.AreEqual(3, (result.Model as Category).Id);
            _tasks.VerifyAllExpectations();
        }

        [Test]
        public void Details_Forwards_To_View_With_Category()
        {
            //Arrange
            var category = new Category();
            category.SetIdTo(3);
            _tasks.Expect(x => x.Get(3)).Return(category);

            //Act
            var result = _controller.Details(3) as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<Category>(result.Model);
            Assert.AreEqual(3, (result.Model as Category).Id);
            _tasks.VerifyAllExpectations();
        }

        [Test]
        public void Delete_Deletes_Product_And_Redirects_To_Index()
        {
            //Arrange
            var category = new Category();
            category.SetIdTo(3);
            _tasks.Expect(x => x.Delete(3));

            //Act
            var result = _controller.Delete(3) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            _tasks.VerifyAllExpectations();
        }
    }
}
