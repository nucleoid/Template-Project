
using System.Web.Mvc;
using MbUnit.Framework;
using TemplateProject.Web.Mvc.Controllers;

namespace TemplateProject.Tests.Web.Mvc.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new HomeController();
        }

        [Test]
        public void Index_Forwards_To_Index_View()
        {
            //Act
            var result = _controller.Index() as ViewResult;

            //Assert
            Assert.IsNull(result.View);
        }
    }
}
