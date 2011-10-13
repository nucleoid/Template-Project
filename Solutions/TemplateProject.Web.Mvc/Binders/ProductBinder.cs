using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using SharpArch.Web.Mvc.ModelBinder;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Tasks;

namespace TemplateProject.Web.Mvc.Binders
{
    [ModelBinderType(typeof(Product))]
    public class ProductBinder : SharpModelBinder
    {
        private const string CategoryIdPropertyName = "SelectedCategoryId";
        private const string CategoryPropertyName = "Category";
        private readonly ICategoryTasks _categoryTasks;

        public ProductBinder(ICategoryTasks categoryTasks)
        {
            _categoryTasks = categoryTasks;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext) as Product;
            if (model != null)
                BindCategory(bindingContext, model);
            return model;
        }

        public void BindCategory(ModelBindingContext bindingContext, Product model)
        {
            var id = bindingContext.ValueProvider.GetValue(CategoryIdPropertyName);
            int parsedId;
            if (id != null && Int32.TryParse(id.AttemptedValue, out parsedId))
            {
                model.Category = _categoryTasks.Get(parsedId);
                var key = bindingContext.ModelState.Keys.FirstOrDefault(x => x.Contains(CategoryPropertyName));
                if (key != null)
                    bindingContext.ModelState[key] = new ModelState { Value = new ValueProviderResult(id, id.AttemptedValue, CultureInfo.CurrentCulture) };
            }
        }
    }
}