
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MbUnit.Framework;
using Rhino.Mocks;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Tasks;
using TemplateProject.Web.Mvc.Areas.Admin.Controllers;
using TemplateProject.Web.Mvc.Binders;

namespace TemplateProject.Tests.Web.Mvc
{
    [TestFixture]
    public class ProductBinderTest
    {
        private ICategoryTasks _categoryTasks;
        private ProductBinder _binder;

        [SetUp]
        public void Setup()
        {
            _categoryTasks = MockRepository.GenerateMock<ICategoryTasks>();
            _binder = new ProductBinder(_categoryTasks);
        }

        [Test]
        public void BindModel_With_No_Base_Binding()
        {
            // Arrange
            var formCollection = new NameValueCollection();

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(Product));

            var bindingContext = new ModelBindingContext
            {
                ModelName = "Product",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };

            var httpcontext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = new ControllerContext(httpcontext, new RouteData(), new ProductsController(null, null, null, null, null));

            // Act
            var result = _binder.BindModel(controllerContext, bindingContext);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void BindModel_With_No_Category_ID()
        {
            // Arrange
            var formCollection = new NameValueCollection { {"Product.Name", "Sploosh"} };

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(Product));

            var bindingContext = new ModelBindingContext
            {
                ModelName = "Product",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };

            var httpcontext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = new ControllerContext(httpcontext, new RouteData(), new ProductsController(null, null, null, null, null));

            // Act
            var result = _binder.BindModel(controllerContext, bindingContext) as Product;

            // Assert
            Assert.IsNull(result.Category);
            Assert.AreEqual("Sploosh", result.Name);
            Assert.IsFalse(bindingContext.ModelState.IsValid);
        }

        [Test]
        public void BindModel_With_Bad_Category_ID()
        {
            // Arrange
            var formCollection = new NameValueCollection { { "Product.Name", "Sploosh" }, { "SelectedCategoryId", "a" } };

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(Product));

            var bindingContext = new ModelBindingContext
            {
                ModelName = "Product",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };

            var httpcontext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = new ControllerContext(httpcontext, new RouteData(), new ProductsController(null, null, null, null, null));

            // Act
            var result = _binder.BindModel(controllerContext, bindingContext) as Product;

            // Assert
            Assert.IsNull(result.Category);
            Assert.IsFalse(bindingContext.ModelState.IsValid);
        }

        [Test]
        public void BindModel_With_Category_ID()
        {
            // Arrange
            var formCollection = new NameValueCollection { { "Product.Name", "Sploosh" }, { "SelectedCategoryId", "2" } };

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(Product));

            var bindingContext = new ModelBindingContext
            {
                ModelName = "Product",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };

            var httpcontext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = new ControllerContext(httpcontext, new RouteData(), new ProductsController(null, null, null, null, null));
            _categoryTasks.Expect(x => x.Get(2)).Return(new Category {Name = "Bones"});

            // Act
            var result = _binder.BindModel(controllerContext, bindingContext) as Product;

            // Assert
            Assert.AreEqual("Bones", result.Category.Name);
            Assert.IsTrue(bindingContext.ModelState.IsValid);
            _categoryTasks.VerifyAllExpectations();
        }

        [Test]
        public void BindModel_With_MultipleAvailability_ID()
        {
            // Arrange
            var formCollection = new NameValueCollection 
            {
                { "Product.Name", "Sploosh" }, 
                { "Product.MultipleAvailability", string.Format("{0},{1},{2}",
                    FlaggedAvailability.Online, FlaggedAvailability.Store, FlaggedAvailability.ThirdParty) }
            };

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(Product));

            var bindingContext = new ModelBindingContext
            {
                ModelName = "Product",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };

            var httpcontext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = new ControllerContext(httpcontext, new RouteData(), new ProductsController(null, null, null, null, null));

            // Act
            var result = _binder.BindModel(controllerContext, bindingContext) as Product;

            // Assert
            Assert.AreEqual(FlaggedAvailability.Online | FlaggedAvailability.Store | FlaggedAvailability.ThirdParty, result.MultipleAvailability);
        }

        [Test]
        public void BindModel_With_JSON_Values()
        {
            // Arrange
            var formCollection = new NameValueCollection { { "Name", "Sploosh" }, { "SelectedCategoryId", "2" } };

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(Product));

            var bindingContext = new ModelBindingContext
            {
                ModelName = string.Empty,
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };

            var httpcontext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = new ControllerContext(httpcontext, new RouteData(), new ProductsController(null, null, null, null, null));
            _categoryTasks.Expect(x => x.Get(2)).Return(new Category { Name = "Bones" });

            // Act
            var result = _binder.BindModel(controllerContext, bindingContext) as Product;

            // Assert
            Assert.AreEqual("Bones", result.Category.Name);
            Assert.IsTrue(bindingContext.ModelState.IsValid);
            _categoryTasks.VerifyAllExpectations();
        }
    }
}
