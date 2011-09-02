
using System;
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

        [Test]
        public void CreateOrUpdate_Sets_Created_And_Modified_And_Saves()
        {
            //Arrange
            var product = new Product();
            _repository.Expect(x => x.SaveOrUpdate(product));

            //Act
            var updated = _tasks.CreateOrUpdate(product);

            //Assert
            var now = DateTime.Now;
            Assert.AreEqual(now.Date, updated.Modified.Date);
            Assert.AreEqual(now.Date, updated.Created.Date);
            _repository.VerifyAllExpectations();
        }

        [Test]
        public void CreateOrUpdate_Does_Not_Set_Created_With_Existing_And_Saves()
        {
            //Arrange
            var product = new Product();
            product.SetIdTo(2);
            _repository.Expect(x => x.SaveOrUpdate(product));

            //Act
            var updated = _tasks.CreateOrUpdate(product);

            //Assert
            Assert.AreEqual(DateTime.MinValue, updated.Created.Date);
            _repository.VerifyAllExpectations();
        }

        [Test]
        public void Delete_Id_Deletes_Product()
        {
            //Arrange
            const int Id = 4;
            var product = new Product();
            product.SetIdTo(Id);
            _repository.Expect(x => x.Get(Id)).Return(product);
            _repository.Expect(x => x.Delete(product));

            //Act
            _tasks.Delete(Id);

            //Assert
            _repository.VerifyAllExpectations();
        }

        [Test]
        public void Delete_Entity_Deletes_Product()
        {
            //Arrange
            var product = new Product();
            _repository.Expect(x => x.Delete(product));

            //Act
            _tasks.Delete(product);

            //Assert
            _repository.VerifyAllExpectations();
        }
    }
}
