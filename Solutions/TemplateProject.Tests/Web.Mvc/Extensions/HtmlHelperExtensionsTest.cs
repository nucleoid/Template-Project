using System.Web.Mvc;
using MbUnit.Framework;
using Rhino.Mocks;
using TemplateProject.Web.Mvc.Extensions;

namespace TemplateProject.Tests.Web.Mvc.Extensions
{
    [TestFixture]
    public class HtmlHelperExtensionsTest
    {
        [Test, ExpectedArgumentException]
        public void CheckBoxWithValue_Without_Name()
        {
            //Arrange
            HtmlHelper helper = MvcHelper.GetHtmlHelper();

            //Act
            helper.CheckBoxWithValue(null, false, null, null);
        }


        [Test]
        public void CheckBoxWithValue_With_Name()
        {
            //Arrange
            HtmlHelper helper = MvcHelper.GetHtmlHelper();

            //Act
            var checkBox = helper.CheckBoxWithValue("print", 3);

            //Assert
            Assert.AreEqual("<input id=\"print\" name=\"print\" type=\"checkbox\" value=\"3\" />", checkBox.ToString());
        }

        [Test]
        public void CheckBoxWithValue_With_Name_And_Checked()
        {
            //Arrange
            HtmlHelper helper = MvcHelper.GetHtmlHelper();

            //Act
            var checkBox = helper.CheckBoxWithValue("print", true, 3);

            Assert.AreEqual("<input checked=\"checked\" id=\"print\" name=\"print\" type=\"checkbox\" value=\"3\" />", checkBox.ToString());
        }

        [Test]
        public void CheckBoxWithValue_With_Name_And_Checked_And_HtmlAttributes()
        {
            //Arrange
            HtmlHelper helper = MvcHelper.GetHtmlHelper();

            //Act
            var checkBox = helper.CheckBoxWithValue("print", true, 3, new { awesomize = "yes", @class = "linkage" });

            //Assert
            Assert.AreEqual("<input awesomize=\"yes\" checked=\"checked\" class=\"linkage\" id=\"print\" name=\"print\" type=\"checkbox\" value=\"3\" />",
                checkBox.ToString());
        }

        [Test]
        public void ActionLinkArea_Routes_To_Area()
        {
            //Arrange
            HtmlHelper helper = MvcHelper.GetHtmlHelper();

            //Act
            var linkage = helper.ActionLinkArea<TestController>(c => c.Tester(), "test link", "SecretArea");

            //Assert
            Assert.AreEqual("<a href=\"/SecretArea/Test/Tester\">test link</a>", linkage.ToString());
        }

        [Test]
        public void ActionLinkArea_HtmlAttributes_Routes_To_Area()
        {
            //Arrange
            HtmlHelper helper = MvcHelper.GetHtmlHelper();

            //Act
            var linkage = helper.ActionLinkArea<TestController>(c => c.Tester(), "test link", "SecretArea", new { coolness = "11" });

            //Assert
            Assert.AreEqual("<a coolness=\"11\" href=\"/SecretArea/Test/Tester\">test link</a>", linkage.ToString());
        }

        private class TestController : Controller
        {
            public ActionResult Tester()
            {
                return null;
            }
        }
    }
}
