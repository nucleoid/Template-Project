
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MbUnit.Framework;
using Rhino.Mocks;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Tasks;
using TemplateProject.Web.Mvc.Areas.Admin.Controllers;
using TemplateProject.Web.Mvc.Areas.Admin.Models;
using TemplateProject.Web.Mvc.Binders;

namespace TemplateProject.Tests.Web.Mvc.Binders
{
    [TestFixture]
    public class ProductEditViewModelBinderTest
    {
        private ICategoryTasks _categoryTasks;
        private ProductEditViewModelBinder _binder;

        [SetUp]
        public void Setup()
        {
            _categoryTasks = MockRepository.GenerateMock<ICategoryTasks>();
            _binder = new ProductEditViewModelBinder(_categoryTasks);
        }

        [Test]
        public void BindModel_With_Category_ID()
        {
            // Arrange
            var formCollection = new NameValueCollection { { "ProductEditViewModel.Product.Id", "2" }, { "ProductEditViewModel.Product.Name", "sploosh" }, { "SelectedCategoryId", "2" } };

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(ProductEditViewModel));

            var bindingContext = new ModelBindingContext
            {
                ModelName = "ProductEditViewModel",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };

            var httpcontext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = new ControllerContext(httpcontext, new RouteData(), new ProductsController(null, null, null, null));
            _categoryTasks.Expect(x => x.Get(2)).Return(new Category { Name = "Bones" });

            // Act
            var result = _binder.BindModel(controllerContext, bindingContext) as ProductEditViewModel;

            // Assert
            Assert.AreEqual(2, result.Product.Id);
            Assert.AreEqual("sploosh", result.Product.Name);
            Assert.AreEqual("Bones", result.Product.Category.Name);
            Assert.IsTrue(bindingContext.ModelState.IsValid);
        }
    }
}
