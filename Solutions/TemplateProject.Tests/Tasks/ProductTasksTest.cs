
using System.Collections.Generic;
using MbUnit.Framework;
using Rhino.Mocks;
using SharpArch.NHibernate.Contracts.Repositories;
using SharpArch.Testing;
using TemplateProject.Domain;
using TemplateProject.Tasks;

namespace TemplateProject.Tests.Tasks
{
    [TestFixture]
    public class ProductTasksTest
    {
        private ProductTasks _tasks;
        private INHibernateRepository<Product> _repository;

        [SetUp]
        public void Setup()
        {
            _repository = MockRepository.GenerateMock<INHibernateRepository<Product>>();
            _tasks = new ProductTasks(_repository);
        }

        [Test]
        public void GetAll_Returns_All_Products()
        {
            //Arrange
            _repository.Expect(x => x.GetAll()).Return(new List<Product>());

            //Act
            var products = _tasks.GetAll();

            //Assert
            Assert.AreEqual(0, products.Count);
            _repository.VerifyAllExpectations();
        }

        [Test]
        public void Get_Returns_Product_With_Correct_Id()
        {
            //Arrange
            const int Id = 4;
            var product = new Product();
            product.SetIdTo(Id);
            _repository.Expect(x => x.Get(Id)).Return(product);

            //Act
            var gotten = _tasks.Get(Id);

            //Assert
            Assert.AreEqual(Id, gotten.Id);
            _repository.VerifyAllExpectations();
        }
    }
}
