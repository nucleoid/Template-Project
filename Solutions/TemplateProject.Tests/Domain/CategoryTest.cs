using System;
using MbUnit.Framework;
using SharpArch.Testing;
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

        [Test, ExpectedArgumentNullException]
        public void AddProduct_With_Null_Product()
        {
            //Act
            new Category().AddProduct(null);
        }

        [Test]
        public void AddProduct_Sets_Category_And_Adds_Product()
        {
            //Arrange
            var product = new Product();
            var category = new Category();
            category.SetIdTo(4);

            //Act
            category.AddProduct(product);

            //Assert
            Assert.AreEqual(1, category.Products.Count);
            Assert.AreEqual(4, product.Category.Id);
        }
    }
}
