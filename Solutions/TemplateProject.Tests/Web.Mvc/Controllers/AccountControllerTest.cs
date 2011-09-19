
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MbUnit.Framework;
using Rhino.Mocks;
using TemplateProject.Web.Mvc.Controllers;
using TemplateProject.Web.Mvc.Models;

namespace TemplateProject.Tests.Web.Mvc.Controllers
{
    [TestFixture]
    public class AccountControllerTest
    {
        private AccountController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new AccountController();
        }

        [Test]
        public void LogOn_Forwards_To_LogOn_View()
        {
            //Act
            var result = _controller.LogOn() as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
        }

        [Test]
        public void LogOn_Validates_Bad_Model_And_Forwards_To_LogOn()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();
            
            //Act
            var result = _controller.LogOn(new LogOnModel(), null) as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<LogOnModel>(result.Model);
        }

        [Test]
        public void LogOn_Validates_Bad_Credentials_And_Forwards_To_LogOn()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();
            var model = new LogOnModel {UserName = "babygirl", Password = "nono"};

            //Act
            var result = _controller.LogOn(model, null) as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<LogOnModel>(result.Model);
            Assert.AreEqual("The user name or password provided is incorrect.", result.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage);
        }

        [Test]
        public void LogOn_Validates_Good_Credentials_And_Redirects_With_Bad_Local_Url()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();
            var model = new LogOnModel { UserName = "cooluser", Password = "coolpassword" };
            var request = new SimulatedHttpRequest("/", string.Empty, string.Empty, string.Empty, null, "localhost");
            HttpContext.Current = new HttpContext(request);
            var urlHelper = MockRepository.GenerateMock<UrlHelper>(new RequestContext(httpContext, new RouteData()));
            _controller.Url = urlHelper;

            //Act
            var result = _controller.LogOn(model, null) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [Test]
        public void LogOn_Validates_Good_Credentials_And_Redirects_With_Good_Local_Url()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();
            var model = new LogOnModel { UserName = "cooluser", Password = "coolpassword" };
            var request = new SimulatedHttpRequest("/", string.Empty, string.Empty, string.Empty, null, "localhost");
            HttpContext.Current = new HttpContext(request);
            var urlHelper = MockRepository.GenerateMock<UrlHelper>(new RequestContext(httpContext, routeData));
            _controller.Url = urlHelper;

            //Act
            var result = _controller.LogOn(model, "/blah") as RedirectResult;

            //Assert
            Assert.AreEqual("/blah", result.Url);
        }

        //TODO Mitch finish unit testing default account controller
        [Test, Ignore("not working yet")]
        public void LogOff_Signs_Out_And_Redirects()
        {
            //Arrange
            var request = new SimulatedHttpRequest("/", string.Empty, string.Empty, string.Empty, null, "localhost");
            HttpContext.Current = new HttpContext(request);

            //Act
            var result = _controller.LogOff() as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }
}
