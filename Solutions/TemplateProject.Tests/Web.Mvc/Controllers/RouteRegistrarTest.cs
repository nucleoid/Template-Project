
using System.Web.Routing;
using MbUnit.Framework;
using MvcContrib.TestHelper;
using TemplateProject.Web.Mvc.Controllers;

namespace TemplateProject.Tests.Web.Mvc.Controllers
{
    [TestFixture]
    public class RouteRegistrarTest
    {
        [FixtureSetUp]
        public void FixtureSetUp()
        {
            RouteRegistrar.RegisterRoutesTo(RouteTable.Routes);
        }

        [Test]
        public void Resource_Route_Ignored()
        {
            //Assert
            "~/blah.axd".ShouldBeIgnored();
        }

        [Test]
        public void favicon_Route_Ignored()
        {
            //Assert
            "~/favicon.ico".ShouldBeIgnored();
        }

        [Test]
        public void Default_Route_Maps_To_Home_Controller()
        {
            //Assert
            "~/".ShouldMapTo<HomeController>(x => x.Index());
        }

        [Test]
        [Row("~/Home")]
        [Row("~/Home/Index")]
        public void Controller_Action_Routes_Routed(string route)
        {
            //Assert
            route.ShouldMapTo<HomeController>(x => x.Index());
        }

        [Test]
        [Row("~/Home/Index/5", "5")]
        [Row("~/Home/Index/test", "test")]
        public void Id_Routes_Routed(string route, string id)
        {
            //Act
            var data = route.Route();

            //Assert
            Assert.AreEqual("Home", data.Values["controller"]);
            Assert.AreEqual("Index", data.Values["action"]);
            Assert.AreEqual(id, data.Values["id"]);
        }

        [Test]
        [Row("~/Index")]
        [Row("~/default")]
        [Row("~/About")]
        public void Invalid_Controller_Routes_Not_Routed(string route)
        {
            //Act
            var data = route.Route();

            //Assert
            Assert.AreNotEqual("Home", data.Values["controller"]);
        }
    }
}
