using System.Globalization;
using System.Web.Mvc;
using SharpArch.Web.Mvc.ModelBinder;
using TemplateProject.Domain.Contracts.Tasks;
using TemplateProject.Web.Mvc.Areas.Admin.Models;

namespace TemplateProject.Web.Mvc.Binders
{
    public class ProductEditViewModelBinder : SharpModelBinder
    {
        private readonly ICategoryTasks _categoryTasks;
        private const string FullCategoryIdPropertyName = "ProductEditViewModel.Product.Category";
        private const string DropDownCategoryIDPropertyName = "ProductEditViewModel.SelectedCategoryId";

        public ProductEditViewModelBinder(ICategoryTasks categoryTasks)
        {
            _categoryTasks = categoryTasks;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext) as ProductEditViewModel;
            new ProductBinder(_categoryTasks).BindCategory(bindingContext, model.Product);
            bindingContext.ModelState[FullCategoryIdPropertyName] = new ModelState { Value = new ValueProviderResult(model.Product.Category.Id, 
                model.Product.Category.Id.ToString(), CultureInfo.CurrentCulture)};
            bindingContext.ModelState[DropDownCategoryIDPropertyName] = new ModelState { Value = new ValueProviderResult(model.Product.Category.Id, 
                model.Product.Category.Id.ToString(), CultureInfo.CurrentCulture)};
            return model;
        }
    }
}