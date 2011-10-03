
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using System.Xml.Linq;
using MbUnit.Framework;
using MvpRestApiLib;
using Rhino.Mocks;

namespace MvcRestApiLib.Tests
{
    [TestFixture]
    public class XmlResultTest
    {
        [Test]
        public void Constructor_Sets_Data()
        {
            //Act
            var result = new XmlResult(54);

            //Assert
            Assert.AreEqual(54, result.Data);
        }

        [Test, ExpectedArgumentNullException]
        public void ExecuteResult_With_Null_Context()
        {
            //Act
            new XmlResult().ExecuteResult(null);
        }

        [Test]
        public void ExecuteResult_With_ContentType()
        {
            //Arrange
            var httpResponse = new FakeResponse();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            httpContext.Expect(x => x.Response).Return(httpResponse);
            var context = new ControllerContext(httpContext, new RouteData(), new TestController());
            var result = new XmlResult {Data = 54, ContentType = "winter"};

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
            var result = new XmlResult {Data = 54};

            //Act
            result.ExecuteResult(context);

            //Assert
            Assert.AreEqual("text/xml", httpResponse.ContentType);
        }

        [Test]
        public void ExecuteResult_With_ContentEncoding()
        {
            //Arrange
            var httpResponse = new FakeResponse();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            httpContext.Expect(x => x.Response).Return(httpResponse);
            var context = new ControllerContext(httpContext, new RouteData(), new TestController());
            var result = new XmlResult {Data = 54, ContentEncoding = Encoding.UTF32};

            //Act
            result.ExecuteResult(context);

            //Assert
            Assert.AreEqual(Encoding.UTF32, httpResponse.ContentEncoding);
        }

        [Test]
        public void ExecuteResult_With_XmlNode()
        {
            //Arrange
            var httpResponse = new FakeResponse();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            httpContext.Expect(x => x.Response).Return(httpResponse);
            var context = new ControllerContext(httpContext, new RouteData(), new TestController());
            var node = new XmlDocument();
            node.LoadXml("<node>winter</node>");
            var result = new XmlResult {Data = node};

            //Act
            result.ExecuteResult(context);

            //Assert
            Assert.AreEqual("<node>winter</node>", httpResponse.TestString);
        }

        [Test]
        public void ExecuteResult_With_XNode()
        {
            //Arrange
            var httpResponse = new FakeResponse();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            httpContext.Expect(x => x.Response).Return(httpResponse);
            var context = new ControllerContext(httpContext, new RouteData(), new TestController());
            var node = new XDocument();
            node.AddFirst(" \t ");
            var result = new XmlResult {Data = node};

            //Act
            result.ExecuteResult(context);

            //Assert
            Assert.AreEqual(" \t ", httpResponse.TestString);
        }

        [Test]
        public void ExecuteResult_With_DataContract_Uses_DataContractSerializer()
        {
            //Arrange
            var httpResponse = new FakeResponse();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            httpContext.Expect(x => x.Response).Return(httpResponse);
            var context = new ControllerContext(httpContext, new RouteData(), new TestController());
            var result = new XmlResult(new TestSerial { Number = 54 });

            //Act
            result.ExecuteResult(context);

            //Assert
            httpResponse.OutputStream.Position = 0;
            var reader = new StreamReader(httpResponse.OutputStream);
            Assert.Contains(reader.ReadToEnd(), "<Number>54</Number>");
        }

        [Test]
        public void ExecuteResult_Uses_XmlSerializer()
        {
            //Arrange
            var httpResponse = new FakeResponse();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            httpContext.Expect(x => x.Response).Return(httpResponse);
            var context = new ControllerContext(httpContext, new RouteData(), new TestController());
            var result = new XmlResult(new NonDataContractTestSerial { Number = 54 });

            //Act
            result.ExecuteResult(context);

            //Assert
            httpResponse.OutputStream.Position = 0;
            var reader = new StreamReader(httpResponse.OutputStream);
            Assert.Contains(reader.ReadToEnd(), "<Number>54</Number>");
        }

        public class NonDataContractTestSerial
        {
            public int Number { get; set; }
        }
    }
}
