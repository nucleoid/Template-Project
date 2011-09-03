using System;
using System.Globalization;
using System.Web.Mvc;
using SharpArch.Web.Mvc.ModelBinder;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Tasks;

namespace TemplateProject.Web.Mvc.Binders
{
    public class ProductBinder : SharpModelBinder
    {
        private const string CategoryIdPropertyName = "SelectedCategoryId";
        private const string FullCategoryIdPropertyName = "Product.Category";
        private readonly ICategoryTasks _categoryTasks;

        public ProductBinder(ICategoryTasks categoryTasks)
        {
            _categoryTasks = categoryTasks;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext) as Product;
            if(model != null)
            {
                var id = bindingContext.ValueProvider.GetValue(CategoryIdPropertyName);
                int parsedId;
                if(id != null && Int32.TryParse(id.AttemptedValue, out parsedId))
                {
                    model.Category = _categoryTasks.Get(parsedId);
                    bindingContext.ModelState[FullCategoryIdPropertyName] = new ModelState { Value = new ValueProviderResult(id, id.AttemptedValue, CultureInfo.CurrentCulture) };
                }
            }
            return model;
        }
    }
}