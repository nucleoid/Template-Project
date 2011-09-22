using System.Linq;
using MvcContrib.Pagination;
using MvpRestApiLib;
using TemplateProject.Domain;

namespace TemplateProject.Web.Mvc.Areas.Admin.Models
{
    public class CategoriesViewModel : IRestModel
    {
        public IPagination<Category> Categories { get; set; }

        public object RestModel
        {
            get { return Categories.ToList(); }
        }
    }
}