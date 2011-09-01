
using System.Web.Mvc;
using MbUnit.Framework;
using TemplateProject.Web.Mvc.Controllers;

namespace TemplateProject.Tests.Web.Mvc.Controllers
{
    [TestFixture]
    public class ErrorControllerTest
    {
        private ErrorController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new ErrorController();
        }

        [Test]
        public void NotFound_Forwards_To_NotFound_View()
        {
            //Act
            var result = _controller.NotFound() as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
        }

        [Test]
        public void Excepted_Forwards_To_Excepted_View()
        {
            //Act
            var result = _controller.Excepted() as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
        }
    }
}
