using System.Web.Mvc;
using SharpArch.NHibernate.Web.Mvc;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Tasks;
using MvcContrib;

namespace TemplateProject.Web.Mvc.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductTasks _tasks;

        public ProductsController(IProductTasks tasks)
        {
            _tasks = tasks;
        }

        public ActionResult Index()
        {
            var products = _tasks.GetAll();
            return View(products);
        }

        public ActionResult Details(int id)
        {
            var product = _tasks.Get(id);
            return View(product);
        }

        public ActionResult Create()
        {
            return View(new Product());
        } 

        public ActionResult Edit(int id)
        {
            var product = _tasks.Get(id);
            return View(product);
        }

        [Transaction]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid && product.IsValid())
            {
                _tasks.CreateOrUpdate(product);
                return this.RedirectToAction(x => x.Index());
            }

            if(product.Id == 0)
                return View("Create", product);
            return View(product);
        }

        public ActionResult Delete(int id)
        {
            _tasks.Delete(id);
            return this.RedirectToAction(x => x.Index());
        }
    }
}
