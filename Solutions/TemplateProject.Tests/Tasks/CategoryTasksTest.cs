
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
    }
}
