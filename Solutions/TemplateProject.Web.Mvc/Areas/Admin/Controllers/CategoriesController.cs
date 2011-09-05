using System.Web.Mvc;
using SharpArch.NHibernate.Web.Mvc;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Tasks;
using MvcContrib;

namespace TemplateProject.Web.Mvc.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryTasks _tasks;

        public CategoriesController(ICategoryTasks tasks)
        {
            _tasks = tasks;
        }

        public ActionResult Index()
        {
            var categories = _tasks.GetAll();
            return View(categories);
        }

        public ActionResult Details(int id)
        {
            var category = _tasks.Get(id);
            return View(category);
        }

        public ActionResult Create()
        {
            return View(new Category());
        } 

        public ActionResult Edit(int id)
        {
            var category = _tasks.Get(id);
            return View(category);
        }

        [Transaction]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid && category.IsValid())
            {
                _tasks.CreateOrUpdate(category);
                return this.RedirectToAction(x => x.Index());
            }

            if (category.Id == 0)
                return View("Create", category);
            return View(category);
        }

        [Transaction]
        public ActionResult Delete(int id)
        {
            _tasks.Delete(id);
            return this.RedirectToAction(x => x.Index());
        }
    }
}
