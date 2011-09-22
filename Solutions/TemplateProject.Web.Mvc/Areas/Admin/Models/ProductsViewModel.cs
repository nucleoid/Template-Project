using System.Collections.Generic;
using System.Linq;
using MvcContrib.Pagination;
using MvpRestApiLib;
using TemplateProject.Domain;

namespace TemplateProject.Web.Mvc.Areas.Admin.Models
{
    public class ProductsViewModel : IRestModel
    {
        public IPagination<Product> Products { get; set; }
        public Dictionary<int, string> Categories { get; set; }

        public object RestModel
        {
            get { return Products.ToList(); }
        }
    }
}