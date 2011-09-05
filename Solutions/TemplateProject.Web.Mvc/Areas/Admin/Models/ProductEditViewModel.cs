
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TemplateProject.Domain;

namespace TemplateProject.Web.Mvc.Areas.Admin.Models
{
    public class ProductEditViewModel
    {
        public Product Product { get; set; }

        [Required(ErrorMessage = "Must have a category")]
        public int? SelectedCategoryId { get; set; }

        public IList<Category> Categories { get; set; }
    }
}