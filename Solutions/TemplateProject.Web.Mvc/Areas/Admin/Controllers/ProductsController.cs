using System.Linq;
using System.Web.Mvc;
using SharpArch.NHibernate.Web.Mvc;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Tasks;
using MvcContrib;

namespace TemplateProject.Web.Mvc.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductTasks _productTasks;
        private readonly ICategoryTasks _categoryTasks;

        public ProductsController(IProductTasks productTasks, ICategoryTasks categoryTasks)
        {
            _productTasks = productTasks;
            _categoryTasks = categoryTasks;
        }

        public ActionResult Index()
        {
            var products = _productTasks.GetAll();
            return View(products);
        }

        public ActionResult Create()
        {
            var categories = _categoryTasks.GetAll();
            if (categories.Count == 0)
                ViewBag.CategoryError = "You must create at least one Category before creating a Product.";
            ViewBag.Categories = categories.ToDictionary(k => k.Id, v => v.Name);
            return View(new Product());
        } 

        public ActionResult Edit(int id)
        {
            var product = _productTasks.Get(id);
            var categories = _categoryTasks.GetAll();
            ViewBag.Categories = categories.ToDictionary(k => k.Id, v => v.Name);
            return View(product);
        }

        [Transaction]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid && product.IsValid())
            {
                _productTasks.CreateOrUpdate(product);
                return this.RedirectToAction(x => x.Index());
            }

            var categories = _categoryTasks.GetAll();
            ViewBag.Categories = categories.ToDictionary(k => k.Id, v => v.Name);
            if (product.Id == 0)
                return View("Create", product);
            return View(product);
        }

        public ActionResult Delete(int id)
        {
            _productTasks.Delete(id);
            return this.RedirectToAction(x => x.Index());
        }
    }
}
