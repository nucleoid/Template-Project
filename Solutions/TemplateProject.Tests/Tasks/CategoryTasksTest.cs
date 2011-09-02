
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
    public class CategoryTasksTest
    {
        private CategoryTasks _tasks;
        private INHibernateRepository<Category> _repository;

        [SetUp]
        public void Setup()
        {
            _repository = MockRepository.GenerateMock<INHibernateRepository<Category>>();
            _tasks = new CategoryTasks(_repository);
        }

        [Test]
        public void GetAll_Returns_All_Categories()
        {
            //Arrange
            _repository.Expect(x => x.GetAll()).Return(new List<Category>());

            //Act
            var categories = _tasks.GetAll();

            //Assert
            Assert.AreEqual(0, categories.Count);
            _repository.VerifyAllExpectations();
        }

        [Test]
        public void Get_Returns_Category_With_Correct_Id()
        {
            //Arrange
            const int Id = 4;
            var category = new Category();
            category.SetIdTo(Id);
            _repository.Expect(x => x.Get(Id)).Return(category);

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
            var category = new Category();
            _repository.Expect(x => x.SaveOrUpdate(category));

            //Act
            var updated = _tasks.CreateOrUpdate(category);

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
            var category = new Category();
            category.SetIdTo(2);
            _repository.Expect(x => x.SaveOrUpdate(category));

            //Act
            var updated = _tasks.CreateOrUpdate(category);

            //Assert
            Assert.AreEqual(DateTime.MinValue, updated.Created.Date);
            _repository.VerifyAllExpectations();
        }

        [Test]
        public void Delete_Id_Deletes_Category()
        {
            //Arrange
            const int Id = 4;
            var category = new Category();
            category.SetIdTo(Id);
            _repository.Expect(x => x.Get(Id)).Return(category);
            _repository.Expect(x => x.Delete(category));

            //Act
            _tasks.Delete(Id);

            //Assert
            _repository.VerifyAllExpectations();
        }

        [Test]
        public void Delete_Entity_Deletes_Category()
        {
            //Arrange
            var category = new Category();
            _repository.Expect(x => x.Delete(category));

            //Act
            _tasks.Delete(category);

            //Assert
            _repository.VerifyAllExpectations();
        }
    }
}
