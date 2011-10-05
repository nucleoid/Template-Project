﻿
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MbUnit.Framework;
using MvpRestApiLib;
using Rhino.Mocks;

namespace MvcRestApiLib.Tests
{
    [TestFixture]
    public class EnableXmlAttributeTest
    {
        private EnableXmlAttribute _attribute;

        [SetUp]
        public void Setup()
        {
            _attribute = new EnableXmlAttribute();
        }

        [Test]
        public void OnActionExecuted_With_Redirect()
        {
            //Arrange
            var filterContext = new ActionExecutedContext { Result = new RedirectToRouteResult("blah", new RouteValueDictionary()) };


            //Act
            _attribute.OnActionExecuted(filterContext);

            //Assert
            Assert.IsInstanceOfType<RedirectToRouteResult>(filterContext.Result);
        }

        [Test]
        public void OnActionExecuted_With_Wrong_AcceptType()
        {
            //Arrange
            var httpcontext = MockRepository.GenerateMock<HttpContextBase>();
            var httpRequest = MockRepository.GenerateMock<HttpRequestBase>();
            httpRequest.Expect(x => x.AcceptTypes).Return(null);
            httpcontext.Expect(x => x.Request).Return(httpRequest);
            var controllerContext = new ControllerContext(httpcontext, new RouteData(), new FakeController { ViewData = new ViewDataDictionary(new TestRestModel()) });
            var descriptor = MockRepository.GenerateStub<ActionDescriptor>();
            var filterContext = new ActionExecutedContext(controllerContext, descriptor, false, null);

            //Act
            _attribute.OnActionExecuted(filterContext);

            //Assert
            Assert.IsInstanceOfType<EmptyResult>(filterContext.Result);
        }

        [Test]
        [Row("application/xml")]
        [Row("text/xml")]
        public void OnActionExecuted_With_Correct_AcceptType(string acceptType)
        {
            //Arrange
            var httpcontext = MockRepository.GenerateMock<HttpContextBase>();
            var httpRequest = MockRepository.GenerateMock<HttpRequestBase>();
            httpRequest.Expect(x => x.AcceptTypes).Return(new[] { acceptType });
            httpRequest.Expect(x => x.ContentType).Return(acceptType);
            httpcontext.Expect(x => x.Request).Return(httpRequest);
            var controllerContext = new ControllerContext(httpcontext, new RouteData(), new FakeController { ViewData = new ViewDataDictionary(new TestRestModel()) });
            var descriptor = MockRepository.GenerateStub<ActionDescriptor>();
            var filterContext = new ActionExecutedContext(controllerContext, descriptor, false, null);

            //Act
            _attribute.OnActionExecuted(filterContext);

            //Assert
            Assert.IsInstanceOfType<XmlResult>(filterContext.Result);
            var result = filterContext.Result as XmlResult;
            Assert.AreEqual("<root>the model<root>", result.Data);
            Assert.AreEqual(Encoding.UTF8, result.ContentEncoding);
            Assert.AreEqual(acceptType, result.ContentType);
        }

        [Test, ExpectedArgumentException("REST Model must implement IRestModel")]
        public void OnActionExecuted_Without_RestModel()
        {
            //Arrange
            var httpcontext = MockRepository.GenerateMock<HttpContextBase>();
            var httpRequest = MockRepository.GenerateMock<HttpRequestBase>();
            httpRequest.Expect(x => x.AcceptTypes).Return(new[] { "application/xml" });
            httpcontext.Expect(x => x.Request).Return(httpRequest);
            var controllerContext = new ControllerContext(httpcontext, new RouteData(), new FakeController { ViewData = new ViewDataDictionary(2) });
            var descriptor = MockRepository.GenerateStub<ActionDescriptor>();
            var filterContext = new ActionExecutedContext(controllerContext, descriptor, false, null);

            //Act
            _attribute.OnActionExecuted(filterContext);
        }

        private class FakeController : Controller
        {
        }

        private class TestRestModel : IRestModel
        {
            public object RestModel
            {
                get { return "<root>the model<root>"; }
            }
        }
    }
}