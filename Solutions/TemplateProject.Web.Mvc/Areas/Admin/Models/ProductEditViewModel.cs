
using System.Collections.Generic;
using TemplateProject.Domain;

namespace TemplateProject.Web.Mvc.Areas.Admin.Models
{
    public class ProductEditViewModel
    {
        public Product Product { get; set; }

        public int? SelectedCategoryId { get; set; }

        public IList<Category> Categories { get; set; }
    }
}