
using System.Web;
using MbUnit.Framework;
using Rhino.Mocks;
using TemplateProject.Web.Mvc.Extensions;

namespace TemplateProject.Tests.Web.Mvc.Extensions
{
    [TestFixture]
    public class RequestBaseExtensionsTest
    {
        [Test]
        [Row("application/xml")]
        [Row("text/xml")]
        [Row("application/json")]
        [Row("text/json")]
        public void IsRestful_With_Rest_Request(string acceptType)
        {
            //Arrange
            var request = MockRepository.GenerateMock<HttpRequestBase>();
            request.Expect(x => x.AcceptTypes).Return(new[] {acceptType});

            //Act
            var restful = request.IsRestful();

            //Assert
            Assert.IsTrue(restful);
        }

        [Test]
        public void IsRestful_Without_Rest_Request()
        {
            //Arrange
            var request = MockRepository.GenerateMock<HttpRequestBase>();
            request.Expect(x => x.AcceptTypes).Return(new[] { "text/html" });

            //Act
            var restful = request.IsRestful();

            //Assert
            Assert.IsFalse(restful);
        }
    }
}
