using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MbUnit.Framework;
using Rhino.Mocks;
using TemplateProject.Domain;
using TemplateProject.Web.Mvc.Areas.Admin.Controllers;
using TemplateProject.Web.Mvc.Binders;

namespace TemplateProject.Tests.Web.Mvc.Binders
{
    [TestFixture]
    public class FlaggedAvailabilityBinderTest
    {
        private FlaggedAvailabilityBinder _binder;

        [SetUp]
        public void Setup()
        {
            _binder = new FlaggedAvailabilityBinder();
        }

        [Test]
        public void BindModel_With_No_MultipleAvailability_ID()
        {
            // Arrange
            var formCollection = new NameValueCollection { { "Product.Name", "sploosh" } };

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(FlaggedAvailability));

            var bindingContext = new ModelBindingContext
            {
                ModelName = "FlaggedAvailability",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };

            var httpcontext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = new ControllerContext(httpcontext, new RouteData(), new ProductsController(null, null, null, null));

            // Act
            var result = _binder.BindModel(controllerContext, bindingContext);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void BindModel_With_Empty_MultipleAvailability_ID()
        {
            // Arrange
            var formCollection = new NameValueCollection { { "Product.Name", "sploosh" }, { "Product.MultipleAvailability", string.Empty } };

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(FlaggedAvailability));

            var bindingContext = new ModelBindingContext
            {
                ModelName = "FlaggedAvailability",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };

            var httpcontext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = new ControllerContext(httpcontext, new RouteData(), new ProductsController(null, null, null, null));

            // Act
            var result = _binder.BindModel(controllerContext, bindingContext);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void BindModel_With_Bad_MultipleAvailability_ID()
        {
            // Arrange
            var formCollection = new NameValueCollection { { "Product.Name", "sploosh" }, { "Product.MultipleAvailability", "a" } };

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(FlaggedAvailability));

            var bindingContext = new ModelBindingContext
            {
                ModelName = "FlaggedAvailability",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };

            var httpcontext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = new ControllerContext(httpcontext, new RouteData(), new ProductsController(null, null, null, null));

            // Act
            var result = _binder.BindModel(controllerContext, bindingContext);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void BindModel_With_MultipleAvailability_ID()
        {
            // Arrange
            var formCollection = new NameValueCollection { { "Product.Name", "sploosh" }, { "Product.MultipleAvailability", FlaggedAvailability.Store.ToString() } };

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(FlaggedAvailability));

            var bindingContext = new ModelBindingContext
            {
                ModelName = "FlaggedAvailability",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };

            var httpcontext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = new ControllerContext(httpcontext, new RouteData(), new ProductsController(null, null, null, null));

            // Act
            var result = _binder.BindModel(controllerContext, bindingContext);

            // Assert
            Assert.AreEqual(FlaggedAvailability.Store, result);
        }

        [Test]
        public void BindModel_With_Multiple_MultipleAvailability()
        {
            // Arrange
            var formCollection = new NameValueCollection
            {
                { "Product.Name", "sploosh" }, 
                { "Product.MultipleAvailability", string.Format("{0},{1}",FlaggedAvailability.Store.ToString(), FlaggedAvailability.ThirdParty.ToString()) }
            };

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(FlaggedAvailability));

            var bindingContext = new ModelBindingContext
            {
                ModelName = "FlaggedAvailability",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };

            var httpcontext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = new ControllerContext(httpcontext, new RouteData(), new ProductsController(null, null, null, null));

            // Act
            var result = _binder.BindModel(controllerContext, bindingContext);

            // Assert
            Assert.AreEqual(FlaggedAvailability.Store | FlaggedAvailability.ThirdParty, result);
        }
    }
}
