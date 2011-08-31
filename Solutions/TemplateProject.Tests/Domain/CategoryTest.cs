using MbUnit.Framework;
using TemplateProject.Domain;

namespace TemplateProject.Tests.Domain
{
    [TestFixture]
    public class CategoryTest
    {
        [Test]
        public void Constructor_Sets_Defaults()
        {
            //Act
            var category = new Category();

            //Assert
            Assert.AreEqual(0, category.Products.Count);
        }
    }
}
