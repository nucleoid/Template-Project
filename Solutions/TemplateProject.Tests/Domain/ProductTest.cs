
using System;
using MbUnit.Framework;
using TemplateProject.Domain;

namespace TemplateProject.Tests.Domain
{
    [TestFixture]
    public class ProductTest
    {
        [Test]
        public void Constructor_Sets_Defaults()
        {
            //Act
            var product = new Product();

            //Assert
            var now = DateTime.Now;
            Assert.AreEqual(now.Date, product.Created.Date);
            Assert.AreEqual(now.Date, product.Modified.Date);
        }
    }
}
