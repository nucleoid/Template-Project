
using System.Collections.Generic;
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
    public class XmlValueProviderFactoryTest
    {
        [Test]
        public void AddToBackingStore_With_Some_Array_Keys_Same()
        {
            //Arrange
            var backingStore = new Dictionary<string, object>();
            var tree = new XElement("root",
                    new XElement("Child",
                        new XElement("innerChild", 3),
                        new XElement("innerChild", 4),
                        new XElement("children", 5)
                    )
            );
            //Act
            XmlValueProviderFactory.AddToBackingStore(backingStore, "omv", tree);

            //Assert
            Assert.AreEqual(0, backingStore.Count);
        }

        [Test]
        public void AddToBackingStore_Without_Array_Or_Elements()
        {
            //Arrange
            var backingStore = new Dictionary<string, object>();
            var tree = new XElement("root",
                new XElement("backItUp", 1)
            );
            //Act
            XmlValueProviderFactory.AddToBackingStore(backingStore, "omv", tree);

            //Assert
            Assert.AreEqual(1, backingStore.Count);
            Assert.AreEqual("1", backingStore["omv.backItUp"]);
        }

        [Test]
        public void AddToBackingStore_Without_Array_With_Elements()
        {
            //Arrange
            var backingStore = new Dictionary<string, object>();
            var tree = new XElement("root",
                new XElement("backItUp", 3),
                new XElement("thisNight",
                    new XElement("spam", 4),
                    new XElement("mail", 5)
                )
            );
            //Act
            XmlValueProviderFactory.AddToBackingStore(backingStore, "omv", tree);

            //Assert
            Assert.AreEqual(3, backingStore.Count);
            Assert.AreEqual("3", backingStore["omv.backItUp"]);
            Assert.AreEqual("4", backingStore["omv.thisNight.spam"]);
            Assert.AreEqual("5", backingStore["omv.thisNight.mail"]);
        }

        [Test]
        public void AddToBackingStore_With_Array()
        {
            //Arrange
            var backingStore = new Dictionary<string, object>();
            var tree = new XElement("root",
                    new XElement("Child", 
                        new XElement("innerChild", 3))
            );
            //Act
            XmlValueProviderFactory.AddToBackingStore(backingStore, "omv", tree);

            //Assert
            Assert.AreEqual(1, backingStore.Count);
            Assert.AreEqual("3", backingStore["omv[0].innerChild"]);
        }

        [Test]
        public void GetDeserializedXml_Without_Xml_Request()
        {
            //Arrange
            var request = MockRepository.GenerateMock<HttpRequestBase>();
            request.Expect(x => x.ContentType).Return("text/html");
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            httpContext.Expect(x => x.Request).Return(request);
            var context = new ControllerContext(httpContext, new RouteData(), new TestController());

            //Act
            var deserialized = XmlValueProviderFactory.GetDeserializedXml(context);

            //Assert
            Assert.IsNull(deserialized);
        }

        [Test]
        public void GetDeserializedXml_With_Bad_Stream()
        {
            //Arrange
            var request = MockRepository.GenerateMock<HttpRequestBase>();
            request.Expect(x => x.ContentType).Return("text/xml");
            request.Expect(x => x.InputStream).Return(null);
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            httpContext.Expect(x => x.Request).Return(request);
            var context = new ControllerContext(httpContext, new RouteData(), new TestController());

            //Act
            var deserialized = XmlValueProviderFactory.GetDeserializedXml(context);

            //Assert
            Assert.IsNull(deserialized);
        }

        [Test]
        public void GetDeserializedXml_Xml()
        {
            //Arrange
            var request = MockRepository.GenerateMock<HttpRequestBase>();
            request.Expect(x => x.ContentType).Return("text/xml");
            request.Expect(x => x.InputStream).Return(new MemoryStream(Encoding.Default.GetBytes("<root></root>")));
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            httpContext.Expect(x => x.Request).Return(request);
            var context = new ControllerContext(httpContext, new RouteData(), new TestController());

            //Act
            var deserialized = XmlValueProviderFactory.GetDeserializedXml(context);

            //Assert
            Assert.AreEqual("<root></root>", deserialized.FirstNode.ToString());
        }

        [Test]
        public void GetValueProvider_With_No_Xml_Data()
        {
            //Arrange
            var request = MockRepository.GenerateMock<HttpRequestBase>();
            request.Expect(x => x.ContentType).Return("text/html");
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            httpContext.Expect(x => x.Request).Return(request);
            var context = new ControllerContext(httpContext, new RouteData(), new TestController());

            //Act
            var valueProvider = new XmlValueProviderFactory().GetValueProvider(context);

            //Assert
            Assert.IsNull(valueProvider);
        }

        [Test]
        public void GetValueProvider_With_Xml()
        {
            //Arrange
            var request = MockRepository.GenerateMock<HttpRequestBase>();
            request.Expect(x => x.ContentType).Return("text/xml");
            request.Expect(x => x.InputStream).Return(new MemoryStream(Encoding.Default.GetBytes("<root><hurry>squee</hurry></root>")));
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            httpContext.Expect(x => x.Request).Return(request);
            var context = new ControllerContext(httpContext, new RouteData(), new TestController());

            //Act
            var valueProvider = new XmlValueProviderFactory().GetValueProvider(context);

            //Assert
            Assert.AreEqual("squee", valueProvider.GetValue("hurry").RawValue);
        }
    }
}
