
using System.Collections.Generic;
using MbUnit.Framework;
using Rhino.Mocks;
using SharpArch.Testing;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Tasks;
using TemplateProject.Tasks.CommandHandlers;
using TemplateProject.Tasks.Commands;

namespace TemplateProject.Tests.Tasks.Commands
{
    [TestFixture]
    public class MassCategoryChangeHandlerTest
    {
        private IProductTasks _productTasks;
        private ICategoryTasks _categoryTasks;
        private MassCategoryChangeHandler _handler;

        [SetUp]
        public void Setup()
        {
            _productTasks = MockRepository.GenerateMock<IProductTasks>();
            _categoryTasks = MockRepository.GenerateMock<ICategoryTasks>();
            _handler = new MassCategoryChangeHandler(_productTasks, _categoryTasks);
        }

        [Test]
        public void Handle_Does_Not_Find_Category()
        {
            //Arrange
            const int catId = 3;
            _categoryTasks.Expect(x => x.Get(catId)).Return(null);

            //Act
            var result = _handler.Handle(new MassCategoryChangeCommand(catId, new List<int>()));

            //Assert
            Assert.IsFalse(result.Success);
            _categoryTasks.VerifyAllExpectations();
            _productTasks.AssertWasNotCalled(x => x.Get(Arg<int>.Is.Anything));
        }

        [Test]
        public void Handle_Does_Not_Find_A_Product()
        {
            //Arrange
            const int catId = 3;
            var productIds = new List<int> {2, 3, 4};
            _categoryTasks.Expect(x => x.Get(catId)).Return(new Category());
            _productTasks.Expect(x => x.Get(Arg<int>.Is.Anything)).Repeat.Once().Return(null);

            //Act
            var result = _handler.Handle(new MassCategoryChangeCommand(catId, productIds));

            //Assert
            Assert.IsFalse(result.Success);
            _categoryTasks.VerifyAllExpectations();
            _productTasks.VerifyAllExpectations();
        }

        [Test]
        public void Handle_Changes_Does_Not_Change_Category_If_Same()
        {
            //Arrange
            const int catId = 3;
            var category = new Category {Name = "sharp"};
            category.SetIdTo(catId);
            var productIds = new List<int> { 2 };
            var product1 = new Product {Category = category};
            product1.SetIdTo(2);
            _categoryTasks.Expect(x => x.Get(catId)).Return(category);
            _productTasks.Expect(x => x.Get(2)).Return(product1);
            _productTasks.AssertWasNotCalled(x => x.CreateOrUpdate(Arg<Product>.Is.Anything));

            //Act
            var result = _handler.Handle(new MassCategoryChangeCommand(catId, productIds));

            //Assert
            Assert.IsTrue(result.Success);
            _categoryTasks.VerifyAllExpectations();
            _productTasks.VerifyAllExpectations();
        }

        [Test]
        public void Handle_Changes_All_Product_Categories()
        {
            //Arrange
            const int catId = 3;
            var category = new Category {Name = "sharp"};
            category.SetIdTo(catId);
            var productIds = new List<int> { 2, 3, 4 };
            var product1 = new Product { Category = new Category() };
            product1.SetIdTo(2);
            var product2 = new Product { Category = new Category() };
            product2.SetIdTo(3);
            var product3 = new Product { Category = new Category() };
            product3.SetIdTo(4);
            _categoryTasks.Expect(x => x.Get(catId)).Return(category);
            _productTasks.Expect(x => x.Get(2)).Return(product1);
            _productTasks.Expect(x => x.Get(3)).Return(product2);
            _productTasks.Expect(x => x.Get(4)).Return(product3);
            _productTasks.Expect(x => x.CreateOrUpdate(Arg<Product>.Matches(y => y.Id == 2 && y.Category.Name == "sharp"))).Return(product1);
            _productTasks.Expect(x => x.CreateOrUpdate(Arg<Product>.Matches(y => y.Id == 3 && y.Category.Name == "sharp"))).Return(product2);
            _productTasks.Expect(x => x.CreateOrUpdate(Arg<Product>.Matches(y => y.Id == 4 && y.Category.Name == "sharp"))).Return(product3);

            //Act
            var result = _handler.Handle(new MassCategoryChangeCommand(catId, productIds));

            //Assert
            Assert.IsTrue(result.Success);
            _categoryTasks.VerifyAllExpectations();
            _productTasks.VerifyAllExpectations();
        }
    }
}
