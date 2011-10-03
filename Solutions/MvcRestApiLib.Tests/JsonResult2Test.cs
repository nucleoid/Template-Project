
using System.IO;
using System.Runtime.Serialization;
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
    public class JsonResult2Test
    {
        [Test]
        public void Constructor_Sets_Data()
        {
            //Act
            var result = new JsonResult2(54);

            //Assert
            Assert.AreEqual(54, result.Data);
        }

        [Test, ExpectedArgumentNullException]
        public void ExecuteResult_With_Null_Context()
        {
            //Act
            new JsonResult2().ExecuteResult(null);
        }

        [Test]
        public void ExecuteResult_With_ContentType()
        {
            //Arrange
            var httpResponse = new FakeResponse();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            httpContext.Expect(x => x.Response).Return(httpResponse);
            var context = new ControllerContext(httpContext, new RouteData(), new TestController());
            var result = new JsonResult2 {Data = 54, ContentType = "winter"};

            //Act
            result.ExecuteResult(context);

            //Assert
            Assert.AreEqual("winter", httpResponse.ContentType);
        }

        [Test]
        public void ExecuteResult_Without_ContentType()
        {
            //Arrange
            var httpResponse = new FakeResponse();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            httpContext.Expect(x => x.Response).Return(httpResponse);
            var context = new ControllerContext(httpContext, new RouteData(), new TestController());
            var result = new JsonResult2 { Data = 54 };

            //Act
            result.ExecuteResult(context);

            //Assert
            Assert.AreEqual("application/json", httpResponse.ContentType);
        }

        [Test]
        public void ExecuteResult_With_ContentEncoding()
        {
            //Arrange
            var httpResponse = new FakeResponse();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            httpContext.Expect(x => x.Response).Return(httpResponse);
            var context = new ControllerContext(httpContext, new RouteData(), new TestController());
            var result = new JsonResult2 { Data = 54, ContentEncoding = Encoding.UTF32 };

            //Act
            result.ExecuteResult(context);

            //Assert
            Assert.AreEqual(Encoding.UTF32, httpResponse.ContentEncoding);
        }

        [Test]
        public void ExecuteResult_Serializes_Data()
        {
            //Arrange
            var httpResponse = new FakeResponse();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            httpContext.Expect(x => x.Response).Return(httpResponse);
            var context = new ControllerContext(httpContext, new RouteData(), new TestController());
            var result = new JsonResult2(new TestSerial {Number = 54});

            //Act
            result.ExecuteResult(context);

            //Assert
            httpResponse.OutputStream.Position = 0;
            var reader = new StreamReader(httpResponse.OutputStream);
            Assert.AreEqual("{\"Number\":54}", reader.ReadToEnd());
        }
    }
}
