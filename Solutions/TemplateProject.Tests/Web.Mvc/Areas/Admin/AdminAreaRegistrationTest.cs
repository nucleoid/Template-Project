
using System.Web.Mvc;
using System.Web.Routing;
using MbUnit.Framework;
using MvcContrib.TestHelper;
using TemplateProject.Web.Mvc.Areas.Admin;
using TemplateProject.Web.Mvc.Areas.Admin.Controllers;

namespace TemplateProject.Tests.Web.Mvc.Areas.Admin
{
    [TestFixture]
    public class AdminAreaRegistrationTest
    {
        [FixtureSetUp]
        public void FixtureSetUp()
        {
            RouteTable.Routes.Clear();
            var area = new AdminAreaRegistration();
            var context = new AreaRegistrationContext(area.AreaName, RouteTable.Routes, null);
            area.RegisterArea(context);
        }

        [Test]
        [Row("~/Admin/Products")]
        [Row("~/Admin/Products/Index")]
        public void Get_List_Action_Routes_Routed(string route)
        {
            //Assert
            route.WithMethod(HttpVerbs.Get).ShouldMapTo<ProductsController>(x => x.Index(null));
        }

        [Test]
        [Row("~/Admin/Products/5", "5")]
        [Row("~/Admin/Products/600", "600")]
        public void Get_Single_Routes_Routed(string route, string id)
        {
            //Act
            var data = route.WithMethod(HttpVerbs.Get).Values;

            //Assert
            Assert.AreEqual("Products", data["controller"]);
            Assert.AreEqual("Index", data["action"]);
            Assert.AreEqual(id, data["id"]);
        }

        [Test]
        public void Get_Single_Action_Routes_Routed()
        {
            //Act
            var data = "~/Admin/Products/Create".WithMethod(HttpVerbs.Get).Values;

            //Assert
            Assert.AreEqual("Products", data["controller"]);
            Assert.AreEqual("Create", data["action"]);
        }

        [Test]
        public void Post_Route_Routed()
        {
            //Act
            var data = "~/Admin/Products".WithMethod(HttpVerbs.Post).Values;

            //Assert
            Assert.AreEqual("Products", data["controller"]);
            Assert.AreEqual("Save", data["action"]);
        }

        [Test]
        public void Put_Route_Routed()
        {
            //Act
            var data = "~/Admin/Products/5".WithMethod(HttpVerbs.Put).Values;

            //Assert
            Assert.AreEqual("Products", data["controller"]);
            Assert.AreEqual("Save", data["action"]);
            Assert.AreEqual("5", data["id"]);
        }

        [Test]
        public void Delete_Route_Routed()
        {
            //Act
            var data = "~/Admin/Products/5".WithMethod(HttpVerbs.Delete).Values;

            //Assert
            Assert.AreEqual("Products", data["controller"]);
            Assert.AreEqual("Delete", data["action"]);
            Assert.AreEqual("5", data["id"]);
        }

        [Test]
        [Row("~/Admin/Index")]
        [Row("~/Admin/default")]
        [Row("~/Admin/About")]
        public void Invalid_Controller_Routes_Not_Routed(string route)
        {
            //Act
            var data = route.WithMethod(HttpVerbs.Get).Values;

            //Assert
            Assert.AreNotEqual("Home", data["controller"]);
        }
    }
}
