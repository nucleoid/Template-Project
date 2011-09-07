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
            var container = MockRepository.GenerateStub<IViewDataContainer>();
            var viewContext = MockRepository.GenerateStub<ViewContext>();
            var helper = new HtmlHelper(viewContext, container);

            //Act
            helper.CheckBoxWithValue(null, false, null, null);
        }


        [Test]
        public void CheckBoxWithValue_With_Name()
        {
            //Arrange
            var container = MockRepository.GenerateStub<IViewDataContainer>();
            var viewContext = MockRepository.GenerateStub<ViewContext>();
            var helper = new HtmlHelper(viewContext, container);

            //Act
            var checkBox = helper.CheckBoxWithValue("print", 3);

            //Assert
            Assert.AreEqual("<input id=\"print\" name=\"print\" type=\"checkbox\" value=\"3\" />", checkBox.ToString());
        }

        [Test]
        public void CheckBoxWithValue_With_Name_And_Checked()
        {
            //Arrange
            var container = MockRepository.GenerateStub<IViewDataContainer>();
            var viewContext = MockRepository.GenerateStub<ViewContext>();
            var helper = new HtmlHelper(viewContext, container);

            //Act
            var checkBox = helper.CheckBoxWithValue("print", true, 3);

            Assert.AreEqual("<input checked=\"checked\" id=\"print\" name=\"print\" type=\"checkbox\" value=\"3\" />", checkBox.ToString());
        }

        [Test]
        public void CheckBoxWithValue_With_Name_And_Checked_And_HtmlAttributes()
        {
            //Arrange
            var container = MockRepository.GenerateStub<IViewDataContainer>();
            var viewContext = MockRepository.GenerateStub<ViewContext>();
            var helper = new HtmlHelper(viewContext, container);

            //Act
            var checkBox = helper.CheckBoxWithValue("print", true, 3, new { awesomize = "yes", @class = "linkage" });

            //Assert
            Assert.AreEqual("<input awesomize=\"yes\" checked=\"checked\" class=\"linkage\" id=\"print\" name=\"print\" type=\"checkbox\" value=\"3\" />",
                checkBox.ToString());
        }
    }
}
