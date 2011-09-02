
using MbUnit.Framework;
using TemplateProject.Web.Mvc.Extensions;

namespace TemplateProject.Tests.Web.Mvc.Extensions
{
    [TestFixture]
    public class IntegerExtensionsTest
    {
        [Test]
        public void PluralizeLabel_No_Items()
        {
            //Act
            var label = 0.PluralizeLabel("object", "objects");

            //Assert
            Assert.AreEqual("objects", label);
        }

        [Test]
        public void PluralizeLabel_Singular_Item()
        {
            //Act
            var label = 1.PluralizeLabel("object", "objects");

            //Assert
            Assert.AreEqual("object", label);
        }

        [Test]
        public void PluralizeLabel_Multiple_Items()
        {
            //Act
            var label = 2.PluralizeLabel("object", "objects");

            //Assert
            Assert.AreEqual("objects", label);
        }
    }
}
