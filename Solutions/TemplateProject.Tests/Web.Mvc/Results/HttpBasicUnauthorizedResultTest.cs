
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MbUnit.Framework;
using Rhino.Mocks;
using TemplateProject.Web.Mvc.Results;

namespace TemplateProject.Tests.Web.Mvc.Results
{
    [TestFixture]
    public class HttpBasicUnauthorizedResultTest
    {
        [Test, ExpectedArgumentNullException]
        public void ExecuteResult_With_Null_Context()
        {
            //Act
            new HttpBasicUnauthorizedResult().ExecuteResult(null);
        }

        [Test]
        public void ExecuteResult_Without_Restful_Request()
        {
            //Arrange
            var request = MockRepository.GenerateMock<HttpRequestBase>();
            request.Expect(x => x.AcceptTypes).Return(new[] {"text/html"});
            var httpcontext = MockRepository.GenerateStub<HttpContextBase>();
            var response = MockRepository.GenerateMock<HttpResponseBase>();
            response.Expect(x => x.AddHeader(Arg<string>.Is.Equal("WWW-Authenticate"), Arg<string>.Is.Equal("Basic"))).Repeat.Never();
            response.Expect(x => x.StatusCode = 401);
            httpcontext.Expect(x => x.Request).Return(request);
            httpcontext.Expect(x => x.Response).Return(response);
            var controllerContext = new ControllerContext(httpcontext, new RouteData(), new TestController());

            //Act
            new HttpBasicUnauthorizedResult().ExecuteResult(controllerContext);

            //Assert
            request.VerifyAllExpectations();
            response.VerifyAllExpectations();
        }

        [Test]
        public void ExecuteResult_With_Restful_Request()
        {
            //Arrange
            var request = MockRepository.GenerateMock<HttpRequestBase>();
            request.Expect(x => x.AcceptTypes).Return(new[] { "text/json" });
            var httpcontext = MockRepository.GenerateStub<HttpContextBase>();
            var response = MockRepository.GenerateMock<HttpResponseBase>();
            response.Expect(x => x.AddHeader(Arg<string>.Is.Equal("WWW-Authenticate"), Arg<string>.Is.Equal("Basic")));
            httpcontext.Expect(x => x.Request).Return(request);
            httpcontext.Expect(x => x.Response).Return(response);
            var controllerContext = new ControllerContext(httpcontext, new RouteData(), new TestController());

            //Act
            new HttpBasicUnauthorizedResult().ExecuteResult(controllerContext);

            //Assert
            request.VerifyAllExpectations();
            response.VerifyAllExpectations();
        }

        private class TestController : Controller
        {
        }
    }
}
